﻿namespace PicTest
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnBg = new System.Windows.Forms.Button();
            this.tbBg = new System.Windows.Forms.TextBox();
            this.btnTag = new System.Windows.Forms.Button();
            this.tbTag = new System.Windows.Forms.TextBox();
            this.pbBg = new System.Windows.Forms.PictureBox();
            this.pbTag = new System.Windows.Forms.PictureBox();
            this.pbOutput = new System.Windows.Forms.PictureBox();
            this.btnOutput = new System.Windows.Forms.Button();
            this.tbX = new System.Windows.Forms.TextBox();
            this.tbY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 450);
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
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "图像合并";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 424);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnBg
            // 
            this.btnBg.Location = new System.Drawing.Point(35, 19);
            this.btnBg.Name = "btnBg";
            this.btnBg.Size = new System.Drawing.Size(75, 23);
            this.btnBg.TabIndex = 0;
            this.btnBg.Text = "打开背景图";
            this.btnBg.UseVisualStyleBackColor = true;
            this.btnBg.Click += new System.EventHandler(this.btnBg_Click);
            // 
            // tbBg
            // 
            this.tbBg.Location = new System.Drawing.Point(132, 20);
            this.tbBg.Name = "tbBg";
            this.tbBg.Size = new System.Drawing.Size(427, 21);
            this.tbBg.TabIndex = 1;
            // 
            // btnTag
            // 
            this.btnTag.Location = new System.Drawing.Point(35, 58);
            this.btnTag.Name = "btnTag";
            this.btnTag.Size = new System.Drawing.Size(75, 23);
            this.btnTag.TabIndex = 2;
            this.btnTag.Text = "打开标记图";
            this.btnTag.UseVisualStyleBackColor = true;
            this.btnTag.Click += new System.EventHandler(this.btnTag_Click);
            // 
            // tbTag
            // 
            this.tbTag.Location = new System.Drawing.Point(132, 58);
            this.tbTag.Name = "tbTag";
            this.tbTag.Size = new System.Drawing.Size(427, 21);
            this.tbTag.TabIndex = 3;
            // 
            // pbBg
            // 
            this.pbBg.Location = new System.Drawing.Point(32, 152);
            this.pbBg.Name = "pbBg";
            this.pbBg.Size = new System.Drawing.Size(200, 200);
            this.pbBg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbBg.TabIndex = 4;
            this.pbBg.TabStop = false;
            // 
            // pbTag
            // 
            this.pbTag.Location = new System.Drawing.Point(274, 152);
            this.pbTag.Name = "pbTag";
            this.pbTag.Size = new System.Drawing.Size(200, 200);
            this.pbTag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbTag.TabIndex = 5;
            this.pbTag.TabStop = false;
            // 
            // pbOutput
            // 
            this.pbOutput.Location = new System.Drawing.Point(509, 152);
            this.pbOutput.Name = "pbOutput";
            this.pbOutput.Size = new System.Drawing.Size(200, 200);
            this.pbOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbOutput.TabIndex = 6;
            this.pbOutput.TabStop = false;
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(318, 380);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(75, 23);
            this.btnOutput.TabIndex = 7;
            this.btnOutput.Text = "图片合并";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // tbX
            // 
            this.tbX.Location = new System.Drawing.Point(171, 103);
            this.tbX.Name = "tbX";
            this.tbX.Size = new System.Drawing.Size(100, 21);
            this.tbX.TabIndex = 8;
            this.tbX.Text = "0";
            // 
            // tbY
            // 
            this.tbY.Location = new System.Drawing.Point(330, 103);
            this.tbY.Name = "tbY";
            this.tbY.Size = new System.Drawing.Size(100, 21);
            this.tbY.TabIndex = 9;
            this.tbY.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(129, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "x坐标";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(287, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "y坐标";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOutput)).EndInit();
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
    }
}
