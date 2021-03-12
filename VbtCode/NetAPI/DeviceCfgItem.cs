namespace NetAPI
{
	public class DeviceCfgItem
	{
		internal string name = "Device1";

		internal string type = "RS232";

		internal string connStr = "COM1,115200";

		internal byte[] addressList;

		public string ReaderName
		{
			get
			{
				return Util.Xmlstring(name);
			}
			set
			{
				name = Util.XmlstringReplace(value);
			}
		}

		public string PortType
		{
			get
			{
				return Util.Xmlstring(type);
			}
			set
			{
				type = Util.XmlstringReplace(value);
			}
		}

		public string ConnStr
		{
			get
			{
				return Util.Xmlstring(connStr);
			}
			set
			{
				connStr = Util.XmlstringReplace(value);
			}
		}

		public byte[] AddressList
		{
			get
			{
				return addressList;
			}
			set
			{
				addressList = value;
			}
		}

		public DeviceCfgItem()
		{
		}

		public DeviceCfgItem(string name, string portType, string connStr)
		{
			this.name = Util.XmlstringReplace(name);
			type = Util.XmlstringReplace(portType);
			this.connStr = Util.XmlstringReplace(connStr);
		}
	}
}
