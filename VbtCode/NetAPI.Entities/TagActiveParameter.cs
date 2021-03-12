namespace NetAPI.Entities
{
	public class TagActiveParameter
	{
		public bool IsLedAlarm;

		public bool IsBeepAlarm;

		public bool IsStatMode;

		public BingResult Result;

		internal byte GetParambyte()
		{
			byte b = 0;
			b = (byte)(b + (byte)(IsLedAlarm ? 1 : 0));
			b = (byte)(b + (byte)((IsBeepAlarm ? 1 : 0) << 1));
			b = (byte)(b + (byte)((IsStatMode ? 1 : 0) << 2));
			return (byte)(b + (byte)((int)Result << 3));
		}

		internal void SetParam(byte v)
		{
			IsLedAlarm = ((v & 1) == 1);
			IsBeepAlarm = ((v & 2) == 2);
			IsStatMode = ((v & 4) == 4);
			Result = (BingResult)((v & 0x18) >> 3);
		}
	}
}
