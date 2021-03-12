using NetAPI.Protocol.VRP;
using System;

namespace NetAPI.Entities
{
	public class TagParameter
	{
		public MemoryBank MemoryBank;

		public uint Ptr;

		public byte[] TagData;

		internal static byte[] GetSelectBuff(TagParameter param)
		{
			byte[] array = EVB.ConvertToEvb(param.Ptr);
			byte[] array2 = new byte[2 + array.Length + param.TagData.Length];
			int num = 0;
			array2[num] = (byte)param.MemoryBank;
			num++;
			Array.Copy(array, 0, array2, num, array.Length);
			num += array.Length;
			array2[num] = (byte)(param.TagData.Length * 8);
			num++;
			Array.Copy(param.TagData, 0, array2, num, param.TagData.Length);
			return array2;
		}
	}
}
