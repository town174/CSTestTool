using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfTest.Dependency;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 路由事件
    /// </summary>
    public partial class MainWindow : Window
    {
        Student stu = new Student();
        public MainWindow()
        {
            InitializeComponent();
            //添加事件监听RouterEvent
            //不需要显示绑定某个控件~~, 解决发布者和订阅者耦合关系
            //绑定某个类型事件
            //this.GridA.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonClick));
            this.StackPanelA.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonClick));
            //设置数据绑定
            Binding binding = new Binding("Text") { Source = this.tbdp1 };
            BindingOperations.SetBinding(stu, Student.NameProperty, binding);
            this.tbdp2.SetBinding(TextBox.TextProperty, binding);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((sender as Panel).Name);
            //设置为handle=true  事件不再向上传播
            e.Handled = true;
        }

        private void ButtonMove(object sender, RoutedEventArgs e)
        {
            var b = e.Source as Button;
            var c = sender as Panel;
            MessageBox.Show($"capture:{c.Name}, source:{b.Name}");
        }

        private void btDp_Click(object sender, RoutedEventArgs e)
        {
            
            //stu.SetValue(Student.NameProperty, this.tbdp1.Text);
            //this.tbdp2.Text = (string)stu.GetValue(Student.NameProperty);
        }
    }

    public class ReportTimeEventArgs : RoutedEventArgs
    {
        public ReportTimeEventArgs(RoutedEvent routedEvent, object source):base(routedEvent,source)
        {

        }

        public DateTime ReportTime { get; set; }
    }

    public class TimeButton : Button
    {
        //声明并注册路由事件
        /*
         * 1、第一个参数ReportTime 为路由事件的名称
         * 2、第二个参数是路由事件的策略，包括Bubble冒泡式，Tunnel隧道式，Direct直达式（和直接事件类似）
         * 3、第三个参数用于指定事件处理器的类型
         * 4、第四个参数用于指定事件的宿主是哪一种类型
         */
        public static readonly RoutedEvent ReportTimeEvent = EventManager.RegisterRoutedEvent
            ("ReportTime", RoutingStrategy.Bubble, typeof(EventHandler<ReportTimeEventArgs>), typeof(TimeButton));

        //CLR事件包装器
        public event RoutedEventHandler ReportTime
        {
            add { this.AddHandler(ReportTimeEvent, value); }
            remove { this.RemoveHandler(ReportTimeEvent, value); }
        }

        //激发路由事件，借用Click事件的激发方法
        protected override void OnClick()
        {
            base.OnClick();

            ReportTimeEventArgs args = new ReportTimeEventArgs(ReportTimeEvent, this);
            args.ReportTime = DateTime.Now;
            this.RaiseEvent(args);
        }
    }
}
