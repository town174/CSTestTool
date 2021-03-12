namespace NetAPI.Protocol.VRP
{
	public class MsgActiveResetToFactoryDefault : AbstractHostMessage
	{
		public MsgActiveResetToFactoryDefault()
		{
			msgBody = new byte[1];
			msgBody[0] = 0;
		}
	}
}
