namespace Sample
{
    partial class SampleForm
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
            this.components = new System.ComponentModel.Container();
            this.picRgb = new System.Windows.Forms.PictureBox();
            this.tmDisplay = new System.Windows.Forms.Timer(this.components);
            this.tblPicArry = new System.Windows.Forms.TableLayoutPanel();
            this.picDepth = new System.Windows.Forms.PictureBox();
            this.picInfrared = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picRgb)).BeginInit();
            this.tblPicArry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInfrared)).BeginInit();
            this.SuspendLayout();
            // 
            // picRgb
            // 
            this.picRgb.BackColor = System.Drawing.SystemColors.GrayText;
            this.picRgb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picRgb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picRgb.Location = new System.Drawing.Point(487, 3);
            this.picRgb.Name = "picRgb";
            this.picRgb.Size = new System.Drawing.Size(478, 257);
            this.picRgb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRgb.TabIndex = 0;
            this.picRgb.TabStop = false;
            // 
            // tmDisplay
            // 
            this.tmDisplay.Tick += new System.EventHandler(this.tmDisplay_Tick);
            // 
            // tblPicArry
            // 
            this.tblPicArry.ColumnCount = 2;
            this.tblPicArry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPicArry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPicArry.Controls.Add(this.picRgb, 1, 0);
            this.tblPicArry.Controls.Add(this.picDepth, 0, 1);
            this.tblPicArry.Controls.Add(this.picInfrared, 1, 1);
            this.tblPicArry.Location = new System.Drawing.Point(0, 0);
            this.tblPicArry.Name = "tblPicArry";
            this.tblPicArry.RowCount = 2;
            this.tblPicArry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPicArry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPicArry.Size = new System.Drawing.Size(968, 527);
            this.tblPicArry.TabIndex = 4;
            // 
            // picDepth
            // 
            this.picDepth.BackColor = System.Drawing.SystemColors.GrayText;
            this.picDepth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picDepth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picDepth.Location = new System.Drawing.Point(3, 266);
            this.picDepth.Name = "picDepth";
            this.picDepth.Size = new System.Drawing.Size(478, 258);
            this.picDepth.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDepth.TabIndex = 1;
            this.picDepth.TabStop = false;
            // 
            // picInfrared
            // 
            this.picInfrared.BackColor = System.Drawing.SystemColors.GrayText;
            this.picInfrared.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picInfrared.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picInfrared.Location = new System.Drawing.Point(487, 266);
            this.picInfrared.Name = "picInfrared";
            this.picInfrared.Size = new System.Drawing.Size(478, 258);
            this.picInfrared.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picInfrared.TabIndex = 2;
            this.picInfrared.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 536);
            this.Controls.Add(this.tblPicArry);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picRgb)).EndInit();
            this.tblPicArry.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInfrared)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picRgb;
        private System.Windows.Forms.Timer tmDisplay;
        private System.Windows.Forms.TableLayoutPanel tblPicArry;
        private System.Windows.Forms.PictureBox picDepth;
        private System.Windows.Forms.PictureBox picInfrared;
    }
}

