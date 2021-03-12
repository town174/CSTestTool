using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgPortDwellTimeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public ushort UseTime
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

			public ushort MinFreeTime
			{
				get
				{
					ushort result = 0;
					if (buff.Length >= 6)
					{
						result = (ushort)((buff[4] << 8) + buff[5]);
					}
					return result;
				}
			}

			public ushort MaxFreeTime
			{
				get
				{
					ushort result = 0;
					if (buff.Length >= 8)
					{
						result = (ushort)((buff[6] << 8) + buff[7]);
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

		public MsgPortDwellTimeConfig(byte antNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = antNO;
		}

		public MsgPortDwellTimeConfig(ushort useTime, ushort minFreeTime, ushort maxFreeTime)
		{
			msgBody = new byte[8];
			msgBody[0] = 0;
			msgBody[1] = 0;
			msgBody[2] = (byte)(useTime >> 8);
			msgBody[3] = (byte)useTime;
			msgBody[4] = (byte)(minFreeTime >> 8);
			msgBody[5] = (byte)minFreeTime;
			msgBody[6] = (byte)(maxFreeTime >> 8);
			msgBody[7] = (byte)maxFreeTime;
		}
	}
}
