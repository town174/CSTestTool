using LiveCharts;
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
using System.Timers;

namespace WpfStyles.Views
{
    /// <summary>
    /// LiveChartDemos.xaml 的交互逻辑
    /// </summary>
    public partial class LiveChartDemos : Window
    {
        public LiveChartDemos()
        {
            InitializeComponent();
            InitLineChart();
            InitPieChart();
            InitGeoMap();
        }

        ChartValues<int> cv1 = new ChartValues<int>() { 1, 3, 5, 3, 1, 4 };
        ChartValues<int> cv2 = new ChartValues<int>() { 2, 4, 3, 6, 5, 8 };
        Timer t = new Timer(1000);
        Random rd = new Random();
        private void InitLineChart()
        {
            //PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            //DataContext = this;
            //去掉区域
            ls1.Values = cv1;
            ls2.Values = cv2;
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() => {
                int seed1 = rd.Next(0, cv1.Count);
                int seed2 = rd.Next(0, cv1.Count);
                int index = DateTime.Now.Second % cv1.Count;
                ls1.Values[index] = seed1;
                ls2.Values[index] = seed2;

                //if(DateTime.Now.Millisecond % 2 == 0)
                //{
                //    ls1.Values = cv1;
                //    ls2.Values = cv2;
                //}
                //else
                //{
                //    ls1.Values = cv2;
                //    ls2.Values = cv1;
                //}
            });
        }

        private void InitPieChart()
        {
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            DataContext = this;
        }

        //饼图数据格式
        public Func<LiveCharts.ChartPoint,string> PointLabel { get; set; }

        private void PieChart_DataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {

        }

        public Dictionary<string, double> Values { get; set; }
        public Dictionary<string, string> LanguagePack { get; set; }
        private void InitGeoMap()
        {
            var r = new Random();

            Values = new Dictionary<string, double>();

            Values["MX"] = r.Next(0, 100);
            Values["CA"] = r.Next(0, 100);
            Values["US"] = r.Next(0, 100);
            Values["IN"] = r.Next(0, 100);
            Values["CN"] = r.Next(0, 100);
            Values["JP"] = r.Next(0, 100);
            Values["BR"] = r.Next(0, 100);
            Values["DE"] = r.Next(0, 100);
            Values["FR"] = r.Next(0, 100);
            Values["GB"] = r.Next(0, 100);

            LanguagePack = new Dictionary<string, string>();
            LanguagePack["MX"] = "México"; // change the language if necessary

            DataContext = this;
        }
    }
}
