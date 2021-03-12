using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveCommunicationModeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public CommunicationMode CommMode
			{
				get
				{
					CommunicationMode result = CommunicationMode.RS232;
					if (buff.Length >= 2)
					{
						result = (CommunicationMode)buff[1];
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

		public MsgActiveCommunicationModeConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgActiveCommunicationModeConfig(CommunicationMode commMode)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)commMode;
		}
	}
}
