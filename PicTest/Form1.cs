using Gif.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            InitLv();
            this.pbZoom.MouseWheel += PbZoom_MouseWheel;
        }

        private void PbZoom_MouseWheel(object sender, MouseEventArgs e)
        {
            string pointName = _lvSelectIdx < 0 ? "" : lvPoints.Items[_lvSelectIdx].SubItems[0].Text;
            PointForm pf = new PointForm(pointName,e.X, e.Y);
            pf.AddPointEvent += AddPointEvenHandle;
            pf.ShowDialog();
        }

        List<AddPointEventArgs> es = new List<AddPointEventArgs>();
        private void AddPointEvenHandle(object sender, AddPointEventArgs e)
        {
            //todo e.x - tag.width / 2 , e.y - tag.height / 2
            e.PointX = e.PointX - pbTag.Image.Width  / 2;
            e.PointY = e.PointY - pbTag.Image.Height / 2;
            if (!es.Any(r => r.PointName.Equals(e.PointName)))
            {
                es.Add(e);
            }
            else
            {
                var exist = es.Find(r => r.PointName.Equals(e.PointName));
                exist.PointX = e.PointX;
                exist.PointY = e.PointY;
            }
            
            this.Invoke(new MethodInvoker(() => {
                this.lvPoints.BeginUpdate();
                //新增描点逻辑;
                List<string> contents = new List<string>() {
                    e.PointName,e.PointX.ToString(),e.PointY.ToString()
                };
                this.lvPoints.Items.Add(new ListViewItem(contents.ToArray()));
                //更新描点逻辑(更新本地，更新数据库)
                if(_lvSelectIdx != -1)
                {
                    lvPoints.Items[_lvSelectIdx].SubItems[1].Text = e.PointX.ToString();
                    lvPoints.Items[_lvSelectIdx].SubItems[2].Text = e.PointY.ToString();
                }
                this.lvPoints.EndUpdate();
                //更新图片
                UpdateNav(e.PointX,e.PointY);
            }));
        }

        private void UpdateNav(int x, int y)
        {
            var cn = "tagCtl";
            //合并bg和tag, 更新到pgZoom
            //pbZoom.Image = UniteImage(pbZoom.Image, pbTag.Image, int.Parse(e.PointX), int.Parse(e.PointY));
            //在panel中生成叠加pictureBox
            var tagPicBox = new PictureBox()
            {
                Name = cn,
                Width = pbTag.Width,
                Height = pbTag.Height,
                Left = x,
                Top = y,
                Image = pbTag.Image
            };
            //设置tagPic在最上面显示
            //todo  移除已添加图片控件
            var zoomCtls = this.pbZoom.Controls;
            if (zoomCtls.ContainsKey(cn))
                zoomCtls.RemoveByKey(cn);
            zoomCtls.Add(tagPicBox);
            tagPicBox.BringToFront();
        }

        private void btnBg_Click(object sender, EventArgs e)
        {
            var result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbBg.Text = openFileDialog1.FileName;
                pbBg.Image = Image.FromFile(tbBg.Text);
                tbZoom.Text = openFileDialog1.FileName;
                pbZoom.Image = Image.FromFile(tbZoom.Text);
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
            cbZoom.SelectedIndex = 2;
        }

        private void InitLv()
        {
            //设置detail视图
            lvPoints.View = View.Details;
            lvPoints.FullRowSelect = true;
            lvPoints.MultiSelect = false;
            lvPoints.GridLines = true;
            lvPoints.SelectedIndexChanged += LvPoints_SelectedIndexChanged;
            //创建表头
            Dictionary<string, int> chDicts = new Dictionary<string, int>() {
                ["点位名称"] = 150,
                ["点位坐标x"] = 80,
                ["点位坐标y"] = 80
            };
            foreach (var kv in chDicts)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = kv.Key;
                ch.Width = kv.Value;
                ch.TextAlign = HorizontalAlignment.Left;
                lvPoints.Columns.Add(ch);
            }
            //从数据库加载数据
            var data = DapperUtils.Query<BookShelfLocation>("SELECT * from bookshelflocations").OrderBy(x => x.BookShelfName);
            //添加lv内容
            this.lvPoints.BeginUpdate();
            foreach (var item in data)
            {
                List<string> contents = new List<string>() {
                    item.BookShelfName,item.CoordinateX.ToString(),item.CoordinateY.ToString()
                };
                this.lvPoints.Items.Add(new ListViewItem(contents.ToArray()));
            }
            this.lvPoints.EndUpdate();
        }

        int _lvSelectIdx = -1;
        private void LvPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            var si = lvPoints.SelectedIndices;
            if (si.Count > 0)
            {
                int idx = si[0];
                _lvSelectIdx = idx;
                var pointName = lvPoints.Items[idx].SubItems[0].Text;
                var pointX = lvPoints.Items[idx].SubItems[1].Text;
                var pointY = lvPoints.Items[idx].SubItems[2].Text;
                //MessageBox.Show($"{pointName},{pointX},{pointY}");
                UpdateNav(int.Parse(pointX), int.Parse(pointY));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(normalImage == null)
            {
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

        string[] gifBases = new string[] { };
        private void btnDirGif_Click(object sender, EventArgs e)
        {
            gifBases = new string[] { };
            if(!Directory.Exists(tbGif.Text))
            {
                MessageBox.Show("目录不存在");
                return;
            }
            var files = Directory.GetFiles(tbGif.Text);
            gifBases = files;
            if (gifBases.Length == 0)
            {
                MessageBox.Show("文件不存在");
                return;
            }
        }

        private void btnGetGif_Click(object sender, EventArgs e)
        {
            if (gifBases.Length == 0)
            {
                MessageBox.Show("文件不存在");
                return;
            }

            string gifFile = "location.gif";
            AnimatedGifEncoder gifEncoder = new AnimatedGifEncoder();
            gifEncoder.Start(gifFile);
            gifEncoder.SetDelay(500);
            gifEncoder.SetRepeat(999);
            gifEncoder.SetSize(40,40);
            gifEncoder.SetTransparent(Color.Transparent);
            for (int i = 0; i < gifBases.Length; i++)
            {
                gifEncoder.AddFrame(Image.FromFile(gifBases[i]));
            }
            gifEncoder.Finish();
            pbGif.Image = Image.FromFile(gifFile);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var dt = NPOIHelper.ListToDataTable(es);
            NPOIHelper.Export(dt, "", "export.xls");
            MessageBox.Show("导出成功");
        }

        private void btnDb_Click(object sender, EventArgs e)
        {
            string sql = "";
            foreach (var p in es)
            {
                sql = $"UPDATE bookshelflocations set CoordinateX = '{p.PointX}', CoordinateY = '{p.PointY}' WHERE BookShelfName = '{p.PointName}';";
                DapperUtils.Execute(sql);
                Thread.Sleep(100);
            }
            MessageBox.Show("导出成功");
        }
    }
}
