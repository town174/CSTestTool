using NetAPI.Core;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgUtcConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public DateTime UTC
			{
				get
				{
					DateTime result = new DateTime(1970, 1, 1);
					if (buff.Length >= 9)
					{
						byte[] array = new byte[8];
						Array.Copy(buff, 1, array, 0, 8);
						return Common.setUtcTime(array);
					}
					return result;
				}
			}

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
			}
		}

		public new ReceivedInfo ReceivedMessage
		{
			get
			{
				if (recvInfo == null)
				{
					if (msgBody == null)
					{
						return null;
					}
					recvInfo = new ReceivedInfo(msgBody);
				}
				return (ReceivedInfo)recvInfo;
			}
		}

		public MsgUtcConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgUtcConfig(DateTime time)
		{
			msgBody = new byte[9];
			msgBody[0] = 0;
			byte[] dateTimeBuff = getDateTimeBuff(time);
			Array.Copy(dateTimeBuff, 0, msgBody, 1, 8);
		}

		private byte[] getDateTimeBuff(DateTime time)
		{
			uint num = 0u;
			uint num2 = 0u;
			TimeSpan timeSpan = time - new DateTime(1970, 1, 1);
			num = (uint)timeSpan.TotalSeconds;
			num2 = (uint)timeSpan.Milliseconds;
			return new byte[8]
			{
				(byte)(num >> 24),
				(byte)(num >> 16),
				(byte)(num >> 8),
				(byte)num,
				0,
				0,
				(byte)(num2 >> 8),
				(byte)num2
			};
		}
	}
}
