using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgKeepAliveConfig_YC001 : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte CtrlBoardNO
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 2)
					{
						result = buff[1];
					}
					return result;
				}
			}

			public bool IsEnable
			{
				get
				{
					bool result = false;
					if (buff.Length >= 3)
					{
						result = (buff[2] > 0);
					}
					return result;
				}
			}

			public ushort Interval
			{
				get
				{
					ushort result = 0;
					if (buff.Length >= 5)
					{
						result = (ushort)((buff[3] << 8) + buff[4]);
					}
					return result;
				}
			}

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
			}
		}

		public new ReceivedInfo ReceivedMessage
		{
			get
			{
				if (recvInfo == null)
				{
					if (msgBody == null)
					{
						return null;
					}
					recvInfo = new ReceivedInfo(msgBody);
				}
				return (ReceivedInfo)recvInfo;
			}
		}

		public MsgKeepAliveConfig_YC001(byte ctrlBoardNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = ctrlBoardNO;
		}

		public MsgKeepAliveConfig_YC001(byte ctrlBoardNO, bool isEnable, ushort interval)
		{
			msgBody = new byte[5];
			msgBody[0] = 0;
			msgBody[1] = ctrlBoardNO;
			msgBody[2] = (byte)(isEnable ? 1 : 0);
			msgBody[3] = (byte)(interval >> 8);
			msgBody[4] = (byte)(interval & 0xFF);
		}
	}
}
