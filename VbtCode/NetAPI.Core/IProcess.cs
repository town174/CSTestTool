using System.Collections.Generic;

namespace NetAPI.Core
{
	public interface IProcess
	{
		void Parse(byte[] buff, out List<byte[]> msgs);

		IReaderMessage ParseMessageNotification(byte[] recvMsg);

		uint GetMessageID(byte[] msg);
	}
}
