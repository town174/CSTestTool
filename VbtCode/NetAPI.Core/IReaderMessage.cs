namespace NetAPI.Core
{
	public interface IReaderMessage
	{
		byte[] ReceivedData
		{
			get;
			set;
		}

		uint MessageID
		{
			get;
		}

		MsgStatus Status
		{
			get;
			set;
		}

		ErrInfo ErrorInfo
		{
			get;
		}

		IReaderMessage Clone();
	}
}
