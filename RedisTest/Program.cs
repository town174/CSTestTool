using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DelayQueue queue = new DelayQueue("delayQueue", 3, 5);
            while (true)
            {
                var m = new QueueMessage() {
                    Key = "test",
                    Content = DateTime.Now.ToString()
                };
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"消息{m.Key}入队, 结果:{queue.Push(m)}");
                var tmps = queue.Pop();
                foreach (var item in tmps)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"消息{item.Key}出队, 内容:{item.Content}");
                }
                Thread.Sleep(1000);
            }
        }
    }
}
