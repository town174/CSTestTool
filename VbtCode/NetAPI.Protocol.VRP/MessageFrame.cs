using System;
using System.Collections.Generic;

namespace NetAPI.Protocol.VRP
{
	public abstract class MessageFrame
	{
		public static Dictionary<string, string> ERROR_DICTIONARY = new Dictionary<string, string>
		{
			{
				"F0",
				"指令字错误"
			},
			{
				"F1",
				"指令执行失败"
			},
			{
				"F2",
				"CRC错误"
			},
			{
				"F3",
				"数据帧格式、帧参数错误"
			},
			{
				"F4",
				"存储区超限"
			},
			{
				"F5",
				"存储区被锁"
			},
			{
				"F6",
				"功率不够"
			},
			{
				"F7",
				"操作标签不支持"
			},
			{
				"F8",
				"未检测到标签或无回应"
			},
			{
				"F9",
				"操作标签失败"
			},
			{
				"FB",
				"没有缓存数据"
			},
			{
				"12",
				"天线错误"
			},
			{
				"14",
				"写flash错误"
			}
		};

		protected byte msgHeader = 85;

		protected byte msgControl;

		internal byte address;

		protected ushort bodyLength;

		protected internal ushort msgType;

		protected internal byte[] msgBody = new byte[0];

		protected ushort crc;

		internal bool isRS485;

		protected byte recvStatusCode;

		public byte[] GetMsgBuff()
		{
			int num = 7 + msgBody.Length;
			if (isRS485)
			{
				num++;
				msgControl = 32;
			}
			byte[] array = new byte[num];
			int num2 = 0;
			array[num2] = msgControl;
			num2++;
			if (isRS485)
			{
				array[num2] = address;
				num2++;
			}
			bodyLength = (ushort)(2 + msgBody.Length);
			array[num2] = (byte)(bodyLength >> 8);
			num2++;
			array[num2] = (byte)(bodyLength & 0xFF);
			num2++;
			array[num2] = (byte)(msgType >> 8);
			num2++;
			array[num2] = (byte)(msgType & 0xFF);
			num2++;
			Array.Copy(msgBody, 0, array, num2, msgBody.Length);
			num2 += msgBody.Length;
			byte[] sourceArray = Checksum.CalculateCRC(array, array.Length - 2);
			Array.Copy(sourceArray, 0, array, num2, 2);
			return Common.formatData(array);
		}
	}
}
