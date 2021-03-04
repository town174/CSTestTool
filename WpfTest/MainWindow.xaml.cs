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
using WpfTest.Entity;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 路由事件
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            //添加事件监听RouterEvent
            //不需要显示绑定某个控件~~, 解决发布者和订阅者耦合关系
            //绑定某个类型事件
            //this.GridA.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonClick));
            this.StackPanelA.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonClick));
            //依赖属性
            Student stu = new Student();
            Binding binding = new Binding("Text") { Source = this.tbdp1 };
            BindingOperations.SetBinding(stu, Student.NameProperty, binding);
            this.tbdp2.SetBinding(TextBox.TextProperty, binding);
            //初始化数据绑定
            InitDataBinding();
            //初始化命令
            InitCommand();
        }

        void InitDataBinding()
        {
            //绑定对象
            Teacher tec = new Teacher { Name = "echo" };
            this.tb2.SetBinding(TextBox.TextProperty, new Binding("Name") { Source = tec });

            //绑定集合
            List<Teacher> stus = new List<Teacher>() {
                new Teacher(){ Name = "abby",Age = "20"},
                new Teacher(){ Name = "john",Age = "35"}
            };
            lsView.ItemsSource = stus;
            lsView.DisplayMemberPath = "Name";
            tbAge.SetBinding(TextBox.TextProperty, 
                new Binding("SelectedItem.Age") { Source = this.lsView });
        }

        //命令使用太麻烦
        private void InitCommand()
        {
            //声明定义命令
            RoutedCommand clearCmd = new RoutedCommand("Clear", typeof(MainWindow));
            //设置命令源,指定快捷键
            this.btnClear.Command = clearCmd;
            clearCmd.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Alt));
            //设置命令目标
            this.btnClear.CommandTarget = this.tbCmdConent;
            //创建命令关联
            CommandBinding cb = new CommandBinding(clearCmd);
            cb.CanExecute += Cb_CanExecute;
            cb.Executed += Cb_Executed;
            //外围容器添加命令关联
            this.spCmd.CommandBindings.Add(cb);
        }

        //命令执行内容
        private void Cb_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.tbCmdConent.Clear();
            e.Handled = true;
        }

        //命令执行条件
        private void Cb_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbCmdConent.Text)) e.CanExecute = false;
            else e.CanExecute = true;
            e.Handled = true;
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

        }

        private void btnCar_Click(object sender, RoutedEventArgs e)
        {
            CarListWnd w = new CarListWnd();
            w.ShowDialog();
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(this.tbCmd1.Text);
            e.Handled = false;
        }

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.tbCmd2.Text = this.tbCmd1.Text;
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
