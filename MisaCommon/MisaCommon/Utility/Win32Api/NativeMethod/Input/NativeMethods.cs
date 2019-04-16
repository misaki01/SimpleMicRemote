namespace MisaCommon.Utility.Win32Api.NativeMethod.Input
{
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// 【注意】
    /// このクラスのメソッドは直接呼び出さず、<see cref="InputOperate"/> クラスを経由して呼び出すこと
    /// 【概要】
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
        #region キーストローク、マウスの動き、ボタンのクリック等の操作を行う

        /// <summary>
        /// マウスの操作を行う
        /// </summary>
        /// <param name="count">
        /// 入力操作の情報（<see cref="Mouse.INPUT"/>構造体）の数
        /// </param>
        /// <param name="input">
        /// 入力操作の情報（<see cref="Mouse.INPUT"/>構造体）を格納した配列
        /// </param>
        /// <param name="size">
        /// 入力操作の情報（<see cref="Mouse.INPUT"/>構造体）のサイズ
        /// （<see cref="Mouse.INPUT"/>構造体）１つ分のサイズ）
        /// </param>
        /// <returns>
        /// キーボード又はマウス入力ストリームへ入力することができたイベントの数を返却
        /// 他のスレッドによって入力ストリームがブロックされており入力できなかった場合は 0 を返却
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern uint SendInput(uint count, Mouse.INPUT[] input, int size);

        /// <summary>
        /// キーボードの操作を行う
        /// </summary>
        /// <param name="count">
        /// 入力操作の情報（<see cref="Keyboard.INPUT"/>構造体）の数
        /// </param>
        /// <param name="input">
        /// 入力操作の情報（<see cref="Keyboard.INPUT"/>構造体）を格納した配列
        /// </param>
        /// <param name="size">
        /// 入力操作の情報（<see cref="Keyboard.INPUT"/>構造体）のサイズ
        /// （<see cref="Keyboard.INPUT"/>構造体）１つ分のサイズ）
        /// </param>
        /// <returns>
        /// キーボード又はマウス入力ストリームへ入力することができたイベントの数を返却
        /// 他のスレッドによって入力ストリームがブロックされており入力できなかった場合は 0 を返却
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern uint SendInput(uint count, Keyboard.INPUT[] input, int size);

        /// <summary>
        /// マウス、キーボード以外の入力デバイスの操作を行う
        /// </summary>
        /// <param name="count">
        /// 入力操作の情報（<see cref="Hardware.INPUT"/>構造体）の数
        /// </param>
        /// <param name="input">
        /// 入力操作の情報（<see cref="Hardware.INPUT"/>構造体）を格納した配列
        /// </param>
        /// <param name="size">
        /// 入力操作の情報（<see cref="Hardware.INPUT"/>構造体）のサイズ
        /// （<see cref="Hardware.INPUT"/>構造体）１つ分のサイズ）
        /// </param>
        /// <returns>
        /// キーボード又はマウス入力ストリームへ入力することができたイベントの数を返却
        /// 他のスレッドによって入力ストリームがブロックされており入力できなかった場合は 0 を返却
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern uint SendInput(uint count, Hardware.INPUT[] input, int size);

        #endregion

        #region SendInputメソッドで使用するパラメータの定義

        /// <summary>
        /// SendInput メソッドで使用するパラメータの定義
        /// </summary>
        internal static class SendInputParameter
        {
            /// <summary>
            /// SendInput の実行が成功したか判定する
            /// </summary>
            /// <remarks>
            /// SendInput の場合、入力が成功した数を返却、失敗時は 0 を返却する
            /// </remarks>
            /// <param name="result">SendInput を実行した際の戻り値を指定</param>
            /// <returns>正常終了：True、異常終了：False</returns>
            public static bool IsSuccess(uint result)
            {
                return result > 0;
            }
        }

        #endregion
    }
}
