namespace NetAPI.Core
{
	public interface IHostMessage : IReaderMessage
	{
		bool IsReturn
		{
			get;
			set;
		}

		int Timeout
		{
			get;
			set;
		}

		byte[] TransmitterData
		{
			get;
			set;
		}

		void TrigerOnExecuting(object obj);

		void TrigerOnExecuted(object obj);
	}
}
