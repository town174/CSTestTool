using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTool.Utils
{
    public class HexUtils
    {
        public static string ByteToAsciiStr(byte[] bytes)
        {
            try
            {
                if (bytes == null) return "";
                string text = System.Text.Encoding.ASCII.GetString(bytes);
                return text.TrimEnd(new char[] { '\0', ' ', '\r', '\n' });
            }
            catch
            {
                return "";
            }
        }

        public static string ByteToHexStr(byte[] bytes)
        {
            try
            {
                string returnStr = "";
                if (bytes != null)
                {
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        returnStr += bytes[i].ToString("X2");
                    }
                }
                return returnStr;
            }
            catch { }
            return "";
        }

        public static byte[] HexStrToByte(string hexText)
        {
            byte[] hex = new byte[2];
            try
            {
                if (string.IsNullOrEmpty(hexText) || hexText.Length < 2) return hex;
                int len = hexText.Length;
                hex = new byte[len / 2];

                for (int i = 0; i < len / 2; i++)
                {
                    hex[i] = Convert.ToByte(hexText.Substring(i * 2, 2), 16);
                }

            }
            catch { }

            return hex;
        }

        public static string HexStrToBinStr(string hexString)
        {
            string result = string.Empty;
            foreach (char c in hexString)
            {
                int v = Convert.ToInt32(c.ToString(), 16);
                int v2 = int.Parse(Convert.ToString(v, 2));
                // 去掉格式串中的空格，即可去掉每个4位二进制数之间的空格，
                result += string.Format("{0:d4} ", v2);
            }
            return result.Replace(" ", "");
        }

        public static bool TryHexStrToByte(string hexText, out byte[] buff)
        {
            bool ret = false;
            buff = new byte[2];
            try
            {
                if (string.IsNullOrEmpty(hexText) || hexText.Length < 2) return false;
                int len = hexText.Length;
                byte[] hex = new byte[len / 2];

                for (int i = 0; i < len / 2; i++)
                {
                    hex[i] = Convert.ToByte(hexText.Substring(i * 2, 2), 16);
                }
                buff = hex;
                ret = true;
            }
            catch { }

            return ret;
        }
    }
}
