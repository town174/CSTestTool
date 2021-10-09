using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace EPPlusTest
{
    public interface IDynamicData
    {
        Dictionary<string, dynamic> DynamicDatas { get; set; }
    }

    public class ExportSearchCashierReport : IDynamicData
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

    /// <summary>
    /// 表头名
    /// </summary>
    public class HeaderColumnDto
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 二级表头
        /// </summary>
        public List<HeaderColumnDto> Childrens { get; set; } = new List<HeaderColumnDto>();
    }
    public static class ExportSearchCashierReportExtension
    {
        public static void ToExcle(this IList<ExportSearchCashierReport> data)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(@"EPPlusTest.xlsx"))) 
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("test");//创建worksheet
                worksheet.Cells[2, 2].LoadFromCollection(data);
                package.Save();
            }
        }

        public static void ToExcle1<T>(List<T> data, List<HeaderColumnDto> headers) where T : IDynamicData
        {
            var ps = typeof(T).GetProperties();
            return;
        }

        /// <summary>
        ///  动态类型最后解析, 注意顺序
        /// </summary>
        /// <param name="data"></param>
        public static void ToExcle2<T>(this IList<T> data, List<HeaderColumnDto> headers) where T: IDynamicData
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo($"EPPlusTest{DateTime.Now.Millisecond}.xlsx")))
            {
                //创建worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("test2");
                //设置标题
                var ps = typeof(T).GetProperties();
                int startCol = 1;
                int startRow = 1;
                //静态标题
                foreach (var p in ps)
                {
                    var title = "";
                    foreach (var attr in p.CustomAttributes)
                    {
                        if (attr.AttributeType.Equals(typeof(DisplayNameAttribute)) && attr.ConstructorArguments.Count > 0)
                        {
                            title = attr.ConstructorArguments[0].Value.ToString();
                            continue;
                        }
                    }
                    if (title == "" && p.Name != nameof(IDynamicData.DynamicDatas))
                    {                        
                        title = p.Name;
                    }
                    worksheet.Cells[startRow, startCol++].Value = title;
                    Console.WriteLine(title + $"{startCol}");
                }
                //动态标题
                var dp = ps.FirstOrDefault(x => x.Name == nameof(IDynamicData.DynamicDatas));
                var titleDict = new Dictionary<string, int>();
                if (dp != null)
                {
                    SetDynamicTitle(headers, worksheet.Cells, startRow, startCol, titleDict);
                }

                //设置内容 按顺序插入和标题顺序匹配
                foreach (var d in data)
                {
                    //静态内容
                    startRow += 1;
                    startCol = 1;
                    foreach (var p in ps)
                    {
                        if (p.Name == nameof(IDynamicData.DynamicDatas)) continue;
                        worksheet.Cells[startRow, startCol++].Value = GetObjectPropertyValue(d, p.Name);
                    }
                    //动态内容
                    if (dp != null && d.DynamicDatas != null)
                    {
                        var dynamics = (Dictionary<string, dynamic>)GetObjectPropertyObject(d, dp.Name);
                        foreach (var item in dynamics)
                        {
                            if(titleDict.ContainsKey(item.Key))
                            {
                                worksheet.Cells[startRow, titleDict[item.Key]].Value = item.Value;
                            }
                            else
                            {
                                worksheet.Cells[startRow, startCol++].Value = item.Value;
                            }
                        }
                    }
                }

                package.Save();
            }
        }

        /// <summary>
        /// 返回head，col对应关系！
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="cells"></param>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// <param name="TitleDict"></param>
        private static void SetDynamicTitle(List<HeaderColumnDto> headers, ExcelRange cells, int startRow, int startCol, Dictionary<string, int> TitleDict)
        {
            int col = startCol;
            foreach (var  h in headers)
            {
                if (h.Childrens.Count == 0)
                {
                    TitleDict.Add(h.Key, col);
                    cells[startRow, col++].Value = h.Title;
                }
                else
                    SetDynamicTitle(h.Childrens, cells, startRow, col, TitleDict);
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
        /// 根据属性名获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="propertyname"></param>
        /// <returns></returns>
        private static object GetObjectPropertyObject<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            object o = property.GetValue(t, null);
            if (o == null) return null;
            return o;
        }
    }
}
