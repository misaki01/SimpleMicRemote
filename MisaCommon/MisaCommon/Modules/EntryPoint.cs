namespace MisaCommon.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;

    using MisaCommon.Exceptions;
    using MisaCommon.MessageResources;
    using Utility = Utility.StaticMethod;

    /// <summary>
    /// アプリケーションのエントリポイント
    /// </summary>
    public static class EntryPoint
    {
        #region クラス変数・定数

        /// <summary>
        /// リソースを解放するクラスのリスト
        /// ローカルHTTPサーバのポート開放や非同期処理の停止等で使用する
        /// </summary>
        /// <remarks>
        /// シングルトンパターンで実装
        /// </remarks>
        private static IList<IDisposable> _disposeClass = null;

        #endregion

        #region プロパティ

        /// <summary>
        /// リソースを解放するクラスのリストを取得する
        /// ローカルHTTPサーバのポート開放や非同期処理の停止等で使用する
        /// </summary>
        /// <remarks>
        /// シングルトンパターンで実装
        /// </remarks>
        public static IList<IDisposable> DisposeClass
        {
            get
            {
                // シングルトンの処理、NULLならオブジェクトを生成する
                if (_disposeClass == null)
                {
                    _disposeClass = new List<IDisposable>();
                }

                return _disposeClass;
            }
        }

        /// <summary>
        /// 言語を変更する場合に設定する変更後の言語を取得・設定する
        /// このプロパティに言語を設定した場合は指定した言語モードで再起動する
        /// （変更しない場合はNULL）
        /// </summary>
        public static CultureInfo ChangeCulture { get; set; } = null;

        #endregion

        #region メソッド：アプリケーションのスタート

        /// <summary>
        /// アプリケーションのスタート
        /// </summary>
        /// <remarks>
        /// このメソッドにて使用する言語の設定を行っている
        /// この言語の設定を最初に開く <see cref="Form"/> クラスを new よりも前に実行したいため
        /// <paramref name="newForm"/> は newを実行する <see cref="Func{TResult}"/> で受け取るようにしている
        /// </remarks>
        /// <param name="newForm">最初に開く <see cref="Form"/> クラスを new する処理</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="newForm"/> がNULLの場合に発生
        /// </exception>
        [STAThread]
        public static void ApplicationStart(Func<Form> newForm)
        {
            ApplicationStart(newForm, null, false, null);
        }

        /// <summary>
        /// アプリケーションのスタート
        /// </summary>
        /// <remarks>
        /// このメソッドにて使用する言語の設定を行っている
        /// この言語の設定を最初に開く <see cref="Form"/> クラスを new よりも前に実行したいため
        /// <paramref name="newForm"/> は newを実行する <see cref="Func{TResult}"/> で受け取るようにしている
        /// </remarks>
        /// <param name="newForm">最初に開く <see cref="Form"/> クラスを new する処理</param>
        /// <param name="culture">アプリケーションの言語の指定（未指定の場合はOSデフォルトの言語で起動）</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="newForm"/> がNULLの場合に発生
        /// </exception>
        [STAThread]
        public static void ApplicationStart(Func<Form> newForm, CultureInfo culture)
        {
            ApplicationStart(newForm, culture, false, null);
        }

        /// <summary>
        /// アプリケーションのスタート
        /// </summary>
        /// <remarks>
        /// このメソッドにて使用する言語の設定を行っている
        /// この言語の設定を最初に開く <see cref="Form"/> クラスを new よりも前に実行したいため
        /// <paramref name="newForm"/> は newを実行する <see cref="Func{TResult}"/> で受け取るようにしている
        /// </remarks>
        /// <param name="newForm">最初に開く <see cref="Form"/> クラスを new する処理</param>
        /// <param name="isSingle">二重起動をさせない場合：True、二重起動の制御を行わない場合はFalse</param>
        /// <param name="mutexName">二重起動を防止するために使用するアプリケーション固有の名称</param>
        /// <exception cref="ArgumentNullException">
        /// 下記の場合に発生する
        /// ・引数の <paramref name="newForm"/> がNULLの場合
        /// ・二重起動防止を行う場合（引数の<paramref name="isSingle"/>がTrue）かつ、
        /// 　引数の <paramref name="mutexName"/> がNULL又は空文字の場合
        /// </exception>
        [STAThread]
        public static void ApplicationStart(Func<Form> newForm, bool isSingle, string mutexName)
        {
            ApplicationStart(newForm, null, isSingle, mutexName);
        }

        /// <summary>
        /// アプリケーションのスタート
        /// </summary>
        /// <remarks>
        /// このメソッドにて使用する言語の設定を行っている
        /// この言語の設定を最初に開く <see cref="Form"/> クラスを new よりも前に実行したいため
        /// <paramref name="newForm"/> は newを実行する <see cref="Func{TResult}"/> で受け取るようにしている
        /// </remarks>
        /// <param name="newForm">最初に開く <see cref="Form"/> クラスを new する処理</param>
        /// <param name="culture">アプリケーションの言語の指定（未指定の場合はOSデフォルトの言語で起動）</param>
        /// <param name="isSingle">二重起動をさせない場合：True、二重起動の制御を行わない場合はFalse</param>
        /// <param name="mutexName">二重起動を防止するために使用するアプリケーション固有の名称</param>
        /// <exception cref="ArgumentNullException">
        /// 下記の場合に発生する
        /// ・引数の <paramref name="newForm"/> がNULLの場合
        /// ・二重起動防止を行う場合（引数の<paramref name="isSingle"/>がTrue）かつ、
        /// 　引数の <paramref name="mutexName"/> がNULL又は空文字の場合
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 二重起動を防止する場合において、捨てに引数（<paramref name="mutexName"/>）で指定した名前のMutexが、
        /// アクセス制御セキュリティで作成されている場合において、
        /// ユーザに <see cref="System.Security.AccessControl.MutexRights.FullControl"/> の権限がない場合に発生
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// 二重起動を防止する場合において、Win32 エラーが発生した場合に発生
        /// </exception>
        /// <exception cref="WaitHandleCannotBeOpenedException">
        /// 二重起動を防止する場合において、名前付きミューテックスを作成できない場合に発生
        /// 原因として、別の型の待機ハンドルに同じ名前が付けられていることが考える
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 二重起動を防止する場合において、引数（<paramref name="mutexName"/>）が260文字を超えている場合に発生
        /// </exception>
        [STAThread]
        public static void ApplicationStart(Func<Form> newForm, CultureInfo culture, bool isSingle, string mutexName)
        {
            // catchされなかった例外を処理するためのイベントハンドラを追加

            // ThreadExceptionのイベントハンドラ（UIスレッドでcatchされなかった例外用）
            Application.ThreadException
                += new ThreadExceptionEventHandler(Handling_ThreadException);

            // UnhandledExceptionのイベントハンドラ（UIスレッド以外のでcatchされなかった例外用）
            Thread.GetDomain().UnhandledException
                += new UnhandledExceptionEventHandler(Handling_UnhandledException);

            // 言語の設定
            if (culture != null)
            {
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            // NULLチェック
            if (newForm == null)
            {
                throw new ArgumentNullException(nameof(newForm));
            }
            else if (isSingle && string.IsNullOrWhiteSpace(mutexName))
            {
                throw new ArgumentNullException(nameof(mutexName));
            }

            // アプリケーションの実行
            bool hasMutexOwnership = false;
            Mutex mutex = null;
            try
            {
                // 二重起動防止用のMutexオブジェクトを所有権を取得する形式で生成
                // 二重起動を防止しない場合は生成しない
                if (isSingle)
                {
                    mutex = new Mutex(true, mutexName, out hasMutexOwnership);
                }

                // 二重起動を防止する場合かつ、Mutexの所有権が得られない場合は既に起動済みと判断し終了する
                if (isSingle && !hasMutexOwnership)
                {
                    Utility.MessageBox.ShowAttention(ErrorMessage.DoubleStartupImpossibleMessage);
                    return;
                }

                while (true)
                {
                    // 言語の変更プロパティをリセット
                    ChangeCulture = null;

                    // Formの起動処理
                    Application.Run(newForm());

                    // 言語の変更プロパティが設定されているか判定
                    if (ChangeCulture == null)
                    {
                        // 言語の変更でない場合は、終了する
                        break;
                    }
                    else
                    {
                        // 言語の変更でない場合は、変更後の言語を設定し再起動する
                        Thread.CurrentThread.CurrentUICulture = ChangeCulture;
                    }
                }
            }
            finally
            {
                // リソースの解放処理を行う
                try
                {
                    // 非同期処理の停止等のため登録されてるDispose対象のクラスについてDisposeを行う
                    for (int i = 0; i < DisposeClass.Count; i++)
                    {
                        DisposeClass[i].Dispose();
                    }

                    // Dispose対象のクラスを格納している配列を解放
                    DisposeClass.Clear();

                    // Mutexの所有権を保持している場合、所有権を解放する
                    if (hasMutexOwnership)
                    {
                        mutex?.ReleaseMutex();
                    }
                }
                catch (Exception ex)
                    when (ex is ApplicationException
                        || ex is ObjectDisposedException)
                {
                    // 下記のエラーの場合はMutexオブジェ区の破棄が完了しているためなにもしない
                    // [ApplicationException]
                    // ・呼び出しスレッドに独自のミューテックスが存在しない場合に発生
                    // 下記の
                    // [ObjectDisposedException]
                    // ・Mutexオブジェクトが既に破棄されている場合に発生
                }
                finally
                {
                    // Mutexオブジェクトを解放する
                    mutex?.Close();
                }
            }
        }

        #endregion

        #region イベントで呼び出されるメソッド：catchされなかった例外に対する処理

        /// <summary>
        /// UIスレッドでcatchされなかった例外に対する処理を行う
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="ex">UIスレッドでcatchされなかった例外データ</param>
        private static void Handling_ThreadException(object sender, ThreadExceptionEventArgs ex)
        {
            if (ex != null)
            {
                ExceptionHandling.CriticalError(ex.Exception);
            }
            else
            {
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// UIスレッド以外のスレッドでcatchされなかった例外に対する処理を行う
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="ex">UIスレッド以外のスレッドでcatchされなかった例外データ</param>
        private static void Handling_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            if (ex != null && ex.ExceptionObject is Exception exception)
            {
                ExceptionHandling.CriticalError(exception);
            }
            else
            {
                Environment.Exit(1);
            }
        }

        #endregion
    }
}
