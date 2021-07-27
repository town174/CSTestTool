using System;
using System.Collections.Generic;

namespace EPPlusTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<ExportSearchCashierReport> list = new List<ExportSearchCashierReport>() {
                new ExportSearchCashierReport(){
                       Begintime = DateTime.Now,
                       Endtime = DateTime.Now.AddHours(-3),
                       StoreName = "test store",
                       PayAmount = 12.35M,
                       PayAmountExceptAll = 0,
                       DynamicDatas = new Dictionary<string, dynamic>(){ { "dym", 8.88M } }
                }
            };
            list.ToExcle();
            list.ToExcle2();
            Console.ReadKey();
        }
    }
}
