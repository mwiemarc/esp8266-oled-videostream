namespace scr2esp {
	partial class Form1 {
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent() {
			this.btnToggle = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtUpdateRate = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtIP = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnToggle
			// 
			this.btnToggle.Location = new System.Drawing.Point(12, 78);
			this.btnToggle.Name = "btnToggle";
			this.btnToggle.Size = new System.Drawing.Size(160, 23);
			this.btnToggle.TabIndex = 0;
			this.btnToggle.Text = "Start";
			this.btnToggle.UseVisualStyleBackColor = true;
			this.btnToggle.Click += new System.EventHandler(this.btnToggle_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(83, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "update rate (ms)";
			// 
			// txtUpdateRate
			// 
			this.txtUpdateRate.Location = new System.Drawing.Point(130, 12);
			this.txtUpdateRate.Name = "txtUpdateRate";
			this.txtUpdateRate.Size = new System.Drawing.Size(42, 20);
			this.txtUpdateRate.TabIndex = 2;
			this.txtUpdateRate.Text = "8";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 41);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(50, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "device ip";
			// 
			// txtIP
			// 
			this.txtIP.Location = new System.Drawing.Point(68, 38);
			this.txtIP.Name = "txtIP";
			this.txtIP.Size = new System.Drawing.Size(104, 20);
			this.txtIP.TabIndex = 2;
			this.txtIP.Text = "espscr1:7777";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(184, 113);
			this.Controls.Add(this.txtIP);
			this.Controls.Add(this.txtUpdateRate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnToggle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.ShowIcon = false;
			this.Text = "scr2esp";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnToggle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtUpdateRate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtIP;
	}
}

