using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgShelfLightModeConfig_YC001 : AbstractHostMessage
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

			public byte LightOnTime
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

			public byte LightOffTime
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 3)
					{
						result = buff[2];
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

		public MsgShelfLightModeConfig_YC001(byte ctrlBoardNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = ctrlBoardNO;
		}

		public MsgShelfLightModeConfig_YC001(byte ctrlBoardNO, byte lightOnTime, byte lightOffTime)
		{
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = ctrlBoardNO;
			msgBody[2] = lightOnTime;
			msgBody[3] = lightOffTime;
		}
	}
}
