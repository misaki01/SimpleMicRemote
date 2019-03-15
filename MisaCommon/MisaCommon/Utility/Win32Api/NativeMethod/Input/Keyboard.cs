namespace MisaCommon.Utility.Win32Api.NativeMethod.Input
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// キーボード操作の情報を扱うクラス
    /// </summary>
    internal static class Keyboard
    {
        #region フラグ定義

        /// <summary>
        /// 入力操作のタイプ：キーボード操作を行う
        /// （<see cref="INPUT.InputType"/> に設定する値）
        /// </summary>
        public const int InputType = 1;

        /// <summary>
        /// キーボード操作を示すビットフラグ
        /// </summary>
        [Flags]
        public enum OperateFlag : int
        {
            /// <summary>
            /// スキャンコードの前にプリフィックスバイト「0xE0（224）」を付与する場合に設定する
            /// </summary>
            KEYEVENTF_EXTENDEDKEY = 0x0001,

            /// <summary>
            /// キーを離す動作の場合に設定する
            /// 設定しない場合はキーを押す動作となる
            /// </summary>
            KEYEVENTF_KEYUP = 0x0002,

            /// <summary>
            /// Unicode文字をそのまま入力する場合に設定する
            /// Unicode文字は <see cref="KEYBDINPUT.ScanCode"/> を使用して設定する
            /// そのため <see cref="KEYBDINPUT.VirtualKeyCode"/> の値は 0 である必要がある
            /// またこのフラグは <see cref="KEYEVENTF_KEYUP"/> と組み合わせて使用することが可能である
            /// </summary>
            KEYEVENTF_UNICODE = 0x0004,

            /// <summary>
            /// <see cref="KEYBDINPUT.ScanCode"/> を使用する場合に設定する
            /// 設定した場合、<see cref="KEYBDINPUT.VirtualKeyCode"/> の値は無視する
            /// </summary>
            KEYEVENTF_SCANCODE = 0x0008,
        }

        #endregion

        #region 構造体定義

        /// <summary>
        /// キーボード操作の情報を扱う構造体
        /// （Win32Api：SendInputの引数に設定するINPUT構造体の定義）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            /// <summary>
            /// 入力操作のタイプ
            /// </summary>
            public int InputType;

            /// <summary>
            /// キーボード操作イベントの情報を扱う構造体
            /// </summary>
            public KEYBDINPUT Keyboard;
        }

        /// <summary>
        /// キーボード操作の情報を扱う構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 24)]
        public struct KEYBDINPUT
        {
            /// <summary>
            /// 仮想キーコード
            /// <see cref="OperateFlag"/> に　<see cref="OperateFlag.KEYEVENTF_UNICODE"/> を指定した場合は、
            /// 0 を設定する必要がある
            /// </summary>
            public short VirtualKeyCode;

            /// <summary>
            /// ハードウェアスキャンコード
            /// <see cref="OperateFlag"/> に　<see cref="OperateFlag.KEYEVENTF_UNICODE"/> を指定した場合は、
            /// Unicode文字を設定する
            /// </summary>
            public short ScanCode;

            /// <summary>
            /// キーボードの操作を示すビットフラグ（<see cref="Keyboard.OperateFlag"/>）を設定する
            /// フラグの設定は状態の変化を示すように設定する
            /// </summary>
            public int OperateFlag;

            /// <summary>
            /// マウスイベントのタイムスタンプ（ミリ秒単位）を設定する
            /// 0 を指定した場合はシステムは独自のタイムスタンプを使用する
            /// </summary>
            public int Time;

            /// <summary>
            /// マウスイベントに関連する追加情報を設定する
            /// 詳細は「GetMessageExtraInfo」を参照
            /// </summary>
            public IntPtr ExtraInfo;

            /// <summary>
            /// 構造体のサイズを<see cref="Mouse"/>と一致させるためのダミー領域
            /// Win32ApiのSendInputメソッドは構造体のサイズを<see cref="Mouse"/>に合わせる必要があるため、その調整用の領域
            /// </summary>
            public int SizingDummyData;
        }

        #endregion
    }
}
