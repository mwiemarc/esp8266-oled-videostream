using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace scr2esp {
    public partial class Form1 : Form {
        Socket sender;
        bool isStreaming = false;
        Thread streamThread;
        int updateRate;
		Stopwatch stopwatch = new Stopwatch();
		long lastMS = 0;

		public Form1() {
            InitializeComponent();
        }

        private void btnToggle_Click(object sender, EventArgs e) {
            if (isStreaming) CloseSocket();
            else OpenSocket();
        }

        private void OpenSocket() {
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); // create Socket

			string[] host = txtIP.Text.Split(':');
            sender.Connect(host[0], Convert.ToInt32(host[1]));

            StartStream();
        }

        void CloseSocket() {
            StopStream();

            if (sender == null) return;
            if (!sender.Connected) return;

            // release the socket.
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            sender = null;
        }

        void StartStream() {
            if (isStreaming) return;
            isStreaming = true;

            updateRate = Convert.ToInt32(txtUpdateRate.Text);


			stopwatch.Start();

			streamThread = new Thread(StreamHandler);
            streamThread.Start();

            btnToggle.Text = "Stop";
            txtUpdateRate.Enabled = false;
            txtIP.Enabled = false;
		}

        void StopStream() {
            if (!isStreaming) return;

			stopwatch.Stop();

            streamThread.Abort();
            isStreaming = false;


            btnToggle.Text = "Start";
            txtUpdateRate.Enabled = true;
            txtIP.Enabled = true;
        }

        void StreamHandler() {
            while (isStreaming) {
				CaptureAndSend();

				Thread.Sleep(updateRate);

				double fps = (1000 / (stopwatch.ElapsedMilliseconds - lastMS));
				Console.WriteLine("fps: " + fps);
				lastMS = stopwatch.ElapsedMilliseconds;
			}
        }

        void CaptureAndSend() {
            if (sender == null) return;
            if (!isStreaming) {
                StopStream();
                return;
            }

			// capture and manipulate image
            Bitmap screenImg = ImageManipulation.CaptureScreen();
            Bitmap sizedImg = ImageManipulation.ResizeImage(screenImg, 128, 64);
			Bitmap ditheredImg = ImageManipulation.Dithering(sizedImg);

			// converto to esp tasty data
			int[,] imgArr = ImageManipulation.BitmapToArray(ditheredImg);

            // free ram
            screenImg.Dispose();
            sizedImg.Dispose();
			ditheredImg.Dispose();

            sender.Send(Encoding.ASCII.GetBytes("a")); // send frame initial package

			// create datastring
			string imgStr = "";

			for (int y = (imgArr.GetLength(1) - 1); y >= 0; y--) {
				for (int x = 0; x < imgArr.GetLength(0); x++) {
					imgStr += imgArr[x, y];
				}
			}

			// size of one package
			int BUFFER_LEN = 512;

			// create and send packages
			for (int i = 0; i < (8192 / BUFFER_LEN); i++) {
                byte[] msg = Encoding.ASCII.GetBytes(imgStr.Substring(i * BUFFER_LEN, BUFFER_LEN)); // encode the data string into a byte array.
				int bytesSent = sender.Send(msg); // send the data through the socket.
			}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            CloseSocket();
        }
    }
}
