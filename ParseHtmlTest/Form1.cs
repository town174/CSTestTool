using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ParseHtmlTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var url = tbOpac.Text;
            if (string.IsNullOrEmpty(url)) return;
            RestClient client = new RestClient(url);
            IRestRequest request = new RestRequest(Method.GET);
            request.Timeout = 5000;
            var respone = client.Get(request);//Get()(request);
            if (respone.StatusCode == System.Net.HttpStatusCode.OK)
            {
                rbNormal.Clear();
                rbNormal.AppendText(respone.Content);
            }
            else
                MessageBox.Show($"查询失败: {(int)respone.StatusCode},{respone.StatusDescription}");
        }
    }
}
