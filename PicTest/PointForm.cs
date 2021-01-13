using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicTest
{
    public class AddPointEventArgs : EventArgs
    {
        public string PointName { get; set; }
        public string PointY { get; set; }
        public string PointX { get; set; }
    }

    public partial class PointForm : Form
    {
        public EventHandler<AddPointEventArgs> AddPointEvent = (a,b)=>{};

        public PointForm()
        {
            InitializeComponent();
        }

        public PointForm(int x, int y)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            tbPointX.Text = x.ToString();
            tbPointY.Text = y.ToString();
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbPointName.Text)) { MessageBox.Show("请输入名称"); return; }
            if (string.IsNullOrEmpty(tbPointX.Text)) { MessageBox.Show("请输入x坐标"); return; }
            if (string.IsNullOrEmpty(tbPointY.Text)) { MessageBox.Show("请输入y坐标"); return; }
            this.AddPointEvent.Invoke(this, new AddPointEventArgs() {
                PointName = tbPointName.Text,
                PointX = tbPointX.Text,
                PointY = tbPointY.Text
            });
        }
    }
}
