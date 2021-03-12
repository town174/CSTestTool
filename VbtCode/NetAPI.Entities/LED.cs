using System;

namespace NetAPI.Entities
{
	[Flags]
	public enum LED
	{
		LED1 = 0x1,
		LED2 = 0x2,
		LED3 = 0x4,
		LED4 = 0x8,
		LED5 = 0x10,
		LED6 = 0x20,
		LED7 = 0x40,
		LED8 = 0x80,
		LED9 = 0x100,
		LED10 = 0x200,
		LED11 = 0x400,
		LED12 = 0x800
	}
}
