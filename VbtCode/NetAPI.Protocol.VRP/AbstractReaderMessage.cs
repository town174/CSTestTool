using NetAPI.Core;
using System;

namespace NetAPI.Protocol.VRP
{
	public abstract class AbstractReaderMessage : MessageFrame, IReaderMessage
	{
		protected byte[] rxData = null;

		protected uint msgID;

		protected MsgStatus statusCode = MsgStatus.Timeout;

		protected ErrInfo errInfo;

		protected ReceivedInfo recvInfo;

		public byte[] ReceivedData
		{
			get
			{
				return rxData;
			}
			set
			{
				rxData = value;
				if (Common.Validate(value) && Common.GetMessageID(value) == msgType)
				{
					msgControl = value[0];
					int num = 1;
					isRS485 = Common.IsRS485(value);
					if (isRS485)
					{
						address = value[1];
						num++;
					}
					bodyLength = (ushort)((value[num] << 8) + value[num + 1]);
					num += 4;
					byte b = value[num];
					num++;
					if (b == 0)
					{
						statusCode = MsgStatus.Success;
						msgBody = new byte[bodyLength - 3];
						if (msgBody.Length != 0)
						{
							Array.Copy(value, num, msgBody, 0, msgBody.Length);
						}
					}
					else
					{
						statusCode = MsgStatus.Failed;
						msgBody = new byte[bodyLength - 3];
						if (msgBody.Length != 0)
						{
							Array.Copy(value, num, msgBody, 0, msgBody.Length);
						}
						if (recvStatusCode == 63)
						{
							if (num < value.Length - 2)
							{
								string text = value[num].ToString("X2");
								errInfo = new ErrInfo(text, MessageFrame.ERROR_DICTIONARY[text]);
							}
						}
						else
						{
							errInfo = new ErrInfo(recvStatusCode.ToString("X2"), "错误未定义：" + recvStatusCode.ToString("X2"));
						}
					}
					crc = (ushort)((value[value.Length - 2] << 8) + value[value.Length - 1]);
				}
			}
		}

		public uint MessageID
		{
			get
			{
				uint result = msgType;
				if (isRS485)
				{
					result = (uint)((msgType << 8) + address);
				}
				return result;
			}
		}

		public virtual MsgStatus Status
		{
			get
			{
				return statusCode;
			}
			set
			{
				statusCode = value;
			}
		}

		public virtual ErrInfo ErrorInfo => errInfo;

		public ReceivedInfo ReceivedMessage => recvInfo;

		protected event EventHandler OnExecuting;

		protected event EventHandler OnExecuted;

		public AbstractReaderMessage()
		{
			string text = GetType().ToString();
			text = text.Substring(text.LastIndexOf('.') + 1);
			if (MessageType.msgClass.ContainsKey(text))
			{
				msgType = MessageType.msgClass[text];
			}
		}

		public IReaderMessage Clone()
		{
			statusCode = MsgStatus.Timeout;
			rxData = null;
			return this;
		}
	}
}
