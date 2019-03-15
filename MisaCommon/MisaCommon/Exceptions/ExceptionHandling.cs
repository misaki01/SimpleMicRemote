namespace MisaCommon.Exceptions
{
    using System;
    using System.Globalization;
    using System.Text;

    using MisaCommon.MessageResources;
    using MisaCommon.Utility.StaticMethod;

    /// <summary>
    /// エラーに対する共通的な処理を行う
    /// </summary>
    public static class ExceptionHandling
    {
        #region メソッド

        #region 共通のエラーメッセージを取得

        /// <summary>
        /// 共通のエラーメッセージを取得する
        /// </summary>
        /// <returns>
        /// 処理名無しの共通エラーメッセージ
        /// </returns>
        public static string GetCommonErrorMessage()
        {
            return GetCommonErrorMessage(null);
        }

        /// <summary>
        /// 共通のエラーメッセージを取得する
        /// </summary>
        /// <param name="processName">処理名</param>
        /// <returns>
        /// 引数の処理名（<paramref name="processName"/>）を指定した場合は、処理名付きの共通エラーメッセージ
        /// 引数の処理名（<paramref name="processName"/>）がNULLの場合は処理名無しの共通エラーメッセージ
        /// </returns>
        public static string GetCommonErrorMessage(string processName)
        {
            return processName == null
                ? ErrorMessage.ErrorMessageFormatCommon
                : string.Format(CultureInfo.InvariantCulture, ErrorMessage.ErrorMessageFormatCommonWithProcessName, processName);
        }

        /// <summary>
        /// エラーメッセージを取得する
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="ex"/> がNULLの場合に発生
        /// </exception>
        /// <returns>エラーメッセージ</returns>
        public static string GetErrorMessage(Exception ex)
        {
            return GetErrorMessage(ex, null);
        }

        /// <summary>
        /// エラーメッセージを取得する
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="ex"/> がNULLの場合に発生
        /// </exception>
        /// <returns>エラーメッセージを返却</returns>
        public static string GetErrorMessage(Exception ex, string message)
        {
            // NULLチェック
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            // メッセージの有無で返却するメッセージを変える
            if (message == null)
            {
                return ex.Message;
            }
            else
            {
                return string.Format(
                    CultureInfo.InvariantCulture, ErrorMessage.ErrorMessageFormat, message, ex.Message);
            }
        }

        /// <summary>
        /// スタックトレース付きのエラーメッセージを取得する
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="ex"/> がNULLの場合に発生
        /// </exception>
        /// <returns>スタックトレース付きエラーメッセージ</returns>
        public static string GetErrorMessageWithStackTrace(Exception ex)
        {
            return GetErrorMessageWithStackTrace(ex, null);
        }

        /// <summary>
        /// スタックトレース付きのエラーメッセージを取得する
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="ex"/> がNULLの場合に発生
        /// </exception>
        /// <returns>スタックトレース付きエラーメッセージ</returns>
        public static string GetErrorMessageWithStackTrace(Exception ex, string message)
        {
            // NULLチェック
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            // 基本となるエラーメッセージを設定
            StringBuilder errorMessage = new StringBuilder();
            errorMessage.Append(message ?? ErrorMessage.ErrorMessageFormatCommon);

            // 再起処理にて内部例外のスタックトレース情報を追加していく
            AddStackTrace(ex);
            void AddStackTrace(Exception exception)
            {
                errorMessage.Append(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        ErrorMessage.ErrorMessageFormatWithStackTrace,
                        exception.Message ?? "[null]",
                        exception.StackTrace ?? "[null]"));
                if (exception.InnerException != null)
                {
                    AddStackTrace(exception.InnerException);
                }

                return;
            }

            // エラーメッセージを返却
            return errorMessage.ToString();
        }

        #endregion

        #region ワーニング発生時の処理

        /// <summary>
        /// ワーニング発生時の処理をする
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void Warning(Exception ex)
        {
            Warning(ex, null);
        }

        /// <summary>
        /// ワーニング発生時の処理をする
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void Warning(Exception ex, string message)
        {
            // メッセージを表示する
            MessageBox.ShowWarning(GetErrorMessage(ex, message));
        }

        #endregion

        #region エラー発生時の処理

        /// <summary>
        /// エラー発生時の処理をする
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void Error(Exception ex)
        {
            Error(ex, null);
        }

        /// <summary>
        /// エラー発生時の処理をする
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void Error(Exception ex, string message)
        {
            // メッセージを表示する
            MessageBox.ShowError(GetErrorMessageWithStackTrace(ex, message));
        }

        #endregion

        #region クリティカルエラー発生時の処理

        /// <summary>
        /// クリティカルエラー発生時の処理をする
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void CriticalError(Exception ex)
        {
            CriticalError(ex, null);
        }

        /// <summary>
        /// クリティカルエラー発生時の処理をする
        /// </summary>
        /// <param name="ex">発生した例外</param>
        /// <param name="message">エラーメッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void CriticalError(Exception ex, string message)
        {
            // メッセージを表示する
            try
            {
                MessageBox.ShowCriticalError(GetErrorMessageWithStackTrace(ex, message));
            }
            finally
            {
                // アプリケーションを終了する
                // 必ず実行する必要があるためtry-finallyで実行
                System.Windows.Forms.Application.ExitThread();
                Environment.Exit(1);
            }
        }

        #endregion

        #endregion
    }
}