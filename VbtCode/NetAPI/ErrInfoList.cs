using System.Collections.Generic;

namespace NetAPI
{
	public class ErrInfoList
	{
		public static Dictionary<string, string> ErrDictionary = new Dictionary<string, string>
		{
			{
				"FF00",
				"指令在指定时间内无返回"
			},
			{
				"FF01",
				"API其他错误"
			},
			{
				"FF02",
				"读写器异常断开"
			},
			{
				"FF03",
				"配置文件错误"
			},
			{
				"FF04",
				"读写器未连接或连接已断开"
			},
			{
				"FF05",
				"发送消息不能为空"
			}
		};
	}
}
