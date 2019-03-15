namespace SimpleMicRemote
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.Modules;
    using MisaCommon.Utility.ExtendMethod;
    using MisaCommon.Utility.Win32Api;

    // TODO：動的プロパティ、メソッドの検討

    /// <summary>
    /// 簡易版の音声認識リモコンFormコントロール
    /// </summary>
    public partial class SimpleForm : Form
    {
        #region クラス変数・定数

        /// <summary>
        /// 入力キーで押しっぱなしを示すための記号
        /// </summary>
        private const string _symbolToKeepPressing = "凹";

        /// <summary>
        /// 設定用のパネルコントロールのステータスが[OK]の時のラベルの文字色
        /// </summary>
        private static readonly Color _colorSettingPanelStatusOk = SystemColors.ControlText;

        /// <summary>
        /// 設定用のパネルコントロールのステータスが[NotSet]の時のラベルの文字色
        /// </summary>
        private static readonly Color _colorSettingPanelStatusNotSet = Color.Gray;

        /// <summary>
        /// 設定用のパネルコントロールのステータスが[Error]の時のラベルの文字色
        /// </summary>
        private static readonly Color _colorSettingPanelStatusError = Color.Red;

        /// <summary>
        /// 設定用のパネルコントロールのデフォルトの背景色
        /// </summary>
        private static readonly Color _colorSettingPanelDefault = SystemColors.Control;

        /// <summary>
        /// 設定用のパネルコントロールの使用中の時の背景色
        /// </summary>
        private static readonly Color _colorSettingPanelInUse = Color.FromArgb(192, 255, 192);

        /// <summary>
        /// 現在の入力モード
        /// </summary>
        private InputMode _currentInputMode;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各コントロールの初期値を設定する
        /// </summary>
        public SimpleForm()
        {
            InitializeComponent();

            // Formのサイズを設定
            Size = Properties.Settings.Default.ClientSize;

            // 設定情報を取得し設定
            Setting = new Setting();

            // 設定情報をプロパティグリッドに設定
            PropertyGridSetting.SelectedObject = Setting;

            // 設定用のパネルコントロール情報を設定
            SettingPanelList = CreateSettingPanelList().AsReadOnly();

            // 入力モードとそれに紐づく名称のディクショナリを設定
            InputModeNameDictionary = new ReadOnlyDictionary<InputMode, string>(new Dictionary<InputMode, string>()
            {
                { InputMode.Off, LbInputModeOff.Text },
                { InputMode.On, LbInputModeOn.Text },
                { InputMode.OnEnter, LbInputModeOnEnter.Text },
            });

            // 起動モードのより開始フラグを設定
            bool isStart;
            switch (Setting.StartStartupMode)
            {
                case StartupMode.StartupNormal:
                    // 設定画面：表示、音声認識：開始
                    WindowState = FormWindowState.Normal;
                    isStart = true;
                    break;
                case StartupMode.StartupMinimize:
                    // 設定画面：最小化、音声認識：開始
                    WindowState = FormWindowState.Minimized;
                    isStart = true;
                    break;
                case StartupMode.Setting:
                default:
                    // 設定画面：表示、音声認識：停止
                    WindowState = FormWindowState.Normal;
                    isStart = false;
                    break;
            }

            // 開始フラグを設定
            IsStart = isStart;

            // 入力モードを設定
            // コンストラクタではCurrentInputModeプロパティのInvokeでの画面表示設定処理が実行できないため
            // プロパティを介さず直接値を設定し、画面表示設定処理を行う
            _currentInputMode = Setting.StartInputMode;

            // 開始フラグに応じた各コントロールの表示設定
            RefreshDisplay(IsStart);

            // 開始フラグが立っている場合、音声認識を開始する
            if (IsStart)
            {
                StartSpeechRecognition();
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 設定用のパネルコントロール情報のリストを取得する
        /// </summary>
        private IReadOnlyList<SettingPanel> SettingPanelList { get; }

        /// <summary>
        /// 入力モードとそれに紐づく名称のディクショナリを取得する
        /// </summary>
        private IReadOnlyDictionary<InputMode, string> InputModeNameDictionary { get; }

        /// <summary>
        /// アプリケーションの設定情報（プロパティグリッドに表示する）を取得・設定する
        /// </summary>
        private Setting Setting { get; set; }

        /// <summary>
        /// 音声認識を開始しているかどうかを示すフラグを取得・設定する
        /// </summary>
        private bool IsStart { get; set; }

        /// <summary>
        /// 現在の入力モードを取得・設定する
        /// </summary>
        private InputMode CurrentInputMode
        {
            get => _currentInputMode;
            set
            {
                _currentInputMode = value;

                // 入力モードによる画面設定を行う
                Invoke((MethodInvoker)(() => SetDisplay(value)));
            }
        }

        #endregion

        #region static のプライベートメソッド

        /// <summary>
        /// 入力キーに該当する設定用のパネルコントロールに関するクラスのインスタンスを生成する
        /// </summary>
        /// <param name="inputKeyPanel">入力キーに該当する設定用のパネルコントロール</param>
        /// <param name="setting">プロパティグリッドコントロールに関連付けされている設定情報</param>
        /// <param name="propertyName">対象とする入力キーに該当する設定情報のプロパティ名</param>
        /// <returns>入力キーに該当する設定用のパネルコントロールに関するクラスのインスタンス</returns>
        private static SettingPanel CreateInstanceKeyInput(Panel inputKeyPanel, Setting setting, string propertyName)
        {
            return new SettingPanel(
                inputKeyPanel,
                () =>
                {
                    InputKey inputKey = GetSettingPropertyValue<InputKey>(setting, propertyName);
                    return inputKey.ConvertToString(inputKey);
                },
                (property) => summaryText(new InputKey(property)),
                (property) => CheckKeyInputProperty(property),
                (property) => InputOperate.MouseKeybordInput(new InputKey(property)));

            // ラベルに表示するための概要文字列を取得するローカル関数
            // 引数1：概要文字を生成するInputKeyオブジェクト
            // 戻り値：概要文字
            string summaryText(InputKey inputKey)
            {
                // 文字列に変換し返却
                StringBuilder convertValue = new StringBuilder();

                // Shiftキー（最初の１文字のみ）
                if (inputKey.Shift)
                {
                    convertValue.Append(nameof(inputKey.Shift).Substring(0, 1));
                    convertValue.Append("|");
                }

                // Ctrlキー（最初の１文字のみ）
                if (inputKey.Ctrl)
                {
                    convertValue.Append(nameof(inputKey.Ctrl).Substring(0, 1));
                    convertValue.Append("|");
                }

                // Altキー（最初の１文字のみ）
                if (inputKey.Alt)
                {
                    convertValue.Append(nameof(inputKey.Alt).Substring(0, 1));
                    convertValue.Append("|");
                }

                // Windowsロゴキー（最初の１文字のみ）
                if (inputKey.Win)
                {
                    convertValue.Append(nameof(inputKey.Win).Substring(0, 1));
                    convertValue.Append("|");
                }

                // キーコード
                convertValue.Append(new InputKey(inputKey.KeyCode, false, false, false, false, false).ToString());

                // 押しっぱなしにするかのフラグ（）
                if (inputKey.IsKeepPressing)
                {
                    convertValue.Append("|");
                    convertValue.Append(_symbolToKeepPressing);
                }

                // 生成した文字列を返却
                return convertValue.ToString();
            }
        }

        /// <summary>
        /// 起動に該当する設定用のパネルコントロールに関するクラスのインスタンスを生成する
        /// </summary>
        /// <param name="exeStartPanel">起動に該当する設定用のパネルコントロール</param>
        /// <param name="setting">プロパティグリッドコントロールに関連付けされている設定情報</param>
        /// <param name="propertyName">対象とする起動に該当する設定情報のプロパティ名</param>
        /// <returns>起動に該当する設定用のパネルコントロールに関するクラスのインスタンス</returns>
        private static SettingPanel CreateInstanceExeStart(Panel exeStartPanel, Setting setting, string propertyName)
        {
            return new SettingPanel(
                exeStartPanel,
                () => GetSettingPropertyValue<string>(setting, propertyName),
                (property) => Path.GetFileName(property),
                (property) => CheckExeStartProperty(property),
                (property) => StartProcess.Start(property));
        }

        /// <summary>
        /// 引数の設定情報（<paramref name="setting"/>）から引数のプロパティ名（<paramref name="propertyName"/>）に該当する
        /// 設定値を<typeparamref name="T"/>型で取得する
        /// </summary>
        /// <typeparam name="T">取得するプロパティの型</typeparam>
        /// <param name="setting">プロパティグリッドコントロールに関連付けされている設定情報</param>
        /// <param name="propertyName">入力キーに該当する設定情報のプロパティ名称</param>
        /// <returns>設定情報プロパティの設定値の文字列</returns>
        private static T GetSettingPropertyValue<T>(Setting setting, string propertyName)
        {
            T value = (T)TypeDescriptor.GetProperties(setting).Find(propertyName, false).GetValue(setting);
            return value;
        }

        /// <summary>
        /// 入力キーに該当する設定情報のプロパティの設定値についてチェックを行う
        /// </summary>
        /// <param name="property">入力キーに該当する設定情報のプロパティ</param>
        /// <returns>
        /// チェック結果のプロパティ設定のステータス（<see cref="SettingPanel.PropertySettingStatus"/>）
        /// </returns>
        private static SettingPanel.PropertySettingStatus CheckKeyInputProperty(string property)
        {
            // 入力キーがなしの場合は未設定を返却、それ以外の場合は正常を返却
            return new InputKey(property).KeyCode == Keys.None
                ? SettingPanel.PropertySettingStatus.NotSet : SettingPanel.PropertySettingStatus.Ok;
        }

        /// <summary>
        /// 起動に該当する設定情報のプロパティの設定値についてチェックを行う
        /// </summary>
        /// <param name="property">起動に該当する設定情報のプロパティ</param>
        /// <returns>
        /// チェック結果のプロパティ設定のステータス（<see cref="SettingPanel.PropertySettingStatus"/>）
        /// </returns>
        private static SettingPanel.PropertySettingStatus CheckExeStartProperty(string property)
        {
            // 起動対象のパスが存在しない場合、未設定を返却
            if (string.IsNullOrWhiteSpace(property))
            {
                return SettingPanel.PropertySettingStatus.NotSet;
            }

            // 起動対象のパスのファイル又はフォルダが存在するかチェック
            // 起動対象のパスのファイル又はフォルダが存在しない場合、エラーを返却
            // （ショートカットの場合は拡張子「.lnk」を付与して判定する）
            if (!File.Exists(property)
                && !Directory.Exists(property)
                && !File.Exists(property + ".lnk"))
            {
                return SettingPanel.PropertySettingStatus.Error;
            }

            // 上記チェックがOKの場合、正常を返却
            return SettingPanel.PropertySettingStatus.Ok;
        }

        #endregion

        #region イベントで呼び出されるメソッド

        /// <summary>
        /// このFormを閉じる時のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void SimpleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 音声認識機能を停止する
            SizePoint sizePoint = ChromeSpeechRecognition.Close();

            // プロパティグリッドに閉じたChromeのサイズ位置情報を設定する
            if (sizePoint != null)
            {
                Setting.ChromeSizePoint = sizePoint;
            }

            // 押しっぱなしのキーを解除する
            InputOperate.KeybordReleaseKey();

            // 設定プロパティグリッドの設定値をシステムの設定情報に設定する
            SetSettingInfo();

            // 設定情報を保存する
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// スタートボタン押下のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "メイン画面のイベントにおいて、エラー発生で強制終了させないため")]
        private void BtStart_Click(object sender, EventArgs e)
        {
            try
            {
                // 音声認識を開始する
                StartSpeechRecognition();
            }
            catch (Exception ex)
            {
                // 開始フラグをOFFにする
                IsStart = false;

                // 各コントロールの表示を更新
                RefreshDisplay(IsStart);

                // エラー発生時の処理
                ExceptionHandling.Error(ex);
            }
        }

        /// <summary>
        /// ストップボタン押下のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "メイン画面のイベントにおいて、エラー発生で強制終了させないため")]
        private void BtStop_Click(object sender, EventArgs e)
        {
            try
            {
                // 音声認識を開始する
                StopSpeechRecognition();
            }
            catch (Exception ex)
            {
                // エラー発生時の処理
                ExceptionHandling.Error(ex);
            }
        }

        /// <summary>
        /// プロパティグリッドの設定値を変更した時のイベント
        /// </summary>
        /// <param name="s">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void PropertyGridSetting_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // 各コントロールの表示を更新
            RefreshDisplay(IsStart);
        }

        /// <summary>
        /// プロパティグリッドの右クリックメニューにおけるリセット押下のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void ContextMenuItemPropertyGridSettingReset_Click(object sender, EventArgs e)
        {
            // 選択されている項目の設定値をデフォルト値にリセットする
            PropertyGridSetting.ResetSelectedProperty();
            PropertyGridSetting.Refresh();

            // 各コントロールの表示を更新
            RefreshDisplay(IsStart);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 設定用のパネルコントロール情報のリストを生成する
        /// </summary>
        /// <returns>設定用のパネルコントロール情報のリスト</returns>
        private List<SettingPanel> CreateSettingPanelList()
        {
            // 設定用のパネルコントロール情報のリストを生成
            List<SettingPanel> settingPanels = new List<SettingPanel>
            {
                // 入力モードOFF
                new SettingPanel(
                    PlInputModeOff,
                    null,
                    null,
                    null,
                    (property) => CurrentInputMode = InputMode.Off),

                // 入力モードON
                new SettingPanel(
                    PlInputModeOn,
                    null,
                    null,
                    null,
                    (property) => CurrentInputMode = InputMode.On),

                // 入力モードON:Enter
                new SettingPanel(
                    PlInputModeOnEnter,
                    null,
                    null,
                    null,
                    (property) => CurrentInputMode = InputMode.OnEnter),

                // このウインドウを閉じる
                new SettingPanel(
                    PlActiveWindowClose,
                    null,
                    null,
                    null,
                    (property) => WindowOperate.CloseActiveWindow()),

                // 入力キー1～7
                CreateInstanceKeyInput(PlKeyInput1, Setting, nameof(Setting.InputKey1)),
                CreateInstanceKeyInput(PlKeyInput2, Setting, nameof(Setting.InputKey2)),
                CreateInstanceKeyInput(PlKeyInput3, Setting, nameof(Setting.InputKey3)),
                CreateInstanceKeyInput(PlKeyInput4, Setting, nameof(Setting.InputKey4)),
                CreateInstanceKeyInput(PlKeyInput5, Setting, nameof(Setting.InputKey5)),
                CreateInstanceKeyInput(PlKeyInput6, Setting, nameof(Setting.InputKey6)),
                CreateInstanceKeyInput(PlKeyInput7, Setting, nameof(Setting.InputKey7)),

                // キーのおしっぱ解除
                new SettingPanel(
                    PlReleaseKey,
                    null,
                    null,
                    null,
                    (property) => InputOperate.KeybordReleaseKey()),

                // 起動1～6
                CreateInstanceExeStart(PlExeStart1, Setting, nameof(Setting.StartTargetPath1)),
                CreateInstanceExeStart(PlExeStart2, Setting, nameof(Setting.StartTargetPath2)),
                CreateInstanceExeStart(PlExeStart3, Setting, nameof(Setting.StartTargetPath3)),
                CreateInstanceExeStart(PlExeStart4, Setting, nameof(Setting.StartTargetPath4)),
                CreateInstanceExeStart(PlExeStart5, Setting, nameof(Setting.StartTargetPath5)),
                CreateInstanceExeStart(PlExeStart6, Setting, nameof(Setting.StartTargetPath6)),
            };

            // 生成した設定用のパネルコントロール情報のリストを返却
            return settingPanels;
        }

        /// <summary>
        /// 設定プロパティグリッドの設定値をシステムの設定情報に設定する
        /// </summary>
        /// <remarks>
        /// マッチングメッセージの設定については紐づけを行っているためここでの設定は不要である
        /// </remarks>
        private void SetSettingInfo()
        {
            // クライアントのサイズ、位置情報を取得
            Rectangle clientRectangle = WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;

            // 設定情報を取得し、設定プロパティに値を格納する
            Properties.Settings.Default.ClientSize = clientRectangle.Size;
            Properties.Settings.Default.ClientPoint = clientRectangle.Location;
            Properties.Settings.Default.StartStartupMode = Setting.StartStartupModeString;
            Properties.Settings.Default.StartInputMode = Setting.StartInputModeString;
            Properties.Settings.Default.ChromeSizePoint
                = Setting.ChromeSizePoint?.ConvertToString(Setting.ChromeSizePoint);
            Properties.Settings.Default.InputKey1
                = Setting.InputKey1?.ConvertToString(Setting.InputKey1);
            Properties.Settings.Default.InputKey2
                = Setting.InputKey2?.ConvertToString(Setting.InputKey2);
            Properties.Settings.Default.InputKey3
                = Setting.InputKey3?.ConvertToString(Setting.InputKey3);
            Properties.Settings.Default.InputKey4
                = Setting.InputKey4?.ConvertToString(Setting.InputKey4);
            Properties.Settings.Default.InputKey5
                = Setting.InputKey5?.ConvertToString(Setting.InputKey5);
            Properties.Settings.Default.InputKey6
                = Setting.InputKey6?.ConvertToString(Setting.InputKey6);
            Properties.Settings.Default.InputKey7
                = Setting.InputKey7?.ConvertToString(Setting.InputKey7);
            Properties.Settings.Default.StartTargetPath1 = Setting.StartTargetPath1;
            Properties.Settings.Default.StartTargetPath2 = Setting.StartTargetPath2;
            Properties.Settings.Default.StartTargetPath3 = Setting.StartTargetPath3;
            Properties.Settings.Default.StartTargetPath4 = Setting.StartTargetPath4;
            Properties.Settings.Default.StartTargetPath5 = Setting.StartTargetPath5;
            Properties.Settings.Default.StartTargetPath6 = Setting.StartTargetPath6;
        }

        /// <summary>
        /// 引数の開始フラグ（<paramref name="isStart"/>）を用いて各コントロールの表示を更新する
        /// </summary>
        /// <param name="isStart">音声認識を開始しているかどうかを示すフラグ</param>
        private void RefreshDisplay(bool isStart)
        {
            // スタート・ストップボタンの有効無効を設定する
            BtStart.Enabled = !isStart;
            BtStop.Enabled = isStart;

            // 設定用のパネルコントロールの読み取り専用を設定する
            // 読み取り専用フラグを更新することで画面の各コントロールの表示も更新する
            bool isReadOnly = isStart;
            foreach (SettingPanel settingPanel in SettingPanelList)
            {
                // 設定値が異なる場合のみ設定を行う
                settingPanel.SetReadOnly(isReadOnly);
            }

            // 入力モードに応じた表示を行う
            SetDisplay(CurrentInputMode);

            // プロパティグリッドの有効無効を設定する
            PropertyGridSetting.Enabled = !isStart;
        }

        /// <summary>
        /// 引数の入力モード（<paramref name="inputMode"/>）に応じた画面の表示設定を行う
        /// </summary>
        /// <param name="inputMode">入力モード</param>
        private void SetDisplay(InputMode inputMode)
        {
            // ラベルの文字を太字設定を切り替えるためのローカル関数
            // 引数１：元のフォントオブジェクト
            // 引数２：太字にする場合：True、太字を解除する場合：False
            // 戻り値：元のフォントオブジェクトから太字の切り替えを行ったFontオブジェクト
            Font SwitchBold(Font original, bool isBold)
            {
                // 太字フラグに応じた新しいスタイルを設定
                // （|：ORのビット演算、&：ANDのビット演算、~：反転ビット演算）
                FontStyle newStyle = isBold
                    ? original.Style | FontStyle.Bold
                    : original.Style & ~FontStyle.Bold;

                // 新しいスタイルに応じたFontオブジェクトを生成し返却する
                return new Font(original, newStyle);
            }

            // 入力モードに応じて対象ラベルのスタイルを切り替える
            // （音声認識が起動している時のみスタイルを変更する）
            switch (inputMode)
            {
                case InputMode.Off:
                    LbInputModeOn.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    LbInputModeOnEnter.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    LbInputModeOff.Font = SwitchBold(LbInputModeOnEnter.Font, IsStart);
                    break;
                case InputMode.On:
                    LbInputModeOn.Font = SwitchBold(LbInputModeOnEnter.Font, IsStart);
                    LbInputModeOnEnter.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    LbInputModeOff.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    break;
                case InputMode.OnEnter:
                    LbInputModeOn.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    LbInputModeOnEnter.Font = SwitchBold(LbInputModeOnEnter.Font, IsStart);
                    LbInputModeOff.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    break;
                default:
                    LbInputModeOn.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    LbInputModeOnEnter.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    LbInputModeOff.Font = SwitchBold(LbInputModeOnEnter.Font, false);
                    break;
            }
        }

        /// <summary>
        /// 音声認識の処理を開始する
        /// </summary>
        private void StartSpeechRecognition()
        {
            // 開始フラグをONにする
            IsStart = true;

            // 各コントロールの表示を更新
            RefreshDisplay(IsStart);

            // Chromeのサイズ、起動位置情報を取得
            SizePoint sizePoint = Setting.ChromeSizePoint ?? new SizePoint(Properties.Settings.Default.ChromeSizePoint);

            // 音声認識に使用するローカルHTTPサーバのポート番号を取得
            // 設定情報に存在するポート番号が数値型に変換可能である場合、
            // ポート番号の設定が存在すると判定しそのポート番号を使用してローカルHTTPサーバを起動する
            int? port = null;
            string tmpPortString = Properties.Settings.Default.ChromeSpeechRecognitionPort;
            if (!string.IsNullOrWhiteSpace(tmpPortString) && int.TryParse(tmpPortString, out int tmpPort))
            {
                port = tmpPort;
            }

            // マッチングメッセージと処理のリストを生成
            IReadOnlyList<KeyValuePair<string, Action>> matchingList
                = SettingPanelList.Select(
                    settingPanel => new KeyValuePair<string, Action>(
                        settingPanel.MatchMessage,
                        () => settingPanel.ProcessToMatchingAction(settingPanel.GetPropertySettingFunc())))
                    .ToList().AsReadOnly();

            // 音声認識のFaviconを設定
            ChromeSpeechRecognition.FaviconData = Properties.Resources.icon;

            // Chromeによる音声認識を開始
            ChromeSpeechRecognition.Start(
                Properties.Settings.Default.ChromeSpeechRecognitionHtmlFilePath,
                Properties.Settings.Default.ChromeSpeechRecognitionPostRegex,
                Properties.Settings.Default.ChromeSpeechRecognitionPostParamName,
                sizePoint,
                port,
                (recognitionText) =>
                {
                    // 音声認識文字か空の場合は処理をせず、レスポンスデータを返却する
                    if (!string.IsNullOrWhiteSpace(recognitionText))
                    {
                        // 音声認識した文字に概要するデータをマッチングリストから取得
                        KeyValuePair<string, Action> matchData
                            = matchingList.FirstOrDefault(x => x.Key.EqualsWithNull(recognitionText));

                        // マッチングデータが取得できたか判定
                        if (!matchData.Equals(default(KeyValuePair<string, Action>)))
                        {
                            // マッチングデータが取得できた場合
                            // マッチングデータに紐づく処理を実行する
                            matchData.Value();
                        }
                        else if (CurrentInputMode == InputMode.On)
                        {
                            // マッチングデータが存在せず、入力モードが ON の場合
                            // 入力モードに応じた音声認識文字の入力処理を行う
                            // （入力モードOffの場合はなにもしない）
                            // 入力モード時の入力
                            InputOperate.TextInput(recognitionText, false);
                        }
                        else if (CurrentInputMode == InputMode.OnEnter)
                        {
                            // マッチングデータが存在せず、入力モードが OnEnter の場合
                            // 入力モードに応じた音声認識文字の入力処理を行う
                            // （入力モードOffの場合はなにもしない）
                            // 入力モード：Enter時の入力
                            InputOperate.TextInput(recognitionText, true);
                        }
                    }

                    // 入力モードに該当する文字列（UTF-8）を返却する（レスポンス）データとして返却
                    return Encoding.UTF8.GetBytes(InputModeNameDictionary[CurrentInputMode]);
                });

            // 起動に成功した場合、起動に成功したポート番号を保持する
            Properties.Settings.Default.ChromeSpeechRecognitionPort
                = ChromeSpeechRecognition.UseLocalHttpServerPort?.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 音声認識の処理を停止する
        /// </summary>
        private void StopSpeechRecognition()
        {
            // 開始フラグをOFFにする
            IsStart = false;

            // 音声認識機能を停止する
            SizePoint sizePoint = ChromeSpeechRecognition.Close();

            // プロパティグリッドに閉じたChromeのサイズ位置情報を設定する
            if (sizePoint != null)
            {
                Setting.ChromeSizePoint = sizePoint;
            }

            // 押しっぱなしのキーを解除する
            InputOperate.KeybordReleaseKey();

            // 各コントロールの表示を更新
            RefreshDisplay(IsStart);
        }

        #endregion

        #region 内部クラス

        /// <summary>
        /// 設定用のパネルコントロールを扱うクラス
        /// コンストラクタの生成は<see cref="SimpleForm"/>のコンストラクタで行うこと
        /// </summary>
        private class SettingPanel
        {
            #region コンストラクタ

            /// <summary>
            /// コンストラクタ
            /// 各プロパティの値を初期化する
            /// </summary>
            /// <param name="panel">
            /// 設定用のパネルコントロール
            /// </param>
            /// <param name="getPropertySetting">
            /// 紐づくプロパティ設定から値を取得するための処理
            /// プロパティ設定は不要な項目の場合はNULLを指定する
            /// ・戻り値：プロパティ設定の設定値
            /// </param>
            /// <param name="getPropertySummaryText">
            /// 紐づくプロパティ設定の設定値の概要文字を取得する（概要文字はラベルに表示する）
            /// プロパティ設定は不要な項目の場合はNULLを指定する
            /// ・引数１：プロパティ設定の設定値
            /// ・戻り値：プロパティ設定の設定値から生成したラベルに表示する文字列
            /// </param>
            /// <param name="checkPropertySetting">
            /// 紐づくプロパティ設定が正しいか判定するための処理
            /// プロパティ設定は不要な項目の場合はNULLを指定する
            /// ・引数１：プロパティ設定の設定値
            /// ・引数２：プロパティ設定からラベルに表示する文字を取得する処理
            /// 　　　　（引数１：プロパティ設定の設定値、戻り値：ラベルに表示する文字列）
            /// ・戻り値：プロパティ設定のステータス（<see cref="PropertySettingStatus"/>）
            /// </param>
            /// <param name="processToMatching">
            /// マッチングした際に実行する処理
            /// ・引数１：プロパティ設定の設定値
            /// </param>
            public SettingPanel(
                Panel panel,
                Func<string> getPropertySetting,
                Func<string, string> getPropertySummaryText,
                Func<string, PropertySettingStatus> checkPropertySetting,
                Action<string> processToMatching)
            {
                // パネル及びパネルの子コントロールのラベル、テキストボックスを取得し設定する
                Panel = panel;
                foreach (Control control in Panel.Controls)
                {
                    if (Label == null && control is Label label)
                    {
                        Label = label;
                    }
                    else if (TextBox == null && control is TextBox textBox)
                    {
                        TextBox = textBox;
                    }

                    if (Label != null && TextBox != null)
                    {
                        break;
                    }
                }

                // ラベルのデフォルト値を保持する
                DefaultLabelText = Label.Text;

                // 紐づくプロパティ設定から値を取得するための処理
                // 引数の値がNULLの場合（プロパティ設定を使用しない場合）はデフォルトの処理を設定する
                GetPropertySettingFunc = getPropertySetting ?? (() => null);

                // 紐づくプロパティ設定の設定値の概要文字を取得する（概要文字はラベルに表示する）
                // 引数の値がNULLの場合（プロパティ設定を使用しない場合）はデフォルトの処理を設定する
                GetPropertySummaryText = getPropertySummaryText ?? ((property) => null);

                // 紐づくプロパティ設定が正しいか判定するための処理
                // 引数の値がNULLの場合（プロパティ設定を使用しない場合）はデフォルトの処理を設定する
                CheckPropertySetting = checkPropertySetting ?? ((property) => PropertySettingStatus.Ok);

                // マッチングした際に実行する処理
                // 引数の値がNULLの場合（プロパティ設定を使用しない場合）はデフォルトの処理を設定する
                ProcessToMatchingAction = processToMatching;
            }

            #endregion

            #region Enum定義

            /// <summary>
            /// プロパティ設定のステータス
            /// </summary>
            public enum PropertySettingStatus
            {
                /// <summary>
                /// 正常
                /// </summary>
                Ok,

                /// <summary>
                /// 未設定
                /// </summary>
                NotSet,

                /// <summary>
                /// エラー
                /// </summary>
                Error,
            }

            #endregion

            #region プロパティ

            /// <summary>
            /// マッチングメッセージ
            /// テキストボックスが入力されていない、プロパティ設定が不正な場合はNULL
            /// </summary>
            public string MatchMessage =>

                    // ※ Statusプロパティの画面設定処理を実行するためStatusのチェック先に行う
                    GetStatus() == PropertySettingStatus.Ok && !string.IsNullOrWhiteSpace(TextBox.Text)
                        ? TextBox.Text : null;

            /// <summary>
            /// 紐づくプロパティ設定から値を取得するための処理を取得する
            /// 戻り値：プロパティ設定の設定値
            /// </summary>
            public Func<string> GetPropertySettingFunc { get; }

            /// <summary>
            /// マッチングした際に実行する処理を取得する
            /// 引数１：プロパティ設定の設定値
            /// </summary>
            public Action<string> ProcessToMatchingAction { get; }

            /// <summary>
            /// 設定用のパネルコントロールを取得する
            /// </summary>
            private Panel Panel { get; }

            /// <summary>
            /// 設定用のパネルに配置されているラベルを取得する
            /// </summary>
            private Label Label { get; }

            /// <summary>
            /// 設定用のパネルに配置されているテキストボックスを取得する
            /// </summary>
            private TextBox TextBox { get; }

            /// <summary>
            /// 設定用のパネルに配置されているラベルのデフォルトテキストを取得する
            /// </summary>
            private string DefaultLabelText { get; }

            /// <summary>
            /// 紐づくプロパティ設定の設定値の概要文字を取得する（概要文字はラベルに表示する）
            /// 引数１：プロパティ設定の設定値
            /// 戻り値：プロパティ設定の設定値から生成したラベルに表示する文字列
            /// </summary>
            private Func<string, string> GetPropertySummaryText { get; }

            /// <summary>
            /// 紐づくプロパティ設定が正しいか判定するための処理を取得する
            /// 引数１：プロパティ設定の設定値
            /// 引数２：プロパティ設定からラベルに表示する文字を取得する処理
            /// 　　　（引数１：プロパティ設定の設定値、戻り値：ラベルに表示する文字列）
            /// 戻り値：プロパティ設定のステータス（<see cref="PropertySettingStatus"/>）
            /// </summary>
            private Func<string, PropertySettingStatus> CheckPropertySetting { get; }

            #endregion

            #region メソッド

            /// <summary>
            /// 読み取り専用の設定を行う
            /// </summary>
            /// <param name="isReadOnly">読み取り専用にするかどうかのフラグ</param>
            public void SetReadOnly(bool isReadOnly)
            {
                // 読み取り専用の設定
                TextBox.ReadOnly = isReadOnly;

                // 読み取り専用かつ、マッチングメッセージが設定されている（利用する）かどうかで背景色の設定する
                // ※ MatchMessageプロパティのチェックを先の行うことでStatusプロパティの画面設定処理を実行している
                Panel.BackColor = MatchMessage != null && isReadOnly
                    ? _colorSettingPanelInUse : _colorSettingPanelDefault;
            }

            /// <summary>
            /// 紐づくプロパティ設定のステータスを取得する
            /// </summary>
            /// <returns>紐づくプロパティ設定のステータス</returns>
            private PropertySettingStatus GetStatus()
            {
                // プロパティの設定値を取得
                string property = GetPropertySettingFunc();

                // チェック処理を実行しプロパティ設定のステータスを取得する
                PropertySettingStatus status = CheckPropertySetting(property);

                // ステータスに応じた画面表示用のテキストと文字色を設定する
                string text;
                Color foreColor;
                switch (status)
                {
                    case PropertySettingStatus.Ok:
                        // 正常の場合
                        text = DefaultLabelText + GetPropertySummaryText(property);
                        foreColor = _colorSettingPanelStatusOk;
                        break;
                    case PropertySettingStatus.NotSet:
                        // 未設定の場合
                        text = DefaultLabelText + Properties.Resources.SimpleFormMatchLabelTextNotSet;
                        foreColor = _colorSettingPanelStatusNotSet;
                        break;
                    case PropertySettingStatus.Error:
                    default:
                        // エラーの場合（デフォルトもエラーとして扱う）
                        text = DefaultLabelText + Properties.Resources.SimpleFormMatchLabelTextError;
                        foreColor = _colorSettingPanelStatusError;
                        break;
                }

                // ラベルの表示設定を行う
                Label.Text = text;
                Label.ForeColor = foreColor;

                // チェック結果を返却
                return status;
            }

            #endregion
        }

        #endregion
    }
}
