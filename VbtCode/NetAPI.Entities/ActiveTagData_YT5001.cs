using System;

namespace NetAPI.Entities
{
	public class ActiveTagData_YT5001
	{
		private byte[] tagId;

		public byte[] TagId
		{
			get
			{
				return tagId;
			}
			set
			{
				tagId = value;
			}
		}

		internal int SetData(byte[] tBuff)
		{
			TagId = new byte[5];
			Array.Copy(tBuff, 0, TagId, 0, 5);
			return 5;
		}
	}
}
