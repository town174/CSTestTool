using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;

namespace NetAPI
{
	internal class LogManager
	{
		private static string folderName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);

		private string logName = folderName + "\\Debug.log";

		private string bakName = folderName + "\\Debug.bak";

		private FileInfo filog = null;

		public static readonly LogManager instance = new LogManager();

		private object lockObj = new object();

		private LogManager()
		{
			if (File.Exists(folderName + "\\DeviceCfg.xml"))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(folderName + "\\DeviceCfg.xml");
				XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("LogName");
				if (xmlNode != null)
				{
					logName = folderName + "\\" + xmlNode.InnerText + ".log";
					bakName = folderName + "\\" + xmlNode.InnerText + ".bak";
				}
			}
			reset();
		}

		private void reset()
		{
			try
			{
				if (!File.Exists(logName))
				{
					StreamWriter streamWriter = new StreamWriter(logName, false, Encoding.UTF8);
					streamWriter.WriteLine("RFID Log");
					streamWriter.Flush();
					streamWriter.Close();
				}
				filog = new FileInfo(logName);
			}
			catch (Exception)
			{
			}
		}

		public void write(string msg, string logFlag)
		{
			lock (lockObj)
			{
				string text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff");
				string text2 = Thread.CurrentThread.ManagedThreadId.ToString() ?? "";
				WriteLog(text + " [" + text2 + "] " + logFlag + " - " + msg);
				filog.Refresh();
				Log.OnLogChangedMethod(new LogChangeEventArgs(msg, text, text2, logFlag));
				if (filog.Length > 2097152)
				{
					if (File.Exists(bakName))
					{
						File.Delete(bakName);
					}
					filog.MoveTo(bakName);
					reset();
				}
			}
		}

		public void WriteLog(string msg)
		{
			try
			{
				reset();
				StreamWriter streamWriter = File.AppendText(logName);
				streamWriter.WriteLine(msg);
				streamWriter.Flush();
				streamWriter.Close();
			}
			catch
			{
			}
		}
	}
}
