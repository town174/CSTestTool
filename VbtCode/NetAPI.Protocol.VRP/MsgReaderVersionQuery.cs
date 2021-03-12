using NetAPI.Core;
using System.Text;

namespace NetAPI.Protocol.VRP
{
	public class MsgReaderVersionQuery : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public string ModelNumber
			{
				get
				{
					if (buff.Length >= 6)
					{
						return Encoding.ASCII.GetString(buff, 0, 6);
					}
					return "";
				}
			}

			public string HardwareVersion
			{
				get
				{
					if (buff.Length >= 10)
					{
						return Encoding.ASCII.GetString(buff, 6, 4);
					}
					return "";
				}
			}

			public string SoftwareVersion
			{
				get
				{
					if (buff.Length >= 16)
					{
						return Encoding.ASCII.GetString(buff, 10, 6);
					}
					return "";
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
	}
}
