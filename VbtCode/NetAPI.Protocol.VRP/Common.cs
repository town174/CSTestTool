using System;

namespace NetAPI.Protocol.VRP
{
	internal class Common
	{
		internal static uint GetMessageID(byte[] recvMsg)
		{
			uint num = uint.MaxValue;
			if (IsRS485(recvMsg))
			{
				return (uint)((recvMsg[4] << 16) + (recvMsg[5] << 8) + recvMsg[1]);
			}
			return (uint)((recvMsg[3] << 8) + recvMsg[4]);
		}

		internal static bool IsRS485(byte[] recvMsg)
		{
			if (recvMsg == null || recvMsg.Length == 0)
			{
				return false;
			}
			return (recvMsg[0] & 0x20) > 0;
		}

		internal static bool Validate(byte[] msgbuff)
		{
			if (msgbuff.Length < 7)
			{
				return false;
			}
			int num = 0;
			num = ((!IsRS485(msgbuff)) ? ((msgbuff[1] << 8) + msgbuff[2] + 5) : ((msgbuff[2] << 8) + msgbuff[3] + 6));
			if (msgbuff.Length != num)
			{
				return false;
			}
			if (!Checksum.CheckCRC(msgbuff))
			{
				return false;
			}
			return true;
		}

		internal static byte[] formatData(byte[] buff)
		{
			int num = buff.Length;
			foreach (byte b in buff)
			{
				if (b == 85 || b == 86)
				{
					num++;
				}
			}
			byte[] array = new byte[num + 1];
			array[0] = 85;
			if (num > buff.Length)
			{
				int num2 = 1;
				foreach (byte b2 in buff)
				{
					switch (b2)
					{
					case 85:
						array[num2] = 86;
						num2++;
						array[num2] = 86;
						break;
					case 86:
						array[num2] = 86;
						num2++;
						array[num2] = 87;
						break;
					default:
						array[num2] = b2;
						break;
					}
					num2++;
				}
			}
			else
			{
				Array.Copy(buff, 0, array, 1, num);
			}
			return array;
		}

		internal static byte[] desFormatData(byte[] buff)
		{
			int num = 0;
			for (int i = 0; i < buff.Length - 1; i++)
			{
				if (buff[i] == 86 && (buff[i + 1] == 86 || buff[i + 1] == 87))
				{
					num++;
					i++;
				}
			}
			if (num == 0)
			{
				return buff;
			}
			byte[] array = new byte[buff.Length - num];
			int num2 = 0;
			for (int j = 0; j < buff.Length - 1; j++)
			{
				if (buff[j] == 86)
				{
					if (buff[j + 1] == 86)
					{
						array[num2] = 85;
					}
					else if (buff[j + 1] == 87)
					{
						array[num2] = 86;
					}
					j++;
				}
				else
				{
					array[num2] = buff[j];
				}
				num2++;
			}
			if (num2 < array.Length)
			{
				array[num2] = buff[buff.Length - 1];
			}
			return array;
		}

		internal static DateTime setUtcTime(byte[] utc)
		{
			try
			{
				double value = (double)((utc[0] << 24) + (utc[1] << 16) + (utc[2] << 8) + utc[3]);
				DateTime result = DateTime.Parse("1970-01-01").AddSeconds(value);
				uint num = (uint)((utc[4] << 24) + (utc[5] << 16) + (utc[6] << 8) + utc[7]) / 1000u;
				if (num < 1000)
				{
					result = result.AddMilliseconds((double)num);
				}
				return result;
			}
			catch
			{
				return DateTime.Parse("1970-01-01");
			}
		}
	}
}
