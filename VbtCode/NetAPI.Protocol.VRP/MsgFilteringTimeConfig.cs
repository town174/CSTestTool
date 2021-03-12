using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgFilteringTimeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public ushort Time
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

		public MsgFilteringTimeConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgFilteringTimeConfig(ushort time)
		{
			msgBody = new byte[3];
			msgBody[0] = 0;
			msgBody[1] = (byte)(time >> 8);
			msgBody[2] = (byte)(time & 0xFF);
		}
	}
}
