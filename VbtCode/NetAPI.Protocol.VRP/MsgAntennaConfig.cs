using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgAntennaConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public AntennaStatus[] AntennaStatusAry
			{
				get
				{
					AntennaStatus[] array = null;
					if (buff.Length >= 9)
					{
						array = new AntennaStatus[64];
						int num = 0;
						for (int i = 0; i < 8; i++)
						{
							for (int j = 0; j < 8; j++)
							{
								array[num] = new AntennaStatus();
								array[num].AntennaNO = (byte)(num + 1);
								byte b = (byte)(1 << j);
								array[num].IsEnable = (((buff[8 - i] & b) > 0) ? true : false);
								num++;
							}
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

		public MsgAntennaConfig()
		{
			msgBody = new byte[1]
			{
				1
			};
		}

		public MsgAntennaConfig(AntennaStatus[] antennaStatusAry)
		{
			msgBody = new byte[9];
			msgBody[0] = 0;
			foreach (AntennaStatus antennaStatus in antennaStatusAry)
			{
				if (antennaStatus.IsEnable && antennaStatus.AntennaNO > 0 && antennaStatus.AntennaNO < 65)
				{
					int num = (int)antennaStatus.AntennaNO % 8;
					int num2 = (int)antennaStatus.AntennaNO / 8;
					if (num == 0)
					{
						num = 8;
						num2--;
					}
					num--;
					byte b = (byte)(1 << num);
					msgBody[8 - num2] |= b;
				}
			}
		}
	}
}
