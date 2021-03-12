using System;

namespace NetAPI.Entities
{
	public class ActiveTagData_V17
	{
		private byte tagType = 4;

		private TagID_V17 tagId;

		private AlarmInfo_V17 alarmInfo;

		private byte userLen;

		private byte[] user;

		private byte[] pwd;

		public byte TagType
		{
			get
			{
				return tagType;
			}
			set
			{
				tagType = value;
			}
		}

		public TagID_V17 TagID
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

		public AlarmInfo_V17 AlarmInfo
		{
			get
			{
				return alarmInfo;
			}
			set
			{
				alarmInfo = value;
			}
		}

		public byte[] UserData
		{
			get
			{
				return user;
			}
			set
			{
				user = value;
			}
		}

		public byte[] UserPwd
		{
			get
			{
				return pwd;
			}
			set
			{
				pwd = value;
			}
		}

		internal int SetData(byte[] tBuff)
		{
			if (tBuff == null)
			{
				return 0;
			}
			int num = 0;
			if (tBuff.Length != 0)
			{
				tagType = tBuff[num];
				num++;
			}
			if (tBuff.Length >= 6)
			{
				byte[] array = new byte[5];
				Array.Copy(tBuff, num, array, 0, 5);
				num += 5;
				tagId = new TagID_V17(array);
			}
			if (tBuff.Length >= 9)
			{
				byte[] array2 = new byte[3];
				Array.Copy(tBuff, num, array2, 0, 3);
				num += 3;
				alarmInfo = new AlarmInfo_V17(array2);
			}
			if (tBuff.Length >= 10)
			{
				userLen = tBuff[num++];
			}
			if (tBuff.Length >= 22)
			{
				if (userLen > 0)
				{
					user = new byte[userLen];
					Array.Copy(tBuff, num, user, 0, userLen);
				}
				num += 12;
			}
			if (tBuff.Length >= 26)
			{
				pwd = new byte[4];
				Array.Copy(tBuff, num, pwd, 0, 4);
				num += 4;
			}
			return num;
		}
	}
}
