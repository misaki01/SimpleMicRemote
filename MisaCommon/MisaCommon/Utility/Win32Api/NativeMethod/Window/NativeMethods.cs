namespace MisaCommon.Utility.Win32Api.NativeMethod.Window
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// 【注意：このクラスのメソッドは直接呼び出さず、<see cref="WindowOperate"/> クラスを経由して呼び出すこと】
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
        #region ウィンドウのハンドルを取得

        /// <summary>
        /// <see cref="EnumWindows"/> メソッドから呼び出されるコールバック関数のデリゲート定義
        /// </summary>
        /// <remarks>
        /// P/Invoke（プラットフォーム呼び出し）において、ジェネリック型は使用不可である
        /// そのため、コールバック関数に <see cref="Func{T1, T2, TResult}"/> の使用ができないため、
        /// このデリゲート定義を行っている
        /// </remarks>
        /// <param name="windowHandle">
        /// <see cref="EnumWindows"/> メソッドから渡されるトップレベルウィンドウのハンドル
        /// </param>
        /// <param name="value">
        /// <see cref="EnumWindows"/> メソッドから渡される値
        /// </param>
        /// <returns>
        /// <see cref="EnumWindows"/> 列挙処理を継続する場合は True、中断する場合は False を返却
        /// </returns>
        internal delegate bool EnumWindowsCallBackDelegate(IntPtr windowHandle, IntPtr value);

        /// <summary>
        /// 画面上のすべてのトップレベルウィンドウを列挙する
        /// </summary>
        /// <remarks>
        /// このメソッドを呼び出すと、下記の動作を繰り返し実行しトップレベルウィンドウの列挙を行う
        /// 　1) トップレベルウィンドウを取得
        /// 　2) トップレベルウィンドウのハンドルを引数のコールバック関数（<paramref name="callBack"/>）に渡す
        /// 　3) コールバック関数（<paramref name="callBack"/>）を実行する
        ///
        /// 上記処理は全てのトップレベルウィンドウを列挙し終えるか、
        /// コールバック関数（<paramref name="callBack"/>）から 0（False）が返却されるまで繰り返す
        /// </remarks>
        /// <param name="callBack">
        /// <see cref="EnumWindowsCallBackDelegate"/> で定義されるコールバック関数を指定
        /// </param>
        /// <param name="callBackValue">
        /// コールバック関数に渡す値を指定
        /// </param>
        /// <returns>
        /// 正常終了：True、異常終了：False
        /// コールバックメソッドが False を返却し処理を中断した場合は異常終了：False を返却
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumWindows(EnumWindowsCallBackDelegate callBack, IntPtr callBackValue);

        /// <summary>
        /// フォアグラウンドウィンドウ（現在ユーザーが作業している最前面ウィンドウ）のハンドルを取得する
        /// </summary>
        /// <returns>
        /// フォアグラウンドウィンドウ（現在ユーザーが作業している最前面ウィンドウのハンドル
        /// </returns>
        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// ディスクトップウィンドのハンドルを取得する
        /// </summary>
        /// <returns>
        /// ディスクトップウィンドのハンドル
        /// </returns>
        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr GetDesktopWindow();

        #endregion

        #region ウインドウが表示されている状態の取得（表示されてるか、最小化されてるか等）

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウが、
        /// 存在しているかチェックする
        /// </summary>
        /// <param name="windowHandle">チェック対象のウィンドウのハンドル</param>
        /// <returns>ウィンドウが存在する場合：True、存在しない場合：False</returns>
        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindow(IntPtr windowHandle);

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウが、
        /// 有効か無効か（マウスやキーボードの入力を受け付けるか受け付けないか）をチェックする
        /// </summary>
        /// <param name="windowHandle">チェック対象のウィンドウのハンドル</param>
        /// <returns>ウィンドウが有効な場合：True、無効な場合：False</returns>
        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowEnabled(IntPtr windowHandle);

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウが、
        /// 表示されているかチェックする
        /// </summary>
        /// <param name="windowHandle">チェック対象のウィンドウのハンドル</param>
        /// <returns>ウィンドウが表示されている場合：True、表示されていない場合：False</returns>
        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr windowHandle);

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウが、
        /// 最小化（アイコン化）されているかチェックする
        /// </summary>
        /// <param name="windowHandle">チェック対象のウィンドウのハンドル</param>
        /// <returns>ウィンドウが最小化されている場合：True、最小化されていない場合：False</returns>
        [DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsIconic(IntPtr windowHandle);

        #endregion

        #region ウィンドウに対する情報を取得

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウにおいて、
        /// そのウィンドウを作成した、スレッドのID 及び プロセスIDを取得する
        /// </summary>
        /// <param name="windowHandle">取得対象のウィンドウのハンドル</param>
        /// <param name="processId">ウィンドウのプロセスIDを格納</param>
        /// <returns>ウィンドウを作成したスレッドのID（処理が失敗した場合は 0 を返却）</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int GetWindowThreadProcessId(IntPtr windowHandle, out int processId);

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウにおいて、
        /// そのウィンドウの上下左右の座標情報を取得する
        /// </summary>
        /// <param name="windowHandle">取得対象のウィンドウのハンドル</param>
        /// <param name="windowRect">ウィンドウの上下左右の座標情報を格納</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr windowHandle, out RECT windowRect);

        #endregion

        #region ウインドウに対する設定

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウを最小化する
        /// </summary>
        /// <param name="windowHandle">最小化対象のウィンドウのハンドル</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseWindow(IntPtr windowHandle);

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウを最小化状態から元に戻す
        /// 元に戻したウィンドウはアクティブにする
        /// </summary>
        /// <param name="windowHandle">最小化状態を戻す対象のウィンドウのハンドル</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool OpenIcon(IntPtr windowHandle);

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウの、
        /// サイズ、位置、及び Zオーダーを変更する
        /// </summary>
        /// <param name="windowHandle">
        /// 送信対象のウィンドウのハンドル
        /// </param>
        /// <param name="windowHandleOrder">
        /// Zオーダーに関する配置順序を設定するためのウィンドウハンドル
        /// 引数（<paramref name="windowHandle"/>）のウィンドウはこのパラメータで指定したウィンドウの後ろに配置する
        /// 又は、このパラメータに指定したコマンドに従ってZオーダの配置を行う
        /// </param>
        /// <param name="pointX">
        /// 設定するウィンドウの左上のX座標
        /// </param>
        /// <param name="pointY">
        /// 設定するウィンドウの左上のY座標
        /// </param>
        /// <param name="sizeWidth">
        /// 設定するウィンドウの幅
        /// </param>
        /// <param name="sizeHeight">
        /// 設定するウィンドウの高さ
        /// </param>
        /// <param name="optionFlag">
        /// ウィンドウのサイズと位置の変更に関するオプションフラグ
        /// </param>
        /// <returns>
        /// 正常終了：True、異常終了：False
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(IntPtr windowHandle, IntPtr windowHandleOrder, int pointX, int pointY, int sizeWidth, int sizeHeight, uint optionFlag);

        #endregion

        #region GetWindowRectメソッドで使用する構造体の定義

        /// <summary>
        /// ウィンドウの上下左右の座標情報を扱う構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// ウインドウの左端の座標
            /// </summary>
            public int Left;

            /// <summary>
            /// ウインドウの上端の座標
            /// </summary>
            public int Top;

            /// <summary>
            /// ウインドウの右端の座標
            /// </summary>
            public int Right;

            /// <summary>
            /// ウインドウの下端の座標
            /// </summary>
            public int Bottom;
        }

        #endregion

        #region GetWindowThreadProcessIdメソッドで使用するパラメータの定義

        /// <summary>
        /// <see cref="GetWindowThreadProcessId"/> メソッドで使用するパラメータの定義
        /// </summary>
        internal static class GetWindowThreadProcessIdParameter
        {
            /// <summary>
            /// <see cref="GetWindowThreadProcessId"/> の実行が成功したか判定する
            /// </summary>
            /// <remarks>
            /// <see cref="GetWindowThreadProcessId"/> の場合、失敗時はスレッドIDを 0 で返却
            /// </remarks>
            /// <param name="result"><see cref="GetWindowThreadProcessId"/> を実行した際の戻り値を指定</param>
            /// <returns>正常終了：True、異常終了：False</returns>
            public static bool IsSuccess(int result)
            {
                return result != 0;
            }
        }

        #endregion

        #region SetWindowPosメソッドで使用するパラメータの定義クラス

        /// <summary>
        /// <see cref="SetWindowPos"/> メソッドで使用するパラメータの定義
        /// </summary>
        internal static class SetWindowPosParameter
        {
            /// <summary>
            /// ウィンドウの配置順番を指定するフラグ
            /// 引数（Zオーダーに関する配置順序を設定するためのウィンドウハンドル）において、
            /// ウィンドウハンドルの代わりに設定する配置順序の指定を行うコマンド
            /// </summary>
            public enum OrderCommand : int
            {
                /// <summary>
                /// ウィンドウをZオーダーの先頭に配置
                /// </summary>
                HWND_TOP = 0,

                /// <summary>
                /// ウィンドウをZオーダーの最後に配置
                /// </summary>
                HWND_BOTTOM = 1,

                /// <summary>
                /// ウィンドウを最前面ウィンドウに配置する
                /// アクティブでない場合でも常に最前面で表示する
                /// </summary>
                HWND_TOPMOST = -1,

                /// <summary>
                /// ウィンドウを最前面ウィンドウの一つ前に配置する
                /// ウィンドウが既に最前面ウィンドウではなかった場合、このフラグは意味を成さない
                /// </summary>
                HWND_NOTOPMOST = -2,
            }

            /// <summary>
            /// ウィンドウのサイズと位置の変更に関するオプションのビットフラグ
            /// </summary>
            [Flags]
            public enum OptionFlag : uint
            {
                /// <summary>
                /// 現在のサイズを維持する
                /// サイズ（幅、高さ）の引数は無視する
                /// </summary>
                SWP_NOSIZE = 0x0001,

                /// <summary>
                /// 現在の位置を維持する
                /// 位置（X座標、Y座標）の引数は無視する
                /// </summary>
                SWP_NOMOVE = 0x0002,

                /// <summary>
                /// 現在のZオーダーを維持する
                /// Zオーダーに関する配置順序（配置順所で指定するウィンドウハンドル）の引数は無視する
                /// </summary>
                SWP_NOZORDER = 0x0004,

                /// <summary>
                /// 変更結果を再描画しない
                /// このフラグを指定すると再描画は一切行われないため、
                /// アプリケーションで明示的に無効化または再描画を行う必要がある
                /// </summary>
                SWP_NOREDRAW = 0x0008,

                /// <summary>
                /// ウィンドウをアクティブ化しない
                /// このフラグを設定しなかった場合、ウィンドウはアクティブ化され最前面で表示する
                /// </summary>
                SWP_NOACTIVATE = 0x0010,

                /// <summary>
                /// ウィンドウを囲む枠を描画する
                /// </summary>
                SWP_DRAWFRAME = 0x0020,

                /// <summary>
                /// ウィンドウを囲む枠を描画する（SetWindowLong 関数を使って新しいフレームスタイルの設定を適用する）
                /// ウィンドウサイズが変更されない場合にも、ウィンドウに WM_NCCALCSIZE メッセージを送る
                /// このフラグを設定しなかった場合、ウィンドウサイズが変更される場合のみ WM_NCCALCSIZE メッセージを送る
                /// </summary>
                /// <remarks>
                /// 結果として <see cref="SWP_DRAWFRAME"/>と同じ動作となるため同じ値となる
                /// </remarks>
                SWP_FRAMECHANGED = 0x0020,

                /// <summary>
                /// ウィンドウを表示する
                /// </summary>
                SWP_SHOWWINDOW = 0x0040,

                /// <summary>
                /// ウィンドウを非表示にする
                /// </summary>
                SWP_HIDEWINDOW = 0x0080,

                /// <summary>
                /// クライアント領域の内容全体を破棄する
                /// </summary>
                SWP_NOCOPYBITS = 0x0100,

                /// <summary>
                /// オーナーウィンドウのZオーダーを変更しない
                /// </summary>
                SWP_NOOWNERZORDER = 0x0200,

                /// <summary>
                /// <see cref="SWP_NOOWNERZORDER"/> と同じ
                /// </summary>
                SWP_NOREPOSITION = 0x0200,

                /// <summary>
                /// WM_WINDOWPOSCHANGING メッセージを送信させない
                /// （WM_WINDOWPOSCHANGING：Zオーダーのサイズ、位置が変更されようとしているウィンドウに送信されるメッセージ）
                /// </summary>
                SWP_NOSENDCHANGING = 0x0400,

                /// <summary>
                /// WM_SYNCPAINT メッセージを生成させない
                /// （WM_SYNCPAINT：表示を同期させるために送信されるメッセージ）
                /// </summary>
                SWP_DEFERERASE = 0x2000,

                /// <summary>
                /// この関数を呼び出したスレッドとウィンドウを所有するスレッドが異なる入力キューに関連付けられている場合に、
                /// ウィンドウを所有するスレッドにも非同期に要求を送信する
                /// こうした場合、要求を受け取ったウィンドウスレッドが要求を処理している間も、
                /// 関数を呼び出したスレッドの実行が止まることはなくなる
                /// </summary>
                SWP_ASYNCWINDOWPOS = 0x4000,
            }
        }

        #endregion
    }
}
