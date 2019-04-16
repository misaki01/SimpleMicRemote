namespace MisaCommon.Utility.StaticMethod
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Windows.Forms;

    using MisaCommon.MessageResources;

    /// <summary>
    /// <see cref="Keys"/> と <see cref="KeyName"/> のマッピングを行うクラス
    /// </summary>
    public static class KeyNameMapping
    {
        #region クラス変数・定数

        /// <summary>
        /// <see cref="Keys"/> と <see cref="KeyName"/> のマッピングDictionary
        /// </summary>
        private static IReadOnlyDictionary<Keys, string> keyNameDictionary = null;

        /// <summary>
        /// <see cref="KeyName"/> が使用する言語
        /// </summary>
        private static CultureInfo resourceCultureInfo = null;

        /// <summary>
        /// スレッドに設定されているユーザインターフェースが使用する言語
        /// </summary>
        private static CultureInfo currentUICulture = null;

        #endregion

        #region プロパティ

        /// <summary>
        /// <see cref="Keys"/> と <see cref="KeyName"/> のマッピングDictionaryを取得する
        /// </summary>
        public static IReadOnlyDictionary<Keys, string> Mapping
        {
            get
            {
                // マッピングが既に設定されており、使用する言語に変更がない場合
                // 設定されているマッピングをそのまま返却
                bool isChange = IsChangeCulture(
                    beforeResourceCulture: resourceCultureInfo,
                    beforeCurrentUICulture: currentUICulture,
                    nowResourceCulture: KeyName.Culture,
                    nowCurrentUICulture: CultureInfo.CurrentUICulture);
                if (keyNameDictionary != null && !isChange)
                {
                    return keyNameDictionary;
                }

                // マッピングが未設定又は言語に変更があった場合は、
                // 現在の言語を保持しマッピングを再生成して返却
                resourceCultureInfo = KeyName.Culture;
                currentUICulture = CultureInfo.CurrentUICulture;
                keyNameDictionary = CreateMapping();

                return keyNameDictionary;
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 引数の <paramref name="key"/> に対応する名称を取得する
        /// 該当する名称が存在しない場合はNULLを返却
        /// </summary>
        /// <param name="key">名称を取得する対象のキー</param>
        /// <returns>
        /// 引数の <paramref name="key"/> に対応する名称
        /// 該当する名称が存在しない場合はNULLを返却
        /// </returns>
        public static string GetName(Keys key)
        {
            return Mapping.ContainsKey(key) ? Mapping[key] : null;
        }

        /// <summary>
        /// 使用言語が変更されたか判定する
        /// </summary>
        /// <param name="beforeResourceCulture">
        /// 以前の <see cref="KeyName"/> が使用していた言語
        /// </param>
        /// <param name="beforeCurrentUICulture">
        /// 以前のスレッドに設定されているユーザインターフェースが使用していた言語
        /// </param>
        /// <param name="nowResourceCulture">
        /// 今の <see cref="KeyName"/> が使用する言語
        /// </param>
        /// <param name="nowCurrentUICulture">
        /// 今のスレッドに設定されているユーザインターフェースが使用する言語
        /// </param>
        /// <returns>
        /// 使用言語が変更された場合 True、使用言語が変更されていない場合 False
        /// </returns>
        private static bool IsChangeCulture(
            CultureInfo beforeResourceCulture,
            CultureInfo beforeCurrentUICulture,
            CultureInfo nowResourceCulture,
            CultureInfo nowCurrentUICulture)
        {
            // リソースが使用する言語がNULLの場合はスレッドの方を使用する
            CultureInfo before = beforeResourceCulture ?? beforeCurrentUICulture;
            CultureInfo now = nowResourceCulture ?? nowCurrentUICulture;

            // 以前と今を比較する
            if (before == null
                || now == null
                || !before.Equals(now))
            {
                // 以前の言語 又は 今の言語がNULL 又は 以前と今の言語が異なる場合は
                // 言語が変更されたと判断し True を返却
                return true;
            }

            // 上記以外の場合は言語の変更なしと判断し Falseを返却
            return false;
        }

        /// <summary>
        /// 新しく <see cref="Keys"/> と <see cref="KeyName"/> のマッピングDictionaryを生成する
        /// </summary>
        /// <returns>
        /// 生成した <see cref="Keys"/> と <see cref="KeyName"/> のマッピングDictionaryを返却
        /// </returns>
        private static IReadOnlyDictionary<Keys, string> CreateMapping()
        {
            IReadOnlyDictionary<Keys, string> mapping = new ReadOnlyDictionary<Keys, string>(
                new Dictionary<Keys, string>()
                {
                    { Keys.None, KeyName.Key0x00_None },
                    { Keys.KeyCode, KeyName.Key0x0000FFFF_KeyCode },
                    { Keys.Shift, KeyName.Key0x00010000_Shift },
                    { Keys.Control, KeyName.Key0x00020000_Control },
                    { Keys.Alt, KeyName.Key0x00040000_Alt },
                    { Keys.Modifiers, KeyName.Key0x000F0000_Modifiers },
                    { Keys.LButton, KeyName.Key0x01_LButton },
                    { Keys.RButton, KeyName.Key0x02_RButton },
                    { Keys.Cancel, KeyName.Key0x03_Cancel },
                    { Keys.MButton, KeyName.Key0x04_MButton },
                    { Keys.XButton1, KeyName.Key0x05_XButton1 },
                    { Keys.XButton2, KeyName.Key0x06_XButton2 },
                    { Keys.Back, KeyName.Key0x08_Back },
                    { Keys.Tab, KeyName.Key0x09_Tab },
                    { Keys.LineFeed, KeyName.Key0x0A_LineFeed },
                    { Keys.Clear, KeyName.Key0x0C_Clear },
                    { Keys.Enter, KeyName.Key0x0D_Enter },
                    { Keys.ShiftKey, KeyName.Key0x10_ShiftKey },
                    { Keys.ControlKey, KeyName.Key0x11_ControlKey },
                    { Keys.Menu, KeyName.Key0x12_Menu },
                    { Keys.Pause, KeyName.Key0x13_Pause },
                    { Keys.CapsLock, KeyName.Key0x14_CapsLock },
                    { Keys.KanaMode, KeyName.Key0x15_KanaMode },
                    { Keys.JunjaMode, KeyName.Key0x17_JunjaMode },
                    { Keys.FinalMode, KeyName.Key0x18_FinalMode },
                    { Keys.KanjiMode, KeyName.Key0x19_KanjiMode },
                    { Keys.Escape, KeyName.Key0x1B_Escape },
                    { Keys.IMEConvert, KeyName.Key0x1C_IMEConvert },
                    { Keys.IMENonconvert, KeyName.Key0x1D_IMENonconvert },
                    { Keys.IMEAccept, KeyName.Key0x1E_IMEAccept },
                    { Keys.IMEModeChange, KeyName.Key0x1F_IMEModeChange },
                    { Keys.Space, KeyName.Key0x20_Space },
                    { Keys.PageUp, KeyName.Key0x21_PageUp },
                    { Keys.PageDown, KeyName.Key0x22_PageDown },
                    { Keys.End, KeyName.Key0x23_End },
                    { Keys.Home, KeyName.Key0x24_Home },
                    { Keys.Left, KeyName.Key0x25_Left },
                    { Keys.Up, KeyName.Key0x26_Up },
                    { Keys.Right, KeyName.Key0x27_Right },
                    { Keys.Down, KeyName.Key0x28_Down },
                    { Keys.Select, KeyName.Key0x29_Select },
                    { Keys.Print, KeyName.Key0x2A_Print },
                    { Keys.Execute, KeyName.Key0x2B_Execute },
                    { Keys.PrintScreen, KeyName.Key0x2C_PrintScreen },
                    { Keys.Insert, KeyName.Key0x2D_Insert },
                    { Keys.Delete, KeyName.Key0x2E_Delete },
                    { Keys.Help, KeyName.Key0x2F_Help },
                    { Keys.D0, KeyName.Key0x30_D0 },
                    { Keys.D1, KeyName.Key0x31_D1 },
                    { Keys.D2, KeyName.Key0x32_D2 },
                    { Keys.D3, KeyName.Key0x33_D3 },
                    { Keys.D4, KeyName.Key0x34_D4 },
                    { Keys.D5, KeyName.Key0x35_D5 },
                    { Keys.D6, KeyName.Key0x36_D6 },
                    { Keys.D7, KeyName.Key0x37_D7 },
                    { Keys.D8, KeyName.Key0x38_D8 },
                    { Keys.D9, KeyName.Key0x39_D9 },
                    { Keys.A, KeyName.Key0x41_A },
                    { Keys.B, KeyName.Key0x42_B },
                    { Keys.C, KeyName.Key0x43_C },
                    { Keys.D, KeyName.Key0x44_D },
                    { Keys.E, KeyName.Key0x45_E },
                    { Keys.F, KeyName.Key0x46_F },
                    { Keys.G, KeyName.Key0x47_G },
                    { Keys.H, KeyName.Key0x48_H },
                    { Keys.I, KeyName.Key0x49_I },
                    { Keys.J, KeyName.Key0x4A_J },
                    { Keys.K, KeyName.Key0x4B_K },
                    { Keys.L, KeyName.Key0x4C_L },
                    { Keys.M, KeyName.Key0x4D_M },
                    { Keys.N, KeyName.Key0x4E_N },
                    { Keys.O, KeyName.Key0x4F_O },
                    { Keys.P, KeyName.Key0x50_P },
                    { Keys.Q, KeyName.Key0x51_Q },
                    { Keys.R, KeyName.Key0x52_R },
                    { Keys.S, KeyName.Key0x53_S },
                    { Keys.T, KeyName.Key0x54_T },
                    { Keys.U, KeyName.Key0x55_U },
                    { Keys.V, KeyName.Key0x56_V },
                    { Keys.W, KeyName.Key0x57_W },
                    { Keys.X, KeyName.Key0x58_X },
                    { Keys.Y, KeyName.Key0x59_Y },
                    { Keys.Z, KeyName.Key0x5A_Z },
                    { Keys.LWin, KeyName.Key0x5B_LWin },
                    { Keys.RWin, KeyName.Key0x5C_RWin },
                    { Keys.Apps, KeyName.Key0x5D_Apps },
                    { Keys.Sleep, KeyName.Key0x5F_Sleep },
                    { Keys.NumPad0, KeyName.Key0x60_NumPad0 },
                    { Keys.NumPad1, KeyName.Key0x61_NumPad1 },
                    { Keys.NumPad2, KeyName.Key0x62_NumPad2 },
                    { Keys.NumPad3, KeyName.Key0x63_NumPad3 },
                    { Keys.NumPad4, KeyName.Key0x64_NumPad4 },
                    { Keys.NumPad5, KeyName.Key0x65_NumPad5 },
                    { Keys.NumPad6, KeyName.Key0x66_NumPad6 },
                    { Keys.NumPad7, KeyName.Key0x67_NumPad7 },
                    { Keys.NumPad8, KeyName.Key0x68_NumPad8 },
                    { Keys.NumPad9, KeyName.Key0x69_NumPad9 },
                    { Keys.Multiply, KeyName.Key0x6A_Multiply },
                    { Keys.Add, KeyName.Key0x6B_Add },
                    { Keys.Separator, KeyName.Key0x6C_Separator },
                    { Keys.Subtract, KeyName.Key0x6D_Subtract },
                    { Keys.Decimal, KeyName.Key0x6E_Decimal },
                    { Keys.Divide, KeyName.Key0x6F_Divide },
                    { Keys.F1, KeyName.Key0x70_F1 },
                    { Keys.F2, KeyName.Key0x71_F2 },
                    { Keys.F3, KeyName.Key0x72_F3 },
                    { Keys.F4, KeyName.Key0x73_F4 },
                    { Keys.F5, KeyName.Key0x74_F5 },
                    { Keys.F6, KeyName.Key0x75_F6 },
                    { Keys.F7, KeyName.Key0x76_F7 },
                    { Keys.F8, KeyName.Key0x77_F8 },
                    { Keys.F9, KeyName.Key0x78_F9 },
                    { Keys.F10, KeyName.Key0x79_F10 },
                    { Keys.F11, KeyName.Key0x7A_F11 },
                    { Keys.F12, KeyName.Key0x7B_F12 },
                    { Keys.F13, KeyName.Key0x7C_F13 },
                    { Keys.F14, KeyName.Key0x7D_F14 },
                    { Keys.F15, KeyName.Key0x7E_F15 },
                    { Keys.F16, KeyName.Key0x7F_F16 },
                    { Keys.F17, KeyName.Key0x80_F17 },
                    { Keys.F18, KeyName.Key0x81_F18 },
                    { Keys.F19, KeyName.Key0x82_F19 },
                    { Keys.F20, KeyName.Key0x83_F20 },
                    { Keys.F21, KeyName.Key0x84_F21 },
                    { Keys.F22, KeyName.Key0x85_F22 },
                    { Keys.F23, KeyName.Key0x86_F23 },
                    { Keys.F24, KeyName.Key0x87_F24 },
                    { Keys.NumLock, KeyName.Key0x90_NumLock },
                    { Keys.Scroll, KeyName.Key0x91_Scroll },
                    { Keys.LShiftKey, KeyName.Key0xA0_LShiftKey },
                    { Keys.RShiftKey, KeyName.Key0xA1_RShiftKey },
                    { Keys.LControlKey, KeyName.Key0xA2_LControlKey },
                    { Keys.RControlKey, KeyName.Key0xA3_RControlKey },
                    { Keys.LMenu, KeyName.Key0xA4_LMenu },
                    { Keys.RMenu, KeyName.Key0xA5_RMenu },
                    { Keys.BrowserBack, KeyName.Key0xA6_BrowserBack },
                    { Keys.BrowserForward, KeyName.Key0xA7_BrowserForward },
                    { Keys.BrowserRefresh, KeyName.Key0xA8_BrowserRefresh },
                    { Keys.BrowserStop, KeyName.Key0xA9_BrowserStop },
                    { Keys.BrowserSearch, KeyName.Key0xAA_BrowserSearch },
                    { Keys.BrowserFavorites, KeyName.Key0xAB_BrowserFavorites },
                    { Keys.BrowserHome, KeyName.Key0xAC_BrowserHome },
                    { Keys.VolumeMute, KeyName.Key0xAD_VolumeMute },
                    { Keys.VolumeDown, KeyName.Key0xAE_VolumeDown },
                    { Keys.VolumeUp, KeyName.Key0xAF_VolumeUp },
                    { Keys.MediaNextTrack, KeyName.Key0xB0_MediaNextTrack },
                    { Keys.MediaPreviousTrack, KeyName.Key0xB1_MediaPreviousTrack },
                    { Keys.MediaStop, KeyName.Key0xB2_MediaStop },
                    { Keys.MediaPlayPause, KeyName.Key0xB3_MediaPlayPause },
                    { Keys.LaunchMail, KeyName.Key0xB4_LaunchMail },
                    { Keys.SelectMedia, KeyName.Key0xB5_SelectMedia },
                    { Keys.LaunchApplication1, KeyName.Key0xB6_LaunchApplication1 },
                    { Keys.LaunchApplication2, KeyName.Key0xB7_LaunchApplication2 },
                    { Keys.OemSemicolon, KeyName.Key0xBA_OemSemicolon },
                    { Keys.Oemplus, KeyName.Key0xBB_Oemplus },
                    { Keys.Oemcomma, KeyName.Key0xBC_Oemcomma },
                    { Keys.OemMinus, KeyName.Key0xBD_OemMinus },
                    { Keys.OemPeriod, KeyName.Key0xBE_OemPeriod },
                    { Keys.OemQuestion, KeyName.Key0xBF_OemQuestion },
                    { Keys.Oemtilde, KeyName.Key0xC0_Oemtilde },
                    { Keys.OemOpenBrackets, KeyName.Key0xDB_OemOpenBrackets },
                    { Keys.OemPipe, KeyName.Key0xDC_OemPipe },
                    { Keys.OemCloseBrackets, KeyName.Key0xDD_OemCloseBrackets },
                    { Keys.OemQuotes, KeyName.Key0xDE_OemQuotes },
                    { Keys.Oem8, KeyName.Key0xDF_Oem8 },
                    { Keys.OemBackslash, KeyName.Key0xE2_OemBackslash },
                    { Keys.ProcessKey, KeyName.Key0xE5_ProcessKey },
                    { Keys.Packet, KeyName.Key0xE7_Packet },
                    { (Keys)0xF0, KeyName.Key0xF0_VK_OEM_ATTN },
                    { (Keys)0xF2, KeyName.Key0xF2_VK_OEM_COPY },
                    { (Keys)0xF3, KeyName.Key0xF3_VK_OEM_AUTO },
                    { (Keys)0xF5, KeyName.Key0xF5_VK_OEM_BACKTAB },
                    { Keys.Attn, KeyName.Key0xF6_Attn },
                    { Keys.Crsel, KeyName.Key0xF7_Crsel },
                    { Keys.Exsel, KeyName.Key0xF8_Exsel },
                    { Keys.EraseEof, KeyName.Key0xF9_EraseEof },
                    { Keys.Play, KeyName.Key0xFA_Play },
                    { Keys.Zoom, KeyName.Key0xFB_Zoom },
                    { Keys.NoName, KeyName.Key0xFC_NoName },
                    { Keys.Pa1, KeyName.Key0xFD_Pa1 },
                    { Keys.OemClear, KeyName.Key0xFE_OemClear },
                });

            return mapping;
        }

        #endregion
    }
}
