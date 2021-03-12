using System;

namespace NetAPI.Entities
{
	public class InventoryTagParameter
	{
		public ushort ReadCount;

		public ushort TotalReadTime;

		public ushort TagFilteringTime = ushort.MaxValue;

		public ushort ReadTime = ushort.MaxValue;

		public ushort StopTime = ushort.MaxValue;

		[Obsolete]
		public ushort IdleTime = ushort.MaxValue;

		public TagParameter SelectTagParam = null;
	}
}
