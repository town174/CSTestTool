using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgWorkModeConfig_AccessControl : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public WorkMode_AccessControl WorkMode
			{
				get
				{
					WorkMode_AccessControl result = WorkMode_AccessControl.OffLine;
					if (buff.Length >= 2)
					{
						result = (WorkMode_AccessControl)buff[1];
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

		public MsgWorkModeConfig_AccessControl()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgWorkModeConfig_AccessControl(WorkMode_AccessControl mode)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)mode;
		}
	}
}
