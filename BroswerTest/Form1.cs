using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BroswerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        ChromiumWebBrowser webview;
        System.Timers.Timer t;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            // Initialize cef with the provided settings
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);

            string url = "http://www.njjnlib.cn:15000/screen/screen?libraryCode=CN-211100-JNLIB&token=c45ec2322e2b404b97fc9ab9cb21a217"; 
            //string url = "http://demo.finereport.com/";
            //string url = "https://echarts.apache.org/examples/zh/index.html";
            webview = new ChromiumWebBrowser(url);
            webview.Dock = DockStyle.Fill;
            this.Controls.Add(webview);

            t = new System.Timers.Timer(30 * 1000);
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() => {
                this.webview.Refresh();
            }));
        }
    }
}
