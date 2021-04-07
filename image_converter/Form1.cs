using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections;
using System.Configuration;

namespace image_converter
{
	public partial class Form1 : Form
	{

		string text_name = null;

		bool image_convert_save = false;
		bool check_scramble = false;
		bool acep_color_check = false;
		byte color_channel = 0;  // bit 7: bw; bit 6: yellow; bit 5: green; bit 4: red; bit 3: blue;
		bool color_channel_bw = true;
		bool color_channel_yellow = false;
		bool color_channel_green = false;
		bool color_channel_red = false;
		bool color_channel_blue = false;
		bool component_ini_finish = false;

		string legio_bw_string = null;
		string legio_yellow_string = null;
		string legio_green_string = null;
		string legio_red_string = null;
		string legio_blue_string = null;


		string image_legio_name_h = null;
		string image_legio_name_upper = null;
		string image_legio_name_ori = null;
		string get_folder_headname = null;
		string image_acep_folder_path = null;
		string get_file_name = null;
		string text_name_greyscale_path = null;


		const int width = 240;
		const int height = 146;
		const int length = 8760; // 240*146/4
		byte[] head_array = new byte[10];   // Header (10 bytes reserved): MAGICWORD, X, Y, sub-image content wygrb000, (6 bytes reserved)
		 
		byte MAGICWORD = 0;                // 0: Legio Acep;  1: Lectum



		bool user_id_admin = false;
		enum form1_width { admin = 1000, customer = 400 };
		enum display_type_enum { enum_legio = 0, enum_lectum = 1 };
		string display_type_legio = "Legio";
		string display_type_lectum = "Lectum";

		


		public Form1()
		{
			InitializeComponent();
			domain_check();
			component_ini_finish = true;


		}

