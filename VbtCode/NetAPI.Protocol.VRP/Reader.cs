using NetAPI.Core;
using NetAPI.Entities;
using System;
using System.Net.Sockets;
using System.Threading;

namespace NetAPI.Protocol.VRP
{
	public class Reader : AbstractReader
	{
		public delegate void OnAutoPowerConfigHandle(byte antennaNO, byte layerNO, int tagNum, byte powerValue);

		private bool isRs485 = false;

		internal bool isEnableRSSI;

		internal bool isEnableAntenna;

		internal volatile bool isEnableKeepAlive;

		internal volatile int keepAliveInterval;

		private Thread threadKeepAlive;

		private DateTime dtKeepAlive = DateTime.MinValue;

		private string modelNumber = "";

		public RS485Item[] RS485Items;

		public FrequencyArea UhfBand = FrequencyArea.Unknown;

		private AutoPowersConfig autoPowersConfig;

		internal volatile bool isStart_AutoPowerConfig = false;

		internal volatile bool isPause_AutoPowerConfig = false;

		private volatile bool isStop_AutoPowerConfig = true;

		public string ModelNumber => modelNumber;

		protected override IHostMessage ConnectMessage
		{
			get
			{
				MsgRfidStatusQuery result = new MsgRfidStatusQuery();
				if (isRs485)
				{
					result = null;
				}
				return result;
			}
		}

		protected override IHostMessage DisconnectMessage
		{
			get
			{
				MsgRfidStatusQuery result = new MsgRfidStatusQuery();
				if (isRs485)
				{
					result = null;
				}
				return result;
			}
		}

		public bool IsEnableRSSI => isEnableRSSI;

		public bool IsEnableAntenna => isEnableAntenna;

		public bool IsStop_AutoPowerConfig
		{
			get
			{
				if (autoPowersConfig != null)
				{
					isStop_AutoPowerConfig = autoPowersConfig.IsStop;
				}
				return isStop_AutoPowerConfig;
			}
		}

		public event BrokenNetworkHandle OnBrokenNetwork;

		public event InventoryReceivedHandle OnInventoryReceived;

		public event InventoryReceivedHandle OnMsgAlarmInfo_AccessControl;

		public event ActiveTagInventoryReceivedHandle OnActiveTagInventoryReceived;

		public event MsgActiveTagActiveReceivedHandle OnMsgActiveTagActiveReceivedHandle;

		public event MsgActiveTagSleepReceivedHandle OnMsgActiveTagSleepReceivedHandle;

		public event FirmwareOnlineUpgradeHandle OnFirmwareOnlineUpgradeReceived;

		public event FirmwareOnlineUpgradeHandle OnAppFirmwareOnlineUpgradeReceived;

		public event CtrlUpdata_YC001Handle OnCtrlUpdata_YC001;

		public event KeepAlive_YC001Handle OnKeepAlive_YC001;

		public event OnAutoPowerConfigInventoryReceivedHandle OnAutoPowerConfigInventoryReceived;

		public event UdpSearchIpHandle OnUdpSearchIpReceived;

		public event OnAutoPowerConfigHandle OnAutoPowerConfig;

		private void threadKeepAliveMethod()
		{
			while (true)
			{
				if (!isEnableKeepAlive)
				{
					return;
				}
				for (int i = 0; i < keepAliveInterval * 10; i++)
				{
					Thread.Sleep(100);
					if (!isEnableKeepAlive)
					{
						break;
					}
				}
				Thread.Sleep(200);
				if (isEnableKeepAlive)
				{
					int num = (int)DateTime.Now.Subtract(dtKeepAlive).TotalMilliseconds;
					if (num > keepAliveInterval * 1000)
					{
						break;
					}
				}
			}
			setIsConnected(isConnected: false);
			if (this.OnBrokenNetwork != null)
			{
				ErrInfo e = new ErrInfo("FF02", ErrInfoList.ErrDictionary["FF02"]);
				this.OnBrokenNetwork(ReaderName, e);
				Disconnect();
			}
		}

