using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Shapes;

namespace WpfSamples.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.SizeChanged += MainView_SizeChanged;
        }

        //通过sizechange可以加载数据，通过load反而不行。。
        private void MainView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(e.NewSize.Height > e.PreviousSize.Height || 
                e.NewSize.Width > e.PreviousSize.Width)
            {
                //注册消息，声明接受对象，token, 消息处理方法
                //Messenger.Default.Register<string>(this, "LoadData",a => { });
                //发送消息,
                Messenger.Default.Send<string>("pls load menu", "LoadData");
            }
        }
    }
}
