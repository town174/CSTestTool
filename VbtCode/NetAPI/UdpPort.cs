using System.Net;

namespace NetAPI
{
	public class UdpPort : IPort
	{
		public UdpPort(int recvPort, IPEndPoint remotePoint, int remotePort = 9091)
			: this("")
		{
			connStr = recvPort.ToString() + "," + remotePoint.ToString() + ":" + remotePort.ToString();
		}

		public UdpPort(string connStr)
			: base(connStr)
		{
			port = PortType.UDP;
		}
	}
}
