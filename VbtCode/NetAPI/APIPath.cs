using System;
using System.IO;
using System.Reflection;

namespace NetAPI
{
	public class APIPath
	{
		public static readonly string folderName = getPath();

		private APIPath()
		{
		}

		private static string getPath()
		{
			string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			PlatformID platform = Environment.OSVersion.Platform;
			if (platform == PlatformID.WinCE)
			{
				return directoryName;
			}
			return directoryName.Substring(6);
		}
	}
}
