namespace MisaCommon.UserControls
{
    partial class SpeechRecognitionSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpeechRecognitionSetting));
            this.PlFillTable = new System.Windows.Forms.TableLayoutPanel();
            this.PlMatchMessage = new System.Windows.Forms.Panel();
            this.TxtMatchMessage = new System.Windows.Forms.TextBox();
            this.PlMatchPattern = new System.Windows.Forms.Panel();
            this.RdoExactMatch = new System.Windows.Forms.RadioButton();
            this.RdoForwardMatch = new System.Windows.Forms.RadioButton();
            this.RdoBackwardMatch = new System.Windows.Forms.RadioButton();
            this.RdoPartialMatch = new System.Windows.Forms.RadioButton();
            this.RdoRegularExpression = new System.Windows.Forms.RadioButton();
            this.BtMinMax = new System.Windows.Forms.Button();
            this.PlControl = new System.Windows.Forms.Panel();
            this.LbSummaryText = new System.Windows.Forms.Label();
            this.PlFillTable.SuspendLayout();
            this.PlMatchMessage.SuspendLayout();
            this.PlMatchPattern.SuspendLayout();
            this.PlControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // PlFillTable
            // 
            resources.ApplyResources(this.PlFillTable, "PlFillTable");
            this.PlFillTable.Controls.Add(this.PlMatchMessage, 0, 0);
            this.PlFillTable.Controls.Add(this.BtMinMax, 1, 0);
            this.PlFillTable.Controls.Add(this.PlControl, 2, 0);
            this.PlFillTable.Name = "PlFillTable";
            // 
            // PlMatchMessage
            // 
            resources.ApplyResources(this.PlMatchMessage, "PlMatchMessage");
            this.PlMatchMessage.Controls.Add(this.TxtMatchMessage);
            this.PlMatchMessage.Controls.Add(this.PlMatchPattern);
            this.PlMatchMessage.Name = "PlMatchMessage";
            // 
            // TxtMatchMessage
            // 
            resources.ApplyResources(this.TxtMatchMessage, "TxtMatchMessage");
            this.TxtMatchMessage.Name = "TxtMatchMessage";
            this.TxtMatchMessage.TextChanged += new System.EventHandler(this.TxtMatchMessage_TextChanged);
            this.TxtMatchMessage.Enter += new System.EventHandler(this.TxtMatchMessage_Enter);
            this.TxtMatchMessage.Leave += new System.EventHandler(this.TxtMatchMessage_Leave);
            // 
            // PlMatchPattern
            // 
            this.PlMatchPattern.Controls.Add(this.RdoExactMatch);
            this.PlMatchPattern.Controls.Add(this.RdoForwardMatch);
            this.PlMatchPattern.Controls.Add(this.RdoBackwardMatch);
            this.PlMatchPattern.Controls.Add(this.RdoPartialMatch);
            this.PlMatchPattern.Controls.Add(this.RdoRegularExpression);
            resources.ApplyResources(this.PlMatchPattern, "PlMatchPattern");
            this.PlMatchPattern.Name = "PlMatchPattern";
            // 
            // RdoExactMatch
            // 
            resources.ApplyResources(this.RdoExactMatch, "RdoExactMatch");
            this.RdoExactMatch.Name = "RdoExactMatch";
            this.RdoExactMatch.TabStop = true;
            this.RdoExactMatch.Tag = "exact";
            this.RdoExactMatch.UseVisualStyleBackColor = true;
            this.RdoExactMatch.CheckedChanged += new System.EventHandler(this.MatchPatternRadioButton_CheckedChanged);
            // 
            // RdoForwardMatch
            // 
            resources.ApplyResources(this.RdoForwardMatch, "RdoForwardMatch");
            this.RdoForwardMatch.Name = "RdoForwardMatch";
            this.RdoForwardMatch.TabStop = true;
            this.RdoForwardMatch.Tag = "forward";
            this.RdoForwardMatch.UseVisualStyleBackColor = true;
            this.RdoForwardMatch.CheckedChanged += new System.EventHandler(this.MatchPatternRadioButton_CheckedChanged);
            // 
            // RdoBackwardMatch
            // 
            resources.ApplyResources(this.RdoBackwardMatch, "RdoBackwardMatch");
            this.RdoBackwardMatch.Name = "RdoBackwardMatch";
            this.RdoBackwardMatch.TabStop = true;
            this.RdoBackwardMatch.Tag = "backward";
            this.RdoBackwardMatch.UseVisualStyleBackColor = true;
            this.RdoBackwardMatch.CheckedChanged += new System.EventHandler(this.MatchPatternRadioButton_CheckedChanged);
            // 
            // RdoPartialMatch
            // 
            resources.ApplyResources(this.RdoPartialMatch, "RdoPartialMatch");
            this.RdoPartialMatch.Name = "RdoPartialMatch";
            this.RdoPartialMatch.TabStop = true;
            this.RdoPartialMatch.Tag = "partial";
            this.RdoPartialMatch.UseVisualStyleBackColor = true;
            this.RdoPartialMatch.CheckedChanged += new System.EventHandler(this.MatchPatternRadioButton_CheckedChanged);
            // 
            // RdoRegularExpression
            // 
            resources.ApplyResources(this.RdoRegularExpression, "RdoRegularExpression");
            this.RdoRegularExpression.Name = "RdoRegularExpression";
            this.RdoRegularExpression.TabStop = true;
            this.RdoRegularExpression.Tag = "regular";
            this.RdoRegularExpression.UseVisualStyleBackColor = true;
            this.RdoRegularExpression.CheckedChanged += new System.EventHandler(this.MatchPatternRadioButton_CheckedChanged);
            // 
            // BtMinMax
            // 
            resources.ApplyResources(this.BtMinMax, "BtMinMax");
            this.BtMinMax.Name = "BtMinMax";
            this.BtMinMax.UseVisualStyleBackColor = true;
            this.BtMinMax.Click += new System.EventHandler(this.BtMinMax_Click);
            // 
            // PlControl
            // 
            resources.ApplyResources(this.PlControl, "PlControl");
            this.PlControl.Controls.Add(this.LbSummaryText);
            this.PlControl.Name = "PlControl";
            // 
            // LbSummaryText
            // 
            this.LbSummaryText.AutoEllipsis = true;
            resources.ApplyResources(this.LbSummaryText, "LbSummaryText");
            this.LbSummaryText.Name = "LbSummaryText";
            // 
            // SpeechRecognitionSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PlFillTable);
            this.Name = "SpeechRecognitionSetting";
            this.PlFillTable.ResumeLayout(false);
            this.PlFillTable.PerformLayout();
            this.PlMatchMessage.ResumeLayout(false);
            this.PlMatchMessage.PerformLayout();
            this.PlMatchPattern.ResumeLayout(false);
            this.PlMatchPattern.PerformLayout();
            this.PlControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel PlFillTable;
        private System.Windows.Forms.Panel PlMatchMessage;
        private System.Windows.Forms.TextBox TxtMatchMessage;
        private System.Windows.Forms.Panel PlMatchPattern;
        private System.Windows.Forms.RadioButton RdoExactMatch;
        private System.Windows.Forms.RadioButton RdoForwardMatch;
        private System.Windows.Forms.RadioButton RdoBackwardMatch;
        private System.Windows.Forms.RadioButton RdoPartialMatch;
        private System.Windows.Forms.RadioButton RdoRegularExpression;
        private System.Windows.Forms.Button BtMinMax;
        private System.Windows.Forms.Panel PlControl;
        private System.Windows.Forms.Label LbSummaryText;
    }
}
