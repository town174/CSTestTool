using NetAPI.Core;
using System;
using System.Collections.Generic;

namespace NetAPI.Protocol.VRP
{
	internal sealed class Decode : IProcess
	{
		private byte[] recvbuff;

		private volatile bool is56 = false;

		private int buffIndex = 0;

		void IProcess.Parse(byte[] buff, out List<byte[]> msgAry)
		{
			if (buffIndex == 2147483647)
			{
				buffIndex = 0;
			}
			Log.Debug("RXD:[" + Util.ConvertbyteArrayToHexstring(buff) + "]--" + buffIndex++.ToString());
			msgAry = new List<byte[]>();
			if (buff != null && buff.Length != 0)
			{
				byte[] array = null;
				if (recvbuff == null)
				{
					recvbuff = buff;
				}
				else
				{
					byte[] destinationArray = new byte[buff.Length + recvbuff.Length];
					Array.Copy(recvbuff, 0, destinationArray, 0, recvbuff.Length);
					Array.Copy(buff, 0, destinationArray, recvbuff.Length, buff.Length);
					recvbuff = destinationArray;
				}
				if (recvbuff.Length >= 8)
				{
					array = recvbuff;
					recvbuff = null;
					int num = -1;
					int num2 = 0;
					while (num2 < array.Length)
					{
						if (array[num2] == 85)
						{
							if (num == -1)
							{
								if (num2 > 0)
								{
									byte[] array2 = new byte[num2];
									Array.Copy(array, 0, array2, 0, array2.Length);
									Log.Info("丢失的数据1：" + Util.ConvertbyteArrayToHexstring(array2));
								}
								num = num2;
								num2++;
								continue;
							}
							byte[] msgBuff = new byte[num2 - num];
							Array.Copy(array, num, msgBuff, 0, msgBuff.Length);
							byte[] array3 = msgBuffCheck(ref msgBuff);
							if (array3 != null)
							{
								msgAry.Add(array3);
								if (msgBuff != null)
								{
									Log.Info("丢失的数据4：" + Util.ConvertbyteArrayToHexstring(msgBuff));
								}
							}
							else
							{
								byte[] array4 = new byte[num2 - num];
								Array.Copy(array, num, array4, 0, array4.Length);
								Log.Info("丢失的数据2：" + Util.ConvertbyteArrayToHexstring(array4));
							}
							num = num2;
						}
						num2++;
					}
					if (num == -1)
					{
						Log.Info("丢失的数据3：" + Util.ConvertbyteArrayToHexstring(array));
						recvbuff = null;
					}
					else if (array.Length - 7 > num)
					{
						byte[] msgBuff2 = new byte[array.Length - num];
						Array.Copy(array, num, msgBuff2, 0, msgBuff2.Length);
						byte[] array5 = msgBuffCheck(ref msgBuff2);
						if (array5 != null)
						{
							msgAry.Add(array5);
							if (msgBuff2 != null)
							{
								Log.Info("丢失的数据5：" + Util.ConvertbyteArrayToHexstring(msgBuff2));
							}
							recvbuff = null;
						}
						else
						{
							recvbuff = msgBuff2;
						}
					}
					else
					{
						recvbuff = new byte[array.Length - num];
						Array.Copy(array, num, recvbuff, 0, recvbuff.Length);
					}
				}
			}
		}

		private byte[] msgBuffCheck(ref byte[] msgBuff)
		{
			if (msgBuff != null && msgBuff.Length >= 8)
			{
				if (msgBuff[msgBuff.Length - 1] == 86 && msgBuff[msgBuff.Length - 2] != 86)
				{
					return null;
				}
				byte[] array = Common.desFormatData(msgBuff);
				byte[] array2 = new byte[array.Length - 1];
				Array.Copy(array, 1, array2, 0, array2.Length);
				bool flag = (array2[0] & 0x20) > 0;
				int num = 0;
				num = ((!flag) ? ((array2[1] << 8) + array2[2] + 5) : ((array2[2] << 8) + array2[3] + 6));
				if (array2.Length == num)
				{
					if (Checksum.CheckCRC(array2))
					{
						msgBuff = null;
						return array2;
					}
				}
				else if (array2.Length > num)
				{
					byte[] array3 = new byte[num];
					Array.Copy(array2, 0, array3, 0, num);
					if (Checksum.CheckCRC(array3))
					{
						byte[] array4 = new byte[array2.Length - num];
						Array.Copy(array2, num, array4, 0, array4.Length);
						byte[] array5 = Common.formatData(array4);
						msgBuff = new byte[array5.Length - 1];
						Array.Copy(array5, 1, msgBuff, 0, msgBuff.Length);
						return array3;
					}
				}
			}
			return null;
		}

		public IReaderMessage ParseMessageNotification(byte[] recvMsg)
		{
			IReaderMessage readerMessage = null;
			ushort messageType = GetMessageType(recvMsg);
			if (MessageType.msgType.ContainsKey(messageType))
			{
				Type type = Type.GetType("NetAPI.Protocol.VRP." + MessageType.msgType[messageType], throwOnError: true);
				readerMessage = (IReaderMessage)Activator.CreateInstance(type, null);
				readerMessage.ReceivedData = recvMsg;
			}
			return readerMessage;
		}

		public ushort GetMessageType(byte[] msg)
		{
			ushort result = 0;
			if (msg.Length >= 7)
			{
				result = (((msg[1] & 0x20) <= 0) ? ((ushort)((msg[3] << 8) + msg[4])) : ((ushort)((msg[4] << 8) + msg[5])));
			}
			return result;
		}

		public uint GetMessageID(byte[] msg)
		{
			return Common.GetMessageID(msg);
		}
	}
}
