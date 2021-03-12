using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveTagSleep : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte[] TagId
			{
				get
				{
					byte[] result = null;
					if (buff.Length >= 5)
					{
						result = buff;
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

		public MsgActiveTagSleep()
		{
			isReturn = false;
		}
	}
}
