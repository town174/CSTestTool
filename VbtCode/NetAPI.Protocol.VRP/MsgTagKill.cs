using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgTagKill : AbstractHostMessage
	{
		public MsgTagKill()
		{
		}

		public MsgTagKill(KillTagParameter param)
		{
			byte[] selectBuff = TagParameter.GetSelectBuff(param.SelectTagParam);
			msgBody = new byte[4 + selectBuff.Length];
			int num = 0;
			Array.Copy(selectBuff, 0, msgBody, num, selectBuff.Length);
			num += selectBuff.Length;
			Array.Copy(param.KillPassword, 0, msgBody, num, 4);
		}
	}
}
