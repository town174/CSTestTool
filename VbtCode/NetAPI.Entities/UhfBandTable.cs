namespace NetAPI.Entities
{
	public class UhfBandTable
	{
		public enum Name
		{
			CN_920_925,
			FCC_902_928,
			EU_865_868
		}

		private string[] list;

		private double f1;

		private double dec;

		public string[] List => list;

		public double F1 => f1;

		public double Dec => dec;

		public UhfBandTable(Name ftName)
		{
			switch (ftName)
			{
			case Name.CN_920_925:
				f1 = 920.625;
				dec = 0.25;
				list = new string[16];
				for (int j = 0; j < 16; j++)
				{
					list[j] = (f1 + (double)j * dec).ToString("000.000");
				}
				break;
			case Name.FCC_902_928:
				f1 = 902.75;
				dec = 0.5;
				list = new string[50];
				for (int k = 0; k < 50; k++)
				{
					list[k] = (f1 + (double)k * dec).ToString("000.00");
				}
				break;
			case Name.EU_865_868:
				f1 = 865.7;
				dec = 0.6;
				list = new string[4];
				for (int i = 0; i < 4; i++)
				{
					list[i] = (f1 + (double)i * dec).ToString("000.0");
				}
				break;
			}
		}
	}
}
