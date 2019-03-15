namespace MisaCommon.UserControls
{
    partial class ControlListBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlListBox));
            this.PlButton = new System.Windows.Forms.FlowLayoutPanel();
            this.BtPlus = new System.Windows.Forms.Button();
            this.BtMinus = new System.Windows.Forms.Button();
            this.BtUp = new System.Windows.Forms.Button();
            this.BtDown = new System.Windows.Forms.Button();
            this.PlFill = new System.Windows.Forms.Panel();
            this.PlListTable = new System.Windows.Forms.TableLayoutPanel();
            this.PlButton.SuspendLayout();
            this.PlFill.SuspendLayout();
            this.SuspendLayout();
            // 
            // PlButton
            // 
            resources.ApplyResources(this.PlButton, "PlButton");
            this.PlButton.Controls.Add(this.BtPlus);
            this.PlButton.Controls.Add(this.BtMinus);
            this.PlButton.Controls.Add(this.BtUp);
            this.PlButton.Controls.Add(this.BtDown);
            this.PlButton.Name = "PlButton";
            // 
            // BtPlus
            // 
            resources.ApplyResources(this.BtPlus, "BtPlus");
            this.BtPlus.Name = "BtPlus";
            this.BtPlus.UseVisualStyleBackColor = true;
            this.BtPlus.Click += new System.EventHandler(this.PlPlus_Click);
            // 
            // BtMinus
            // 
            resources.ApplyResources(this.BtMinus, "BtMinus");
            this.BtMinus.Name = "BtMinus";
            this.BtMinus.UseVisualStyleBackColor = true;
            this.BtMinus.Click += new System.EventHandler(this.BtMinus_Click);
            // 
            // BtUp
            // 
            resources.ApplyResources(this.BtUp, "BtUp");
            this.BtUp.Name = "BtUp";
            this.BtUp.UseVisualStyleBackColor = true;
            this.BtUp.Click += new System.EventHandler(this.BtUpDown_Click);
            // 
            // BtDown
            // 
            resources.ApplyResources(this.BtDown, "BtDown");
            this.BtDown.Name = "BtDown";
            this.BtDown.UseVisualStyleBackColor = true;
            this.BtDown.Click += new System.EventHandler(this.BtUpDown_Click);
            // 
            // PlFill
            // 
            resources.ApplyResources(this.PlFill, "PlFill");
            this.PlFill.Controls.Add(this.PlListTable);
            this.PlFill.Name = "PlFill";
            // 
            // PlListTable
            // 
            resources.ApplyResources(this.PlListTable, "PlListTable");
            this.PlListTable.Name = "PlListTable";
            // 
            // ControlListBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PlFill);
            this.Controls.Add(this.PlButton);
            this.Name = "ControlListBox";
            this.PlButton.ResumeLayout(false);
            this.PlFill.ResumeLayout(false);
            this.PlFill.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel PlButton;
        private System.Windows.Forms.Button BtPlus;
        private System.Windows.Forms.Button BtMinus;
        private System.Windows.Forms.Button BtUp;
        private System.Windows.Forms.Button BtDown;
        private System.Windows.Forms.Panel PlFill;
        private System.Windows.Forms.TableLayoutPanel PlListTable;
    }
}
