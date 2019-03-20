namespace MisaCommon.Utility.Win32Api.NativeMethod.IconCursor
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// 【注意：このクラスのメソッドは直接呼び出さず、<see cref="IconCursorOperate"/> クラスを経由して呼び出すこと】
    /// プラットフォーム呼び出しサービスを使用してアンマネージ コードへのアクセスを提供するためのクラス
    /// このクラスではWin32APIのウィンドウに関する機能を扱う
    /// </summary>
    /// <remarks>
    /// このクラスは、アンマネージ コード アクセス許可のスタック ウォークを出力する
    /// そのため <see cref="SuppressUnmanagedCodeSecurityAttribute"/> の提供は不可とする
    /// 再利用できるライブラリにする場合は「SafeNativeMethods」クラス、
    /// 又は「UnsafeNativeMethods 」クラスで定義することを検討すること
    /// </remarks>
    internal static class NativeMethods
    {
        #region カーソルの情報を取得

        /// <summary>
        /// グローバルのカーソル情報を取得する
        /// </summary>
        /// <param name="cursorInfo">
        /// カーソル情報を受け取る <see cref="Cursor.CURSORINFO"/> 構造体
        /// この関数を呼び出す前に、<see cref="Cursor.CURSORINFO.StructureSize"/> メンバに
        /// sizeof(<see cref="Cursor.CURSORINFO.StructureSize"/>) を設定する必要がある。
        /// </param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorInfo(ref Cursor.CURSORINFO cursorInfo);

        #endregion

        #region アイコンの情報を取得

        /// <summary>
        /// グローバルのカーソル情報を取得する
        /// </summary>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <param name="iconInfo">アイコン情報を受け取る <see cref="Icon.ICONINFO"/> 構造体</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetIconInfo(IntPtr iconCursorHandle, out Icon.ICONINFO iconInfo);

        #endregion

        #region アイコンへのハンドルの複製

        /// <summary>
        /// 引数で指定された他のモジュールのアイコンへのハンドル（<paramref name="iconCursorHandle"/>）を
        /// 現在のモジュールのアイコンへのハンドルに複製する
        /// </summary>
        /// <remarks>
        /// <see cref="CopyIcon(IntPtr)"/>の機能は、別のモジュールが所有しているアイコンを、
        /// アプリケーション または、DLLへの独自のハンドルで取得することを可能にします。
        /// この結果、他のモジュールが解放されてもアプリケーションアイコンはアイコンとして使用できます。
        /// </remarks>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <returns>複製したアイコンへのハンドル（処理失敗時は NULL を返却）</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr CopyIcon(IntPtr iconCursorHandle);

        #endregion

        #region リソースの解放

        /// <summary>
        /// アイコンを破棄する
        /// （<see cref="CopyIcon(IntPtr)"/> で複製したアイコンは必ずこのメソッドで破棄する必要がある）
        /// </summary>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DestroyIcon(IntPtr iconCursorHandle);

        /// <summary>
        /// オブジェクトに関連付けられているすべてのシステムリソースを解放する
        /// （論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットを削除する
        /// 　オブジェクトが削除されると、指定されたハンドルは無効になる）
        /// </summary>
        /// <param name="objectHandle">
        /// 論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットへのハンドル
        /// </param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr objectHandle);

        #endregion

        #region CopyIconメソッドで使用するパラメータの定義

        /// <summary>
        /// <see cref="CopyIcon"/> メソッドで使用するパラメータの定義
        /// </summary>
        internal static class CopyIconParameter
        {
            /// <summary>
            /// <see cref="CopyIcon"/> の実行が成功したか判定する
            /// </summary>
            /// <remarks>
            /// <see cref="CopyIcon"/> の場合、失敗時はアイコンへのハンドルが NULL で返却
            /// </remarks>
            /// <param name="result"><see cref="CopyIcon"/> を実行した際の戻り値を指定</param>
            /// <returns>正常終了：True、異常終了：False</returns>
            public static bool IsSuccess(IntPtr result)
            {
                return result != IntPtr.Zero;
            }
        }

        #endregion

    }
}
