using NetAPI.Core;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgFirmwareOnlineUpgrade : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public uint FrameAddress
			{
				get
				{
					uint result = 0u;
					if (buff.Length >= 4)
					{
						result = (uint)((buff[0] << 24) + (buff[1] << 16) + (buff[2] << 8) + buff[3]);
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

		public MsgFirmwareOnlineUpgrade()
		{
		}

		public MsgFirmwareOnlineUpgrade(uint fAddr, byte[] data)
		{
			if (data == null || data.Length > 200)
			{
				throw new Exception("升级数据长度不合法,其范围应在1-200字节");
			}
			msgBody = new byte[4 + data.Length];
			msgBody[0] = (byte)(fAddr >> 24);
			msgBody[1] = (byte)((fAddr >> 16) & 0xFF);
			msgBody[2] = (byte)((fAddr >> 8) & 0xFF);
			msgBody[3] = (byte)(fAddr & 0xFF);
			Array.Copy(data, 0, msgBody, 4, data.Length);
		}
	}
}
