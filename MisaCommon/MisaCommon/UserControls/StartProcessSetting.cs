namespace MisaCommon.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    using MisaCommon.CustomType;
    using MisaCommon.CustomType.Attribute;
    using MisaCommon.MessageResources.UserControl;
    using MisaCommon.Modules;

    /// <summary>
    /// プロセスの実行に関する情報の設定を行うためのユーザコントロール
    /// </summary>
    public partial class StartProcessSetting : UserControl, ISpeechRecognitionSettingControl
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各コントロールの初期化を行う
        /// </summary>
        public StartProcessSetting()
        {
            InitializeComponent();

            // 画面の各コントロールの初期化を行う
            Initialize();
        }

        /// <summary>
        /// コンストラクタ
        /// 各コントロールの初期化を行う
        /// </summary>
        /// <param name="startProcessInfo">プロセスの実行に関する情報</param>
        public StartProcessSetting(StartProcessInfo startProcessInfo)
            : this()
        {
            // 画面の各コントロールの初期化を行う
            Initialize(startProcessInfo);
        }

        #endregion

        #region イベント定義（ISpeechRecognitionSettingControlの実装）

        /// <summary>
        /// 設定データを変更した際に発生させるイベント
        /// </summary>
        [LocalizableCategory("PropertyChangeCategory", typeof(StartProcessSettingMessage))]
        [LocalizableDescription("SettingDataChangedDescription", typeof(StartProcessSettingMessage))]
        public event EventHandler SettingDataChanged;

        #endregion

        #region 公開プロパティ

        #region ISpeechRecognitionSettingControlの実装

        /// <summary>
        /// 操作設定の設定データ（文字列形式）を取得する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public string GetSettingData
        {
            get
            {
                // このクラスで設定されたプロセスの実行に関する情報を文字列に変換して返却する
                StartProcessInfo startProcessInfo = SettingStartProcessInfo;
                return startProcessInfo?.ConvertToString(startProcessInfo);
            }
        }

        #endregion

        /// <summary>
        /// 現在のプロセスの実行に関する情報を取得・設定する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public StartProcessInfo CurrentStartProcessInfo { get; set; }

        /// <summary>
        /// このコントロールで設定したプロセスの実行に関する情報（未設定、キャンセルの場合はNULL）を取得する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public StartProcessInfo SettingStartProcessInfo
        {
            get => StartInfo?.DeepCopy();
        }

        #endregion

        #region プライベートプロパティ

        /// <summary>
        /// このコントロールで現在設定しているプロセスの実行に関する情報を取得・設定する
        /// </summary>
        private StartProcessInfo StartInfo { get; set; } = new StartProcessInfo();

        #endregion

        #region 公開メソッド（ISpeechRecognitionSettingControlの実装）

        /// <summary>
        /// 設定データの概要を示すテキストを取得する
        /// </summary>
        /// <param name="settingData">設定データ</param>
        /// <returns>設定データの概要を示すテキスト</returns>
        public string GetSummaryText(string settingData)
        {
            // 引数の設定データからプロセスの実行に関する情報を生成する
            StartProcessInfo startProcessInfo = new StartProcessInfo();
            startProcessInfo = startProcessInfo.ConvertFromString(settingData ?? string.Empty);

            // プロセスの実行に関する情報を文字列に変換する
            // 起動するファイルのパス
            string fileName;
            try
            {
                fileName = Path.GetFileName(startProcessInfo.ProcessPath);
            }
            catch (ArgumentException)
            {
                // 設定値がファイルパス形式出ない場合は設定値をそのまま使用する
                fileName = startProcessInfo.ProcessPath;
            }

            // 起動したプロセスの位置とサイズ
            string sizePoint = startProcessInfo.SizePoint?.ToString();

            // 設定値の概要を生成する
            StringBuilder convertValue = new StringBuilder();
            convertValue.Append(fileName ?? string.Empty);
            convertValue.Append(" | ");
            convertValue.Append(sizePoint ?? string.Empty);

            // 概要文字列を生成し返却する
            return string.Format(
                CultureInfo.InvariantCulture,
                StartProcessSettingMessage.SummaryTextFormat,
                convertValue.ToString());
        }

        /// <summary>
        /// 操作設定のコントロールを引数の設定データ（<paramref name="settingData"/>）で初期化したコントロールを取得する
        /// </summary>
        /// <param name="settingData">設定データ</param>
        /// <returns> 操作設定のコントロールを引数の設定データ（<paramref name="settingData"/>）で初期化したコントロール</returns>
        public Control GetInitializeControl(string settingData)
        {
            // 引数の設定データからプロセスの実行に関する情報を生成する
            StartProcessInfo startProcessInfo = new StartProcessInfo();
            startProcessInfo = startProcessInfo.ConvertFromString(settingData ?? string.Empty);

            // プロセスの実行に関する情報で初期化を行い、このコントロールを返却する
            Initialize(startProcessInfo);
            return this;
        }

        #endregion

        #region イベントで呼び出されるメソッド

        /// <summary>
        /// 起動プロセスパス変更のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void TxtPath_TextChanged(object sender, EventArgs e)
        {
            // 起動プロセスのパスを更新する
            StartInfo.ProcessPath = string.IsNullOrWhiteSpace(TxtPath.Text) ? null : TxtPath.Text;

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        /// <summary>
        /// 起動プロセス選択用のボタン押下イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">キー押下のイベントデータ</param>
        private void BtPath_Click(object sender, EventArgs e)
        {
            // ファイル選択ダイアログを開き、回転パラメータファイルを選択する
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                // ファイル選択ダイアログの表示設定
                dialog.Title = StartProcessSettingMessage.FileSelectDialogTitle;
                dialog.Filter = StartProcessSettingMessage.FileSelectDialogFilter;
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;

                // ダイアログを表示
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // ファイル選択でOKが押された場合、選択したファイルパスをテキストボックスに設定
                    TxtPath.Text = dialog.FileName;
                }
            }
        }

        /// <summary>
        /// 起動パラメータ変更のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void TxtParam_TextChanged(object sender, EventArgs e)
        {
            // 起動パラメータのパスを更新する
            StartInfo.StartupParam = string.IsNullOrWhiteSpace(TxtParam.Text) ? null : TxtParam.Text;

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        /// <summary>
        /// ディレイを指定しないのチェックボックスのチェック変更イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">キー押下のイベントデータ</param>
        private void ChkDelay_CheckedChanged(object sender, EventArgs e)
        {
            // センダーオブジェクトからチェックボックスを取得する
            // チェックボックスのオブジェクトでない場合は処理を行わない
            if (!(sender is CheckBox checkBox))
            {
                return;
            }

            // ディレイの有効無効を切り替え
            TxtDelay.Enabled = !checkBox.Checked;

            // 未チェックの場合、ディレイを更新する（使用しないためNULLとする）
            if (!checkBox.Checked)
            {
                StartInfo.WaitDelay = null;
            }

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        /// <summary>
        /// ディレイ変更のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void TxtDelay_ValueChanged(object sender, EventArgs e)
        {
            // ディレイを更新する
            StartInfo.WaitDelay = decimal.ToInt32(TxtDelay.Value);

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        /// <summary>
        /// サイズ位置を指定しないのチェックボックスのチェック変更イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">キー押下のイベントデータ</param>
        private void ChkSizePosition_CheckedChanged(object sender, EventArgs e)
        {
            // センダーオブジェクトからチェックボックスを取得する
            // チェックボックスのオブジェクトでない場合は処理を行わない
            if (!(sender is CheckBox checkBox))
            {
                return;
            }

            // サイズ位置の有効無効を切り替え
            PlSizePosition.Enabled = !checkBox.Checked;

            // 未チェックの場合、サイズ位置を更新する（使用しないためNULLとする）
            if (!checkBox.Checked)
            {
                StartInfo.SizePoint = null;
            }

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        /// <summary>
        /// サイズ位置の変更のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void SizePosition_ValueChanged(object sender, EventArgs e)
        {
            // サイズ位置を更新する
            int width = decimal.ToInt32(TxtSizeWidth.Value);
            int height = decimal.ToInt32(TxtSizeHeight.Value);
            int x = decimal.ToInt32(TxtPositionX.Value);
            int y = decimal.ToInt32(TxtPositionY.Value);
            StartInfo.SizePoint = new SizePoint(width, height, x, y);

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 引数のプロセスの実行に関する情報（<paramref name="startProcessInfo"/>）で初期化を行う
        /// </summary>
        /// <param name="startProcessInfo">プロセスの実行に関する情報</param>
        private void Initialize(StartProcessInfo startProcessInfo = null)
        {
            // 引数のプロセスの実行に関する情報をプロパティに設定
            CurrentStartProcessInfo = startProcessInfo;
            StartInfo = startProcessInfo?.DeepCopy() ?? new StartProcessInfo();
            StartProcessInfo info = StartInfo.DeepCopy();

            // 起動パスの値が存在する場合はその値を設定する
            if (!string.IsNullOrWhiteSpace(info.ProcessPath))
            {
                TxtPath.Text = info.ProcessPath;
            }

            // 起動パラメータの値が存在する場合はその値を設定する
            if (!string.IsNullOrWhiteSpace(info.StartupParam))
            {
                TxtParam.Text = info.StartupParam;
            }

            // ディレイの値が存在する場合はその値を設定する
            if (info.WaitDelay.HasValue)
            {
                TxtDelay.Value = info.WaitDelay.Value;

                // 使用しないのチェックを外す
                ChkDelay.Checked = false;
            }
            else
            {
                // ディレイ入力のテキストボックスの初期値は現在デフォルトで使用されるディレイの値を設定する
                TxtDelay.Value = StartProcess.WaitDelay;

                // 使用しないのチェックを入れる
                ChkDelay.Checked = true;
            }

            // サイズ位置の値が存在する場合はその値を設定する
            if (info.SizePoint != null)
            {
                TxtSizeWidth.Value = info.SizePoint.SizeWidth;
                TxtSizeHeight.Value = info.SizePoint.SizeHeight;
                TxtPositionX.Value = info.SizePoint.PositionX;
                TxtPositionY.Value = info.SizePoint.PositionY;

                // 使用しないのチェックを外す
                ChkSizePosition.Checked = false;
            }
            else
            {
                // サイズ位置のテキストボックスの初期値は0を設定する
                TxtSizeWidth.Value = 0;
                TxtSizeHeight.Value = 0;
                TxtPositionX.Value = 0;
                TxtPositionY.Value = 0;

                // 使用しないのチェックを入れる
                ChkSizePosition.Checked = true;
            }
        }

        #endregion
    }
}
