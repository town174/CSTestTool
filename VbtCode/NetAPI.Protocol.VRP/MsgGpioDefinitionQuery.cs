using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgGpioDefinitionQuery : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public GpioInfo[] GpioInformation
			{
				get
				{
					GpioInfo[] array = null;
					if (buff.Length > 1)
					{
						array = new GpioInfo[buff.Length - 1];
						for (int i = 1; i < buff.Length; i++)
						{
							array[i - 1] = new GpioInfo();
							array[i - 1].PortNO = (byte)i;
							array[i - 1].Definition = (GpioDefinition)buff[i];
						}
					}
					return array;
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

		public MsgGpioDefinitionQuery()
		{
			msgType = 540;
			msgBody = new byte[1]
			{
				1
			};
		}
	}
}
