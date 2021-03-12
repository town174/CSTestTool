using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgFrequencyConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public FrequencyTable FreqInfo
			{
				get
				{
					FrequencyTable frequencyTable = new FrequencyTable();
					if (buff.Length >= 2)
					{
						frequencyTable.IsAutoSet = ((buff[1] == 1) ? true : false);
					}
					if (buff.Length > 2)
					{
						frequencyTable.FreqTable = new byte[buff.Length - 2];
						for (int i = 2; i < buff.Length; i++)
						{
							frequencyTable.FreqTable[i - 2] = buff[i];
						}
					}
					return frequencyTable;
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

		public MsgFrequencyConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgFrequencyConfig(FrequencyTable param)
		{
			int num = 2;
			if (param.FreqTable != null)
			{
				num += param.FreqTable.Length;
			}
			msgBody = new byte[num];
			msgBody[0] = 0;
			msgBody[1] = (byte)(param.IsAutoSet ? 1 : 0);
			if (num > 2)
			{
				for (int i = 0; i < param.FreqTable.Length; i++)
				{
					msgBody[i + 2] = param.FreqTable[i];
				}
			}
		}
	}
}
