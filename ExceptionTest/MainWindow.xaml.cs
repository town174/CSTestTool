using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ExceptionTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Thread.Sleep(10 * 1000);
            //MessageBox.Show("click me");
            CalcDal();
        }

        private int CalcDal()
        {
            //try
            {
                var query = 0;
                Thread.Sleep(2000);
                return 1 / query;
            }
            //catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //throw;
            }
        }
    }
}
