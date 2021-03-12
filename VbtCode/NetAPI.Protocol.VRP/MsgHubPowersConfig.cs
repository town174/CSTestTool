using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgHubPowersConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte AntennaNO
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

			public byte LayerNO
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

			public byte Power
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 4)
					{
						result = buff[3];
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

		public MsgHubPowersConfig()
		{
		}

		public MsgHubPowersConfig(byte antennaNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = antennaNO;
		}

		public MsgHubPowersConfig(byte antennaNO, byte layerNO)
		{
			msgBody = new byte[3];
			msgBody[0] = 1;
			msgBody[1] = antennaNO;
			msgBody[2] = layerNO;
		}

		public MsgHubPowersConfig(byte antennaNO, byte layerNO, byte power)
		{
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = antennaNO;
			msgBody[2] = layerNO;
			msgBody[3] = power;
		}
	}
}
