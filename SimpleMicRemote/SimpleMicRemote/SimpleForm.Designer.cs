namespace SimpleMicRemote
{
    partial class SimpleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleForm));
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabMain = new System.Windows.Forms.TabPage();
            this.PlFill = new System.Windows.Forms.FlowLayoutPanel();
            this.PlInputModeOn = new System.Windows.Forms.Panel();
            this.TxtInputModeOn = new System.Windows.Forms.TextBox();
            this.LbInputModeOn = new System.Windows.Forms.Label();
            this.PlInputModeOnEnter = new System.Windows.Forms.Panel();
            this.TxtInputModeOnEnter = new System.Windows.Forms.TextBox();
            this.LbInputModeOnEnter = new System.Windows.Forms.Label();
            this.PlInputModeOff = new System.Windows.Forms.Panel();
            this.TxtInputModeOff = new System.Windows.Forms.TextBox();
            this.LbInputModeOff = new System.Windows.Forms.Label();
            this.PlActiveWindowClose = new System.Windows.Forms.Panel();
            this.TxtActiveWindowClose = new System.Windows.Forms.TextBox();
            this.LbActiveWindowClose = new System.Windows.Forms.Label();
            this.PlKeyInput1 = new System.Windows.Forms.Panel();
            this.TxtKeyInput1 = new System.Windows.Forms.TextBox();
            this.LbKeyInput1 = new System.Windows.Forms.Label();
            this.PlKeyInput2 = new System.Windows.Forms.Panel();
            this.TxtKeyInput2 = new System.Windows.Forms.TextBox();
            this.LbKeyInput2 = new System.Windows.Forms.Label();
            this.PlKeyInput3 = new System.Windows.Forms.Panel();
            this.TxtKeyInput3 = new System.Windows.Forms.TextBox();
            this.LbKeyInput3 = new System.Windows.Forms.Label();
            this.PlKeyInput4 = new System.Windows.Forms.Panel();
            this.TxtKeyInput4 = new System.Windows.Forms.TextBox();
            this.LbKeyInput4 = new System.Windows.Forms.Label();
            this.PlKeyInput5 = new System.Windows.Forms.Panel();
            this.TxtKeyInput5 = new System.Windows.Forms.TextBox();
            this.LbKeyInput5 = new System.Windows.Forms.Label();
            this.PlKeyInput6 = new System.Windows.Forms.Panel();
            this.TxtKeyInput6 = new System.Windows.Forms.TextBox();
            this.LbKeyInput6 = new System.Windows.Forms.Label();
            this.PlKeyInput7 = new System.Windows.Forms.Panel();
            this.TxtKeyInput7 = new System.Windows.Forms.TextBox();
            this.LbKeyInput7 = new System.Windows.Forms.Label();
            this.PlReleaseKey = new System.Windows.Forms.Panel();
            this.TxtReleaseKey = new System.Windows.Forms.TextBox();
            this.LbReleaseKey = new System.Windows.Forms.Label();
            this.PlExeStart1 = new System.Windows.Forms.Panel();
            this.TxtExeStart1 = new System.Windows.Forms.TextBox();
            this.LbExeStart1 = new System.Windows.Forms.Label();
            this.PlExeStart2 = new System.Windows.Forms.Panel();
            this.TxtExeStart2 = new System.Windows.Forms.TextBox();
            this.LbExeStart2 = new System.Windows.Forms.Label();
            this.PlExeStart3 = new System.Windows.Forms.Panel();
            this.TxtExeStart3 = new System.Windows.Forms.TextBox();
            this.LbExeStart3 = new System.Windows.Forms.Label();
            this.PlExeStart4 = new System.Windows.Forms.Panel();
            this.TxtExeStart4 = new System.Windows.Forms.TextBox();
            this.LbExeStart4 = new System.Windows.Forms.Label();
            this.PlExeStart5 = new System.Windows.Forms.Panel();
            this.TxtExeStart5 = new System.Windows.Forms.TextBox();
            this.LbExeStart5 = new System.Windows.Forms.Label();
            this.PlExeStart6 = new System.Windows.Forms.Panel();
            this.TxtExeStart6 = new System.Windows.Forms.TextBox();
            this.LbExeStart6 = new System.Windows.Forms.Label();
            this.PlTop = new System.Windows.Forms.TableLayoutPanel();
            this.BtStart = new System.Windows.Forms.Button();
            this.BtStop = new System.Windows.Forms.Button();
            this.TabSetting = new System.Windows.Forms.TabPage();
            this.PropertyGridSetting = new System.Windows.Forms.PropertyGrid();
            this.ContextMenuStripPropertyGridSetting = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenuItemPropertyGridSettingReset = new System.Windows.Forms.ToolStripMenuItem();
            this.TabControl.SuspendLayout();
            this.TabMain.SuspendLayout();
            this.PlFill.SuspendLayout();
            this.PlInputModeOn.SuspendLayout();
            this.PlInputModeOnEnter.SuspendLayout();
            this.PlInputModeOff.SuspendLayout();
            this.PlActiveWindowClose.SuspendLayout();
            this.PlKeyInput1.SuspendLayout();
            this.PlKeyInput2.SuspendLayout();
            this.PlKeyInput3.SuspendLayout();
            this.PlKeyInput4.SuspendLayout();
            this.PlKeyInput5.SuspendLayout();
            this.PlKeyInput6.SuspendLayout();
            this.PlKeyInput7.SuspendLayout();
            this.PlReleaseKey.SuspendLayout();
            this.PlExeStart1.SuspendLayout();
            this.PlExeStart2.SuspendLayout();
            this.PlExeStart3.SuspendLayout();
            this.PlExeStart4.SuspendLayout();
            this.PlExeStart5.SuspendLayout();
            this.PlExeStart6.SuspendLayout();
            this.PlTop.SuspendLayout();
            this.TabSetting.SuspendLayout();
            this.ContextMenuStripPropertyGridSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.TabMain);
            this.TabControl.Controls.Add(this.TabSetting);
            resources.ApplyResources(this.TabControl, "TabControl");
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            // 
            // TabMain
            // 
            this.TabMain.BackColor = System.Drawing.SystemColors.Control;
            this.TabMain.Controls.Add(this.PlFill);
            this.TabMain.Controls.Add(this.PlTop);
            resources.ApplyResources(this.TabMain, "TabMain");
            this.TabMain.Name = "TabMain";
            // 
            // PlFill
            // 
            this.PlFill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlFill.Controls.Add(this.PlInputModeOn);
            this.PlFill.Controls.Add(this.PlInputModeOnEnter);
            this.PlFill.Controls.Add(this.PlInputModeOff);
            this.PlFill.Controls.Add(this.PlActiveWindowClose);
            this.PlFill.Controls.Add(this.PlKeyInput1);
            this.PlFill.Controls.Add(this.PlKeyInput2);
            this.PlFill.Controls.Add(this.PlKeyInput3);
            this.PlFill.Controls.Add(this.PlKeyInput4);
            this.PlFill.Controls.Add(this.PlKeyInput5);
            this.PlFill.Controls.Add(this.PlKeyInput6);
            this.PlFill.Controls.Add(this.PlKeyInput7);
            this.PlFill.Controls.Add(this.PlReleaseKey);
            this.PlFill.Controls.Add(this.PlExeStart1);
            this.PlFill.Controls.Add(this.PlExeStart2);
            this.PlFill.Controls.Add(this.PlExeStart3);
            this.PlFill.Controls.Add(this.PlExeStart4);
            this.PlFill.Controls.Add(this.PlExeStart5);
            this.PlFill.Controls.Add(this.PlExeStart6);
            resources.ApplyResources(this.PlFill, "PlFill");
            this.PlFill.Name = "PlFill";
            // 
            // PlInputModeOn
            // 
            resources.ApplyResources(this.PlInputModeOn, "PlInputModeOn");
            this.PlInputModeOn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlInputModeOn.Controls.Add(this.TxtInputModeOn);
            this.PlInputModeOn.Controls.Add(this.LbInputModeOn);
            this.PlInputModeOn.Name = "PlInputModeOn";
            // 
            // TxtInputModeOn
            // 
            this.TxtInputModeOn.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageInputModeOn", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtInputModeOn, "TxtInputModeOn");
            this.TxtInputModeOn.Name = "TxtInputModeOn";
            this.TxtInputModeOn.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageInputModeOn;
            // 
            // LbInputModeOn
            // 
            this.LbInputModeOn.AutoEllipsis = true;
            resources.ApplyResources(this.LbInputModeOn, "LbInputModeOn");
            this.LbInputModeOn.Name = "LbInputModeOn";
            // 
            // PlInputModeOnEnter
            // 
            resources.ApplyResources(this.PlInputModeOnEnter, "PlInputModeOnEnter");
            this.PlInputModeOnEnter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlInputModeOnEnter.Controls.Add(this.TxtInputModeOnEnter);
            this.PlInputModeOnEnter.Controls.Add(this.LbInputModeOnEnter);
            this.PlInputModeOnEnter.Name = "PlInputModeOnEnter";
            // 
            // TxtInputModeOnEnter
            // 
            this.TxtInputModeOnEnter.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageInputModeOnEnter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtInputModeOnEnter, "TxtInputModeOnEnter");
            this.TxtInputModeOnEnter.Name = "TxtInputModeOnEnter";
            this.TxtInputModeOnEnter.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageInputModeOnEnter;
            // 
            // LbInputModeOnEnter
            // 
            this.LbInputModeOnEnter.AutoEllipsis = true;
            resources.ApplyResources(this.LbInputModeOnEnter, "LbInputModeOnEnter");
            this.LbInputModeOnEnter.Name = "LbInputModeOnEnter";
            // 
            // PlInputModeOff
            // 
            resources.ApplyResources(this.PlInputModeOff, "PlInputModeOff");
            this.PlInputModeOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlInputModeOff.Controls.Add(this.TxtInputModeOff);
            this.PlInputModeOff.Controls.Add(this.LbInputModeOff);
            this.PlInputModeOff.Name = "PlInputModeOff";
            // 
            // TxtInputModeOff
            // 
            this.TxtInputModeOff.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageInputModeOff", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtInputModeOff, "TxtInputModeOff");
            this.TxtInputModeOff.Name = "TxtInputModeOff";
            this.TxtInputModeOff.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageInputModeOff;
            // 
            // LbInputModeOff
            // 
            this.LbInputModeOff.AutoEllipsis = true;
            resources.ApplyResources(this.LbInputModeOff, "LbInputModeOff");
            this.LbInputModeOff.Name = "LbInputModeOff";
            // 
            // PlActiveWindowClose
            // 
            resources.ApplyResources(this.PlActiveWindowClose, "PlActiveWindowClose");
            this.PlActiveWindowClose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlActiveWindowClose.Controls.Add(this.TxtActiveWindowClose);
            this.PlActiveWindowClose.Controls.Add(this.LbActiveWindowClose);
            this.PlActiveWindowClose.Name = "PlActiveWindowClose";
            // 
            // TxtActiveWindowClose
            // 
            this.TxtActiveWindowClose.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageActiveWindowClose", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtActiveWindowClose, "TxtActiveWindowClose");
            this.TxtActiveWindowClose.Name = "TxtActiveWindowClose";
            this.TxtActiveWindowClose.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageActiveWindowClose;
            // 
            // LbActiveWindowClose
            // 
            this.LbActiveWindowClose.AutoEllipsis = true;
            resources.ApplyResources(this.LbActiveWindowClose, "LbActiveWindowClose");
            this.LbActiveWindowClose.Name = "LbActiveWindowClose";
            // 
            // PlKeyInput1
            // 
            resources.ApplyResources(this.PlKeyInput1, "PlKeyInput1");
            this.PlKeyInput1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlKeyInput1.Controls.Add(this.TxtKeyInput1);
            this.PlKeyInput1.Controls.Add(this.LbKeyInput1);
            this.PlKeyInput1.Name = "PlKeyInput1";
            // 
            // TxtKeyInput1
            // 
            this.TxtKeyInput1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageKeyInput1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtKeyInput1, "TxtKeyInput1");
            this.TxtKeyInput1.Name = "TxtKeyInput1";
            this.TxtKeyInput1.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageKeyInput1;
            // 
            // LbKeyInput1
            // 
            this.LbKeyInput1.AutoEllipsis = true;
            resources.ApplyResources(this.LbKeyInput1, "LbKeyInput1");
            this.LbKeyInput1.Name = "LbKeyInput1";
            // 
            // PlKeyInput2
            // 
            resources.ApplyResources(this.PlKeyInput2, "PlKeyInput2");
            this.PlKeyInput2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlKeyInput2.Controls.Add(this.TxtKeyInput2);
            this.PlKeyInput2.Controls.Add(this.LbKeyInput2);
            this.PlKeyInput2.Name = "PlKeyInput2";
            // 
            // TxtKeyInput2
            // 
            this.TxtKeyInput2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageKeyInput2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtKeyInput2, "TxtKeyInput2");
            this.TxtKeyInput2.Name = "TxtKeyInput2";
            this.TxtKeyInput2.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageKeyInput2;
            // 
            // LbKeyInput2
            // 
            this.LbKeyInput2.AutoEllipsis = true;
            resources.ApplyResources(this.LbKeyInput2, "LbKeyInput2");
            this.LbKeyInput2.Name = "LbKeyInput2";
            // 
            // PlKeyInput3
            // 
            resources.ApplyResources(this.PlKeyInput3, "PlKeyInput3");
            this.PlKeyInput3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlKeyInput3.Controls.Add(this.TxtKeyInput3);
            this.PlKeyInput3.Controls.Add(this.LbKeyInput3);
            this.PlKeyInput3.Name = "PlKeyInput3";
            // 
            // TxtKeyInput3
            // 
            this.TxtKeyInput3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageKeyInput3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtKeyInput3, "TxtKeyInput3");
            this.TxtKeyInput3.Name = "TxtKeyInput3";
            this.TxtKeyInput3.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageKeyInput3;
            // 
            // LbKeyInput3
            // 
            this.LbKeyInput3.AutoEllipsis = true;
            resources.ApplyResources(this.LbKeyInput3, "LbKeyInput3");
            this.LbKeyInput3.Name = "LbKeyInput3";
            // 
            // PlKeyInput4
            // 
            resources.ApplyResources(this.PlKeyInput4, "PlKeyInput4");
            this.PlKeyInput4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlKeyInput4.Controls.Add(this.TxtKeyInput4);
            this.PlKeyInput4.Controls.Add(this.LbKeyInput4);
            this.PlKeyInput4.Name = "PlKeyInput4";
            // 
            // TxtKeyInput4
            // 
            this.TxtKeyInput4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageKeyInput4", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtKeyInput4, "TxtKeyInput4");
            this.TxtKeyInput4.Name = "TxtKeyInput4";
            this.TxtKeyInput4.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageKeyInput4;
            // 
            // LbKeyInput4
            // 
            this.LbKeyInput4.AutoEllipsis = true;
            resources.ApplyResources(this.LbKeyInput4, "LbKeyInput4");
            this.LbKeyInput4.Name = "LbKeyInput4";
            // 
            // PlKeyInput5
            // 
            resources.ApplyResources(this.PlKeyInput5, "PlKeyInput5");
            this.PlKeyInput5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlKeyInput5.Controls.Add(this.TxtKeyInput5);
            this.PlKeyInput5.Controls.Add(this.LbKeyInput5);
            this.PlKeyInput5.Name = "PlKeyInput5";
            // 
            // TxtKeyInput5
            // 
            this.TxtKeyInput5.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageKeyInput5", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtKeyInput5, "TxtKeyInput5");
            this.TxtKeyInput5.Name = "TxtKeyInput5";
            this.TxtKeyInput5.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageKeyInput5;
            // 
            // LbKeyInput5
            // 
            this.LbKeyInput5.AutoEllipsis = true;
            resources.ApplyResources(this.LbKeyInput5, "LbKeyInput5");
            this.LbKeyInput5.Name = "LbKeyInput5";
            // 
            // PlKeyInput6
            // 
            resources.ApplyResources(this.PlKeyInput6, "PlKeyInput6");
            this.PlKeyInput6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlKeyInput6.Controls.Add(this.TxtKeyInput6);
            this.PlKeyInput6.Controls.Add(this.LbKeyInput6);
            this.PlKeyInput6.Name = "PlKeyInput6";
            // 
            // TxtKeyInput6
            // 
            this.TxtKeyInput6.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageKeyInput6", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtKeyInput6, "TxtKeyInput6");
            this.TxtKeyInput6.Name = "TxtKeyInput6";
            this.TxtKeyInput6.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageKeyInput6;
            // 
            // LbKeyInput6
            // 
            this.LbKeyInput6.AutoEllipsis = true;
            resources.ApplyResources(this.LbKeyInput6, "LbKeyInput6");
            this.LbKeyInput6.Name = "LbKeyInput6";
            // 
            // PlKeyInput7
            // 
            resources.ApplyResources(this.PlKeyInput7, "PlKeyInput7");
            this.PlKeyInput7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlKeyInput7.Controls.Add(this.TxtKeyInput7);
            this.PlKeyInput7.Controls.Add(this.LbKeyInput7);
            this.PlKeyInput7.Name = "PlKeyInput7";
            // 
            // TxtKeyInput7
            // 
            this.TxtKeyInput7.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageKeyInput7", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtKeyInput7, "TxtKeyInput7");
            this.TxtKeyInput7.Name = "TxtKeyInput7";
            this.TxtKeyInput7.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageKeyInput7;
            // 
            // LbKeyInput7
            // 
            this.LbKeyInput7.AutoEllipsis = true;
            resources.ApplyResources(this.LbKeyInput7, "LbKeyInput7");
            this.LbKeyInput7.Name = "LbKeyInput7";
            // 
            // PlReleaseKey
            // 
            resources.ApplyResources(this.PlReleaseKey, "PlReleaseKey");
            this.PlReleaseKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlReleaseKey.Controls.Add(this.TxtReleaseKey);
            this.PlReleaseKey.Controls.Add(this.LbReleaseKey);
            this.PlReleaseKey.Name = "PlReleaseKey";
            // 
            // TxtReleaseKey
            // 
            this.TxtReleaseKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageReleaseKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtReleaseKey, "TxtReleaseKey");
            this.TxtReleaseKey.Name = "TxtReleaseKey";
            this.TxtReleaseKey.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageReleaseKey;
            // 
            // LbReleaseKey
            // 
            this.LbReleaseKey.AutoEllipsis = true;
            resources.ApplyResources(this.LbReleaseKey, "LbReleaseKey");
            this.LbReleaseKey.Name = "LbReleaseKey";
            // 
            // PlExeStart1
            // 
            resources.ApplyResources(this.PlExeStart1, "PlExeStart1");
            this.PlExeStart1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlExeStart1.Controls.Add(this.TxtExeStart1);
            this.PlExeStart1.Controls.Add(this.LbExeStart1);
            this.PlExeStart1.Name = "PlExeStart1";
            // 
            // TxtExeStart1
            // 
            this.TxtExeStart1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageExeStart1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtExeStart1, "TxtExeStart1");
            this.TxtExeStart1.Name = "TxtExeStart1";
            this.TxtExeStart1.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageExeStart1;
            // 
            // LbExeStart1
            // 
            this.LbExeStart1.AutoEllipsis = true;
            resources.ApplyResources(this.LbExeStart1, "LbExeStart1");
            this.LbExeStart1.Name = "LbExeStart1";
            // 
            // PlExeStart2
            // 
            resources.ApplyResources(this.PlExeStart2, "PlExeStart2");
            this.PlExeStart2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlExeStart2.Controls.Add(this.TxtExeStart2);
            this.PlExeStart2.Controls.Add(this.LbExeStart2);
            this.PlExeStart2.Name = "PlExeStart2";
            // 
            // TxtExeStart2
            // 
            this.TxtExeStart2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageExeStart2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtExeStart2, "TxtExeStart2");
            this.TxtExeStart2.Name = "TxtExeStart2";
            this.TxtExeStart2.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageExeStart2;
            // 
            // LbExeStart2
            // 
            this.LbExeStart2.AutoEllipsis = true;
            resources.ApplyResources(this.LbExeStart2, "LbExeStart2");
            this.LbExeStart2.Name = "LbExeStart2";
            // 
            // PlExeStart3
            // 
            resources.ApplyResources(this.PlExeStart3, "PlExeStart3");
            this.PlExeStart3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlExeStart3.Controls.Add(this.TxtExeStart3);
            this.PlExeStart3.Controls.Add(this.LbExeStart3);
            this.PlExeStart3.Name = "PlExeStart3";
            // 
            // TxtExeStart3
            // 
            this.TxtExeStart3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageExeStart3", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtExeStart3, "TxtExeStart3");
            this.TxtExeStart3.Name = "TxtExeStart3";
            this.TxtExeStart3.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageExeStart3;
            // 
            // LbExeStart3
            // 
            this.LbExeStart3.AutoEllipsis = true;
            resources.ApplyResources(this.LbExeStart3, "LbExeStart3");
            this.LbExeStart3.Name = "LbExeStart3";
            // 
            // PlExeStart4
            // 
            resources.ApplyResources(this.PlExeStart4, "PlExeStart4");
            this.PlExeStart4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlExeStart4.Controls.Add(this.TxtExeStart4);
            this.PlExeStart4.Controls.Add(this.LbExeStart4);
            this.PlExeStart4.Name = "PlExeStart4";
            // 
            // TxtExeStart4
            // 
            this.TxtExeStart4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageExeStart4", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtExeStart4, "TxtExeStart4");
            this.TxtExeStart4.Name = "TxtExeStart4";
            this.TxtExeStart4.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageExeStart4;
            // 
            // LbExeStart4
            // 
            this.LbExeStart4.AutoEllipsis = true;
            resources.ApplyResources(this.LbExeStart4, "LbExeStart4");
            this.LbExeStart4.Name = "LbExeStart4";
            // 
            // PlExeStart5
            // 
            resources.ApplyResources(this.PlExeStart5, "PlExeStart5");
            this.PlExeStart5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlExeStart5.Controls.Add(this.TxtExeStart5);
            this.PlExeStart5.Controls.Add(this.LbExeStart5);
            this.PlExeStart5.Name = "PlExeStart5";
            // 
            // TxtExeStart5
            // 
            this.TxtExeStart5.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageExeStart5", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtExeStart5, "TxtExeStart5");
            this.TxtExeStart5.Name = "TxtExeStart5";
            this.TxtExeStart5.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageExeStart5;
            // 
            // LbExeStart5
            // 
            this.LbExeStart5.AutoEllipsis = true;
            resources.ApplyResources(this.LbExeStart5, "LbExeStart5");
            this.LbExeStart5.Name = "LbExeStart5";
            // 
            // PlExeStart6
            // 
            resources.ApplyResources(this.PlExeStart6, "PlExeStart6");
            this.PlExeStart6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PlExeStart6.Controls.Add(this.TxtExeStart6);
            this.PlExeStart6.Controls.Add(this.LbExeStart6);
            this.PlExeStart6.Name = "PlExeStart6";
            // 
            // TxtExeStart6
            // 
            this.TxtExeStart6.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SimpleMicRemote.Properties.Settings.Default, "MatchMessageExeStart6", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.TxtExeStart6, "TxtExeStart6");
            this.TxtExeStart6.Name = "TxtExeStart6";
            this.TxtExeStart6.Text = global::SimpleMicRemote.Properties.Settings.Default.MatchMessageExeStart6;
            // 
            // LbExeStart6
            // 
            this.LbExeStart6.AutoEllipsis = true;
            resources.ApplyResources(this.LbExeStart6, "LbExeStart6");
            this.LbExeStart6.Name = "LbExeStart6";
            // 
            // PlTop
            // 
            resources.ApplyResources(this.PlTop, "PlTop");
            this.PlTop.Controls.Add(this.BtStart, 0, 0);
            this.PlTop.Controls.Add(this.BtStop, 1, 0);
            this.PlTop.Name = "PlTop";
            // 
            // BtStart
            // 
            resources.ApplyResources(this.BtStart, "BtStart");
            this.BtStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BtStart.Name = "BtStart";
            this.BtStart.UseVisualStyleBackColor = true;
            this.BtStart.Click += new System.EventHandler(this.BtStart_Click);
            // 
            // BtStop
            // 
            resources.ApplyResources(this.BtStop, "BtStop");
            this.BtStop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtStop.Name = "BtStop";
            this.BtStop.UseVisualStyleBackColor = true;
            this.BtStop.Click += new System.EventHandler(this.BtStop_Click);
            // 
            // TabSetting
            // 
            this.TabSetting.BackColor = System.Drawing.SystemColors.Control;
            this.TabSetting.Controls.Add(this.PropertyGridSetting);
            resources.ApplyResources(this.TabSetting, "TabSetting");
            this.TabSetting.Name = "TabSetting";
            // 
            // PropertyGridSetting
            // 
            this.PropertyGridSetting.ContextMenuStrip = this.ContextMenuStripPropertyGridSetting;
            resources.ApplyResources(this.PropertyGridSetting, "PropertyGridSetting");
            this.PropertyGridSetting.Name = "PropertyGridSetting";
            this.PropertyGridSetting.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGridSetting.ToolbarVisible = false;
            this.PropertyGridSetting.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGridSetting_PropertyValueChanged);
            // 
            // ContextMenuStripPropertyGridSetting
            // 
            this.ContextMenuStripPropertyGridSetting.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuItemPropertyGridSettingReset});
            this.ContextMenuStripPropertyGridSetting.Name = "ContextMenuStripPropertyGridSetting";
            resources.ApplyResources(this.ContextMenuStripPropertyGridSetting, "ContextMenuStripPropertyGridSetting");
            // 
            // ContextMenuItemPropertyGridSettingReset
            // 
            this.ContextMenuItemPropertyGridSettingReset.Name = "ContextMenuItemPropertyGridSettingReset";
            resources.ApplyResources(this.ContextMenuItemPropertyGridSettingReset, "ContextMenuItemPropertyGridSettingReset");
            this.ContextMenuItemPropertyGridSettingReset.Click += new System.EventHandler(this.ContextMenuItemPropertyGridSettingReset_Click);
            // 
            // SimpleForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabControl);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::SimpleMicRemote.Properties.Settings.Default, "ClientPoint", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Location = global::SimpleMicRemote.Properties.Settings.Default.ClientPoint;
            this.Name = "SimpleForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleForm_FormClosing);
            this.TabControl.ResumeLayout(false);
            this.TabMain.ResumeLayout(false);
            this.PlFill.ResumeLayout(false);
            this.PlFill.PerformLayout();
            this.PlInputModeOn.ResumeLayout(false);
            this.PlInputModeOn.PerformLayout();
            this.PlInputModeOnEnter.ResumeLayout(false);
            this.PlInputModeOnEnter.PerformLayout();
            this.PlInputModeOff.ResumeLayout(false);
            this.PlInputModeOff.PerformLayout();
            this.PlActiveWindowClose.ResumeLayout(false);
            this.PlActiveWindowClose.PerformLayout();
            this.PlKeyInput1.ResumeLayout(false);
            this.PlKeyInput1.PerformLayout();
            this.PlKeyInput2.ResumeLayout(false);
            this.PlKeyInput2.PerformLayout();
            this.PlKeyInput3.ResumeLayout(false);
            this.PlKeyInput3.PerformLayout();
            this.PlKeyInput4.ResumeLayout(false);
            this.PlKeyInput4.PerformLayout();
            this.PlKeyInput5.ResumeLayout(false);
            this.PlKeyInput5.PerformLayout();
            this.PlKeyInput6.ResumeLayout(false);
            this.PlKeyInput6.PerformLayout();
            this.PlKeyInput7.ResumeLayout(false);
            this.PlKeyInput7.PerformLayout();
            this.PlReleaseKey.ResumeLayout(false);
            this.PlReleaseKey.PerformLayout();
            this.PlExeStart1.ResumeLayout(false);
            this.PlExeStart1.PerformLayout();
            this.PlExeStart2.ResumeLayout(false);
            this.PlExeStart2.PerformLayout();
            this.PlExeStart3.ResumeLayout(false);
            this.PlExeStart3.PerformLayout();
            this.PlExeStart4.ResumeLayout(false);
            this.PlExeStart4.PerformLayout();
            this.PlExeStart5.ResumeLayout(false);
            this.PlExeStart5.PerformLayout();
            this.PlExeStart6.ResumeLayout(false);
            this.PlExeStart6.PerformLayout();
            this.PlTop.ResumeLayout(false);
            this.TabSetting.ResumeLayout(false);
            this.ContextMenuStripPropertyGridSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage TabMain;
        private System.Windows.Forms.TableLayoutPanel PlTop;
        private System.Windows.Forms.Button BtStart;
        private System.Windows.Forms.Button BtStop;
        private System.Windows.Forms.FlowLayoutPanel PlFill;
        private System.Windows.Forms.Panel PlInputModeOn;
        private System.Windows.Forms.Label LbInputModeOn;
        private System.Windows.Forms.TextBox TxtInputModeOn;
        private System.Windows.Forms.Panel PlInputModeOnEnter;
        private System.Windows.Forms.Label LbInputModeOnEnter;
        private System.Windows.Forms.TextBox TxtInputModeOnEnter;
        private System.Windows.Forms.Panel PlInputModeOff;
        private System.Windows.Forms.Label LbInputModeOff;
        private System.Windows.Forms.TextBox TxtInputModeOff;
        private System.Windows.Forms.Panel PlActiveWindowClose;
        private System.Windows.Forms.Label LbActiveWindowClose;
        private System.Windows.Forms.TextBox TxtActiveWindowClose;
        private System.Windows.Forms.Panel PlKeyInput1;
        private System.Windows.Forms.Label LbKeyInput1;
        private System.Windows.Forms.TextBox TxtKeyInput1;
        private System.Windows.Forms.Panel PlKeyInput2;
        private System.Windows.Forms.Label LbKeyInput2;
        private System.Windows.Forms.TextBox TxtKeyInput2;
        private System.Windows.Forms.Panel PlKeyInput3;
        private System.Windows.Forms.Label LbKeyInput3;
        private System.Windows.Forms.TextBox TxtKeyInput3;
        private System.Windows.Forms.Panel PlKeyInput4;
        private System.Windows.Forms.Label LbKeyInput4;
        private System.Windows.Forms.TextBox TxtKeyInput4;
        private System.Windows.Forms.Panel PlKeyInput5;
        private System.Windows.Forms.Label LbKeyInput5;
        private System.Windows.Forms.TextBox TxtKeyInput5;
        private System.Windows.Forms.Panel PlKeyInput6;
        private System.Windows.Forms.Label LbKeyInput6;
        private System.Windows.Forms.TextBox TxtKeyInput6;
        private System.Windows.Forms.Panel PlKeyInput7;
        private System.Windows.Forms.Label LbKeyInput7;
        private System.Windows.Forms.TextBox TxtKeyInput7;
        private System.Windows.Forms.Panel PlReleaseKey;
        private System.Windows.Forms.Label LbReleaseKey;
        private System.Windows.Forms.TextBox TxtReleaseKey;
        private System.Windows.Forms.Panel PlExeStart1;
        private System.Windows.Forms.Label LbExeStart1;
        private System.Windows.Forms.TextBox TxtExeStart1;
        private System.Windows.Forms.Panel PlExeStart2;
        private System.Windows.Forms.Label LbExeStart2;
        private System.Windows.Forms.TextBox TxtExeStart2;
        private System.Windows.Forms.Panel PlExeStart3;
        private System.Windows.Forms.Label LbExeStart3;
        private System.Windows.Forms.TextBox TxtExeStart3;
        private System.Windows.Forms.Panel PlExeStart4;
        private System.Windows.Forms.Label LbExeStart4;
        private System.Windows.Forms.TextBox TxtExeStart4;
        private System.Windows.Forms.Panel PlExeStart5;
        private System.Windows.Forms.Label LbExeStart5;
        private System.Windows.Forms.TextBox TxtExeStart5;
        private System.Windows.Forms.Panel PlExeStart6;
        private System.Windows.Forms.Label LbExeStart6;
        private System.Windows.Forms.TextBox TxtExeStart6;
        private System.Windows.Forms.TabPage TabSetting;
        private System.Windows.Forms.PropertyGrid PropertyGridSetting;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStripPropertyGridSetting;
        private System.Windows.Forms.ToolStripMenuItem ContextMenuItemPropertyGridSettingReset;
    }
}