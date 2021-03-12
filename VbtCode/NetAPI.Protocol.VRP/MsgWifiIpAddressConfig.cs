using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgWifiIpAddressConfig : AbstractHostMessage
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

		private ushort port;

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

		public MsgWifiIpAddressConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgWifiIpAddressConfig(string ip, string subnet, string gate)
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

		public MsgWifiIpAddressConfig(string ip, ushort port)
		{
			msgBody = new byte[13];
			msgBody[0] = 0;
			byte[] data = new IpParameter
			{
				IP = ip,
				Subnet = "0.0.0.0",
				Gateway = "0.0.0.0"
			}.getData();
			Array.Copy(data, 0, msgBody, 1, data.Length);
			this.port = port;
			base.OnExecuting += MsgWifiIpAddressConfig_OnExecuting;
		}

		private void MsgWifiIpAddressConfig_OnExecuting(object sender, EventArgs e)
		{
			Reader reader = (Reader)sender;
			MsgWifiModeConfig msgWifiModeConfig = new MsgWifiModeConfig(ReaderTcpMode.Client, port);
			if (!reader.Send(msgWifiModeConfig))
			{
				statusCode = msgWifiModeConfig.Status;
				errInfo = msgWifiModeConfig.ErrorInfo;
				throw new Exception(msgWifiModeConfig.ErrorInfo.ErrMsg);
			}
		}

		~MsgWifiIpAddressConfig()
		{
			base.OnExecuting -= MsgWifiIpAddressConfig_OnExecuting;
		}
	}
}
