using NetAPI.Communication;
using System;
using System.Net.Sockets;

namespace NetAPI.Core
{
	public abstract class AbstractReaders : IDisposable
	{
		public bool IsConnected = false;

		public string ReadersName = "Readers1";

		private TcpServer tcpServer;

		public IPort CommPort
		{
			get;
			set;
		}

		public AbstractReaders(string readersName, IPort commPort)
		{
			ReadersName = readersName;
			CommPort = commPort;
		}

		public AbstractReaders(string readersName)
		{
			ReadersName = readersName;
			DeviceCfg deviceCfg = new DeviceCfg();
			DeviceCfgItem deviceCfgItem = deviceCfg.FindReaderItem(readersName);
			if (deviceCfgItem != null)
			{
				PortType portType = (PortType)Enum.Parse(typeof(PortType), deviceCfgItem.PortType);
				PortType portType2 = portType;
				if (portType2 == PortType.TcpServer)
				{
					CommPort = new TcpServerPort(deviceCfgItem.ConnStr);
				}
			}
		}

		public bool Start(out ErrInfo err)
		{
			err = new ErrInfo("FF01", "");
			if (CommPort == null)
			{
				err = new ErrInfo("FF01", "端口类型错误");
				return false;
			}
			PortType port = CommPort.Port;
			PortType portType = port;
			if (portType == PortType.TcpServer)
			{
				tcpServer = new TcpServer(int.Parse(CommPort.ConnStr));
				tcpServer.OnAddSocket += TcpServer_OnAddSocket;
				tcpServer.OnDelSocket += TcpServer_OnDelSocket;
				IsConnected = tcpServer.Start(out err);
				if (!IsConnected)
				{
					tcpServer.Stop();
					tcpServer = null;
				}
			}
			else
			{
				err = new ErrInfo("FF01", "端口类型错误");
			}
			return IsConnected;
		}

		protected abstract void AddReader(Socket socket);

		private void TcpServer_OnAddSocket(Socket socket)
		{
			AddReader(socket);
		}

		protected abstract void DelReader(string readersName);

		private void TcpServer_OnDelSocket(string readerName)
		{
			DelReader(readerName);
		}

		public virtual void Stop()
		{
			stop();
		}

		protected void stop()
		{
			if (IsConnected)
			{
				if (tcpServer != null)
				{
					tcpServer.OnAddSocket -= TcpServer_OnAddSocket;
					tcpServer.OnDelSocket -= TcpServer_OnDelSocket;
					tcpServer.Stop();
					tcpServer = null;
				}
				IsConnected = false;
			}
		}

		public void Dispose()
		{
			if (IsConnected)
			{
				Stop();
			}
		}
	}
}
