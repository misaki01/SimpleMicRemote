namespace SimpleMicRemote
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.Modules;
    using MisaCommon.Utility.Win32Api;

    // TODO：動的プロパティ、メソッドの検討

    /// <summary>
    /// 簡易版の音声認識リモコンFormコントロール
    /// </summary>
    public partial class SimpleForm : Form
    {
        #region クラス変数・定数

        /// <summary>
        /// 現在の入力モード
        /// </summary>
        private InputMode currentInputMode;

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
            InputModeNameDictionary
                = new ReadOnlyDictionary<InputMode, string>(new Dictionary<InputMode, string>()
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
            currentInputMode = Setting.StartInputMode;

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
            get => currentInputMode;
            set
            {
                currentInputMode = value;

                // 入力モードによる画面設定を行う
                Invoke((MethodInvoker)(() => SetDisplay(value)));
            }
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

        #region 画面の表示設定

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
                SettingPanel.CreateInstanceKeyInput(PlKeyInput1, Setting, nameof(Setting.InputKey1)),
                SettingPanel.CreateInstanceKeyInput(PlKeyInput2, Setting, nameof(Setting.InputKey2)),
                SettingPanel.CreateInstanceKeyInput(PlKeyInput3, Setting, nameof(Setting.InputKey3)),
                SettingPanel.CreateInstanceKeyInput(PlKeyInput4, Setting, nameof(Setting.InputKey4)),
                SettingPanel.CreateInstanceKeyInput(PlKeyInput5, Setting, nameof(Setting.InputKey5)),
                SettingPanel.CreateInstanceKeyInput(PlKeyInput6, Setting, nameof(Setting.InputKey6)),
                SettingPanel.CreateInstanceKeyInput(PlKeyInput7, Setting, nameof(Setting.InputKey7)),

                // キーのおしっぱ解除
                new SettingPanel(
                    PlReleaseKey,
                    null,
                    null,
                    null,
                    (property) => InputOperate.KeybordReleaseKey()),

                // 起動1～6
                SettingPanel.CreateInstanceExeStart(PlExeStart1, Setting, nameof(Setting.StartTargetPath1)),
                SettingPanel.CreateInstanceExeStart(PlExeStart2, Setting, nameof(Setting.StartTargetPath2)),
                SettingPanel.CreateInstanceExeStart(PlExeStart3, Setting, nameof(Setting.StartTargetPath3)),
                SettingPanel.CreateInstanceExeStart(PlExeStart4, Setting, nameof(Setting.StartTargetPath4)),
                SettingPanel.CreateInstanceExeStart(PlExeStart5, Setting, nameof(Setting.StartTargetPath5)),
                SettingPanel.CreateInstanceExeStart(PlExeStart6, Setting, nameof(Setting.StartTargetPath6)),
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

        #endregion

        #region 音声認識の処理

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
            SizePoint sizePoint = Setting.ChromeSizePoint ??
                new SizePoint(Properties.Settings.Default.ChromeSizePoint);

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
                            = matchingList.FirstOrDefault(
                                x => string.Equals(x.Key, recognitionText, StringComparison.Ordinal));

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

        #endregion
    }
}
