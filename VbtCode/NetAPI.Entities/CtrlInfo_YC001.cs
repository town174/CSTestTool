namespace NetAPI.Entities
{
	public class CtrlInfo_YC001
	{
		public byte LayerNum;

		public CtrlBoardConfig_YC001 SensorNum;

		internal byte[] GetParambytes()
		{
			return new byte[2]
			{
				LayerNum,
				SensorNum.GetParambyte()
			};
		}

		internal void SetParambytes(byte[] param)
		{
			if (param != null && param.Length != 0 && param.Length >= 1)
			{
				LayerNum = param[0];
			}
			if (param.Length >= 2)
			{
				SensorNum = new CtrlBoardConfig_YC001();
				SensorNum.SetParambyte(param[1]);
			}
		}
	}
}
