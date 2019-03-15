namespace MisaCommon.UserControls
{
    partial class KeyInputSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyInputSetting));
            this.PlTop = new System.Windows.Forms.Panel();
            this.PlInput = new System.Windows.Forms.Panel();
            this.PlInputText = new System.Windows.Forms.Panel();
            this.TxtInput = new System.Windows.Forms.TextBox();
            this.PlInputLabel = new System.Windows.Forms.Panel();
            this.ChkKeepPressing = new System.Windows.Forms.CheckBox();
            this.LbInput = new System.Windows.Forms.Label();
            this.PlModifierKey = new System.Windows.Forms.Panel();
            this.LbModifierKey = new System.Windows.Forms.Label();
            this.ChkShift = new System.Windows.Forms.CheckBox();
            this.ChkWin = new System.Windows.Forms.CheckBox();
            this.ChkCtrl = new System.Windows.Forms.CheckBox();
            this.ChkAlt = new System.Windows.Forms.CheckBox();
            this.PlBar = new System.Windows.Forms.Panel();
            this.PlComboBox = new System.Windows.Forms.FlowLayoutPanel();
            this.PlMouse = new System.Windows.Forms.Panel();
            this.LbMouse = new System.Windows.Forms.Label();
            this.CmbBoxMouse = new System.Windows.Forms.ComboBox();
            this.PlInputKey = new System.Windows.Forms.Panel();
            this.LbInputKey = new System.Windows.Forms.Label();
            this.CmbBoxInputKey = new System.Windows.Forms.ComboBox();
            this.PlOperateKey = new System.Windows.Forms.Panel();
            this.LbOperateKey = new System.Windows.Forms.Label();
            this.CmbBoxOperateKey = new System.Windows.Forms.ComboBox();
            this.PlImeKey = new System.Windows.Forms.Panel();
            this.LbImeKey = new System.Windows.Forms.Label();
            this.CmbBoxImeKey = new System.Windows.Forms.ComboBox();
            this.PlSpecialKey = new System.Windows.Forms.Panel();
            this.LbSpecialKey = new System.Windows.Forms.Label();
            this.CmbBoxSpecialKey = new System.Windows.Forms.ComboBox();
            this.PlTop.SuspendLayout();
            this.PlInput.SuspendLayout();
            this.PlInputText.SuspendLayout();
            this.PlInputLabel.SuspendLayout();
            this.PlModifierKey.SuspendLayout();
            this.PlComboBox.SuspendLayout();
            this.PlMouse.SuspendLayout();
            this.PlInputKey.SuspendLayout();
            this.PlOperateKey.SuspendLayout();
            this.PlImeKey.SuspendLayout();
            this.PlSpecialKey.SuspendLayout();
            this.SuspendLayout();
            // 
            // PlTop
            // 
            this.PlTop.Controls.Add(this.PlInput);
            this.PlTop.Controls.Add(this.PlModifierKey);
            resources.ApplyResources(this.PlTop, "PlTop");
            this.PlTop.Name = "PlTop";
            // 
            // PlInput
            // 
            this.PlInput.Controls.Add(this.PlInputText);
            this.PlInput.Controls.Add(this.PlInputLabel);
            resources.ApplyResources(this.PlInput, "PlInput");
            this.PlInput.Name = "PlInput";
            // 
            // PlInputText
            // 
            this.PlInputText.Controls.Add(this.TxtInput);
            resources.ApplyResources(this.PlInputText, "PlInputText");
            this.PlInputText.Name = "PlInputText";
            // 
            // TxtInput
            // 
            resources.ApplyResources(this.TxtInput, "TxtInput");
            this.TxtInput.Name = "TxtInput";
            this.TxtInput.ReadOnly = true;
            this.TxtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtInput_KeyDown);
            // 
            // PlInputLabel
            // 
            this.PlInputLabel.Controls.Add(this.ChkKeepPressing);
            this.PlInputLabel.Controls.Add(this.LbInput);
            resources.ApplyResources(this.PlInputLabel, "PlInputLabel");
            this.PlInputLabel.Name = "PlInputLabel";
            // 
            // ChkKeepPressing
            // 
            resources.ApplyResources(this.ChkKeepPressing, "ChkKeepPressing");
            this.ChkKeepPressing.Name = "ChkKeepPressing";
            this.ChkKeepPressing.UseVisualStyleBackColor = true;
            this.ChkKeepPressing.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // LbInput
            // 
            resources.ApplyResources(this.LbInput, "LbInput");
            this.LbInput.Name = "LbInput";
            // 
            // PlModifierKey
            // 
            this.PlModifierKey.Controls.Add(this.LbModifierKey);
            this.PlModifierKey.Controls.Add(this.ChkShift);
            this.PlModifierKey.Controls.Add(this.ChkWin);
            this.PlModifierKey.Controls.Add(this.ChkCtrl);
            this.PlModifierKey.Controls.Add(this.ChkAlt);
            resources.ApplyResources(this.PlModifierKey, "PlModifierKey");
            this.PlModifierKey.Name = "PlModifierKey";
            // 
            // LbModifierKey
            // 
            resources.ApplyResources(this.LbModifierKey, "LbModifierKey");
            this.LbModifierKey.Name = "LbModifierKey";
            // 
            // ChkShift
            // 
            resources.ApplyResources(this.ChkShift, "ChkShift");
            this.ChkShift.Name = "ChkShift";
            this.ChkShift.UseVisualStyleBackColor = true;
            this.ChkShift.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // ChkWin
            // 
            resources.ApplyResources(this.ChkWin, "ChkWin");
            this.ChkWin.Name = "ChkWin";
            this.ChkWin.UseVisualStyleBackColor = true;
            this.ChkWin.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // ChkCtrl
            // 
            resources.ApplyResources(this.ChkCtrl, "ChkCtrl");
            this.ChkCtrl.Name = "ChkCtrl";
            this.ChkCtrl.UseVisualStyleBackColor = true;
            this.ChkCtrl.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // ChkAlt
            // 
            resources.ApplyResources(this.ChkAlt, "ChkAlt");
            this.ChkAlt.Name = "ChkAlt";
            this.ChkAlt.UseVisualStyleBackColor = true;
            this.ChkAlt.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // PlBar
            // 
            this.PlBar.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.PlBar, "PlBar");
            this.PlBar.Name = "PlBar";
            // 
            // PlComboBox
            // 
            resources.ApplyResources(this.PlComboBox, "PlComboBox");
            this.PlComboBox.Controls.Add(this.PlMouse);
            this.PlComboBox.Controls.Add(this.PlInputKey);
            this.PlComboBox.Controls.Add(this.PlOperateKey);
            this.PlComboBox.Controls.Add(this.PlImeKey);
            this.PlComboBox.Controls.Add(this.PlSpecialKey);
            this.PlComboBox.Name = "PlComboBox";
            // 
            // PlMouse
            // 
            resources.ApplyResources(this.PlMouse, "PlMouse");
            this.PlMouse.Controls.Add(this.LbMouse);
            this.PlMouse.Controls.Add(this.CmbBoxMouse);
            this.PlMouse.Name = "PlMouse";
            // 
            // LbMouse
            // 
            resources.ApplyResources(this.LbMouse, "LbMouse");
            this.LbMouse.Name = "LbMouse";
            // 
            // CmbBoxMouse
            // 
            this.CmbBoxMouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBoxMouse.FormattingEnabled = true;
            resources.ApplyResources(this.CmbBoxMouse, "CmbBoxMouse");
            this.CmbBoxMouse.Name = "CmbBoxMouse";
            this.CmbBoxMouse.SelectedIndexChanged += new System.EventHandler(this.KeyComboBox_SelectedIndexChanged);
            // 
            // PlInputKey
            // 
            resources.ApplyResources(this.PlInputKey, "PlInputKey");
            this.PlInputKey.Controls.Add(this.LbInputKey);
            this.PlInputKey.Controls.Add(this.CmbBoxInputKey);
            this.PlInputKey.Name = "PlInputKey";
            // 
            // LbInputKey
            // 
            resources.ApplyResources(this.LbInputKey, "LbInputKey");
            this.LbInputKey.Name = "LbInputKey";
            // 
            // CmbBoxInputKey
            // 
            this.CmbBoxInputKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBoxInputKey.FormattingEnabled = true;
            resources.ApplyResources(this.CmbBoxInputKey, "CmbBoxInputKey");
            this.CmbBoxInputKey.Name = "CmbBoxInputKey";
            this.CmbBoxInputKey.SelectedIndexChanged += new System.EventHandler(this.KeyComboBox_SelectedIndexChanged);
            // 
            // PlOperateKey
            // 
            resources.ApplyResources(this.PlOperateKey, "PlOperateKey");
            this.PlOperateKey.Controls.Add(this.LbOperateKey);
            this.PlOperateKey.Controls.Add(this.CmbBoxOperateKey);
            this.PlOperateKey.Name = "PlOperateKey";
            // 
            // LbOperateKey
            // 
            resources.ApplyResources(this.LbOperateKey, "LbOperateKey");
            this.LbOperateKey.Name = "LbOperateKey";
            // 
            // CmbBoxOperateKey
            // 
            this.CmbBoxOperateKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBoxOperateKey.FormattingEnabled = true;
            resources.ApplyResources(this.CmbBoxOperateKey, "CmbBoxOperateKey");
            this.CmbBoxOperateKey.Name = "CmbBoxOperateKey";
            this.CmbBoxOperateKey.SelectedIndexChanged += new System.EventHandler(this.KeyComboBox_SelectedIndexChanged);
            // 
            // PlImeKey
            // 
            resources.ApplyResources(this.PlImeKey, "PlImeKey");
            this.PlImeKey.Controls.Add(this.LbImeKey);
            this.PlImeKey.Controls.Add(this.CmbBoxImeKey);
            this.PlImeKey.Name = "PlImeKey";
            // 
            // LbImeKey
            // 
            resources.ApplyResources(this.LbImeKey, "LbImeKey");
            this.LbImeKey.Name = "LbImeKey";
            // 
            // CmbBoxImeKey
            // 
            this.CmbBoxImeKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBoxImeKey.FormattingEnabled = true;
            resources.ApplyResources(this.CmbBoxImeKey, "CmbBoxImeKey");
            this.CmbBoxImeKey.Name = "CmbBoxImeKey";
            this.CmbBoxImeKey.SelectedIndexChanged += new System.EventHandler(this.KeyComboBox_SelectedIndexChanged);
            // 
            // PlSpecialKey
            // 
            resources.ApplyResources(this.PlSpecialKey, "PlSpecialKey");
            this.PlSpecialKey.Controls.Add(this.LbSpecialKey);
            this.PlSpecialKey.Controls.Add(this.CmbBoxSpecialKey);
            this.PlSpecialKey.Name = "PlSpecialKey";
            // 
            // LbSpecialKey
            // 
            resources.ApplyResources(this.LbSpecialKey, "LbSpecialKey");
            this.LbSpecialKey.Name = "LbSpecialKey";
            // 
            // CmbBoxSpecialKey
            // 
            this.CmbBoxSpecialKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBoxSpecialKey.FormattingEnabled = true;
            resources.ApplyResources(this.CmbBoxSpecialKey, "CmbBoxSpecialKey");
            this.CmbBoxSpecialKey.Name = "CmbBoxSpecialKey";
            this.CmbBoxSpecialKey.SelectedIndexChanged += new System.EventHandler(this.KeyComboBox_SelectedIndexChanged);
            // 
            // KeyInputSetting
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PlComboBox);
            this.Controls.Add(this.PlBar);
            this.Controls.Add(this.PlTop);
            this.Name = "KeyInputSetting";
            this.Load += new System.EventHandler(this.KeyInputSetting_Load);
            this.PlTop.ResumeLayout(false);
            this.PlInput.ResumeLayout(false);
            this.PlInputText.ResumeLayout(false);
            this.PlInputText.PerformLayout();
            this.PlInputLabel.ResumeLayout(false);
            this.PlInputLabel.PerformLayout();
            this.PlModifierKey.ResumeLayout(false);
            this.PlModifierKey.PerformLayout();
            this.PlComboBox.ResumeLayout(false);
            this.PlComboBox.PerformLayout();
            this.PlMouse.ResumeLayout(false);
            this.PlMouse.PerformLayout();
            this.PlInputKey.ResumeLayout(false);
            this.PlInputKey.PerformLayout();
            this.PlOperateKey.ResumeLayout(false);
            this.PlOperateKey.PerformLayout();
            this.PlImeKey.ResumeLayout(false);
            this.PlImeKey.PerformLayout();
            this.PlSpecialKey.ResumeLayout(false);
            this.PlSpecialKey.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PlTop;
        private System.Windows.Forms.Panel PlInput;
        private System.Windows.Forms.Panel PlInputLabel;
        private System.Windows.Forms.Label LbInput;
        private System.Windows.Forms.CheckBox ChkKeepPressing;
        private System.Windows.Forms.Panel PlInputText;
        private System.Windows.Forms.TextBox TxtInput;
        private System.Windows.Forms.Panel PlModifierKey;
        private System.Windows.Forms.Label LbModifierKey;
        private System.Windows.Forms.CheckBox ChkShift;
        private System.Windows.Forms.CheckBox ChkCtrl;
        private System.Windows.Forms.CheckBox ChkAlt;
        private System.Windows.Forms.CheckBox ChkWin;
        private System.Windows.Forms.Panel PlBar;
        private System.Windows.Forms.FlowLayoutPanel PlComboBox;
        private System.Windows.Forms.Panel PlMouse;
        private System.Windows.Forms.Label LbMouse;
        private System.Windows.Forms.ComboBox CmbBoxMouse;
        private System.Windows.Forms.Panel PlInputKey;
        private System.Windows.Forms.Label LbInputKey;
        private System.Windows.Forms.ComboBox CmbBoxInputKey;
        private System.Windows.Forms.Panel PlOperateKey;
        private System.Windows.Forms.Label LbOperateKey;
        private System.Windows.Forms.ComboBox CmbBoxOperateKey;
        private System.Windows.Forms.Panel PlImeKey;
        private System.Windows.Forms.Label LbImeKey;
        private System.Windows.Forms.ComboBox CmbBoxImeKey;
        private System.Windows.Forms.Panel PlSpecialKey;
        private System.Windows.Forms.Label LbSpecialKey;
        private System.Windows.Forms.ComboBox CmbBoxSpecialKey;
    }
}
