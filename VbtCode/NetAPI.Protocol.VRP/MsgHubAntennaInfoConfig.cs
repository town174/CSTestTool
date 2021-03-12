using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgHubAntennaInfoConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte HubAntennaCount
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

			public byte LayerAntennaCount
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 3)
					{
						result = buff[2];
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

		public MsgHubAntennaInfoConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgHubAntennaInfoConfig(byte hubAntennaCount, byte layerAntennaCount)
		{
			msgBody = new byte[3];
			msgBody[0] = 0;
			msgBody[1] = hubAntennaCount;
			msgBody[2] = layerAntennaCount;
		}
	}
}
