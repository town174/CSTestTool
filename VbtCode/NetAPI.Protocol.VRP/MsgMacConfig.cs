using NetAPI.Core;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgMacConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte[] MAC
			{
				get
				{
					byte[] array = new byte[6];
					if (buff.Length >= 7)
					{
						Array.Copy(buff, 1, array, 0, 6);
					}
					return array;
				}
			}

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
			}

			public string getstringMAC()
			{
				string text = "";
				for (int i = 1; i < 7; i++)
				{
					text = text + buff[i].ToString("X2").PadLeft(2, '0') + ":";
				}
				if (!string.IsNullOrEmpty(text))
				{
					text = text.Trim(':');
				}
				return text;
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

		public MsgMacConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgMacConfig(byte[] mac)
		{
			msgBody = new byte[7];
			msgBody[0] = 0;
			Array.Copy(mac, 0, msgBody, 1, mac.Length);
		}
	}
}
