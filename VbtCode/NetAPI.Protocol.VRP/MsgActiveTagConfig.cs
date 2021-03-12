using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveTagConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public byte[] TagID
			{
				get
				{
					byte[] array = null;
					if (buff.Length >= 5)
					{
						array = new byte[5];
						Array.Copy(buff, 0, array, 0, 5);
					}
					return array;
				}
			}

			public TagActiveParameter TagActiveParam
			{
				get
				{
					TagActiveParameter tagActiveParameter = null;
					if (buff.Length >= 6)
					{
						tagActiveParameter = new TagActiveParameter();
						tagActiveParameter.SetParam(buff[6]);
					}
					return tagActiveParameter;
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

		public MsgActiveTagConfig()
		{
		}

		public MsgActiveTagConfig(byte[] tagID, TagActiveParameter param)
		{
			int num = (tagID != null) ? tagID.Length : 0;
			msgBody = new byte[2 + num];
			msgBody[0] = 0;
			if (num > 0)
			{
				Array.Copy(tagID, 0, msgBody, 1, num);
			}
			msgBody[1 + num] = param.GetParambyte();
		}
	}
}
