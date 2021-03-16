using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NetAPI.Protocol.VRP;
using NetAPI;

namespace VbtCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "rd";
            var port = new Rs232Port("COM1", BaudRate.R9600);
            Reader reader = new Reader(name,port);
            var rt = reader.Connect();
            Console.WriteLine($"{rt.IsSucessed},{rt.ErrorInfo.ErrCode},{rt.ErrorInfo.ErrMsg}");
            Console.ReadKey();
        }
    }
}
