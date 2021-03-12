using NetAPI.Core;
using NetAPI.Entities;
using System;

namespace NetAPI.Protocol.VRP
{
	public class MsgGpiTriggerConfig : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			private GpiTriggerParameter tParam;

			private byte[] msgbody;

			public GpiTriggerParameter TriggerParameter => tParam;

			public byte[] TriggerMsgBody => msgbody;

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
				if (buff.Length >= 6)
				{
					tParam = new GpiTriggerParameter();
					tParam.PortNO = buff[1];
					tParam.TriggerCondition = (GpiTriggerCondition)buff[2];
					tParam.StopCondition = (GpiStopCondition)buff[3];
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
							msgbody = new byte[num];
							Array.Copy(buff, 10, msgbody, 0, num);
							((MessageFrame)hostMessage).msgBody = msgbody;
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

		public MsgGpiTriggerConfig()
		{
		}

		public MsgGpiTriggerConfig(byte portNO)
		{
			msgBody = new byte[2];
			msgBody[0] = 1;
			msgBody[1] = portNO;
		}

		public MsgGpiTriggerConfig(GpiTriggerParameter param)
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
			msgBody[3] = (byte)param.StopCondition;
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
