using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgHubAntennaPortDwellTimeConfig : AbstractHostMessage
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

			public ushort Time
			{
				get
				{
					ushort result = 0;
					if (buff.Length >= 4)
					{
						result = (ushort)((buff[2] << 8) + buff[3]);
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

		public MsgHubAntennaPortDwellTimeConfig(byte antennaNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = antennaNO;
		}

		public MsgHubAntennaPortDwellTimeConfig(byte antennaNO, ushort time)
		{
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = antennaNO;
			msgBody[2] = (byte)(time >> 8);
			msgBody[3] = (byte)time;
		}
	}
}
