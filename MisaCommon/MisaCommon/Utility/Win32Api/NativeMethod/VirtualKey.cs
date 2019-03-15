namespace MisaCommon.Utility.Win32Api.NativeMethod
{
    using System;
    using System.Globalization;
    using WindowFormKeys = System.Windows.Forms.Keys;

    /// <summary>
    /// 仮想キーの定義
    /// </summary>
    internal static class VirtualKey
    {
        #region 仮想キーの定義

        /// <summary>
        /// 仮想キーの定義
        /// </summary>
        public enum Keys : short
        {
            /// <summary>押下キーなし</summary>
            VK_NONE = 0x00,

            /// <summary>マウスの左ボタン</summary>
            VK_LBUTTON = 0x01,

            /// <summary>マウスの右ボタン</summary>
            VK_RBUTTON = 0x02,

            /// <summary>【使用不可】{Break}キー（ブレイク処理のキー：{Ctrl}+{Pause} 又は {Ctrl}+{ScrollLock}）</summary>
            VK_CANCEL = 0x03,

            /// <summary>マウスの中央ボタン（3ボタンマウス）</summary>
            VK_MBUTTON = 0x04,

            /// <summary>マウスの第1拡張ボタン（5ボタンマウス）</summary>
            VK_XBUTTON1 = 0x05,

            /// <summary>マウスの第2拡張ボタン（5ボタンマウス）</summary>
            VK_XBUTTON2 = 0x06,

            /// <summary>{Backspace}キー</summary>
            VK_BACK = 0x08,

            /// <summary>{Tab}キー</summary>
            VK_TAB = 0x09,

            /// <summary>【使用不可】Clearキー（NumLockオフのテンキーの{5}キーも含む）</summary>
            VK_CLEAR = 0x0C,

            /// <summary>{Enter}キー（テンキーの{Enter}キーも含む）</summary>
            VK_RETURN = 0x0D,

            /// <summary>{Shift}キー</summary>
            VK_SHIFT = 0x10,

            /// <summary>{Ctrl}キー</summary>
            VK_CONTROL = 0x11,

            /// <summary>{Alt}キー</summary>
            VK_MENU = 0x12,

            /// <summary>{Pause}キー</summary>
            VK_PAUSE = 0x13,

            /// <summary>CapsLock（{Shift}+{英数 CapsLock}キー）</summary>
            VK_CAPITAL = 0x14,

            /// <summary>
            /// IME かな入力モードへの切り替え
            /// 既にかな入力の場合はローマ字入力モードに切り替わる
            /// （ハングルを使用している場合はハングル入力⇔ローマ字入力の切り替えとなる）
            /// </summary>
            VK_KANA = 0x15,

            /// <summary>【使用不可】IME Junjaモード</summary>
            VK_JUNJA = 0x17,

            /// <summary>【使用不可】IME Finalモード</summary>
            VK_FINAL = 0x18,

            /// <summary>IME 漢字モード（{Alt}+{半角／全角}キー）</summary>
            VK_KANJI = 0x19,

            /// <summary>{Esc}キー</summary>
            VK_ESCAPE = 0x1B,

            /// <summary>{変換}キー（IME変換）</summary>
            VK_CONVERT = 0x1C,

            /// <summary>{無変換}キー（IME無変換）</summary>
            VK_NONCONVERT = 0x1D,

            /// <summary>【使用不可】IME Accept</summary>
            VK_ACCEPT = 0x1E,

            /// <summary>【使用不可】IME モード変更</summary>
            VK_MODECHANGE = 0x1F,

            /// <summary>{Space}キー</summary>
            VK_SPACE = 0x20,

            /// <summary>{PageUp}キー（NumLockオフのテンキーの{9}キーも含む）</summary>
            VK_PRIOR = 0x21,

            /// <summary>{PageDown}キー（NumLockオフのテンキーの{3}キーも含む）</summary>
            VK_NEXT = 0x22,

            /// <summary>{End}キー（NumLockオフのテンキーの{1}キーも含む）</summary>
            VK_END = 0x23,

            /// <summary>{Home}キー（NumLockオフのテンキーの{7}キーも含む）</summary>
            VK_HOME = 0x24,

            /// <summary>{←}キー（NumLockオフのテンキーの{4}キーも含む）</summary>
            VK_LEFT = 0x25,

            /// <summary>{↑}キー（NumLockオフのテンキーの{8}キーも含む）</summary>
            VK_UP = 0x26,

            /// <summary>{→}キー（NumLockオフのテンキーの{6}キーも含む）</summary>
            VK_RIGHT = 0x27,

            /// <summary>{↓}キー（NumLockオフのテンキーの{2}キーも含む）</summary>
            VK_DOWN = 0x28,

            /// <summary>【使用不可】Select</summary>
            VK_SELECT = 0x29,

            /// <summary>【使用不可】Print</summary>
            VK_PRINT = 0x2A,

            /// <summary>【使用不可】Execute</summary>
            VK_EXECUTE = 0x2B,

            /// <summary>{PrintScreen}キー</summary>
            VK_SNAPSHOT = 0x2C,

            /// <summary>{Insert}キー（NumLockオフのテンキーの{0}キーも含む）</summary>
            VK_INSERT = 0x2D,

            /// <summary>{Delete}キー（NumLockオフのテンキーの{.}キーも含む）</summary>
            VK_DELETE = 0x2E,

            /// <summary>【使用不可】Help</summary>
            VK_HELP = 0x2F,

            /// <summary>{0}キー</summary>
            VK_0 = 0x30,

            /// <summary>{1}キー</summary>
            VK_1 = 0x31,

            /// <summary>{2}キー</summary>
            VK_2 = 0x32,

            /// <summary>{3}キー</summary>
            VK_3 = 0x33,

            /// <summary>{4}キー</summary>
            VK_4 = 0x34,

            /// <summary>{5}キー</summary>
            VK_5 = 0x35,

            /// <summary>{6}キー</summary>
            VK_6 = 0x36,

            /// <summary>{7}キー</summary>
            VK_7 = 0x37,

            /// <summary>{8}キー</summary>
            VK_8 = 0x38,

            /// <summary>{9}キー</summary>
            VK_9 = 0x39,

            /// <summary>{A}キー</summary>
            VK_A = 0x41,

            /// <summary>{B}キー</summary>
            VK_B = 0x42,

            /// <summary>{C}キー</summary>
            VK_C = 0x43,

            /// <summary>{D}キー</summary>
            VK_D = 0x44,

            /// <summary>{E}キー</summary>
            VK_E = 0x45,

            /// <summary>{F}キー</summary>
            VK_F = 0x46,

            /// <summary>{G}キー</summary>
            VK_G = 0x47,

            /// <summary>{H}キー</summary>
            VK_H = 0x48,

            /// <summary>{I}キー</summary>
            VK_I = 0x49,

            /// <summary>{J}キー</summary>
            VK_J = 0x4A,

            /// <summary>{K}キー</summary>
            VK_K = 0x4B,

            /// <summary>{L}キー</summary>
            VK_L = 0x4C,

            /// <summary>{M}キー</summary>
            VK_M = 0x4D,

            /// <summary>{N}キー</summary>
            VK_N = 0x4E,

            /// <summary>{O}キー</summary>
            VK_O = 0x4F,

            /// <summary>{P}キー</summary>
            VK_P = 0x50,

            /// <summary>{Q}キー</summary>
            VK_Q = 0x51,

            /// <summary>{R}キー</summary>
            VK_R = 0x52,

            /// <summary>{S}キー</summary>
            VK_S = 0x53,

            /// <summary>{T}キー</summary>
            VK_T = 0x54,

            /// <summary>{U}キー</summary>
            VK_U = 0x55,

            /// <summary>{V}キー</summary>
            VK_V = 0x56,

            /// <summary>{W}キー</summary>
            VK_W = 0x57,

            /// <summary>{X}キー</summary>
            VK_X = 0x58,

            /// <summary>{Y}キー</summary>
            VK_Y = 0x59,

            /// <summary>{Z}キー</summary>
            VK_Z = 0x5A,

            /// <summary>左の{Windowsロゴ}キー（Microsoft Natural Keyboard）</summary>
            VK_LWIN = 0x5B,

            /// <summary>右の{Windowsロゴ}キー（Microsoft Natural Keyboard）</summary>
            VK_RWIN = 0x5C,

            /// <summary>{アプリケーション}キー（右のWindowsキーとCtrlキーの間にあるメニューが開くキー）（Microsoft Natural Keyboard）</summary>
            VK_APPS = 0x5D,

            /// <summary>【使用不可】{Sleep}キー（コンピュータをスリープさせるキー）</summary>
            VK_SLEEP = 0x5F,

            /// <summary>NumLockオンのテンキーの{0}キー</summary>
            VK_NUMPAD0 = 0x60,

            /// <summary>NumLockオンのテンキーの{1}キー</summary>
            VK_NUMPAD1 = 0x61,

            /// <summary>NumLockオンのテンキーの{2}キー</summary>
            VK_NUMPAD2 = 0x62,

            /// <summary>NumLockオンのテンキーの{3}キー</summary>
            VK_NUMPAD3 = 0x63,

            /// <summary>NumLockオンのテンキーの{4}キー</summary>
            VK_NUMPAD4 = 0x64,

            /// <summary>NumLockオンのテンキーの{5}キー</summary>
            VK_NUMPAD5 = 0x65,

            /// <summary>NumLockオンのテンキーの{6}キー</summary>
            VK_NUMPAD6 = 0x66,

            /// <summary>NumLockオンのテンキーの{7}キー</summary>
            VK_NUMPAD7 = 0x67,

            /// <summary>NumLockオンのテンキーの{8}キー</summary>
            VK_NUMPAD8 = 0x68,

            /// <summary>NumLockオンのテンキーの{9}キー</summary>
            VK_NUMPAD9 = 0x69,

            /// <summary>テンキーの{*}キー</summary>
            VK_MULTIPLY = 0x6A,

            /// <summary>テンキーの{+}キー</summary>
            VK_ADD = 0x6B,

            /// <summary>【使用不可】区切り記号（カンマ{,}）</summary>
            VK_SEPARATOR = 0x6C,

            /// <summary>テンキーの{-}キー</summary>
            VK_SUBTRACT = 0x6D,

            /// <summary>テンキーの{.}キー</summary>
            VK_DECIMAL = 0x6E,

            /// <summary>テンキーの{/}キー</summary>
            VK_DIVIDE = 0x6F,

            /// <summary>{F1}キー</summary>
            VK_F1 = 0x70,

            /// <summary>{F2}キー</summary>
            VK_F2 = 0x71,

            /// <summary>{F3}キー</summary>
            VK_F3 = 0x72,

            /// <summary>{F4}キー</summary>
            VK_F4 = 0x73,

            /// <summary>{F5}キー</summary>
            VK_F5 = 0x74,

            /// <summary>{F6}キー</summary>
            VK_F6 = 0x75,

            /// <summary>{F7}キー</summary>
            VK_F7 = 0x76,

            /// <summary>{F8}キー</summary>
            VK_F8 = 0x77,

            /// <summary>{F9}キー</summary>
            VK_F9 = 0x78,

            /// <summary>{F10}キー</summary>
            VK_F10 = 0x79,

            /// <summary>{F11}キー</summary>
            VK_F11 = 0x7A,

            /// <summary>{F12}キー</summary>
            VK_F12 = 0x7B,

            /// <summary>{F13}キー</summary>
            VK_F13 = 0x7C,

            /// <summary>{F14}キー</summary>
            VK_F14 = 0x7D,

            /// <summary>{F15}キー</summary>
            VK_F15 = 0x7E,

            /// <summary>{F16}キー</summary>
            VK_F16 = 0x7F,

            /// <summary>{F17}キー</summary>
            VK_F17 = 0x80,

            /// <summary>{F18}キー</summary>
            VK_F18 = 0x81,

            /// <summary>{F19}キー</summary>
            VK_F19 = 0x82,

            /// <summary>{F20}キー</summary>
            VK_F20 = 0x83,

            /// <summary>{F21}キー</summary>
            VK_F21 = 0x84,

            /// <summary>{F22}キー</summary>
            VK_F22 = 0x85,

            /// <summary>{F23}キー</summary>
            VK_F23 = 0x86,

            /// <summary>{F24}キー</summary>
            VK_F24 = 0x87,

            /// <summary>{NumLock}キー</summary>
            VK_NUMLOCK = 0x90,

            /// <summary>{ScrollLock}キー</summary>
            VK_SCROLL = 0x91,

            /// <summary>左の{Shift}キー</summary>
            VK_LSHIFT = 0xA0,

            /// <summary>右の{Shift}キー</summary>
            VK_RSHIFT = 0xA1,

            /// <summary>左の{Ctrl}キー</summary>
            VK_LCONTROL = 0xA2,

            /// <summary>右の{Ctrl}キー</summary>
            VK_RCONTROL = 0xA3,

            /// <summary>左の{Alt}キー</summary>
            VK_LMENU = 0xA4,

            /// <summary>右の{Alt}キー</summary>
            VK_RMENU = 0xA5,

            /// <summary>ブラウザーの戻るキー</summary>
            VK_BROWSER_BACK = 0xA6,

            /// <summary>ブラウザーの進むキー</summary>
            VK_BROWSER_FORWARD = 0xA7,

            /// <summary>ブラウザーの更新のキー</summary>
            VK_BROWSER_REFRESH = 0xA8,

            /// <summary>ブラウザーの中止キー</summary>
            VK_BROWSER_STOP = 0xA9,

            /// <summary>【使用不可】ブラウザーの検索キー</summary>
            VK_BROWSER_SEARCH = 0xAA,

            /// <summary>【使用不可】ブラウザーのお気に入りキー</summary>
            VK_BROWSER_FAVORITES = 0xAB,

            /// <summary>ブラウザーのホームキー</summary>
            VK_BROWSER_HOME = 0xAC,

            /// <summary>音量ミュートキー</summary>
            VK_VOLUME_MUTE = 0xAD,

            /// <summary>音量 - キー</summary>
            VK_VOLUME_DOWN = 0xAE,

            /// <summary>音量 + キー</summary>
            VK_VOLUME_UP = 0xAF,

            /// <summary>次のトラックキー</summary>
            VK_MEDIA_NEXT_TRACK = 0xB0,

            /// <summary>前のトラックキー（曲の先頭へ戻る又は前のトラックへ行く）</summary>
            VK_MEDIA_PREV_TRACK = 0xB1,

            /// <summary>【使用不可】メディア停止キー</summary>
            VK_MEDIA_STOP = 0xB2,

            /// <summary>メディア再生/一時停止キー</summary>
            VK_MEDIA_PLAY_PAUSE = 0xB3,

            /// <summary>【使用不可】メール起動キー</summary>
            VK_LAUNCH_MAIL = 0xB4,

            /// <summary>【使用不可】メディア選択キー</summary>
            VK_LAUNCH_MEDIA_SELECT = 0xB5,

            /// <summary>【使用不可】カスタムホットキー1</summary>
            VK_LAUNCH_APP1 = 0xB6,

            /// <summary>【使用不可】カスタムホットキー2</summary>
            VK_LAUNCH_APP2 = 0xB7,

            /// <summary>米国標準キーボードの{;}キー（日本語キーボードの場合{: *}キー）</summary>
            VK_OEM_1 = 0xBA,

            /// <summary>国または地域別キーボードの{+}キー（日本語キーボードの場合{; +}キー）</summary>
            VK_OEM_PLUS = 0xBB,

            /// <summary>国または地域別キーボードの{,}キー（日本語キーボードの場合{, &lt;}キー）</summary>
            VK_OEM_COMMA = 0xBC,

            /// <summary>国または地域別キーボードの{-}キー（日本語キーボードの場合{- =}キー）</summary>
            VK_OEM_MINUS = 0xBD,

            /// <summary>国または地域別キーボードの{.}キー（日本語キーボードの場合{. &gt;}キー）</summary>
            VK_OEM_PERIOD = 0xBE,

            /// <summary>米国標準キーボードの{?}キー（日本語キーボードの場合{/ ?}キー）</summary>
            VK_OEM_2 = 0xBF,

            /// <summary>米国標準キーボードの{~}キー（日本語キーボードの場合{@ `}キー）</summary>
            VK_OEM_3 = 0xC0,

            /// <summary>米国標準キーボードの{[}キー（日本語キーボードの場合{[ {}キー）</summary>
            VK_OEM_4 = 0xDB,

            /// <summary>米国標準キーボードの{|}キー（日本語キーボードの場合{\ |}キー）</summary>
            VK_OEM_5 = 0xDC,

            /// <summary>米国標準キーボード{]}キー（日本語キーボードの場合{] }}キー）</summary>
            VK_OEM_6 = 0xDD,

            /// <summary>米国標準キーボードの{&apos; &quot;}キー（日本語キーボードの場合{^ ~}キー）</summary>
            VK_OEM_7 = 0xDE,

            /// <summary>【使用不可】OEM8</summary>
            VK_OEM_8 = 0xDF,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_AX = 0xE1,

            /// <summary>RT102キーボードの{^ \}キー（日本語キーボードの場合{＼ _}キー）</summary>
            VK_OEM_102 = 0xE2,

            /// <summary>【使用不可】OEM固有</summary>
            VK_ICO_HELP = 0xE3,

            /// <summary>【使用不可】OEM固有</summary>
            VK_ICO_00 = 0xE4,

            /// <summary>【使用不可】ProcessKey</summary>
            VK_PROCESSKEY = 0xE5,

            /// <summary>【使用不可】OEM固有</summary>
            VK_ICO_CLEAR = 0xE6,

            /// <summary>Unicode文字をキーストロークであるかのように渡すための32ビット仮想キー値の下位ワード</summary>
            VK_PACKET = 0xE7,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_RESET = 0xE9,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_JUMP = 0xEA,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_PA1 = 0xEB,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_PA2 = 0xEC,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_PA3 = 0xED,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_WSCTRL = 0xEE,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_CUSEL = 0xEF,

            /// <summary>【<see cref="System.Windows.Forms.Keys"/> に定義なし】{英数 CapsLock}キー</summary>
            VK_OEM_ATTN = 0xF0,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_FINISH = 0xF1,

            /// <summary>【<see cref="System.Windows.Forms.Keys"/> に定義なし】{カタカナ ひらがな}キー</summary>
            VK_OEM_COPY = 0xF2,

            /// <summary>【<see cref="System.Windows.Forms.Keys"/> に定義なし】{半角／全角}キー</summary>
            VK_OEM_AUTO = 0xF3,

            /// <summary>【使用不可】OEM固有</summary>
            VK_OEM_ENLW = 0xF4,

            /// <summary>【<see cref="System.Windows.Forms.Keys"/> に定義なし】ローマ字キー（{Alt}+{カタカナ ひらがな}キー）</summary>
            VK_OEM_BACKTAB = 0xF5,

            /// <summary>【使用不可】Attn</summary>
            VK_ATTN = 0xF6,

            /// <summary>【使用不可】Crsel</summary>
            VK_CRSEL = 0xF7,

            /// <summary>【使用不可】Exsel</summary>
            VK_EXSEL = 0xF8,

            /// <summary>【使用不可】Erase Eof</summary>
            VK_EREOF = 0xF9,

            /// <summary>【使用不可】Play</summary>
            VK_PLAY = 0xFA,

            /// <summary>【使用不可】Zoom</summary>
            VK_ZOOM = 0xFB,

            /// <summary>【使用不可】予約済定数</summary>
            VK_NONAME = 0xFC,

            /// <summary>【使用不可】PA1</summary>
            VK_PA1 = 0xFD,

            /// <summary>【使用不可】Clear</summary>
            VK_OEM_CLEAR = 0xFE,
        }

        #endregion

        #region 使用しないキーの定義

        /// <summary>
        /// 使用しないキーの定義
        /// </summary>
        private enum NotUsedKeys : int
        {
            /// <summary>
            /// {Break}キー（ブレイク処理のキー：{Ctrl}+{Pause} 又は {Ctrl}+{ScrollLock}）
            /// ●押下しても反応しないため対象外とする
            /// </summary>
            Cancel = WindowFormKeys.Cancel,

            /// <summary>
            /// 改行（LF：ラインフィード）
            /// ●仮想キーに対応なし、Unicodeで送信するしかないため対象外とする
            /// </summary>
            LineFeed = WindowFormKeys.LineFeed,

            /// <summary>
            /// Clear（NumLockオフのテンキーの{5}キーも含む）
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            Clear = WindowFormKeys.Clear,

            /// <summary>
            /// IME Junjaモード
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            JunjaMode = WindowFormKeys.JunjaMode,

            /// <summary>
            /// IME ファイナルモード
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            FinalMode = WindowFormKeys.FinalMode,

            /// <summary>
            /// IME Accept
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            IMEAccept = WindowFormKeys.IMEAccept,

            /// <summary>
            /// IME モード変更
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            IMEModeChange = WindowFormKeys.IMEModeChange,

            /// <summary>
            /// Select
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            Select = WindowFormKeys.Select,

            /// <summary>
            /// Print
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            Print = WindowFormKeys.Print,

            /// <summary>
            /// Execute
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            Execute = WindowFormKeys.Execute,

            /// <summary>
            /// Help
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            Help = WindowFormKeys.Help,

            /// <summary>
            /// {Sleep}キー（コンピュータをスリープさせるキー）
            /// ●使用しないため対象外とする
            /// </summary>
            Sleep = WindowFormKeys.Sleep,

            /// <summary>
            /// 区切り記号（カンマ{,}）
            /// ●通常のキーボード上に存在しないため使用しないせず対象外とする
            /// </summary>
            Separator = WindowFormKeys.Separator,

            /// <summary>
            /// ブラウザーの検索キー
            /// ●検索操作は検索文字の入力があるため当機能では対象外とする
            /// </summary>
            BrowserSearch = WindowFormKeys.BrowserSearch,

            /// <summary>
            /// ブラウザーのお気に入りキー
            /// ●お気に入りはブラウザ依存が大きいため対象外とする
            /// </summary>
            BrowserFavorites = WindowFormKeys.BrowserFavorites,

            /// <summary>
            /// メディア停止キー
            /// ●昨今のアプリケーションは停止ボタン（■ボタン）が存在しないため対象外とする
            /// 　（一時停止（||）ボタンしかないものが多かった）
            /// </summary>
            MediaStop = WindowFormKeys.MediaStop,

            /// <summary>
            /// メール起動キー
            /// ●昨今はブラウザメールがおおくメールソフトが起動しても困るだけであるため対象外とする
            /// </summary>
            LaunchMail = WindowFormKeys.LaunchMail,

            /// <summary>
            /// メディア選択キー
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            SelectMedia = WindowFormKeys.SelectMedia,

            /// <summary>
            /// カスタムホットキー1
            /// ●謎のキー、カスタム操作は対象外とする
            /// </summary>
            LaunchApplication1 = WindowFormKeys.LaunchApplication1,

            /// <summary>
            /// カスタムホットキー2
            /// ●謎のキー、カスタム操作は対象外とする
            /// </summary>
            LaunchApplication2 = WindowFormKeys.LaunchApplication2,

            /// <summary>
            /// OEM 8
            /// ●使用していないと想定されるキーのため対象外とする
            /// </summary>
            Oem8 = WindowFormKeys.Oem8,

            /// <summary>
            /// ProcessKey
            /// ●謎のキー、押下しても反応しないため対象外とする
            /// </summary>
            ProcessKey = WindowFormKeys.ProcessKey,

            /// <summary>
            /// Unicode文字をキーストロークであるかのように渡すための32ビット仮想キー値の下位ワード
            /// ●Unicode文字の送信は別途実装が必要なためここでは対象外とする
            /// </summary>
            Packet = WindowFormKeys.Packet,

            /// <summary>
            /// Attn
            /// ●謎のキー、使用しないため対象外とする
            /// </summary>
            Attn = WindowFormKeys.Attn,

            /// <summary>
            /// Crsel
            /// ●謎のキー、使用しないため対象外とする
            /// </summary>
            Crsel = WindowFormKeys.Crsel,

            /// <summary>
            /// Exsel
            /// ●謎のキー、使用しないため対象外とする
            /// </summary>
            Exsel = WindowFormKeys.Exsel,

            /// <summary>
            /// Erase Eof
            /// ●謎のキー、使用しないため対象外とする
            /// </summary>
            EraseEof = WindowFormKeys.EraseEof,

            /// <summary>
            /// Play
            /// ●謎のキー、使用しないため対象外とする
            /// </summary>
            Play = WindowFormKeys.Play,

            /// <summary>
            /// Zoom
            /// ●謎のキー、使用しないため対象外とする
            /// </summary>
            Zoom = WindowFormKeys.Zoom,

            /// <summary>
            /// 予約済定数
            /// ●使用しないため対象外とする
            /// </summary>
            NoName = WindowFormKeys.NoName,

            /// <summary>
            /// PA1
            /// ●謎のキー、使用しないため対象外とする
            /// </summary>
            Pa1 = WindowFormKeys.Pa1,

            /// <summary>
            /// Clear
            /// ●謎のキー、使用しないため対象外とする
            /// </summary>
            OemClear = WindowFormKeys.OemClear,
        }

        #endregion

        #region System.Windows.Forms.Keysに関するメモ
        /*
        【System.Windows.Forms.Keysで定義されているパラメータのメモ】
        ※●は使用しないキー

        None    0    キーを押していない

        LButton    0x01    マウスの左ボタン
        RButton    0x02    マウスの右ボタン
        ●Cancel    0x03    {Cancel}キー（{Break}（ブレイク処理）キー：{Ctrl}+{Pause} 又は {Ctrl}+{ScrollLock}）
        MButton    0x04    マウスの中央ボタン（3ボタンマウス）
        XButton1    0x05    マウスの第1拡張ボタン（5ボタンマウス）
        XButton2    0x06    マウスの第2拡張ボタン（5ボタンマウス）

        Back    0x08    {BackSpace}キー
        Tab    0x09    {Tab}キー
        ●LineFeed    0x0A    改行（LF：ラインフィード）、仮想キーに対応なしUnicode送信するしかない
        ●Clear    0x0C    Clear（NumLockオフのテンキーの{5}キーも含む）
        Enter    0x0D    {Enter}キー（テンキーの{Enter}キーも含む）
        Return    0x0D    {Enter}キー（テンキーの{Enter}キーも含む））
        ShiftKey    0x10    {Shift}キー
        ControlKey    0x11    {Ctrl}キー
        Menu    0x12    {Alt}キー
        Pause    0x13    {Pause}キー
        CapsLock    0x14    CapsLock（{Shift}+{CapsLock}キー）
        Capital    0x14    CapsLock（{Shift}+{CapsLock}キー）
        KanaMode    0x15    IME かなモード
        HanguelMode    0x15    IME ハングルモード (互換性を保つために保持されています。HangulMode を使用します)
        HangulMode    0x15    IME ハングルモード
        ●JunjaMode    0x17    IME Junjaモード
        ●FinalMode    0x18    IME Finalモード
        KanjiMode    0x19    IME 漢字モード（{Alt}+{半角／全角}キー）
        HanjaMode    0x19    IME 漢字モード（{Alt}+{半角／全角}キー）
        Escape    0x1B    {Esc}キー
        IMEConvert    0x1C    {変換}キー（IME変換）
        IMENonconvert    0x1D    {無変換}キー（IME無変換）
        ●IMEAccept    0x1E    IME Accept
        ●IMEAceept    0x1E    IME Accept 互換性を維持するために残されています。代わりに IMEAccept を使用してください。
        ●IMEModeChange    0x1F    IME モード変更
        Space    0x20    {Space}キー
        PageUp    0x21    {PageUp}キー（NumLockオフのテンキーの{9}キーも含む）
        Prior    0x21    {PageUp}キー（NumLockオフのテンキーの{9}キーも含む）
        PageDown    0x22    {PageDown}キー（NumLockオフのテンキーの{3}キーも含む）
        Next    0x22    {PageDown}キー（NumLockオフのテンキーの{3}キーも含む）
        End    0x23    {End}キー（NumLockオフのテンキーの{1}キーも含む）
        Home    0x24    {Home}キー（NumLockオフのテンキーの{7}キーも含む）
        Left    0x25    {←}キー（NumLockオフのテンキーの{4}キーも含む）
        Up    0x26    {↑}キー（NumLockオフのテンキーの{8}キーも含む）
        Right    0x27    {→}キー（NumLockオフのテンキーの{6}キーも含む）
        Down    0x28    {↓}キー（NumLockオフのテンキーの{2}キーも含む）
        ●Select    0x29    Select
        ●Print    0x2A    Print
        ●Execute    0x2B    Execute
        PrintScreen    0x2C    {PrintScreen}キー
        Snapshot    0x2C    {PrintScreen}キー
        Insert    0x2D    {Insert}キー（NumLockオフのテンキーの{0}キーも含む）
        Delete    0x2E    {Delete}キー（NumLockオフのテンキーの{.}キーも含む）
        ●Help    0x2F    Help

        D0    0x30    {0}キー
        D1    0x31    {1}キー
        D2    0x32    {2}キー
        D3    0x33    {3}キー
        D4    0x34    {4}キー
        D5    0x35    {5}キー
        D6    0x36    {6}キー
        D7    0x37    {7}キー
        D8    0x38    {8}キー
        D9    0x39    {9}キー
        A    0x41    {A}キー
        B    0x42    {B}キー
        C    0x43    {C}キー
        D    0x44    {D}キー
        E    0x45    {E}キー
        F    0x46    {F}キー
        G    0x47    {G}キー
        H    0x48    {H}キー
        I    0x49    {I}キー
        J    0x4A    {J}キー
        K    0x4B    {K}キー
        L    0x4C    {L}キー
        M    0x4D    {M}キー
        N    0x4E    {N}キー
        O    0x4F    {O}キー
        P    0x50    {P}キー
        Q    0x51    {Q}キー
        R    0x52    {R}キー
        S    0x53    {S}キー
        T    0x54    {T}キー
        U    0x55    {U}キー
        V    0x56    {V}キー
        W    0x57    {W}キー
        X    0x58    {X}キー
        Y    0x59    {Y}キー
        Z    0x5A    {Z}キー

        LWin    0x5B    左の{Windowsロゴ}キー（Microsoft Natural Keyboard）
        RWin    0x5C    右の{Windowsロゴ}キー（Microsoft Natural Keyboard）
        Apps    0x5D    {アプリケーション}キー（右のWindowsキーとCtrlキーの間にあるメニューが開くキー）（Microsoft Natural Keyboard）
        ●Sleep    0x5F    {Sleep}キー（コンピュータをスリープさせるキー）

        NumPad0    0x60    NumLockオンのテンキーの{0}キー
        NumPad1    0x61    NumLockオンのテンキーの{1}キー
        NumPad2    0x62    NumLockオンのテンキーの{2}キー
        NumPad3    0x63    NumLockオンのテンキーの{3}キー
        NumPad4    0x64    NumLockオンのテンキーの{4}キー
        NumPad5    0x65    NumLockオンのテンキーの{5}キー
        NumPad6    0x66    NumLockオンのテンキーの{6}キー
        NumPad7    0x67    NumLockオンのテンキーの{7}キー
        NumPad8    0x68    NumLockオンのテンキーの{5}キー
        NumPad9    0x69    NumLockオンのテンキーの{9}キー
        Multiply    0x6A    テンキーの{*}キー
        Add    0x6B    テンキーの{+}キー
        ●Separator    0x6C    区切り記号（カンマ{,}）
        Subtract    0x6D    テンキーの{-}キー
        Decimal    0x6E    テンキーの{.}キー
        Divide    0x6F    テンキーの{/}キー

        F1    0x70    {F1}キー
        F2    0x71    {F2}キー
        F3    0x72    {F3}キー
        F4    0x73    {F4}キー
        F5    0x74    {F5}キー
        F6    0x75    {F6}キー
        F7    0x76    {F7}キー
        F8    0x77    {F8}キー
        F9    0x78    {F9}キー
        F10    0x79    {F10}キー
        F11    0x7A    {F11}キー
        F12    0x7B    {F12}キー
        F13    0x7C    {F13}キー
        F14    0x7D    {F14}キー
        F15    0x7E    {F15}キー
        F16    0x7F    {F16}キー
        F17    0x80    {F17}キー
        F18    0x81    {F18}キー
        F19    0x82    {F19}キー
        F20    0x83    {F20}キー
        F21    0x84    {F21}キー
        F22    0x85    {F22}キー
        F23    0x86    {F23}キー
        F24    0x87    {F24}キー
        NumLock    0x90    {NumLock}キー
        Scroll    0x91    {ScrollLock}キー
        LShiftKey    0xA0    左の{Shift}キー
        RShiftKey    0xA1    右の{Shift}キー
        LControlKey    0xA2    左の{Ctrl}キー
        RControlKey    0xA3    右の{Ctrl}キー
        LMenu    0xA4    左の{Alt}キー
        RMenu    0xA5    右の{Alt}キー

        BrowserBack    0xA6    ブラウザーの戻るキー
        BrowserForward    0xA7    ブラウザーの進むキー
        BrowserRefresh    0xA8    ブラウザーの更新のキー
        BrowserStop    0xA9    ブラウザーの中止キー
        ●BrowserSearch    0xAA    ブラウザーの検索キー
        ●BrowserFavorites    0xAB    ブラウザーのお気に入りキー
        BrowserHome    0xAC    ブラウザーのホームキー
        VolumeMute    0xAD    音量ミュートキー
        VolumeDown    0xAE    音量 - キー
        VolumeUp    0xAF    音量 + キー
        MediaNextTrack    0xB0    次のトラックキー
        MediaPreviousTrack    0xB1    前のトラックキー（曲の先頭へ戻る又は前のトラックへ行く）
        ●MediaStop    0xB2    メディア停止キー
        MediaPlayPause    0xB3    メディア再生/一時停止キー
        ●LaunchMail    0xB4    メール起動キー
        ●SelectMedia    0xB5    メディア選択キー
        ●LaunchApplication1    0xB6    カスタムホットキー1
        ●LaunchApplication2    0xB7    カスタムホットキー2

        OemSemicolon    0xBA    米国標準キーボードの{;}キー（日本語キーボードの場合{: *}キー）
        Oem1    0xBA    米国標準キーボードの{;}キー（日本語キーボードの場合{: *}キー）
        Oemplus    0xBB    国または地域別キーボードの{+}キー（日本語キーボードの場合{; +}キー）
        Oemcomma    0xBC    国または地域別キーボードの{,}キー（日本語キーボードの場合{, <}キー）
        OemMinus    0xBD    国または地域別キーボードの{-}キー（日本語キーボードの場合{- =}キー）
        OemPeriod    0xBE    国または地域別キーボードの{.}キー（日本語キーボードの場合{. >}キー）
        OemQuestion    0xBF    米国標準キーボードの{?}キー（日本語キーボードの場合{/ ?}キー）
        Oem2    0xBF    米国標準キーボードの{?}キー（日本語キーボードの場合{/ ?}キー）
        Oemtilde    0xC0    米国標準キーボードの{~}キー（日本語キーボードの場合{@ `}キー）
        Oem3    0xC0    米国標準キーボードの{~}キー（日本語キーボードの場合{@ `}キー）
        OemOpenBrackets    0xDB    米国標準キーボードの{[}キー（日本語キーボードの場合{[ {}キー）
        Oem4    0xDB    米国標準キーボードの{[}キー（日本語キーボードの場合{[ {}キー）
        OemPipe    0xDC    米国標準キーボードの{|}キー（日本語キーボードの場合{\ |}キー）
        Oem5    0xDC    米国標準キーボードの{|}キー（日本語キーボードの場合{\ |}キー）
        OemCloseBrackets    0xDD    米国標準キーボード{]}キー（日本語キーボードの場合{] }}キー）
        Oem6    0xDD    米国標準キーボード{]}キー（日本語キーボードの場合{] }}キー）
        OemQuotes    0xDE    米国標準キーボードの{' "}キー（日本語キーボードの場合{^ ~}キー）
        Oem7    0xDE    米国標準キーボードの{' "}キー（日本語キーボードの場合{^ ~}キー）
        ●Oem8    0xDF    OEM 8
        OemBackslash    0xE2    RT102キーボードの{^ \}（日本語キーボードの場合{＼ _}キー）
        Oem102    0xE2    RT102キーボードの{^ \}（日本語キーボードの場合{＼ _}キー）

        ●ProcessKey    0xE5    ProcessKey

        ●Packet    0xE7    Unicode文字をキーストロークであるかのように渡すための32ビット仮想キー値の下位ワード

        ●Attn    0xF6    Attn
        ●Crsel    0xF7    Crsel
        ●Exsel    0xF8    Exsel
        ●EraseEof    0xF9    Erase Eof
        ●Play    0xFA    Play
        ●Zoom    0xFB    Zoom
        ●NoName    0xFC    予約済定数
        ●Pa1    0xFD    PA1
        ●OemClear    0xFE    Clear

        ●KeyCode    65535    キー値からキー コードを抽出するビット マスク。
        ●Modifiers    -65536    キー値から修飾子を抽出するビット マスク。
        ●Shift    0x10000    {Shift}修飾子キー
        ●Control    0x20000    {Ctrl}修飾子キー
        ●Alt    0x40000    {Alt}修飾子キー


        【仮想キーにおいてSystem.Windows.Forms.Keysで定義がないもの】
        VK_OEM_ATTN    0xF0    {英数 CapsLock}キー
        VK_OEM_COPY    0xF2    {カタカナ ひらがな}キー
        VK_OEM_AUTO    0xF3    {半角／全角}キー
        VK_OEM_BACKTAB    0xF5    ローマ字キー（{Alt}+{カタカナ ひらがな}キー）
        */
        #endregion

        #region メソッド

        /// <summary>
        /// <see cref="System.Windows.Forms.Keys"/> のキーを仮想キーへ変換する
        /// </summary>
        /// <param name="key">
        /// <see cref="System.Windows.Forms.Keys"/> で定義されるキー
        /// 複数のキーの情報（Ctrl+A等）をもっている入力は不可とする、入力された場合は変換不可とする
        /// </param>
        /// <param name="virtualKey">
        /// 変換した仮想キーの値、変換できない場合は <see cref="Keys.VK_NONE"/> を返却
        /// </param>
        /// <returns>変換に正解した場合は True、変換できない場合は False を返却</returns>
        public static bool TryToVirtualKey(WindowFormKeys key, out Keys virtualKey)
        {
            // 使用しないキーの定義に含まれているキーの場合は False を返却する
            if (Enum.IsDefined(typeof(NotUsedKeys), (int)key))
            {
                virtualKey = Keys.VK_NONE;
                return false;
            }

            // 引数のキーの値を取得する
            if (!short.TryParse(((int)key).ToString(CultureInfo.InvariantCulture), out short keyCode))
            {
                // short型に変換できない場合は False を返却する
                // short型に変換できないキーは下記のもの又は、複数のキーの情報をもっているものとなる（Ctrl+A等）
                // KeyCode  ：0x0000FFFF   キー値からキーコードを抽出するビットマスク
                // Modifiers：0xFFFF0000   キー値から修飾子を抽出するビットマスク
                // Shift    ：0x00010000   Shift修飾子キー
                // Control  ：0x00020000   Ctrl修飾子キー
                // Alt      ：0x00040000   Alt修飾子キー
                virtualKey = Keys.VK_NONE;
                return false;
            }

            // 仮想キーの定義に含まれていないキーの場合は False を返却する
            if (!Enum.IsDefined(typeof(Keys), keyCode))
            {
                virtualKey = Keys.VK_NONE;
                return false;
            }

            // 仮想キーに変換して、True を返却
            virtualKey = (Keys)keyCode;
            return true;
        }

        #endregion
    }
}
