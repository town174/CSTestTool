using System;

namespace NetAPI
{
	public class LogChangeEventArgs : EventArgs
	{
		private string logContent;

		private string logTime;

		private string logManagedThreadId;

		private string logFlag;

		public string LogContent
		{
			get
			{
				return logContent;
			}
			set
			{
				logContent = value;
			}
		}

		public string LogTime
		{
			get
			{
				return logTime;
			}
			set
			{
				logTime = value;
			}
		}

		public string LogManagedThreadId
		{
			get
			{
				return logManagedThreadId;
			}
			set
			{
				logManagedThreadId = value;
			}
		}

		public string LogFlag
		{
			get
			{
				return logFlag;
			}
			set
			{
				logFlag = value;
			}
		}

		public LogChangeEventArgs(string logContent, string logTime, string logManagedThreadId, string logFlag)
		{
			this.logContent = logContent;
			this.logTime = logTime;
			this.logManagedThreadId = logManagedThreadId;
			this.logFlag = logFlag;
		}
	}
}
