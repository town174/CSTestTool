using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitCb();
            this.pbZoom.MouseDown += pbZoom_MouseDown;
        }

        private void pbZoom_MouseDown(object sender, MouseEventArgs e)
        {
            //鼠标在pannel内生效，并且左键点击
            if (e.Button == MouseButtons.Left)
            {
                MessageBox.Show($"鼠标位于所在区域，x:{e.X}, y:{e.Y}");
            }
            if (e.Button == MouseButtons.Right)
            {
                PointForm pf = new PointForm(e.X, e.Y);
                pf.AddPointEvent += AddPointEvenHandle;
                pf.ShowDialog();
            }
        }

        Dictionary<string, string> _ExistPointDicts = new Dictionary<string, string>();
        private void AddPointEvenHandle(object sender, AddPointEventArgs e)
        {
            if(_ExistPointDicts.Keys.Any(r => r.Equals(e.PointName)))
            {
                MessageBox.Show("存在同名点位, 重新编辑");
                return;
            }
            this.lvPoints.Invoke(new MethodInvoker(() => {
                //this.lvPoints
            }));
        }

        ObservableCollection<string> observers = new ObservableCollection<string>();
        private void InitLv()
        {
            //this.lvPoints.DataBindings = observers;
        }

        private void btnBg_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbBg.Text = openFileDialog1.FileName;
                pbBg.Image = Image.FromFile(tbBg.Text);
            }            
        }

        private void btnTag_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbTag.Text = openFileDialog1.FileName;
                pbTag.Image = Image.FromFile(tbTag.Text);
            }
        }

        public Image UniteImage(Image imgBg, Image imgTag, int x = 0, int y = 0)
        {
            Image img = new Bitmap(imgBg.Width, imgBg.Height);
            Graphics g = Graphics.FromImage(img);
            g.Clear(System.Drawing.Color.Transparent);
            g.DrawImage(imgBg, 0, 0, imgBg.Width, imgBg.Height);
            g.DrawImage(imgTag, x, y, imgTag.Width, imgTag.Height);
            return img;
        }

        public Image ZoomImage(Image imgSource, int width = 0, int heith = 0)
        {
            var imgTmp = new Bitmap(imgSource);
            var imgDest = new Bitmap(imgTmp, width, heith);
            return imgDest;
        }

        public Image ZoomImage(Image imgSource, float proportion)
        {
            var imgTmp = new Bitmap(imgSource);
            var imgDest = new Bitmap(imgTmp, (int)(imgTmp.Width * proportion), (int)(imgTmp.Height * proportion));
            return imgDest;
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
           pbOutput.Image = UniteImage(pbBg.Image, pbTag.Image, int.Parse(tbX.Text), int.Parse(tbY.Text));
           pbOutput.Image.Save("Combine.png");
        }

        Image normalImage = null;
        private void btnZoom_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();            
            if (result == DialogResult.OK)
            {
                tbZoom.Text = openFileDialog1.FileName;
                cbZoom.SelectedIndex = 2;
                normalImage = Image.FromFile(tbZoom.Text);
                pbZoom.Image = normalImage;
            }
        }

        private void InitCb()
        {
            Dictionary<float, string> CbData = new Dictionary<float, string>() {
                [0.2f] = "20%",
                [0.5f] = "50%",
                [1.0f] = "100%",
                [1.5f] = "150%",
                [2.0f] = "200%",
                [3.0f] = "300%",
            };
            BindingSource bs = new BindingSource();
            bs.DataSource = CbData;
            cbZoom.DataSource = bs;
            cbZoom.ValueMember = "Key";
            cbZoom.DisplayMember = "Value";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(normalImage == null)
            {
                //MessageBox.Show("请选中一张图片");
                return;
            }
            var proportion = float.Parse(cbZoom.SelectedValue.ToString());
            pbZoom.Image = ZoomImage(normalImage, proportion);
        }

        private bool ContainsPoint(Point ctPoint, Size ctSize, Point mouse)
        {
            if (mouse.X < ctPoint.X) return false;
            if (mouse.X > ctPoint.X + ctSize.Width) return false;
            if (mouse.Y < ctPoint.Y) return false;
            if (mouse.Y > ctPoint.Y + ctSize.Height) return false;
            return true;
        }
    }
}
