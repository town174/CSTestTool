﻿using System;
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
    public partial class TransformView : UserControl
    {
        public TransformView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Hyperlink link = sender as Hyperlink;
            string url = "https://docs.microsoft.com/zh-cn/dotnet/desktop/wpf/graphics-multimedia/transforms-overview?view=netframeworkdesktop-4.8";
            Process.Start(new ProcessStartInfo(url));
        }
    }
}
