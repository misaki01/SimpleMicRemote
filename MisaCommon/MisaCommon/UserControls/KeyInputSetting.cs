namespace MisaCommon.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;

    using MisaCommon.CustomType;
    using MisaCommon.CustomType.Attribute;
    using MisaCommon.MessageResources.UserControl;
    using MisaCommon.Utility.StaticMethod;

    /// <summary>
    /// キーボード入力の設定を行うためのユーザコントロール
    /// </summary>
    public partial class KeyInputSetting : UserControl, ISpeechRecognitionSettingControl
    {
        #region クラス定数

        /// <summary>
        /// このコントロールの最小幅
        /// </summary>
        private const int MinWidthForAutoSize = 400;

        /// <summary>
        /// コンボボックスの最大の幅（ピクセル）
        /// </summary>
        private const int MaxComboBoxWidth = 300;

        /// <summary>
        /// スクロールバーのサイズの定義
        /// </summary>
        private readonly Size _scrollBarSize = new Size(30, 30);

        #region マウス用のコンボボックスのデータ

        /// <summary>
        /// マウス用のコンボボックスデータ
        /// </summary>
        private IReadOnlyDictionary<Keys, string> _comboBoxDataMouse = new ReadOnlyDictionary<Keys, string>(
            new Dictionary<Keys, string>()
            {
                // なし
                { Keys.None, KeyNameMapping.GetName(Keys.None) },

                // マウスの左ボタン
                { Keys.LButton, KeyNameMapping.GetName(Keys.LButton) },

                // マウスの右ボタン
                { Keys.RButton, KeyNameMapping.GetName(Keys.RButton) },

                // マウスの中央ボタン（3ボタンマウス）
                { Keys.MButton, KeyNameMapping.GetName(Keys.MButton) },

                // マウスの第1拡張ボタン（5ボタンマウス）
                { Keys.XButton1, KeyNameMapping.GetName(Keys.XButton1) },

                // マウスの第2拡張ボタン（5ボタンマウス）
                { Keys.XButton2, KeyNameMapping.GetName(Keys.XButton2) },
            });

        #endregion

        #region 入力キー用のコンボボックスデータ

        /// <summary>
        /// 入力キー用のコンボボックスデータ
        /// </summary>
        private IReadOnlyDictionary<Keys, string> _comboBoxDataInputKey = new ReadOnlyDictionary<Keys, string>(
            new Dictionary<Keys, string>()
            {
                // なし
                { Keys.None, KeyNameMapping.GetName(Keys.None) },

                // {0}キー
                { Keys.D0, KeyNameMapping.GetName(Keys.D0) },

                // {1}キー
                { Keys.D1, KeyNameMapping.GetName(Keys.D1) },

                // {2}キー
                { Keys.D2, KeyNameMapping.GetName(Keys.D2) },

                // {3}キー
                { Keys.D3, KeyNameMapping.GetName(Keys.D3) },

                // {4}キー
                { Keys.D4, KeyNameMapping.GetName(Keys.D4) },

                // {5}キー
                { Keys.D5, KeyNameMapping.GetName(Keys.D5) },

                // {6}キー
                { Keys.D6, KeyNameMapping.GetName(Keys.D6) },

                // {7}キー
                { Keys.D7, KeyNameMapping.GetName(Keys.D7) },

                // {8}キー
                { Keys.D8, KeyNameMapping.GetName(Keys.D8) },

                // {9}キー
                { Keys.D9, KeyNameMapping.GetName(Keys.D9) },

                // {A}キー
                { Keys.A, KeyNameMapping.GetName(Keys.A) },

                // {B}キー
                { Keys.B, KeyNameMapping.GetName(Keys.B) },

                // {C}キー
                { Keys.C, KeyNameMapping.GetName(Keys.C) },

                // {D}キー
                { Keys.D, KeyNameMapping.GetName(Keys.D) },

                // {E}キー
                { Keys.E, KeyNameMapping.GetName(Keys.E) },

                // {F}キー
                { Keys.F, KeyNameMapping.GetName(Keys.F) },

                // {G}キー
                { Keys.G, KeyNameMapping.GetName(Keys.G) },

                // {H}キー
                { Keys.H, KeyNameMapping.GetName(Keys.H) },

                // {I}キー
                { Keys.I, KeyNameMapping.GetName(Keys.I) },

                // {J}キー
                { Keys.J, KeyNameMapping.GetName(Keys.J) },

                // {K}キー
                { Keys.K, KeyNameMapping.GetName(Keys.K) },

                // {L}キー
                { Keys.L, KeyNameMapping.GetName(Keys.L) },

                // {M}キー
                { Keys.M, KeyNameMapping.GetName(Keys.M) },

                // {N}キー
                { Keys.N, KeyNameMapping.GetName(Keys.N) },

                // {O}キー
                { Keys.O, KeyNameMapping.GetName(Keys.O) },

                // {P}キー
                { Keys.P, KeyNameMapping.GetName(Keys.P) },

                // {Q}キー
                { Keys.Q, KeyNameMapping.GetName(Keys.Q) },

                // {R}キー
                { Keys.R, KeyNameMapping.GetName(Keys.R) },

                // {S}キー
                { Keys.S, KeyNameMapping.GetName(Keys.S) },

                // {T}キー
                { Keys.T, KeyNameMapping.GetName(Keys.T) },

                // {U}キー
                { Keys.U, KeyNameMapping.GetName(Keys.U) },

                // {V}キー
                { Keys.V, KeyNameMapping.GetName(Keys.V) },

                // {W}キー
                { Keys.W, KeyNameMapping.GetName(Keys.W) },

                // {X}キー
                { Keys.X, KeyNameMapping.GetName(Keys.X) },

                // {Y}キー
                { Keys.Y, KeyNameMapping.GetName(Keys.Y) },

                // {Z}キー
                { Keys.Z, KeyNameMapping.GetName(Keys.Z) },

                // テンキーの{0}キー
                { Keys.NumPad0, KeyNameMapping.GetName(Keys.NumPad0) },

                // テンキーの{1}キー
                { Keys.NumPad1, KeyNameMapping.GetName(Keys.NumPad1) },

                // テンキーの{2}キー
                { Keys.NumPad2, KeyNameMapping.GetName(Keys.NumPad2) },

                // テンキーの{3}キー
                { Keys.NumPad3, KeyNameMapping.GetName(Keys.NumPad3) },

                // テンキーの{4}キー
                { Keys.NumPad4, KeyNameMapping.GetName(Keys.NumPad4) },

                // テンキーの{5}キー
                { Keys.NumPad5, KeyNameMapping.GetName(Keys.NumPad5) },

                // テンキーの{6}キー
                { Keys.NumPad6, KeyNameMapping.GetName(Keys.NumPad6) },

                // テンキーの{7}キー
                { Keys.NumPad7, KeyNameMapping.GetName(Keys.NumPad7) },

                // テンキーの{8}キー
                { Keys.NumPad8, KeyNameMapping.GetName(Keys.NumPad8) },

                // テンキーの{9}キー
                { Keys.NumPad9, KeyNameMapping.GetName(Keys.NumPad9) },

                // テンキーの{*}キー
                { Keys.Multiply, KeyNameMapping.GetName(Keys.Multiply) },

                // テンキーの{+}キー
                { Keys.Add, KeyNameMapping.GetName(Keys.Add) },

                // テンキーの{-}キー
                { Keys.Subtract, KeyNameMapping.GetName(Keys.Subtract) },

                // テンキーの{.}キー
                { Keys.Decimal, KeyNameMapping.GetName(Keys.Decimal) },

                // テンキーの{/}キー
                { Keys.Divide, KeyNameMapping.GetName(Keys.Divide) },

                // 米国標準キーボードの{;}キー（日本語キーボードの場合{: *}キー）
                { Keys.OemSemicolon, KeyNameMapping.GetName(Keys.OemSemicolon) },

                // 国または地域別キーボードの{+}キー（日本語キーボードの場合{; +}キー）
                { Keys.Oemplus, KeyNameMapping.GetName(Keys.Oemplus) },

                // 国または地域別キーボードの{,}キー（日本語キーボードの場合{, <}キー）
                { Keys.Oemcomma, KeyNameMapping.GetName(Keys.Oemcomma) },

                // 国または地域別キーボードの{-}キー（日本語キーボードの場合{- =}キー）
                { Keys.OemMinus, KeyNameMapping.GetName(Keys.OemMinus) },

                // 国または地域別キーボードの{.}キー（日本語キーボードの場合{. >}キー）
                { Keys.OemPeriod, KeyNameMapping.GetName(Keys.OemPeriod) },

                // 米国標準キーボードの{?}キー（日本語キーボードの場合{/ ?}キー）
                { Keys.OemQuestion, KeyNameMapping.GetName(Keys.OemQuestion) },

                // 米国標準キーボードの{~}キー（日本語キーボードの場合{@ `}キー）
                { Keys.Oemtilde, KeyNameMapping.GetName(Keys.Oemtilde) },

                // 米国標準キーボードの{[}キー（日本語キーボードの場合{[ {}キー）
                { Keys.OemOpenBrackets, KeyNameMapping.GetName(Keys.OemOpenBrackets) },

                // 米国標準キーボードの{|}キー（日本語キーボードの場合{\ |}キー）
                { Keys.OemPipe, KeyNameMapping.GetName(Keys.OemPipe) },

                // 米国標準キーボード{]}キー（日本語キーボードの場合{] }}キー）
                { Keys.OemCloseBrackets, KeyNameMapping.GetName(Keys.OemCloseBrackets) },

                // 米国標準キーボードの{' "}キー（日本語キーボードの場合{^ ~}キー）
                { Keys.OemQuotes, KeyNameMapping.GetName(Keys.OemQuotes) },

                // RT102キーボードの{^ \}キー（日本語キーボードの場合{＼ _}キー）
                { Keys.OemBackslash, KeyNameMapping.GetName(Keys.OemBackslash) },

                // Shiftキー
                { Keys.ShiftKey, KeyNameMapping.GetName(Keys.ShiftKey) },

                // Ctrlキー
                { Keys.ControlKey, KeyNameMapping.GetName(Keys.ControlKey) },

                // Altキー
                { Keys.Menu, KeyNameMapping.GetName(Keys.Menu) },

                // Windowsロゴキー
                { Keys.LWin, KeyNameMapping.GetName(Keys.LWin) },
            });

        #endregion

        #region 操作キー用のコンボボックスデータ

        /// <summary>
        /// 操作キー用のコンボボックスデータ
        /// </summary>
        private IReadOnlyDictionary<Keys, string> _comboBoxDataOperateKey = new ReadOnlyDictionary<Keys, string>(
            new Dictionary<Keys, string>()
            {
                // なし
                { Keys.None, KeyNameMapping.GetName(Keys.None) },

                // {Enter}キー
                { Keys.Enter, KeyNameMapping.GetName(Keys.Enter) },

                // {Space}キー
                { Keys.Space, KeyNameMapping.GetName(Keys.Space) },

                // {BackSpace}キー
                { Keys.Back, KeyNameMapping.GetName(Keys.Back) },

                // {Tab}キー
                { Keys.Tab, KeyNameMapping.GetName(Keys.Tab) },

                // {Insert}キー
                { Keys.Insert, KeyNameMapping.GetName(Keys.Insert) },

                // {Delete}キー
                { Keys.Delete, KeyNameMapping.GetName(Keys.Delete) },

                // {Home}キー
                { Keys.Home, KeyNameMapping.GetName(Keys.Home) },

                // {End}キー
                { Keys.End, KeyNameMapping.GetName(Keys.End) },

                // {PageUp}キー
                { Keys.PageUp, KeyNameMapping.GetName(Keys.PageUp) },

                // {PageDown}キー
                { Keys.PageDown, KeyNameMapping.GetName(Keys.PageDown) },

                // {←}キー
                { Keys.Left, KeyNameMapping.GetName(Keys.Left) },

                // {↑}キー
                { Keys.Up, KeyNameMapping.GetName(Keys.Up) },

                // {→}キー
                { Keys.Right, KeyNameMapping.GetName(Keys.Right) },

                // {↓}キー
                { Keys.Down, KeyNameMapping.GetName(Keys.Down) },

                // {PrintScreen}キー
                { Keys.PrintScreen, KeyNameMapping.GetName(Keys.PrintScreen) },

                // {アプリケーション}キー
                { Keys.Apps, KeyNameMapping.GetName(Keys.Apps) },

                // {Esc}キー
                { Keys.Escape, KeyNameMapping.GetName(Keys.Escape) },

                // {Pause}キー
                { Keys.Pause, KeyNameMapping.GetName(Keys.Pause) },

                // {F1}キー
                { Keys.F1, KeyNameMapping.GetName(Keys.F1) },

                // {F2}キー
                { Keys.F2, KeyNameMapping.GetName(Keys.F2) },

                // {F3}キー
                { Keys.F3, KeyNameMapping.GetName(Keys.F3) },

                // {F4}キー
                { Keys.F4, KeyNameMapping.GetName(Keys.F4) },

                // {F5}キー
                { Keys.F5, KeyNameMapping.GetName(Keys.F5) },

                // {F6}キー
                { Keys.F6, KeyNameMapping.GetName(Keys.F6) },

                // {F7}キー
                { Keys.F7, KeyNameMapping.GetName(Keys.F7) },

                // {F8}キー
                { Keys.F8, KeyNameMapping.GetName(Keys.F8) },

                // {F9}キー
                { Keys.F9, KeyNameMapping.GetName(Keys.F9) },

                // {F10}キー
                { Keys.F10, KeyNameMapping.GetName(Keys.F10) },

                // {F11}キー
                { Keys.F11, KeyNameMapping.GetName(Keys.F11) },

                // {F12}キー
                { Keys.F12, KeyNameMapping.GetName(Keys.F12) },

                // {F13}キー
                { Keys.F13, KeyNameMapping.GetName(Keys.F13) },

                // {F14}キー
                { Keys.F14, KeyNameMapping.GetName(Keys.F14) },

                // {F15}キー
                { Keys.F15, KeyNameMapping.GetName(Keys.F15) },

                // {F16}キー
                { Keys.F16, KeyNameMapping.GetName(Keys.F16) },

                // {F17}キー
                { Keys.F17, KeyNameMapping.GetName(Keys.F17) },

                // {F18}キー
                { Keys.F18, KeyNameMapping.GetName(Keys.F18) },

                // {F19}キー
                { Keys.F19, KeyNameMapping.GetName(Keys.F19) },

                // {F20}キー
                { Keys.F20, KeyNameMapping.GetName(Keys.F20) },

                // {F21}キー
                { Keys.F21, KeyNameMapping.GetName(Keys.F21) },

                // {F22}キー
                { Keys.F22, KeyNameMapping.GetName(Keys.F22) },

                // {F23}キー
                { Keys.F23, KeyNameMapping.GetName(Keys.F23) },

                // {F24}キー
                { Keys.F24, KeyNameMapping.GetName(Keys.F24) },
            });

        #endregion

        #region IMEモード等に関連するキーのコンボボックスデータ

        /// <summary>
        /// IMEモード等に関連するキーのコンボボックスデータ
        /// </summary>
        private IReadOnlyDictionary<Keys, string> _comboBoxDataImeKey = new ReadOnlyDictionary<Keys, string>(
            new Dictionary<Keys, string>()
            {
                // なし
                { Keys.None, KeyNameMapping.GetName(Keys.None) },

                // CapsLock（{Shift}+{CapsLock}キー）
                { Keys.CapsLock, KeyNameMapping.GetName(Keys.CapsLock) },

                // {NumLock}キー
                { Keys.NumLock, KeyNameMapping.GetName(Keys.NumLock) },

                // {ScrollLock}キー
                { Keys.Scroll, KeyNameMapping.GetName(Keys.Scroll) },

                // IME かなモード
                { Keys.KanaMode, KeyNameMapping.GetName(Keys.KanaMode) },

                // IME 漢字モード（{Alt}+{半角／全角}キー）
                { Keys.KanjiMode, KeyNameMapping.GetName(Keys.KanjiMode) },

                // {変換}キー（IME変換）
                { Keys.IMEConvert, KeyNameMapping.GetName(Keys.IMEConvert) },

                // {無変換}キー（IME無変換）
                { Keys.IMENonconvert, KeyNameMapping.GetName(Keys.IMENonconvert) },
            });

        #endregion

        #region 特殊キーのコンボボックスデータ

        /// <summary>
        /// 特殊キーのコンボボックスデータ
        /// </summary>
        private IReadOnlyDictionary<Keys, string> _comboBoxDataSpecialKey = new ReadOnlyDictionary<Keys, string>(
            new Dictionary<Keys, string>()
            {
                // なし
                { Keys.None, KeyNameMapping.GetName(Keys.None) },

                // ブラウザーの戻るキー
                { Keys.BrowserBack, KeyNameMapping.GetName(Keys.BrowserBack) },

                // ブラウザーの進むキー
                { Keys.BrowserForward, KeyNameMapping.GetName(Keys.BrowserForward) },

                // ブラウザーの更新のキー
                { Keys.BrowserRefresh, KeyNameMapping.GetName(Keys.BrowserRefresh) },

                // ブラウザーの中止キー
                { Keys.BrowserStop, KeyNameMapping.GetName(Keys.BrowserStop) },

                // ブラウザーのホームキー
                { Keys.BrowserHome, KeyNameMapping.GetName(Keys.BrowserHome) },

                // 音量ミュートキー
                { Keys.VolumeMute, KeyNameMapping.GetName(Keys.VolumeMute) },

                // 音量 - キー
                { Keys.VolumeDown, KeyNameMapping.GetName(Keys.VolumeDown) },

                // 音量 + キー
                { Keys.VolumeUp, KeyNameMapping.GetName(Keys.VolumeUp) },

                // 次のトラックキー
                { Keys.MediaNextTrack, KeyNameMapping.GetName(Keys.MediaNextTrack) },

                // 前のトラックキー（曲の先頭へ戻る又は前のトラックへ行く）
                { Keys.MediaPreviousTrack, KeyNameMapping.GetName(Keys.MediaPreviousTrack) },

                // メディア再生/一時停止キー
                { Keys.MediaPlayPause, KeyNameMapping.GetName(Keys.MediaPlayPause) },
            });

        #endregion

        #endregion

        #region クラス変数

        /// <summary>
        /// マウスのコンボボックス領域の表示・非表示の設定
        /// </summary>
        private bool _visibleMouse = true;

        /// <summary>
        /// 入力キーのコンボボックス領域の表示・非表示の設定
        /// </summary>
        private bool _visibleInputKey = true;

        /// <summary>
        /// 操作キーのコンボボックス領域の表示・非表示の設定
        /// </summary>
        private bool _visibleOperateKey = true;

        /// <summary>
        /// IMEキーのコンボボックス領域の表示・非表示の設定
        /// </summary>
        private bool _visibleImeKey = true;

        /// <summary>
        /// 特殊キーのコンボボックス領域の表示・非表示の設定
        /// </summary>
        private bool _visibleSpecialKey = true;

        /// <summary>
        /// コントロールが、その内容に合わせて自動的に幅を変更するかどうかの設定
        /// </summary>
        private bool _autoSizeWidth;

        /// <summary>
        /// コントロールが、その内容に合わせて自動的に高さを変更するかどうかの設定
        /// </summary>
        private bool _autoSizeHeight;

        /// <summary>
        /// 自動高さ調整時におけるコンボボックス領域の表示行数の設定
        /// 0を指定した場合は全てを表示するサイズに調整する
        /// </summary>
        private int _autoSizeHeightDisplayLines = 1;

        /// <summary>
        /// コンボボックスエリアのコンボボックスのリスト
        /// </summary>
        private IList<ComboBoxControl> _comboBoxList = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各コントロールの初期化を行う
        /// </summary>
        public KeyInputSetting()
        {
            InitializeComponent();

            // 入力テキストのデフォルト値を取得し保持する
            DefaultInputText = TxtInput.Text;

            // コンボボックスにデータを設定するローカル関数
            // 設定データに合わせてサイズの調整も行う
            // （サイズ調整を行うためコンスラクタのみでしか使用しない、そのためローカル関数で定義）
            // 引数1：comboBox     設定対象のコンボボックス
            // 引数2：parent       設定対象のコンボボックスの親のパネル
            // 引数3：dataList     設定するデータ
            void SetComboBoxData(ComboBox comboBox, Panel parent, IReadOnlyDictionary<Keys, string> dataList)
            {
                // コンボボックスにデータを設定する
                // 同時に設定するデータの表示サイズを取得し、その最大値をコンボボックスの幅に設定する
                float width = 0;
                using (Graphics graphics = comboBox.CreateGraphics())
                {
                    foreach (KeyValuePair<Keys, string> data in dataList)
                    {
                        // コンボボックスにデータを追加
                        comboBox.Items.Add(data);

                        // 設定したデータの表示サイズを取得
                        SizeF valueSize = graphics.MeasureString(data.Value, comboBox.Font, MaxComboBoxWidth);

                        // 表示サイズが以前の値より大きい場合は値を更新する
                        width = width < valueSize.Width ? valueSize.Width : width;
                    }
                }

                // コンボボックスの表示メンバーを設定
                KeyValuePair<Keys, string> keyValue = default(KeyValuePair<Keys, string>);
                comboBox.DisplayMember = nameof(keyValue.Value);

                // 設定したデータの最大サイズに[▼]の表示領域分幅を追加したものを
                // コンボボックスのサイズに設定する
                comboBox.Width = (int)width + 25;

                // 親パネルのPaddingの値、コンボボックスのMarginの値を取得する
                int parentPadding = parent.Padding.Left + parent.Padding.Right;
                int comboBoxMargin = comboBox.Margin.Left + comboBox.Margin.Right;

                // 親パネルの幅を設定する
                parent.Width = comboBox.Width + comboBoxMargin + parentPadding;

                // 最初のデータを選択する
                if (dataList.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                }
            }

            // 各コンボボックスにデータを設定する
            // また設定データに合わせてサイズの調整も行う
            SetComboBoxData(CmbBoxMouse, PlMouse, _comboBoxDataMouse);
            SetComboBoxData(CmbBoxInputKey, PlInputKey, _comboBoxDataInputKey);
            SetComboBoxData(CmbBoxOperateKey, PlOperateKey, _comboBoxDataOperateKey);
            SetComboBoxData(CmbBoxImeKey, PlImeKey, _comboBoxDataImeKey);
            SetComboBoxData(CmbBoxSpecialKey, PlSpecialKey, _comboBoxDataSpecialKey);
        }

        /// <summary>
        /// コンストラクタ
        /// 各コントロールの初期化を行う
        /// </summary>
        /// <param name="inputKey">初期設定されるキーの設定</param>
        public KeyInputSetting(InputKey inputKey)
            : this()
        {
            // 各コントロールの初期化を行う
            Initialize(inputKey);
        }

        #endregion

        #region イベント定義（ISpeechRecognitionSettingControlの実装）

        /// <summary>
        /// 設定データを変更した際に発生させるイベント
        /// </summary>
        [LocalizableCategory("PropertyChangeCategory", typeof(KeyInputSettingMessage))]
        [LocalizableDescription("SettingDataChangedDescription", typeof(KeyInputSettingMessage))]
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
                // このクラスで設定された入力キー情報を文字列に変換して返却する
                InputKey inputKey = SettingInputKey;
                return inputKey?.ConvertToString(inputKey);
            }
        }

        #endregion

        /// <summary>
        /// マウスのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("EnabledMouseDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool EnabledMouse
        {
            get => PlMouse.Enabled;
            set
            {
                PlMouse.Enabled = value;
                PlMouse.Visible = value ? _visibleMouse : value;
                SetPlComboBoxVisible();
            }
        }

        /// <summary>
        /// 入力キーのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("EnabledInputKeyDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool EnabledInputKey
        {
            get => PlInputKey.Enabled;
            set
            {
                PlInputKey.Enabled = value;
                PlInputKey.Visible = value ? _visibleInputKey : value;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// 操作キーのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("EnabledOperateKeyDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool EnabledOperateKey
        {
            get => PlOperateKey.Enabled;
            set
            {
                PlOperateKey.Enabled = value;
                PlOperateKey.Visible = value ? _visibleOperateKey : value;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// IMEキーのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("EnabledImeKeyDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool EnabledImeKey
        {
            get => PlImeKey.Enabled;
            set
            {
                PlImeKey.Enabled = value;
                PlImeKey.Visible = value ? _visibleImeKey : value;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// 特殊キーのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("EnabledSpecialKeyDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool EnabledSpecialKey
        {
            get => PlSpecialKey.Enabled;
            set
            {
                PlSpecialKey.Enabled = value;
                PlSpecialKey.Visible = value ? _visibleSpecialKey : value;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// マウスのコンボボックスの表示・非表示を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("VisibleMouseDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool VisibleMouse
        {
            get => _visibleMouse;
            set
            {
                _visibleMouse = value;
                PlMouse.Visible = PlMouse.Enabled ? value : false;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// 入力キーのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("VisibleInputKeyDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool VisibleInputKey
        {
            get => _visibleInputKey;
            set
            {
                _visibleInputKey = value;
                PlInputKey.Visible = PlInputKey.Enabled ? value : false;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// 操作キーのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("VisibleOperateKeyDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool VisibleOperateKey
        {
            get => _visibleOperateKey;
            set
            {
                _visibleOperateKey = value;
                PlOperateKey.Visible = PlOperateKey.Enabled ? value : false;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// IMEキーのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("VisibleImeKeyDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool VisibleImeKey
        {
            get => _visibleImeKey;
            set
            {
                _visibleImeKey = value;
                PlImeKey.Visible = PlImeKey.Enabled ? value : false;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// 特殊キーのコンボボックスの有効・無効を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("VisibleSpecialKeyDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(true)]
        public bool VisibleSpecialKey
        {
            get => _visibleSpecialKey;
            set
            {
                _visibleSpecialKey = value;
                PlSpecialKey.Visible = PlSpecialKey.Enabled ? value : false;

                // コンボボックス領域の表示設定
                SetPlComboBoxVisible();

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// コントロールが、その内容に合わせて自動的に幅を変更するかどうかを取得・設定する
        /// AutoSizeプロパティがFalseの場合のみ指定が可能
        /// </summary>
        [LocalizableCategory("LayoutCategory", typeof(KeyInputSettingMessage))]
        [LocalizableDescription("AutoSizeWidthDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(false)]
        public bool AutoSizeWidth
        {
            // AutoSizeプロパティがTrueの場合は常にFalseを返却
            get => AutoSize ? false : _autoSizeWidth;
            set
            {
                // AutoSizeプロパティがTrueの場合は常にFalseを設定
                _autoSizeWidth = AutoSize ? false : value;

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// コントロールが、その内容に合わせて自動的に高さを変更するかどうかを取得・設定する
        /// AutoSizeプロパティがFalseの場合のみ指定が可能
        /// </summary>
        [LocalizableCategory("LayoutCategory", typeof(KeyInputSettingMessage))]
        [LocalizableDescription("AutoSizeHeightDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(false)]
        public bool AutoSizeHeight
        {
            // AutoSizeプロパティがTrueの場合は常にFalseを返却
            get => AutoSize ? false : _autoSizeHeight;
            set
            {
                // AutoSizeプロパティがTrueの場合は常にFalseを設定
                _autoSizeHeight = AutoSize ? false : value;

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// 自動高さ調整時におけるコンボボックス領域の表示行数を取得・設定する
        /// 0を指定した場合は全てを表示するサイズに調整する
        /// </summary>
        [LocalizableCategory("LayoutCategory", typeof(KeyInputSettingMessage))]
        [LocalizableDescription("AutoSizeHeightDisplayLinesDescription", typeof(KeyInputSettingMessage))]
        [DefaultValue(1)]
        public int AutoSizeHeightDisplayLines
        {
            get => _autoSizeHeightDisplayLines;
            set
            {
                _autoSizeHeightDisplayLines = value;

                // 自動幅調を行う
                SetAutoSize(AutoSizeMode, _autoSizeWidth, _autoSizeHeight);
            }
        }

        /// <summary>
        /// 現在の入力キーの情報を取得・設定する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public InputKey CurrentInputKey { get; set; }

        /// <summary>
        /// このコントロールで設定した入力キーの情報（未設定、キャンセルの場合はNULL）を取得する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public InputKey SettingInputKey
        {
            get => KeyData?.DeepCopy();
        }

        #endregion

        #region プライベートプロパティ

        /// <summary>
        /// コンボボックスエリアのコンボボックスのリストを取得する
        /// </summary>
        private IList<ComboBoxControl> ComboBoxList
        {
            get
            {
                if (_comboBoxList == null)
                {
                    _comboBoxList = new List<ComboBoxControl>
                    {
                        new ComboBoxControl(PlMouse, CmbBoxMouse),
                        new ComboBoxControl(PlInputKey, CmbBoxInputKey),
                        new ComboBoxControl(PlOperateKey, CmbBoxOperateKey),
                        new ComboBoxControl(PlImeKey, CmbBoxImeKey),
                        new ComboBoxControl(PlSpecialKey, CmbBoxSpecialKey)
                    };
                }

                return _comboBoxList;
            }
        }

        /// <summary>
        /// 入力テキストボックスのデフォルト値を取得する
        /// 値はコンストラクタでデザイナで指定された文言を設定する
        /// </summary>
        private string DefaultInputText { get; }

        /// <summary>
        /// このコントロールで現在設定しているキーの情報を取得・設定する
        /// </summary>
        private InputKey KeyData { get; set; } = new InputKey();

        #endregion

        #region 公開メソッド（ISpeechRecognitionSettingControlの実装）

        /// <summary>
        /// 設定データの概要を示すテキストを取得する
        /// </summary>
        /// <param name="settingData">設定データ</param>
        /// <returns>設定データの概要を示すテキスト</returns>
        public string GetSummaryText(string settingData)
        {
            // 引数の設定データから入力キー情報を生成する
            InputKey inputKey = new InputKey();
            inputKey = inputKey.ConvertFromString(settingData ?? string.Empty);

            // 入力キー情報を概要の文字列に変換する
            StringBuilder convertValue = new StringBuilder();

            // Shiftキー
            if (inputKey.Shift)
            {
                convertValue.Append(nameof(inputKey.Shift)).Append("|");
            }

            // Ctrlキー
            if (inputKey.Ctrl)
            {
                convertValue.Append(nameof(inputKey.Ctrl)).Append("|");
            }

            // Altキー
            if (inputKey.Alt)
            {
                convertValue.Append(nameof(inputKey.Alt)).Append("|");
            }

            // Windowsロゴキー
            if (inputKey.Win)
            {
                convertValue.Append(nameof(inputKey.Win)).Append("|");
            }

            // キーコード
            string keyCode = ((int)inputKey.KeyCode).ToString("X2", CultureInfo.InvariantCulture);
            convertValue.Append("0x").Append(keyCode);
            convertValue.Append(":").Append(KeyNameMapping.GetName(inputKey.KeyCode));

            // 押しっぱにするかのフラグ
            if (inputKey.IsKeepPressing)
            {
                convertValue.Append(" [");
                convertValue.Append(KeyInputSettingMessage.SummaryTextKeepPressing);
                convertValue.Append("]");
            }

            // 概要文字列を生成し返却する
            return string.Format(
                CultureInfo.InvariantCulture,
                KeyInputSettingMessage.SummaryTextFormat,
                convertValue.ToString());
        }

        /// <summary>
        /// 操作設定のコントロールを引数の設定データ（<paramref name="settingData"/>）で初期化したコントロールを取得する
        /// </summary>
        /// <param name="settingData">設定データ</param>
        /// <returns> 操作設定のコントロールを引数の設定データ（<paramref name="settingData"/>）で初期化したコントロール</returns>
        public Control GetInitializeControl(string settingData)
        {
            // 引数の設定データから入力キー情報を生成する
            InputKey inputKey = new InputKey();
            inputKey = inputKey.ConvertFromString(settingData ?? string.Empty);

            // 入力キー情報で初期化を行い、このコントロールを返却する
            Initialize(inputKey);
            return this;
        }

        #endregion

        #region イベントで呼び出されるメソッド

        /// <summary>
        /// このコントロールのロードイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void KeyInputSetting_Load(object sender, EventArgs e)
        {
            // プロパティに応じたサイズ調整を行う
            SetAutoSize(AutoSizeMode, AutoSizeWidth, AutoSizeHeight);
        }

        /// <summary>
        /// 入力テキストボックスでのキー押下のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">キー押下のイベントデータ</param>
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            // イベントデータからキー情報を生成
            InputKey inputKey = new InputKey(e);

            // コンボボックスの選択
            SelectComboBox(inputKey);

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        /// <summary>
        /// チェックボックスのチェック変更イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // 現在のキーの設定が何も押下しない設定であり、
            // その状態からチェックボックスがチェックされた場合か判定
            if (KeyData.KeyCode == Keys.None
                && !KeyData.Shift
                && !KeyData.Ctrl
                && !KeyData.Alt
                && !KeyData.Win
                && sender is CheckBox checkBox
                && checkBox.Checked)
            {
                // チェックされたチェックボックスのキーを押下キーに設定する
                if (checkBox.Equals(ChkShift))
                {
                    // Shiftキー
                    KeyData.KeyCode = Keys.ShiftKey;
                }
                else if (checkBox.Equals(ChkCtrl))
                {
                    // Ctrlキー
                    KeyData.KeyCode = Keys.ControlKey;
                }
                else if (checkBox.Equals(ChkAlt))
                {
                    // Altキー
                    KeyData.KeyCode = Keys.Menu;
                }
                else if (checkBox.Equals(ChkWin))
                {
                    // Winキー
                    KeyData.KeyCode = Keys.LWin;
                }
            }

            // チェックボックスの設定をキー情報に設定する
            KeyData.IsKeepPressing = ChkKeepPressing.Checked;
            KeyData.Shift = ChkShift.Checked;
            KeyData.Ctrl = ChkCtrl.Checked;
            KeyData.Alt = ChkAlt.Checked;
            KeyData.Win = ChkWin.Checked;

            // 選択した値をテキストボックスに表示する
            SetInputText(KeyData);

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        /// <summary>
        /// コンボボックスの選択の変更イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void KeyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // センダーオブジェクトがConboBoxでない場合、処理を行わない
            if (!(sender is ComboBox comboBox))
            {
                return;
            }

            // 全てのコンボボックスにおいて0番目が選択状態か判定
            bool isAllZero = true;
            foreach (ComboBoxControl comboBoxControl in ComboBoxList)
            {
                if (comboBoxControl.ComboBox.Items.Count > 0
                    && comboBoxControl.ComboBox.SelectedIndex != 0)
                {
                    isAllZero = false;
                    break;
                }
            }

            // 操作されたコンボボックスにおいて0番目が選択された場合は処理を行わない
            // ただし、全てのコンボボックスにおいて0番目が選択状態となる場合はテキストボックスの表示を更新する
            if (comboBox.Items.Count == 0 || comboBox.SelectedIndex == 0)
            {
                if (isAllZero)
                {
                    // キー情報のキーコードをなしに設定する
                    KeyData.KeyCode = Keys.None;

                    // テキストボックスの表示を更新する
                    SetInputText(KeyData);

                    // 設定データ変更イベントを発生させる
                    SettingDataChanged?.Invoke(this, null);
                }

                // 処理を抜ける
                return;
            }

            // 上記以外の場合
            // 選択された以外のコンボボックスは初期値の選択に戻す
            foreach (ComboBoxControl comboBoxControl in ComboBoxList)
            {
                if (!comboBoxControl.ComboBox.Equals(comboBox)
                    && comboBoxControl.ComboBox.Items.Count > 0)
                {
                    comboBoxControl.ComboBox.SelectedIndex = 0;
                }
            }

            // 選択された値をキー情報に保持する
            if (comboBox.SelectedItem is KeyValuePair<Keys, string> key)
            {
                KeyData.KeyCode = key.Key;
            }

            // 選択した値をテキストボックスに表示する
            SetInputText(KeyData);

            // 設定データ変更イベントを発生させる
            SettingDataChanged?.Invoke(this, null);
        }

        #endregion

        #region プライベートメソッド

        #region 表示制御

        /// <summary>
        /// コンボボックス領域の表示設定を行う
        /// 表示するコントロールが存在しない場合は領域毎非表示にする
        /// </summary>
        private void SetPlComboBoxVisible()
        {
            // 各コンボボックスにおいて表示されるものが存在するか判定
            bool isVisible = false;
            foreach (ComboBoxControl comboBoxControl in ComboBoxList)
            {
                if (comboBoxControl.ParentPanel.Visible)
                {
                    isVisible = true;
                    break;
                }
            }

            // 表示されるコンボボックスが存在する場合、コンボボックス領域も表示する
            PlComboBox.Visible = isVisible;
            PlBar.Visible = isVisible;
        }

        #endregion

        #region サイズ調整

        /// <summary>
        /// 各子コントロールの表示非表示に応じて、コントロールの全体のサイズを設定する
        /// AutoSizeMode プロパティに応じてサイズの調整を行う
        /// </summary>
        /// <param name="autoSizeMode">
        /// 自動サイズ調整のモードを指定
        /// GrowOnly：拡大のみのサイズ調整を行う（縮小はしない）
        /// GrowAndShrink：拡大縮小のサイズ調整を行う
        /// </param>
        /// <param name="isAutoWidth">
        /// 幅の自動サイズ調整を行うかどうか
        /// </param>
        /// <param name="isAutoHeight">
        /// 高さの自動サイズ調整を行うかどうか
        /// </param>
        private void SetAutoSize(AutoSizeMode autoSizeMode, bool isAutoWidth, bool isAutoHeight)
        {
            if (isAutoWidth)
            {
                SetAutoWidth(autoSizeMode);
            }

            if (isAutoHeight)
            {
                SetAutoHeight(autoSizeMode);
            }
        }

        /// <summary>
        /// 各子コントロールの表示非表示に応じて、コントロールの全体の幅を設定する
        /// AutoSizeMode プロパティに応じてサイズの調整を行う
        /// （コンボボックス領域は一行で表示されるサイズに調整する）
        /// </summary>
        /// <param name="autoSizeMode">
        /// 自動サイズ調整のモードを指定
        /// GrowOnly：拡大のみのサイズ調整を行う（縮小はしない）
        /// GrowAndShrink：拡大縮小のサイズ調整を行う
        /// </param>
        private void SetAutoWidth(AutoSizeMode autoSizeMode)
        {
            // 幅の計算
            int width = 0;
            foreach (ComboBoxControl comboBoxControl in ComboBoxList)
            {
                // 親パネルを取得
                Panel panel = comboBoxControl.ParentPanel;

                // 非表示の場合は幅を0として返却
                if (!panel.Visible)
                {
                    continue;
                }

                // パネルの幅と左右のMarginの合計を返却
                width += panel.Margin.Left + panel.Width + panel.Margin.Right;
            }

            // 最小幅より小さくなる場合は最小幅を採用する
            width = width < MinWidthForAutoSize ? MinWidthForAutoSize : width;

            // 幅の設定
            // 幅の設定
            if (width > Width)
            {
                // 拡大の場合
                // スクロールバーの分を加算した値に設定してから、その後本来の値に縮小する
                Width = width + _scrollBarSize.Width;
                Width = width;
            }
            else if (autoSizeMode == AutoSizeMode.GrowAndShrink)
            {
                // 縮小の場合はAutoSizeMode.GrowAndShrinkのみ処理を行う
                Width = width;
            }
        }

        /// <summary>
        /// 各子コントロールの表示非表示に応じて、コントロールの全体の高さを設定する
        /// AutoSizeMode プロパティに応じてサイズの調整を行う
        /// </summary>
        /// <param name="autoSizeMode">
        /// 自動サイズ調整のモードを指定
        /// GrowOnly：拡大のみのサイズ調整を行う（縮小はしない）
        /// GrowAndShrink：拡大縮小のサイズ調整を行う
        /// </param>
        private void SetAutoHeight(AutoSizeMode autoSizeMode)
        {
            // 高さの計算
            int height = 0;
            height += PlTop.Height;
            height += PlBar.Visible ? PlBar.Height : 0;

            // コンボボックス領域が表示されている場合、各行の高さを取得し表示行数分の高さを求める
            // 行毎の高さを設定するディクショナリ
            SortedDictionary<int, int> linesHeight = new SortedDictionary<int, int>();
            if (PlComboBox.Visible)
            {
                // 各コンボボックスに関するコントロールにおける行毎の高さの情報を取得する
                foreach (ComboBoxControl comboBoxControl in ComboBoxList)
                {
                    // 親パネルを取得
                    Panel panel = comboBoxControl.ParentPanel;

                    // 非表示のパネルは無視する
                    if (!panel.Visible)
                    {
                        continue;
                    }

                    // パネルコントロールのLocation_Yと高さを取得
                    int key = panel.Location.Y;
                    int value = panel.Margin.Top + panel.Height + panel.Margin.Bottom;

                    // 既に設定されているLocation_Yか判定
                    if (linesHeight.ContainsKey(key))
                    {
                        // 設定済みの場合、対象の高さが現在の設定値よりも大きい場合は値を更新する
                        if (linesHeight[key] < value)
                        {
                            linesHeight[key] = value;
                        }
                    }
                    else
                    {
                        // 未設定の場合、データを追加する
                        linesHeight.Add(key, value);
                    }
                }
            }

            // 自動設定する高さに、表示行数分の行毎の高さを加算する
            // プロパティ「表示行数」の値が 0 の場合は全ての行数分高さを加算する
            int tmpDisplayLines = AutoSizeHeightDisplayLines == 0 ? linesHeight.Count : AutoSizeHeightDisplayLines;
            int loopCount = linesHeight.Count < tmpDisplayLines ? linesHeight.Count : tmpDisplayLines;
            int tmpCount = 0;
            foreach (var data in linesHeight)
            {
                // ループカウントを超える場合は処理を抜ける
                tmpCount++;
                if (tmpCount > loopCount)
                {
                    break;
                }

                // 行の高さを加算する
                height += data.Value;
            }

            // 高さの設定
            if (height > Height)
            {
                // 拡大の場合
                // スクロールバーの分を加算した値に設定してから、その後本来の値に縮小する
                Height = height + _scrollBarSize.Height;
                Height = height;
            }
            else if (autoSizeMode == AutoSizeMode.GrowAndShrink)
            {
                // 縮小の場合はAutoSizeMode.GrowAndShrinkのみ処理を行う
                Height = height;
            }
        }

        #endregion

        /// <summary>
        /// 引数のキー設定（<paramref name="inputKey"/>）で初期化を行う
        /// </summary>
        /// <param name="inputKey">初期設定されるキーの設定</param>
        private void Initialize(InputKey inputKey)
        {
            // 引数のキーの設定情報をプロパティに設定
            CurrentInputKey = inputKey;
            KeyData = inputKey?.DeepCopy() ?? new InputKey();

            // 初期設定されるキーが存在する場合、チェックボックス、コンボボックスの初期選択の設定を行う
            if (inputKey != null)
            {
                ChkKeepPressing.Checked = inputKey.IsKeepPressing;
                ChkShift.Checked = inputKey.Shift;
                ChkCtrl.Checked = inputKey.Ctrl;
                ChkAlt.Checked = inputKey.Alt;
                ChkWin.Checked = inputKey.Win;
                SelectComboBox(inputKey);
            }
        }

        /// <summary>
        /// 引数の <paramref name="key"/> のキーコードに該当するコンボボックスのデータを選択する
        /// </summary>
        /// <param name="key">選択するキーの情報</param>
        private void SelectComboBox(InputKey key)
        {
            // NULLチェック
            if (key == null)
            {
                // NULLの場合、選択を行わないため処理を終了する
                return;
            }

            // 引数のキーコードが「なし」の場合
            if (key.KeyCode == Keys.None)
            {
                // 全てのコンボボックスにおいて「なし」を選択するようにし、処理を終了する
                foreach (ComboBoxControl comboBoxControl in ComboBoxList)
                {
                    if (comboBoxControl.ComboBox.Items.Count > 0)
                    {
                        comboBoxControl.ComboBox.SelectedIndex = 0;
                    }
                }

                // キー情報のキーコードをなしに設定する
                KeyData.KeyCode = Keys.None;

                // テキストボックスの表示を更新する
                SetInputText(KeyData);

                // 処理を終了する
                return;
            }

            // 全てのコンボボックスのアイテムに対してキーコードに該当するデータを検索する
            // 該当が一個でもでたら処理を終了する
            foreach (ComboBoxControl comboBoxControl in ComboBoxList)
            {
                // 無効の設定がされているコンボボックスは無視する
                if (!comboBoxControl.ParentPanel.Enabled)
                {
                    continue;
                }

                // コンボボックスのアイテム分ループ
                foreach (KeyValuePair<Keys, string> data in comboBoxControl.ComboBox.Items)
                {
                    // 該当するデータの場合
                    if (data.Key == key.KeyCode)
                    {
                        // 該当したデータを選択する
                        comboBoxControl.ComboBox.SelectedItem = data;

                        // 処理を終了する
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// テキストボックスにキー情報の値を設定する
        /// </summary>
        /// <param name="key">対象とするキー情報</param>
        private void SetInputText(InputKey key)
        {
            // キー情報がNULLの場合はデフォルト値を表示する
            if (key == null)
            {
                TxtInput.Text = DefaultInputText;
            }

            // キー情報を表示する
            TxtInput.Text = key.ToString();
        }

        #endregion

        #region 内部クラス

        /// <summary>
        /// コンボボックスエリアのキーのコンボボックスとその親のパネルを纏めて扱うクラス
        /// 内部処理で使用する
        /// </summary>
        private class ComboBoxControl
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="parentPanel">親のパネル</param>
            /// <param name="comboBox">コンボボックス</param>
            public ComboBoxControl(Panel parentPanel, ComboBox comboBox)
            {
                ParentPanel = parentPanel;
                ComboBox = comboBox;
            }

            /// <summary>
            /// 親のパネルを取得・設定
            /// </summary>
            public Panel ParentPanel { get; set; }

            /// <summary>
            /// コンボボックスを取得・設定
            /// </summary>
            public ComboBox ComboBox { get; set; }
        }

        #endregion
    }
}
