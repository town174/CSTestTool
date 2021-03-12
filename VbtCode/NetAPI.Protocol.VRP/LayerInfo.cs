using System.Runtime.Serialization;

namespace NetAPI.Protocol.VRP
{
	[DataContract]
	public class LayerInfo
	{
		[DataMember]
		public byte AntennaNO
		{
			get;
			set;
		}

		[DataMember]
		public byte LayerNO
		{
			get;
			set;
		}

		[DataMember]
		public byte Power
		{
			get;
			set;
		}
	}
}
