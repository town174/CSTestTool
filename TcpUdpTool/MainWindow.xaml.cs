using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
using TcpUdpTool.Utils;

namespace TcpUdpTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BoxIP.Text = "172.16.2.240";
            BoxPort.Text = "2400";
            RichtxtboxInput("1720080828    105715AO|AB900100|AY1AZF7E3|", RBoxSend);
        }

        //创建socket
        Socket socket = null;
        EndPoint point = null;
        System.Timers.Timer timer = null;
        readonly static object obj = new object(); 
        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //设置长连接
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)15000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)15000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

            //定时读取数据
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += Timer_Elapsed;
            CreateBtn.IsEnabled = false;
        }

        byte[] recieve = new byte[1024];
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (obj)
            {
                int rec = socket.Receive(recieve);
                if (rec != 0)
                {
                    RichtxtboxInput(Encoding.UTF8.GetString(recieve, 0, rec), RBoxRec);
                } 
            }
        }

        private void RichtxtboxInput(string txt, RichTextBox richtxtbox)
        {
            //MessageBox.Show(txt);
            Action a = () => {
                Run r = new Run(txt);
                Paragraph para = new Paragraph();
                para.Inlines.Add(r);
                richtxtbox.Document.Blocks.Clear();
                richtxtbox.Document.Blocks.Add(para);
            };
            richtxtbox.Dispatcher.Invoke(a);
            // 如果不设置等待，整个程序死循环
            //Thread.Sleep(100);
        }

        //连接到特定服务器
        bool isLink = false;
        private void LinkBtn_Click(object sender, RoutedEventArgs e)
        {
            if(isLink)
            {
                try
                {
                    socket.Disconnect(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
                }
                timer.Stop();
                MessageBox.Show("断开成功");
                isLink = false;
                LinkBtn.Content = "连接";
            }
            else
            {
                IPAddress ipaddress = IPAddress.Parse(BoxIP.Text);
                point = new IPEndPoint(ipaddress, int.Parse(BoxPort.Text));
                try
                {
                    socket.Connect(point);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
                }
                //timer.Start();
                MessageBox.Show("连接成功");
                isLink = true;
                LinkBtn.Content = "断开";
            }
        }

        //发送数据
        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            TextRange sendRange = new TextRange(RBoxSend.Document.ContentStart, RBoxSend.Document.ContentEnd);
            String sendText = sendRange.Text;
            if ((bool)CBoxEnter.IsChecked) sendText = string.Concat(sendText, "\r");

            int num = 0;
            try
            {
                 num = socket.Send(Encoding.UTF8.GetBytes(sendText));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
            }

            while (true)
            {
                int rec = 0;
                try
                {
                    rec = socket.Receive(recieve);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
                }
                if (rec != 0)
                {
                    MessageBox.Show(HexUtils.ByteToHexStr(recieve));
                    RichtxtboxInput(Encoding.UTF8.GetString(recieve, 0, rec), RBoxRec);
                    break;
                }
            }

        }
    }
}
