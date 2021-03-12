using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgPowerOn : AbstractHostMessage
	{
		public MsgPowerOn(AntennaStatus[] antennaStatusAry)
		{
			msgBody = new byte[9];
			msgBody[0] = 1;
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
					msgBody[num2 + 1] |= b;
				}
			}
		}
	}
}
