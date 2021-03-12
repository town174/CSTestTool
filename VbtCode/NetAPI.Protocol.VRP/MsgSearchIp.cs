using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgSearchIp : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte[] MAC
			{
				get
				{
					byte[] array = null;
					if (buff.Length >= 7)
					{
						array = new byte[6];
						Array.Copy(buff, 1, array, 0, 6);
					}
					return array;
				}
			}

			public string MACStr
			{
				get
				{
					string text = "";
					byte[] array = null;
					if (buff.Length >= 7)
					{
						array = new byte[6];
						Array.Copy(buff, 1, array, 0, 6);
						byte[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							byte b = array2[i];
							text = text + b.ToString("X2") + ":";
						}
						text = text.Trim(':');
					}
					return text;
				}
			}

			public string IP
			{
				get;
			} = "";


			public string Subnet
			{
				get;
			} = "";


			public string Gateway
			{
				get;
			} = "";


			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				byte[] array = new byte[12];
				Array.Copy(buff, 7, array, 0, 12);
				IpParameter ipParameter = new IpParameter();
				ipParameter.setData(array);
				IP = ipParameter.IP;
				Subnet = ipParameter.Subnet;
				Gateway = ipParameter.Gateway;
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

		public MsgSearchIp()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
			isReturn = false;
		}
	}
}
