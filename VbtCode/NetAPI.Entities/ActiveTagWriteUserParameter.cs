using System;

namespace NetAPI.Entities
{
	public class ActiveTagWriteUserParameter
	{
		public byte[] TagId = new byte[5];

		public byte[] AccessPassword = new byte[2];

		public byte[] WriteData = null;

		internal byte[] GetParambytes()
		{
			int num = 8;
			if (WriteData != null)
			{
				num += WriteData.Length;
			}
			byte[] array = new byte[num];
			Array.Copy(TagId, 0, array, 0, 5);
			Array.Copy(AccessPassword, 0, array, 5, 2);
			if (WriteData != null)
			{
				array[7] = (byte)WriteData.Length;
				Array.Copy(WriteData, 0, array, 8, WriteData.Length);
			}
			return array;
		}
	}
}
