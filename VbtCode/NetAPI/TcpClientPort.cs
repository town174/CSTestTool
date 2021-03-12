namespace NetAPI
{
	public class TcpClientPort : IPort
	{
		public TcpClientPort(string IP, int port)
			: this("")
		{
			connStr = IP + ":" + port.ToString();
		}

		public TcpClientPort(string connStr)
			: base(connStr)
		{
			port = PortType.TcpClient;
		}
	}
}
