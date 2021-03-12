using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveLedEnableConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public bool IsEnable
			{
				get
				{
					bool result = false;
					if (buff.Length >= 2)
					{
						result = ((buff[1] == 1) ? true : false);
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

		public MsgActiveLedEnableConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgActiveLedEnableConfig(bool isEnable)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)(isEnable ? 1 : 0);
		}
	}
}
