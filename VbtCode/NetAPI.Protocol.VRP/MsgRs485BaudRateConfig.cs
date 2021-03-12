using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgRs485BaudRateConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public BaudRate RS485BaudRate
			{
				get
				{
					BaudRate result = BaudRate.R115200;
					if (buff.Length >= 2)
					{
						result = (BaudRate)buff[1];
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

		public MsgRs485BaudRateConfig()
		{
			msgBody = new byte[1]
			{
				1
			};
		}

		public MsgRs485BaudRateConfig(BaudRate baudRate)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)baudRate;
		}
	}
}
