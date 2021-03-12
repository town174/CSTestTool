using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgTagInventory : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			private RxdTagData tagData;

			private byte epclen;

			private byte ant;

			private byte hub;

			private byte port;

			private byte rssi;

			public RxdTagData TagData => tagData;

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				if (base.buff.Length >= 1)
				{
					tagData = new RxdTagData();
					epclen = buff[0];
					if (buff.Length > epclen)
					{
						tagData.EPC = new byte[epclen];
						Array.Copy(buff, 1, tagData.EPC, 0, epclen);
					}
					if (buff.Length > epclen + 2)
					{
						tagData.Antenna = (ant = buff[epclen + 1]);
						if (buff.Length == epclen + 4)
						{
							tagData.Hub = (hub = buff[epclen + 2]);
							tagData.Port = (port = buff[epclen + 3]);
						}
						else if (buff.Length == epclen + 5)
						{
							tagData.Hub = (hub = buff[epclen + 2]);
							tagData.Port = (port = buff[epclen + 3]);
							tagData.RSSI = (rssi = buff[epclen + 4]);
						}
						else
						{
							tagData.RSSI = (rssi = buff[epclen + 2]);
						}
					}
				}
			}

			internal void setAntennaAndRSSI(bool a, bool r)
			{
				if (ant == 0 && rssi == 0)
				{
					if (a)
					{
						if (buff.Length > epclen + 1)
						{
							tagData.Antenna = (ant = buff[epclen + 1]);
							if (buff.Length >= epclen + 4)
							{
								tagData.Hub = (hub = buff[epclen + 2]);
								tagData.Port = (port = buff[epclen + 3]);
							}
						}
						if (r && buff.Length > epclen + 2)
						{
							if (buff.Length == epclen + 5)
							{
								tagData.RSSI = (rssi = buff[epclen + 4]);
							}
							else
							{
								tagData.RSSI = (rssi = buff[epclen + 2]);
							}
						}
					}
					else if (r && buff.Length > epclen + 1)
					{
						tagData.RSSI = (rssi = buff[epclen + 1]);
					}
				}
			}
		}

		internal bool isAutoPowerConft = false;

		private InventoryTagParameter param;

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

		public MsgTagInventory()
		{
			isReturn = false;
			msgBody = new byte[4];
		}

		public MsgTagInventory(InventoryTagParameter param)
		{
			this.param = param;
			if (param.ReadCount != 1)
			{
				isReturn = false;
				if (param.TagFilteringTime != 65535 || param.ReadTime != 65535 || param.StopTime != 65535)
				{
					base.OnExecuting += MsgTagInventory_OnExecuting1;
				}
			}
			msgBody = new byte[4];
			msgBody[0] = (byte)(param.ReadCount >> 8);
			msgBody[1] = (byte)(param.ReadCount & 0xFF);
			msgBody[2] = (byte)(param.TotalReadTime >> 8);
			msgBody[3] = (byte)(param.TotalReadTime & 0xFF);
			if (param.SelectTagParam != null)
			{
				base.OnExecuting += MsgTagInventory_OnExecuting2;
			}
		}

		private void MsgTagInventory_OnExecuting1(object sender, EventArgs e)
		{
			Reader reader = (Reader)sender;
			if (param.TagFilteringTime != 65535)
			{
				reader.Send(new MsgFilteringTimeConfig(param.TagFilteringTime));
			}
			if (param.ReadTime != 65535 || param.StopTime != 65535)
			{
				reader.Send(new MsgIntervalTimeConfig(param.ReadTime, param.StopTime));
			}
		}

		private void MsgTagInventory_OnExecuting2(object sender, EventArgs e)
		{
			Reader reader = (Reader)sender;
			MsgTagSelect msgTagSelect = new MsgTagSelect(param.SelectTagParam);
			if (!reader.Send(msgTagSelect))
			{
				statusCode = msgTagSelect.Status;
				errInfo = msgTagSelect.ErrorInfo;
				throw new Exception(msgTagSelect.ErrorInfo.ErrMsg);
			}
		}

		~MsgTagInventory()
		{
			base.OnExecuting -= MsgTagInventory_OnExecuting1;
			base.OnExecuting -= MsgTagInventory_OnExecuting2;
		}
	}
}
