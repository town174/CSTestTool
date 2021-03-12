namespace NetAPI
{
	public class Rs485Port : IPort
	{
		private byte[] addresses;

		public byte[] Addresses
		{
			get
			{
				return addresses;
			}
			set
			{
				addresses = value;
			}
		}

		public Rs485Port(string portName, BaudRate baudRate, byte[] addresses)
			: this("", addresses)
		{
			connStr = portName + "," + baudRate.ToString().Substring(1);
		}

		public Rs485Port(string connStr, byte[] addresses)
			: base(connStr)
		{
			port = PortType.RS485;
			this.addresses = addresses;
		}
	}
}
