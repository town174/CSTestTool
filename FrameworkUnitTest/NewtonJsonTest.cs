using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FrameworkUnitTest
{
    [TestClass]
    public class NewtonJsonTest
    {
        /// <summary>
        /// 输出格式化
        /// </summary>
        [TestMethod]
        public void Indented()
        {
            var reportModel = new ReportModel()
            {
                ProductName = "法式小众设计感长裙气质显瘦纯白色仙女连衣裙",
                TotalPayment = 100,
                TotalCustomerCount = 2,
                TotalProductCount = 333
            };

            var json = JsonConvert.SerializeObject(reportModel, Formatting.Indented);
            Debug.WriteLine(json, "Indented");
        }

        /// <summary>
        /// 忽略未赋值字段
        /// </summary>
        [TestMethod]
        public void IgnoreValue()
        {
            var reportModel = new ReportModel()
            {
                ProductName = "法式小众设计感长裙气质显瘦纯白色仙女连衣裙",
                TotalPayment = 100
            };

            var json = JsonConvert.SerializeObject(reportModel, Formatting.Indented,
                new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
            Debug.WriteLine(json, "IgnoreValue");
        }

        /// <summary>
        /// 按照驼峰蛇形命名法输出
        /// </summary>
        [TestMethod]
        public void NameCaseOutput()
        {
            var reportModel = new ReportModel()
            {
                ProductName = "法式小众设计感长裙气质显瘦纯白色仙女连衣裙",
                TotalPayment = 100,
                TotalCustomerCount = 2,
                TotalProductCount = 333
            };

            var json1 = JsonConvert.SerializeObject(reportModel, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            Debug.WriteLine(json1, "CamelCase");

            var json2 = JsonConvert.SerializeObject(reportModel, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                });
            Debug.WriteLine(json2, "SnakeCase");
        }

        /// <summary>
        /// json字段和model字段映射
        /// </summary>
        [TestMethod]
        public void NameMapping()
        {

            var json = "{'title':'法式小众设计感长裙气质显瘦纯白色仙女连衣裙','customercount':1000,'totalpayment':100.0,'productcount':10000}";
            var reportModel = JsonConvert.DeserializeObject<ReportModelMapper>(json);
        }

        /// <summary>
        /// 过滤指定字段
        /// 正向过滤：默认显示，手工标记不显示
        /// 反向过滤: 默认不显示，手工标记显示
        /// </summary>
        [TestMethod]
        public void FliterValue()
        {
            var reportModel1 = new ReportModelFliter1()
            {
                ProductName = "法式小众设计感长裙气质显瘦纯白色仙女连衣裙",
                TotalPayment = 100
            };

            var json1 = JsonConvert.SerializeObject(reportModel1, Formatting.Indented);
            Debug.WriteLine(json1, "FltOptOut");

            var reportModel2 = new ReportModelFliter2()
            {
                ProductName = "法式小众设计感长裙气质显瘦纯白色仙女连衣裙",
                TotalPayment = 100,
                TotalCustomerCount = 10
            };

            var json2 = JsonConvert.SerializeObject(reportModel2, Formatting.Indented);
            Debug.WriteLine(json2, "FltOptIn");
        }

        /// <summary>
        /// 多个json 合并到 一个Model
        /// </summary>
        [TestMethod]
        public void MutliJson()
        {

            var json1 = "{'ProductName':'法式小众设计感长裙气质显瘦纯白色仙女连衣裙'}";
            var json2 = "{'TotalCustomerCount':1000,'TotalPayment':100.0,'TotalProductCount':10000}";

            var reportModel = new ReportModel();

            JsonConvert.PopulateObject(json1, reportModel);
            JsonConvert.PopulateObject(json2, reportModel);
        }

        /// <summary>
        /// 弱语言解析
        /// 不需要定义类
        /// </summary>
        [TestMethod]
        public void WeakParse()
        {
            var json = @"{
                           'DisplayName': '新一代算法模型',
                           'CustomerType': 1,
                           'Report': {
                             'TotalCustomerCount': 1000,
                             'TotalTradeCount': 50
                           },
                           'CustomerIDHash': [1,2,3,4,5]
                         }";

            var dict = JsonConvert.DeserializeObject<Dictionary<object, object>>(json);

            var report = dict["Report"] as JObject;
            var totalCustomerCount = report["TotalCustomerCount"];

            Debug.WriteLine($"totalCustomerCount={totalCustomerCount}", "WeakParse");

            var arr = dict["CustomerIDHash"] as JArray;
            var list = arr.Select(m => m.Value<int>()).ToList();

            Debug.WriteLine($"list={string.Join(",", list)}", "WeakParse");
        }

        /// <summary>
        /// enum美化，显示描述而不是值
        /// </summary>
        [TestMethod]
        public void EnumBtf()
        {
            var model = new ThreadModel()
            {
                ThreadStateEnum = System.Threading.ThreadState.Running,
                TaskStatusEnum = TaskStatus.RanToCompletion
            };

            var json = JsonConvert.SerializeObject(model);

            Debug.WriteLine(json, "EnumBtf");
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        [TestMethod]
        public void FormatDate()
        {
            var json = JsonConvert.SerializeObject(new Order()
            {
                OrderTitle = "女装大佬",
                Created = DateTime.Now
            }, new JsonSerializerSettings
            {
                DateFormatString = "yyyy年/MM月/dd日",
            });

            Debug.WriteLine(json, "FormatDate");
        }

        /// <summary>
        /// 全局设置
        /// </summary>
        [TestMethod]
        public void GlobalSetting()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = "yyyy年/MM月/dd日"
                };
                return settings;
            };

            var order = new Order() { OrderTitle = "女装大佬", Created = DateTime.Now };

            var json1 = JsonConvert.SerializeObject(order);
            var json2 = JsonConvert.SerializeObject(order);

            Debug.WriteLine(json1, "GlobalSetting");
            Debug.WriteLine(json2, "GlobalSetting");
        }

        /// <summary>
        /// 未知字段报错
        /// </summary>
        [TestMethod]
        public void AlarmUnkownField()
        {
            var json = "{'OrderTitle':'女装大佬', 'Created':'2020/6/23','Memo':'订单备注'}";

            var order = JsonConvert.DeserializeObject<Order>(json, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error
            });

            Debug.WriteLine(order);
        }

        /// <summary>
        /// 提取未知字段
        /// </summary>
        [TestMethod]
        public void GetUnkownField()
        {
            var json = "{'OrderTitle':'女装大佬', 'Created':'2020/6/23','Memo':'订单备注'}";

            var order = JsonConvert.DeserializeObject<Order>(json);

            Debug.WriteLine(order);
        }
    }

    class Order
    {
        public string OrderTitle { get; set; }
        public DateTime Created { get; set; }
        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData;
    }

    class ThreadModel
{
    public System.Threading.ThreadState ThreadStateEnum { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TaskStatus TaskStatusEnum { get; set; }
}

[JsonObject(MemberSerialization.OptOut)]
class ReportModelFliter1
{
    public string ProductName { get; set; }
    [JsonIgnore]
    public int TotalCustomerCount { get; set; }
    [JsonIgnore]
    public decimal TotalPayment { get; set; }
    [JsonIgnore]
    public int TotalProductCount { get; set; }
}

[JsonObject(MemberSerialization.OptIn)]
class ReportModelFliter2
{
    [JsonProperty]
    public string ProductName { get; set; }
    [JsonProperty]
    public int TotalCustomerCount { get; set; }
    public decimal TotalPayment { get; set; }
    public int TotalProductCount { get; set; }
}

class ReportModelMapper
{
    [JsonProperty("title")]
    public string ProductName { get; set; }
    [JsonProperty("customercount")]
    public int TotalCustomerCount { get; set; }
    [JsonProperty("totalpayment")]
    public decimal TotalPayment { get; set; }
    [JsonProperty("productcount")]
    public int TotalProductCount { get; set; }
}

class ReportModel
{
    public string ProductName { get; set; }
    public int TotalCustomerCount { get; set; }
    public decimal TotalPayment { get; set; }
    public int TotalProductCount { get; set; }
}
}
