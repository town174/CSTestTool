using NetAPI.Core;
using System;

namespace NetAPI.Protocol.VRP
{
	public abstract class AbstractHostMessage : MessageFrame, IHostMessage, IReaderMessage
	{
		protected bool isReturn = true;

		protected int timeout = 1000;

		protected byte[] rxData = null;

		protected MsgStatus statusCode = MsgStatus.Timeout;

		protected byte[] txData = null;

		protected ErrInfo errInfo;

		protected ReceivedInfo recvInfo;

		public bool IsReturn
		{
			get
			{
				return isReturn;
			}
			set
			{
				isReturn = value;
			}
		}

		public int Timeout
		{
			get
			{
				return timeout;
			}
			set
			{
				timeout = value;
			}
		}

		public byte[] TransmitterData
		{
			get
			{
				txData = GetMsgBuff();
				return txData;
			}
			set
			{
				txData = value;
			}
		}

		public virtual byte[] ReceivedData
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
					recvStatusCode = value[num];
					num++;
					if (recvStatusCode == 0)
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
								if (MessageFrame.ERROR_DICTIONARY.ContainsKey(text))
								{
									errInfo = new ErrInfo(text, MessageFrame.ERROR_DICTIONARY[text]);
								}
								else
								{
									errInfo = new ErrInfo(text, "错误未定义：" + text);
								}
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

		public ErrInfo ErrorInfo
		{
			get
			{
				if (Status == MsgStatus.Timeout)
				{
					string text = "FF00";
					if (ErrInfoList.ErrDictionary.ContainsKey(text))
					{
						errInfo = new ErrInfo(text, ErrInfoList.ErrDictionary[text]);
					}
					else
					{
						errInfo = new ErrInfo(text, "");
					}
				}
				Log.Debug("errInfo:" + errInfo?.ToString());
				return errInfo;
			}
		}

		public ReceivedInfo ReceivedMessage => recvInfo;

		protected event EventHandler OnExecuting;

		protected event EventHandler OnExecuted;

		public AbstractHostMessage()
		{
			string text = GetType().ToString();
			text = text.Substring(text.LastIndexOf('.') + 1);
			if (MessageType.msgClass.ContainsKey(text))
			{
				msgType = MessageType.msgClass[text];
			}
		}

		public virtual byte[] FromXml(string xmlStr)
		{
			return null;
		}

		void IHostMessage.TrigerOnExecuting(object obj)
		{
			if (this.OnExecuting != null)
			{
				this.OnExecuting(obj, EventArgs.Empty);
			}
		}

		void IHostMessage.TrigerOnExecuted(object obj)
		{
			if (this.OnExecuted != null)
			{
				this.OnExecuted(obj, EventArgs.Empty);
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
