using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;

namespace EPPlusTest
{
    public class ExportSearchCashierReport
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [DisplayName("查询开始日期")]
        public DateTime? Begintime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DisplayName("查询结束日期")]
        public DateTime? Endtime { get; set; }

        /// <summary>
        /// 公园名称
        /// </summary>
        [DisplayName("公园名称")]
        public string ParkName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [DisplayName("交易日期")]
        public DateTime DailyDate { get; set; }

        /// <summary>
        /// 收银员
        /// </summary>
        [DisplayName("收银员")]
        public string CashierName { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        [DisplayName("门店名称")]
        public string StoreName { get; set; }


        /// <summary>
        /// 门店属性 文本
        /// </summary>
        [DisplayName("门店属性")]
        public string StoreType { get; set; }

        /// <summary>
        /// 商户经营类型
        /// </summary>
        [DisplayName("经营模式")]
        public string BusinessModel { get; set; }

        /// <summary>
        /// 实收金额 汇总
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 秒通折后不含券总额
        /// </summary>
        public decimal PayAmountExceptAll { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<string, dynamic> DynamicDatas { get; set; } = new Dictionary<string, dynamic>();
    }

    public static class ExportSearchCashierReportExtension
    {
        public static void ToExcle(this IList<ExportSearchCashierReport> data)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(@"EPPlusTest.xlsx"))) 
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("test1");//创建worksheet
                worksheet.Cells[2, 2].LoadFromCollection(data);
                package.Save();
            }
        }

        /// <summary>
        /// todo  针对动态类型再次解析
        /// </summary>
        /// <param name="data"></param>
        public static void ToExcle2(this IList<ExportSearchCashierReport> data)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(@"EPPlusTest.xlsx")))
            {
                //创建worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("test2");
                //设置标题
                var ps = typeof(ExportSearchCashierReport).GetProperties();
                int i = 0;
                foreach (var p in ps)
                {
                    var title = "";
                    foreach (var attr in p.CustomAttributes)
                    {
                        if (attr.AttributeType.Equals(typeof(DisplayNameAttribute)) && attr.ConstructorArguments.Count > 0)
                        {
                            title = attr.ConstructorArguments[0].Value.ToString();
                            break;
                        }
                    }
                    if(title == "") title = p.Name;
                    worksheet.Cells[1, ++i].Value = title;
                    Console.WriteLine(title + $"{i}");
                }
                //设置内容 按顺序插入
                var dc = data.Count;
                for (int k = 0; k < dc; k++)
                {
                    for (int m = 0; m < ps.Length; m++)
                    {
                        var value = GetObjectPropertyValue(data[k], ps[m].Name);
                        //value = data[k].GetType().GetProperty(ps[m].Name).GetValue(data[k], null).ToString();
                        worksheet.Cells[2 + k, ++m].Value = value;
                    }
                }
                
                package.Save();
            }
        }

        /// <summary>
        /// 根据属性名获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        private static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            object o = property.GetValue(t, null);
            if (o == null) return string.Empty;
            return o.ToString();
        }

        /// <summary>
        /// 根据属性名获取标题
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        private static string GetObjectPropertyName<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            var attr = property.GetCustomAttribute(typeof(DisplayNameAttribute));
            //todo  获取对应值            
            object o = property.GetValue(t, null);
            if (o == null) return string.Empty;
            return o.ToString();
        }
    }
}
