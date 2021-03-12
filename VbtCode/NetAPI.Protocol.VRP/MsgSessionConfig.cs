using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgSessionConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public SessionInfo SessionParam
			{
				get
				{
					SessionInfo sessionInfo = new SessionInfo();
					if (buff.Length >= 3)
					{
						sessionInfo.Session = (Session)buff[1];
						sessionInfo.Flag = (Flag)buff[2];
					}
					return sessionInfo;
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

		public MsgSessionConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgSessionConfig(SessionInfo param)
		{
			msgBody = new byte[3];
			msgBody[0] = 0;
			msgBody[1] = (byte)param.Session;
			msgBody[2] = (byte)param.Flag;
		}
	}
}
