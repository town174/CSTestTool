using System;

namespace NetAPI.Protocol.VRP
{
	public class Checksum
	{
		public static bool CheckCRC(byte[] msgBuff)
		{
			if (msgBuff != null && msgBuff.Length >= 7)
			{
				int num = msgBuff.Length - 2;
				int sourceIndex = 0;
				byte[] array = new byte[num];
				Array.Copy(msgBuff, sourceIndex, array, 0, num);
				byte[] array2 = new byte[2];
				Array.Copy(msgBuff, msgBuff.Length - 2, array2, 0, 2);
				byte[] source = CalculateCRC(array, array.Length);
				if (EqualsbyteArray(source, array2))
				{
					return true;
				}
			}
			return false;
		}

		public static byte[] CalculateCRC(byte[] buff, int len)
		{
			if (len > buff.Length)
			{
				return null;
			}
			uint num = 0u;
			int num2 = 0;
			while (len-- > 0)
			{
				for (byte b = 128; b != 0; b = (byte)(b >> 1))
				{
					num = (((num & 0x8000) == 0) ? (num << 1) : ((num << 1) ^ 0x8005));
					if ((buff[num2] & b) != 0)
					{
						num ^= 0x8005;
					}
				}
				num2++;
			}
			byte[] bytes = BitConverter.GetBytes((ushort)num);
			Array.Reverse(bytes);
			return bytes;
		}

		public static bool EqualsbyteArray(byte[] source, byte[] target)
		{
			if (BitConverter.ToString(source).Equals(BitConverter.ToString(target)))
			{
				return true;
			}
			return false;
		}
	}
}
