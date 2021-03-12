using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgCommunicationModeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte Mode
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

		public MsgCommunicationModeConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgCommunicationModeConfig(byte mode)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = mode;
		}
	}
}
