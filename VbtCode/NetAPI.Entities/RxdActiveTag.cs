using NetAPI.Protocol.VRP;
using System;

namespace NetAPI.Entities
{
	public class RxdActiveTag
	{
		private byte port;

		private ActiveTagData_V17 tagData;

		private DateTime utc = DateTime.MinValue;

		private byte rssi;

		private InOutState peopleInOutState = InOutState.Unknown;

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

		public ActiveTagData_V17 TagData
		{
			get
			{
				return tagData;
			}
			set
			{
				tagData = value;
			}
		}

		public DateTime UTC
		{
			get
			{
				return utc;
			}
			set
			{
				utc = value;
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

		public InOutState PeopleInOutState
		{
			get
			{
				return peopleInOutState;
			}
			set
			{
				peopleInOutState = value;
			}
		}

		public void SetData(byte[] tBuff)
		{
			TagData = new ActiveTagData_V17();
			int num = TagData.SetData(tBuff);
			if (tBuff.Length >= num + 1)
			{
				RSSI = tBuff[num];
				num++;
			}
			if (tBuff.Length >= num + 8)
			{
				byte[] array = new byte[8];
				Array.Copy(tBuff, num, array, 0, 8);
				UTC = Common.setUtcTime(array);
				num += 8;
			}
			if (tBuff.Length >= num + 1)
			{
				peopleInOutState = (InOutState)tBuff[num];
				num++;
			}
		}
	}
}
