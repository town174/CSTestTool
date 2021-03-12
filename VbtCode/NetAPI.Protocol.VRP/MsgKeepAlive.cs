using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgKeepAlive : AbstractReaderMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public bool IsEnable
			{
				get
				{
					bool result = false;
					if (buff.Length >= 2)
					{
						result = (buff[1] > 0);
					}
					return result;
				}
			}

			public ushort Interval
			{
				get
				{
					ushort result = 0;
					if (buff.Length >= 4)
					{
						result = (ushort)((buff[2] << 8) & buff[3]);
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
	}
}
