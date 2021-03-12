using System;

namespace NetAPI.Protocol.VRP
{
	internal class EVB
	{
		public static byte[] ConvertToEvb(uint value)
		{
			string text = Convertstring(value.ToString(), 10, 2);
			if (text.Length % 7 > 0)
			{
				text = text.PadLeft(text.Length + 7 - text.Length % 7, '0');
			}
			byte[] array = new byte[text.Length / 7];
			if (array.Length > 1)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (i == array.Length - 1)
					{
						array[i] = Convert.ToByte("0" + text.Substring(7 * i, 7), 2);
					}
					else
					{
						array[i] = Convert.ToByte("1" + text.Substring(7 * i, 7), 2);
					}
				}
			}
			else if (array.Length != 0)
			{
				array[0] = Convert.ToByte("0" + text, 2);
			}
			return array;
		}

		public static string Convertstring(string value, int fromBase, int toBase)
		{
			uint num = Convert.ToUInt32(value, fromBase);
			return Convert.ToString(num, toBase);
		}

		public static uint ConvertToUInt32(byte[] value)
		{
			uint num = 0u;
			foreach (byte b in value)
			{
				num = (uint)((int)num + (b & 0x7F));
				if ((b & 0x80) == 0)
				{
					break;
				}
				num <<= 7;
			}
			return num;
		}
	}
}
