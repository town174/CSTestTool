using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSamples.LogicCore.Modules
{
    /// <summary>
    /// 模块特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute
    {
        /// <summary>
        /// 模块构造函数
        /// </summary>
        /// <param name="code">模块编码</param>
        /// <param name="name">模块名称</param>
        /// <param name="icon">ICON</param>
        public ModuleAttribute(ModuleType moduleType, string code, string name, string icon = "")
        {
            this.moduleType = moduleType;
            this.code = code;
            this.name = name;
            this.icon = icon;
        }

        #region private

        private ModuleType moduleType;
        private string code;
        private string name;
        private string icon;

        #endregion

        #region 只读属性

        public string Code { get { return code; } }

        /// <summary>
        /// 图标
        /// </summary>
        public string ICON
        {
            get { return icon; }
        }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        public ModuleType ModuleType
        {
            get { return moduleType; }
        }

        public string ModuleTypeName
        {
            get { return GetEnumAttrbute.GetDescription(ModuleType).Caption; }
        }


        #endregion
    }

    /// <summary>
    /// 模块类型特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DescriptionAttribute : Attribute
    {
        protected string caption = string.Empty;
        protected string remark = string.Empty;

        public string Caption { get { return caption; } }
        public string Remark { get { return remark; } }

        public DescriptionAttribute(string caption, string remark = "BorderAll")
        {
            this.caption = caption;
            this.remark = remark;
        }

    }
}
