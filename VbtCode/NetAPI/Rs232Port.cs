namespace NetAPI
{
	public class Rs232Port : IPort
	{
		public Rs232Port(string portName, BaudRate baudRate)
			: this("")
		{
			connStr = portName + "," + baudRate.ToString().Substring(1);
		}

		public Rs232Port(string connStr)
			: base(connStr)
		{
			port = PortType.RS232;
		}
	}
}
