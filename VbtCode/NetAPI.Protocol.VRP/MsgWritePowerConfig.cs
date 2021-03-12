using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgWritePowerConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte Power
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 2)
					{
						result = buff[1];
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

		public MsgWritePowerConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgWritePowerConfig(byte power)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = power;
		}
	}
}
