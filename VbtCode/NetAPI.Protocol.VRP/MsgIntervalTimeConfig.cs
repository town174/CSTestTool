using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgIntervalTimeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public ushort ReadTime
			{
				get
				{
					ushort result = 0;
					if (buff.Length >= 3)
					{
						result = (ushort)((buff[1] << 8) + buff[2]);
					}
					return result;
				}
			}

			public ushort StopTime
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

		public MsgIntervalTimeConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgIntervalTimeConfig(ushort readTime, ushort stopTime)
		{
			msgBody = new byte[5];
			msgBody[0] = 0;
			msgBody[1] = (byte)(readTime >> 8);
			msgBody[2] = (byte)(readTime & 0xFF);
			msgBody[3] = (byte)(stopTime >> 8);
			msgBody[4] = (byte)(stopTime & 0xFF);
		}
	}
}
