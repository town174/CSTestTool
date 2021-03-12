using System;

namespace NetAPI.Entities
{
	public class ActiveRfidParameter
	{
		public byte ModuleID;

		public RFFreq Freq;

		public RFSpeed RfSpeed;

		public byte RecvDataLen;

		public byte RecvAddressLen;

		public byte[] RecvAddress = new byte[5];

		public byte RfParityBitLen;

		internal byte[] GetParambytes()
		{
			byte[] array = new byte[11]
			{
				ModuleID,
				(byte)(int.Parse(Freq.ToString().Replace("Freq_", "").Replace("MHz", "")) - 2400),
				(byte)RfSpeed,
				RecvDataLen,
				RecvAddressLen,
				0,
				0,
				0,
				0,
				0,
				0
			};
			Array.Copy(RecvAddress, 0, array, 5, 5);
			array[10] = RfParityBitLen;
			return array;
		}

		internal void SetParam(byte[] buff)
		{
			ModuleID = buff[0];
			Freq = RFFreq.Freq_2450MHz;
			try
			{
				Freq = (RFFreq)Enum.Parse(typeof(RFFreq), "Freq_" + (buff[1] + 2400).ToString() + "MHz");
			}
			catch (Exception ex)
			{
				Log.Error("RFFreq转换错误：" + ex.Message);
			}
			RfSpeed = (RFSpeed)buff[2];
			RecvDataLen = buff[3];
			RecvAddressLen = buff[4];
			Array.Copy(buff, 5, RecvAddress, 0, 5);
			RfParityBitLen = buff[10];
		}
	}
}
