namespace NetAPI
{
	public class TcpServerPort : IPort
	{
		public TcpServerPort(int port)
			: this("")
		{
			connStr = port.ToString();
		}

		public TcpServerPort(string connStr)
			: base(connStr)
		{
			port = PortType.TcpServer;
		}
	}
}
