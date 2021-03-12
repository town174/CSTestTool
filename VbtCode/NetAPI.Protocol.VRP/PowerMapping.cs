using System.Runtime.Serialization;

namespace NetAPI.Protocol.VRP
{
	[DataContract]
	internal class PowerMapping
	{
		[DataMember]
		public byte Power
		{
			get;
			set;
		}

		[DataMember]
		public int Max
		{
			get;
			set;
		}

		[DataMember]
		public int Min
		{
			get;
			set;
		}
	}
}
