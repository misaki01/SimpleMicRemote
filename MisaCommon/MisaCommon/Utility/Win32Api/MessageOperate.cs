namespace MisaCommon.Utility.Win32Api
{
    using System;
    using System.Runtime.InteropServices;

    using MisaCommon.Exceptions;
    using MisaCommon.MessageResources;
    using MisaCommon.Utility.Win32Api.NativeMethod;

    using Win32Api = NativeMethod.Message.NativeMethods;

    /// <summary>
    /// Win32APIの機能を使用してメッセージに関する操作を行うクラス
    /// </summary>
    public static class MessageOperate
    {
        #region ウィンドウに対してメッセージを送信し処理を行う

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウに、
        /// 引数で指定したメッセージを送信する
        /// </summary>
        /// <param name="windowHandle">
        /// 送信対象のウィンドウのハンドル
        /// </param>
        /// <param name="message">
        /// 送信するべきメッセージを指定
        /// </param>
        /// <param name="wParam">
        /// メッセージ特有の１番目の追加情報を指定
        /// </param>
        /// <param name="lParam">
        /// メッセージ特有の２番目の追加情報を指定
        /// </param>
        /// <param name="isSucessFunc">
        /// 指定したメッセージの戻り値に対する成功失敗を判定する処理を指定
        /// 引数1：指定したメッセージの戻り値
        /// 戻り値：判定結果、成功：True、失敗：False
        /// </param>
        /// <param name="isThrowExceptionCloseFail">
        /// ウィンドウを閉じる処理に失敗した場合に例外を発生させるかどうかを指定するフラグ
        /// 閉じるだけの処理であるため失敗しても良いと判断される場合は False を設定、
        /// 閉じる処理の成功の保証がいる場合は Ture を設定
        /// 失敗するパターンとしては下記のパターンが考えられる
        /// ・メモ帳等を閉じる際に表示される「保存しますか？」のダイアログが表示され、
        /// 　待機となりタイムアウトが発生した場合
        /// </param>
        /// <param name="isExcludeTimeoutExceptions">
        /// 発生させる例外のうち、タイムアウトに関する例外は除外するかどうかを指定するフラグ
        /// 引数 <paramref name="isExcludeTimeoutExceptions"/> が True の場合のみ有効
        /// このフラグを True した場合、タイムアウトに関するもの以外の例外のみが発生する
        /// このフラグを False にした場合、タイムアウトに関するものも含めて例外が発生する
        /// 保存の確認ダイアログ等の待ちでタイムアウトが発生する場合があるため、
        /// その際に例外を発生させるかどうかの制御として使用
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        public static void SendMessage(
            HandleRef windowHandle,
            int message,
            IntPtr wParam,
            IntPtr lParam,
            Func<IntPtr, bool> isSucessFunc,
            bool isThrowExceptionCloseFail,
            bool isExcludeTimeoutExceptions)
        {
            // 対象のウィンドウが存在しない場合は処理を終了する
            if (!WindowOperate.IsWindow(windowHandle))
            {
                return;
            }

            // オプション及びタイムアウト値の設定
            Win32Api.SendMessageTimeoutParameter.OptionFlag flag
                = Win32Api.SendMessageTimeoutParameter.OptionFlag.SMTO_NORMAL
                    | Win32Api.SendMessageTimeoutParameter.OptionFlag.SMTO_ABORTIFHUNG;
            uint optionFlag = (uint)flag;
            uint timeout = Win32Api.SendMessageTimeoutParameter.TimeoutTime;

            // Win32Apiの実行処理
            // Win32ApiのMessage共通の呼び出し機能を用いて、ウィンドウへのメッセージ送信処理を呼び出す
            Win32ApiResult function()
            {
                IntPtr tmpResult = Win32Api.SendMessageTimeout(
                    windowHandle: windowHandle,
                    message: message,
                    wParam: wParam,
                    lParam: lParam,
                    optionFlag: optionFlag,
                    timeoutTime: timeout,
                    messageResult: out IntPtr messageResult);
                int win32ErrorCode = Marshal.GetLastWin32Error();
                bool win32Result = Win32Api.SendMessageTimeoutParameter.IsSuccess(tmpResult);
                win32Result &= isSucessFunc(messageResult);

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.SendMessageTimeout);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 例外を発生させる場合のみ、正常終了したかチェックを行う
            if (isThrowExceptionCloseFail && !result.Result)
            {
                // タイムアウトに関する例外は判定
                if (Win32Api.SendMessageTimeoutParameter.IsTimeoutError(result.Result, result.ErrorCode))
                {
                    // タイムアウトに関連する例外の場合
                    // タイムアウトに関する例外が除外されていない場合は例外をスローする
                    if (!isExcludeTimeoutExceptions)
                    {
                        // 送信先のスレッドがハングアップした場合、エラーコードが 0：正常終了となる
                        // その場合とそれ以外に分けて例外を生成する
                        Win32OperateException ex;
                        if (result.ErrorCode == (int)ErrorCode.NO_ERROR)
                        {
                            string timeoutMessage = ErrorMessage.Win32OperateErrorTimeout;
                            ex = Win32ApiCommon.GetWin32OperateException(
                                dllName, methodName, timeoutMessage);
                        }
                        else
                        {
                            ex = Win32ApiCommon.GetWin32OperateException(
                                dllName, methodName, result.ErrorCode);
                        }

                        // タイムアウトに関する例外をスロー
                        throw ex;
                    }
                }
                else
                {
                    // タイムアウトエラー以外の例外をスロー
                    throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
                }
            }
        }

        #endregion
    }
}
