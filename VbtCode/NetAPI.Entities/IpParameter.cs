namespace NetAPI.Entities
{
	public class IpParameter
	{
		public string IP;

		public string Subnet;

		public string Gateway;

		internal void setData(byte[] data)
		{
			string[] array = new string[3];
			for (int i = 0; i < data.Length; i++)
			{
				ref string reference = ref array[i / 4];
				reference = reference + data[i].ToString() + ".";
				if (i % 4 == 3)
				{
					array[i / 4] = array[i / 4].Trim('.');
				}
			}
			IP = array[0];
			Subnet = array[1];
			Gateway = array[2];
		}

		internal byte[] getData()
		{
			byte[] array = new byte[12];
			try
			{
				int num = 0;
				string[] array2 = IP.Split('.');
				string[] array3 = array2;
				foreach (string s in array3)
				{
					array[num] = byte.Parse(s);
					num++;
				}
				string[] array4 = Subnet.Split('.');
				string[] array5 = array4;
				foreach (string s2 in array5)
				{
					array[num] = byte.Parse(s2);
					num++;
				}
				string[] array6 = Gateway.Split('.');
				string[] array7 = array6;
				foreach (string s3 in array7)
				{
					array[num] = byte.Parse(s3);
					num++;
				}
			}
			catch
			{
			}
			return array;
		}
	}
}
