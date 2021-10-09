using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace CmdTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(ManType.man.ToInt());
            Console.WriteLine(ManType.woman.ToInt());

            //Task1();
            //Console.WriteLine($"main thread: {Thread.CurrentThread.ManagedThreadId}");
            //Console.ReadKey();

            //Console.WriteLine(new DateTimeOffset(DateTime.Now));
            //Console.WriteLine(DateTimeOffset.Now);
            //Console.WriteLine(DateTimeOffset.UtcNow);
            //Console.ReadKey();

            //string strInput = @"E:\智能书架后台程序\HaiHeng.SmartBookShelf.Host.exe";
            //Process p = new Process();
            ////设置要启动的应用程序
            //p.StartInfo.FileName = "cmd.exe";
            ////是否使用操作系统shell启动
            //p.StartInfo.UseShellExecute = false;
            //// 接受来自调用程序的输入信息
            //p.StartInfo.RedirectStandardInput = true;
            ////输出信息
            //p.StartInfo.RedirectStandardOutput = true;
            //// 输出错误
            //p.StartInfo.RedirectStandardError = true;
            ////不显示程序窗口
            //p.StartInfo.CreateNoWindow = true;
            ////启动程序
            //p.Start();

            ////向cmd窗口发送输入信息
            //p.StandardInput.WriteLine(strInput + "&exit"); //+ "&exit");
            //p.StandardInput.AutoFlush = true;

            ////获取输出信息
            //string strOuput = p.StandardOutput.ReadToEnd();
            ////等待程序执行完退出进程
            //p.WaitForExit();
            //p.Close();

            //Console.WriteLine(strOuput);

            Console.ReadKey();
        }

        static async Task Task1()
        {
             Task.Factory.StartNew(() => {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"task 1, {i} task thread:{Thread.CurrentThread.ManagedThreadId}");
                }
            });

            await Task.Factory.StartNew(() => {
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(500);
                    Console.WriteLine($"task 2, {i} task thread:{Thread.CurrentThread.ManagedThreadId}");
                }
            });
        }

    }

    public enum ManType
    {
        man = 0,
        woman
    }

    public static class EnumExt
    {
        public static int ToInt(this Enum value)
        {
            return value.GetHashCode();
        }
    }
}
