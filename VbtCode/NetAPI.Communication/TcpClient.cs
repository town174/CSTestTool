using System.Net.Sockets;

namespace NetAPI.Communication
{
	public class TcpClient : NetSocket
	{
		public TcpClient()
		{
		}

		public TcpClient(Socket socket)
			: base(socket)
		{
		}
	}
}
