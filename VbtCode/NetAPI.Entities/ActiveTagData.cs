using System;

namespace NetAPI.Entities
{
	public class ActiveTagData
	{
		private byte category;

		private byte[] tagId;

		private byte checkCode;

		private string softwareVersion;

		private string hardwareVersion;

		private bool isG_Sensor;

		private bool isAntiDemolitionAlarm;

		private bool isTemperatureMonitoring;

		private bool isLowElectricityDetection;

		private bool isInertialNavigation;

		private bool alarmG_Sensor;

		private bool alarmAntiDemolitionAlarm;

		private bool alarmTemperatureMonitoring;

		private bool alarmLowElectricityDetection;

		private bool alarmInertialNavigation;

		public byte Category
		{
			get
			{
				return category;
			}
			set
			{
				category = value;
			}
		}

		public byte[] TagId
		{
			get
			{
				return tagId;
			}
			set
			{
				tagId = value;
			}
		}

		public byte CheckCode
		{
			get
			{
				return checkCode;
			}
			set
			{
				checkCode = value;
			}
		}

		public string SoftwareVersion
		{
			get
			{
				return softwareVersion;
			}
			set
			{
				softwareVersion = value;
			}
		}

		public string HardwareVersion
		{
			get
			{
				return hardwareVersion;
			}
			set
			{
				hardwareVersion = value;
			}
		}

		public bool IsG_Sensor
		{
			get
			{
				return isG_Sensor;
			}
			set
			{
				isG_Sensor = value;
			}
		}

		public bool IsAntiDemolitionAlarm
		{
			get
			{
				return isAntiDemolitionAlarm;
			}
			set
			{
				isAntiDemolitionAlarm = value;
			}
		}

		public bool IsTemperatureMonitoring
		{
			get
			{
				return isTemperatureMonitoring;
			}
			set
			{
				isTemperatureMonitoring = value;
			}
		}

		public bool IsLowElectricityDetection
		{
			get
			{
				return isLowElectricityDetection;
			}
			set
			{
				isLowElectricityDetection = value;
			}
		}

		public bool IsInertialNavigation
		{
			get
			{
				return isInertialNavigation;
			}
			set
			{
				isInertialNavigation = value;
			}
		}

		public bool Alarm_Shock
		{
			get
			{
				return alarmG_Sensor;
			}
			set
			{
				alarmG_Sensor = value;
			}
		}

		public bool Alarm_Dismantle
		{
			get
			{
				return alarmAntiDemolitionAlarm;
			}
			set
			{
				alarmAntiDemolitionAlarm = value;
			}
		}

		public bool Alarm_HighTemperature
		{
			get
			{
				return alarmTemperatureMonitoring;
			}
			set
			{
				alarmTemperatureMonitoring = value;
			}
		}

		public bool Alarm_LowPower
		{
			get
			{
				return alarmLowElectricityDetection;
			}
			set
			{
				alarmLowElectricityDetection = value;
			}
		}

		public bool Alarm_Move
		{
			get
			{
				return alarmInertialNavigation;
			}
			set
			{
				alarmInertialNavigation = value;
			}
		}

		internal int SetData(byte[] tBuff)
		{
			Category = tBuff[0];
			TagId = new byte[4];
			Array.Copy(tBuff, 1, TagId, 0, 4);
			CheckCode = tBuff[5];
			HardwareVersion = ((tBuff[6] >> 4).ToString() ?? "");
			SoftwareVersion = ((tBuff[6] & 0xF).ToString() ?? "");
			IsG_Sensor = ((tBuff[7] & 1) > 0);
			IsAntiDemolitionAlarm = ((tBuff[7] & 2) > 0);
			IsTemperatureMonitoring = ((tBuff[7] & 4) > 0);
			IsLowElectricityDetection = ((tBuff[7] & 8) > 0);
			IsInertialNavigation = ((tBuff[7] & 0x10) > 0);
			Alarm_Shock = ((tBuff[8] & 1) > 0);
			Alarm_Dismantle = ((tBuff[8] & 2) > 0);
			Alarm_HighTemperature = ((tBuff[8] & 4) > 0);
			Alarm_LowPower = ((tBuff[8] & 8) > 0);
			Alarm_Move = ((tBuff[8] & 0x10) > 0);
			return 13;
		}
	}
}
