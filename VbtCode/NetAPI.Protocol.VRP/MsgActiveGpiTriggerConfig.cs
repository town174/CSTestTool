using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgActiveGpiTriggerConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			private GpiTriggerParameter tParam;

			public GpiTriggerParameter TriggerParameter => tParam;

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				if (buff.Length >= 6)
				{
					tParam = new GpiTriggerParameter();
					tParam.PortNO = buff[1];
					switch (buff[2])
					{
					case 0:
						tParam.TriggerCondition = GpiTriggerCondition.Disabled;
						break;
					case 3:
						tParam.TriggerCondition = GpiTriggerCondition.HighLevel;
						break;
					case 4:
						tParam.TriggerCondition = GpiTriggerCondition.LowLevel;
						break;
					}
					tParam.StopCondition = (GpiStopCondition)(buff[3] - 2);
					tParam.DelayTime = (ushort)((buff[4] << 8) + buff[5]);
					if (buff.Length == 8 && buff[6] == 0 && buff[7] == 0)
					{
						tParam.TriggerMsg = null;
					}
					if (buff.Length >= 10)
					{
						IHostMessage hostMessage = null;
						ushort key = (ushort)((buff[8] << 8) + buff[9]);
						if (MessageType.msgType.ContainsKey(key))
						{
							Type type = Type.GetType("NetAPI.Protocol.VRP." + MessageType.msgType[key], throwOnError: true);
							hostMessage = (IHostMessage)Activator.CreateInstance(type, null);
							int num = buff.Length - 10;
							((MessageFrame)hostMessage).msgBody = new byte[num];
							Array.Copy(buff, 10, ((MessageFrame)hostMessage).msgBody, 0, num);
						}
						tParam.TriggerMsg = hostMessage;
					}
				}
			}
		}

		public new ReceivedInfo ReceivedMessage
		{
			get
			{
				if (recvInfo == null)
				{
					if (msgBody == null)
					{
						return null;
					}
					recvInfo = new ReceivedInfo(msgBody);
				}
				return (ReceivedInfo)recvInfo;
			}
		}

		public MsgActiveGpiTriggerConfig()
		{
		}

		public MsgActiveGpiTriggerConfig(byte portNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = portNO;
		}

		public MsgActiveGpiTriggerConfig(GpiTriggerParameter param)
		{
			int num = 6;
			byte[] array = new byte[0];
			if (param.TriggerMsg == null)
			{
				num = 8;
			}
			else
			{
				array = ((MessageFrame)param.TriggerMsg).msgBody;
			}
			num += array.Length + 4;
			msgBody = new byte[num];
			msgBody[0] = 0;
			msgBody[1] = param.PortNO;
			msgBody[2] = (byte)param.TriggerCondition;
			switch (param.TriggerCondition)
			{
			case GpiTriggerCondition.HighLevel:
				msgBody[2] = 3;
				break;
			case GpiTriggerCondition.LowLevel:
				msgBody[2] = 4;
				break;
			}
			msgBody[3] = (byte)((byte)param.StopCondition + 2);
			msgBody[4] = (byte)(param.DelayTime >> 8);
			msgBody[5] = (byte)(param.DelayTime & 0xFF);
			if (param.TriggerMsg != null)
			{
				int num2 = 2 + array.Length;
				msgBody[6] = (byte)(num2 >> 8);
				msgBody[7] = (byte)(num2 & 0xFF);
				msgBody[8] = (byte)(((MessageFrame)param.TriggerMsg).msgType >> 8);
				msgBody[9] = (byte)(((MessageFrame)param.TriggerMsg).msgType & 0xFF);
				Array.Copy(array, 0, msgBody, 10, array.Length);
			}
		}
	}
}
