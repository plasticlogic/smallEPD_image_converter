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
	public partial class Form1 
	{

		public bool WriteFileHeader(string legio_bw_string, string legio_yellow_string, string legio_green_string, string legio_red_string, string legio_blue_string)   // for Legio Acep
		{
			try
			{
				string image_head_start = "# ifndef " + image_legio_name_h + "\r\n" + "#define " + image_legio_name_h + "\r\n" + "const unsigned char " + image_legio_name_ori + "[] PROGMEM = { ";

				string image_head_array_string = null;
				image_head_array_string = byte_array_to_string(head_array);

				image_head_start = image_head_start + image_head_array_string;




				string image_head_end = "};" + "\r\n" + "#endif";
				string image_head_complete = image_head_start + legio_bw_string + legio_yellow_string + legio_green_string + legio_red_string + legio_blue_string + image_head_end;

				string head_path = image_acep_folder_path + "\\" + text_name;

				File.WriteAllText(head_path, image_head_complete);
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		public bool WriteFileHeader(string legio_grey_string)  // for Greyscale Lectum
		{
			try
			{
				string image_head_start = "# ifndef " + image_legio_name_h + "\r\n" + "#define " + image_legio_name_h + "\r\n" + "const unsigned char " + image_legio_name_ori + "[] PROGMEM = { ";

				string image_head_array_string = null;
				image_head_array_string = byte_array_to_string(head_array);

				image_head_start = image_head_start + image_head_array_string;




				string image_head_end = "};" + "\r\n" + "#endif";
				string image_head_complete = image_head_start + legio_grey_string + image_head_end;

				string head_path = image_acep_folder_path + "\\" + text_name;

				File.WriteAllText(head_path, image_head_complete);
				File.WriteAllText(text_name_greyscale_path, image_head_array_string+legio_grey_string);  // scramble for MSP430 byte array
				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}



		public string empty_to_string()
		{
			string char_string = null;


			for (int i = 0; i < length; i++)
			{
				string comma_show = string.Empty;
				if (i != (length - 1))
				{
					comma_show = ",";
				}

				char_string = char_string + "0x00" + comma_show;

			}

			return char_string;
		}

		public string byte_array_to_string(byte[] byte_array)
		{
			string char_string = null;


			for (int i = 0; i < byte_array.Length; i++)
			{
				string comma_show = string.Empty;
				if (i != (byte_array.Length - 1))
				{
					comma_show = ",";
				}

				char_string = char_string + "0x" + byte_array[i].ToString("X2") + comma_show;

			}

			return char_string;
		}




		public string byte_array_to_char(byte[] byte_array)
		{
			string  char_string = null;


			for (int i = 0; i < byte_array.Length; i++)
			{
				string comma_show = string.Empty;


				char_string = char_string  + Convert.ToChar( byte_array[i]);

			}

			return char_string;
		}




		public string reform_file_name(string file_name)
		{
			string reform_name_string = null;
			ArrayList File_Name_ArL = new ArrayList();
			foreach (var ch_char in file_name)
			{
				if (!Char.IsDigit(ch_char))
				{
					if (ch_char == '.')
					{
						File_Name_ArL.Add("");
					}
					else if (ch_char == '-')
					{
						File_Name_ArL.Add('_');
					}

					else
					{

						File_Name_ArL.Add(ch_char);
					}
				}
				else
				{
					File_Name_ArL.Add(ch_char);
				}
			}
			var strings = from object o in File_Name_ArL
						  select o.ToString();

			var reform_name_string_temp = string.Join("", strings.ToArray());
			ArrayList File_Name_end_ArL = new ArrayList();
			bool first_char_find = false;
			if (reform_name_string_temp[0] != '_' && !Char.IsDigit(reform_name_string_temp[0]))
			{
				reform_name_string = reform_name_string_temp;
			}
			else
			{
				for (int i = 0; i < reform_name_string_temp.Length; i++)
				{
					if (((reform_name_string_temp[i] != '_') && !Char.IsDigit(reform_name_string_temp[i])) && (!first_char_find))
					{
						first_char_find = true;
					}
					if (first_char_find)
					{
						File_Name_end_ArL.Add(reform_name_string_temp[i]);
					}
				}
				var strings_temp = from object o in File_Name_end_ArL
								   select o.ToString();

				reform_name_string = string.Join("", strings_temp.ToArray());

			}


			return reform_name_string;

		}

	}
}
