using NetAPI.Core;

namespace NetAPI.Entities
{
	public class GpiTriggerParameter
	{
		public byte PortNO;

		public GpiTriggerCondition TriggerCondition;

		public GpiStopCondition StopCondition;

		public ushort DelayTime;

		public IHostMessage TriggerMsg;
	}
}
