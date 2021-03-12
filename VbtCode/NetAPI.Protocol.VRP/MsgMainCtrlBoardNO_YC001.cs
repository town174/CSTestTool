using NetAPI.Core;

namespace NetAPI.Protocol.VRP
{
	public class MsgMainCtrlBoardNO_YC001 : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte MainCtrlBoardNO
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

		public MsgMainCtrlBoardNO_YC001()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgMainCtrlBoardNO_YC001(byte mainCtrlBoardNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = mainCtrlBoardNO;
		}
	}
}