		private void button1_MouseClick(object sender, MouseEventArgs e)
		{
			// open file dialog   
			OpenFileDialog open = new OpenFileDialog();
			ImageConverter image_converter_test = new ImageConverter();			
			try
			{			
				image_null();  ///////////////////////////////////////////////////////   clear PictureBox
				Process[] prs = Process.GetProcesses();

				foreach (Process pr in prs)
				{
					if (pr.ProcessName == "notepad")
					{
						pr.Kill();
					}

				}
				string path = Directory.GetCurrentDirectory();
				string[] txtList = Directory.GetFiles(path, "*.txt");
				foreach (string f in txtList)
				{
					if (f == path + "\\"+ text_name)
					{
						File.Delete(text_name);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.pgm )|*.jpg; *.jpeg; *.gif; *.bmp; *.png; *.pgm";
			try
			{
				if (open.ShowDialog() == DialogResult.OK)
				{
					string image_string_filename = open.FileName;
					image_acep_folder_path = System.IO.Path.GetDirectoryName(open.FileName);
					get_file_name = System.IO.Path.GetFileNameWithoutExtension(open.FileName);

				    image_legio_name_ori = reform_file_name(get_file_name);
					image_legio_name_upper = image_legio_name_ori.ToUpper();
					image_legio_name_h = image_legio_name_upper + "_h";
					text_name = image_legio_name_ori + ".h";
				    string	text_name_greyscale = image_legio_name_ori + ".txt";
					text_name_greyscale_path = image_acep_folder_path + "\\" + text_name_greyscale;

					switch (MAGICWORD)
					{

						case (int)display_type_enum.enum_legio:  // for Acep Arduino, now not for Acep scramble

							legio_convert(image_string_filename);
							break;

						case (int)display_type_enum.enum_lectum:	// for Lectum

							lectum_convert(image_string_filename);
							break;


					}
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				textBox1.Text = "error";
			}
		}

		


		private void checkBox_scramble_CheckedChanged(object sender, EventArgs e)
		{
			check_scramble = checkBox_scramble.Checked;
		}

		private void checkBox_create_img_test_CheckedChanged(object sender, EventArgs e)
		{
			image_convert_save = checkBox_create_img_test.Checked;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (component_ini_finish)
			{

				pictureBox1.Image = null;
				label2.Text = "path:";
				textBox1.Clear();

				if (comboBox1.SelectedIndex == 0)
				{
					check_scramble = false;
					MAGICWORD = 0;
					AddOrUpdateAppSettings("display type", display_type_legio);
				}
				else if (comboBox1.SelectedIndex == 1)
				{
					check_scramble = true;
					MAGICWORD = 1;
					AddOrUpdateAppSettings("display type", display_type_lectum);
				}
			}
		}

		public void domain_check()
		{

			string config_test = GetConfig("Domain");
			if (config_test == "Admin" || config_test == "admin")
			{

				user_id_admin = true;
			}
			else if (config_test == "User" || config_test == "user")
			{
				user_id_admin = false;
			}
			string display_type = GetConfig("display type");
			if (display_type == "Legio")
			{
				comboBox1.Text = comboBox1.Items[0].ToString();
				check_scramble = false;
				acep_color_check = true;
				MAGICWORD = 0;

			}
			else if (display_type == "Lectum")
			{
				comboBox1.Text = comboBox1.Items[1].ToString();
				check_scramble = true;
				acep_color_check = false;
				MAGICWORD = 0x01;
			}


			if (user_id_admin)
			{
				this.ClientSize = new System.Drawing.Size((int)form1_width.admin, 600);
				checkBox_create_img_test.Visible = true;
				checkBox_scramble.Visible = true;
			}
			else
			{
				this.ClientSize = new System.Drawing.Size((int)form1_width.customer, 600);
				checkBox_create_img_test.Visible = false;
				checkBox_scramble.Visible = false;
			}
			head_array[0] = MAGICWORD;
			head_array[1] = (byte)width;  // X, image_width, 2.1" is 240
			head_array[2] = (byte)Height;  // Y, image_height, 2.1" is 146
		}


		public void image_null()
		{

			if (pictureBox1.Image != null)
			{
				pictureBox1.Image.Dispose();
				pictureBox1.Image = null;
			}

			if (pictureBox_bw.Image != null)
			{
				pictureBox_bw.Image.Dispose();
				pictureBox_bw.Image = null;
			}

			if (pictureBox_yg.Image != null)
			{
				pictureBox_yg.Image.Dispose();
				pictureBox_yg.Image = null;
			}
			if (pictureBox_r.Image != null)
			{
				pictureBox_r.Image.Dispose();
				pictureBox_r.Image = null;
			}
			if (pictureBox_g.Image != null)
			{
				pictureBox_g.Image.Dispose();
				pictureBox_g.Image = null;
			}

			if (pictureBox_b.Image != null)
			{
				pictureBox_b.Image.Dispose();
				pictureBox_b.Image = null;
			}
			this.Invoke((MethodInvoker)delegate () { textBox1.Clear(); });
			this.Invoke((MethodInvoker)delegate () { label2.Text = "path:"; });


		}


		public void legio_convert(string image_string_filename)
		{

				//	string open_file_name = Path.GetExtension(open.FileName);
							label2.Text = "path: " + image_string_filename;
							string get_folder_filename = image_acep_folder_path + "\\" + get_file_name;
							get_folder_headname = image_acep_folder_path + "\\" + image_legio_name_ori;
							if (image_convert_save)
							{
								if (!File.Exists(get_folder_filename))
									System.IO.Directory.CreateDirectory(get_folder_filename);
							}
							//if (!File.Exists(get_folder_headname))
							//   System.IO.Directory.CreateDirectory(get_folder_headname);

							Bitmap source = new Bitmap(image_string_filename);
							//	pictureBox1.Image = new Bitmap(open.FileName);

							this.Invoke((MethodInvoker)delegate () { pictureBox1.Image = source; });


							int image_width = source.Width;
							int image_height = source.Height;
							acep_color_byte_array_all(source, get_folder_filename, get_file_name, image_width, image_height);
							textBox1.AppendText(image_legio_name_ori + ".h created" + "\r\n");
							try
							{
								Process.Start("explorer.exe", @image_acep_folder_path);
							}
							catch (Win32Exception win32Exception)
							{
								//The system cannot find the file specified...
								Console.WriteLine(win32Exception.Message);
							}



		}


		public void lectum_convert(string image_string_filename)
		{

			string image_string_all = null;
			string image_string_head = null;
			string open_file_name = Path.GetExtension(image_string_filename);
			try
			{
				Bitmap pgm_to_bitmap = PGMUtil.ToBitmap(image_string_filename);
				pictureBox1.Image = pgm_to_bitmap;



				this.Invoke((MethodInvoker)delegate
				{
					// Running on the UI thread
					pictureBox1.Image = pgm_to_bitmap;
				});

			}
			catch
			{
			}
			label2.Text = "path: " + image_string_filename;
			bool name_check_bool = (open_file_name == ".PGM" || open_file_name == ".pgm");
			if (name_check_bool)
			{

				PgmImage pgmImage = LoadImage(image_string_filename);


				int image_height = pgmImage.height;       // height 146
				int image_width = pgmImage.width;         //  width 240
				byte[][] buffer_pixel = pgmImage.pixels;

				byte[,] buffer = new byte[image_height, image_width];


				for (int i = 0; i < image_height; i++)
				{
					for (int j = 0; j < image_width; j++)
						buffer[i, j] = buffer_pixel[i][j];
				}



				byte[] image_uc8156_buffer = image_uc8156_array(buffer, image_height, image_width);
				byte[] image_uc8156_no_scramble_buffer = image_uc8156_array_no_scramble(buffer, image_height, image_width);
	
				image_string_all = byte_array_to_string(image_uc8156_buffer);
				image_string_head = byte_array_to_string(image_uc8156_no_scramble_buffer);






				//	byte[] buffer_all = new byte[pgmImage.];
			}
			else   // für PNG or PNG or BMP
			{
				//return;
				pictureBox1.Image = new Bitmap(image_string_filename);
				// image file path  


				byte[,] image_byte_array = ConvertToGrayscale(pictureBox1.Image);

				Bitmap source = (Bitmap)pictureBox1.Image;
				int image_width = source.Width;
				int image_height = source.Height;
				byte[] image_uc8156_buffer = image_uc8156_array(image_byte_array, image_height, image_width);
				byte[] image_uc8156_no_scramble_buffer = image_uc8156_array_no_scramble(image_byte_array, image_height, image_width);
				image_string_all = byte_array_to_string(image_uc8156_buffer);
				image_string_head = byte_array_to_string(image_uc8156_no_scramble_buffer);

			}

			
			image_string_head = "," + image_string_head;
			File.WriteAllText(text_name_greyscale_path, image_string_all);

			WriteFileHeader(image_string_head);
			string s = image_string_filename;
			string[] subs = s.Split('\\');
			
			textBox1.AppendText(subs[subs.Length - 1] + " convert success" + "\r\n");
			textBox1.AppendText(image_legio_name_ori + ".h created" + "\r\n");
			textBox1.AppendText(image_legio_name_ori + ".txt created" + "\r\n");

			Process.Start("explorer.exe", @image_acep_folder_path);


		}


	}
}
