namespace NetAPI.Entities
{
	public class AlarmInfo_V17
	{
		private bool alarmTemperatureMonitoring;

		private byte temperature;

		private bool alarmG_Sensor;

		private bool alarmAntiDemolitionAlarm;

		private bool alarmLowElectricityDetection;

		private bool alarmVoice;

		private bool alarmLight;

		public bool Alarm_HighTemperature => alarmTemperatureMonitoring;

		public byte Temperature => temperature;

		public bool Alarm_Voice => alarmVoice;

		public bool Alarm_Light => alarmLight;

		public bool Alarm_Shock => alarmG_Sensor;

		public bool Alarm_Dismantle => alarmAntiDemolitionAlarm;

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

		public AlarmInfo_V17()
		{
		}

		public AlarmInfo_V17(byte[] buff)
		{
			setBuff(buff);
		}

		internal void setBuff(byte[] buff)
		{
			alarmTemperatureMonitoring = ((buff[1] & 0x40) > 0);
			temperature = (byte)((buff[1] & 0x3F) << 2);
			alarmVoice = ((buff[2] & 0x10) > 0);
			alarmLight = ((buff[2] & 8) > 0);
			alarmG_Sensor = ((buff[2] & 4) > 0);
			alarmAntiDemolitionAlarm = ((buff[2] & 2) > 0);
			alarmLowElectricityDetection = ((buff[2] & 1) > 0);
		}

		internal byte[] getBuff()
		{
			byte[] array = new byte[3];
			array[1] += (byte)(alarmTemperatureMonitoring ? 64 : 0);
			array[1] += (byte)(temperature >> 2);
			array[2] += (byte)(alarmVoice ? 16 : 0);
			array[2] += (byte)(alarmLight ? 8 : 0);
			array[2] += (byte)(alarmG_Sensor ? 4 : 0);
			array[2] += (byte)(alarmAntiDemolitionAlarm ? 2 : 0);
			array[2] += (byte)(alarmLowElectricityDetection ? 1 : 0);
			return array;
		}
	}
}
