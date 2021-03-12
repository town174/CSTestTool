using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgAlarmInfo_AccessControl : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			private RxdTagData tagData;

			public RxdTagData TagData => tagData;

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				tagData = new RxdTagData();
				if (buff.Length > 1)
				{
					tagData.EPC = new byte[buff.Length - 1];
					Array.Copy(buff, 1, tagData.EPC, 0, tagData.EPC.Length);
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

		public MsgAlarmInfo_AccessControl()
		{
			isReturn = false;
			msgBody = new byte[3];
			msgBody[0] = 1;
		}

		public MsgAlarmInfo_AccessControl(ushort tagNum)
		{
			isReturn = false;
			msgBody = new byte[3];
			msgBody[0] = 1;
			msgBody[1] = (byte)(tagNum >> 8);
			msgBody[2] = (byte)(tagNum & 0xFF);
		}
	}
}
