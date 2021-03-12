using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgUhfBandConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public FrequencyArea UhfBand
			{
				get
				{
					FrequencyArea result = FrequencyArea.Unknown;
					if (buff.Length >= 2)
					{
						result = (FrequencyArea)buff[1];
					}
					return result;
				}
			}

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
			}
		}

		private FrequencyArea band;

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

		public MsgUhfBandConfig()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgUhfBandConfig(FrequencyArea band)
		{
			this.band = band;
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = (byte)band;
			base.OnExecuted += MsgUhfBandConfig_OnExecuted;
		}

		private void MsgUhfBandConfig_OnExecuted(object sender, EventArgs e)
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
					rS485Item.UhfBand = band;
				}
				else
				{
					reader.UhfBand = band;
				}
			}
		}

		~MsgUhfBandConfig()
		{
			base.OnExecuted -= MsgUhfBandConfig_OnExecuted;
		}
	}
}
