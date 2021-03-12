using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgAlarmDataCacheNumQuery_AccessControl : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public ushort DataCacheNum
			{
				get
				{
					if (buff.Length >= 3)
					{
						return (ushort)((buff[1] << 8) + buff[2]);
					}
					return 0;
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

		public MsgAlarmDataCacheNumQuery_AccessControl()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}
	}
}
