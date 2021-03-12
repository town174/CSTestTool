using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgFilteringConfig_YC002 : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte DecisionDeviceNO
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

			public byte FilteringTime
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

		public MsgFilteringConfig_YC002(byte decisionNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = decisionNO;
		}

		public MsgFilteringConfig_YC002(byte decisionNO, byte filteringTime)
		{
			msgBody = new byte[3];
			msgBody[0] = 0;
			msgBody[1] = decisionNO;
			msgBody[2] = filteringTime;
		}
	}
}
