using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgPowerConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public AntennaPower[] Powers
			{
				get
				{
					AntennaPower[] array = null;
					if (buff.Length >= 2)
					{
						array = new AntennaPower[buff.Length - 1];
						for (int i = 1; i < buff.Length; i++)
						{
							array[i - 1] = new AntennaPower();
							array[i - 1].AntennaNO = (byte)i;
							array[i - 1].PowerValue = buff[i];
						}
					}
					return array;
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

		public MsgPowerConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgPowerConfig(byte[] powers)
		{
			msgBody = new byte[1 + powers.Length];
			msgBody[0] = 0;
			for (int i = 0; i < powers.Length; i++)
			{
				msgBody[i + 1] = powers[i];
			}
		}
	}
}
