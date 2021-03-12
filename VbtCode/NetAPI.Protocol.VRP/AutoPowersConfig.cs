using NetAPI.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;

namespace NetAPI.Protocol.VRP
{
	internal class AutoPowersConfig
	{
		public delegate void OnAutoPowerConfigHandle(byte antennaNO, byte layerNO, int tagNum, byte powerValue);

		private int interval = 3600;

		private int scanTime = 18;

		private int preScanTime = 18;

		internal List<LayerInfo> Layers;

		private List<PowerMapping> powers;

		private byte scanPower = 24;

		private byte preScanPower = 9;

		private Reader reader;

		private XmlDocument xmlDocument = new XmlDocument();

		private XmlNode root;

		private string filename = APIPath.folderName + "\\AutoPowersConfig.xml";

		public volatile bool IsStop = true;

		private volatile byte curAntNO;

		private volatile byte curLayerNO;

		private volatile int curIndex;

		private Thread threadPowerConfig;

		private Dictionary<string, int> tagList = new Dictionary<string, int>();

		private List<byte> preReadPort = new List<byte>();

		private object lockObj = new object();

		public int Interval
		{
			get
			{
				return interval;
			}
			set
			{
				interval = value;
				XmlNode xmlNode = root.SelectSingleNode("Interval");
				xmlNode.InnerText = interval.ToString();
				xmlDocument.Save(filename);
			}
		}

		public int ScanTime
		{
			get
			{
				return scanTime;
			}
			set
			{
				scanTime = value;
				XmlNode xmlNode = root.SelectSingleNode("ScanTime");
				xmlNode.InnerText = scanTime.ToString();
				xmlDocument.Save(filename);
			}
		}

		public int PreScanTime
		{
			get
			{
				return preScanTime;
			}
			set
			{
				preScanTime = value;
				XmlNode xmlNode = root.SelectSingleNode("PreScanTime");
				xmlNode.InnerText = preScanTime.ToString();
				xmlDocument.Save(filename);
			}
		}

		public event OnAutoPowerConfigHandle OnAutoPowerConfig;

