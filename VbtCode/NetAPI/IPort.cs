namespace NetAPI
{
	public abstract class IPort
	{
		protected PortType port;

		protected string connStr = "";

		public PortType Port => port;

		public string ConnStr => connStr;

		public IPort()
		{
		}

		protected IPort(string connStr)
		{
			this.connStr = connStr;
		}
	}
}