		public Reader(string readerName, IPort commPort)
			: base(readerName, commPort)
		{
			isRs485 = false;
			RS485Items = null;
			if (commPort is Rs485Port)
			{
				isRs485 = true;
				Rs485Port rs485Port = commPort as Rs485Port;
				RS485Items = new RS485Item[rs485Port.Addresses.Length];
				for (int i = 0; i < rs485Port.Addresses.Length; i++)
				{
					RS485Item rS485Item = new RS485Item
					{
						Address = rs485Port.Addresses[i],
						pReader = this
					};
					RS485Items[i] = rS485Item;
				}
			}
			AbstractReader.OnApiException += Reader_OnApiException;
			base.OnReaderMessageReceived += Reader_OnMessageNotificationReceived;
		}

		public Reader(string readerName)
			: base(readerName)
		{
			isRs485 = false;
			RS485Items = null;
			DeviceCfg deviceCfg = new DeviceCfg();
			DeviceCfgItem deviceCfgItem = deviceCfg.FindReaderItem(ReaderName);
			if (deviceCfgItem != null)
			{
				switch ((PortType)Enum.Parse(typeof(PortType), deviceCfgItem.PortType))
				{
				case PortType.TcpClient:
					base.CommPort = new TcpClientPort(deviceCfgItem.ConnStr);
					break;
				case PortType.RS232:
					base.CommPort = new Rs232Port(deviceCfgItem.ConnStr);
					break;
				case PortType.RS485:
				{
					isRs485 = true;
					RS485Items = new RS485Item[deviceCfgItem.AddressList.Length];
					int num = 0;
					byte[] addressList = deviceCfgItem.AddressList;
					foreach (byte address in addressList)
					{
						RS485Items[num].Address = address;
						RS485Items[num].pReader = this;
						num++;
					}
					base.CommPort = new Rs485Port(deviceCfgItem.ConnStr, deviceCfgItem.AddressList);
					break;
				}
				}
			}
			AbstractReader.OnApiException += Reader_OnApiException;
			base.OnReaderMessageReceived += Reader_OnMessageNotificationReceived;
		}

		internal Reader(Socket socket)
			: base(socket)
		{
			AbstractReader.OnApiException += Reader_OnApiException;
			base.OnReaderMessageReceived += Reader_OnMessageNotificationReceived;
		}

