using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgGpioConfig_AccessControl : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public GpioConfig_AccessControl InfoState
			{
				get
				{
					GpioConfig_AccessControl gpioConfig_AccessControl = new GpioConfig_AccessControl();
					if (buff.Length >= 4)
					{
						gpioConfig_AccessControl.IsEnable = (buff[1] == 1);
						gpioConfig_AccessControl.GpioPort = buff[2];
						gpioConfig_AccessControl.Level = (GpioLevel)buff[3];
					}
					return gpioConfig_AccessControl;
				}
			}

			public GpioConfig_AccessControl AlarmState
			{
				get
				{
					GpioConfig_AccessControl gpioConfig_AccessControl = new GpioConfig_AccessControl();
					if (buff.Length >= 7)
					{
						gpioConfig_AccessControl.IsEnable = (buff[4] == 1);
						gpioConfig_AccessControl.GpioPort = buff[5];
						gpioConfig_AccessControl.Level = (GpioLevel)buff[6];
					}
					return gpioConfig_AccessControl;
				}
			}

			public ReceivedInfo(byte[] buff)
				: base(buff)
			{
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

		public MsgGpioConfig_AccessControl()
		{
			msgBody = new byte[1];
			msgBody[0] = 1;
		}

		public MsgGpioConfig_AccessControl(GpioConfig_AccessControl info, GpioConfig_AccessControl alarm)
		{
			msgBody = new byte[7];
			msgBody[0] = 0;
			msgBody[1] = (byte)(info.IsEnable ? 1 : 0);
			msgBody[2] = info.GpioPort;
			msgBody[3] = (byte)info.Level;
			msgBody[4] = (byte)(alarm.IsEnable ? 1 : 0);
			msgBody[5] = alarm.GpioPort;
			msgBody[6] = (byte)alarm.Level;
		}
	}
}
