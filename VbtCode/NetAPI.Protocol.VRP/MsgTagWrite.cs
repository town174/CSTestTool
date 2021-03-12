using NetAPI.Entities;
using System;
using System.Collections.Generic;

namespace NetAPI.Protocol.VRP
{
	public class MsgTagWrite : AbstractHostMessage
	{
		private WriteTagParameter param;

		public MsgTagWrite()
		{
		}

		public MsgTagWrite(WriteTagParameter param)
		{
			this.param = param;
			if (param.IsLoop)
			{
				base.IsReturn = false;
				setWriteMsgBody();
			}
			else
			{
				setWriteMsgBody();
			}
		}

		private byte[] getWriteBuff(TagParameter param)
		{
			byte[] array = EVB.ConvertToEvb(param.Ptr);
			if (param.TagData.Length % 2 == 1)
			{
				byte[] array2 = new byte[param.TagData.Length + 1];
				Array.Copy(param.TagData, 0, array2, 0, array2.Length - 1);
				param.TagData = array2;
			}
			byte[] array3 = new byte[1 + array.Length + param.TagData.Length];
			int num = 0;
			Array.Copy(array, 0, array3, num, array.Length);
			num += array.Length;
			array3[num] = (byte)(param.TagData.Length / 2);
			num++;
			Array.Copy(param.TagData, 0, array3, num, param.TagData.Length);
			return array3;
		}

		private void setWriteMsgBody()
		{
			List<byte> list = new List<byte>();
			list.Add((byte)(param.IsLoop ? 1 : 0));
			list.AddRange(param.AccessPassword);
			byte[] selectBuff = TagParameter.GetSelectBuff(param.SelectTagParam);
			list.AddRange(selectBuff);
			byte[] array = null;
			byte[] array2 = null;
			byte[] array3 = null;
			TagParameter[] writeDataAry = param.WriteDataAry;
			foreach (TagParameter tagParameter in writeDataAry)
			{
				switch (tagParameter.MemoryBank)
				{
				case MemoryBank.EPCMemory:
					array = getWriteBuff(tagParameter);
					break;
				case MemoryBank.UserMemory:
					array2 = getWriteBuff(tagParameter);
					break;
				case MemoryBank.ReservedMemory:
					array3 = getWriteBuff(tagParameter);
					break;
				}
			}
			if (array == null)
			{
				array = new byte[2];
			}
			if (array2 == null)
			{
				array2 = new byte[2];
			}
			if (array3 == null)
			{
				array3 = new byte[2];
			}
			list.AddRange(array);
			list.AddRange(array2);
			list.AddRange(array3);
			msgBody = list.ToArray();
		}
	}
}
