namespace NetAPI.Core
{
	public abstract class ReceivedInfo
	{
		protected byte[] buff;

		public ReceivedInfo(byte[] buff)
		{
			this.buff = buff;
		}
	}
}
