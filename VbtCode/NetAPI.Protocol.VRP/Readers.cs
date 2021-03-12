using NetAPI.Core;
using System.Collections.Generic;
using System.Net.Sockets;

namespace NetAPI.Protocol.VRP
{
	public class Readers : AbstractReaders
	{
		public Dictionary<string, Reader> ReaderDic = new Dictionary<string, Reader>();

		public event OnAddReaderHandle OnAddReader;

		public event OnDelReaderHandle OnDelReader;

		public Readers(string readersName, IPort commPort)
			: base(readersName, commPort)
		{
		}

		public Readers(string readersName)
			: base(readersName)
		{
		}

		protected override void AddReader(Socket socket)
		{
			Reader reader = new Reader(socket);
			ReaderDic.Add(reader.ReaderName, reader);
			if (this.OnAddReader != null)
			{
				this.OnAddReader(this, reader);
			}
		}

		protected override void DelReader(string readersName)
		{
			if (ReaderDic.ContainsKey(readersName))
			{
				ReaderDic[readersName].Disconnect();
				ReaderDic.Remove(readersName);
			}
			if (this.OnDelReader != null)
			{
				this.OnDelReader(this, readersName);
			}
		}

		public override void Stop()
		{
			if (ReaderDic != null && ReaderDic.Count > 0)
			{
				foreach (Reader value in ReaderDic.Values)
				{
					value.Disconnect();
				}
				ReaderDic.Clear();
			}
			stop();
		}
	}
}
