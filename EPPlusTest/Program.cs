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
                       DynamicDatas = new Dictionary<string, dynamic>(){ { "money_wx", 8.88M }, { "money_aiPay", 8.88M } }
                },
                new ExportSearchCashierReport(){
                       Begintime = DateTime.Now,
                       Endtime = DateTime.Now.AddHours(-3),
                       StoreName = "熊大汉堡",
                       PayAmount = 12.35M,
                       PayAmountExceptAll = 98.12M,
                       DynamicDatas = new Dictionary<string, dynamic>(){ { "money_cash", 45.12M }, { "money_aiPay", 8.88M } }
                }
            };
            List<HeaderColumnDto> headers = new List<HeaderColumnDto>() { 
                new HeaderColumnDto(){  Key = "money_wx",    Title = "微信支付"},
                new HeaderColumnDto(){  Key = "money_aiPay", Title = "支付宝支付"},
                new HeaderColumnDto(){  Key = "money_mt",    Title = "秒通支付", Childrens = new List<HeaderColumnDto>(){
                    new HeaderColumnDto(){  Key = "money_mt_1",    Title = "秒通支付_1"},
                    new HeaderColumnDto(){  Key = "money_mt_2",    Title = "秒通支付_2"},
                    new HeaderColumnDto(){  Key = "money_mt_3",    Title = "秒通支付_3"},
                } },
            };
            ExportSearchCashierReportExtension.ToExcle1(list,headers);
            list.ToExcle2(headers);
            Console.ReadKey();
        }
    }
}
