using CefSharp.WinForms;
using System;
using System.Collections;
using System.Windows.Forms;

namespace BroswerTest
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser browser;
        private System.Timers.Timer timer;
        public Form1()
        {
            InitializeComponent();
            InitBroswer();
        }

        //string url = "http://www.njjnlib.cn:15000/screen/screen?libraryCode=CN-211100-JNLIB&token=c45ec2322e2b404b97fc9ab9cb21a217";
        //string url = "http://172.16.0.120:9800/";
        string url = "";
        private void InitBroswer()
        {
            url = this.tb1.Text;
            WindowState = FormWindowState.Maximized;

            //CefSettings settings = new CefSettings();
            CefSharp.Cef.EnableHighDPISupport();

            browser = new ChromiumWebBrowser(url);
            //去掉弹窗
            browser.LifeSpanHandler = new LifeSpanHandler();
            this.tlp1.Controls.Add(browser);

            browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
            browser.ConsoleMessage += Browser_ConsoleMessage;
            browser.StatusMessage += Browser_StatusMessage;
            browser.TitleChanged += Browser_TitleChanged;
            browser.AddressChanged += Browser_AddressChanged;


            //timer = new System.Timers.Timer(9 * 1000);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() => {
                if (e.SignalTime.Second % 2 == 0)
                    browser.Load("https://www.baidu.com");
                else
                    browser.Load(url);
                //browser.Refresh();
            }));
        }

        bool firstLoad = true;
        private void Browser_AddressChanged(object sender, CefSharp.AddressChangedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() => {
                this.tb1.Text = e.Address;
                if (firstLoad)
                {
                    firstLoad = false;
                    //forwardStack.Push(e.Address);
                }
            }));
            //throw new NotImplementedException();
        }

        private void Browser_TitleChanged(object sender, CefSharp.TitleChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Browser_StatusMessage(object sender, CefSharp.StatusMessageEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Browser_ConsoleMessage(object sender, CefSharp.ConsoleMessageEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Browser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        Stack forwardStack = new Stack();
        Stack backStack = new Stack();
        private void btnQuery_Click(object sender, EventArgs e)
        {
            url = this.tb1.Text;
            browser.Load(url);
            return;
            forwardStack.Push(url);
            while (backStack.Count > 0) backStack.Pop();
        }

        //string forward = null;
        //string back = null;
        private void btnForward_Click(object sender, EventArgs e)
        {
            browser.GetBrowser().GoForward();
            return;
            if (backStack.Count > 0)
            {
                var tmp = backStack.Pop().ToString();
                browser.Load(tmp);
                forwardStack.Push(tmp);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            browser.GetBrowser().GoBack();
            return;
            if (forwardStack.Count > 0)
            {
                var tmp = forwardStack.Pop().ToString();
                browser.Load(tmp);
                backStack.Push(tmp);
            }
        }
    }
}
