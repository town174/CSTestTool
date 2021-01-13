using SocketTool.Utils;
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

namespace SocketTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public class CbItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            BoxIP.Text = "127.0.0.1";
            BoxPort.Text = "2400";
            BoxIPRec.Text = "127.0.0.1";
            BoxPortRec.Text = "2400";
            RichtxtboxInput("9300CNuniverson|COF1KawHuoLcMTsevOM8GySM6DltiXaf0msSS5Pm5x0ls=|CPCN-310106-1-G02|AY1AZF659", RBoxSend);
            List<CbItem> items = new List<CbItem>() {
                new CbItem(){ Name = "utf-7"},
                new CbItem(){ Name = "utf-8"},
                new CbItem(){ Name = "utf-16"},                
                new CbItem(){ Name = "gb2312"}
            };

            EncodingCbx.ItemsSource = items;
            EncodingCbx.SelectedIndex = 0;
        }

        //创建clientsocket
        Socket SocketClient = null;
        EndPoint ClientPoint = null;
        System.Timers.Timer ClientTimer = null;

        //创建serviceSocket
        Socket SocketSend = null;
        Socket SocketWatch = null;
        EndPoint WatchPoint = null;
        System.Timers.Timer WatchTimer = null;
        //将远程连接的客户端的IP地址和Socket存入集合中
        Dictionary<string, Socket> SocketDict = new Dictionary<string, Socket>();
        //创建监听连接的线程
        Thread AcceptSocketThread;
        //接收客户端发送消息的线程
        Thread threadReceive;


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
            SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketClient.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
            SocketClient.ReceiveTimeout = 5000;

            //定时读取数据
            ClientTimer = new System.Timers.Timer(2000);
            ClientTimer.Elapsed += sendTimer_Elapsed;
            CreateBtn.IsEnabled = false;
        }

        private void CreateBtnRec_Click(object sender, RoutedEventArgs e)
        {
            //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //设置长连接
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)15000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)15000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
            SocketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketWatch.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

            //定时读取数据
            WatchTimer = new System.Timers.Timer(2000);
            WatchTimer.Elapsed += recTimer_Elapsed;
            CreateBtnRec.IsEnabled = false;
        }

        byte[] recieve = new byte[1024];
        private void sendTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (obj)
            {
                if (!SocketClient.Connected) return;
                int rec = 0;//SocketClient.Receive(recieve);
                if (rec != 0)
                {
                    RichtxtboxInput(Encoding.GetEncoding(EncodingCbx.SelectedValue.ToString()).GetString(recieve, 0, rec), RBoxRec);
                } 
            }
        }
        byte[] recieve2 = new byte[1024];
        private void recTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (obj)
            {
                int rec = 0;//recSocket.Receive(recieve2);
                if (rec != 0)
                {
                    RichtxtboxInput(Encoding.GetEncoding(EncodingCbx.SelectedValue.ToString()).GetString(recieve, 0, rec), RBoxServiceRec);
                }
            }
        }

        private void RichtxtboxInput(string txt, RichTextBox richtxtbox)
        {
            Action a = () => {
                Run r = new Run(txt);
                Paragraph para = new Paragraph();
                para.Inlines.Add(r);
                richtxtbox.Document.Blocks.Clear();
                richtxtbox.Document.Blocks.Add(para);
            };
            richtxtbox.Dispatcher.Invoke(a);
        }
        //连接到特定服务器
        bool isLink = false;
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkBtn_Click(object sender, RoutedEventArgs e)
        {
            if(isLink)
            {
                try
                {
                    SocketClient.Disconnect(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
                }
                ClientTimer.Stop();
                MessageBox.Show("断开成功");
                isLink = false;
                LinkBtn.Content = "连接";
            }
            else
            {
                IPAddress ipaddress = IPAddress.Parse(BoxIP.Text);
                ClientPoint = new IPEndPoint(ipaddress, int.Parse(BoxPort.Text));
                try
                {
                    SocketClient.Connect(ClientPoint);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
                }
                ClientTimer.Start();
                MessageBox.Show("连接成功");
                isLink = true;
                LinkBtn.Content = "断开";
            }
        }
        bool isListen = false;
        /// <summary>
        /// 创建监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListenBtnRec_Click(object sender, RoutedEventArgs e)
        {
            if (isListen)
            {
                try
                {
                    SocketWatch.Disconnect(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
                }
                WatchTimer.Stop();
                MessageBox.Show("断开成功");
                isListen = false;
                ListenBtnRec.Content = "监听";
            }
            else
            {
                IPAddress ipaddress = IPAddress.Parse(BoxIPRec.Text);
                WatchPoint = new IPEndPoint(ipaddress, int.Parse(BoxPortRec.Text));
                try
                {
                    SocketWatch.Bind(WatchPoint);
                    SocketWatch.Listen(10);

                    //创建线程
                    AcceptSocketThread = new Thread(new ParameterizedThreadStart(StartListen));
                    AcceptSocketThread.IsBackground = true;
                    AcceptSocketThread.Start(SocketWatch);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
                }
                WatchTimer.Start();
                MessageBox.Show("监听成功");
                isListen = true;
                ListenBtnRec.Content = "断开";
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
                 num = SocketClient.Send(Encoding.GetEncoding(EncodingCbx.SelectedValue.ToString()).GetBytes(sendText));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
            }

            int maxCount = 2;
            int waitTime = 500;
            int count = 0;
            while (true)
            {
                int rec = 0;
                try
                {
                    rec = SocketClient.Receive(recieve);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误类型: {ex.GetType().ToString()}\r错误码: {ex.HResult}\r错误提示: {ex.Message}\r错误帮助: {ex.HelpLink}");
                    break;
                }
                if (rec != 0)
                {
                    RichtxtboxInput(Encoding.GetEncoding(EncodingCbx.SelectedValue.ToString()).GetString(recieve, 0, rec), RBoxRec);
                    break;
                }
                if(count++ >= maxCount)
                {
                    break;
                }
                Thread.Sleep(waitTime);
            }

        }

        #region service
        /// <summary>
        /// 等待客户端的连接，并且创建与之通信用的Socket
        /// </summary>
        /// <param name="obj"></param>
        private void StartListen(object obj)
        {
            Socket socketWatch = obj as Socket;
            while (true)
            {
                //等待客户端的连接，并且创建一个用于通信的Socket
                SocketSend = socketWatch.Accept();
                //获取远程主机的ip地址和端口号
                string strIp = SocketSend.RemoteEndPoint.ToString();
                SocketDict.Add(strIp, SocketSend);
                string strMsg = "远程主机：" + SocketSend.RemoteEndPoint + "连接成功" + "\r\n";
                //使用回调
                RichtxtboxInput(strMsg, RBoxServiceRec);

                Encoding enc = null;
                EncodingCbx.Dispatcher.Invoke(() => {
                    enc = Encoding.GetEncoding(EncodingCbx.SelectedValue.ToString());
                });
                //string welcome = "Welcome to hh lib system, Account Name";
                //SocketSend.Send(enc.GetBytes(welcome));

                //定义接收客户端消息的线程
                Thread threadReceive = new Thread(new ParameterizedThreadStart(Receive));
                threadReceive.IsBackground = true;
                threadReceive.Start(SocketSend);
            }
        }



        /// <summary>
        /// 服务器端不停的接收客户端发送的消息
        /// </summary>
        /// <param name="obj"></param>
        private void Receive(object obj)
        {
            Socket SocketSend = obj as Socket;
            string cacheStr = "";
            while (true)
            {
                //客户端连接成功后，服务器接收客户端发送的消息
                byte[] buffer = new byte[2048];
                //实际接收到的有效字节数
                int count = SocketSend.Receive(buffer);
                if (count == 0)//count 表示客户端关闭，要退出循环
                {
                    break;
                }
                else
                {
                    Encoding enc = null;
                    EncodingCbx.Dispatcher.Invoke(() => {
                        enc = Encoding.GetEncoding(EncodingCbx.SelectedValue.ToString());
                    });

                    string str = enc.GetString(buffer, 0, count);
                    if (!str.EndsWith("\n")) { cacheStr += str; continue; }
                    cacheStr = cacheStr.TrimEnd();
                    string strReceiveMsg = "接收：" + SocketSend.RemoteEndPoint + "发送的消息:" + cacheStr + "\r\n";
                    const string output1 = "Account Name";
                    const string output2 = "Password";
                    const string output3 = "ACS Ready";
                    const string output4 = "ACS Failed";
                    const string user = "g02sip2";
                    const string pwd = "sip2jaxzl";
                    const string end = "\n";
                    switch (cacheStr)
                    {
                        case user:
                            SocketSend.Send(enc.GetBytes(output2 + end));break;
                        case pwd:
                            SocketSend.Send(enc.GetBytes(output3 + end)); break;
                        default:
                            SocketSend.Send(enc.GetBytes(output1 + end)); break;
                    }
                    cacheStr = "";
                    RichtxtboxInput(strReceiveMsg, RBoxServiceRec);
                }
            }
        }
        #endregion
    }
}
