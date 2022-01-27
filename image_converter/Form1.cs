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
using System.Globalization;

namespace image_converter
{
	public partial class Form1 : Form
	{

		string text_name = null;

		bool image_convert_save = false;
		//bool check_scramble = false;

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


		int image_height = 0;       // height 146
		int image_width = 0;         //  width 240

		enum display_width_enum { enum_S021_width = 240, enum_S011_width = 148, enum_S014_width = 180, enum_S031_width = 148, enum_S041_width = 104, enum_S0357_width = 640 };
		enum display_height_enum { enum_S021_height = 146, enum_S011_height = 70, enum_S014_height = 100, enum_S031_height = 156, enum_S041_height = 512, enum_S0357_height = 400};
		const int length = 8760; // 240*146/4
		byte[] head_array = new byte[10];   // Header (10 bytes reserved): MAGICWORD, X, Y, sub-image content wygrb000, (6 bytes reserved)
		 
		byte MAGICWORD = 0;                // 0b1000: S11; 0b0100: S14; 0b0010: S21; 0b0001: S31        
										   // 0b1000: red; 0b0100: yellow; 0b0010: Lectum Grey; 0b0001: Legio Color	
		byte display_size = 0; // for display size:    0b1000: S11; 0b0100: S14; 0b0010: S21; 0b0001: S31	
		byte display_color = 0;  // for display color: 0b1000: red; 0b0100: yellow; 0b0010: Lectum Grey; 0b0001: Legio Color		

		bool user_id_admin = false;
		enum form1_width { admin = 1000, customer = 400 };
		enum display_size_enum { enum_S011 = 0b1000, enum_S014 = 0b0100, enum_S021 = 0b0010,  enum_S031 = 0b0001, enum_S041 = 0b0011, enum_S0357 = 0b0101 };
		enum display_color_enum {  enum_lectum_tricolor_red = 0b1000, enum_lectum_tricolor_yellow = 0b0100, enum_lectum_gl = 0b0010, enum_legio = 0b0001, enum_bw = 0b0011 };  // enum_bw for UC8179


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
				
					
					head_array[0] = MAGICWORD;


					label2.Text = "path: " + image_string_filename;

					switch (display_color)
					{
						// 0b1000: S11; 0b0100: S14; 0b0010: S21; 0b0001: S31; 0b0011: S41; 0b0101: S0357        
						// 0b1000: red; 0b0100: yellow; 0b0010: Lectum Grey; 0b0001: Legio Color; 0b0011: BW (UC8179)	

						case 0x01:   // for Legio
							Legio_convert(image_string_filename);
							break;

						case 0x03:
							bw_convert(image_string_filename);
							break;
							


						default:   // for Lectum grey and tricolor red and yellow 
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

		




		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (component_ini_finish)
			{

				pictureBox1.Image = null;
				label2.Text = "path:";
				textBox1.Clear();

				byte display_size_choose = (byte)comboBox1.SelectedIndex;
				

				switch (display_size_choose)
				{
					case 0:  // for 1.1 
						display_size = (byte)display_size_enum.enum_S011;
						image_width = (int)display_width_enum.enum_S011_width;
						image_height = (int)display_height_enum.enum_S011_height;

						head_array[1] = (byte)image_width;  //
						head_array[2] = (byte)image_height;  //
						break;

					case 1:   // for 1.4 
						display_size = (byte)display_size_enum.enum_S014;
						image_width = (int)display_width_enum.enum_S014_width;
						image_height = (int)display_height_enum.enum_S014_height;

						head_array[1] = (byte)image_width;  //
						head_array[2] = (byte)image_height;  //
						break;
					case 2:  // for 2.1 
						display_size = (byte)display_size_enum.enum_S021;
						image_width = (int)display_width_enum.enum_S021_width;
						image_height = (int)display_height_enum.enum_S021_height;

						head_array[1] = (byte)image_width;  //
						head_array[2] = (byte)image_height;  //
						break;
					case 3:  // for 3.1 
						display_size = (byte)display_size_enum.enum_S031;
						image_width = (int)display_width_enum.enum_S031_width;
						image_height = (int)display_height_enum.enum_S031_height;

						head_array[1] = (byte)image_width;  //
						head_array[2] = (byte)image_height;  //
						break;

					case 4:  // for 4.1 UC8179
						display_size = (byte)display_size_enum.enum_S041;
						image_width = (int)display_width_enum.enum_S041_width;
						image_height = (int)display_height_enum.enum_S041_height;

						head_array[1] = (byte)image_width;  //
						head_array[2] = (byte)image_height;  //
						break;
					case 5:  // for 3.57 
						display_size = (byte)display_size_enum.enum_S0357;
						image_width = (int)display_width_enum.enum_S0357_width;
						image_height = (int)display_height_enum.enum_S0357_height;

						head_array[1] = (byte)(image_width / 4);  //
						head_array[2] = (byte)(image_height / 4);  //
						break;
				}





				MAGICWORD = (byte)(((display_size << 4) & 0xF0) | display_color);
				string display_type_save = MAGICWORD.ToString("X2");
				AddOrUpdateAppSettings("display type", display_type_save);



			}
		}

