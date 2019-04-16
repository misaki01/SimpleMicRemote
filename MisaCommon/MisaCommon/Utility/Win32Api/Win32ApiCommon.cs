namespace MisaCommon.Utility.Win32Api
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using MisaCommon.Exceptions;
    using MisaCommon.MessageResources;

    /// <summary>
    /// Win32Apiに関する共通処理を行うクラス
    /// </summary>
    public static class Win32ApiCommon
    {
        #region Win32Apiの実行

        /// <summary>
        /// 引数 <paramref name="runFunction"/> で指定されたWin32Apiの実行処理を実行する
        /// </summary>
        /// <param name="runFunction">Win32Apiの実行処理</param>
        /// <param name="dllName">実行するWin32Apiが使用しているDLLの名称</param>
        /// <param name="methodName">実行するWin32Apiのメソッド名</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="runFunction"/> がNULLの場合に発生する
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの内部処理で例外が発生した場合に発生
        /// </exception>
        /// <returns>実行したWin32Apiの実行結果を返却</returns>
        public static Win32ApiResult Run(
            Func<Win32ApiResult> runFunction, string dllName, string methodName)
        {
            try
            {
                // NULLチェック
                if (runFunction == null)
                {
                    throw new ArgumentNullException(nameof(runFunction));
                }

                // 実行
                return runFunction();
            }
            catch (RuntimeWrappedException ex)
            {
                // メソッド内部から例外がスローされた場合、Win32OperateExceptionにラップしてスロー
                throw GetWin32OperateException(dllName, methodName, ex);
            }
            catch (TypeLoadException ex)
            {
                // メソッドの呼び出しにおいて例外がスローされた場合、
                // PlatformInvokeExceptionにラップしスロー
                string format = ErrorMessage.Win32OperateErrorFailDllImportFormat;
                string message = string.Format(CultureInfo.InvariantCulture, format, dllName, methodName);
                throw new PlatformInvokeException(message, ex);
            }
        }

        #endregion

        #region Win32OperateExceptionの取得

        /// <summary>
        /// <see cref="Win32OperateException"/> を取得する
        /// Win32Apiの実行後、実行結果が異常終了の場合にスローする例外で使用する
        /// </summary>
        /// <param name="dllName">実行したWin32Apiが使用しているDLLの名称</param>
        /// <param name="methodName">実行したWin32Apiのメソッド名</param>
        /// <returns><see cref="Win32OperateException"/> オブジェクト</returns>
        public static Win32OperateException GetWin32OperateException(
            string dllName, string methodName)
        {
            string format = ErrorMessage.Win32OperateErrorFormat;
            string message = string.Format(CultureInfo.InvariantCulture, format, dllName, methodName);
            return new Win32OperateException(message, null);
        }

        /// <summary>
        /// <see cref="Win32OperateException"/> を取得する
        /// Win32Apiの実行後、実行結果が異常終了の場合にスローする例外で使用する
        /// </summary>
        /// <param name="dllName">実行したWin32Apiが使用しているDLLの名称</param>
        /// <param name="methodName">実行したWin32Apiのメソッド名</param>
        /// <param name="ex">内部例外</param>
        /// <returns><see cref="Win32OperateException"/> オブジェクト</returns>
        public static Win32OperateException GetWin32OperateException(
            string dllName, string methodName, Exception ex)
        {
            string message;
            if (ex is RuntimeWrappedException)
            {
                // 発生した例外が RuntimeWrappedException の場合、それ用のメッセージを生成する
                string format = ErrorMessage.Win32OperateErrorRuntimeWrappedExceptionFormat;
                message = string.Format(CultureInfo.InvariantCulture, format, dllName, methodName);
            }
            else
            {
                // 上記以外の場合、汎用のメッセージを生成する
                string format = ErrorMessage.Win32OperateErrorFormat;
                message = string.Format(CultureInfo.InvariantCulture, format, dllName, methodName);
            }

            return new Win32OperateException(message, ex);
        }

        /// <summary>
        /// エラーコード付きの <see cref="Win32OperateException"/> を取得する
        /// Win32Apiの実行後、実行結果が異常終了の場合にスローする例外で使用する
        /// </summary>
        /// <param name="dllName">実行したWin32Apiが使用しているDLLの名称</param>
        /// <param name="methodName">実行したWin32Apiのメソッド名</param>
        /// <param name="errorCode">実行したWin32Apiのエラーコード</param>
        /// <returns>エラーコード付きの <see cref="Win32OperateException"/> オブジェクト</returns>
        public static Win32OperateException GetWin32OperateException(
            string dllName, string methodName, int errorCode)
        {
            Win32Exception ex = new Win32Exception(errorCode);
            string format = ErrorMessage.Win32OperateErrorFormatWithErrorCode;
            string message = string.Format(
                CultureInfo.InvariantCulture, format, dllName, methodName, ex.Message);
            return new Win32OperateException(message, ex);
        }

        /// <summary>
        /// エラーメッセージ付きの <see cref="Win32OperateException"/> を取得する
        /// Win32Apiの実行後、実行結果が異常終了の場合にスローする例外で使用する
        /// </summary>
        /// <param name="dllName">実行したWin32Apiが使用しているDLLの名称</param>
        /// <param name="methodName">実行したWin32Apiのメソッド名</param>
        /// <param name="errorMessage"><see cref="Win32OperateException"/> に追加するエラーメッセージ</param>
        /// <returns>エラーコード付きの <see cref="Win32OperateException"/> オブジェクト</returns>
        public static Win32OperateException GetWin32OperateException(
            string dllName, string methodName, string errorMessage)
        {
            string format = ErrorMessage.Win32OperateErrorFormatWithErrorCode;
            string message = string.Format(
                CultureInfo.InvariantCulture, format, dllName, methodName, errorMessage);
            return new Win32OperateException(message);
        }

        #endregion
    }
}
