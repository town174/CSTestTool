using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveRfidParameterConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public ActiveRfidParameter RfidParameter
			{
				get
				{
					ActiveRfidParameter activeRfidParameter = null;
					if (buff.Length >= 11)
					{
						activeRfidParameter = new ActiveRfidParameter();
						activeRfidParameter.SetParam(buff);
					}
					return activeRfidParameter;
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

		public MsgActiveRfidParameterConfig(byte moduleID)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = moduleID;
		}

		public MsgActiveRfidParameterConfig(ActiveRfidParameter param)
		{
			byte[] parambytes = param.GetParambytes();
			msgBody = new byte[1 + parambytes.Length];
			msgBody[0] = 0;
			Array.Copy(parambytes, 0, msgBody, 1, parambytes.Length);
		}
	}
}
