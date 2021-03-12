using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveTagWriteUser : AbstractHostMessage
	{
		public MsgActiveTagWriteUser()
		{
		}

		public MsgActiveTagWriteUser(ActiveTagWriteUserParameter param)
		{
			byte[] parambytes = param.GetParambytes();
			msgBody = new byte[parambytes.Length];
			Array.Copy(parambytes, 0, msgBody, 0, parambytes.Length);
		}
	}
}
