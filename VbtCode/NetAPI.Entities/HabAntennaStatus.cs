namespace NetAPI.Entities
{
	public class HabAntennaStatus
	{
		public byte HabNO;

		public AntennaStatus[] Antennas;

		public void SetAntennaStatus(byte[] buff)
		{
			if (buff != null && buff.Length != 0)
			{
				int num = buff.Length;
				Antennas = new AntennaStatus[num * 8];
				for (int num2 = num - 1; num2 >= 0; num2--)
				{
					for (int i = 0; i < 8; i++)
					{
						Antennas[(num - 1 - num2) * 8 + i] = new AntennaStatus();
						Antennas[(num - 1 - num2) * 8 + i].AntennaNO = (byte)((num - 1 - num2) * 8 + i + 1);
						Antennas[(num - 1 - num2) * 8 + i].IsEnable = ((buff[num2] & (1 << i)) > 0);
					}
				}
			}
		}

		public byte[] GetAntennaStatusBuff()
		{
			byte[] array = new byte[6];
			if (Antennas != null && Antennas.Length != 0)
			{
				AntennaStatus[] antennas = Antennas;
				foreach (AntennaStatus antennaStatus in antennas)
				{
					if (antennaStatus.AntennaNO <= 48 && antennaStatus.IsEnable)
					{
						int num = (antennaStatus.AntennaNO - 1) / 8;
						int num2 = (antennaStatus.AntennaNO - 1) % 8;
						if ((array[5 - num] & (1 << num2)) == 0)
						{
							array[5 - num] += (byte)(1 << num2);
						}
					}
				}
			}
			return array;
		}
	}
}
