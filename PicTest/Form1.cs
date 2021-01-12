using System;
using System.Collections.Generic;
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

        private void btnOutput_Click(object sender, EventArgs e)
        {
           pbOutput.Image = UniteImage(pbBg.Image, pbTag.Image, int.Parse(tbX.Text), int.Parse(tbY.Text));
           pbOutput.Image.Save("Combine.png");
        }
    }
}
