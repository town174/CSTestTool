using System.Collections.Generic;

namespace NetAPI.Protocol.VRP
{
	public class MessageType
	{
		public static readonly Dictionary<string, ushort> msgClass;

		public static readonly Dictionary<ushort, string> msgType;

		static MessageType()
		{
			msgClass = null;
			msgType = null;
			msgClass = new Dictionary<string, ushort>();
			msgClass.Add("MsgFirmwareUpgradePreparation", 521);
			msgClass.Add("MsgFirmwareOnlineUpgrade", 528);
			msgClass.Add("MsgPowerOn", 529);
			msgClass.Add("MsgPowerOff", 530);
			msgClass.Add("MsgTemperatureDetection", 531);
			msgClass.Add("MsgReaderVersionQuery", 532);
			msgClass.Add("MsgAntennaConfig", 534);
			msgClass.Add("MsgAntennaStatusQuery", 535);
			msgClass.Add("MsgRfidStatusQuery", 536);
			msgClass.Add("MsgGpioDefinitionConfig", 540);
			msgClass.Add("MsgGpiTriggerConfig", 541);
			msgClass.Add("MsgGpoConfig", 542);
			msgClass.Add("MsgGpiQuery", 543);
			msgClass.Add("MsgFilteringTimeConfig", 547);
			msgClass.Add("MsgIntervalTimeConfig", 548);
			msgClass.Add("MsgRssiThresholdConfig", 549);
			msgClass.Add("MsgUhfBandConfig", 550);
			msgClass.Add("MsgPowerConfig", 551);
			msgClass.Add("MsgAirProtocolConfig", 552);
			msgClass.Add("MsgFrequencyConfig", 553);
			msgClass.Add("MsgResetToFactoryDefault", 554);
			msgClass.Add("MsgReadPowerConfig", 555);
			msgClass.Add("MsgWritePowerConfig", 556);
			msgClass.Add("MsgIdleTimeConfig", 560);
			msgClass.Add("MsgUtcConfig", 790);
			msgClass.Add("MsgReaderCapabilityQuery", 562);
			msgClass.Add("MsgWorkModeConfig_AccessControl", 563);
			msgClass.Add("MsgResetToFactoryDefault_AccessControl", 564);
			msgClass.Add("MsgEpcAlarmConfig_AccessControl", 565);
			msgClass.Add("MsgGpioConfig_AccessControl", 566);
			msgClass.Add("MsgAlarmDataCacheNumQuery_AccessControl", 567);
			msgClass.Add("MsgAlarmInfo_AccessControl", 568);
			msgClass.Add("MsgAlarmDataCacheClean_AccessControl", 569);
			msgClass.Add("MsgQValueConfig", 576);
			msgClass.Add("MsgPowerLevelTableQuery", 577);
			msgClass.Add("MsgCommunicationModeConfig", 579);
			msgClass.Add("MsgPowerCalibrationConfig", 580);
			msgClass.Add("MsgSessionConfig", 581);
			msgClass.Add("MsgPortDwellTimeConfig", 582);
			msgClass.Add("MsgAntennaWorkOrderConfig", 583);
			msgClass.Add("MsgReaderWorkModeConfig", 584);
			msgClass.Add("MsgScanModeConfig", 585);
			msgClass.Add("MsgLbtConfig", 586);
			msgClass.Add("MsgAntennaEchoLossThresholdConfig", 588);
			msgClass.Add("MsgR2000DcOffsetCalibrationConfig", 589);
			msgClass.Add("MsgForwardAndBackwardPowerQuery", 590);
			msgClass.Add("MsgR2000RegisterConfig", 591);
			msgClass.Add("MsgTagReadingSuccessRateQuery", 592);
			msgClass.Add("MsgReaderCpuReset", 593);
			msgClass.Add("MsgR2000Reset", 594);
			msgClass.Add("MsgAntennaNumberConfig", 595);
			msgClass.Add("MsgR2000ResetInformationReport", 596);
			msgClass.Add("MsgCpuVersionQuery", 769);
			msgClass.Add("MsgRs232BaudRateConfig", 538);
			msgClass.Add("MsgRs485BaudRateConfig", 771);
			msgClass.Add("MsgRs485AddressConfig", 539);
			msgClass.Add("MsgTcpModeConfig", 773);
			msgClass.Add("MsgIpAddressConfig", 774);
			msgClass.Add("MsgMacConfig", 775);
			msgClass.Add("MsgWifiMacConfig", 776);
			msgClass.Add("MsgWifiModeConfig", 777);
			msgClass.Add("MsgWifiIpAddressConfig", 778);
			msgClass.Add("MsgWifiHotspotConfig", 779);
			msgClass.Add("MsgWifiStatusConfig", 780);
			msgClass.Add("MsgSearchIp", 781);
			msgClass.Add("MsgAppFirmwareUpgradePreparation", 784);
			msgClass.Add("MsgAppFirmwareOnlineUpgrade", 785);
			msgClass.Add("MsgMasterAndSlaveModeConfig", 786);
			msgClass.Add("MsgSlaveModeCacheClean", 787);
			msgClass.Add("MsgSlaveModeCacheCountQuery", 788);
			msgClass.Add("MsgSlaveModeCacheQuery", 789);
			msgClass.Add("MsgKeepAlive", 791);
			msgClass.Add("MsgKeepAliveConfig", 793);
			msgClass.Add("MsgHubAntennaConfig", 816);
			msgClass.Add("MsgHubAntennaTimeResidentConfig", 818);
			msgClass.Add("MsgHubAntennaInfoConfig", 819);
			msgClass.Add("MsgHubPowersConfig", 820);
			msgClass.Add("MsgHubAntennaPortDwellTimeConfig", 821);
			msgClass.Add("MsgCtrlBoardConfig_YC001", 832);
			msgClass.Add("MsgLightSwitchConfig_YC001", 833);
			msgClass.Add("MsgShelfLightStateConfig_YC001", 834);
			msgClass.Add("MsgShelfLightModeConfig_YC001", 835);
			msgClass.Add("MsgCtrlUpdata_YC001", 836);
			msgClass.Add("MsgKeepAlive_YC001", 837);
			msgClass.Add("MsgKeepAliveConfig_YC001", 838);
			msgClass.Add("MsgMainCtrlBoardNO_YC001", 839);
			msgClass.Add("MsgSingleLayerLightStateConfig_YC001", 840);
			msgClass.Add("MsgSingleLightStateConfig_YC001", 841);
			msgClass.Add("MsgFilteringConfig_YC002", 865);
			msgClass.Add("Msg6CTagFieldConfig", 1537);
			msgClass.Add("MsgTagInventory", 1538);
			msgClass.Add("MsgTagRead", 1539);
			msgClass.Add("MsgTagWrite", 1540);
			msgClass.Add("MsgTagLock", 1541);
			msgClass.Add("MsgTagKill", 1542);
			msgClass.Add("MsgTagSelect", 1543);
			msgClass.Add("MsgActiveResetToFactoryDefault", 4097);
			msgClass.Add("MsgActiveResetCpu", 4098);
			msgClass.Add("MsgActiveAddress", 4099);
			msgClass.Add("MsgActiveFilteringTimeConfig", 4100);
			msgClass.Add("MsgActiveRssiThresholdConfig", 4101);
			msgClass.Add("MsgActiveAttenuationValueConfig", 4102);
			msgClass.Add("MsgActiveBeepEnableConfig", 4103);
			msgClass.Add("MsgActiveLedEnableConfig", 4104);
			msgClass.Add("MsgActiveTagInventory", 4105);
			msgClass.Add("MsgActiveTagInventory2", 864);
			msgClass.Add("MsgActiveTagInventoryStop", 4106);
			msgClass.Add("MsgActiveGpiTriggerConfig", 4107);
			msgClass.Add("MsgActiveGpoConfig", 4108);
			msgClass.Add("MsgActiveCommunicationModeConfig", 4109);
			msgClass.Add("MsgActiveVersionQuery", 4112);
			msgClass.Add("MsgActiveRfidParameterConfig", 4113);
			msgClass.Add("MsgActiveTagWriteUser", 4114);
			msgClass.Add("MsgActiveTagActive", 4115);
			msgClass.Add("MsgActiveTagSleep", 4116);
			msgClass.Add("MsgActiveTagWriteID", 4117);
			msgClass.Add("MsgActiveStandby", 4118);
			msgClass.Add("MsgActiveTagConfig", 4122);
			msgType = new Dictionary<ushort, string>();
			foreach (string key in msgClass.Keys)
			{
				msgType.Add(msgClass[key], key);
			}
		}
	}
}
