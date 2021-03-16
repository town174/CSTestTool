using NetAPI.Communication;
using NetAPI.Protocol.VRP;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace NetAPI.Core
{
	public abstract class AbstractReader : IDisposable
	{
		public string ReaderName = "Device1";

		protected ICommunication iComm;

		private bool isExistReaderConfig = true;

		public bool IsConnected
		{
			get
			{
				bool result = false;
				if (iComm != null)
				{
					result = iComm.isConnected;
				}
				return result;
			}
		}

		public IPort CommPort
		{
			get;
			set;
		}

		protected virtual IHostMessage ConnectMessage => null;

		protected virtual IHostMessage DisconnectMessage => null;

		public event ReaderMessageReceivedHandle OnReaderMessageReceived;

		public static event ApiExceptionHandle OnApiException;

		public event BufferReceivedHandle OnBufferReceived;

		public static event ErrorMessageHandle OnErrorMessage;

		protected void setIsConnected(bool isConnected)
		{
			if (iComm != null)
			{
				iComm.isConnected = isConnected;
			}
		}

		private void iConn_OnMsgReceived(IReaderMessage e)
		{
			if (this.OnReaderMessageReceived != null)
			{
				this.OnReaderMessageReceived(this, e);
			}
		}

		protected internal static void TrigerApiException(string senderName, ErrInfo e)
		{
			if (AbstractReader.OnApiException != null)
			{
				AbstractReader.OnApiException(senderName, e);
			}
		}

		private void iConn_OnBuffReceived(byte[] e)
		{
			if (this.OnBufferReceived != null)
			{
				this.OnBufferReceived(this, e);
			}
		}

		public static void TrigerErrorMessage(IReaderMessage e)
		{
			if (AbstractReader.OnErrorMessage != null)
			{
				AbstractReader.OnErrorMessage(e);
			}
		}

		public AbstractReader(string readerName, IPort commPort)
		{
			ReaderName = readerName;
			CommPort = commPort;
		}

		public AbstractReader(string readerName)
		{
			ReaderName = readerName;
		}

		protected AbstractReader(Socket socket)
		{
			IPEndPoint iPEndPoint = (IPEndPoint)socket.RemoteEndPoint;
			ReaderName = iPEndPoint.Address.ToString() + ":" + iPEndPoint.Port.ToString();
			CommPort = new TcpClientPort(iPEndPoint.ToString());
            //Type type = Type.GetType("NetAPI.Communication.TcpClient", throwOnError: true);
            //iComm = (ICommunication)Activator.CreateInstance(type, socket);
            iComm = new Communication.TcpClient();
            iComm.readerName = ReaderName;
		}

		protected bool Connect(out ErrInfo err)
		{
			err = new ErrInfo("FF01", "");
			if (iComm == null)
			{
				try
				{
					string text = CommPort.Port.ToString();
					if (text == "RS232" || text == "RS485")
					{
                        //text = "COM";
                        iComm = new COM();
                    }
                    else
                    {
                        iComm = new NetAPI.Communication.TcpClient();
                    }
					//Type type = Type.GetType("NetAPI.Communication." + text, throwOnError: true);
					//iComm = (ICommunication)Activator.CreateInstance(type);
					iComm.readerName = ReaderName;
				}
				catch (Exception ex)
				{
					err = new ErrInfo("FF01", ex.Message);
					Log.Fatal(CommPort.Port.ToString() + ":" + ex.Message);
					return false;
				}
			}
			if (iComm != null)
			{
                //Assembly assembly = Assembly.LoadFrom(APIPath.folderName + "\\API.dll");
                //Type type2 = assembly.GetType("NetAPI.Protocol.VRP.Decode", throwOnError: true);
                //iComm.iProcess = (IProcess)Activator.CreateInstance(type2);
                iComm.iProcess = new Decode();
                try
				{
					if (iComm.Open(CommPort.ConnStr))
					{
						iComm.OnMsgReceived += iConn_OnMsgReceived;
						iComm.OnBuffReceived += iConn_OnBuffReceived;
						iComm.threadProcess = new Thread(iComm.process);
						iComm.threadProcess.Start();
						if (ConnectMessage != null && !(CommPort is UdpPort))
						{
							bool flag = false;
							for (int i = 0; i < 2; i++)
							{
								IHostMessage connectMessage = ConnectMessage;
								bool flag2 = Send(connectMessage);
								if (connectMessage.Status != MsgStatus.Timeout)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								err = new ErrInfo("FF01", "协议消息测试失败");
								Disconnect();
								return false;
							}
						}
						return true;
					}
				}
				catch (Exception ex2)
				{
					err = new ErrInfo("FF01", ex2.Message);
					Log.Error("iComm:" + ex2.Message);
					return false;
				}
			}
			return false;
		}

		public void Disconnect()
		{
			if (iComm != null)
			{
				if (iComm.isConnected && DisconnectMessage != null && !(CommPort is UdpPort))
				{
					iComm.isConnected = false;
					IHostMessage disconnectMessage = DisconnectMessage;
					Send(disconnectMessage, 500);
					Thread.Sleep(50);
				}
				iComm.isConnected = false;
			}
			if (iComm != null)
			{
				iComm.Close();
				iComm.OnMsgReceived -= iConn_OnMsgReceived;
				iComm.OnBuffReceived -= iConn_OnBuffReceived;
			}
			if (iComm != null && iComm.threadProcess != null && iComm.threadProcess.ThreadState != ThreadState.Stopped)
			{
				try
				{
					iComm.threadProcess.Abort();
				}
				catch
				{
				}
			}
			iComm = null;
		}

		public bool Send(byte[] msg)
		{
			if (iComm == null || msg == null)
			{
				return false;
			}
			int num = iComm.Send(msg);
			if (num == msg.Length)
			{
				return true;
			}
			return false;
		}

		public virtual bool Send(IHostMessage msg)
		{
			if (msg == null)
			{
				return false;
			}
			try
			{
				msg.TrigerOnExecuting(this);
			}
			catch (Exception ex)
			{
				Log.Debug(ReaderName + ":" + ex.Message);
				return false;
			}
			if (iComm == null)
			{
				return false;
			}
			bool result = iComm.Send(msg, msg.Timeout);
			msg.TrigerOnExecuted(this);
			return result;
		}

		public bool Send(IHostMessage msg, int timeout)
		{
			msg.Timeout = timeout;
			return Send(msg);
		}

		public void Dispose()
		{
			if (iComm != null)
			{
				Disconnect();
			}
		}
	}
}
