namespace MisaCommon.Network.Http
{
    using System;
    using System.IO;
    using System.Net;

    using MisaCommon.Utility;

    // レスポンスデータの生成と、そのデータに対するStreamを設定する機能のデリゲートの定義
    #region デリゲートの定義

    /// <summary>
    /// レスポンスデータに対する <see cref="Stream"/> を引数の <paramref name="stream"/>に設定する
    /// </summary>
    /// <param name="stream">レスポンスデータを設定する <see cref="Stream"/> オブジェクト</param>
    /// <param name="request">Httpリクエスト</param>
    /// <returns>レスポンスデータのサイズ（バイト単位）</returns>
    public delegate long SetResponseData(Stream stream, HttpListenerRequest request);

    #endregion

    /// <summary>
    /// Http通信でのレスポンス処理で使用するMIMEタイプとデータ取得処理を扱うクラス
    /// </summary>
    public class HttpResponseData
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数の値で初期化を行う
        /// </summary>
        /// <param name="mimeType">Httpレスポンスの処理で返却するデータのMIMEタイプ</param>
        /// <param name="setStream">レスポンスデータに対するStreamを設定する機能</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="mimeType"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 引数の <paramref name="mimeType"/> が空文字の場合に発生
        /// </exception>
        public HttpResponseData(string mimeType, SetResponseData setStream)
        {
            // MIMEタイプのチェック
            // NULL、空の場合は例外をスロー
            if (mimeType == null)
            {
                throw new ArgumentNullException(nameof(mimeType));
            }
            else if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentException(CommonMessage.ArgumentExceptionMessageEmpty, nameof(mimeType));
            }

            // 値の設定
            MimeType = mimeType;
            SetStream = setStream;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// Httpレスポンスの処理で返却するデータのMIMEタイプを取得する
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// レスポンスデータに対するStreamを設定する機能を取得する
        /// </summary>
        public SetResponseData SetStream { get; private set; }

        #endregion

        #region メソッド

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public HttpResponseData DeepCopy()
        {
            return new HttpResponseData(
                mimeType: MimeType,
                setStream: (SetResponseData)SetStream?.Clone());
        }

        #endregion
    }
}
