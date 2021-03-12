using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace NetAPI.Core
{
	public abstract class ICommunication : IDisposable
	{
		public class messageInfo
		{
			public AutoResetEvent ev = null;

			public IHostMessage msg = null;

			public volatile bool isDone = false;
		}

		protected internal string readerName;

		protected internal IProcess iProcess;

		protected internal volatile bool isConnected;

		protected internal Socket socket;

		private messageInfo info;

		private object lockEvent = new object();

		private object lockinfo = new object();

		private volatile bool isReturnReadTag = false;

		private volatile bool isGetOneTag = false;

		private object lockQBufferObj = new object();

		private Queue<byte[]> qBuffer = new Queue<byte[]>();

		protected internal Thread threadProcess;

		protected byte[] BufferQueue
		{
			get
			{
				lock (lockQBufferObj)
				{
					byte[] result = null;
					if (qBuffer == null || qBuffer.Count == 0)
					{
						Monitor.Wait(lockQBufferObj);
					}
					if (qBuffer == null)
					{
						qBuffer = new Queue<byte[]>();
					}
					try
					{
						if (qBuffer != null && qBuffer.Count > 0)
						{
							result = qBuffer.Dequeue();
						}
					}
					catch (InvalidOperationException ex)
					{
						Log.Error(ex.Message);
						return new byte[0];
					}
					catch (Exception ex2)
					{
						Log.Error(ex2.Message);
						return new byte[0];
					}
					Monitor.Pulse(lockQBufferObj);
					return result;
				}
			}
			set
			{
				lock (lockQBufferObj)
				{
					if (qBuffer == null)
					{
						qBuffer = new Queue<byte[]>();
					}
					if (qBuffer.Count >= 1024)
					{
						Monitor.Wait(lockQBufferObj);
					}
					qBuffer.Enqueue(value);
					Monitor.Pulse(lockQBufferObj);
				}
			}
		}

		internal event MsgReceivedHandle OnMsgReceived;

		internal event BuffReceivedHandle OnBuffReceived;

		public abstract bool Open(string connstring);

		public abstract int Send(byte[] data);

		public abstract void Close();

		internal bool Send(IHostMessage msg, int timeout)
		{
			if (msg == null)
			{
				Log.Debug("ICommunication:" + ErrInfoList.ErrDictionary["FF05"]);
				return false;
			}
			bool flag = false;
			if (msg.IsReturn)
			{
				lock (lockEvent)
				{
					lock (lockinfo)
					{
						info = new messageInfo();
						info.msg = msg;
						info.ev = new AutoResetEvent(initialState: false);
					}
					byte[] transmitterData = msg.TransmitterData;
					try
					{
						Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]发送数据：" + Util.ConvertbyteArrayToHexWordstring(transmitterData));
					}
					catch (Exception ex)
					{
						Log.Error(ex.Message);
					}
					Log.Debug("TXD:[" + Util.ConvertbyteArrayToHexstring(transmitterData) + "]");
					int num = Send(transmitterData);
					if (num > 0 && num == transmitterData.Length)
					{
						flag = info.ev.WaitOne(timeout, exitContext: true);
						if (flag)
						{
							if (msg.Status != 0)
							{
								flag = false;
							}
						}
						else if (!isReturnReadTag)
						{
							msg.Status = MsgStatus.Timeout;
						}
					}
					lock (lockinfo)
					{
						info = null;
					}
				}
			}
			else
			{
				byte[] transmitterData2 = msg.TransmitterData;
				Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]发送数据：" + Util.ConvertbyteArrayToHexWordstring(transmitterData2));
				Log.Debug("TXD:[" + Util.ConvertbyteArrayToHexstring(transmitterData2) + "]");
				int num2 = Send(transmitterData2);
				if (num2 > 0 && num2 == transmitterData2.Length)
				{
					flag = true;
				}
			}
			if (isReturnReadTag)
			{
				isReturnReadTag = false;
			}
			if (isGetOneTag)
			{
				isGetOneTag = false;
			}
			return flag;
		}

		internal void process()
		{
			while (isConnected)
			{
				List<byte[]> msgs = null;
				byte[] bufferQueue = BufferQueue;
				iProcess.Parse(bufferQueue, out msgs);
				if (msgs != null && msgs.Count > 0)
				{
					foreach (byte[] item in msgs)
					{
						try
						{
							Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]接收数据：" + Util.ConvertbyteArrayToHexWordstring(item));
							Console.WriteLine("");
						}
						catch (Exception ex)
						{
							Log.Error(ex.Message);
						}
						lock (lockinfo)
						{
							if (info != null && !info.isDone && iProcess.GetMessageID(item) == info.msg.MessageID)
							{
								info.msg.ReceivedData = item;
								info.isDone = true;
								info.ev.Set();
								continue;
							}
						}
						IReaderMessage readerMessage = null;
						try
						{
							readerMessage = iProcess.ParseMessageNotification(item);
							if (this.OnMsgReceived != null && readerMessage != null)
							{
								this.OnMsgReceived.BeginInvoke(readerMessage, null, null);
							}
						}
						catch (Exception ex2)
						{
							string text = "";
							if (readerMessage != null)
							{
								text = readerMessage.MessageID.ToString("X4");
							}
							if (text.Length % 2 == 1)
							{
								text = "0" + text;
							}
							Log.Error("ICommunication:" + ex2.Message + "|MsgID:" + text);
						}
					}
				}
				if (this.OnBuffReceived != null)
				{
					this.OnBuffReceived(bufferQueue);
				}
			}
			Log.Info("ICommunication:线程退出");
		}

		public virtual void Dispose()
		{
		}
	}
}
