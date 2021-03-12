namespace NetAPI.Entities
{
	public class ReadTagParameter : InventoryTagParameter
	{
		public bool IsLoop = false;

		public byte[] AccessPassword = new byte[4];

		public bool IsReturnEPC = false;

		public bool IsReturnTID = false;

		public uint UserPtr;

		public byte UserLen;

		public bool IsReturnReserved = false;

		public ReadTagParameter()
		{
			ReadCount = 1;
		}
	}
}
