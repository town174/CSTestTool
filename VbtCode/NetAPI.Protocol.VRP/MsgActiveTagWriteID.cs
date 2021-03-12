using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveTagWriteID : AbstractHostMessage
	{
		public MsgActiveTagWriteID()
		{
		}

		public MsgActiveTagWriteID(ActiveTagWriteIdParameter param)
		{
			byte[] parambytes = param.GetParambytes();
			msgBody = new byte[parambytes.Length];
			Array.Copy(parambytes, 0, msgBody, 0, parambytes.Length);
		}
	}
}
