using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSamples.LogicCore.Modules
{
    /// <summary>
    /// 模块类型
    /// </summary>
    public enum ModuleType
    {
        //[Description("产品信息", "\xe642")]
        //Product,

        //[Description("生产计划", "\xe63a")]
        //ProductionPlan,

        //[Description("仓库管理", "\xe81b")]
        //WarehouseManagement,

        //[Description("设备管理", "\xe64a")]
        //EquipmentManagement,

        //[Description("插件管理", "\xe63b")]
        //PluginManagement,

        //[Description("参数配置", "\xe6ee")]
        //ParameterConfiguration,

        //[Description("调试软件", "\xe629")]
        //DebuggingSoftware,

        //[Description("演示平台", "\xe667")]
        //DemoPlatform,

        //[Description("演示软件", "\xe6a0")]
        //DemoSoftware,

        //[Description("标签功能", "\xe75c")]
        //LabelFunction,

        //[Description("公共数据", "\xe610")]
        //PublicData,

        //[Description("文档中心", "\xe64d")]
        //DocumentCenter,

        [Description("0_KP", "\xe74b")]
        KnowledgePoint,

        [Description("1_FRM", "\xe644")]
        Framework,

        [Description("2_SAMPLE", "\xe64d")]
        Sample,
    }
}
