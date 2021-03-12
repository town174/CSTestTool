using System;

namespace NetAPI.Entities
{
	public class ActiveTagData_04
	{
		private byte category = 4;

		private byte[] tagId;

		private byte checkCode;

		private AlarmInfo_04 alarmInfo;

		private byte[] user;

		private byte[] pwd;

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

		public byte CheckCode
		{
			get
			{
				return checkCode;
			}
			set
			{
				checkCode = value;
			}
		}

		public AlarmInfo_04 AlarmInfo
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

		internal int SetData(byte[] tBuff)
		{
			if (tBuff == null)
			{
				return 0;
			}
			int num = 0;
			if (tBuff.Length != 0)
			{
				category = tBuff[num];
				num++;
			}
			if (tBuff.Length >= 6)
			{
				tagId = new byte[5];
				Array.Copy(tBuff, num, tagId, 0, 5);
				num += 5;
			}
			if (tBuff.Length >= 7)
			{
				checkCode = tBuff[num];
				num++;
			}
			if (tBuff.Length >= 10)
			{
				alarmInfo = new AlarmInfo_04();
				alarmInfo.Alarm_HighTemperature = ((tBuff[num + 1] & 0x40) > 0);
				alarmInfo.Alarm_Shock = ((tBuff[num + 2] & 4) > 0);
				alarmInfo.Alarm_Dismantle = ((tBuff[num + 2] & 2) > 0);
				alarmInfo.Alarm_LowPower = ((tBuff[num + 2] & 1) > 0);
				num += 3;
			}
			if (tBuff.Length >= 11)
			{
				int num2 = tBuff[num];
				num++;
				if (num2 > 0 && tBuff.Length >= 12 + num2)
				{
					user = new byte[num2];
					Array.Copy(tBuff, num, user, 0, num2);
				}
				num += 12;
			}
			if (tBuff.Length >= 25)
			{
				pwd = new byte[2];
				Array.Copy(tBuff, num, pwd, 0, 2);
				num += 2;
			}
			return num;
		}
	}
}
