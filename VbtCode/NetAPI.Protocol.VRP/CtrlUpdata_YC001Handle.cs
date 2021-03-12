using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public delegate void CtrlUpdata_YC001Handle(byte ctrlBoardNO, IrTrigger[] irStates, SenssorTrigger[] senssorStates);
}