		public void domain_check()
		{

			string config_test = GetConfig("Domain");
			if (config_test == "Admin" || config_test == "admin")
			{

				user_id_admin = true;
			}
		//	else if (config_test == "User" || config_test == "user")
		   else
			{
				user_id_admin = false;
			}
			string display_type = GetConfig("display type");

		
	   	    byte.TryParse(display_type, NumberStyles.HexNumber, null,  out MAGICWORD);               // get the MAGICWORD for display, info about display size and display color
			head_array[0] = MAGICWORD;
			display_size = (byte)( MAGICWORD >> 4);	// for display size: 0:1.1; 1:1.4; 2:2.1; 3: 3.1;
			display_color = (byte)(MAGICWORD & 0x0F);  // for display color: 0: Lectum; 1: Acep

			switch (display_size)
			{

				case (int)display_size_enum.enum_S011:  // for Acep 

					comboBox1.Text = comboBox1.Items[0].ToString();
					head_array[1] = (byte)display_width_enum.enum_S011_width;  
					head_array[2] = (byte)display_height_enum.enum_S011_height;  
					image_width = (int)display_width_enum.enum_S011_width;
					image_height = (int)display_height_enum.enum_S011_height;
					break;

				case (int)display_size_enum.enum_S014:   // for S014_Lectum 
					comboBox1.Text = comboBox1.Items[1].ToString();
					head_array[1] = (byte)display_width_enum.enum_S014_width;  
					head_array[2] = (byte)display_height_enum.enum_S014_height;  
					image_width = (int)display_width_enum.enum_S014_width;
					image_height = (int)display_height_enum.enum_S014_height;
					break;
				case (int)display_size_enum.enum_S021:  // for S021_Lectum 
					comboBox1.Text = comboBox1.Items[2].ToString();
					head_array[1] = (byte)display_width_enum.enum_S021_width;  
					head_array[2] = (byte)display_height_enum.enum_S021_height; 
					image_width = (int)display_width_enum.enum_S021_width;
					image_height = (int)display_height_enum.enum_S021_height;
					break;
				case (int)display_size_enum.enum_S031:  // for S031_Lectum 
					comboBox1.Text = comboBox1.Items[3].ToString();
					head_array[1] = (byte)display_width_enum.enum_S031_width;  
					head_array[2] = (byte)display_height_enum.enum_S031_height;  
					image_width = (int)display_width_enum.enum_S031_width;
					image_height = (int)display_height_enum.enum_S031_height;
					break;


				case (int)display_size_enum.enum_S041:  // for S041_Lectum 
					comboBox1.Text = comboBox1.Items[4].ToString();
					head_array[1] = (byte)display_width_enum.enum_S041_width;  
					head_array[2] = (byte)( (int)display_height_enum.enum_S041_height /4); //  512 / 4
					image_width = (int)display_width_enum.enum_S041_width;
					image_height = (int)display_height_enum.enum_S041_height;
					break;


				case (int)display_size_enum.enum_S0357:  // for S011_Lectum 
					comboBox1.Text = comboBox1.Items[5].ToString();
					head_array[1] = (byte)((int)display_width_enum.enum_S0357_width / 4);  // 640 /4
					head_array[2] = (byte)((int)display_height_enum.enum_S0357_height / 4);  // 400 / 4
					image_width = (int)display_width_enum.enum_S0357_width;
					image_height = (int)display_height_enum.enum_S0357_height;
					break;







			}


			switch (display_color)
			{

				case (byte)display_color_enum.enum_lectum_gl:  // for lectum greyscale
					comboBox2.Text = comboBox2.Items[0].ToString();					
					break;

				case (byte)display_color_enum.enum_lectum_tricolor_red:  // for lectum tricolor
					comboBox2.Text = comboBox2.Items[1].ToString();		
					break;
				case (byte)display_color_enum.enum_lectum_tricolor_yellow:  // for lectum tricolor
					comboBox2.Text = comboBox2.Items[2].ToString();
					break;

				case (byte)display_color_enum.enum_legio:   // for legio acep 
					comboBox2.Text = comboBox2.Items[3].ToString();			
					break;
				case (byte)display_color_enum.enum_bw:   // for legio acep 
					comboBox2.Text = comboBox2.Items[4].ToString();
					break;

			}


			if (user_id_admin)
			{
				this.ClientSize = new System.Drawing.Size((int)form1_width.admin, 600);

			}
			else
			{
				this.ClientSize = new System.Drawing.Size((int)form1_width.customer, 600);

			}


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


		public void Legio_convert(string image_string_filename)
		{

				       //	string open_file_name = Path.GetExtension(open.FileName);
						
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
							if (display_size == (int)display_size_enum.enum_S031 )
								{
									pictureBox1.Image = RotateImage(source);
								}
							else
								{
									pictureBox1.Image = source;
								}



							acep_color_byte_array_all(source, get_folder_filename, get_file_name, source.Width, source.Height);
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
			string open_file_name = Path.GetExtension(image_string_filename);
		
			//label2.Text = "path: " + image_string_filename;
			bool name_check_PGM = (open_file_name == ".PGM" || open_file_name == ".pgm");
			if (name_check_PGM)
			{

				PgmImage pgmImage = LoadImage(image_string_filename);



				byte[][] buffer_pixel = pgmImage.pixels;
				 

				byte[,] buffer = new byte[pgmImage.height, pgmImage.width];


				for (int i = 0; i < pgmImage.height; i++)
				{
					for (int j = 0; j < pgmImage.width; j++)
						buffer[i, j] = buffer_pixel[i][j];
				}

			


				byte[] image_uc8156_no_scramble_buffer = image_uc8156_array_no_scramble(buffer, pgmImage.height, pgmImage.width);

				string image_string_head = null; image_string_head = byte_array_to_string(image_uc8156_no_scramble_buffer);

				image_string_head = "," + image_string_head;
				WriteFileHeader(image_string_head);
				string s = image_string_filename;
				string[] subs = s.Split('\\');

				textBox1.AppendText(subs[subs.Length - 1] + " convert success" + "\r\n");
				textBox1.AppendText(image_legio_name_ori + ".h created" + "\r\n");
				textBox1.AppendText(image_legio_name_ori + ".txt created" + "\r\n");

				Process.Start("explorer.exe", @image_acep_folder_path);




				//	byte[] buffer_all = new byte[pgmImage.];
			}
			else   // für PNG or PNG or BMP
			{
				Bitmap image_read = new Bitmap(image_string_filename);
				bool right_image_size = false;   
				bool image_rotate_bool = false;
				check_image_height_width(display_size, image_read.Width, image_read.Height, out right_image_size, out image_rotate_bool);




				if (right_image_size)           // the right size for image source
				{

					if (!image_rotate_bool)
					{
						if (display_size == (int)display_size_enum.enum_S031)
						{
							pictureBox1.Image = RotateImage(image_read);
						}
						else
						{

							pictureBox1.Image = image_read;
						}
					}
					else
					{
						if (display_size != (int)display_size_enum.enum_S031)
						{
							pictureBox1.Image = RotateImage(image_read);
							image_read = new Bitmap(  pictureBox1.Image);



						}
						else
						{
							pictureBox1.Image = image_read;
							image_read = new Bitmap( RotateImage(pictureBox1.Image));

						}

					}
					image_lectum_convert(image_read, image_string_filename);

				}
				else
				{
					textBox1.Clear();
					textBox1.AppendText("please check image size, resize it or choose the right one");
				}

			}



		}



		public void bw_convert(string image_string_filename)
		{
			string open_file_name = Path.GetExtension(image_string_filename);

			//label2.Text = "path: " + image_string_filename;
			bool name_check_PGM = (open_file_name == ".PGM" || open_file_name == ".pgm");
			if (name_check_PGM)
			{



				PgmImage pgmImage = LoadImage(image_string_filename);



				byte[][] buffer_pixel = pgmImage.pixels;


				byte[,] buffer = new byte[pgmImage.height, pgmImage.width];


				for (int i = 0; i < pgmImage.height; i++)
				{
					for (int j = 0; j < pgmImage.width; j++)
						buffer[i, j] = buffer_pixel[i][j];
				}

				Bitmap Bitmap_4GL = SaveByteArryToBitmap(buffer, pgmImage.width, pgmImage.height);
				pictureBox_bw.Image = RotateImage( Bitmap_4GL);

				byte[] image_uc8179_no_scramble_buffer = image_uc8179_array_no_scramble(buffer, pgmImage.height, pgmImage.width);

				string image_string_head = null; image_string_head = byte_array_to_string(image_uc8179_no_scramble_buffer);

				image_string_head = "," + image_string_head;
				WriteFileHeader(image_string_head);
				string s = image_string_filename;
				string[] subs = s.Split('\\');

				textBox1.AppendText(subs[subs.Length - 1] + " convert success" + "\r\n");
				textBox1.AppendText(image_legio_name_ori + ".h created" + "\r\n");
				textBox1.AppendText(image_legio_name_ori + ".txt created" + "\r\n");

				Process.Start("explorer.exe", @image_acep_folder_path);


			}
			else
			{
				Bitmap image_read = new Bitmap(image_string_filename);
				byte[,] image_byte_array = ConvertToGrayscale(image_read);

				pictureBox_bw.Image = RotateImage(image_read);

				byte[] image_uc8179_no_scramble_buffer = image_uc8179_array_no_scramble(image_byte_array, image_read.Height, image_read.Width);

				string image_string_head = null; 
				image_string_head = byte_array_to_string(image_uc8179_no_scramble_buffer);

				image_string_head = "," + image_string_head;
				WriteFileHeader(image_string_head);
				string s = image_string_filename;
				string[] subs = s.Split('\\');

				textBox1.AppendText(subs[subs.Length - 1] + " convert success" + "\r\n");
				textBox1.AppendText(image_legio_name_ori + ".h created" + "\r\n");
				textBox1.AppendText(image_legio_name_ori + ".txt created" + "\r\n");

				Process.Start("explorer.exe", @image_acep_folder_path);


			}
		}




		public Image RotateImage(Image img)   // rotate 270°
		{
			var bmp = new Bitmap(img);

			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				gfx.Clear(Color.White);
				gfx.DrawImage(img, 0, 0, img.Width, img.Height);
			}

			bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
			return bmp;
		}


