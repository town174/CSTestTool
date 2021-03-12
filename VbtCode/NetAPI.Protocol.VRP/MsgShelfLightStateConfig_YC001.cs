using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgShelfLightStateConfig_YC001 : AbstractHostMessage
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

			public ShelfLight[] LightState
			{
				get
				{
					ShelfLight[] array = null;
					if (buff.Length >= 4)
					{
						array = new ShelfLight[12];
						int num = 0;
						for (int i = 0; i < 12; i++)
						{
							array[i] = new ShelfLight();
							array[i].ShelfNO = (byte)(i + 1);
							num = i / 8;
							array[i].State = (((buff[3 - num] & (1 << i - num * 8)) > 0) ? NetAPI.Entities.LightState.On : NetAPI.Entities.LightState.Off);
						}
					}
					return array;
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

		public MsgShelfLightStateConfig_YC001(byte ctrlBoardNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = ctrlBoardNO;
		}

		public MsgShelfLightStateConfig_YC001(byte ctrlBoardNO, ShelfLight[] shelfLights)
		{
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = ctrlBoardNO;
			foreach (ShelfLight shelfLight in shelfLights)
			{
				if (shelfLight.State != 0 && shelfLight.ShelfNO <= 12)
				{
					msgBody[3 - (shelfLight.ShelfNO - 1) / 8] += (byte)(1 << shelfLight.ShelfNO - 1 - (shelfLight.ShelfNO - 1) / 8 * 8);
				}
			}
		}
	}
}
