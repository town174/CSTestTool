using NetAPI.Core;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetAPI.Communication
{
	public class TcpServer
	{
		private Socket ServerSocket = null;

		private bool Flag_Listen = true;

		private int port;

		internal event OnAddSocketHandle OnAddSocket;

		internal event OnDelSocketHandle OnDelSocket;

		public TcpServer(int port)
		{
			this.port = port;
		}

		public bool Start(out ErrInfo err)
		{
			err = new ErrInfo("FF01", "");
			try
			{
				Flag_Listen = true;
				ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
				try
				{
					ServerSocket.Bind(localEP);
				}
				catch (Exception ex)
				{
					err = new ErrInfo("FF01", ex.Message);
					Log.Fatal("TcpServer:" + ex.Message);
					return false;
				}
				ServerSocket.Listen(20);
				Thread thread = new Thread(ListenConnecting);
				thread.IsBackground = true;
				thread.Start();
				return true;
			}
			catch (Exception ex2)
			{
				err = new ErrInfo("FF01", ex2.Message);
				Log.Fatal("TcpServer:" + ex2.Message);
				return false;
			}
		}

		public void Stop()
		{
			if (ServerSocket != null)
			{
				Flag_Listen = false;
				if (ServerSocket != null)
				{
					ServerSocket.Close();
				}
				ServerSocket = null;
			}
		}

		private void ListenConnecting()
		{
			while (Flag_Listen)
			{
				try
				{
					Socket socket = ServerSocket.Accept();
					string text = socket.RemoteEndPoint.ToString();
					if (this.OnAddSocket != null)
					{
						this.OnAddSocket(socket);
					}
				}
				catch (Exception ex)
				{
					Log.Error("服务端监听错误：" + ex.Message);
				}
				Thread.Sleep(100);
			}
		}

		internal void DelSocket(string readerName)
		{
			if (this.OnDelSocket != null)
			{
				this.OnDelSocket(readerName);
			}
		}
	}
}
