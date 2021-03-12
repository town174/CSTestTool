using NetAPI.Core;
using System.Text;

namespace NetAPI.Protocol.VRP
{
	public class MsgReaderCapabilityQuery : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public string ModelNumber
			{
				get
				{
					if (buff.Length >= 7)
					{
						return Encoding.ASCII.GetString(buff, 1, 6);
					}
					return "";
				}
			}

			public byte AntennaCount
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 8)
					{
						result = buff[7];
					}
					return result;
				}
			}

			public byte MinPowerValue
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 9)
					{
						result = buff[8];
					}
					return result;
				}
			}

			public byte MaxPowerValue
			{
				get
				{
					byte result = 0;
					if (buff.Length >= 10)
					{
						result = buff[9];
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

		public MsgReaderCapabilityQuery()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}
	}
}
