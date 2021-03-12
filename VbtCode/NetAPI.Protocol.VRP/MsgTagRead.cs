using NetAPI.Core;
using NetAPI.Entities;
using System;
using System.Collections.Generic;

namespace NetAPI.Protocol.VRP
{
	public class MsgTagRead : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			private RxdTagData tagData;

			private int epcLen = 0;

			private int tidLen = 0;

			private int userLen = 0;

			private int resvLen = 0;

			private byte ant = 0;

			private byte hub;

			private byte port;

			private byte rssi = 0;

			public RxdTagData TagData => tagData;

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				tagData = new RxdTagData();
				int num = 0;
				if (buff.Length >= 1)
				{
					epcLen = buff[num];
					num++;
				}
				if (buff.Length >= 1 + epcLen)
				{
					tagData.EPC = new byte[epcLen];
					Array.Copy(buff, num, tagData.EPC, 0, epcLen);
					num += epcLen;
				}
				if (buff.Length >= 1 + num)
				{
					tidLen = buff[num];
					num++;
				}
				if (buff.Length >= num + tidLen)
				{
					tagData.TID = new byte[tidLen];
					Array.Copy(buff, num, tagData.TID, 0, tidLen);
					num += tidLen;
				}
				if (buff.Length >= 1 + num)
				{
					userLen = buff[num];
					num++;
				}
				if (buff.Length >= num + userLen)
				{
					tagData.User = new byte[userLen];
					Array.Copy(buff, num, tagData.User, 0, userLen);
					num += userLen;
				}
				if (buff.Length >= 1 + num)
				{
					resvLen = buff[num];
					num++;
				}
				if (buff.Length >= num + resvLen)
				{
					tagData.Reserved = new byte[resvLen];
					Array.Copy(buff, num, tagData.Reserved, 0, resvLen);
					num += resvLen;
				}
				if (buff.Length >= num + 2)
				{
					tagData.Antenna = (ant = buff[num]);
					if (buff.Length == num + 3)
					{
						tagData.Hub = (hub = buff[num + 1]);
						tagData.Port = (port = buff[num + 2]);
					}
					else if (buff.Length == num + 4)
					{
						tagData.Hub = (hub = buff[num + 1]);
						tagData.Port = (port = buff[num + 2]);
						tagData.RSSI = (rssi = buff[num + 3]);
					}
					else
					{
						tagData.RSSI = (rssi = buff[num + 1]);
					}
				}
			}

			internal void setAntennaAndRSSI(bool a, bool r)
			{
				if (ant == 0 && rssi == 0)
				{
					int num = epcLen + userLen + tidLen + resvLen + 4;
					if (a)
					{
						if (buff.Length >= num + 1)
						{
							tagData.Antenna = (ant = buff[num]);
							if (buff.Length >= num + 3)
							{
								tagData.Hub = (hub = buff[num + 1]);
								tagData.Port = (port = buff[num + 2]);
							}
						}
						if (r && buff.Length >= num + 2)
						{
							if (buff.Length == num + 4)
							{
								tagData.RSSI = (rssi = buff[num + 3]);
							}
							else
							{
								tagData.RSSI = (rssi = buff[num + 1]);
							}
						}
					}
					else if (r && buff.Length >= num + 1)
					{
						tagData.RSSI = (rssi = buff[num]);
					}
				}
			}
		}

		private ReadTagParameter param;

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

		public MsgTagRead()
		{
			isReturn = false;
			setReadMsgBody(new ReadTagParameter
			{
				IsLoop = true,
				IsReturnTID = true,
				ReadCount = 0
			});
		}

		public MsgTagRead(ReadTagParameter param)
		{
			this.param = param;
			if (param.IsLoop)
			{
				isReturn = false;
				if (param.TagFilteringTime != 65535 || param.ReadTime != 65535 || param.StopTime != 65535)
				{
					base.OnExecuting += MsgTagRead_OnExecuting1;
				}
			}
			if (param.SelectTagParam != null)
			{
				base.OnExecuting += MsgTagRead_OnExecuting2;
			}
			setReadMsgBody(param);
		}

		private void MsgTagRead_OnExecuting1(object sender, EventArgs e)
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

		private void MsgTagRead_OnExecuting2(object sender, EventArgs e)
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

		internal void setReadMsgBody(ReadTagParameter param)
		{
			List<byte> list = new List<byte>();
			list.Add((byte)(param.IsLoop ? 1 : 0));
			list.AddRange(param.AccessPassword);
			list.Add((byte)(param.IsReturnEPC ? 1 : 0));
			if (param.IsReturnTID)
			{
				list.AddRange(new byte[3]
				{
					0,
					6,
					1
				});
			}
			else
			{
				list.AddRange(new byte[3]);
			}
			list.AddRange(EVB.ConvertToEvb(param.UserPtr));
			list.Add(param.UserLen);
			if (param.IsReturnReserved)
			{
				list.AddRange(new byte[2]
				{
					0,
					4
				});
			}
			else
			{
				list.AddRange(new byte[2]);
			}
			list.Add((byte)(param.ReadCount >> 8));
			list.Add((byte)(param.ReadCount & 0xFF));
			list.Add((byte)(param.TotalReadTime >> 8));
			list.Add((byte)(param.TotalReadTime & 0xFF));
			msgBody = list.ToArray();
		}

		~MsgTagRead()
		{
			base.OnExecuting -= MsgTagRead_OnExecuting1;
			base.OnExecuting -= MsgTagRead_OnExecuting2;
		}
	}
}
