using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgTagSelect : AbstractHostMessage
	{
		public MsgTagSelect()
		{
		}

		public MsgTagSelect(TagParameter param)
		{
			byte[] selectBuff = TagParameter.GetSelectBuff(param);
			msgBody = new byte[1 + selectBuff.Length];
			Array.Copy(selectBuff, 0, msgBody, 0, selectBuff.Length);
		}
	}
}
