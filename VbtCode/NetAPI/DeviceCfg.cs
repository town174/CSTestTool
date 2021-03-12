using System;
using System.IO;
using System.Text;
using System.Xml;

namespace NetAPI
{
	public class DeviceCfg
	{
		protected XmlDocument doc = new XmlDocument();

		protected internal XmlNode root;

		protected string fn = APIPath.folderName + "\\DeviceCfg.xml";

		public DeviceCfg()
		{
			if (!File.Exists(fn))
			{
				Create();
			}
			doc.Load(fn);
			root = doc.DocumentElement;
		}

		private void Create()
		{
			try
			{
				XmlTextWriter xmlTextWriter = new XmlTextWriter(fn, Encoding.UTF8);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteStartElement("Devices");
				DeviceCfgItem deviceCfgItem = new DeviceCfgItem();
				xmlTextWriter.WriteStartElement("Device");
				xmlTextWriter.WriteAttributeString("Name", deviceCfgItem.name);
				xmlTextWriter.WriteStartElement("Port");
				xmlTextWriter.WriteAttributeString("Type", deviceCfgItem.type);
				xmlTextWriter.WriteString(deviceCfgItem.connStr);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndDocument();
				xmlTextWriter.Close();
			}
			catch (Exception ex)
			{
				Util.logAndTriggerApiErr("DeviceCfg.xml", "FF03", ex.Message, LogType.Fatal);
			}
		}

		public void AddReaderItem(DeviceCfgItem item)
		{
			if (item == null)
			{
				Util.logAndTriggerApiErr("DeviceCfg.xml", "FF03", "", LogType.Debug);
			}
			else
			{
				XmlNode xmlNode = root.SelectSingleNode("//Device[@Name='" + item.name + "']");
				if (xmlNode == null)
				{
					XmlNode xmlNode2 = doc.CreateElement("Device");
					XmlAttribute node = doc.CreateAttribute("Name");
					xmlNode2.Attributes.Append(node);
					xmlNode2.Attributes["Name"].Value = item.name;
					XmlNode xmlNode3 = doc.CreateElement("Port");
					XmlAttribute node2 = doc.CreateAttribute("Type");
					xmlNode3.Attributes.Append(node2);
					xmlNode3.Attributes["Type"].Value = item.type;
					xmlNode3.InnerText = item.connStr;
					xmlNode2.AppendChild(xmlNode3);
					if (item.addressList != null)
					{
						byte[] addressList = item.addressList;
						for (int i = 0; i < addressList.Length; i++)
						{
							byte b = addressList[i];
							XmlNode xmlNode4 = doc.CreateElement("Address");
							xmlNode4.InnerText = b.ToString();
							xmlNode2.AppendChild(xmlNode4);
						}
					}
					root.AppendChild(xmlNode2);
					doc.Save(fn);
				}
				else
				{
					Util.logAndTriggerApiErr("DeviceCfg.xml", "FF03", "", LogType.Debug);
				}
			}
		}

		public void RemoveReaderItem(string readerName)
		{
			readerName = Util.XmlstringReplace(readerName);
			XmlNode xmlNode = root.SelectSingleNode("//Device[@Name='" + readerName + "']");
			if (xmlNode != null)
			{
				root.RemoveChild(xmlNode);
				doc.Save(fn);
			}
			else
			{
				Util.logAndTriggerApiErr("DeviceCfg.xml", "FF03", "", LogType.Debug);
			}
		}

		public DeviceCfgItem FindReaderItem(string readerName)
		{
			readerName = Util.XmlstringReplace(readerName);
			XmlNode xmlNode = null;
			DeviceCfgItem deviceCfgItem = null;
			xmlNode = root.SelectSingleNode("//Device[@Name='" + readerName + "']");
			if (xmlNode != null)
			{
				deviceCfgItem = new DeviceCfgItem();
				deviceCfgItem.name = xmlNode.Attributes["Name"].Value;
				deviceCfgItem.type = xmlNode.FirstChild.Attributes["Type"].Value;
				deviceCfgItem.connStr = xmlNode.FirstChild.InnerText;
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("Address");
				if (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					int num = 0;
					deviceCfgItem.addressList = new byte[xmlNodeList.Count];
					foreach (XmlNode item in xmlNodeList)
					{
						deviceCfgItem.addressList[num++] = byte.Parse(item.InnerText);
					}
				}
			}
			return deviceCfgItem;
		}

		public void ModifyReaderItem(string targetReaderName, DeviceCfgItem item)
		{
			targetReaderName = Util.XmlstringReplace(targetReaderName);
			XmlNode xmlNode = null;
			xmlNode = root.SelectSingleNode("//Device[@Name='" + targetReaderName + "']");
			if (xmlNode != null)
			{
				root.RemoveChild(xmlNode);
				AddReaderItem(item);
				doc.Save(fn);
			}
			else
			{
				Util.logAndTriggerApiErr("DeviceCfg.xml", "FF03", "", LogType.Debug);
			}
		}
	}
}
