namespace NetAPI.Entities
{
	public class LightSwitch
	{
		public LightState Light1;

		public LightState Light2;

		internal byte GetParambyte()
		{
			byte b = 0;
			return (byte)(((int)Light2 << 1) + Light1);
		}

		internal void SetParambyte(byte param)
		{
			Light1 = (LightState)(param & 1);
			Light2 = (LightState)((param >> 1) & 1);
		}
	}
}
