using System;

namespace NetAPI.Entities
{
	public class TagID_V17
	{
		private byte[] data;

		private byte category;

		private byte subCategory;

		private byte[] serialNumber;

		public byte Category
		{
			get
			{
				return category;
			}
			set
			{
				category = value;
			}
		}

		public byte SubCategory
		{
			get
			{
				return subCategory;
			}
			set
			{
				subCategory = value;
			}
		}

		public byte[] SerialNumber
		{
			get
			{
				return serialNumber;
			}
			set
			{
				serialNumber = value;
			}
		}

		public byte[] Data => data;

		public TagID_V17()
		{
		}

		public TagID_V17(byte[] buff)
		{
			setBuff(buff);
		}

		internal void setBuff(byte[] buff)
		{
			data = buff;
			if (buff != null && buff.Length >= 5)
			{
				category = buff[0];
				subCategory = buff[1];
				serialNumber = new byte[3];
				Array.Copy(buff, 2, serialNumber, 0, 3);
			}
		}

		internal byte[] getBuff()
		{
			byte[] array = new byte[5]
			{
				category,
				subCategory,
				0,
				0,
				0
			};
			if (serialNumber != null && serialNumber.Length >= 3)
			{
				Array.Copy(serialNumber, 0, array, 2, 3);
			}
			return array;
		}
	}
}
