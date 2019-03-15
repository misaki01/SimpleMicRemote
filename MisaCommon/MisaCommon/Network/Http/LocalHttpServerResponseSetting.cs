namespace MisaCommon.Network.Http
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Httpレスポンスの処理の設定クラス
    /// </summary>
    public class LocalHttpServerResponseSetting
    {
        #region クラス変数・定数

        /// <summary>
        /// favicon.icoファイルの取得で指定されるURL（相対パス）
        /// </summary>
        public const string FaviconUrl = "/favicon.ico";

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="defaultProcess">
        /// デフォルトで呼び出されるHttpレスポンスの処理の情報を格納したクラス
        /// （NULLを指定した場合は0byteのデータを返却する機能を持つクラスを設定）
        /// </param>
        /// <param name="faviconProcess">
        /// GET favicon.icoのリクエスト処理で呼び出されるHttpレスポンスの処理の情報を格納したクラス
        /// （NULLを指定した場合は0byteのデータを返却する機能を持つクラスを設定）
        /// </param>
        /// <param name="responceProcesses">
        /// Httpレスポンスの処理のクラスの配列
        /// </param>
        public LocalHttpServerResponseSetting(
            HttpResponseData defaultProcess,
            HttpResponseData faviconProcess,
            params LocalHttpServerResponceProcess[] responceProcesses)
        {
            // 各プロパティに値を設定

            // デフォルトの処理
            // 引数がNULLの場合は0byteのデータを返却する機能を持つクラスを設定
            DefaultResponse = defaultProcess ?? LocalHttpServerCommon.EmptyDataResponse;

            // GET favicon.icoのリクエスト処理の処理
            // 引数がNULLの場合は0byteのデータを返却する機能を持つクラスを設定
            FaviconResponse = faviconProcess ?? LocalHttpServerCommon.EmptyDataResponse;

            // Httpレスポンスの処理のクラスの配列
            // 引数がNULLの場合は要素0の配列を生成し設定
            ResponceProcesses = responceProcesses == null
                ? new Collection<LocalHttpServerResponceProcess>()
                : new Collection<LocalHttpServerResponceProcess>(responceProcesses);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// デフォルトで呼び出されるHttpレスポンスの処理の情報を格納したクラスを取得する
        /// </summary>
        public HttpResponseData DefaultResponse { get; private set; }

        /// <summary>
        /// GET favicon.icoのリクエスト処理で呼び出されるHttpレスポンスの処理の情報を格納したクラスを取得する
        /// </summary>
        public HttpResponseData FaviconResponse { get; private set; }

        /// <summary>
        /// <see cref="HttpMethod"/> とURLに紐づくHttpレスポンスの処理情報を扱うクラスの配列を取得する
        /// </summary>
        public ICollection<LocalHttpServerResponceProcess> ResponceProcesses { get; private set; }

        #endregion

        #region メソッド

        #region 条件に合致するデータを検索（Find、FindAll）

        /// <summary>
        /// <see cref="HttpMethod"/> とURLに紐づくHttpレスポンスの処理のクラスの配列から、
        /// 引数の条件に合致するデータを取得する
        /// </summary>
        /// <param name="method">検索対象とするHttpメソッド</param>
        /// <param name="processName">検索対象とする処理名：URLのパスの部分の名称（相対パス）</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="method"/> 又は、<paramref name="processName"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// 正規表現のマッチング処理にてタイムアウトが発生した場合に発生
        /// </exception>
        /// <returns>
        /// 引数に合致するHttpレスポンスの処理のクラス
        /// 条件に合致するデータが存在しない場合はNULLを返却する
        /// 複数取得できる場合は先頭の一つのみ取得する
        /// </returns>
        public LocalHttpServerResponceProcess Find(string method, string processName)
        {
            // NULLチェック
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            else if (processName == null)
            {
                throw new ArgumentNullException(nameof(processName));
            }

            // 引数に合致するHttpレスポンスの処理のクラスを取得
            // 取得できなかった場合は、NULL
            LocalHttpServerResponceProcess responceProcess =
                ResponceProcesses?.FirstOrDefault((x) => x.MethodString.Equals(method) && x.ProcessNameRegex.IsMatch(processName));

            // 取得したHttpレスポンスの処理のクラスを返却
            return responceProcess;
        }

        /// <summary>
        /// <see cref="HttpMethod"/> とURLに紐づくHttpレスポンスの処理のクラスの配列から、
        /// 引数の条件に合致するデータを全て取得する
        /// </summary>
        /// <param name="method">検索対象とするHttpメソッド</param>
        /// <param name="processName">検索対象とする処理名：URLのパスの部分の名称（相対パス）</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="method"/> 又は、<paramref name="processName"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// 正規表現のマッチング処理にてタイムアウトが発生した場合に発生
        /// </exception>
        /// <returns>
        /// 引数に合致するHttpレスポンスの処理のクラスの配列
        /// 条件に合致するデータが存在しない場合はNULLを返却する
        /// </returns>
        public LocalHttpServerResponceProcess[] FindAll(string method, string processName)
        {
            // NULLチェック
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            else if (processName == null)
            {
                throw new ArgumentNullException(nameof(processName));
            }

            // 引数に合致するHttpレスポンスの処理のクラスを取得
            IEnumerable<LocalHttpServerResponceProcess> processes =
                ResponceProcesses?.Where((x) => x.MethodString.Equals(method) && x.ProcessNameRegex.IsMatch(processName));
            if (processes == null || !processes.Any())
            {
                // 取得できなかった場合は、NULL返却
                return null;
            }

            // 取得したHttpレスポンスの処理のクラスのリストを返却
            return processes.ToArray();
        }

        #endregion

        #region インスタンスのDeepCopy

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public LocalHttpServerResponseSetting DeepCopy()
        {
            // Httpレスポンスの処理のクラスの配列を複製
            LocalHttpServerResponceProcess[] copyList = null;
            if (ResponceProcesses != null)
            {
                copyList = new LocalHttpServerResponceProcess[ResponceProcesses.Count];
                int index = 0;
                foreach (LocalHttpServerResponceProcess responceProcess in ResponceProcesses)
                {
                    copyList[index] = responceProcess.DeepCopy();
                    index++;
                }
            }

            // このクラスのインスタンスを生成し返却
            return new LocalHttpServerResponseSetting(
                defaultProcess: DefaultResponse.DeepCopy(),
                faviconProcess: FaviconResponse.DeepCopy(),
                responceProcesses: copyList);
        }

        #endregion

        #endregion
    }
}