		public AutoPowersConfig(Reader reader)
		{
			this.reader = reader;
			if (!File.Exists(filename))
			{
				createXml();
				createNodes();
			}
			else
			{
				xmlDocument.Load(filename);
				root = xmlDocument.DocumentElement;
				interval = int.Parse(root.SelectSingleNode("Interval").InnerText);
				scanTime = int.Parse(root.SelectSingleNode("ScanTime").InnerText);
				scanPower = byte.Parse(root.SelectSingleNode("ScanPower").InnerText);
				preScanTime = int.Parse(root.SelectSingleNode("PreScanTime").InnerText);
				preScanPower = byte.Parse(root.SelectSingleNode("PreScanPower").InnerText);
			}
			string innerText = root.SelectSingleNode("Layers").InnerText;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(LayerInfo[]));
			Layers = new List<LayerInfo>((LayerInfo[])dataContractJsonSerializer.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(innerText))));
			string innerText2 = root.SelectSingleNode("PowerMapping").InnerText;
			DataContractJsonSerializer dataContractJsonSerializer2 = new DataContractJsonSerializer(typeof(PowerMapping[]));
			powers = new List<PowerMapping>((PowerMapping[])dataContractJsonSerializer2.ReadObject(new MemoryStream(Encoding.ASCII.GetBytes(innerText2))));
		}

		private void createNodes()
		{
			root.RemoveAll();
			XmlElement xmlElement = xmlDocument.CreateElement("Interval");
			xmlElement.InnerText = Interval.ToString();
			root.AppendChild(xmlElement);
			XmlElement xmlElement2 = xmlDocument.CreateElement("PreScanTime");
			xmlElement2.InnerText = preScanTime.ToString();
			root.AppendChild(xmlElement2);
			XmlElement xmlElement3 = xmlDocument.CreateElement("PreScanPower");
			xmlElement3.InnerText = preScanPower.ToString();
			root.AppendChild(xmlElement3);
			XmlElement xmlElement4 = xmlDocument.CreateElement("ScanTime");
			xmlElement4.InnerText = ScanTime.ToString();
			root.AppendChild(xmlElement4);
			XmlElement xmlElement5 = xmlDocument.CreateElement("ScanPower");
			xmlElement5.InnerText = scanPower.ToString();
			root.AppendChild(xmlElement5);
			XmlElement xmlElement6 = xmlDocument.CreateElement("Layers");
			xmlElement6.InnerText = "[{\"AntennaNO\":1,\"LayerNO\":1},{\"AntennaNO\":1,\"LayerNO\":2},{\"AntennaNO\":1,\"LayerNO\":3},{\"AntennaNO\":1,\"LayerNO\":4}]";
			root.AppendChild(xmlElement6);
			XmlElement xmlElement7 = xmlDocument.CreateElement("PowerMapping");
			xmlElement7.InnerText = "[{\"Power\":9,\"Min\":0,\"Max\":0},{\"Power\":11,\"Min\":1,\"Max\":5},{\"Power\":13,\"Min\":6,\"Max\":10},{\"Power\":15,\"Min\":11,\"Max\":15},{\"Power\":17,\"Min\":16,\"Max\":20},{\"Power\":19,\"Min\":21,\"Max\":25},{\"Power\":20,\"Min\":26,\"Max\":30},{\"Power\":21,\"Min\":31,\"Max\":35},{\"Power\":22,\"Min\":36,\"Max\":40},{\"Power\":23,\"Min\":41,\"Max\":45},{\"Power\":24,\"Min\":45,\"Max\":50},{\"Power\":25,\"Min\":51,\"Max\":55},{\"Power\":25,\"Min\":56,\"Max\":0}]";
			root.AppendChild(xmlElement7);
			xmlDocument.Save(filename);
		}

		private void createXml()
		{
			try
			{
				XmlNode newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "");
				xmlDocument.AppendChild(newChild);
				root = xmlDocument.CreateElement("Configuration");
				xmlDocument.AppendChild(root);
				xmlDocument.Save(filename);
			}
			catch (Exception ex)
			{
				Log.Error("创建AutoPowersConfig.xml文件错误：" + ex.Message);
			}
		}

		public LayerInfo[] QueryAllLayerPower()
		{
			LayerInfo[] array = new LayerInfo[Layers.Count];
			foreach (LayerInfo layer in Layers)
			{
				MsgHubPowersConfig msgHubPowersConfig = new MsgHubPowersConfig(layer.AntennaNO, layer.LayerNO);
				if (reader.Send(msgHubPowersConfig))
				{
					layer.Power = msgHubPowersConfig.ReceivedMessage.Power;
				}
			}
			Layers.CopyTo(array);
			return array;
		}

		public bool Start(out string err)
		{
			bool result = false;
			err = "";
			if (reader == null || !reader.IsConnected)
			{
				err = "读写器已断开，无法进行功率调整";
				return false;
			}
			try
			{
				clearTagList();
				if (reader != null)
				{
					reader.OnAutoPowerConfigInventoryReceived += tagInventoryRecv;
				}
				if (threadPowerConfig == null)
				{
					threadPowerConfig = new Thread(doPowerConfig);
				}
				if (threadPowerConfig.ThreadState == ThreadState.Unstarted)
				{
					threadPowerConfig.Start();
				}
				result = true;
			}
			catch (Exception ex)
			{
				err = "自动功率调整启动错误：" + ex.Message;
				Log.Error(err);
			}
			return result;
		}

		public void Stop()
		{
			IsStop = true;
			if (reader != null)
			{
				reader.isPause_AutoPowerConfig = false;
				reader.isPause_AutoPowerConfig = false;
				reader.OnAutoPowerConfigInventoryReceived -= tagInventoryRecv;
			}
			clearTagList();
		}

		private void clearTagList()
		{
			lock (lockObj)
			{
				tagList.Clear();
				preReadPort.Clear();
			}
		}

		private void tagInventoryRecv(string readerName, RxdTagData tagData)
		{
			if (tagData.Antenna == curAntNO)
			{
				string text = Util.ConvertbyteArrayToHexstring(tagData.EPC);
				if (!string.IsNullOrEmpty(text))
				{
					lock (lockObj)
					{
						if (tagList.ContainsKey(text))
						{
							Dictionary<string, int> dictionary = tagList;
							string key = text;
							dictionary[key]++;
						}
						else
						{
							tagList.Add(text, 1);
						}
						if (!preReadPort.Contains(tagData.Port))
						{
							preReadPort.Add(tagData.Port);
						}
					}
				}
			}
		}

		private void doPowerConfig()
		{
			string err = "";
			IsStop = false;
			curAntNO = Layers[curIndex].AntennaNO;
			curLayerNO = Layers[curIndex].LayerNO;
			while (!IsStop)
			{
				if (process(out err))
				{
					clearTagList();
					curIndex++;
					if (curIndex == Layers.Count)
					{
						curIndex = 0;
						for (int i = 0; i < Interval * 10; i++)
						{
							Thread.Sleep(100);
							if (IsStop)
							{
								break;
							}
						}
					}
					curAntNO = Layers[curIndex].AntennaNO;
					curLayerNO = Layers[curIndex].LayerNO;
				}
				if (reader != null)
				{
					reader.isStart_AutoPowerConfig = false;
				}
				Thread.Sleep(10);
			}
			IsStop = true;
		}

		private bool process(out string err)
		{
			bool flag = false;
			IsStop = false;
			err = "";
			if (reader != null)
			{
				reader.isStart_AutoPowerConfig = true;
			}
			int num = 4;
			int num2 = 4;
			byte power = scanPower;
			for (int i = 0; i < 3; i++)
			{
				MsgHubAntennaInfoConfig msgHubAntennaInfoConfig = new MsgHubAntennaInfoConfig();
				if (reader.Send(msgHubAntennaInfoConfig))
				{
					num = msgHubAntennaInfoConfig.ReceivedMessage.LayerAntennaCount;
					flag = true;
					break;
				}
				flag = false;
				err = "获取每层天线数失败：" + msgHubAntennaInfoConfig.ErrorInfo.ErrMsg;
			}
			HabAntennaStatus habAntennaStatus = null;
			for (int j = 0; j < 3; j++)
			{
				MsgHubAntennaConfig msgHubAntennaConfig = new MsgHubAntennaConfig(curAntNO);
				if (reader.Send(msgHubAntennaConfig))
				{
					habAntennaStatus = msgHubAntennaConfig.ReceivedMessage.PortInfo;
					flag = true;
					break;
				}
				flag = false;
				err = "获取天线启用状态失败：" + msgHubAntennaConfig.ErrorInfo.ErrMsg;
			}
			if (!flag)
			{
				return false;
			}
			flag = false;
			for (int k = 0; k < 4; k++)
			{
				AntennaStatus[] antennas = habAntennaStatus.Antennas;
				foreach (AntennaStatus antennaStatus in antennas)
				{
					if (antennaStatus.AntennaNO == k + 1 + (curLayerNO - 1) * 4)
					{
						antennaStatus.IsEnable = true;
						break;
					}
				}
			}
			for (int m = 0; m < 3; m++)
			{
				if (reader == null)
				{
					break;
				}
				if (!reader.IsConnected)
				{
					break;
				}
				MsgHubAntennaConfig msgHubAntennaConfig2 = new MsgHubAntennaConfig(habAntennaStatus);
				if (reader.Send(msgHubAntennaConfig2, 2000))
				{
					flag = true;
					break;
				}
				flag = false;
				err = "设置天线启用状态失败：" + msgHubAntennaConfig2.ErrorInfo.ErrMsg;
			}
			if (flag)
			{
				flag = false;
				for (int n = 0; n < 3; n++)
				{
					if (reader == null)
					{
						break;
					}
					if (!reader.IsConnected)
					{
						break;
					}
					MsgHubPowersConfig msgHubPowersConfig = new MsgHubPowersConfig(curAntNO, curLayerNO, preScanPower);
					msgHubPowersConfig.msgBody[0] = 2;
					if (reader.Send(msgHubPowersConfig, 2000))
					{
						flag = true;
						if (err != "")
						{
							err = "";
						}
						break;
					}
					err = "设置预读取功率错误: " + msgHubPowersConfig.ErrorInfo.ErrMsg;
					flag = false;
					if (PauseMethod(isSendMsg: false))
					{
						return false;
					}
				}
			}
			if (PauseMethod(isSendMsg: false))
			{
				return false;
			}
			if (flag)
			{
				flag = false;
				MsgHubAntennaPortDwellTimeConfig msgHubAntennaPortDwellTimeConfig = new MsgHubAntennaPortDwellTimeConfig(curAntNO, (ushort)(preScanTime * 100 / num));
				for (int num3 = 0; num3 < 3; num3++)
				{
					if (reader == null)
					{
						break;
					}
					if (!reader.IsConnected)
					{
						break;
					}
					if (reader.Send(msgHubAntennaPortDwellTimeConfig, 2000))
					{
						flag = true;
						if (err != "")
						{
							err = "";
						}
						break;
					}
					err = "设置预读驻留时间错误: " + msgHubAntennaPortDwellTimeConfig.ErrorInfo.ErrMsg;
					flag = false;
					if (PauseMethod(isSendMsg: false))
					{
						return false;
					}
				}
				if (PauseMethod(isSendMsg: true))
				{
					return false;
				}
				if (!flag)
				{
					return false;
				}
				flag = false;
				for (int num4 = 0; num4 < 3; num4++)
				{
					if (reader == null)
					{
						break;
					}
					if (!reader.IsConnected)
					{
						break;
					}
					MsgTagInventory msgTagInventory = new MsgTagInventory();
					msgTagInventory.isAutoPowerConft = true;
					if (reader.Send(msgTagInventory))
					{
						flag = true;
						if (err != "")
						{
							err = "";
						}
						break;
					}
					err = "发送预读盘存指令错误: " + msgTagInventory.ErrorInfo.ErrMsg;
					flag = false;
				}
				if (flag)
				{
					for (int num5 = 0; num5 < preScanTime; num5++)
					{
						Thread.Sleep(100);
						if (reader == null || reader.isPause_AutoPowerConfig)
						{
							break;
						}
					}
					Thread.Sleep(150);
					for (int num6 = 0; num6 < 3; num6++)
					{
						if (reader == null)
						{
							break;
						}
						if (!reader.IsConnected)
						{
							break;
						}
						MsgPowerOff msgPowerOff = new MsgPowerOff();
						if (reader.Send(msgPowerOff))
						{
							flag = true;
							if (err != "")
							{
								err = "";
							}
							break;
						}
						err = "发送预读停止指令错误: " + msgPowerOff.ErrorInfo.ErrMsg;
						flag = false;
					}
					for (int num7 = 0; num7 < 5; num7++)
					{
						Thread.Sleep(100);
						if (reader == null || reader.isPause_AutoPowerConfig)
						{
							break;
						}
					}
					MsgHubAntennaPortDwellTimeConfig msgHubAntennaPortDwellTimeConfig2 = new MsgHubAntennaPortDwellTimeConfig(curAntNO, 0);
					for (int num8 = 0; num8 < 3; num8++)
					{
						if (reader == null)
						{
							break;
						}
						if (!reader.IsConnected)
						{
							break;
						}
						if (reader.Send(msgHubAntennaPortDwellTimeConfig2, 2000))
						{
							flag = true;
							if (err != "")
							{
								err = "";
							}
							break;
						}
						err = "设置驻留时间(0)错误: " + msgHubAntennaPortDwellTimeConfig2.ErrorInfo.ErrMsg;
						flag = false;
						if (PauseMethod(isSendMsg: true))
						{
							return false;
						}
					}
					switch (flag)
					{
					case false:
						return false;
					default:
					{
						flag = false;
						bool flag2 = false;
						for (int num9 = 0; num9 < 4; num9++)
						{
							byte b = (byte)((curLayerNO - 1) * 4 + (num9 + 1));
							if (!preReadPort.Contains(b))
							{
								AntennaStatus[] antennas2 = habAntennaStatus.Antennas;
								foreach (AntennaStatus antennaStatus2 in antennas2)
								{
									if (antennaStatus2.AntennaNO == b)
									{
										antennaStatus2.IsEnable = false;
										num2--;
										flag2 = true;
										break;
									}
								}
							}
						}
						if (flag2)
						{
							for (int num11 = 0; num11 < 3; num11++)
							{
								if (reader == null)
								{
									break;
								}
								if (!reader.IsConnected)
								{
									break;
								}
								MsgHubAntennaConfig msgHubAntennaConfig3 = new MsgHubAntennaConfig(habAntennaStatus);
								if (reader.Send(msgHubAntennaConfig3, 2000))
								{
									flag = true;
									break;
								}
								flag = false;
								err = "设置天线启用状态失败：" + msgHubAntennaConfig3.ErrorInfo.ErrMsg;
							}
						}
						else
						{
							flag = true;
						}
						if (!flag)
						{
							return false;
						}
						flag = false;
						for (int num12 = 0; num12 < 3; num12++)
						{
							if (reader == null)
							{
								break;
							}
							if (!reader.IsConnected)
							{
								break;
							}
							MsgHubPowersConfig msgHubPowersConfig2 = new MsgHubPowersConfig(curAntNO, curLayerNO, power);
							msgHubPowersConfig2.msgBody[0] = 2;
							if (reader.Send(msgHubPowersConfig2, 2000))
							{
								flag = true;
								if (err != "")
								{
									err = "";
								}
								break;
							}
							err = "设置参考功率错误: " + msgHubPowersConfig2.ErrorInfo.ErrMsg;
							flag = false;
							if (PauseMethod(isSendMsg: false))
							{
								return false;
							}
						}
						if (PauseMethod(isSendMsg: false))
						{
							return false;
						}
						if (flag && num2 > 0)
						{
							flag = false;
							for (int num13 = 0; num13 < 3; num13++)
							{
								if (reader == null)
								{
									break;
								}
								if (!reader.IsConnected)
								{
									break;
								}
								MsgHubAntennaPortDwellTimeConfig msgHubAntennaPortDwellTimeConfig3 = new MsgHubAntennaPortDwellTimeConfig(curAntNO, (ushort)(ScanTime * 100 / num));
								if (reader.Send(msgHubAntennaPortDwellTimeConfig3, 2000))
								{
									flag = true;
									if (err != "")
									{
										err = "";
									}
									break;
								}
								err = "设置驻留时间错误: " + msgHubAntennaPortDwellTimeConfig3.ErrorInfo.ErrMsg;
								flag = false;
								if (PauseMethod(isSendMsg: false))
								{
									return false;
								}
							}
							if (PauseMethod(isSendMsg: true))
							{
								return false;
							}
							if (!flag)
							{
								return false;
							}
							flag = false;
							for (int num14 = 0; num14 < 3; num14++)
							{
								if (reader == null)
								{
									break;
								}
								if (!reader.IsConnected)
								{
									break;
								}
								MsgTagInventory msgTagInventory2 = new MsgTagInventory();
								msgTagInventory2.isAutoPowerConft = true;
								if (reader.Send(msgTagInventory2))
								{
									flag = true;
									if (err != "")
									{
										err = "";
									}
									break;
								}
								err = "发送盘存指令错误: " + msgTagInventory2.ErrorInfo.ErrMsg;
								flag = false;
							}
							if (!flag)
							{
								return false;
							}
							int num15 = ScanTime * num2 / 4;
							if (ScanTime * num2 % 4 != 0)
							{
								num15++;
							}
							for (int num16 = 0; num16 < num15; num16++)
							{
								Thread.Sleep(100);
								if (reader == null || reader.isPause_AutoPowerConfig)
								{
									break;
								}
							}
							Thread.Sleep(150);
							for (int num17 = 0; num17 < 3; num17++)
							{
								if (reader == null)
								{
									break;
								}
								if (!reader.IsConnected)
								{
									break;
								}
								MsgPowerOff msgPowerOff2 = new MsgPowerOff();
								if (reader.Send(msgPowerOff2))
								{
									flag = true;
									if (err != "")
									{
										err = "";
									}
									break;
								}
								err = "发送停止指令错误: " + msgPowerOff2.ErrorInfo.ErrMsg;
								flag = false;
							}
							for (int num18 = 0; num18 < 5; num18++)
							{
								Thread.Sleep(100);
								if (reader == null || reader.isPause_AutoPowerConfig)
								{
									break;
								}
							}
							for (int num19 = 0; num19 < 3; num19++)
							{
								if (reader == null)
								{
									break;
								}
								if (!reader.IsConnected)
								{
									break;
								}
								MsgHubAntennaPortDwellTimeConfig msgHubAntennaPortDwellTimeConfig4 = new MsgHubAntennaPortDwellTimeConfig(curAntNO, 0);
								if (reader.Send(msgHubAntennaPortDwellTimeConfig4, 2000))
								{
									flag = true;
									if (err != "")
									{
										err = "";
									}
									break;
								}
								err = "设置驻留时间(0)错误: " + msgHubAntennaPortDwellTimeConfig4.ErrorInfo.ErrMsg;
								flag = false;
								if (PauseMethod(isSendMsg: true))
								{
									return false;
								}
							}
						}
						else if (!flag)
						{
							return false;
						}
						if (!flag)
						{
							return false;
						}
						power = getPowerValue();
						if (!flag)
						{
							return false;
						}
						flag = false;
						for (int num20 = 0; num20 < 3; num20++)
						{
							if (reader == null)
							{
								break;
							}
							if (!reader.IsConnected)
							{
								break;
							}
							MsgHubPowersConfig msgHubPowersConfig3 = new MsgHubPowersConfig(curAntNO, curLayerNO, power);
							if (reader.Send(msgHubPowersConfig3, 2000))
							{
								flag = true;
								if (err != "")
								{
									err = "";
								}
								if (this.OnAutoPowerConfig != null)
								{
									this.OnAutoPowerConfig(curAntNO, curLayerNO, tagList.Count, power);
								}
								break;
							}
							err = "设置参考功率错误: " + msgHubPowersConfig3.ErrorInfo.ErrMsg;
							flag = false;
							if (PauseMethod(isSendMsg: false))
							{
								return false;
							}
						}
						return flag;
					}
					}
				}
				return false;
			}
			return false;
			IL_07d5:
			bool flag3 = false;
			return flag3;
		}

		private byte getPowerValue()
		{
			byte power = scanPower;
			int count = tagList.Count;
			foreach (PowerMapping power2 in powers)
			{
				if (count == power2.Min)
				{
					power = power2.Power;
					break;
				}
				if (count > power2.Min && count <= power2.Max)
				{
					power = power2.Power;
					break;
				}
				if (count > power2.Min && power2.Max < power2.Min)
				{
					power = power2.Power;
					break;
				}
			}
			return power;
		}

		private bool PauseMethod(bool isSendMsg)
		{
			if (reader == null)
			{
				IsStop = true;
				return true;
			}
			if (!reader.isPause_AutoPowerConfig)
			{
				return false;
			}
			if (isSendMsg && reader.IsConnected)
			{
				reader.Send(new MsgHubAntennaPortDwellTimeConfig(curAntNO, 0));
			}
			if (reader != null)
			{
				reader.isStart_AutoPowerConfig = false;
			}
			while (reader != null && reader.isPause_AutoPowerConfig)
			{
				Thread.Sleep(100);
				if (IsStop)
				{
					break;
				}
			}
			if (reader == null)
			{
				IsStop = true;
			}
			return true;
		}

		~AutoPowersConfig()
		{
			Stop();
		}
	}
}
