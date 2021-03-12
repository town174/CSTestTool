namespace NetAPI.Entities
{
	public class WriteTagParameter
	{
		public bool IsLoop = false;

		public byte[] AccessPassword = new byte[4];

		public TagParameter SelectTagParam = null;

		public TagParameter[] WriteDataAry = null;
	}
}
