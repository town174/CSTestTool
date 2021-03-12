using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgAirProtocolConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public AirProtocol Protocol
			{
				get
				{
					AirProtocol result = AirProtocol.ISO18000_6C;
					if (buff.Length >= 2)
					{
						result = (AirProtocol)buff[1];
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

		public MsgAirProtocolConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgAirProtocolConfig(AirProtocol param)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)param;
		}
	}
}
