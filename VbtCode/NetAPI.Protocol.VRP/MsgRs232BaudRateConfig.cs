using NetAPI.Core;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgRs232BaudRateConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public BaudRate RS232BaudRate
			{
				get
				{
					BaudRate result = BaudRate.R115200;
					if (buff.Length >= 2)
					{
						result = (BaudRate)buff[1];
					}
					return result;
				}
			}

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
			}
		}

		private BaudRate baudRate;

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

		public MsgRs232BaudRateConfig()
		{
			msgBody = new byte[1]
			{
				1
			};
			base.OnExecuting += MsgRs232BaudRateConfig_OnExecuting1;
		}

		private void MsgRs232BaudRateConfig_OnExecuting1(object sender, EventArgs e)
		{
			base.OnExecuting -= MsgRs232BaudRateConfig_OnExecuting1;
			Reader reader = (Reader)sender;
			msgType = 770;
			msgBody = new byte[1]
			{
				1
			};
			if (!reader.Send(this))
			{
				msgType = 538;
			}
		}

		public MsgRs232BaudRateConfig(BaudRate baudRate)
		{
			this.baudRate = baudRate;
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)baudRate;
			base.OnExecuting += MsgRs232BaudRateConfig_OnExecuting;
		}

		private void MsgRs232BaudRateConfig_OnExecuting(object sender, EventArgs e)
		{
			base.OnExecuting -= MsgRs232BaudRateConfig_OnExecuting;
			Reader reader = (Reader)sender;
			msgType = 770;
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)baudRate;
			if (!reader.Send(this))
			{
				msgType = 538;
			}
		}

		~MsgRs232BaudRateConfig()
		{
			base.OnExecuting -= MsgRs232BaudRateConfig_OnExecuting;
			base.OnExecuting -= MsgRs232BaudRateConfig_OnExecuting1;
		}
	}
}
