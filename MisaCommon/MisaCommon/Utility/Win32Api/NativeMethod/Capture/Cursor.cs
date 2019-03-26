namespace MisaCommon.Utility.Win32Api.NativeMethod.Capture
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// カーソル（マウス）に関する情報を扱うクラス
    /// </summary>
    internal static class Cursor
    {
        #region フラグ定義

        /// <summary>
        /// カーソルの状態
        /// （<see cref="CURSORINFO.Flag"/> で使用）
        /// </summary>
        public enum State : int
        {
            /// <summary>
            /// カーソル：非表示
            /// </summary>
            CURSOR_HIDE = 0x0000,

            /// <summary>
            /// カーソル：表示
            /// </summary>
            CURSOR_SHOWING = 0x0001,

            /// <summary>
            /// カーソル：非表示
            /// Windows 8以降において、ユーザーがマウスではない タッチ または、ペンを介して
            /// 入力を行っているため、システムがカーソルを描画していない状態
            /// </summary>
            CURSOR_SUPPRESSED = 0x0002,
        }

        #endregion

        #region 構造体定義

        /// <summary>
        /// カーソルの座標情報を扱う構造体
        /// </summary>
        public struct POINT
        {
            /// <summary>
            /// カーソルの座標_X
            /// </summary>
            public int X;

            /// <summary>
            /// カーソルの座標_Y
            /// </summary>
            public int Y;
        }

        /// <summary>
        /// カーソル情報を扱う構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            /// <summary>
            /// この構造体のサイズバイト単位）
            /// 呼び出し側において、このパラメータを設定する必要がある
            /// （sizeof(<see cref="StructureSize"/>) で構造体のサイズを設定する）
            /// </summary>
            public int StructureSize;

            /// <summary>
            /// カーソルの状態を示すフラグ（<see cref="State"/>
            /// </summary>
            public int Flag;

            /// <summary>
            /// カーソルへのハンドル
            /// </summary>
            public IntPtr CursorHandle;

            /// <summary>
            /// カーソルのスクリーン座標
            /// </summary>
            public POINT ScreenPosition;
        }

        #endregion
    }
}