		private void Reader_OnMessageNotificationReceived(AbstractReader reader, IReaderMessage msg)
		{
			if (msg.Status == MsgStatus.Success || msg is MsgActiveTagActive || msg is MsgActiveTagSleep)
			{
				if (msg is MsgTagInventory)
				{
					if (isStart_AutoPowerConfig && !isPause_AutoPowerConfig)
					{
						if (this.OnAutoPowerConfigInventoryReceived != null)
						{
							MsgTagInventory msgTagInventory = msg as MsgTagInventory;
							if (IsEnableAntenna || IsEnableRSSI)
							{
								msgTagInventory.ReceivedMessage.setAntennaAndRSSI(IsEnableAntenna, IsEnableRSSI);
							}
							this.OnAutoPowerConfigInventoryReceived(reader.ReaderName, msgTagInventory.ReceivedMessage.TagData);
						}
					}
					else if (this.OnInventoryReceived != null)
					{
						MsgTagInventory msgTagInventory2 = msg as MsgTagInventory;
						if (msgTagInventory2.isRS485)
						{
							RS485Item[] rS485Items = RS485Items;
							foreach (RS485Item rS485Item in rS485Items)
							{
								if (rS485Item.Address == msgTagInventory2.address)
								{
									if (rS485Item.IsEnableAntenna || rS485Item.IsEnableRSSI)
									{
										msgTagInventory2.ReceivedMessage.setAntennaAndRSSI(IsEnableAntenna, IsEnableRSSI);
									}
									break;
								}
							}
						}
						else if (IsEnableAntenna || IsEnableRSSI)
						{
							msgTagInventory2.ReceivedMessage.setAntennaAndRSSI(IsEnableAntenna, IsEnableRSSI);
						}
						this.OnInventoryReceived(reader.ReaderName, msgTagInventory2.ReceivedMessage.TagData);
					}
				}
				if (msg is MsgTagRead && this.OnInventoryReceived != null)
				{
					MsgTagRead msgTagRead = msg as MsgTagRead;
					if (msgTagRead.isRS485)
					{
						RS485Item[] rS485Items2 = RS485Items;
						foreach (RS485Item rS485Item2 in rS485Items2)
						{
							if (rS485Item2.Address == msgTagRead.address)
							{
								if (rS485Item2.IsEnableAntenna || rS485Item2.IsEnableRSSI)
								{
									msgTagRead.ReceivedMessage.setAntennaAndRSSI(IsEnableAntenna, IsEnableRSSI);
								}
								break;
							}
						}
					}
					else if (IsEnableAntenna || IsEnableRSSI)
					{
						msgTagRead.ReceivedMessage.setAntennaAndRSSI(IsEnableAntenna, IsEnableRSSI);
					}
					this.OnInventoryReceived(reader.ReaderName, msgTagRead.ReceivedMessage.TagData);
				}
				if (msg is MsgAlarmInfo_AccessControl && this.OnMsgAlarmInfo_AccessControl != null)
				{
					MsgAlarmInfo_AccessControl msgAlarmInfo_AccessControl = msg as MsgAlarmInfo_AccessControl;
					this.OnMsgAlarmInfo_AccessControl(reader.ReaderName, msgAlarmInfo_AccessControl.ReceivedMessage.TagData);
				}
				if (msg is MsgActiveTagInventory && this.OnActiveTagInventoryReceived != null)
				{
					MsgActiveTagInventory msgActiveTagInventory = msg as MsgActiveTagInventory;
					this.OnActiveTagInventoryReceived(reader.ReaderName, msgActiveTagInventory.ReceivedMessage.ActiveTagList);
				}
				if (msg is MsgActiveTagInventory2 && this.OnActiveTagInventoryReceived != null)
				{
					MsgActiveTagInventory2 msgActiveTagInventory2 = msg as MsgActiveTagInventory2;
					this.OnActiveTagInventoryReceived(reader.ReaderName, msgActiveTagInventory2.ReceivedMessage.ActiveTagList);
				}
				if (msg is MsgActiveTagActive && this.OnMsgActiveTagActiveReceivedHandle != null)
				{
					MsgActiveTagActive msg2 = msg as MsgActiveTagActive;
					this.OnMsgActiveTagActiveReceivedHandle(reader.ReaderName, msg2);
				}
				if (msg is MsgActiveTagSleep && this.OnMsgActiveTagSleepReceivedHandle != null)
				{
					MsgActiveTagSleep msg3 = msg as MsgActiveTagSleep;
					this.OnMsgActiveTagSleepReceivedHandle(reader.ReaderName, msg3);
				}
				if (msg is MsgFirmwareOnlineUpgrade && this.OnFirmwareOnlineUpgradeReceived != null)
				{
					MsgFirmwareOnlineUpgrade msgFirmwareOnlineUpgrade = msg as MsgFirmwareOnlineUpgrade;
					uint frameAddress = msgFirmwareOnlineUpgrade.ReceivedMessage.FrameAddress;
					ushort total = (ushort)(frameAddress >> 16);
					ushort rate = (ushort)frameAddress;
					this.OnFirmwareOnlineUpgradeReceived(total, rate);
				}
				if (msg is MsgAppFirmwareOnlineUpgrade && this.OnFirmwareOnlineUpgradeReceived != null)
				{
					MsgAppFirmwareOnlineUpgrade msgAppFirmwareOnlineUpgrade = msg as MsgAppFirmwareOnlineUpgrade;
					uint frameAddress2 = msgAppFirmwareOnlineUpgrade.ReceivedMessage.FrameAddress;
					ushort total2 = (ushort)(frameAddress2 >> 16);
					ushort rate2 = (ushort)frameAddress2;
					this.OnAppFirmwareOnlineUpgradeReceived(total2, rate2);
				}
				if (msg is MsgCtrlUpdata_YC001 && this.OnCtrlUpdata_YC001 != null)
				{
					MsgCtrlUpdata_YC001 msgCtrlUpdata_YC = msg as MsgCtrlUpdata_YC001;
					byte ctrlBoardNO = msgCtrlUpdata_YC.ReceivedMessage.CtrlBoardNO;
					IrTrigger[] irStates = msgCtrlUpdata_YC.ReceivedMessage.IrStates;
					SenssorTrigger[] senssorStates = msgCtrlUpdata_YC.ReceivedMessage.SenssorStates;
					this.OnCtrlUpdata_YC001(ctrlBoardNO, irStates, senssorStates);
				}
				if (msg is MsgKeepAlive_YC001 && this.OnKeepAlive_YC001 != null)
				{
					this.OnKeepAlive_YC001();
				}
				if (msg is MsgKeepAlive)
				{
					if (keepAliveInterval == 0)
					{
						MsgKeepAliveConfig msgKeepAliveConfig = new MsgKeepAliveConfig();
						if (Send(msgKeepAliveConfig, 2000))
						{
							isEnableKeepAlive = msgKeepAliveConfig.ReceivedMessage.IsEnable;
							keepAliveInterval = msgKeepAliveConfig.ReceivedMessage.Interval;
						}
					}
					if (threadKeepAlive == null || !threadKeepAlive.IsAlive)
					{
						threadKeepAlive = new Thread(threadKeepAliveMethod);
						threadKeepAlive.Start();
					}
					dtKeepAlive = DateTime.Now;
				}
				if (msg is MsgSearchIp)
				{
					MsgSearchIp info = msg as MsgSearchIp;
					if (this.OnUdpSearchIpReceived != null)
					{
						this.OnUdpSearchIpReceived(ReaderName, info);
					}
				}
			}
		}

