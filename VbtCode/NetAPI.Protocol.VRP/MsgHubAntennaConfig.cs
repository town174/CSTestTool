using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgHubAntennaConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public HabAntennaStatus PortInfo
			{
				get
				{
					HabAntennaStatus habAntennaStatus = new HabAntennaStatus();
					if (buff.Length >= 8)
					{
						habAntennaStatus.HabNO = buff[1];
						byte[] array = new byte[6];
						Array.Copy(buff, 2, array, 0, 6);
						habAntennaStatus.SetAntennaStatus(array);
					}
					return habAntennaStatus;
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

		public MsgHubAntennaConfig(byte habNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = habNO;
		}

		public MsgHubAntennaConfig(HabAntennaStatus portInfo)
		{
			byte[] antennaStatusBuff = portInfo.GetAntennaStatusBuff();
			msgBody = new byte[2 + antennaStatusBuff.Length];
			msgBody[0] = 0;
			msgBody[1] = portInfo.HabNO;
			Array.Copy(antennaStatusBuff, 0, msgBody, 2, antennaStatusBuff.Length);
		}
	}
}
