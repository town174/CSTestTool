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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfSamples.View
{
    /// <summary>
    /// XamlView.xaml 的交互逻辑
    /// </summary>
    public partial class MeterView : UserControl
    {
        public MeterView()
        {
            InitializeComponent();
            DrawMeterScale();
            InitClock();
        }

        /// <summary>
        /// 画仪表刻度
        /// </summary>
        private void DrawMeterScale()
        {
            for (int i = 0; i <= 180; i += 5)
            {
                //添加刻度线
                Line lineScale = new Line();

                if (i % 25 == 0)//说明已经画了5个小刻度了，加一个大刻度
                {
                    lineScale.X1 = 200 - 160 * Math.Cos(i * Math.PI / 180);
                    lineScale.Y1 = 200 - 160 * Math.Sin(i * Math.PI / 180);
                    lineScale.Stroke = new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0));
                    lineScale.StrokeThickness = 3;

                    //添加刻度值
                    TextBlock txtScale = new TextBlock();
                    txtScale.Text = (i).ToString();
                    txtScale.FontSize = 10;
                    if (i <= 90)//对坐标值进行一定的修正
                    {
                        Canvas.SetLeft(txtScale, 200 - 155 * Math.Cos(i * Math.PI / 180));
                    }
                    else
                    {
                        Canvas.SetLeft(txtScale, 190 - 155 * Math.Cos(i * Math.PI / 180));
                    }
                    Canvas.SetTop(txtScale, 200 - 155 * Math.Sin(i * Math.PI / 180));
                    this.gaugeCanvas.Children.Add(txtScale);
                }
                else
                {
                    lineScale.X1 = 200 - 170 * Math.Cos(i * Math.PI / 180);
                    lineScale.Y1 = 200 - 170 * Math.Sin(i * Math.PI / 180);
                    lineScale.Stroke = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0));
                    lineScale.StrokeThickness = 1;
                }
                //直线刻度的终点，注意角度转为弧度制
                lineScale.X2 = 200 - 180 * Math.Cos(i * Math.PI / 180);
                lineScale.Y2 = 200 - 180 * Math.Sin(i * Math.PI / 180);
                //将直线画在Canvas画布上
                this.gaugeCanvas.Children.Add(lineScale);
            }
        }

        int angleNext = 0;
        int angelCurrent = 0;
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RotateTransform rt = new RotateTransform();
            rt.CenterX = 200;
            rt.CenterY = 200;

            this.indicatorPin.RenderTransform = rt;

            angelCurrent = angleNext;
            Random random = new Random();
            angleNext = random.Next(180);
            double timeAnimation = Math.Abs(angelCurrent - angleNext) * 8;
            DoubleAnimation da = new DoubleAnimation(angelCurrent, angleNext, new Duration(TimeSpan.FromMilliseconds(timeAnimation)));
            da.AccelerationRatio = 1;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        System.Timers.Timer timer = new System.Timers.Timer(1000);
        private void InitClock()
        {
            #region 初始化时间
            secondPointer.Angle = DateTime.Now.Second * 6;
            minutePointer.Angle = DateTime.Now.Minute * 6;
            hourPointer.Angle = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5);
            this.labTime.Content = DateTime.Now.ToString("HH:mm:ss");
            #endregion
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //UI异步更新
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                //秒针转动,秒针绕一圈360度，共60秒，所以1秒转动6度
                secondPointer.Angle = DateTime.Now.Second * 6;
                //分针转动,分针绕一圈360度，共60分，所以1分转动6度
                minutePointer.Angle = DateTime.Now.Minute * 6;
                //时针转动,时针绕一圈360度，共12时，所以1时转动30度。
                //另外同一个小时内，随着分钟数的变化(绕一圈60分钟），时针也在缓慢变化（转动30度，30/60=0.5)
                hourPointer.Angle = (DateTime.Now.Hour * 30) + (DateTime.Now.Minute * 0.5);
                //更新时间值
                this.labTime.Content = DateTime.Now.ToString("HH:mm:ss");
            }));
        }
    }
}
