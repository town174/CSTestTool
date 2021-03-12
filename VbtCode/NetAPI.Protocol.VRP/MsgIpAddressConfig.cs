using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgIpAddressConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			private string ip = "";

			private string subnet = "";

			private string gateway = "";

			public string IP => ip;

			public string Subnet => subnet;

			public string Gateway => gateway;

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				byte[] array = new byte[12];
				Array.Copy(buff, 1, array, 0, 12);
				IpParameter ipParameter = new IpParameter();
				ipParameter.setData(array);
				ip = ipParameter.IP;
				subnet = ipParameter.Subnet;
				gateway = ipParameter.Gateway;
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

		public MsgIpAddressConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgIpAddressConfig(string ip, string subnet, string gate)
		{
			msgBody = new byte[13];
			msgBody[0] = 0;
			byte[] data = new IpParameter
			{
				IP = ip,
				Subnet = subnet,
				Gateway = gate
			}.getData();
			Array.Copy(data, 0, msgBody, 1, data.Length);
		}
	}
}
