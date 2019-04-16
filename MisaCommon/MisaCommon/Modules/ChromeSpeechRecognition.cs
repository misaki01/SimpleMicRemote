namespace MisaCommon.Modules
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Security;

    using Microsoft.Win32;
    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.MessageResources;
    using MisaCommon.Network.Http;
    using MisaCommon.Utility.Win32Api;

    #region Chromeが使用可能かどうかの状態を表すEnum定義

    /// <summary>
    /// Chromeが使用可能かどうかの状態を表すEnum定義
    /// </summary>
    public enum ChromeState
    {
        /// <summary>
        /// 使用可能
        /// </summary>
        CanUse,

        /// <summary>
        /// 使用不可：インストールされていない
        /// </summary>
        NoInstall,

        /// <summary>
        /// 使用不可：<see cref="ChromeSpeechRecognition.ChromePath"/>で指定したパスが間違っている
        /// </summary>
        ChromePathIsWrong,
    }

    #endregion

    /// <summary>
    /// Chromeブラウザを使用した音声認識の処理を行うクラス
    /// </summary>
    public static class ChromeSpeechRecognition
    {
        #region クラス変数・定数

        /// <summary>
        /// Chromeのインストールパスを定義しているレジストリキー
        /// </summary>
        private const string ChromeRegistryKey
            = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";

        /// <summary>
        /// Chromeのインストールパスを定義しているレジストリの値の名称：（規定）
        /// </summary>
        private const string ChromeRegistryValueNameDefault = "";

        /// <summary>
        /// Chromeの起動時に指定する起動パラメータのフォーマット
        /// {0}：最初に開くページのURLを指定する
        /// </summary>
        /// <remarks>
        /// 起動パラメータの詳細説明
        /// ・ --disable-gpu
        /// 　ハードウェアアクセラレーションが使用可能な場合は使用するの設定をOFFにする
        /// 　ONの場合、配信でChromeの画面が映らない場合があることからの対策
        /// ・ --app="{0}"
        ///   アプリケーションモードでの起動　{0}は開くURLを指定
        /// </remarks>
        private const string ChromeStartUpParamFormat = " --disable-gpu --app=\"{0}\"";

        #endregion

        #region プロパティ

        /// <summary>
        /// ChromeのEXEファイルの絶対パスを取得・設定する
        /// （未設定の場合はNULL）
        /// </summary>
        public static string ChromePath { get; set; } = null;

        /// <summary>
        /// Chromeがインストールされているか判定を取得する
        /// Trueの場合は <see cref="ChromePath"/> にChrome.exeのパスを設定する
        /// </summary>
        /// <exception cref="SecurityException">
        /// レジストリキーの読み取りに必要な権限がない場合に発生
        /// </exception>
        /// <exception cref="IOException">
        /// <see cref="RegistryKey"/>を含む、指定された値が削除対象としてマークされている場合に発生
        /// </exception>
        public static ChromeState CanUseChrome
        {
            get
            {
                // プロパティ：ChromePathが設定されている場合とそうでない場合でそれぞれ判定を行う
                if (string.IsNullOrEmpty(ChromePath))
                {
                    // プロパティ：ChromePathが設定されていない場合

                    // レジストリからパスを取得し使用する
                    string path = ChromePathInRegistry;

                    // レジストリからパスが取得できない、又はそのパスのファイルが存在しない場合
                    // インストールされていないと判定する
                    if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    {
                        return ChromeState.NoInstall;
                    }

                    // インストールされている場合は取得したパスをプロパティ：ChromePathに設定する
                    ChromePath = path;
                }
                else
                {
                    // プロパティ：ChromePathが設定されている場合

                    // ChromePathが設定されているパスのファイルの存在チェックを行う
                    if (!File.Exists(ChromePath))
                    {
                        // ファイルが存在しない場合は、指定しているパスが間違っているを返却する
                        return ChromeState.ChromePathIsWrong;
                    }
                }

                // 全てのチェックがOKのため使用可能を返す
                return ChromeState.CanUse;
            }
        }

        /// <summary>
        /// Chromeによる音声認識で起動したローカルHttpサーバが使用しているポート番号を取得する
        /// （未起動の場合はNULL）
        /// </summary>
        public static int? UseLocalHttpServerPort => Server?.Url?.Port;

        /// <summary>
        /// 音声認識のFavicon（アイコン）を取得・設定する
        /// （使用しない場合はNULL）
        /// </summary>
        /// <remarks>
        /// 音声認識のために起動したChromeで表示するサイトのアイコン
        /// </remarks>
        public static Icon FaviconData { get; set; } = null;

        /// <summary>
        /// レジストリに登録されているChromeのEXEファイルの絶対パスを取得する
        /// （レジストリから取得できなかった場合はNULL）
        /// </summary>
        /// <exception cref="SecurityException">
        /// レジストリキーの読み取りに必要な権限がない場合に発生
        /// </exception>
        /// <exception cref="IOException">
        /// <see cref="RegistryKey"/>を含む、指定された値が削除対象としてマークされている場合に発生
        /// </exception>
        private static string ChromePathInRegistry
            => Registry.GetValue(ChromeRegistryKey, ChromeRegistryValueNameDefault, null) as string;

        /// <summary>
        /// このクラスで起動したローカルHttpサーバを取得・設定する
        /// （未起動の場合はNULL）
        /// </summary>
        private static LocalHttpServer Server { get; set; } = null;

        /// <summary>
        /// このクラスで起動したChromeのプロセス情報を紐づけるためのキーを取得・設定する
        /// （未起動の場合はNULL）
        /// </summary>
        private static long? ChromeProcessInfoKey { get; set; } = null;

        #endregion

        #region メソッド

        /// <summary>
        /// Chromeによる音声認識を開始する
        /// </summary>
        /// <param name="htmlFilePath">
        /// 音声認識に使用する既定の「音声認識用.html」ファイルのパス
        /// </param>
        /// <param name="postRegex">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するURLの正規表現
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="postParamName">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するPOSTデータのパラメータ名
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="sizePoint">
        /// 音声認識用に起動するChromeの初期のサイズと位置
        /// （NULLを指定した場合はデフォルトのサイズと位置で起動する）
        /// </param>
        /// <param name="responceFunctions">
        /// 音声認識した文字に対する処理の指定（複数指定可、優先順位は上から）
        /// ・引数1 　string：音声認識した文字列を渡す
        /// ・戻り値　byte[]：処理を実行した結果返却するデータ
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="htmlFilePath"/> 又は <paramref name="postRegex"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生する
        /// ・引数の <paramref name="htmlFilePath"/> が下記の場合
        /// 　長さ 0 の文字列
        /// 　空白のみで構成される
        /// 　<see cref="Path.InvalidPathChars"/> で定義される 1 つ以上の正しくない文字を含む
        /// ・引数の <paramref name="postRegex"/> が正規表現として不正な値の場合
        /// </exception>
        /// <exception cref="SpeechRecognitionException">
        /// Chromeが使用不可能な場合に発生
        /// （<see cref="ChromePath"/> の指定が間違ってる 又は、Chromeがインストールされていない場合）
        /// </exception>
        /// <exception cref="IOException">
        /// 下記の場合に発生
        /// ・引数の <paramref name="htmlFilePath"/> がシステム定義の最大長を超えている場合
        /// 　[<see cref="PathTooLongException"/>]
        /// 　（たとえば、Windowsでは、パスは 248文字未満、ファイル名は 260 文字未満である必要がある）
        /// ・引数の <paramref name="htmlFilePath"/> が存在しないディレクトリを示している場合
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// ・引数の <paramref name="htmlFilePath"/> で指定されたファイルが存在しない場合
        /// 　[<see cref="FileNotFoundException"/>]
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 引数の <paramref name="htmlFilePath"/> がファイルを指定しないない（ディレクトリを指定）場合、
        /// 又は、呼び出し元に必要なアクセス許可がない場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        public static void Start(
            string htmlFilePath,
            string postRegex,
            string postParamName,
            SizePoint sizePoint,
            params Func<string, byte[]>[] responceFunctions)
        {
            // 開始
            Start(htmlFilePath, postRegex, postParamName, sizePoint, null, responceFunctions);
        }

        /// <summary>
        /// Chromeによる音声認識を開始する
        /// </summary>
        /// <param name="chromePath">
        /// ChromeのEXEファイルの絶対パス
        /// 指定した値は <see cref="ChromePath"/> プロパティに設定する
        /// </param>
        /// <param name="htmlFilePath">
        /// 音声認識に使用する既定の「音声認識用.html」ファイルのパス
        /// </param>
        /// <param name="postRegex">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するURLの正規表現
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="postParamName">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するPOSTデータのパラメータ名
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="sizePoint">
        /// 音声認識用に起動するChromeの初期のサイズと位置
        /// （NULLを指定した場合はデフォルトのサイズと位置で起動する）
        /// </param>
        /// <param name="responceFunctions">
        /// 音声認識した文字に対する処理の指定（複数指定可、優先順位は上から）
        /// ・引数1 　string：音声認識した文字列を渡す
        /// ・戻り値　byte[]：処理を実行した結果返却するデータ
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="htmlFilePath"/> 又は <paramref name="postRegex"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生する
        /// ・引数の <paramref name="htmlFilePath"/> が下記の場合
        /// 　長さ 0 の文字列
        /// 　空白のみで構成される
        /// 　<see cref="Path.InvalidPathChars"/> で定義される 1 つ以上の正しくない文字を含む
        /// ・引数の <paramref name="postRegex"/> が正規表現として不正な値の場合
        /// </exception>
        /// <exception cref="SpeechRecognitionException">
        /// Chromeが使用不可能な場合に発生
        /// （<see cref="ChromePath"/> の指定が間違ってる 又は、Chromeがインストールされていない場合）
        /// </exception>
        /// <exception cref="IOException">
        /// 下記の場合に発生
        /// ・引数の <paramref name="htmlFilePath"/> がシステム定義の最大長を超えている場合
        /// 　[<see cref="PathTooLongException"/>]
        /// 　（たとえば、Windowsでは、パスは 248文字未満、ファイル名は 260 文字未満である必要がある）
        /// ・引数の <paramref name="htmlFilePath"/> が存在しないディレクトリを示している場合
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// ・引数の <paramref name="htmlFilePath"/> で指定されたファイルが存在しない場合
        /// 　[<see cref="FileNotFoundException"/>]
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 引数の <paramref name="htmlFilePath"/> がファイルを指定しないない（ディレクトリを指定）場合、
        /// 又は、呼び出し元に必要なアクセス許可がない場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        public static void Start(
            string chromePath,
            string htmlFilePath,
            string postRegex,
            string postParamName,
            SizePoint sizePoint,
            params Func<string, byte[]>[] responceFunctions)
        {
            // 開始
            Start(chromePath, htmlFilePath, postRegex, postParamName, sizePoint, null, responceFunctions);
        }

        /// <summary>
        /// Chromeによる音声認識を開始する
        /// </summary>
        /// <param name="chromePath">
        /// ChromeのEXEファイルの絶対パス
        /// 指定した値は <see cref="ChromePath"/> プロパティに設定する
        /// </param>
        /// <param name="htmlFilePath">
        /// 音声認識に使用する既定の「音声認識用.html」ファイルのパス
        /// </param>
        /// <param name="postRegex">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するURLの正規表現
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="postParamName">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するPOSTデータのパラメータ名
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="sizePoint">
        /// 音声認識用に起動するChromeの初期のサイズと位置
        /// （NULLを指定した場合はデフォルトのサイズと位置で起動する）
        /// </param>
        /// <param name="localHttpServerPort">
        /// 音声認識に使用するローカルHTTPサーバのポート番号を指定する場合に設定するポート番号
        /// 指定しない場合（NULLの場合）、または指定したポート番号が使用できない場合は乱数からポート番号を取得し使用する
        /// </param>
        /// <param name="responceFunctions">
        /// 音声認識した文字に対する処理の指定（複数指定可、優先順位は上から）
        /// ・引数1 　string：音声認識した文字列を渡す
        /// ・戻り値　byte[]：処理を実行した結果返却するデータ
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="htmlFilePath"/> 又は <paramref name="postRegex"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生する
        /// ・引数の <paramref name="htmlFilePath"/> が下記の場合
        /// 　長さ 0 の文字列
        /// 　空白のみで構成される
        /// 　<see cref="Path.InvalidPathChars"/> で定義される 1 つ以上の正しくない文字を含む
        /// ・引数の <paramref name="postRegex"/> が正規表現として不正な値の場合
        /// </exception>
        /// <exception cref="SpeechRecognitionException">
        /// Chromeが使用不可能な場合に発生
        /// （<see cref="ChromePath"/> の指定が間違ってる 又は、Chromeがインストールされていない場合）
        /// </exception>
        /// <exception cref="IOException">
        /// 下記の場合に発生
        /// ・引数の <paramref name="htmlFilePath"/> がシステム定義の最大長を超えている場合
        /// 　[<see cref="PathTooLongException"/>]
        /// 　（たとえば、Windowsでは、パスは 248文字未満、ファイル名は 260 文字未満である必要がある）
        /// ・引数の <paramref name="htmlFilePath"/> が存在しないディレクトリを示している場合
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// ・引数の <paramref name="htmlFilePath"/> で指定されたファイルが存在しない場合
        /// 　[<see cref="FileNotFoundException"/>]
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 引数の <paramref name="htmlFilePath"/> がファイルを指定しないない（ディレクトリを指定）場合、
        /// 又は、呼び出し元に必要なアクセス許可がない場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        public static void Start(
            string chromePath,
            string htmlFilePath,
            string postRegex,
            string postParamName,
            SizePoint sizePoint,
            int? localHttpServerPort,
            params Func<string, byte[]>[] responceFunctions)
        {
            // プロパティにChromeのEXEファイルのパスを設定する
            ChromePath = chromePath;

            // 開始
            Start(htmlFilePath, postRegex, postParamName, sizePoint, localHttpServerPort, responceFunctions);
        }

        /// <summary>
        /// Chromeによる音声認識を開始する
        /// <see cref="ChromePath"/> プロパティが未設定の場合はChromeのEXEファイルパスを自動的に検索し使用する
        /// </summary>
        /// <param name="htmlFilePath">
        /// 音声認識に使用する既定の「音声認識用.html」ファイルのパス
        /// </param>
        /// <param name="postRegex">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するURLの正規表現
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="postParamName">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するPOSTデータのパラメータ名
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="sizePoint">
        /// 音声認識用に起動するChromeの初期のサイズと位置
        /// （NULLを指定した場合はデフォルトのサイズと位置で起動する）
        /// </param>
        /// <param name="localHttpServerPort">
        /// 音声認識に使用するローカルHTTPサーバのポート番号を指定する場合に設定するポート番号
        /// 指定しない場合（NULLの場合）、または指定したポート番号が使用できない場合は乱数からポート番号を取得し使用する
        /// </param>
        /// <param name="responceFunctions">
        /// 音声認識した文字に対する処理の指定（複数指定可、優先順位は上から）
        /// ・引数1 　string：音声認識した文字列を渡す
        /// ・戻り値　byte[]：処理を実行した結果返却するデータ
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="htmlFilePath"/> 又は <paramref name="postRegex"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生する
        /// ・引数の <paramref name="htmlFilePath"/> が下記の場合
        /// 　長さ 0 の文字列
        /// 　空白のみで構成される
        /// 　<see cref="Path.InvalidPathChars"/> で定義される 1 つ以上の正しくない文字を含む
        /// ・引数の <paramref name="postRegex"/> が正規表現として不正な値の場合
        /// </exception>
        /// <exception cref="SpeechRecognitionException">
        /// Chromeが使用不可能な場合に発生
        /// （<see cref="ChromePath"/> の指定が間違ってる 又は、Chromeがインストールされていない場合）
        /// </exception>
        /// <exception cref="IOException">
        /// 下記の場合に発生
        /// ・引数の <paramref name="htmlFilePath"/> がシステム定義の最大長を超えている場合
        /// 　[<see cref="PathTooLongException"/>]
        /// 　（たとえば、Windowsでは、パスは 248文字未満、ファイル名は 260 文字未満である必要がある）
        /// ・引数の <paramref name="htmlFilePath"/> が存在しないディレクトリを示している場合
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// ・引数の <paramref name="htmlFilePath"/> で指定されたファイルが存在しない場合
        /// 　[<see cref="FileNotFoundException"/>]
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 引数の <paramref name="htmlFilePath"/> がファイルを指定しないない（ディレクトリを指定）場合、
        /// 又は、呼び出し元に必要なアクセス許可がない場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        public static void Start(
            string htmlFilePath,
            string postRegex,
            string postParamName,
            SizePoint sizePoint,
            int? localHttpServerPort,
            params Func<string, byte[]>[] responceFunctions)
        {
            // NULLチェック
            if (htmlFilePath == null)
            {
                throw new ArgumentNullException(nameof(htmlFilePath));
            }
            else if (postRegex == null)
            {
                throw new ArgumentNullException(nameof(postRegex));
            }

            // Chromeが使用可能かチェックする
            switch (CanUseChrome)
            {
                case ChromeState.CanUse:
                    // OK：処理を継続する
                    break;
                case ChromeState.NoInstall:
                    // NG：メッセージを表示して処理を中止する
                    throw new SpeechRecognitionException(ErrorMessage.ChromeSpeechRecognitionMessageNoInstall);
                case ChromeState.ChromePathIsWrong:
                default:
                    // NG：メッセージを表示して処理を中止する
                    throw new SpeechRecognitionException(
                        ErrorMessage.ChromeSpeechRecognitionMessageChromePathIsWrong);
            }

            // 開始前の既にローカルHTTPサーバが起動している場合は落とす
            if (Server != null)
            {
                Server.Stop();
                Server = null;
            }

            // 開始前の既に起動している音声認識用のChromeが存在する場合は落とす
            if (ChromeProcessInfoKey.HasValue)
            {
                StartProcess.CloseWindow(ChromeProcessInfoKey.Value);
                ChromeProcessInfoKey = null;
            }

            // HTTPサーバのクラスを生成
            HttpResponseData faviconResponse = FaviconData == null
                    ? LocalHttpServerCommon.EmptyDataResponse
                    : LocalHttpServerCommon.GetIconDataResponse(FaviconData);
            Server = new LocalHttpServer(
                LocalHttpServerCommon.GetHttpFileDataResponse(htmlFilePath),
                faviconResponse,
                GetResponceProcesses(responceFunctions, postRegex, postParamName));

            // HTTPサーバのクラスをリソース解放対象として登録する
            // （非同期での動作を止めるために登録）
            EntryPoint.DisposeClass.Add(Server);

            // HTTPサーバを起動する
            Server.Start(localHttpServerPort);

            // 起動パラメータを設定
            string startUpParam = string.Format(
                CultureInfo.InvariantCulture, ChromeStartUpParamFormat, Server.Url.ToString());

            // Chromeを起動する
            long? key = StartProcess.Start(ChromePath, startUpParam, sizePoint);

            // 起動キーをプロパティに保持する
            ChromeProcessInfoKey = key;
        }

        /// <summary>
        /// この機能を使用して起動したChromeを閉じる
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        /// <returns>
        /// 閉じたChromeのサイズ位置の情報
        /// サイズ位置の情報が取得できない場合はNULLを返却
        /// </returns>
        public static SizePoint Close()
        {
            // 閉じたChromeのサイズ位置の情報の変数を宣言
            SizePoint sizePoint = null;

            // Chromeの音声認識が起動しているかチェック
            if (ChromeProcessInfoKey.HasValue)
            {
                // 閉じるChromeのウィンドウサイズ位置情報を取得
                foreach (ProcessInfo info in StartProcess.StartProcessList)
                {
                    // 起動したChromeのウィンドウハンドルを取得しサイズ位置情報を取得する
                    if (info.StartNumId == ChromeProcessInfoKey.Value && info.HasWindowInfo)
                    {
                        sizePoint = WindowOperate.GetWindowRect(info.WindowInfo.WindowHandle);
                    }

                    // サイズ位置情報が取得できた場合、ループを抜ける
                    if (sizePoint != null)
                    {
                        break;
                    }
                }

                // 起動している場合、対象のChromeを閉じる
                StartProcess.CloseWindow(ChromeProcessInfoKey.Value);
                ChromeProcessInfoKey = null;
            }

            // ローカルHTTPサーバが起動しているかチェック
            if (Server != null)
            {
                // 起動している場合は停止する
                Server.Stop();
                Server = null;
            }

            // 取得したサイズ位置情報を返却
            return sizePoint;
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 引数 <paramref name="responceFunctions"/> からローカルHTTPサーバで動作させる
        /// レスポンス処理を生成し取得する
        /// </summary>
        /// <param name="responceFunctions">
        /// 音声認識した文字に対する処理の指定（複数指定可、優先順位は上から）
        /// ・引数1 　string：音声認識した文字列を渡す
        /// ・戻り値　byte[]：処理を実行した結果返却するデータ
        /// </param>
        /// <param name="postRegex">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用するURLの正規表現
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <param name="postParamName">
        /// 「音声認識用.html」において、音声認識認識した文字列をPOSTする際に使用する
        /// POSTデータのパラメータ名
        /// （「音声認識用.html」と整合性をとる必要があるパラメータ）
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="postRegex"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 引数の <paramref name="postRegex"/> が正規表現として不正な値の場合に発生
        /// </exception>
        /// <returns>
        /// 生成したローカルHTTPサーバで動作させるレスポンス処理
        /// 引数 <paramref name="responceFunctions"/> の値がNULLの場合はNULLをそのまま返却
        /// </returns>
        private static LocalHttpServerResponceProcess[] GetResponceProcesses(
            Func<string, byte[]>[] responceFunctions,
            string postRegex,
            string postParamName)
        {
            // 引数チェック
            if (responceFunctions == null)
            {
                // responceActionsの値がNULLの場合はNULLをそのまま返却
                return null;
            }
            else if (postRegex == null)
            {
                throw new ArgumentNullException(nameof(postRegex));
            }

            // 戻り値の配列を生成（要素数は引数と同じになる）
            LocalHttpServerResponceProcess[] responceProcesses
                = new LocalHttpServerResponceProcess[responceFunctions.Length];

            // 引数の配列でループ
            for (int i = 0; i < responceFunctions.Length; i++)
            {
                // 対象の処理を取得
                Func<string, byte[]> function = responceFunctions[i];

                // レスポンスデータを扱うクラスを生成
                HttpResponseData httpResponseData = new HttpResponseData(
                    mimeType: "text/html",
                    setStream: (Stream stream, HttpListenerRequest request) =>
                    {
                        // POSTデータの受け取り
                        NameValueCollection postData = LocalHttpServerCommon.GetPostData(request);

                        // 処理
                        string voiceTxt = postData[postParamName];
                        byte[] responseData = null;
                        if (!string.IsNullOrEmpty(voiceTxt))
                        {
                            responseData = function(voiceTxt);
                        }

                        // レスポンスデータをレスポンス用のStreamに格納し返却する
                        return LocalHttpServerCommon.GetDataResponse(
                            responseData ?? new byte[0], "text/html").SetStream(stream, request);
                    });

                // ローカルHTTPサーバで動作させるレスポンス処理のクラスを生成し戻り値の配列に設定する
                responceProcesses[i] = new LocalHttpServerResponceProcess(
                    method: System.Net.Http.HttpMethod.Post,
                    processName: postRegex,
                    responseData: httpResponseData);
            }

            // 生成した配列を返却する
            return responceProcesses;
        }

        #endregion
    }
}
