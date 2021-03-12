using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgSlaveModeCacheCountQuery : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public int NumberOfCaches
			{
				get
				{
					int result = 0;
					if (buff.Length >= 5)
					{
						result = (buff[1] << 24) + (buff[2] << 16) + (buff[3] << 8) + buff[4];
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

		public MsgSlaveModeCacheCountQuery()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}
	}
}
