using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace scr2esp {
	public class ImageManipulation {
		public static Bitmap CaptureScreen() {
			Rectangle r = Screen.PrimaryScreen.Bounds;
			Bitmap bmp = new Bitmap(r.Width, r.Height);

			using (var graphics = Graphics.FromImage(bmp)) {
				graphics.CopyFromScreen(r.X, r.Y, 0, 0, r.Size, CopyPixelOperation.SourceCopy);
			}

			return bmp;
		}

		public static Bitmap ResizeImage(Image image, int width, int height) {
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage)) {
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.NearestNeighbor; // HighQualityBicubic
				graphics.SmoothingMode = SmoothingMode.None;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes()) {
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		public static Bitmap Dithering(Bitmap img) {
			int h = img.Height;
			int w = img.Width;
			int[,] arr = new int[h, w];
			int row = 0, col = 0, tmp = 0;
			Bitmap newImg = new Bitmap(w, h);

			for (col = 0; col < w; col++) {
				for (row = 0; row < h; row++) {
					Color pixel = img.GetPixel(col, row);
					arr[row, col] = (pixel.R + pixel.G + pixel.B) / 3;
				}
			}

			for (row = 1; row < h - 1; row++) {
				for (col = 1; col < w - 1; col++) {
					CalcDithering(row, col, ref arr);
				}
			}

			for (col = 0; col < w; col++) {
				for (row = 0; row < h; row++) {
					Color pixel = img.GetPixel(col, row);
					tmp = arr[row, col];

					if (tmp == 0) tmp = 0;
					else tmp = 255;

					pixel = Color.FromArgb(tmp, tmp, tmp);
					newImg.SetPixel(col, row, pixel);
				}
			}

			return newImg;
		}

		private static void CalcDithering(int row, int col, ref int[,] arr) {
			int divider = 0;

			if (arr[row, col] < 128) {
				divider = arr[row, col] / 16;
				arr[row, col] = 0;
			} else {
				divider = (arr[row, col] - 255) / 16;
				arr[row, col] = 1;
			}

			arr[row + 1, col - 1] += (divider * 3);
			arr[row + 1, col] += (divider * 5);
			arr[row + 1, col + 1] += divider;
			arr[row, col + 1] += (divider * 7);
		}

		static public int[,] BitmapToArray(Bitmap img) {
			int[,] data = new int[img.Width, img.Height];

			for (int y = (img.Height - 1); y >= 0; y--) {
				for (int x = 0; x < img.Width; x++) {
					Color c = img.GetPixel(x, y);

					data[x, y] = (c.R == 0 && c.G == 0 && c.B == 0) ? 0 : 1;
				}
			}

			return data;
		}
	}
}
