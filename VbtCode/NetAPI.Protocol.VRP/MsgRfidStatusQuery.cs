using NetAPI.Core;
using NetAPI.Entities;

namespace NetAPI.Protocol.VRP
{
	public class MsgRfidStatusQuery : AbstractHostMessage
	{
		public class ReceivedInfo : NetAPI.Core.ReceivedInfo
		{
			public RfidStatus Status
			{
				get
				{
					RfidStatus result = RfidStatus.Idle;
					if (buff.Length >= 1)
					{
						result = (RfidStatus)buff[0];
					}
					return result;
				}
			}

			public AirProtocol Protocol
			{
				get
				{
					AirProtocol result = AirProtocol.ISO18000_6C;
					if (buff.Length >= 2)
					{
						result = (AirProtocol)buff[1];
					}
					return result;
				}
			}

			public FrequencyArea UhfBand
			{
				get
				{
					FrequencyArea result = FrequencyArea.FCC;
					if (buff.Length >= 3)
					{
						result = (FrequencyArea)buff[2];
					}
					return result;
				}
			}

			public AntennaPowerStatus[] Antennas
			{
				get
				{
					AntennaPowerStatus[] array = null;
					if (buff.Length >= 11)
					{
						array = new AntennaPowerStatus[64];
						int num = 11;
						int num2 = 0;
						for (int i = 0; i < 8; i++)
						{
							for (int j = 0; j < 8; j++)
							{
								array[num2] = new AntennaPowerStatus();
								array[num2].AntennaNO = (byte)(num2 + 1);
								byte b = (byte)(1 << j);
								array[num2].IsEnable = (((buff[10 - i] & b) > 0) ? true : false);
								if (buff.Length > num)
								{
									array[num2].PowerValue = buff[num];
									num++;
								}
								num2++;
							}
						}
					}
					return array;
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
	}
}
