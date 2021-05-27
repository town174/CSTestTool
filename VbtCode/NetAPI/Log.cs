using System.IO;
using System.Reflection;
using System.Xml;

namespace NetAPI
{
	public class Log
	{
		public static string LogLevel;

		private static string folderName;

		public static event LogChangeDelegate OnLogChanged;

		static Log()
		{
			LogLevel = "DEBUG";
			folderName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
			if (File.Exists(folderName + "\\DeviceCfg.xml"))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(folderName + "\\DeviceCfg.xml");
				XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("LogLevel");
				if (xmlNode != null)
				{
					LogLevel = xmlNode.InnerText;
					if (!(LogLevel.ToUpper() != "DEBUG") && !(LogLevel.ToUpper() != "INFO") && !(LogLevel.ToUpper() != "WARN") && !(LogLevel.ToUpper() != "ERROR") && !(LogLevel.ToUpper() != "FATAL"))
					{
						LogLevel = "INFO";
					}
				}
			}
		}

		public static void Debug(string logStr)
		{
			if (LogLevel.ToUpper() == "DEBUG")
			{
				LogManager.instance.write(logStr, "DEBUG");
			}
		}

		public static void Info(string logStr)
		{
			if (LogLevel.ToUpper() == "DEBUG" || LogLevel.ToUpper() == "INFO")
			{
				LogManager.instance.write(logStr, "INFO");
			}
		}

		public static void Warn(string logStr)
		{
			if (LogLevel.ToUpper() == "DEBUG" || LogLevel.ToUpper() == "INFO" || LogLevel.ToUpper() == "WARN")
			{
				LogManager.instance.write(logStr, "WARN");
			}
		}

		public static void Error(string logStr)
		{
			if (LogLevel.ToUpper() == "DEBUG" || LogLevel.ToUpper() == "INFO" || LogLevel.ToUpper() == "WARN" || LogLevel.ToUpper() == "ERROR")
			{
				LogManager.instance.write(logStr, "ERROR");
			}
		}

		public static void Fatal(string logStr)
		{
			if (LogLevel.ToUpper() == "DEBUG" || LogLevel.ToUpper() == "INFO" || LogLevel.ToUpper() == "WARN" || LogLevel.ToUpper() == "ERROR" || LogLevel.ToUpper() == "FATAL")
			{
				LogManager.instance.write(logStr, "FATAL");
			}
		}

		internal static void OnLogChangedMethod(LogChangeEventArgs arg)
		{
			if (Log.OnLogChanged != null)
			{
				Log.OnLogChanged(arg);
			}
		}
	}
}
