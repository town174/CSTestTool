using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgHubAntennaTimeResidentConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte HabNO
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

			public ushort Time
			{
				get
				{
					ushort result = 0;
					if (buff.Length >= 4)
					{
						result = (ushort)((buff[2] << 8) + buff[3]);
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

		public MsgHubAntennaTimeResidentConfig(byte habNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = habNO;
		}

		public MsgHubAntennaTimeResidentConfig(byte habNO, ushort time)
		{
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = habNO;
			msgBody[2] = (byte)(time >> 8);
			msgBody[3] = (byte)time;
		}
	}
}
