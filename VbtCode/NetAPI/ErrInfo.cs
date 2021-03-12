namespace NetAPI
{
	public class ErrInfo
	{
		private string errCode;

		private string errMsg;

		public string ErrMsg
		{
			get
			{
				return errMsg;
			}
			set
			{
				errMsg = value;
			}
		}

		public string ErrCode
		{
			get
			{
				return errCode;
			}
			set
			{
				errCode = value;
			}
		}

		public ErrInfo(string errCode, string errMsg)
		{
			this.errCode = errCode;
			this.errMsg = errMsg;
		}
	}
}
