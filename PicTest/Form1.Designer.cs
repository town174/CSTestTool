namespace PicTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbY = new System.Windows.Forms.TextBox();
            this.tbX = new System.Windows.Forms.TextBox();
            this.btnOutput = new System.Windows.Forms.Button();
            this.pbOutput = new System.Windows.Forms.PictureBox();
            this.pbTag = new System.Windows.Forms.PictureBox();
            this.pbBg = new System.Windows.Forms.PictureBox();
            this.tbTag = new System.Windows.Forms.TextBox();
            this.btnTag = new System.Windows.Forms.Button();
            this.tbBg = new System.Windows.Forms.TextBox();
            this.btnBg = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lvPoints = new System.Windows.Forms.ListView();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbZoom = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbZoom = new System.Windows.Forms.ComboBox();
            this.tbZoom = new System.Windows.Forms.TextBox();
            this.btnZoom = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnExport = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBg)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1067, 562);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbY);
            this.tabPage1.Controls.Add(this.tbX);
            this.tabPage1.Controls.Add(this.btnOutput);
            this.tabPage1.Controls.Add(this.pbOutput);
            this.tabPage1.Controls.Add(this.pbTag);
            this.tabPage1.Controls.Add(this.pbBg);
            this.tabPage1.Controls.Add(this.tbTag);
            this.tabPage1.Controls.Add(this.btnTag);
            this.tabPage1.Controls.Add(this.tbBg);
            this.tabPage1.Controls.Add(this.btnBg);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1059, 533);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图像合并";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(383, 134);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "y坐标";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 134);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "x坐标";
            // 
            // tbY
            // 
            this.tbY.Location = new System.Drawing.Point(440, 129);
            this.tbY.Margin = new System.Windows.Forms.Padding(4);
            this.tbY.Name = "tbY";
            this.tbY.Size = new System.Drawing.Size(132, 25);
            this.tbY.TabIndex = 9;
            this.tbY.Text = "0";
            // 
            // tbX
            // 
            this.tbX.Location = new System.Drawing.Point(228, 129);
            this.tbX.Margin = new System.Windows.Forms.Padding(4);
            this.tbX.Name = "tbX";
            this.tbX.Size = new System.Drawing.Size(132, 25);
            this.tbX.TabIndex = 8;
            this.tbX.Text = "0";
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(424, 475);
            this.btnOutput.Margin = new System.Windows.Forms.Padding(4);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(100, 29);
            this.btnOutput.TabIndex = 7;
            this.btnOutput.Text = "图片合并";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // pbOutput
            // 
            this.pbOutput.Location = new System.Drawing.Point(679, 190);
            this.pbOutput.Margin = new System.Windows.Forms.Padding(4);
            this.pbOutput.Name = "pbOutput";
            this.pbOutput.Size = new System.Drawing.Size(267, 250);
            this.pbOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbOutput.TabIndex = 6;
            this.pbOutput.TabStop = false;
            // 
            // pbTag
            // 
            this.pbTag.Location = new System.Drawing.Point(365, 190);
            this.pbTag.Margin = new System.Windows.Forms.Padding(4);
            this.pbTag.Name = "pbTag";
            this.pbTag.Size = new System.Drawing.Size(267, 250);
            this.pbTag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbTag.TabIndex = 5;
            this.pbTag.TabStop = false;
            // 
            // pbBg
            // 
            this.pbBg.Location = new System.Drawing.Point(43, 190);
            this.pbBg.Margin = new System.Windows.Forms.Padding(4);
            this.pbBg.Name = "pbBg";
            this.pbBg.Size = new System.Drawing.Size(267, 250);
            this.pbBg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBg.TabIndex = 4;
            this.pbBg.TabStop = false;
            // 
            // tbTag
            // 
            this.tbTag.Location = new System.Drawing.Point(176, 72);
            this.tbTag.Margin = new System.Windows.Forms.Padding(4);
            this.tbTag.Name = "tbTag";
            this.tbTag.Size = new System.Drawing.Size(568, 25);
            this.tbTag.TabIndex = 3;
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(47, 72);
            this.btnTag.Margin = new System.Windows.Forms.Padding(4);
            this.btnTag.Name = "btnTag";
            this.btnTag.Size = new System.Drawing.Size(100, 29);
            this.btnTag.TabIndex = 2;
            this.btnTag.Text = "打开标记图";
            this.btnTag.UseVisualStyleBackColor = true;
            this.btnTag.Click += new System.EventHandler(this.btnTag_Click);
            // 
            // tbBg
            // 
            this.tbBg.Location = new System.Drawing.Point(176, 25);
            this.tbBg.Margin = new System.Windows.Forms.Padding(4);
            this.tbBg.Name = "tbBg";
            this.tbBg.Size = new System.Drawing.Size(568, 25);
            this.tbBg.TabIndex = 1;
            // 
            // btnBg
            // 
            this.btnBg.Location = new System.Drawing.Point(47, 24);
            this.btnBg.Margin = new System.Windows.Forms.Padding(4);
            this.btnBg.Name = "btnBg";
            this.btnBg.Size = new System.Drawing.Size(100, 29);
            this.btnBg.TabIndex = 0;
            this.btnBg.Text = "打开背景图";
            this.btnBg.UseVisualStyleBackColor = true;
            this.btnBg.Click += new System.EventHandler(this.btnBg_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnExport);
            this.tabPage2.Controls.Add(this.lvPoints);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.cbZoom);
            this.tabPage2.Controls.Add(this.tbZoom);
            this.tabPage2.Controls.Add(this.btnZoom);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1059, 533);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "图像缩放";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lvPoints
            // 
            this.lvPoints.HideSelection = false;
            this.lvPoints.Location = new System.Drawing.Point(12, 216);
            this.lvPoints.Margin = new System.Windows.Forms.Padding(4);
            this.lvPoints.Name = "lvPoints";
            this.lvPoints.Size = new System.Drawing.Size(400, 200);
            this.lvPoints.TabIndex = 12;
            this.lvPoints.UseCompatibleStateImageBehavior = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 184);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "点位集合";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pbZoom);
            this.panel1.Location = new System.Drawing.Point(451, 16);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 500);
            this.panel1.TabIndex = 10;
            // 
            // pbZoom
            // 
            this.pbZoom.Location = new System.Drawing.Point(4, 4);
            this.pbZoom.Margin = new System.Windows.Forms.Padding(4);
            this.pbZoom.Name = "pbZoom";
            this.pbZoom.Size = new System.Drawing.Size(494, 494);
            this.pbZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbZoom.TabIndex = 7;
            this.pbZoom.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 102);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "缩放比例";
            // 
            // cbZoom
            // 
            this.cbZoom.FormattingEnabled = true;
            this.cbZoom.Location = new System.Drawing.Point(9, 135);
            this.cbZoom.Margin = new System.Windows.Forms.Padding(4);
            this.cbZoom.Name = "cbZoom";
            this.cbZoom.Size = new System.Drawing.Size(160, 23);
            this.cbZoom.TabIndex = 8;
            this.cbZoom.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // tbZoom
            // 
            this.tbZoom.Location = new System.Drawing.Point(11, 60);
            this.tbZoom.Margin = new System.Windows.Forms.Padding(4);
            this.tbZoom.Name = "tbZoom";
            this.tbZoom.Size = new System.Drawing.Size(283, 25);
            this.tbZoom.TabIndex = 3;
            // 
            // btnZoom
            // 
            this.btnZoom.Location = new System.Drawing.Point(11, 15);
            this.btnZoom.Margin = new System.Windows.Forms.Padding(4);
            this.btnZoom.Name = "btnZoom";
            this.btnZoom.Size = new System.Drawing.Size(100, 29);
            this.btnZoom.TabIndex = 2;
            this.btnZoom.Text = "打开图像";
            this.btnZoom.UseVisualStyleBackColor = true;
            this.btnZoom.Click += new System.EventHandler(this.btnZoom_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(12, 434);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 29);
            this.btnExport.TabIndex = 13;
            this.btnExport.Text = "导出点位";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 562);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBg)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnBg;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox tbBg;
        private System.Windows.Forms.Button btnTag;
        private System.Windows.Forms.TextBox tbTag;
        private System.Windows.Forms.PictureBox pbBg;
        private System.Windows.Forms.PictureBox pbOutput;
        private System.Windows.Forms.PictureBox pbTag;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.TextBox tbY;
        private System.Windows.Forms.TextBox tbX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbZoom;
        private System.Windows.Forms.Button btnZoom;
        private System.Windows.Forms.PictureBox pbZoom;
        private System.Windows.Forms.ComboBox cbZoom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lvPoints;
        private System.Windows.Forms.Button btnExport;
    }
}

