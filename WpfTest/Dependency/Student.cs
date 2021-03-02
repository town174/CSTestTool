using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfTest.Dependency
{
    /// <summary>
    /// 1 继承DependencyObject
    /// 2 注册对应依赖属性
    /// 3 包装成普通属性(可选)
    /// </summary>
    public class Student : DependencyObject
    {
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        //register参数依次是
        // 要注册的依赖属性的名称。 名称必须在所有者类型的注册命名空间中是唯一的。
        // 属性的类型。
        // 正在注册依赖属性的所有者类型。
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(Student));
    }
}
