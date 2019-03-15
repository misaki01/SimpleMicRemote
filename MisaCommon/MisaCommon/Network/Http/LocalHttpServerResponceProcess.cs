namespace MisaCommon.Network.Http
{
    using System;
    using System.Net.Http;
    using System.Text.RegularExpressions;

    /// <summary>
    /// <see cref="HttpMethod"/> とURLに紐づくHttpレスポンスの処理情報を扱うクラス
    /// </summary>
    public class LocalHttpServerResponceProcess
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="method">
        /// Httpメソッド
        /// </param>
        /// <param name="processName">
        /// 処理名：URLのパスの部分の名称（正規表現で設定）
        /// </param>
        /// <param name="responseData">
        /// Httpレスポンスの処理情報を格納したクラス
        /// （NULLを指定した場合は0byteのデータを返却する機能を持つクラスを設定）
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="method"/> 又は、<paramref name="processName"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 引数の <paramref name="processName"/> が正規表現として不正な値の場合に発生
        /// </exception>
        public LocalHttpServerResponceProcess(HttpMethod method, string processName, HttpResponseData responseData)
        {
            // 各プロパティに値を設定

            // Httpメソッド
            Method = method ?? throw new ArgumentNullException(nameof(method));

            // Httpメソッドの文字列
            MethodString = Method.ToString();

            // URL（正規表現で設定）
            ProcessName = processName ?? throw new ArgumentNullException(nameof(processName));

            // URLの正規表現オブジェクト
            ProcessNameRegex = new Regex(ProcessName);

            // レスポンスデータに対する処理情報を格納したクラス
            // 引数がNULLの場合は0byteのデータを返却する機能を持つクラスを設定する
            ResponseData = responseData ?? LocalHttpServerCommon.EmptyDataResponse;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// Httpメソッドを取得する
        /// </summary>
        public HttpMethod Method { get; private set; }

        /// <summary>
        /// Httpメソッドの文字列を取得する
        /// </summary>
        public string MethodString { get; private set; }

        /// <summary>
        /// 処理名：URLのパスの部分の名称（正規表現で設定）を取得する
        /// </summary>
        public string ProcessName { get; private set; }

        /// <summary>
        /// 処理名の正規表現オブジェクトを取得する
        /// </summary>
        public Regex ProcessNameRegex { get; private set; }

        /// <summary>
        /// レスポンスデータに対する処理情報を格納したクラスを取得する
        /// </summary>
        public HttpResponseData ResponseData { get; private set; }

        #endregion

        #region メソッド

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public LocalHttpServerResponceProcess DeepCopy()
        {
            return new LocalHttpServerResponceProcess(
                method: Method,
                processName: ProcessName,
                responseData: ResponseData?.DeepCopy());
        }

        #endregion
    }
}
