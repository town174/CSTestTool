using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace AsyncTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now.ToString()} Main: {Thread.CurrentThread.ManagedThreadId} Before");
            try
            {
                AsyncMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString()} Exception: {ex.ToString()}");
            }
            //AsyncHttp();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{DateTime.Now.ToString()} Main: {Thread.CurrentThread.ManagedThreadId} After");
            }
            Console.ReadKey();
        }

        static async Task<string> AsyncHttp()
        {
            HttpWebRequest request = WebRequest.Create("https://www.baidu.com") as HttpWebRequest;
            request.Method = "Get";
            HttpWebResponse respone = await request.GetResponseAsync() as HttpWebResponse;
            Thread.Sleep(5000);
            Console.WriteLine($"{DateTime.Now.ToString()} Time-consuming: {Thread.CurrentThread.ManagedThreadId} server:{respone.Server}");
            Console.WriteLine($"{DateTime.Now.ToString()} Time-consuming: {Thread.CurrentThread.ManagedThreadId} stausCode:{respone.StatusCode}");
            Console.WriteLine($"{DateTime.Now.ToString()} Time-consuming: {Thread.CurrentThread.ManagedThreadId} tostring:{respone.ToString()}");
            return $"{DateTime.Now.ToString()} AsyncHttp complete {respone.ResponseUri.AbsolutePath}";
        }

        static async Task AsyncMethod()
        {
            var ResultFromTimeConsumingMethod = TimeConsumingMethod();
            string Result1 = await ResultFromTimeConsumingMethod + " " + Thread.CurrentThread.ManagedThreadId;            
            Console.WriteLine(Result1);            
            //返回值是Task的函数可以不用return
        }

        //这个函数就是一个耗时函数，可能是IO操作，也可能是cpu密集型工作。
        static async Task<string> TimeConsumingMethod()
        {
            var task = Task.Run(() => {
                Console.WriteLine($"{DateTime.Now.ToString()} I am TimeConsumingMethod {Thread.CurrentThread.ManagedThreadId} before ");
                Thread.Sleep(5000);
                Console.WriteLine($"{DateTime.Now.ToString()} I am TimeConsumingMethod {Thread.CurrentThread.ManagedThreadId } after Sleep(5000) ");

                try
                {
                    throw new Exception("cst exception");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now.ToString()} Exception: {ex.ToString()}");
                }
                return $"{DateTime.Now.ToString()} I am TimeConsumingMethod complete";
            });

            string Result2 = await AsyncHttp();
            Console.WriteLine(Result2);

            return task.Result;
        }
    }
}
