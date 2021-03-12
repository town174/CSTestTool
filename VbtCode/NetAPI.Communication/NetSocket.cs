using NetAPI.Core;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace NetAPI.Communication
{
	public abstract class NetSocket : ICommunication
	{
		private int portNum = 9090;

		private string hostName = "192.168.1.100";

		private string localIP = "";

		private int localPort = 0;

		private Thread threadRun = null;

		private BinaryReader reader;

		private BinaryWriter writer;

		private object lockObj = new object();

		private byte[] inOptionValues;

		protected new Socket socket;

		protected TcpServer tcpServer;

		private AutoResetEvent closeEvent = null;

		private volatile bool threadIsAlive = false;

		public NetSocket()
		{
			uint num = 0u;
			inOptionValues = new byte[Marshal.SizeOf((object)num) * 3];
			BitConverter.GetBytes(1u).CopyTo(inOptionValues, 0);
			BitConverter.GetBytes(5000u).CopyTo(inOptionValues, Marshal.SizeOf((object)num));
			BitConverter.GetBytes(1000u).CopyTo(inOptionValues, Marshal.SizeOf((object)num) * 2);
		}

		public NetSocket(Socket socket)
			: this()
		{
			this.socket = socket;
			IPEndPoint iPEndPoint = (IPEndPoint)socket.RemoteEndPoint;
			hostName = iPEndPoint.Address.ToString();
			portNum = iPEndPoint.Port;
		}

		protected virtual void Open()
		{
			using (Ping ping = new Ping())
			{
				PingReply pingReply = null;
				try
				{
					pingReply = ping.Send(hostName, 1000);
					if (pingReply.Status != 0)
					{
						isConnected = false;
						Util.logAndTriggerApiErr(readerName, "FF01", "Ping不通", LogType.Fatal);
						return;
					}
				}
				catch (Exception ex)
				{
					Log.Info("建立连接失败！" + hostName + "：" + ex.Message);
					return;
				}
			}
			try
			{
				if (socket == null)
				{
					if (string.IsNullOrEmpty(localIP))
					{
						socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					}
					else
					{
						socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
						IPAddress address = IPAddress.Parse(localIP);
						IPEndPoint localEP = new IPEndPoint(address, localPort);
						socket.Bind(localEP);
					}
				}
				if (!socket.Connected)
				{
					IPAddress address2 = IPAddress.Parse(hostName);
					IPEndPoint remoteEP = new IPEndPoint(address2, portNum);
					socket.Connect(remoteEP);
				}
				if (!socket.Connected)
				{
					socket = null;
					return;
				}
				socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
			}
			catch (SocketException ex2)
			{
				isConnected = false;
				socket.Close();
				Util.logAndTriggerApiErr(readerName, "FF01", ex2.Message, LogType.Fatal);
				return;
			}
			catch (Exception ex3)
			{
				isConnected = false;
				socket.Close();
				Util.logAndTriggerApiErr(readerName, "FF01", ex3.Message, LogType.Fatal);
				return;
			}
			if (threadRun == null)
			{
				threadRun = new Thread(RunClient);
				threadRun.Start();
			}
			Thread.Sleep(50);
		}

		public override bool Open(string connstring)
		{
			if (socket == null)
			{
				string[] array = null;
				array = ((connstring.IndexOf(';') == -1) ? new string[1]
				{
					connstring
				} : connstring.Split(';'));
				if (array[0].IndexOf(':') != -1)
				{
					hostName = array[0].Substring(0, array[0].IndexOf(':'));
					portNum = int.Parse(array[0].Substring(array[0].IndexOf(':') + 1));
				}
				else
				{
					hostName = array[0];
				}
				if (array.Length > 1)
				{
					if (array[1].IndexOf(':') != -1)
					{
						localIP = array[1].Substring(0, array[1].IndexOf(':'));
						localPort = int.Parse(array[1].Substring(array[1].IndexOf(':') + 1));
					}
					else
					{
						localIP = array[1];
					}
				}
			}
			Open();
			return isConnected;
		}

		public override void Close()
		{
			isConnected = false;
			closeEvent = new AutoResetEvent(initialState: false);
			if (threadIsAlive)
			{
				closeEvent.WaitOne(1000, exitContext: false);
			}
			if (writer != null)
			{
				writer.Close();
			}
			if (reader != null)
			{
				reader.Close();
			}
			if (socket != null)
			{
				socket.Close();
			}
			threadRun = null;
			closeEvent = null;
			Log.Debug(readerName + " closed.");
		}

		public override int Send(byte[] data)
		{
			if (socket == null || (socket != null && !socket.Connected))
			{
				isConnected = false;
				Util.logAndTriggerApiErr(readerName, "FF04", "", LogType.Warn);
				return 0;
			}
			int result = 0;
			if (data != null)
			{
				try
				{
					send(data);
					result = data.Length;
				}
				catch (Exception ex)
				{
					Util.logAndTriggerApiErr(readerName, "FF01", ex.Message, LogType.Error);
					result = 0;
				}
			}
			else
			{
				Util.logAndTriggerApiErr(readerName, "FF05", "", LogType.Info);
			}
			return result;
		}

		private void RunClient()
		{
			threadIsAlive = true;
			try
			{
				using (NetworkStream networkStream = new NetworkStream(socket))
				{
					writer = new BinaryWriter(networkStream);
					reader = new BinaryReader(networkStream);
					int num = 1024;
					byte[] array = new byte[num];
					int num2 = 0;
					byte[] array2 = null;
					isConnected = socket.Connected;
					Log.Debug(readerName + " is running.Connected:" + isConnected.ToString());
					while (isConnected)
					{
						num2 = reader.Read(array, 0, num);
						if (num2 <= 0)
						{
							isConnected = false;
							Util.logAndTriggerApiErr(readerName, "FF02", "", LogType.Warn);
							break;
						}
						array2 = new byte[num2];
						Array.Copy(array, 0, array2, 0, num2);
						Log.Debug("TCP RXD:[" + Util.ConvertbyteArrayToHexstring(array2) + "]");
						base.BufferQueue = array2;
					}
				}
			}
			catch (ThreadAbortException ex)
			{
				isConnected = false;
				Util.logAndTriggerApiErr(readerName, "FF02", ex.Message, LogType.Warn);
			}
			catch (SocketException ex2)
			{
				isConnected = false;
				Util.logAndTriggerApiErr(readerName, "FF02", ex2.Message, LogType.Warn);
			}
			catch (IOException ex3)
			{
				isConnected = false;
				Util.logAndTriggerApiErr(readerName, "FF02", ex3.Message, LogType.Warn);
			}
			catch (Exception ex4)
			{
				isConnected = false;
				Util.logAndTriggerApiErr(readerName, "FF02", ex4.Message, LogType.Warn);
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
				}
				if (reader != null)
				{
					reader.Close();
				}
				if (socket != null)
				{
					socket.Close();
				}
				isConnected = false;
				if (closeEvent != null)
				{
					closeEvent.Set();
				}
				threadIsAlive = false;
				if (tcpServer != null)
				{
					tcpServer.DelSocket(readerName);
				}
			}
			Log.Info(readerName + " RunClient stoped.");
		}

		private void send(byte[] sendMsg)
		{
			try
			{
				lock (lockObj)
				{
					writer.Write(sendMsg, 0, sendMsg.Length);
				}
			}
			catch (SocketException ex)
			{
				throw ex;
			}
			catch (Exception ex2)
			{
				throw ex2;
			}
		}

		~NetSocket()
		{
			Close();
		}

		public override void Dispose()
		{
			lock (this)
			{
				Close();
			}
			GC.SuppressFinalize(this);
			base.Dispose();
		}
	}
}
