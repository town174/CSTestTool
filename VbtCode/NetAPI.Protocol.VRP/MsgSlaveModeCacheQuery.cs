namespace NetAPI.Protocol.VRP
{
	public class MsgSlaveModeCacheQuery : AbstractHostMessage
	{
		public MsgSlaveModeCacheQuery()
		{
			msgBody = new byte[5];
			msgBody[0] = 1;
		}

		public MsgSlaveModeCacheQuery(int numberOfCaches)
		{
			msgBody = new byte[5];
			msgBody[0] = 1;
			msgBody[1] = (byte)(numberOfCaches >> 24);
			msgBody[2] = (byte)(numberOfCaches >> 16);
			msgBody[3] = (byte)(numberOfCaches >> 8);
			msgBody[4] = (byte)(numberOfCaches & 0xFF);
		}
	}
}