		public ConnectResponse Connect()
		{
			ConnectResponse connectResponse = new ConnectResponse();
			try
			{
				Connect(out connectResponse.ErrorInfo);
				if (base.IsConnected)
				{
					if (isRs485)
					{
						RS485Item[] rS485Items = RS485Items;
						foreach (RS485Item rS485Item in rS485Items)
						{
							MsgRfidStatusQuery msgRfidStatusQuery = new MsgRfidStatusQuery();
							if (rS485Item.Send(msgRfidStatusQuery))
							{
								rS485Item.UhfBand = msgRfidStatusQuery.ReceivedMessage.UhfBand;
							}
							Msg6CTagFieldConfig msg6CTagFieldConfig = new Msg6CTagFieldConfig();
							if (rS485Item.Send(msg6CTagFieldConfig))
							{
								rS485Item.isEnableAntenna = msg6CTagFieldConfig.ReceivedMessage.IsEnableAntenna;
								rS485Item.isEnableRSSI = msg6CTagFieldConfig.ReceivedMessage.IsEnableRSSI;
							}
							MsgReaderVersionQuery msgReaderVersionQuery = new MsgReaderVersionQuery();
							if (rS485Item.Send(msgReaderVersionQuery))
							{
								rS485Item.modelNumber = msgReaderVersionQuery.ReceivedMessage.ModelNumber;
							}
						}
					}
					else if (!(base.CommPort is UdpPort))
					{
						MsgRfidStatusQuery msgRfidStatusQuery2 = new MsgRfidStatusQuery();
						if (Send(msgRfidStatusQuery2))
						{
							UhfBand = msgRfidStatusQuery2.ReceivedMessage.UhfBand;
						}
						Msg6CTagFieldConfig msg6CTagFieldConfig2 = new Msg6CTagFieldConfig();
						if (Send(msg6CTagFieldConfig2))
						{
							isEnableAntenna = msg6CTagFieldConfig2.ReceivedMessage.IsEnableAntenna;
							isEnableRSSI = msg6CTagFieldConfig2.ReceivedMessage.IsEnableRSSI;
						}
						MsgReaderVersionQuery msgReaderVersionQuery2 = new MsgReaderVersionQuery();
						if (Send(msgReaderVersionQuery2))
						{
							modelNumber = msgReaderVersionQuery2.ReceivedMessage.ModelNumber;
						}
					}
				}
			}
			catch (Exception ex)
			{
				connectResponse.ErrorInfo = new ErrInfo("FF01", ex.Message);
				Log.Error(ex.Message);
			}
			connectResponse.IsSucessed = base.IsConnected;
			return connectResponse;
		}

