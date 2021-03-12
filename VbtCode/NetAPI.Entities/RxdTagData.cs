using System;

namespace NetAPI.Entities
{
	public class RxdTagData
	{
		private byte antenna;

		private byte hub;

		private byte port;

		private byte[] epc;

		private byte[] tid;

		private byte[] userData;

		private byte[] reserved;

		private byte rssi;

		public byte Antenna
		{
			get
			{
				return antenna;
			}
			set
			{
				antenna = value;
			}
		}

		public byte Hub
		{
			get
			{
				return hub;
			}
			set
			{
				hub = value;
			}
		}

		public byte Port
		{
			get
			{
				return port;
			}
			set
			{
				port = value;
			}
		}

		public byte[] EPC
		{
			get
			{
				return epc;
			}
			set
			{
				epc = value;
			}
		}

		public byte[] TID
		{
			get
			{
				return tid;
			}
			set
			{
				tid = value;
			}
		}

		public byte[] User
		{
			get
			{
				return userData;
			}
			set
			{
				userData = value;
			}
		}

		public byte[] Reserved
		{
			get
			{
				return reserved;
			}
			set
			{
				reserved = value;
			}
		}

		public byte RSSI
		{
			get
			{
				return rssi;
			}
			set
			{
				rssi = value;
			}
		}

		public double GetRSSI
		{
			get
			{
				int num = rssi & 7;
				int num2 = rssi >> 3;
				return 20.0 * Math.Log10(Math.Pow(2.0, (double)num2) * (1.0 + (double)num / Math.Pow(2.0, 3.0))) - 110.0;
			}
		}
	}
}
