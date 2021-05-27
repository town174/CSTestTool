using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfSamples.View
{
    /// <summary>
    /// XamlView.xaml 的交互逻辑
    /// </summary>
    public partial class XamlView : UserControl
    {
        public XamlView()
        {
            InitializeComponent();
            InitRb();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Hyperlink link = sender as Hyperlink;
            string url = "https://docs.microsoft.com/zh-cn/dotnet/desktop/wpf/xaml/?view=netdesktop-5.0";
            Process.Start(new ProcessStartInfo(url));
        }

        private void InitRb()
        {
            string content = "语法:对象,属性,集合,内容,事件\r\n大小写严格\r\n" +
                "标记扩展:提供特性语法的值时，大括号（{ 和 }）表示标记扩展用法" +
                "命名元素\r\n附加属性附加事件";
            rb.AppendText(content);
        }
    }
}
