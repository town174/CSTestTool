using NetAPI.Entities;
using System.Collections.Generic;

namespace NetAPI.Protocol.VRP
{
	public delegate void ActiveTagInventoryReceivedHandle(string readerName, List<RxdActiveTag> activeTagList);
}
