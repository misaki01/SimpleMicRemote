namespace MisaCommon.UserControls
{
    partial class StartProcessSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartProcessSetting));
            this.PlTable = new System.Windows.Forms.TableLayoutPanel();
            this.LbPath = new System.Windows.Forms.Label();
            this.TxtPath = new System.Windows.Forms.TextBox();
            this.BtPath = new System.Windows.Forms.Button();
            this.LbParam = new System.Windows.Forms.Label();
            this.LbDelay = new System.Windows.Forms.Label();
            this.ChkDelay = new System.Windows.Forms.CheckBox();
            this.TxtParam = new System.Windows.Forms.TextBox();
            this.TxtDelay = new System.Windows.Forms.NumericUpDown();
            this.LbSizePosition = new System.Windows.Forms.Label();
            this.ChkSizePosition = new System.Windows.Forms.CheckBox();
            this.PlSizePosition = new System.Windows.Forms.FlowLayoutPanel();
            this.PlSizeWidth = new System.Windows.Forms.Panel();
            this.LbSizeWidth = new System.Windows.Forms.Label();
            this.TxtSizeWidth = new System.Windows.Forms.NumericUpDown();
            this.PlSizeHeight = new System.Windows.Forms.Panel();
            this.LbSizeHeight = new System.Windows.Forms.Label();
            this.TxtSizeHeight = new System.Windows.Forms.NumericUpDown();
            this.PlPositionX = new System.Windows.Forms.Panel();
            this.LbPositionX = new System.Windows.Forms.Label();
            this.TxtPositionX = new System.Windows.Forms.NumericUpDown();
            this.PlPositionY = new System.Windows.Forms.Panel();
            this.LbPositionY = new System.Windows.Forms.Label();
            this.TxtPositionY = new System.Windows.Forms.NumericUpDown();
            this.PlTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDelay)).BeginInit();
            this.PlSizePosition.SuspendLayout();
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
            // PlTable
            // 
            resources.ApplyResources(this.PlTable, "PlTable");
            this.PlTable.Controls.Add(this.LbPath, 0, 0);
            this.PlTable.Controls.Add(this.TxtPath, 0, 1);
            this.PlTable.Controls.Add(this.BtPath, 5, 1);
            this.PlTable.Controls.Add(this.LbParam, 0, 3);
            this.PlTable.Controls.Add(this.LbDelay, 3, 3);
            this.PlTable.Controls.Add(this.ChkDelay, 4, 3);
            this.PlTable.Controls.Add(this.TxtParam, 0, 4);
            this.PlTable.Controls.Add(this.TxtDelay, 3, 4);
            this.PlTable.Controls.Add(this.LbSizePosition, 0, 6);
            this.PlTable.Controls.Add(this.ChkSizePosition, 1, 6);
            this.PlTable.Name = "PlTable";
            // 
            // LbPath
            // 
            resources.ApplyResources(this.LbPath, "LbPath");
            this.PlTable.SetColumnSpan(this.LbPath, 6);
            this.LbPath.Name = "LbPath";
            // 
            // TxtPath
            // 
            this.PlTable.SetColumnSpan(this.TxtPath, 5);
            resources.ApplyResources(this.TxtPath, "TxtPath");
            this.TxtPath.Name = "TxtPath";
            this.TxtPath.TextChanged += new System.EventHandler(this.TxtPath_TextChanged);
            // 
            // BtPath
            // 
            resources.ApplyResources(this.BtPath, "BtPath");
            this.BtPath.Name = "BtPath";
            this.BtPath.UseVisualStyleBackColor = true;
            this.BtPath.Click += new System.EventHandler(this.BtPath_Click);
            // 
            // LbParam
            // 
            resources.ApplyResources(this.LbParam, "LbParam");
            this.PlTable.SetColumnSpan(this.LbParam, 3);
            this.LbParam.Name = "LbParam";
            // 
            // LbDelay
            // 
            resources.ApplyResources(this.LbDelay, "LbDelay");
            this.LbDelay.Name = "LbDelay";
            // 
            // ChkDelay
            // 
            resources.ApplyResources(this.ChkDelay, "ChkDelay");
            this.ChkDelay.Name = "ChkDelay";
            this.ChkDelay.UseVisualStyleBackColor = true;
            this.ChkDelay.CheckedChanged += new System.EventHandler(this.ChkDelay_CheckedChanged);
            // 
            // TxtParam
            // 
            this.PlTable.SetColumnSpan(this.TxtParam, 2);
            resources.ApplyResources(this.TxtParam, "TxtParam");
            this.TxtParam.Name = "TxtParam";
            this.TxtParam.TextChanged += new System.EventHandler(this.TxtParam_TextChanged);
            // 
            // TxtDelay
            // 
            this.PlTable.SetColumnSpan(this.TxtDelay, 3);
            resources.ApplyResources(this.TxtDelay, "TxtDelay");
            this.TxtDelay.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.TxtDelay.Name = "TxtDelay";
            this.TxtDelay.Value = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.TxtDelay.ValueChanged += new System.EventHandler(this.TxtDelay_ValueChanged);
            // 
            // LbSizePosition
            // 
            resources.ApplyResources(this.LbSizePosition, "LbSizePosition");
            this.LbSizePosition.Name = "LbSizePosition";
            // 
            // ChkSizePosition
            // 
            resources.ApplyResources(this.ChkSizePosition, "ChkSizePosition");
            this.PlTable.SetColumnSpan(this.ChkSizePosition, 5);
            this.ChkSizePosition.Name = "ChkSizePosition";
            this.ChkSizePosition.UseVisualStyleBackColor = true;
            this.ChkSizePosition.CheckedChanged += new System.EventHandler(this.ChkSizePosition_CheckedChanged);
            // 
            // PlSizePosition
            // 
            resources.ApplyResources(this.PlSizePosition, "PlSizePosition");
            this.PlSizePosition.Controls.Add(this.PlSizeWidth);
            this.PlSizePosition.Controls.Add(this.PlSizeHeight);
            this.PlSizePosition.Controls.Add(this.PlPositionX);
            this.PlSizePosition.Controls.Add(this.PlPositionY);
            this.PlSizePosition.Name = "PlSizePosition";
            // 
            // PlSizeWidth
            // 
            resources.ApplyResources(this.PlSizeWidth, "PlSizeWidth");
            this.PlSizeWidth.Controls.Add(this.LbSizeWidth);
            this.PlSizeWidth.Controls.Add(this.TxtSizeWidth);
            this.PlSizeWidth.Name = "PlSizeWidth";
            // 
            // LbSizeWidth
            // 
            resources.ApplyResources(this.LbSizeWidth, "LbSizeWidth");
            this.LbSizeWidth.Name = "LbSizeWidth";
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
            this.TxtSizeWidth.ValueChanged += new System.EventHandler(this.SizePosition_ValueChanged);
            // 
            // PlSizeHeight
            // 
            resources.ApplyResources(this.PlSizeHeight, "PlSizeHeight");
            this.PlSizeHeight.Controls.Add(this.LbSizeHeight);
            this.PlSizeHeight.Controls.Add(this.TxtSizeHeight);
            this.PlSizeHeight.Name = "PlSizeHeight";
            // 
            // LbSizeHeight
            // 
            resources.ApplyResources(this.LbSizeHeight, "LbSizeHeight");
            this.LbSizeHeight.Name = "LbSizeHeight";
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
            this.TxtSizeHeight.ValueChanged += new System.EventHandler(this.SizePosition_ValueChanged);
            // 
            // PlPositionX
            // 
            resources.ApplyResources(this.PlPositionX, "PlPositionX");
            this.PlPositionX.Controls.Add(this.LbPositionX);
            this.PlPositionX.Controls.Add(this.TxtPositionX);
            this.PlPositionX.Name = "PlPositionX";
            // 
            // LbPositionX
            // 
            resources.ApplyResources(this.LbPositionX, "LbPositionX");
            this.LbPositionX.Name = "LbPositionX";
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
            this.TxtPositionX.ValueChanged += new System.EventHandler(this.SizePosition_ValueChanged);
            // 
            // PlPositionY
            // 
            resources.ApplyResources(this.PlPositionY, "PlPositionY");
            this.PlPositionY.Controls.Add(this.LbPositionY);
            this.PlPositionY.Controls.Add(this.TxtPositionY);
            this.PlPositionY.Name = "PlPositionY";
            // 
            // LbPositionY
            // 
            resources.ApplyResources(this.LbPositionY, "LbPositionY");
            this.LbPositionY.Name = "LbPositionY";
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
            this.TxtPositionY.ValueChanged += new System.EventHandler(this.SizePosition_ValueChanged);
            // 
            // StartProcessSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PlSizePosition);
            this.Controls.Add(this.PlTable);
            this.Name = "StartProcessSetting";
            this.PlTable.ResumeLayout(false);
            this.PlTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtDelay)).EndInit();
            this.PlSizePosition.ResumeLayout(false);
            this.PlSizePosition.PerformLayout();
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
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PlTable;
        private System.Windows.Forms.Label LbPath;
        private System.Windows.Forms.TextBox TxtPath;
        private System.Windows.Forms.Button BtPath;
        private System.Windows.Forms.Label LbParam;
        private System.Windows.Forms.Label LbDelay;
        private System.Windows.Forms.CheckBox ChkDelay;
        private System.Windows.Forms.TextBox TxtParam;
        private System.Windows.Forms.NumericUpDown TxtDelay;
        private System.Windows.Forms.Label LbSizePosition;
        private System.Windows.Forms.CheckBox ChkSizePosition;
        private System.Windows.Forms.Panel PlSizeWidth;
        private System.Windows.Forms.Label LbSizeWidth;
        private System.Windows.Forms.FlowLayoutPanel PlSizePosition;
        private System.Windows.Forms.NumericUpDown TxtSizeWidth;
        private System.Windows.Forms.Panel PlSizeHeight;
        private System.Windows.Forms.Label LbSizeHeight;
        private System.Windows.Forms.NumericUpDown TxtSizeHeight;
        private System.Windows.Forms.Panel PlPositionX;
        private System.Windows.Forms.Label LbPositionX;
        private System.Windows.Forms.NumericUpDown TxtPositionX;
        private System.Windows.Forms.Panel PlPositionY;
        private System.Windows.Forms.Label LbPositionY;
        private System.Windows.Forms.NumericUpDown TxtPositionY;
    }
}
