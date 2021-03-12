using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class RS485Item
	{
		private byte address;

		internal Reader pReader;

		internal bool isEnableRSSI;

		internal bool isEnableAntenna;

		public FrequencyArea UhfBand = FrequencyArea.Unknown;

		internal string modelNumber = "";

		public byte Address
		{
			get
			{
				return address;
			}
			set
			{
				address = value;
			}
		}

		public string ModelNumber => modelNumber;

		public bool IsEnableRSSI => isEnableRSSI;

		public bool IsEnableAntenna => isEnableAntenna;

		public bool Send(IHostMessage msg)
		{
			if (msg == null)
			{
				return false;
			}
			((MessageFrame)msg).isRS485 = true;
			((MessageFrame)msg).address = address;
			return pReader.Send(msg);
		}

		public bool Send(IHostMessage msg, int timeout)
		{
			if (msg == null)
			{
				return false;
			}
			((MessageFrame)msg).isRS485 = true;
			((MessageFrame)msg).address = address;
			msg.Timeout = timeout;
			return pReader.Send(msg);
		}

		public bool Send(byte[] msgBuff)
		{
			if (msgBuff == null)
			{
				return false;
			}
			return pReader.Send(msgBuff);
		}
	}
}
