namespace NetAPI.Entities
{
	public class AlarmInfo_04
	{
		private bool alarmG_Sensor;

		private bool alarmAntiDemolitionAlarm;

		private bool alarmTemperatureMonitoring;

		private bool alarmLowElectricityDetection;

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
	}
}
