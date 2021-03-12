namespace NetAPI.Entities
{
	public class LockTagParameter
	{
		public LockBank LockBank;

		public LockType LockType;

		public TagParameter SelectTagParam = null;

		public byte[] AccessPassword = new byte[4];
	}
}