		public Image RotateImage_90(Image img)
		{
			var bmp = new Bitmap(img);

			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				gfx.Clear(Color.White);
				gfx.DrawImage(img, 0, 0, img.Width, img.Height);
			}

			bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
			return bmp;
		}

		public Image RotateImage_180(Image img)
		{
			var bmp = new Bitmap(img);

			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				gfx.Clear(Color.White);
				gfx.DrawImage(img, 0, 0, img.Width, img.Height);
			}

			bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
			return bmp;
		}







		public void check_image_height_width(int display_type ,int image_width_real, int image_height_real, out bool right_image_size, out bool image_rotate_bool)
		{


			int width_check = 0;
			int height_check = 0;

			right_image_size = false;
			image_rotate_bool = false;


			switch (display_size)
			{

				case (int)display_size_enum.enum_S011:  // for S011
					width_check = (int)display_height_enum.enum_S011_height;		// according to the setting of Parrot here should be 70
					height_check = (int)display_width_enum.enum_S011_width;			// according to the setting of Parrot here should be 148
					break;

				case (int)display_size_enum.enum_S014:   // for S014_Lectum , 
					width_check = (int)display_width_enum.enum_S014_width;      // according to the setting of Parrot here should be 180
					height_check = (int)display_height_enum.enum_S014_height;
					break;
				case (int)display_size_enum.enum_S021:  // for S021_Lectum 
					width_check = (int)display_width_enum.enum_S021_width;
					height_check = (int)display_height_enum.enum_S021_height;
					break;
				case (int)display_size_enum.enum_S031:  // for S031_Lectum 
					width_check = (int)display_width_enum.enum_S031_width /2;
					height_check = (int)display_height_enum.enum_S031_height*2;

					break;
			}

			if (image_width_real == width_check)
			{
				if (image_height_real == height_check)
				{
					right_image_size = true;
					image_rotate_bool = false;
				}
				else
				{
					right_image_size = false;
					image_rotate_bool = false;
				}
			}
			else  // image_width_real != width_check
			{
				if (image_width_real == height_check)
				{
					if (image_height_real == width_check)
					{
						right_image_size = true;
						image_rotate_bool = true;
					}
					else
					{
						right_image_size = false;
						image_rotate_bool = false;
					}

				}

			}

		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (component_ini_finish)
			{

				pictureBox1.Image = null;
				label2.Text = "path:";
				textBox1.Clear();
				byte display_item = (byte)comboBox2.SelectedIndex;
				
				switch (display_item)
				{
					case 0:
						display_color = (byte)display_color_enum.enum_lectum_gl;
						break;
					case 1:
						display_color = (byte)display_color_enum.enum_lectum_tricolor_red;
						break;
					case 2:
						display_color = (byte)display_color_enum.enum_lectum_tricolor_yellow;
						break;
					case 3:
						display_color = (byte)display_color_enum.enum_legio;
						break;

					case 4:
						display_color = (byte)display_color_enum.enum_bw;
						break;

				}




				MAGICWORD = (byte)(((display_size << 4) & 0xF0) | (display_color &0x0F));
				string display_type_save = MAGICWORD.ToString("X2");
				AddOrUpdateAppSettings("display type", display_type_save);
			}
		}

		private void button_rotate_Click(object sender, EventArgs e)
		{
			try
			{
				
				if (pictureBox1.Image != null)
				{
					Image image_read = pictureBox1.Image;
					pictureBox1.Image = RotateImage_180(image_read);

					switch (display_color)
					{
						// 0b1000: S11; 0b0100: S14; 0b0010: S21; 0b0001: S31        
						// 0b1000: red; 0b0100: yellow; 0b0010: Lectum Grey; 0b0001: Legio Color	

						case 0x01:   // for Legio
									 //rotate for S031 still not work



							Bitmap source = new Bitmap(pictureBox1.Image);
							string get_folder_filename = image_acep_folder_path + "\\" + get_file_name;
							acep_color_byte_array_all(source, get_folder_filename, get_file_name, source.Width, source.Height);
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



							break;


						default:   // for Lectum grey and tricolor red and yellow 
							if (display_size == (int)display_size_enum.enum_S031)
							{
								//pictureBox1.Image = RotateImage(image_read);
								Bitmap image_read_new = new Bitmap(RotateImage_90(pictureBox1.Image));
								image_lectum_convert(image_read_new, get_file_name);
							}
							else
							{

								Bitmap image_read_new = new Bitmap(pictureBox1.Image);
								image_lectum_convert(image_read_new, get_file_name);
							}
							break;

					}
					




					textBox1.Clear();
					textBox1.AppendText("the image rotated 180°");
				}
				else
				{
					textBox1.Clear();
					textBox1.AppendText("choose an image first");
				}

			     	


			}
			catch
			{


			}
		}


		public void image_lectum_convert(Bitmap image_read, string image_string_filename)
		{

			byte[,] image_byte_array = ConvertToGrayscale(image_read);
			Bitmap Bitmap_4GL = SaveByteArryToBitmap(image_byte_array, image_read.Width, image_read.Height);
			pictureBox_bw.Image = Bitmap_4GL;


			byte[] image_uc8156_no_scramble_buffer = image_uc8156_array_no_scramble(image_byte_array, image_read.Height, image_read.Width);
			string image_string_head = byte_array_to_string(image_uc8156_no_scramble_buffer);


			image_string_head = "," + image_string_head;
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
