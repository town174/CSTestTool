using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgScanModeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public ScanMode ReaderScanMode
			{
				get
				{
					ScanMode result = ScanMode.MutiTag;
					if (buff.Length >= 2)
					{
						result = (ScanMode)buff[1];
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

		public MsgScanModeConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgScanModeConfig(ScanMode scanMode)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)scanMode;
		}
	}
}
