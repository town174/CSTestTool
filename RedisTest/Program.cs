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
            DelayQueue queue = new DelayQueue("delayQueue", 3, 10);
            int i = 100;
            while (i-- > 0)
            {
                var k = new Random().Next(1, 6).ToString();
                var m = new QueueMessage() {
                    Key = k,
                    Content = k + "___" + DateTime.Now.ToString()
                };
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{DateTime.Now.ToString()}: 消息{m.Key}入队, 内容:{m.Content} 结果:{queue.Push(m)}");
                //if(DateTime.Now.Second % 2 == 0)
                //{
                    var tmps = queue.Pop();
                    foreach (var item in tmps)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{DateTime.Now.ToString()}: 消息出队, 内容:{item.Content}");
                    }
                //}
                Thread.Sleep(500);
            }
            Console.ReadKey();
        }
    }
}
