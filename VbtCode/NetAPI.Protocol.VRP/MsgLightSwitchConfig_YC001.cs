using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgLightSwitchConfig_YC001 : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte CtrlBoardNO
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 2)
					{
						result = buff[1];
					}
					return result;
				}
			}

			public LightSwitch LightSwitchState
			{
				get
				{
					LightSwitch lightSwitch = null;
					if (buff.Length >= 3)
					{
						lightSwitch = new LightSwitch();
						lightSwitch.SetParambyte(buff[2]);
					}
					return lightSwitch;
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

		public MsgLightSwitchConfig_YC001(byte ctrlBoardNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = ctrlBoardNO;
		}

		public MsgLightSwitchConfig_YC001(byte ctrlBoardNO, LightSwitch lightSwitch)
		{
			msgBody = new byte[3];
			msgBody[0] = 0;
			msgBody[1] = ctrlBoardNO;
			msgBody[2] = lightSwitch.GetParambyte();
		}
	}
}
