using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgTcpModeConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public ReaderTcpMode Mode
			{
				get
				{
					ReaderTcpMode result = ReaderTcpMode.Server;
					if (buff.Length >= 2)
					{
						result = (ReaderTcpMode)buff[1];
					}
					return result;
				}
			}

			public ushort ServerPortNO
			{
				get
				{
					ushort result = 0;
					if (buff.Length >= 4)
					{
						result = (ushort)((buff[2] << 8) + buff[3]);
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

		public MsgTcpModeConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgTcpModeConfig(ReaderTcpMode mode, ushort serverPortNO)
		{
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = (byte)mode;
			msgBody[2] = (byte)(serverPortNO >> 8);
			msgBody[3] = (byte)(serverPortNO & 0xFF);
		}
	}
}
