using NetAPI.Core;
using System;

namespace NetAPI.Protocol.VRP
{
	public class Msg6CTagFieldConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public bool IsEnableAntenna
			{
				get
				{
					bool result = false;
					if (buff.Length >= 3)
					{
						result = ((buff[1] != 0) ? true : false);
					}
					return result;
				}
			}

			public bool IsEnableRSSI
			{
				get
				{
					bool result = false;
					if (buff.Length >= 3)
					{
						result = ((buff[2] != 0) ? true : false);
					}
					return result;
				}
			}

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
			}
		}

		private bool isEnableAntenna;

		private bool isEnableRSSI;

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

		public Msg6CTagFieldConfig()
		{
			msgBody = new byte[1]
			{
				1
			};
		}

		public Msg6CTagFieldConfig(bool isEnableAntenna, bool isEnableRSSI)
		{
			this.isEnableAntenna = isEnableAntenna;
			this.isEnableRSSI = isEnableRSSI;
			msgBody = new byte[3];
			msgBody[0] = 0;
			msgBody[1] = (byte)(isEnableAntenna ? 1 : 0);
			msgBody[2] = (byte)(isEnableRSSI ? 1 : 0);
			base.OnExecuted += Msg6CTagFieldConfig_OnExecuted;
		}

		private void Msg6CTagFieldConfig_OnExecuted(object sender, EventArgs e)
		{
			if (statusCode == MsgStatus.Success)
			{
				Reader reader = (Reader)sender;
				if (reader.RS485Items != null && reader.RS485Items.Length != 0)
				{
					RS485Item[] rS485Items = reader.RS485Items;
					int num = 0;
					RS485Item rS485Item;
					while (true)
					{
						if (num >= rS485Items.Length)
						{
							return;
						}
						rS485Item = rS485Items[num];
						if (rS485Item.Address == address)
						{
							break;
						}
						num++;
					}
					rS485Item.isEnableAntenna = isEnableAntenna;
					rS485Item.isEnableRSSI = isEnableRSSI;
				}
				else
				{
					reader.isEnableAntenna = isEnableAntenna;
					reader.isEnableRSSI = isEnableRSSI;
				}
			}
		}

		~Msg6CTagFieldConfig()
		{
			base.OnExecuted -= Msg6CTagFieldConfig_OnExecuted;
		}
	}
}