		private void Reader_OnApiException(string senderName, ErrInfo e)
		{
			if (e.ErrCode == "FF02" && this.OnBrokenNetwork != null)
			{
				this.OnBrokenNetwork(senderName, e);
			}
		}

		public new void Disconnect()
		{
			try
			{
				isEnableKeepAlive = false;
				if (autoPowersConfig != null)
				{
					autoPowersConfig.Stop();
				}
				base.Disconnect();
			}
			catch (Exception ex)
			{
				Log.Error("断开连接错误：" + ex.Message);
			}
		}

		public override bool Send(IHostMessage msg)
		{
			if (msg == null)
			{
				return false;
			}
			if (isStart_AutoPowerConfig && !isPause_AutoPowerConfig)
			{
				if (msg is MsgTagInventory && !((MsgTagInventory)msg).isAutoPowerConft)
				{
					isPause_AutoPowerConfig = true;
					for (int i = 0; i < 200; i++)
					{
						Thread.Sleep(10);
						if (!isStart_AutoPowerConfig)
						{
							break;
						}
					}
				}
				if (msg is MsgTagRead && !msg.IsReturn)
				{
					isPause_AutoPowerConfig = true;
					for (int j = 0; j < 200; j++)
					{
						Thread.Sleep(10);
						if (!isStart_AutoPowerConfig)
						{
							break;
						}
					}
				}
			}
			else if (!isStart_AutoPowerConfig && isPause_AutoPowerConfig && msg is MsgPowerOff)
			{
				isPause_AutoPowerConfig = false;
			}
			return base.Send(msg);
		}

		public LayerInfo[] QueryAllLayerPower()
		{
			if (autoPowersConfig == null)
			{
				autoPowersConfig = new AutoPowersConfig(this);
				autoPowersConfig.OnAutoPowerConfig += AutoPowersConfig_OnAutoPowerConfig;
			}
			return autoPowersConfig.QueryAllLayerPower();
		}

		public bool StartAutoPowerConfig(out string err)
		{
			err = "";
			if (autoPowersConfig != null && !autoPowersConfig.IsStop)
			{
				return true;
			}
			if (autoPowersConfig == null)
			{
				autoPowersConfig = new AutoPowersConfig(this);
				autoPowersConfig.OnAutoPowerConfig += AutoPowersConfig_OnAutoPowerConfig;
			}
			return autoPowersConfig.Start(out err);
		}

		private void AutoPowersConfig_OnAutoPowerConfig(byte antennaNO, byte layerNO, int tagNum, byte powerValue)
		{
			if (this.OnAutoPowerConfig != null)
			{
				this.OnAutoPowerConfig(antennaNO, layerNO, tagNum, powerValue);
			}
		}

		public void StopAutoPowerConfig()
		{
			if (autoPowersConfig != null && !autoPowersConfig.IsStop)
			{
				autoPowersConfig.Stop();
			}
			autoPowersConfig.OnAutoPowerConfig -= AutoPowersConfig_OnAutoPowerConfig;
			autoPowersConfig = null;
		}

		~Reader()
		{
			Disconnect();
			base.OnReaderMessageReceived -= Reader_OnMessageNotificationReceived;
			AbstractReader.OnApiException -= Reader_OnApiException;
		}
	}
}
