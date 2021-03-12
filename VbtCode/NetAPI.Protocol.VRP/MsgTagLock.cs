using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgTagLock : AbstractHostMessage
	{
		public MsgTagLock()
		{
		}

		public MsgTagLock(LockTagParameter param)
		{
			byte[] selectBuff = TagParameter.GetSelectBuff(param.SelectTagParam);
			msgBody = new byte[6 + selectBuff.Length];
			int num = 0;
			Array.Copy(selectBuff, 0, msgBody, num, selectBuff.Length);
			num += selectBuff.Length;
			Array.Copy(param.AccessPassword, 0, msgBody, num, 4);
			num += 4;
			msgBody[num] = (byte)param.LockType;
			num++;
			msgBody[num] = (byte)param.LockBank;
		}
	}
}
