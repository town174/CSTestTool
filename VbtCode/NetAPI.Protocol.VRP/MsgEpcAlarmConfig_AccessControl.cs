using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgEpcAlarmConfig_AccessControl : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public EpcAlarmConfig_AccessControl EpcAlarmConfig
			{
				get
				{
					EpcAlarmConfig_AccessControl epcAlarmConfig_AccessControl = null;
					if (buff.Length >= 2)
					{
						epcAlarmConfig_AccessControl = new EpcAlarmConfig_AccessControl();
						epcAlarmConfig_AccessControl.NO = (byte)(buff[1] & 0x7F);
						epcAlarmConfig_AccessControl.IsEnable = ((buff[1] & 0x80) > 0);
						if (buff.Length > 2)
						{
							int num = (buff.Length - 2) / 2;
							epcAlarmConfig_AccessControl.Epc = new byte[num];
							Array.Copy(buff, 2, epcAlarmConfig_AccessControl.Epc, 0, num);
							epcAlarmConfig_AccessControl.Mask = new byte[num];
							Array.Copy(buff, 2 + num, epcAlarmConfig_AccessControl.Mask, 0, num);
						}
					}
					return epcAlarmConfig_AccessControl;
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

		public MsgEpcAlarmConfig_AccessControl(byte groupNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = groupNO;
		}

		public MsgEpcAlarmConfig_AccessControl(EpcAlarmConfig_AccessControl param)
		{
			int num = 2;
			if (param.Epc != null)
			{
				num += param.Epc.Length;
			}
			if (param.Mask != null)
			{
				num += param.Mask.Length;
			}
			msgBody = new byte[num];
			msgBody[0] = 0;
			msgBody[1] = (byte)(param.NO + (param.IsEnable ? 128 : 0));
			if (param.Epc != null)
			{
				Array.Copy(param.Epc, 0, msgBody, 2, param.Epc.Length);
			}
			if (param.Mask != null)
			{
				Array.Copy(param.Mask, 0, msgBody, num - param.Mask.Length, param.Mask.Length);
			}
		}
	}
}
