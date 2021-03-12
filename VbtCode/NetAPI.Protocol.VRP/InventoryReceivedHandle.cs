using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public delegate void InventoryReceivedHandle(string readerName, RxdTagData tagData);
}
