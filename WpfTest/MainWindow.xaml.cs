using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WpfTest.Dependency;
using WpfTest.Entity;
using System.Collections.Specialized;
using WpfTest.Validater;
using WpfTest.Convert;
using System.ComponentModel;

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
            //初始化控件
            //InitControl();
            InitMutilLang();
        }

        void InitMutilLang()
        {

        }

        void InitControl()
        {
            //添加head控件
            HeaderedContentControl hc = new HeaderedContentControl();
            hc.Header = "header";
            hc.Content = new Button() { Content = "Button", Width = 100, HorizontalAlignment = HorizontalAlignment.Center };
            hc.Style = this.FindResource("HcTemplate") as Style;

            //加入主界面
            StackPanel sp = new StackPanel();
            sp.Children.Add(hc);
            TabItem ti = new TabItem();
            ti.Header = "控件";
            ti.Content = sp;
            this.TcBase.Items.Add(ti);
        }

        List<Teacher> tecs1 = new List<Teacher>();
        ObservableCollection<Teacher> tecs2 = new ObservableCollection<Teacher>();
        DataTable dt = new DataTable();
        void InitDataBinding()
        {
            //绑定对象
            Teacher tec = new Teacher { Name = "echo" };
            this.tb2.SetBinding(TextBox.TextProperty, new Binding("Name") { Source = tec });

            //绑定list
            tecs1 = new List<Teacher>() {
                new Teacher(){ Name = "abby", Age = "20"},
                new Teacher(){ Name = "john", Age = "35"}
            };
            lsView1.ItemsSource = tecs1;
            lsView1.DisplayMemberPath = "Name";
            tbAge1.SetBinding(TextBox.TextProperty, 
                new Binding("SelectedItem.Age") { Source = this.lsView1 });

            //绑定observableCollection
            tecs2 = new ObservableCollection<Teacher>() {
                new Teacher(){ Name = "abby", Age = "20", Class="5"},
                new Teacher(){ Name = "john", Age = "35", Class="10"},
                new Teacher(){ Name = "lucy", Age = "40", Class="8"}
            };
            tecs2.CollectionChanged += Tecs2_CollectionChanged;
            //lsView2.DataContext = tecs2;
            lsView2.ItemsSource = tecs2;            
            lsView2.DisplayMemberPath = "Name";
            tbAge2.SetBinding(TextBox.TextProperty,
                new Binding("SelectedItem.Age") { Source = this.lsView2 });

            //绑定DataTable
            dt.Columns.Add("Age", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Class", typeof(string));
            foreach (var item in tecs2)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.Age;
                dr[1] = item.Name;
                dr[2] = item.Class;
                dt.Rows.Add(dr);
            }
            lsView3.DataContext = dt;
            lsView3.SetBinding(ListView.ItemsSourceProperty, new Binding());
            lsView3.Items.SortDescriptions.Add(new SortDescription("Class", ListSortDirection.Ascending));

            //绑定XmlDataProvider, xml作为数据传输保存格式应用太少了
            XmlDataProvider xdp = new XmlDataProvider();
            xdp.Source = new Uri("/Assert/Data/Depertment.xml",UriKind.Relative);
            xdp.XPath = @"/Department";
            tvDepartment.DataContext = xdp;
            tvDepartment.SetBinding(TreeView.ItemsSourceProperty, new Binding());

            //绑定相对资源
            RelativeSource rs = new RelativeSource();
            rs.AncestorLevel = 1;
            rs.AncestorType = typeof(StackPanel);
            this.tbRelative.SetBinding(TextBlock.TextProperty, new Binding("Name") { RelativeSource = rs });

            #region 绑定校验器
            //绑定校验器
            Binding bind = new Binding("Value") { ElementName = "sldValid" };
            //设置更新绑定源方式
            bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //设置校验失败通知
            bind.NotifyOnValidationError = true;
            //创建校验器
            RangeValidator rvr = new RangeValidator();
            //设置校验
            rvr.ValidatesOnTargetUpdated = true;
            bind.ValidationRules.Add(rvr);
            //绑定
            this.tbValid.SetBinding(TextBox.TextProperty, bind);
            //设置校验失败处理事件
            this.tbValid.AddHandler(Validation.ErrorEvent, new RoutedEventHandler((s, e) => {
                if (Validation.GetErrors(this.tbValid).Count > 0)
                {
                    this.tbValid.ToolTip = Validation.GetErrors(this.tbValid)[0].ErrorContent.ToString();
                    e.Handled = true;
                }
            }));
            //设置源更新时，移除tooltips
            this.tbValid.TargetUpdated += (s, e) => {
                (s as TextBox).ToolTip = "";
            };
            this.tbValid.SourceUpdated += (s, e) => {
                (s as TextBox).ToolTip = "";
            };
            #endregion

            //绑定多路器
            Binding bm1 = new Binding("Text") { ElementName = "tbm1" };
            Binding bm2 = new Binding("Text") { ElementName = "tbm2" };
            MultiBinding mb = new MultiBinding();
            mb.Converter = new MutilConvert();
            mb.Bindings.Add(bm1);
            mb.Bindings.Add(bm2);
            this.btnMTest.SetBinding(Button.IsEnabledProperty, mb);
        }

        private void Tecs2_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                MessageBox.Show($"新增tec, {e.NewItems[0].ToString()}");
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                MessageBox.Show($"移除tec, {e.OldItems[0].ToString()}");
            }
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

        private void btnAddTec_Click(object sender, RoutedEventArgs e)
        {
            tecs1.Add(new Teacher() { Name = "mike", Age = "40" });
        }

        private void btnAddTec2_Click(object sender, RoutedEventArgs e)
        {
            tecs2.Add(new Teacher() { Name = "mike", Age = "40" });
        }

        private void btnRemoveTec2_Click(object sender, RoutedEventArgs e)
        {
            if(tecs2.Count >= 1)
                tecs2.RemoveAt(tecs2.Count - 1);
        }

        private void btnMTest_Click(object sender, RoutedEventArgs e)
        {
            this.tbm1.Text = "";
            this.tbm2.Text = "";
        }

        private void BtnRes_Click(object sender, RoutedEventArgs e)
        {
            this.Resources["staticRes"] = "静态资源发生改变";
            this.Resources["dynamicRes"] = "动态资源发生改变";
        }

        string ens = "Assert/Lang/en-us/Tips.xaml";
        string zhs = "Assert/Lang/zh-cn/Tips.xaml";
        private void btnLangCh_Click(object sender, RoutedEventArgs e)
        {
            var appResource = App.Current.Resources.MergedDictionaries;
            foreach (ResourceDictionary item in appResource)
            {
                if (item.Source.ToString() == zhs)
                {
                    return;
                }
                if (item.Source.ToString() == ens)
                {
                    appResource.Remove(item);
                    break;
                }
            }
            appResource.Clear();
            ResourceDictionary resdic = new ResourceDictionary();
            resdic.Source = new Uri(zhs, UriKind.Relative);
            appResource.Add(resdic);
        }

        private void btnLangEn_Click(object sender, RoutedEventArgs e)
        {
            var appResource = App.Current.Resources.MergedDictionaries;
            foreach (ResourceDictionary item in appResource)
            {
                if (item.Source.ToString() == ens)
                {
                    return;
                }
                if (item.Source.ToString() == zhs)
                {
                    appResource.Remove(item);
                    break;
                }
            }
            appResource.Clear();
            ResourceDictionary resdic = new ResourceDictionary();
            resdic.Source = new Uri(ens, UriKind.Relative);
            appResource.Add(resdic);
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void GridViewColumnHeaderClickedHandler(object sender,RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lsView3.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
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
