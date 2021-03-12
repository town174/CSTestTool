namespace NetAPI.Entities
{
	public class ActiveParameter
	{
		public bool SleepFlag;

		public ushort Count;

		internal byte[] GetParambytes()
		{
			return new byte[3]
			{
				(byte)(SleepFlag ? 1 : 0),
				(byte)(Count >> 8),
				(byte)(Count & 0xFF)
			};
		}
	}
}
