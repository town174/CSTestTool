using NetAPI.Core;
using System;
using System.Text;

namespace NetAPI.Protocol.VRP
{
	public class MsgWifiHotspotConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			private int ssidLen = 0;

			private int pwdLen = 0;

			private string ssid = "";

			private string pwd = "";

			public string SSID => ssid;

			public string Password => pwd;

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				if (buff.Length >= 2)
				{
					ssidLen = buff[1];
					if (buff.Length >= 2 + ssidLen)
					{
						ssid = Encoding.ASCII.GetString(buff, 2, ssidLen);
					}
				}
				if (buff.Length >= 3 + ssidLen)
				{
					pwdLen = buff[2 + ssidLen];
					if (buff.Length >= 3 + ssidLen + pwdLen)
					{
						pwd = Encoding.ASCII.GetString(buff, 3 + ssidLen, pwdLen);
					}
				}
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

		public MsgWifiHotspotConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgWifiHotspotConfig(string ssid, string pwd)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(ssid);
			byte[] array = new byte[0];
			if (!string.IsNullOrEmpty(pwd))
			{
				array = Encoding.ASCII.GetBytes(pwd);
			}
			msgBody = new byte[bytes.Length + array.Length + 3];
			msgBody[0] = 0;
			msgBody[1] = (byte)bytes.Length;
			Array.Copy(bytes, 0, msgBody, 2, bytes.Length);
			msgBody[2 + bytes.Length] = (byte)array.Length;
			Array.Copy(array, 0, msgBody, 3 + bytes.Length, array.Length);
		}
	}
}
