using NetAPI.Core;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgKeepAliveConfig : AbstractHostMessage
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
						result = (buff[1] > 0);
					}
					return result;
				}
			}

			public ushort Interval
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

		private bool isEnable;

		private ushort interval;

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

		public MsgKeepAliveConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgKeepAliveConfig(bool isEnable, ushort interval)
		{
			this.isEnable = isEnable;
			this.interval = interval;
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = (byte)(isEnable ? 1 : 0);
			msgBody[2] = (byte)(interval >> 8);
			msgBody[3] = (byte)(interval & 0xFF);
			base.OnExecuted += MsgKeepAliveConfig_OnExecuted;
		}

		private void MsgKeepAliveConfig_OnExecuted(object sender, EventArgs e)
		{
			if (statusCode == MsgStatus.Success)
			{
				Reader reader = (Reader)sender;
				reader.isEnableKeepAlive = isEnable;
				reader.keepAliveInterval = interval;
			}
		}

		~MsgKeepAliveConfig()
		{
			base.OnExecuted -= MsgKeepAliveConfig_OnExecuted;
		}
	}
}
