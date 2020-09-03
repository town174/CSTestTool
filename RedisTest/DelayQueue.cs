using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RedisTest
{
    /// <summary>
    /// 延迟队列
    /// 1 入队消息，延迟A秒执行
    /// 2 单位时间B秒内，对于相同key消息，只允许入队一次 => 默认8秒超期 
    /// 3 确保key唯一
    /// </summary>
    public class DelayQueue
    {
        /// <summary>
        /// 延迟时间
        /// </summary>
        public int DelayTime { get; set; }

        /// <summary>
        /// 接收周期时间
        /// </summary>
        public int UnitTime { get; set; }

        /// <summary>
        /// 队列名
        /// </summary>
        public string QueueName { get; set; }

        ConnectionMultiplexer _Redis = null;
        IDatabase _DefaultDb = null;
        ISubscriber _Sub = null;
        Timer _RemoveKeyTimer = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="delay"></param>
        /// <param name="unit"></param>
        /// <param name="conn"></param>
        public DelayQueue(string name, int delay, int unit, string conn = "127.0.0.1:6379")
        {
            QueueName = name;
            DelayTime = delay;
            UnitTime = unit;
             
            try
            {
                _RemoveKeyTimer = new Timer(1000);
                _RemoveKeyTimer.Elapsed += _RemoveKeyTimer_Elapsed;
                _Redis = ConnectionMultiplexer.Connect(conn);
                _DefaultDb = _Redis.GetDatabase(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"初始化Redis异常：{ ex.Message}\r\n{ ex.StackTrace}");
            }
        }

        private void _RemoveKeyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //删除超期key
            //DateTimeOffset off = new DateTimeOffset();
            //off = off.AddTicks(tick);
            //TimeSpan sp = new TimeSpan(tick);
            //sp.TotalSeconds()

            var tick = DateTime.Now.Ticks;
            var needRemove = _unitkeysDict.Where(r => new TimeSpan(tick - r.Value).TotalSeconds > UnitTime);
            if (needRemove != null || needRemove.Count() == 0)
            {
                var keys = needRemove.Select(r => r.Key);
                foreach (var key in keys)
                {
                    _unitkeysDict.Remove(key);
                }
            }
        }

        Dictionary<string, long> _unitkeysDict = new Dictionary<string, long>();

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int Push(QueueMessage msg)
        {
            if (!_unitkeysDict.Keys.Any(k => k.Equals(msg.Key)))
            {
                var dt = DateTime.Now;
                _DefaultDb.SortedSetAdd(QueueName, msg.Content, dt.AddSeconds(DelayTime).Ticks , CommandFlags.None);
                _unitkeysDict.Add(msg.Key, dt.Ticks);
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// 出队
        /// dt>=score出对
        /// </summary>
        public List<QueueMessage> Pop()
        {
            List<QueueMessage> rt = new List<QueueMessage>();
            var values = _DefaultDb.SortedSetRangeByScore(QueueName, DateTime.Now.Ticks, double.MaxValue);
            if(values != null && values.Count() > 0)
            {
                foreach (var v in values)
                {
                    rt.Add(new QueueMessage() { Content = v });
                }
            }
            return rt;
        }
    }

    /// <summary>
    /// 入队消息
    /// </summary>
    public class QueueMessage
    {
        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 内容,格式化json或自定义字符串
        /// </summary>
        public string Content { get; set; }
    }
}
