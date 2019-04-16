namespace MisaCommon.Utility.Win32Api.NativeMethod.Input
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// キーボード、マウス以外の入力デバイスの操作の情報を扱うクラス
    /// </summary>
    internal static class Hardware
    {
        #region フラグ定義

        /// <summary>
        /// 入力操作のタイプ：キーボード、マウス以外の入力デバイスの操作を行う
        /// （<see cref="INPUT.InputType"/> に設定する値）
        /// </summary>
        public const int InputType = 2;

        #endregion

        #region 構造体定義

        /// <summary>
        /// キーボード、マウス以外の入力デバイスの操作の情報を扱う構造体
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
            /// キーボード、マウス以外の入力デバイスの操作の情報を扱う構造体
            /// </summary>
            public HARDWAREINPUT Hardware;
        }

        /// <summary>
        /// キーボード、マウス以外の入力デバイスの操作の情報を扱う構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 24)]
        public struct HARDWAREINPUT
        {
            /// <summary>
            /// 入力するメッセージを設定
            /// </summary>
            public int Message;

            /// <summary>
            /// <see cref="Message"/> に対するlParamパラメータ（下位）を設定
            /// </summary>
            public short LowlParam;

            /// <summary>
            /// <see cref="Message"/> に対するlParamパラメータ（上位）を設定
            /// </summary>
            public short HighlParam;

            /// <summary>
            /// 構造体のサイズを<see cref="Mouse"/>と一致させるためのダミー領域
            /// Win32ApiのSendInputメソッドは構造体のサイズを <see cref="Mouse"/> に
            /// 合わせる必要があるため、その調整用の領域
            /// </summary>
            public int SizingDummyData1;

            /// <summary>
            /// 構造体のサイズを<see cref="Mouse"/>と一致させるためのダミー領域
            /// Win32ApiのSendInputメソッドは構造体のサイズを <see cref="Mouse"/> に
            /// 合わせる必要があるため、その調整用の領域
            /// </summary>
            public IntPtr SizingDummyData2;

            /// <summary>
            /// 構造体のサイズを<see cref="Mouse"/>と一致させるためのダミー領域
            /// Win32ApiのSendInputメソッドは構造体のサイズを <see cref="Mouse"/> に
            /// 合わせる必要があるため、その調整用の領域
            /// </summary>
            public int SizingDummyData3;
        }

        #endregion
    }
}
