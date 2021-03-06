using NetAPI.Core;
using NetAPI.Entities;
using System;
using System.Collections.Generic;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveTagInventory : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			private List<RxdActiveTag> lst = new List<RxdActiveTag>();

			private byte tagDatalen;

			private byte tagCount;

			public List<RxdActiveTag> ActiveTagList => lst;

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				if (base.buff.Length >= 2)
				{
					tagCount = buff[0];
					tagDatalen = buff[1];
					if (buff.Length >= tagCount * tagDatalen)
					{
						for (int i = 0; i < tagCount; i++)
						{
							byte[] array = new byte[tagDatalen];
							Array.Copy(buff, i * tagDatalen + 2, array, 0, tagDatalen);
							RxdActiveTag rxdActiveTag = new RxdActiveTag();
							rxdActiveTag.SetData(array);
							lst.Add(rxdActiveTag);
						}
					}
				}
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

		public MsgActiveTagInventory()
		{
			isReturn = false;
			msgBody = new byte[4];
		}

		public MsgActiveTagInventory(ushort readCount, ushort readTime)
		{
			isReturn = false;
			msgBody = new byte[4];
			msgBody[0] = (byte)(readCount >> 8);
			msgBody[1] = (byte)(readCount & 0xFF);
			msgBody[2] = (byte)(readTime >> 8);
			msgBody[3] = (byte)(readTime & 0xFF);
		}
	}
}
