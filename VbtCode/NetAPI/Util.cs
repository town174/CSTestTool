using NetAPI.Core;
using System;
using System.Text.RegularExpressions;

namespace NetAPI
{
	public class Util
	{
		public static string ConvertbyteArrayToHexstring(byte[] byte_array)
		{
			if (byte_array == null)
			{
				return "";
			}
			string text = string.Empty;
			foreach (byte b in byte_array)
			{
				text += $"{b:X2}";
			}
			return text;
		}

		public static string ConvertbyteArrayToHexWordstring(byte[] byte_array)
		{
			if (byte_array == null)
			{
				return "";
			}
			string text = string.Empty;
			byte[] array = null;
			if (byte_array.Length % 2 == 1)
			{
				array = new byte[byte_array.Length + 1];
				Array.Copy(byte_array, 0, array, 0, byte_array.Length);
			}
			else
			{
				array = byte_array;
			}
			for (int i = 0; i < array.Length; i += 2)
			{
				text += $"{array[i]:X2}{array[i + 1]:X2} ";
			}
			return text;
		}

		public static string ConvertbyteArrayToHexWithBlankSpace(byte[] byte_array)
		{
			if (byte_array == null)
			{
				return "";
			}
			string text = string.Empty;
			foreach (byte b in byte_array)
			{
				text += $"{b:X2} ";
			}
			return text.Trim();
		}

		public static byte[] ConvertHexstringTobyteArray(string str)
		{
			str = str.Replace(" ", "");
			float num = (float)str.Length;
			int num2 = (int)Math.Ceiling((double)(num / 2f));
			string text = null;
			byte[] array = new byte[num2];
			text = ((!((float)(num2 * 2) > num)) ? str : ("0" + str));
			for (int i = 0; i < num2; i++)
			{
				int num3 = i * 2;
				char[] value = new char[2]
				{
					text[num3],
					text[num3 + 1]
				};
				string value2 = new string(value);
				try
				{
					array[i] = Convert.ToByte(value2, 16);
				}
				catch (OverflowException)
				{
					Console.WriteLine("Conversion from string to byte overflowed.");
				}
				catch (FormatException)
				{
					Console.WriteLine("The string is not formatted as a byte.");
				}
				catch (ArgumentNullException)
				{
					Console.WriteLine("The string is null.");
				}
			}
			return array;
		}

		public static string XmlstringReplace(string str)
		{
			str = str.Replace("&", "&amp;");
			str = str.Replace("<", "&lt;");
			str = str.Replace(">", "&gt;");
			str = str.Replace("'", "&apos;");
			str = str.Replace("\"", "&quot;");
			return str;
		}

		public static string Xmlstring(string str)
		{
			str = str.Replace("&lt;", "<");
			str = str.Replace("&gt;", ">");
			str = str.Replace("&apos;", "'");
			str = str.Replace("&quot;", "\"");
			str = str.Replace("&quot;", "&");
			return str;
		}

		public static void logAndTriggerApiErr(string senderName, string errCode, string exceptionMsg, LogType logType)
		{
			string str = senderName + ":";
			str = ((!ErrInfoList.ErrDictionary.ContainsKey(errCode)) ? (str + " ErrCode(" + errCode + ") ") : (str + ErrInfoList.ErrDictionary[errCode]));
			if (!string.IsNullOrEmpty(exceptionMsg))
			{
				str = str + "|Exception:" + exceptionMsg;
			}
			AbstractReader.TrigerApiException(senderName, new ErrInfo(errCode, str));
			switch (logType)
			{
			case LogType.Fatal:
				Log.Fatal(str);
				break;
			case LogType.Error:
				Log.Error(str);
				break;
			case LogType.Warn:
				Log.Warn(str);
				break;
			case LogType.Info:
				Log.Info(str);
				break;
			case LogType.Debug:
				Log.Debug(str);
				break;
			}
		}

		public static int ConverTagNumToQ(int tagNum)
		{
			return (int)Math.Ceiling(Math.Log10(Convert.ToDouble(tagNum)) / Math.Log10(2.0));
		}

		public static bool IsHexstring(string str)
		{
			str = str.Replace(" ", "");
			string pattern = "^[0-9a-fA-F]+$";
			if (Regex.IsMatch(str, pattern))
			{
				return true;
			}
			return false;
		}

		public static byte[] ConvertTobyteArray(ushort value)
		{
			return new byte[2]
			{
				(byte)(value >> 8),
				(byte)(value & 0xFF)
			};
		}

		public static DateTime GetUTC(byte[] readTime)
		{
			DateTime result = DateTime.MaxValue;
			if (readTime != null && readTime.Length == 8)
			{
				double num = (double)((readTime[0] << 24) + (readTime[1] << 16) + (readTime[2] << 8) + readTime[3]) * 1000.0;
				uint num2 = (uint)((readTime[4] << 24) + (readTime[5] << 16) + (readTime[6] << 8) + readTime[7]) / 1000u;
				if (num2 < 1000)
				{
					num += (double)num2;
				}
				result = DateTime.Parse("1970-01-01").AddMilliseconds(num);
			}
			return result;
		}

		public static int bytesToInt32(byte[] buff, int startIndex)
		{
			int result = 0;
			if (buff != null && buff.Length - startIndex > 4)
			{
				byte[] array = new byte[4];
				Array.Copy(buff, startIndex, array, 0, 4);
				Array.Reverse(array);
				result = BitConverter.ToInt32(array, 0);
			}
			return result;
		}

		public static byte[] Int32Tobytes(int data)
		{
			byte[] bytes = BitConverter.GetBytes(data);
			Array.Reverse(bytes);
			return bytes;
		}
	}
}
