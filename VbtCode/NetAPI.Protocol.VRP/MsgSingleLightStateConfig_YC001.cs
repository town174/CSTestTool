using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgSingleLightStateConfig_YC001 : AbstractHostMessage
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

			public byte LightNO
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 3)
					{
						result = buff[2];
					}
					return result;
				}
			}

			public LightState LightState
			{
				get
				{
					LightState result = LightState.Off;
					if (buff.Length >= 4)
					{
						result = (LightState)buff[3];
					}
					return result;
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

		public MsgSingleLightStateConfig_YC001(byte ctrlBoardNO, byte lightNO)
		{
			msgBody = new byte[3];
			msgBody[0] = 1;
			msgBody[1] = ctrlBoardNO;
			msgBody[2] = lightNO;
		}

		public MsgSingleLightStateConfig_YC001(byte ctrlBoardNO, byte lightNO, LightState lightState)
		{
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = ctrlBoardNO;
			msgBody[2] = lightNO;
			msgBody[3] = (byte)lightState;
		}
	}
}
