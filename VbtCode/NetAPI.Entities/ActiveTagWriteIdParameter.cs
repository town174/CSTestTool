using System;

namespace NetAPI.Entities
{
	public class ActiveTagWriteIdParameter
	{
		public byte[] TagId = new byte[5];

		public byte[] Password = new byte[4];

		internal byte[] GetParambytes()
		{
			int num = 0;
			if (TagId != null)
			{
				num += TagId.Length;
			}
			if (Password != null)
			{
				num += Password.Length;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			if (TagId != null)
			{
				Array.Copy(TagId, 0, array, 0, TagId.Length);
				num2 += TagId.Length;
			}
			if (Password != null)
			{
				Array.Copy(Password, 0, array, num2, Password.Length);
			}
			return array;
		}
	}
}
