﻿using System;
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
	public partial class Form1
	{
		public static byte[] converterDemo(Image x)
		{
			ImageConverter _imageConverter = new ImageConverter();
			byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
			return xByte;
		}

		public byte[] imageToByteArray(System.Drawing.Image image)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
				return ms.ToArray();
			}
		}

		static byte[,] ConvertToGrayscale(Image Source)
		{
			Bitmap source = (Bitmap)Source;
			byte[,] bytes = new byte[source.Height, source.Width]; //create array to contain bitmap data with padding

			for (int x = 0; x < source.Height; x++)
			{
				for (int y = 0; y < source.Width; y++)
				{
					Color c = source.GetPixel(y, x);
					byte c_red = c.R;
					byte c_green = c.G;
					byte c_blue = c.B;
					//	int g = Convert.ToInt32(0.3 * c.R + 0.59 *
					//c.G + 0.11 * c.B); //grayscale shade corresponding to rgb
					if (c_red != c_green)
					{
						Debug.Print("GL error");
					}



					if (c_red <= 0x43)
					{
						c_red = 0x00;
					}
					else if (c_red > 0x43 && c_red <= 0x91)
					{
						c_red = 0x44;
					}
					else if (c_red > 0x91 && c_red <= 0xD0)
					{
						c_red = 0xAA;
					}
					else if (c_red > 0xD0 && c_red == 0xFF)
					{
						c_red = 0xFF;
					}



					bytes[x, y] = c_red;
				}

			}
			return bytes;
		}



		static byte[,] Acep_ConvertToByteArray(Image Source, int color_sort1, int color_sort2, int color_sort3, int color_sort4, out bool color_channel_bool)
		{
			Bitmap source = (Bitmap)Source;
			byte[,] bytes = new byte[source.Height, source.Width]; //create array to contain bitmap data with padding
			color_channel_bool = false;
			for (int x = 0; x < source.Height; x++)
			{
				for (int y = 0; y < source.Width; y++)
				{
					Color c = source.GetPixel(y, x);
					int c_red = c.R;
					int c_green = c.G;
					int c_blue = c.B;
					int get_pixel_color = color_check(c_red, c_green, c_blue);
					int g = 0xff;
					if (get_pixel_color == color_sort1 || get_pixel_color == color_sort2 || get_pixel_color == color_sort3 || get_pixel_color == color_sort4)
					{
						g = 0x00;
						color_channel_bool = true;
					}

					bytes[x, y] = (byte)g;
				}

			}

			return bytes;
		}





		public class PgmImage
		{
			public int width;
			public int height;
			public int maxVal;
			public byte[][] pixels;

			public PgmImage(int width, int height, int maxVal,
			  byte[][] pixels)
			{
				this.width = width;
				this.height = height;
				this.maxVal = maxVal;
				this.pixels = pixels;
			}
		}


		public PgmImage LoadImage(string file)
		{
			FileStream ifs = new FileStream(file, FileMode.Open);
			BinaryReader br = new BinaryReader(ifs);

			string magic = NextNonCommentLine(br);
			if (magic != "P5")
				throw new Exception("Unknown magic number: " + magic);


			string widthHeight = NextNonCommentLine(br);
			string[] tokens = widthHeight.Split(' ');
			int width = int.Parse(tokens[0]);
			int height = int.Parse(tokens[1]);


			string sMaxVal = NextNonCommentLine(br);
			int maxVal = int.Parse(sMaxVal);



			// read width * height pixel values . . .
			byte[][] pixels = new byte[height][];
			for (int i = 0; i < height; ++i)
				pixels[i] = new byte[width];

			for (int i = 0; i < height; ++i)
				for (int j = 0; j < width; ++j)
					pixels[i][j] = br.ReadByte();


			br.Close(); ifs.Close();

			PgmImage result = new PgmImage(width, height, maxVal, pixels);
			return result;
		}




		static string NextAnyLine(BinaryReader br)
		{
			string s = "";
			byte b = 0; // dummy
			while (b != 10) // newline
			{
				b = br.ReadByte();
				char c = (char)b;
				s += c;
			}
			return s.Trim();
		}

		static string NextNonCommentLine(BinaryReader br)
		{
			string s = NextAnyLine(br);
			while (s.StartsWith("#") || s == "")
				s = NextAnyLine(br);
			return s;
		}


		public byte[] image_uc8156_array(byte[,] buffer_in, int image_height, int image_width)  // for 2.1 height 146, width 240
		{


			int image_pixel_sum = image_height * image_width;
			byte[] image_array_buffer = new byte[image_pixel_sum];
			byte[] image_array_buffer_temp = new byte[image_pixel_sum];


			byte[] image_uc8156_buffer = new byte[image_height * image_width / 4];

			for (int i = 0; i < image_height; i++)
			{
				for (int j = 0; j < image_width; j++)
				{
					image_array_buffer_temp[i * image_width + j] = buffer_in[i, j];  // image_array_buffer_temp here no scramble
				}
			}

			byte[] image_scramble_buffer1 = new byte[image_pixel_sum];

			for (int i = 0; i < image_height; i++)
			{
				for (int j = 0; j < image_width; j++)
				{
					if (j < image_width / 2)
					{

						image_array_buffer[i * image_width + j] = image_array_buffer_temp[i * image_width + j + image_width / 2];
					}
					else  //  image_array_buffer[j] von 120 bis 239
					{

						image_scramble_buffer1[i * image_width + j] = image_array_buffer_temp[i * image_width + j - image_width / 2];
					}
				}

			}
			byte[] image_array_split = new byte[image_height * image_width]; // column 120, row 146
			for (int i = 0; i < image_height; i++)
			{
				for (int j = 0; j < image_width; j++)
				{
					if (j >= image_width / 2)
					{
						image_array_split[i * image_width + j] = image_scramble_buffer1[i * image_width + image_width + image_width / 2 - 1 - j];
					}

				}
			}




			for (int i = 0; i < image_height; i++)
			{
				for (int j = 0; j < image_width; j++)
				{

					if (j >= image_width / 2)
					{

						image_array_buffer[i * image_width + j] = image_array_split[i * image_width + j];
					}
				}


			}



			int image_uc8156_buffer_count = image_height * image_width / 4;

			//	image_uc8156_buffer = byte_array_pixel_8156(image_array_buffer_temp, image_height, image_width);
			image_uc8156_buffer = byte_array_pixel_8156(image_array_buffer, image_height, image_width);
			return image_uc8156_buffer;
		}








		public byte[] image_uc8156_S031_array(byte[,] buffer_in, int image_height, int image_width)  // for 2.1 height 146, width 240
		{


			int image_pixel_sum = image_height * image_width;            // height->gate: 312; width-> source: 74
			int gate_line_new = image_height / 2;						// 312 / 2 = 156
			int source_line_new = image_width * 2;						// 74x2	= 148
			byte[,] buffer_array_temp = new byte[gate_line_new, source_line_new];  // [156, 148]
		
			byte[] image_array_buffer = new byte[image_pixel_sum];
			for (int j = 0; j < gate_line_new; j++)
			{
				byte[] source_buffer = new byte[source_line_new];
				for (int k = 0; k < image_width; k++)
				{
					source_buffer[k] = buffer_in[j*2,k];
					source_buffer[image_width + k] = buffer_in[j*2+1, k];
				}
				for (int i = 0; i < source_line_new/2; i++)
				{
					buffer_array_temp[j, i * 2] = source_buffer[source_line_new / 2 + i];
					buffer_array_temp[j, i * 2 + 1] = source_buffer[i];
				}


			}

			for (int j = 0; j < gate_line_new; j++)
			{
				for (int i = 0; i < source_line_new; i++)
				{
					image_array_buffer[j * source_line_new + i] = buffer_array_temp[j,i];
				}

			}

			//	image_uc8156_buffer = byte_array_pixel_8156(image_array_buffer_temp, image_height, image_width);
			byte[] image_uc8156_buffer = byte_array_pixel_8156(image_array_buffer, gate_line_new, source_line_new);
			return image_uc8156_buffer;
		}








		public byte[] image_uc8156_array_no_scramble(byte[,] buffer_in, int image_height, int image_width)  // 2D byte array to 1D byte array ,right now for 2.1 Legio and 1.1 Lectum
		{

			int image_pixel_sum = image_height * image_width;
			byte[] image_array_buffer = new byte[image_pixel_sum];
			byte[] image_array_buffer_temp = new byte[image_pixel_sum];


			byte[] image_uc8156_buffer = new byte[image_height * image_width / 4];

			for (int i = 0; i < image_height; i++)
			{
				for (int j = 0; j < image_width; j++)
				{
					image_array_buffer_temp[i * image_width + j] = buffer_in[i, j];
				}
			}
		
			image_uc8156_buffer = byte_array_pixel_8156(image_array_buffer_temp, image_height, image_width);
			

			return image_uc8156_buffer;
		}


		public byte[] image_uc8179_array_no_scramble(byte[,] buffer_in, int image_height, int image_width)  // 2D byte array to 1D byte array ,right now for 2.1 Legio and 1.1 Lectum
		{

			int image_pixel_sum = image_height * image_width;
			byte[] image_array_buffer = new byte[image_pixel_sum];
			byte[] image_array_buffer_temp = new byte[image_pixel_sum];


			byte[] image_uc8156_buffer = new byte[image_height * image_width / 8];

			for (int i = 0; i < image_height; i++)
			{
				for (int j = 0; j < image_width; j++)
				{
					image_array_buffer_temp[i * image_width + j] = buffer_in[i, j];
				}
			}

			image_uc8156_buffer = byte_array_pixel_8179(image_array_buffer_temp, image_height, image_width);


			return image_uc8156_buffer;
		}




		public byte[] image_uc8156_array_full_no_scramble(byte[,] buffer_in, int image_height, int image_width)  // full byte array without scrambling or bit work
		{

			int image_pixel_sum = image_height * image_width;
			byte[] image_array_buffer = new byte[image_pixel_sum];


			for (int i = 0; i < image_height; i++)
			{
				for (int j = 0; j < image_width; j++)
				{
					image_array_buffer[i * image_width + j] = buffer_in[i, j];
				}
			}

			return image_array_buffer;
		}




		public static class PGMUtil
		{
			private static ColorPalette grayScale;

			public static Bitmap ToBitmap(string filePath)
			{
				using (FileStream fs = new FileStream(filePath, FileMode.Open))
				{
					using (BinaryReader reader = new BinaryReader(fs, Encoding.ASCII))
					{
						if (reader.ReadChar() == 'P' && reader.ReadChar() == '5')
						{




							while (reader.ReadChar() == '\n')
							{
								break;
							}

							char read_next_char = reader.ReadChar();
							bool test = (read_next_char == '#');
							if (test)
							{
								while (true)
								{
									char check_char = reader.ReadChar();
									if (check_char == '\n')
									{
									
										break;
									}

								}

							}


							//reader.ReadChar();
							int width = 0;
							int height = 0;
							int level = 0;
							bool two = false;
							StringBuilder sb = new StringBuilder();
							if (!test)
							{
								int width_test = ReadNumber(reader, sb);
								string width_string = width_test.ToString();
								string width_temp = read_next_char.ToString() + width_string;
								int.TryParse(width_temp, out width);

							}
							else
							{
								width = ReadNumber(reader, sb);
							}
							height = ReadNumber(reader, sb);
							level = ReadNumber(reader, sb);
							two = (level > 255);

							Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
							if (grayScale == null)
							{
								grayScale = bmp.Palette;
								for (int i = 0; i < 256; i++)
								{
									grayScale.Entries[i] = Color.FromArgb(i, i, i);
								}
							}
							bmp.Palette = grayScale;
							BitmapData dt = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
							int offset = dt.Stride - dt.Width;
							unsafe
							{
								byte* ptr = (byte*)dt.Scan0;

								for (int i = 0; i < height; i++)
								{
									for (int j = 0; j < width; j++)
									{
										byte v;
										if (two)
										{
											v = (byte)(((double)((reader.ReadByte() << 8) + reader.ReadByte()) / level) * 255.0);
										}
										else
										{
											v = reader.ReadByte();
										}
										*ptr = v;
										ptr++;
									}
									ptr += offset;
								}
							}

							bmp.UnlockBits(dt);
							return bmp;

						}
						else
						{
							throw new InvalidOperationException("target file is not a PGM file");
						}
					}
				}
			}

			public static void SaveAsBitmap(string src, string dest)
			{
				ToBitmap(src).Save(dest, ImageFormat.Bmp);
			}

			private static int ReadNumber(BinaryReader reader, StringBuilder sb)
			{
				char c = '\0';
				sb.Length = 0;


				while (Char.IsDigit(c = reader.ReadChar()))
				{
					sb.Append(c);
				}
				return int.Parse(sb.ToString());
		
			}
		}






		public static int color_check(int acep_red, int acep_green, int acep_blue)
		{
			int acep_color_check_int = (int)acep_color_enum.white;
			if (acep_red <= 0xCD && acep_green <= 0xCD && acep_blue <= 0xCD)        // black
			{
				acep_color_check_int = (int)acep_color_enum.black;
			}

			if (acep_red >= 0xCE && acep_green >= 0xCE && acep_blue >= 0xCE)        // white
			{
				acep_color_check_int = (int)acep_color_enum.white;
			}

			if (acep_red >= 0xAF && acep_green <= 0x50 && acep_blue <= 0x50)        // red
			{
				acep_color_check_int = (int)acep_color_enum.red;
			}


			if (acep_red <= 0x50 && acep_green >= 0xAF && acep_blue <= 0x50)        // green
			{
				acep_color_check_int = (int)acep_color_enum.green;
			}

			if (acep_red <= 0x50 && acep_green <= 0x50 && acep_blue >= 0xAF)        // blue
			{
				acep_color_check_int = (int)acep_color_enum.blue;
			}

			if (acep_red >= 0xAF && acep_green >= 0xAF && acep_blue <= 0x50)        // yellow
			{
				acep_color_check_int = (int)acep_color_enum.yellow;
			}

			return acep_color_check_int;

		}

		enum acep_color_enum
		{
			white = (int)00,
			black = (int)01,
			red = (int)02,
			green = (int)03,
			blue = (int)04,
			yellow = (int)05

		}



		public void ImageFromByteArray(byte[,] arr, int width, int height, string path)
		{
			//Image bitmap;
			//int stride = width * 4;
			//unsafe
			//{
			//	fixed (byte* intPtr = &arr[0, 0])
			//	{
			//		bitmap = new Bitmap(width, height, stride, pixelFormat, new IntPtr(intPtr));
			//	}
			//}
			byte[] buffer_temp = new byte[width * height];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					buffer_temp[i * width + j] = arr[i, j];
				}

			}
			System.IO.File.WriteAllBytes(@path, buffer_temp);

			pictureBox1.ImageLocation = @path;


		}


		public void acep_color_byte_array_all(Bitmap source, string folder_path, string file_name, int image_width, int image_height)
		{
			try
			{
				string image_string_all = null;
				string get_new_bitmap_name = null;
				string new_bitmap_save_path = null;
				string new_test_map_name = null;
				/////////////////////////////////////////////////////////////////////// for Legio_bw EPD_BLACK
				byte[,] image_byte_array = Acep_ConvertToByteArray(source, (int)acep_color_enum.black, (int)acep_color_enum.red, (int)acep_color_enum.yellow, (int)acep_color_enum.green, out color_channel_bw);
				if (color_channel_bw)
				{
					color_channel = (byte)(color_channel | 0x80);
					pictureBox_bw.Image = bitmap_EPD_Image(source, image_byte_array, image_width, image_height);












					get_new_bitmap_name = file_name + "_Legio_bw.txt"; // 
					new_bitmap_save_path = folder_path + "\\" + get_new_bitmap_name;
					new_test_map_name = folder_path + "\\" + file_name + "_Legio_bw.bmp";



					byte[] image_uc8156_buffer = image_uc8156_array_no_scramble(image_byte_array, image_height, image_width);

					legio_bw_string = "," + byte_array_to_string(image_uc8156_buffer);


					for (int i = 0; i < image_uc8156_buffer.Length; i++)
					{
						string comma_string = null;
						if (i < image_uc8156_buffer.Length - 1)
						{
							comma_string = ",";
						}
						else
						{
							comma_string = null;
						}
						image_string_all = image_string_all + image_uc8156_buffer[i].ToString() + comma_string;

					}

					if (image_convert_save)
					{
						pictureBox_bw.Image.Save(@new_test_map_name);
						File.WriteAllText(new_bitmap_save_path, image_string_all);
					}

					textBox1.Clear();
					textBox1.AppendText(get_new_bitmap_name + " convert success" + '\n');
					image_string_all = null;
				}
				else
				{
					legio_bw_string = "," + empty_to_string();
				}
				/////////////////////////////////////////////////////////////////////////////////////////////


				/////////////////////////////////////////////////////////////////////// for Legio_yg  EPD_YELLOW
				image_byte_array = Acep_ConvertToByteArray(source, -1, -1, (int)acep_color_enum.yellow, (int)acep_color_enum.green, out color_channel_yellow);

				if (color_channel_yellow)
				{
					color_channel = (byte)(color_channel | 0x40);
					pictureBox_yg.Image = bitmap_EPD_Image(source, image_byte_array, image_width, image_height);
					get_new_bitmap_name = file_name + "_Legio_yg.txt"; // 
					new_bitmap_save_path = folder_path + "\\" + get_new_bitmap_name;
					new_test_map_name = folder_path + "\\" + file_name + "_Legio_yg.bmp";




					byte[] image_uc8156_buffer = image_uc8156_array_no_scramble(image_byte_array, image_height, image_width);
					legio_yellow_string = "," + byte_array_to_string(image_uc8156_buffer);




					for (int i = 0; i < image_uc8156_buffer.Length; i++)
					{
						string comma_string = null;
						if (i < image_uc8156_buffer.Length - 1)
						{
							comma_string = ",";
						}
						else
						{
							comma_string = null;
						}
						image_string_all = image_string_all + image_uc8156_buffer[i].ToString() + comma_string;

					}

					if (image_convert_save)
					{
						pictureBox_yg.Image.Save(@new_test_map_name);
						File.WriteAllText(new_bitmap_save_path, image_string_all);
					}
					textBox1.AppendText(get_new_bitmap_name + " convert success" + '\n');


				}
				else
				{
					legio_yellow_string = "," + empty_to_string();
				}
				/////////////////////////////////////////////////////////////////////////////////////////////



				/////////////////////////////////////////////////////////////////////// for Legio_g  EPD_GREEN
				image_byte_array = Acep_ConvertToByteArray(source, -1, -1, -1, (int)acep_color_enum.green, out color_channel_green);

				if (color_channel_green)
				{
					color_channel = (byte)(color_channel | 0x20);
					pictureBox_g.Image = bitmap_EPD_Image(source, image_byte_array, image_width, image_height);
					get_new_bitmap_name = file_name + "_Legio_g.txt"; // 
					new_bitmap_save_path = folder_path + "\\" + get_new_bitmap_name;


					new_test_map_name = folder_path + "\\" + file_name + "_Legio_g.bmp";




					byte[] image_uc8156_buffer = image_uc8156_array_no_scramble(image_byte_array, image_height, image_width);

					legio_green_string = "," + byte_array_to_string(image_uc8156_buffer);




					for (int i = 0; i < image_uc8156_buffer.Length; i++)
					{
						string comma_string = null;
						if (i < image_uc8156_buffer.Length - 1)
						{
							comma_string = ",";
						}
						else
						{
							comma_string = null;
						}
						image_string_all = image_string_all + image_uc8156_buffer[i].ToString() + comma_string;

					}


					if (image_convert_save)
					{
						pictureBox_g.Image.Save(@new_test_map_name);
						File.WriteAllText(new_bitmap_save_path, image_string_all);
					}


					textBox1.AppendText(get_new_bitmap_name + " convert success" + '\n');
					image_string_all = null;
				}
				else
				{
					legio_green_string = "," + empty_to_string();
				}





				/////////////////////////////////////////////////////////////////////////////////////////////


				/////////////////////////////////////////////////////////////////////// for Legio_r  EPD_RED
				image_byte_array = Acep_ConvertToByteArray(source, -1, (int)acep_color_enum.red, -1, -1, out color_channel_red);


				if (color_channel_red)
				{
					color_channel = (byte)(color_channel | 0x10);
					pictureBox_r.Image = bitmap_EPD_Image(source, image_byte_array, image_width, image_height);
					


					get_new_bitmap_name = file_name + "_Legio_r.txt"; // 
					new_bitmap_save_path = folder_path + "\\" + get_new_bitmap_name;
					new_test_map_name = folder_path + "\\" + file_name + "_Legio_r.bmp";




					byte[] image_uc8156_buffer = image_uc8156_array_no_scramble(image_byte_array, image_height, image_width);
					legio_red_string = "," + byte_array_to_string(image_uc8156_buffer);




					for (int i = 0; i < image_uc8156_buffer.Length; i++)
					{
						string comma_string = null;
						if (i < image_uc8156_buffer.Length - 1)
						{
							comma_string = ",";
						}
						else
						{
							comma_string = null;
						}
						image_string_all = image_string_all + image_uc8156_buffer[i].ToString() + comma_string;

					}


					if (image_convert_save)
					{
						pictureBox_r.Image.Save(@new_test_map_name);
						File.WriteAllText(new_bitmap_save_path, image_string_all);
					}

					textBox1.AppendText(get_new_bitmap_name + " convert success" + '\n');
					image_string_all = null;
				}
				else
				{
					legio_red_string = "," + empty_to_string();
				}


				/////////////////////////////////////////////////////////////////////////////////////////////



				/////////////////////////////////////////////////////////////////////// for Legio_b  EPD_BLUE
				image_byte_array = Acep_ConvertToByteArray(source, -1, (int)acep_color_enum.blue, -1, -1, out color_channel_blue);

				if (color_channel_blue)
				{
					color_channel = (byte)(color_channel | 0x08);
					pictureBox_b.Image = bitmap_EPD_Image(source, image_byte_array, image_width, image_height);
					get_new_bitmap_name = file_name + "_Legio_b.txt"; // 
					new_bitmap_save_path = folder_path + "\\" + get_new_bitmap_name;
					new_test_map_name = folder_path + "\\" + file_name + "_Legio_b.bmp";




					byte[] image_uc8156_buffer = image_uc8156_array_no_scramble(image_byte_array, image_height, image_width);
					legio_blue_string = "," + byte_array_to_string(image_uc8156_buffer);



					for (int i = 0; i < image_uc8156_buffer.Length; i++)
					{
						string comma_string = null;
						if (i < image_uc8156_buffer.Length - 1)
						{
							comma_string = ",";
						}
						else
						{
							comma_string = null;
						}
						image_string_all = image_string_all + image_uc8156_buffer[i].ToString() + comma_string;

					}

					if (image_convert_save)
					{
						pictureBox_b.Image.Save(@new_test_map_name);
						File.WriteAllText(new_bitmap_save_path, image_string_all);
					}

					textBox1.AppendText(get_new_bitmap_name + " convert success" + "\r\n");
					
					image_string_all = null;
				}
				else
				{
					legio_blue_string = "," + empty_to_string();
				}








				head_array[3] = color_channel;
				/////////////////////////////////////////////////////////////////////////////////////////////
			
				StringBuilder sb = new StringBuilder();
				Debug.Print("color channel: " + "0x" + sb.AppendFormat("{0:x2}", color_channel));

				WriteFileHeader(legio_bw_string, legio_yellow_string, legio_green_string, legio_red_string, legio_blue_string);
				color_channel = 0;



			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				textBox1.Text = "error";
			}


		}


		public Bitmap bitmap_EPD_Image(Bitmap source, byte[,] image_byte_array, int image_width, int image_height)
		{
			Bitmap bitmap_test = new Bitmap(source);
			for (int x = 0; x < image_height; x++)
			{
				for (int y = 0; y < image_width; y++)
				{
					if (image_byte_array[x, y] < 0x0f)
					{
						bitmap_test.SetPixel(y, x, Color.Black);

					}
					else
					{
						bitmap_test.SetPixel(y, x, Color.White);
					}
				}
			}

			return bitmap_test;
		}





		public byte[] byte_array_pixel_8156(byte[] image_array_buffer, int image_height, int image_width)  // rebuild the byte array for UC8156
		{

			int image_uc8156_buffer_count = image_height * image_width / 4;
			byte[] image_uc8156_buffer = new byte[image_uc8156_buffer_count];
			for (int i = 0; i < image_uc8156_buffer_count; i++)
			{
				image_uc8156_buffer[i] = (byte)((image_array_buffer[i * 4] >> 6) << 6 | (image_array_buffer[i * 4 + 1] >> 6) << 4 | (image_array_buffer[i * 4 + 2] >> 6) << 2 | (image_array_buffer[i * 4 + 3] >> 6));


			}
			return image_uc8156_buffer;

		}



		public byte[] byte_array_pixel_8179(byte[] image_array_buffer, int image_height, int image_width)  // rebuild the byte array for UC8179
		{

			int image_uc8179_buffer_count = image_height * image_width / 8;
			byte[] image_uc8179_buffer = new byte[image_uc8179_buffer_count];
			for (int i = 0; i < image_uc8179_buffer_count; i++)
			{
				image_uc8179_buffer[i] = (byte)((image_array_buffer[i * 8] & 0x80) | ((image_array_buffer[i * 8 + 1] & 0x80) >> 1 )| ((image_array_buffer[i * 8 + 2] & 0x80) >> 2) | ((image_array_buffer[i * 8 + 3]) >> 3) | ((image_array_buffer[i * 8 + 4]) >> 4) | ((image_array_buffer[i * 8 + 5]) >> 5) | ((image_array_buffer[i * 8 + 6]) >> 6) | ((image_array_buffer[i * 8 + 7]) >> 7));


			}
			return image_uc8179_buffer;

		}


		public Bitmap SaveByteArryToBitmap(Byte[,] data, int width, int height)
		{
			Bitmap bmp = new Bitmap(width, height);

			for (int r = 0; r < height; r++)
			{
				for (int c = 0; c < width; c++)
				{
					Byte value = data[r, c];
					bmp.SetPixel(c, r, Color.FromArgb(value, value, value));
				}
			}

			return bmp;

		}




	}
}
