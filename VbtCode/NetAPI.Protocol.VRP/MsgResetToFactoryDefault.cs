namespace NetAPI.Protocol.VRP
{
	public class MsgResetToFactoryDefault : AbstractHostMessage
	{
		public MsgResetToFactoryDefault()
		{
			msgBody = new byte[1];
			msgBody[0] = 0;
		}
	}
}
