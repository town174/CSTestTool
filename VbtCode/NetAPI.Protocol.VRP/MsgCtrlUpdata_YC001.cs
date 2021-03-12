using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgCtrlUpdata_YC001 : AbstractReaderMessage
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
						result = buff[0];
					}
					return result;
				}
			}

			public IrTrigger[] IrStates
			{
				get
				{
					IrTrigger[] array = null;
					if (buff.Length >= 2)
					{
						array = new IrTrigger[12];
						int num = 0;
						for (int i = 0; i < 12; i++)
						{
							num = i / 8;
							array[i] = new IrTrigger();
							array[i].IrNO = (byte)(i + 1);
							array[i].IsTrigger = ((buff[2 - num] & (1 << i - num * 8)) > 0);
						}
					}
					return array;
				}
			}

			public SenssorTrigger[] SenssorStates
			{
				get
				{
					SenssorTrigger[] array = null;
					if (buff.Length >= 4)
					{
						array = new SenssorTrigger[2]
						{
							new SenssorTrigger(),
							null
						};
						array[0].SenssorNO = 1;
						array[0].IsTrigger = ((buff[3] & 1) > 0);
						array[1] = new SenssorTrigger();
						array[1].SenssorNO = 2;
						array[1].IsTrigger = ((buff[3] & 2) > 0);
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
	}
}
