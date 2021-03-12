namespace NetAPI.Protocol.VRP
{
	public class MsgResetToFactoryDefault_AccessControl : AbstractHostMessage
	{
		public MsgResetToFactoryDefault_AccessControl()
		{
			msgBody = new byte[2];
			msgBody[0] = 0;
			msgBody[1] = 1;
		}
	}
}
