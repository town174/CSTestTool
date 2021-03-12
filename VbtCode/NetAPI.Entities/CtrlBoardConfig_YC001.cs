namespace NetAPI.Entities
{
	public class CtrlBoardConfig_YC001
	{
		public bool IsEnable;

		public bool IsEnableFront;

		public bool IsEnableBack;

		public byte SensorNum;

		internal byte GetParambyte()
		{
			byte b = 0;
			if (IsEnable)
			{
				b = (byte)(b + 128);
			}
			if (IsEnableFront)
			{
				b = (byte)(b + 32);
			}
			if (IsEnableBack)
			{
				b = (byte)(b + 16);
			}
			return (byte)(b + SensorNum);
		}

		internal void SetParambyte(byte param)
		{
			SensorNum = (byte)(param & 0xF);
			IsEnableBack = ((param & 0x10) > 0);
			IsEnableFront = ((param & 0x20) > 0);
			IsEnable = ((param & 0x80) > 0);
		}
	}
}
