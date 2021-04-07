
namespace image_converter
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.checkBox_scramble = new System.Windows.Forms.CheckBox();
			this.pictureBox_bw = new System.Windows.Forms.PictureBox();
			this.pictureBox_yg = new System.Windows.Forms.PictureBox();
			this.pictureBox_g = new System.Windows.Forms.PictureBox();
			this.pictureBox_r = new System.Windows.Forms.PictureBox();
			this.pictureBox_b = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.checkBox_create_img_test = new System.Windows.Forms.CheckBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_bw)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_yg)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_g)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_r)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_b)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(241, 222);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(87, 29);
			this.button1.TabIndex = 0;
			this.button1.Text = "choose";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(74, 18);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(240, 146);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(98, 231);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(12, 180);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(362, 29);
			this.label2.TabIndex = 3;
			this.label2.Text = "path:";
			// 
			// textBox1
			// 
			this.textBox1.Enabled = false;
			this.textBox1.Location = new System.Drawing.Point(34, 320);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(331, 145);
			this.textBox1.TabIndex = 4;
			// 
			// checkBox_scramble
			// 
			this.checkBox_scramble.AutoSize = true;
			this.checkBox_scramble.Location = new System.Drawing.Point(237, 282);
			this.checkBox_scramble.Name = "checkBox_scramble";
			this.checkBox_scramble.Size = new System.Drawing.Size(68, 17);
			this.checkBox_scramble.TabIndex = 5;
			this.checkBox_scramble.Text = "scramble";
			this.checkBox_scramble.UseVisualStyleBackColor = true;
			this.checkBox_scramble.Visible = false;
			this.checkBox_scramble.CheckedChanged += new System.EventHandler(this.checkBox_scramble_CheckedChanged);
			// 
			// pictureBox_bw
			// 
			this.pictureBox_bw.Location = new System.Drawing.Point(405, 18);
			this.pictureBox_bw.Name = "pictureBox_bw";
			this.pictureBox_bw.Size = new System.Drawing.Size(240, 146);
			this.pictureBox_bw.TabIndex = 7;
			this.pictureBox_bw.TabStop = false;
			// 
			// pictureBox_yg
			// 
			this.pictureBox_yg.Location = new System.Drawing.Point(710, 18);
			this.pictureBox_yg.Name = "pictureBox_yg";
			this.pictureBox_yg.Size = new System.Drawing.Size(240, 146);
			this.pictureBox_yg.TabIndex = 8;
			this.pictureBox_yg.TabStop = false;
			// 
			// pictureBox_g
			// 
			this.pictureBox_g.Location = new System.Drawing.Point(405, 198);
			this.pictureBox_g.Name = "pictureBox_g";
			this.pictureBox_g.Size = new System.Drawing.Size(240, 146);
			this.pictureBox_g.TabIndex = 9;
			this.pictureBox_g.TabStop = false;
			// 
			// pictureBox_r
			// 
			this.pictureBox_r.Location = new System.Drawing.Point(710, 194);
			this.pictureBox_r.Name = "pictureBox_r";
			this.pictureBox_r.Size = new System.Drawing.Size(240, 146);
			this.pictureBox_r.TabIndex = 10;
			this.pictureBox_r.TabStop = false;
			// 
			// pictureBox_b
			// 
			this.pictureBox_b.Location = new System.Drawing.Point(569, 375);
			this.pictureBox_b.Name = "pictureBox_b";
			this.pictureBox_b.Size = new System.Drawing.Size(240, 146);
			this.pictureBox_b.TabIndex = 11;
			this.pictureBox_b.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(459, 167);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(117, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "legio_bw  EPD_BLACK";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(779, 167);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(125, 13);
			this.label4.TabIndex = 13;
			this.label4.Text = "legio_yq  EPD_YELLOW";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(463, 347);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(113, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "legio_g  EPD_GREEN";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(789, 343);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(92, 13);
			this.label6.TabIndex = 15;
			this.label6.Text = "legio_r EPD_RED";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(649, 524);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(103, 13);
			this.label7.TabIndex = 16;
			this.label7.Text = "legio_b  EPD_BLUE";
			// 
			// checkBox_create_img_test
			// 
			this.checkBox_create_img_test.AutoSize = true;
			this.checkBox_create_img_test.Location = new System.Drawing.Point(237, 259);
			this.checkBox_create_img_test.Name = "checkBox_create_img_test";
			this.checkBox_create_img_test.Size = new System.Drawing.Size(113, 17);
			this.checkBox_create_img_test.TabIndex = 17;
			this.checkBox_create_img_test.Text = "create image text  ";
			this.checkBox_create_img_test.UseVisualStyleBackColor = true;
			this.checkBox_create_img_test.Visible = false;
			this.checkBox_create_img_test.CheckedChanged += new System.EventHandler(this.checkBox_create_img_test_CheckedChanged);
			// 
			// comboBox1
			// 
			this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.ItemHeight = 15;
			this.comboBox1.Items.AddRange(new object[] {
            "2.1\" Legio ( 6 color)",
            "2.1\" Lectum ( 4 Greylevels)"});
			this.comboBox1.Location = new System.Drawing.Point(34, 223);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(189, 23);
			this.comboBox1.TabIndex = 18;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1038, 562);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.checkBox_create_img_test);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.pictureBox_b);
			this.Controls.Add(this.pictureBox_r);
			this.Controls.Add(this.pictureBox_g);
			this.Controls.Add(this.pictureBox_yg);
			this.Controls.Add(this.pictureBox_bw);
			this.Controls.Add(this.checkBox_scramble);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "PL Image Converter 1.0";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_bw)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_yg)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_g)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_r)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox_b)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}













		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.CheckBox checkBox_scramble;
		private System.Windows.Forms.PictureBox pictureBox_bw;
		private System.Windows.Forms.PictureBox pictureBox_yg;
		private System.Windows.Forms.PictureBox pictureBox_g;
		private System.Windows.Forms.PictureBox pictureBox_r;
		private System.Windows.Forms.PictureBox pictureBox_b;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox checkBox_create_img_test;
		private System.Windows.Forms.ComboBox comboBox1;
	}
}

