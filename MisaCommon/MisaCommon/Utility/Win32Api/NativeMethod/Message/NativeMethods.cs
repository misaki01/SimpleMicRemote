namespace MisaCommon.Utility.Win32Api.NativeMethod.Message
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// 【注意：このクラスのメソッドは直接呼び出さず、<see cref="MessageOperate"/> クラス等のラッパークラスをを経由して呼び出すこと】
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
        #region ウインドウにメッセージの送信

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウに、
        /// 引数で指定したメッセージを送信する
        /// </summary>
        /// <param name="windowHandle">送信対象のウィンドウのハンドル</param>
        /// <param name="message">送信するべきメッセージを指定</param>
        /// <param name="wParam">メッセージ特有の１番目の追加情報を指定</param>
        /// <param name="lParam">メッセージ特有の２番目の追加情報を指定</param>
        /// <param name="optionFlag">メッセージの送信方法を指定するフラグ</param>
        /// <param name="timeoutTime">タイムアウトまでの時間を指定（ミリ秒単位）</param>
        /// <param name="messageResult">関数から制御が返った際に設定される、メッセージ処理の結果（送信されたメッセージにより異なる値となる）</param>
        /// <returns>
        /// 正常終了：0以外の値、異常終了：0
        /// 異常終了：0 かつ、GetLastErrorの結果が 0 の場合はタイムアウトを意味する
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr SendMessageTimeout(IntPtr windowHandle, int message, IntPtr wParam, IntPtr lParam, uint optionFlag, uint timeoutTime, out IntPtr messageResult);

        #endregion

        #region SendMessageTimeoutメソッドで使用するパラメータの定義

        /// <summary>
        /// <see cref="SendMessageTimeout"/> メソッドで使用するパラメータの定義
        /// </summary>
        internal static class SendMessageTimeoutParameter
        {
            /// <summary>
            /// タイムアウトまでの時間を指定（ミリ秒単位）
            /// </summary>
            public const uint TimeoutTime = 5000;

            /// <summary>
            /// ウィンドウのサイズと位置の変更に関するオプションのビットフラグ
            /// </summary>
            [Flags]
            public enum OptionFlag : uint
            {
                /// <summary>
                /// 処理中に呼び出し側スレッドに対する要求をブロックしない
                /// </summary>
                SMTO_NORMAL = 0x0000,

                /// <summary>
                /// 処理中は呼び出し側スレッドに対する要求をブロックする
                /// </summary>
                SMTO_BLOCK = 0x0001,

                /// <summary>
                /// 受信側のスレッドがハングアップ状態であると判断した場合、タイムアウト期間の経過を待たずに中断する
                /// 受信側のスレッドが5秒以内に GetMessage 等の関数で処理を行わなかった場合にハングアップと判定する
                /// </summary>
                SMTO_ABORTIFHUNG = 0x0002,

                /// <summary>
                /// Windows 2000/XP：受信側スレッドがハングアップしていない場合、タイムアウト期間が経過しても中断しない
                /// </summary>
                SMTO_NOTIMEOUTIFNOTHUNG = 0x0008,
            }

            /// <summary>
            /// <see cref="SendMessageTimeout"/> の実行が成功したか判定する
            /// </summary>
            /// <remarks>
            /// <see cref="SendMessageTimeout"/> の場合、正常時には 0以外の値を返却、それ以外の場合は 0 を返却する
            /// </remarks>
            /// <param name="lResult"><see cref="SendMessageTimeout"/> を実行した際の戻り値を指定</param>
            /// <returns>正常終了：True、異常終了：False</returns>
            public static bool IsSuccess(IntPtr lResult)
            {
                return lResult.ToInt32() != 0;
            }

            /// <summary>
            /// 引数からタイムアウトエラーに関するものか判定する
            /// </summary>
            /// <param name="result"><see cref="SendMessageTimeout"/> の実行の成否</param>
            /// <param name="errorCode">判定対象のエラーコード</param>
            /// <returns>タイムアウトに関するエラーコードの場合：True、それ以外の場合：False</returns>
            public static bool IsTimeoutError(bool result, int errorCode)
            {
                bool checkResult = false;
                if (!result)
                {
                    switch (errorCode)
                    {
                        // 実行結果が失敗かつ下記のエラーコードの場合、タイムアウトに関するエラーと判定する
                        case (int)ErrorCode.NO_ERROR:
                        case (int)ErrorCode.ERROR_TIMEOUT:
                            checkResult = true;
                            break;
                        default:
                            checkResult = false;
                            break;
                    }
                }

                // 判定結果を返却
                return checkResult;
            }
        }

        #endregion

        #region WM_CLOSE：ウィンドウを閉じるコマンド

        /// <summary>
        /// ウィンドウを閉じるコマンド
        /// </summary>
        internal static class WM_CLOSE
        {
            /// <summary>
            /// メッセージに設定する値（<see cref="WM_CLOSE"/> の値）
            /// </summary>
            public const int MESSAGE = 0x0010;

            /// <summary>
            /// wParamに設定する値
            /// </summary>
            public static IntPtr WPARAM => IntPtr.Zero;

            /// <summary>
            /// lParamに設定する値
            /// </summary>
            public static IntPtr LPARAM => IntPtr.Zero;

            /// <summary>
            /// <see cref="WM_CLOSE"/> の実行が成功したか判定する
            /// </summary>
            /// <remarks>
            /// <see cref="WM_CLOSE"/> の場合、正常時には 0 を返却、それ以外の場合は 0 以外の値を返却する
            /// </remarks>
            /// <param name="lResult"><see cref="WM_CLOSE"/> を実行した際の戻り値を指定</param>
            /// <returns>正常終了：True、異常終了：False</returns>
            public static bool IsSuccess(IntPtr lResult)
            {
                return lResult.ToInt32() == 0;
            }
        }

        #endregion
    }
}
