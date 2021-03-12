using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgSingleLayerLightStateConfig_YC001 : AbstractHostMessage
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

			public byte LayerNO
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

			public LayerLightState LayerLightState
			{
				get
				{
					LayerLightState result = LayerLightState.Off;
					if (buff.Length >= 4)
					{
						result = (LayerLightState)buff[3];
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

		public MsgSingleLayerLightStateConfig_YC001(byte ctrlBoardNO, byte layerNO)
		{
			msgBody = new byte[3];
			msgBody[0] = 1;
			msgBody[1] = ctrlBoardNO;
			msgBody[2] = layerNO;
		}

		public MsgSingleLayerLightStateConfig_YC001(byte ctrlBoardNO, byte layerNO, LayerLightState layerLightState)
		{
			msgBody = new byte[4];
			msgBody[0] = 0;
			msgBody[1] = ctrlBoardNO;
			msgBody[2] = layerNO;
			msgBody[3] = (byte)layerLightState;
		}
	}
}
