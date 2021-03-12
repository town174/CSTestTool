using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgCtrlBoardConfig_YC001 : AbstractHostMessage
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

			public CtrlInfo_YC001 CtrlInfo
			{
				get
				{
					CtrlInfo_YC001 ctrlInfo_YC = null;
					if (buff.Length >= 4)
					{
						ctrlInfo_YC = new CtrlInfo_YC001();
						byte[] array = new byte[2];
						Array.Copy(buff, 2, array, 0, 2);
						ctrlInfo_YC.SetParambytes(array);
					}
					return ctrlInfo_YC;
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

		public MsgCtrlBoardConfig_YC001(byte ctrlBoardNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = ctrlBoardNO;
		}

		public MsgCtrlBoardConfig_YC001(byte ctrlBoardNO, CtrlInfo_YC001 info)
		{
			msgBody = new byte[15];
			msgBody[0] = 0;
			msgBody[1] = ctrlBoardNO;
			Array.Copy(info.GetParambytes(), 0, msgBody, 2, 13);
		}
	}
}
