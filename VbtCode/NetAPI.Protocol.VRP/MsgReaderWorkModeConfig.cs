using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgReaderWorkModeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public WorkMode ReaderWorkMode
			{
				get
				{
					WorkMode result = WorkMode.NormalMode;
					if (buff.Length >= 2)
					{
						result = (WorkMode)buff[1];
					}
					return result;
				}
			}

			public TagMB TagMemoryBank
			{
				get
				{
					TagMB result = TagMB.EPC;
					if (buff.Length >= 3)
					{
						result = (TagMB)buff[2];
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

		public MsgReaderWorkModeConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgReaderWorkModeConfig(WorkMode readerWorkMode, TagMB tagMB)
		{
			int num = 2;
			if (readerWorkMode == WorkMode.AutoScanMode)
			{
				num = 3;
			}
			msgBody = new byte[num];
			msgBody[0] = 0;
			msgBody[1] = (byte)readerWorkMode;
			if (num == 3)
			{
				msgBody[2] = (byte)tagMB;
			}
		}
	}
}
