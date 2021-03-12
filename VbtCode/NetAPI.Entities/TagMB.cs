using System;

namespace NetAPI.Entities
{
	[Flags]
	public enum TagMB
	{
		EPC = 0x1,
		TID = 0x2,
		User = 0x4
	}
}
