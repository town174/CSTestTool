using NetAPI.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetAPI.Communication
{
	public class UDP : ICommunication
	{
		private object lockObj = new object();

		private object lockServer = new object();

		private List<IPEndPoint> remotePoints = new List<IPEndPoint>();

		private int localPort = 9091;

		private int remotePort = 9091;

		private Socket udpSever;

		private Thread tRecv;

		public override bool Open(string connstring)
		{
			if (!string.IsNullOrEmpty(connstring))
			{
				string[] array = connstring.Split(',');
				localPort = int.Parse(array[0]);
				if (array.Length > 1)
				{
					string[] array2 = array[1].Split('-');
					string str = array2[0].Substring(0, array2[0].LastIndexOf('.'));
					string s = array2[0].Substring(array2[0].LastIndexOf('.') + 1);
					int num = int.Parse(s);
					int num2 = num;
					if (array2.Length > 1)
					{
						num2 = int.Parse(array2[1]);
					}
					remotePoints.Clear();
					for (int i = num; i <= num2; i++)
					{
						IPAddress address = IPAddress.Parse(str + "." + i.ToString());
						remotePoints.Add(new IPEndPoint(address, remotePort));
					}
				}
			}
			try
			{
				lock (lockServer)
				{
					udpSever = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					udpSever.Bind(new IPEndPoint(IPAddress.Any, localPort));
				}
				tRecv = new Thread(RecvThread);
				tRecv.Start();
				isConnected = true;
				return true;
			}
			catch (Exception ex)
			{
				Log.Fatal("UDP连接错误:" + ex.Message);
			}
			Close();
			return false;
		}

		private void RecvThread()
		{
			EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
			while (isConnected)
			{
				Thread.Sleep(50);
				try
				{
					lock (lockServer)
					{
						ArrayList arrayList = new ArrayList();
						if (udpSever != null)
						{
							arrayList.Add(udpSever);
							Socket.Select(arrayList, null, null, 100);
							for (int i = 0; i < arrayList.Count; i++)
							{
								Socket socket = (Socket)arrayList[i];
								int num = 1024;
								byte[] array = new byte[num];
								int num2 = 0;
								byte[] array2 = null;
								num2 = socket.ReceiveFrom(array, ref remoteEP);
								if (num2 <= 0)
								{
									isConnected = false;
									Util.logAndTriggerApiErr(readerName, "FF02", "", LogType.Warn);
									break;
								}
								array2 = new byte[num2];
								Array.Copy(array, 0, array2, 0, num2);
								Log.Debug("UDP RXD:[" + Util.ConvertbyteArrayToHexstring(array2) + "]");
								base.BufferQueue = array2;
							}
						}
					}
				}
				catch (SocketException ex)
				{
					Close();
					Util.logAndTriggerApiErr(readerName, "FF01", ex.Message, LogType.Fatal);
					return;
				}
			}
		}

		public override int Send(byte[] data)
		{
			if (udpSever == null && isConnected)
			{
				isConnected = false;
				Util.logAndTriggerApiErr(readerName, "FF04", "", LogType.Warn);
				return 0;
			}
			if (data != null)
			{
				lock (lockObj)
				{
					try
					{
						foreach (IPEndPoint remotePoint in remotePoints)
						{
							udpSever.SendTo(data, remotePoint);
						}
						return data.Length;
					}
					catch (Exception ex)
					{
						Util.logAndTriggerApiErr(readerName, "FF01", ex.Message, LogType.Error);
					}
				}
			}
			else
			{
				Util.logAndTriggerApiErr(readerName, "FF05", "", LogType.Info);
			}
			return 0;
		}

		public override void Close()
		{
			isConnected = false;
			try
			{
				lock (lockServer)
				{
					if (udpSever != null)
					{
						udpSever.Close();
					}
					udpSever = null;
				}
			}
			catch (Exception ex)
			{
				Log.Error("UDP Close:" + ex.Message);
			}
		}

		~UDP()
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
