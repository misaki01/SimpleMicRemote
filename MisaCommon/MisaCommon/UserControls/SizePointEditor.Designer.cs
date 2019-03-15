namespace MisaCommon.UserControls
{
    partial class SizePointEditor
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SizePointEditor));
            this.PlTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.BtCancel = new System.Windows.Forms.Button();
            this.BtOk = new System.Windows.Forms.Button();
            this.PlBar1 = new System.Windows.Forms.Panel();
            this.LbSize = new System.Windows.Forms.Label();
            this.PlSizeWidth = new System.Windows.Forms.Panel();
            this.TxtSizeWidth = new System.Windows.Forms.NumericUpDown();
            this.LbSizeWidth = new System.Windows.Forms.Label();
            this.PlSizeHeight = new System.Windows.Forms.Panel();
            this.TxtSizeHeight = new System.Windows.Forms.NumericUpDown();
            this.LbSizeHeight = new System.Windows.Forms.Label();
            this.PlBar2 = new System.Windows.Forms.Panel();
            this.LbPosition = new System.Windows.Forms.Label();
            this.PlPositionX = new System.Windows.Forms.Panel();
            this.TxtPositionX = new System.Windows.Forms.NumericUpDown();
            this.LbPositionX = new System.Windows.Forms.Label();
            this.PlPositionY = new System.Windows.Forms.Panel();
            this.TxtPositionY = new System.Windows.Forms.NumericUpDown();
            this.LbPositionY = new System.Windows.Forms.Label();
            this.BtTopLeft = new System.Windows.Forms.Button();
            this.BtTopRight = new System.Windows.Forms.Button();
            this.BtBottomLeft = new System.Windows.Forms.Button();
            this.BtBottomRight = new System.Windows.Forms.Button();
            this.PlTableLayout.SuspendLayout();
            this.PlSizeWidth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtSizeWidth)).BeginInit();
            this.PlSizeHeight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtSizeHeight)).BeginInit();
            this.PlPositionX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPositionX)).BeginInit();
            this.PlPositionY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPositionY)).BeginInit();
            this.SuspendLayout();
            // 
            // PlTableLayout
            // 
            resources.ApplyResources(this.PlTableLayout, "PlTableLayout");
            this.PlTableLayout.Controls.Add(this.BtCancel, 0, 0);
            this.PlTableLayout.Controls.Add(this.BtOk, 1, 0);
            this.PlTableLayout.Controls.Add(this.PlBar1, 0, 1);
            this.PlTableLayout.Controls.Add(this.LbSize, 0, 2);
            this.PlTableLayout.Controls.Add(this.PlSizeWidth, 0, 3);
            this.PlTableLayout.Controls.Add(this.PlSizeHeight, 1, 3);
            this.PlTableLayout.Controls.Add(this.PlBar2, 0, 4);
            this.PlTableLayout.Controls.Add(this.LbPosition, 0, 5);
            this.PlTableLayout.Controls.Add(this.PlPositionX, 0, 6);
            this.PlTableLayout.Controls.Add(this.PlPositionY, 1, 6);
            this.PlTableLayout.Controls.Add(this.BtTopLeft, 0, 7);
            this.PlTableLayout.Controls.Add(this.BtTopRight, 1, 7);
            this.PlTableLayout.Controls.Add(this.BtBottomLeft, 0, 8);
            this.PlTableLayout.Controls.Add(this.BtBottomRight, 1, 8);
            this.PlTableLayout.Name = "PlTableLayout";
            // 
            // BtCancel
            // 
            this.BtCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.BtCancel, "BtCancel");
            this.BtCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.BtCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.UseVisualStyleBackColor = false;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtOk
            // 
            this.BtOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BtOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.BtOk, "BtOk");
            this.BtOk.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BtOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Honeydew;
            this.BtOk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BtOk.Name = "BtOk";
            this.BtOk.UseVisualStyleBackColor = false;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // PlBar1
            // 
            this.PlBar1.BackColor = System.Drawing.Color.Black;
            this.PlTableLayout.SetColumnSpan(this.PlBar1, 2);
            resources.ApplyResources(this.PlBar1, "PlBar1");
            this.PlBar1.Name = "PlBar1";
            // 
            // LbSize
            // 
            resources.ApplyResources(this.LbSize, "LbSize");
            this.PlTableLayout.SetColumnSpan(this.LbSize, 2);
            this.LbSize.Name = "LbSize";
            // 
            // PlSizeWidth
            // 
            resources.ApplyResources(this.PlSizeWidth, "PlSizeWidth");
            this.PlSizeWidth.Controls.Add(this.TxtSizeWidth);
            this.PlSizeWidth.Controls.Add(this.LbSizeWidth);
            this.PlSizeWidth.Name = "PlSizeWidth";
            // 
            // TxtSizeWidth
            // 
            resources.ApplyResources(this.TxtSizeWidth, "TxtSizeWidth");
            this.TxtSizeWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TxtSizeWidth.Name = "TxtSizeWidth";
            this.TxtSizeWidth.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TxtSizeWidth.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSizePosition_KeyPress);
            // 
            // LbSizeWidth
            // 
            resources.ApplyResources(this.LbSizeWidth, "LbSizeWidth");
            this.LbSizeWidth.Name = "LbSizeWidth";
            // 
            // PlSizeHeight
            // 
            resources.ApplyResources(this.PlSizeHeight, "PlSizeHeight");
            this.PlSizeHeight.Controls.Add(this.TxtSizeHeight);
            this.PlSizeHeight.Controls.Add(this.LbSizeHeight);
            this.PlSizeHeight.Name = "PlSizeHeight";
            // 
            // TxtSizeHeight
            // 
            resources.ApplyResources(this.TxtSizeHeight, "TxtSizeHeight");
            this.TxtSizeHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TxtSizeHeight.Name = "TxtSizeHeight";
            this.TxtSizeHeight.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TxtSizeHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSizePosition_KeyPress);
            // 
            // LbSizeHeight
            // 
            resources.ApplyResources(this.LbSizeHeight, "LbSizeHeight");
            this.LbSizeHeight.Name = "LbSizeHeight";
            // 
            // PlBar2
            // 
            this.PlBar2.BackColor = System.Drawing.Color.Black;
            this.PlTableLayout.SetColumnSpan(this.PlBar2, 2);
            resources.ApplyResources(this.PlBar2, "PlBar2");
            this.PlBar2.Name = "PlBar2";
            // 
            // LbPosition
            // 
            resources.ApplyResources(this.LbPosition, "LbPosition");
            this.PlTableLayout.SetColumnSpan(this.LbPosition, 2);
            this.LbPosition.Name = "LbPosition";
            // 
            // PlPositionX
            // 
            resources.ApplyResources(this.PlPositionX, "PlPositionX");
            this.PlPositionX.Controls.Add(this.TxtPositionX);
            this.PlPositionX.Controls.Add(this.LbPositionX);
            this.PlPositionX.Name = "PlPositionX";
            // 
            // TxtPositionX
            // 
            resources.ApplyResources(this.TxtPositionX, "TxtPositionX");
            this.TxtPositionX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TxtPositionX.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.TxtPositionX.Name = "TxtPositionX";
            this.TxtPositionX.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TxtPositionX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSizePosition_KeyPress);
            // 
            // LbPositionX
            // 
            resources.ApplyResources(this.LbPositionX, "LbPositionX");
            this.LbPositionX.Name = "LbPositionX";
            // 
            // PlPositionY
            // 
            resources.ApplyResources(this.PlPositionY, "PlPositionY");
            this.PlPositionY.Controls.Add(this.TxtPositionY);
            this.PlPositionY.Controls.Add(this.LbPositionY);
            this.PlPositionY.Name = "PlPositionY";
            // 
            // TxtPositionY
            // 
            resources.ApplyResources(this.TxtPositionY, "TxtPositionY");
            this.TxtPositionY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TxtPositionY.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.TxtPositionY.Name = "TxtPositionY";
            this.TxtPositionY.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TxtPositionY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtSizePosition_KeyPress);
            // 
            // LbPositionY
            // 
            resources.ApplyResources(this.LbPositionY, "LbPositionY");
            this.LbPositionY.Name = "LbPositionY";
            // 
            // BtTopLeft
            // 
            this.BtTopLeft.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.BtTopLeft, "BtTopLeft");
            this.BtTopLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.BtTopLeft.Name = "BtTopLeft";
            this.BtTopLeft.UseVisualStyleBackColor = false;
            this.BtTopLeft.Click += new System.EventHandler(this.BtTopLeft_Click);
            // 
            // BtTopRight
            // 
            this.BtTopRight.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.BtTopRight, "BtTopRight");
            this.BtTopRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.BtTopRight.Name = "BtTopRight";
            this.BtTopRight.UseVisualStyleBackColor = false;
            this.BtTopRight.Click += new System.EventHandler(this.BtTopRight_Click);
            // 
            // BtBottomLeft
            // 
            this.BtBottomLeft.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.BtBottomLeft, "BtBottomLeft");
            this.BtBottomLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.BtBottomLeft.Name = "BtBottomLeft";
            this.BtBottomLeft.UseVisualStyleBackColor = false;
            this.BtBottomLeft.Click += new System.EventHandler(this.BtBottomLeft_Click);
            // 
            // BtBottomRight
            // 
            this.BtBottomRight.BackColor = System.Drawing.Color.Silver;
            resources.ApplyResources(this.BtBottomRight, "BtBottomRight");
            this.BtBottomRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.BtBottomRight.Name = "BtBottomRight";
            this.BtBottomRight.UseVisualStyleBackColor = false;
            this.BtBottomRight.Click += new System.EventHandler(this.BtBottomRight_Click);
            // 
            // SizePointEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.PlTableLayout);
            this.Name = "SizePointEditor";
            this.PlTableLayout.ResumeLayout(false);
            this.PlTableLayout.PerformLayout();
            this.PlSizeWidth.ResumeLayout(false);
            this.PlSizeWidth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtSizeWidth)).EndInit();
            this.PlSizeHeight.ResumeLayout(false);
            this.PlSizeHeight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtSizeHeight)).EndInit();
            this.PlPositionX.ResumeLayout(false);
            this.PlPositionX.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPositionX)).EndInit();
            this.PlPositionY.ResumeLayout(false);
            this.PlPositionY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPositionY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PlTableLayout;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.Panel PlBar1;
        private System.Windows.Forms.Label LbSize;
        private System.Windows.Forms.Panel PlSizeWidth;
        private System.Windows.Forms.NumericUpDown TxtSizeWidth;
        private System.Windows.Forms.Label LbSizeWidth;
        private System.Windows.Forms.Panel PlSizeHeight;
        private System.Windows.Forms.NumericUpDown TxtSizeHeight;
        private System.Windows.Forms.Label LbSizeHeight;
        private System.Windows.Forms.Panel PlBar2;
        private System.Windows.Forms.Label LbPosition;
        private System.Windows.Forms.Button BtTopLeft;
        private System.Windows.Forms.Button BtTopRight;
        private System.Windows.Forms.Button BtBottomLeft;
        private System.Windows.Forms.Button BtBottomRight;
        private System.Windows.Forms.Panel PlPositionX;
        private System.Windows.Forms.Label LbPositionX;
        private System.Windows.Forms.NumericUpDown TxtPositionX;
        private System.Windows.Forms.Panel PlPositionY;
        private System.Windows.Forms.Label LbPositionY;
        private System.Windows.Forms.NumericUpDown TxtPositionY;
    }
}
