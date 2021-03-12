using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgMasterAndSlaveModeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public ReaderWorkMode WorkMode
			{
				get
				{
					byte result = 1;
					if (buff.Length >= 2)
					{
						result = buff[1];
					}
					return (ReaderWorkMode)result;
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

		public MsgMasterAndSlaveModeConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgMasterAndSlaveModeConfig(ReaderWorkMode readerWorkMode)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)readerWorkMode;
		}
	}
}
