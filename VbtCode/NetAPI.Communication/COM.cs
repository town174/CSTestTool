using NetAPI.Core;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace NetAPI.Communication
{
	public class COM : ICommunication
	{
		private SerialPort MyCom;

		private object lockObj = new object();

		private string portName = "COM1";

		private int baudRate = 115200;

		private Parity parity = Parity.None;

		private int dataBits = 8;

		private StopBits stopBits = StopBits.One;

		public override bool Open(string connstring)
		{
			string[] array = connstring.Split(',');
			if (array.Length >= 1)
			{
				portName = array[0];
			}
			if (array.Length >= 2)
			{
				baudRate = int.Parse(array[1]);
			}
			if (array.Length >= 3)
			{
				string text = array[2];
				switch (text)
				{
				case "None":
					parity = Parity.None;
					break;
				case "Odd":
					parity = Parity.Odd;
					break;
				case "Even":
					parity = Parity.Even;
					break;
				case "Mark":
					parity = Parity.Mark;
					break;
				case "Space":
					parity = Parity.Space;
					break;
				}
			}
			if (array.Length >= 4)
			{
				dataBits = int.Parse(array[3]);
			}
			if (array.Length >= 5)
			{
				string text2 = array[4];
				switch (text2)
				{
				case "None":
					stopBits = StopBits.None;
					break;
				case "1":
					stopBits = StopBits.One;
					break;
				case "1.5":
					stopBits = StopBits.OnePointFive;
					break;
				case "2":
					stopBits = StopBits.Two;
					break;
				}
			}
			MyCom = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
			MyCom.DataReceived += MyCom_DataReceived;
			MyCom.ErrorReceived += XCCom_ErrorReceived;
			try
			{
				MyCom.Open();
				for (int i = 0; i < 100; i++)
				{
					if (MyCom.IsOpen)
					{
						isConnected = true;
						return true;
					}
					Thread.Sleep(5);
				}
			}
			catch (UnauthorizedAccessException ex)
			{
				throw ex;
			}
			catch (IOException ex2)
			{
				throw ex2;
			}
			catch (Exception ex3)
			{
				throw ex3;
			}
			Close();
			return false;
		}

		private void XCCom_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
			Log.Error("COM:" + e.EventType.ToString());
		}

		private void MyCom_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			int bytesToRead = MyCom.BytesToRead;
			byte[] array = new byte[bytesToRead];
			int num = 0;
			try
			{
				num = MyCom.Read(array, 0, bytesToRead);
			}
			catch (IOException ex)
			{
				Log.Error("MyCom_DataReceived:" + ex.Message);
			}
			if (num > 0)
			{
				byte[] array2 = new byte[num];
				Array.Copy(array, array2, num);
				base.BufferQueue = array2;
				Log.Debug("COM:RXD[" + Util.ConvertbyteArrayToHexstring(array2) + "]");
			}
		}

		public override int Send(byte[] data)
		{
			if (MyCom == null || (MyCom != null && !MyCom.IsOpen))
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
						MyCom.Write(data, 0, data.Length);
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
			if (MyCom != null)
			{
				MyCom.DataReceived -= MyCom_DataReceived;
				MyCom.ErrorReceived -= XCCom_ErrorReceived;
				try
				{
					MyCom.Close();
				}
				catch (IOException ex)
				{
					Log.Error("MyComClose:" + ex.Message);
				}
			}
		}

		~COM()
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
