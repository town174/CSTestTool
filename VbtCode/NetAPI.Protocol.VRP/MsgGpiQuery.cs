using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgGpiQuery : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public GpioLevelParameter GpiLevel
			{
				get
				{
					GpioLevelParameter gpioLevelParameter = new GpioLevelParameter();
					if (buff.Length >= 3)
					{
						gpioLevelParameter.PortNO = buff[1];
						gpioLevelParameter.Level = (GpioLevel)buff[2];
					}
					return gpioLevelParameter;
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

		public MsgGpiQuery()
		{
		}

		public MsgGpiQuery(byte portNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = portNO;
		}
	}
}
