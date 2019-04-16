namespace MisaCommon.Network.Http
{
    using System;
    using System.Collections.Specialized;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Security;

    /// <summary>
    /// 共通的に使用する <see cref="HttpResponseData"/> データ（0byteデータ返却、ファイルを取得し返却、等）
    /// 及び、<see cref="LocalHttpServer"/> の処理で共通的に使用する処理を扱うクラス
    /// </summary>
    public static class LocalHttpServerCommon
    {
        #region 汎用的に使用する HttpResponseData オブジェクト

        /// <summary>
        /// 空のデータ（0byteのデータ）レスポンスデータを扱う <see cref="HttpResponseData"/> オブジェクト
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// SetStreamに設定するデリゲートの引数（stream）がNULLの場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Stream"/> が閉じた後でメソッドが呼び出された場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="Stream"/> は読み取り/書き込みがサポートされていない環境の場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="IOException">
        /// I/O エラーが発生した場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        public static HttpResponseData EmptyDataResponse =>
            new HttpResponseData(
                mimeType: "text/html",
                setStream: (Stream stream, HttpListenerRequest request) =>
                {
                    byte[] data = new byte[0];
                    SetDataToStream(stream, data);
                    return data.LongLength;
                });

        /// <summary>
        /// 引数（<paramref name="data"/>）のデータレスポンスデータを扱う
        /// <see cref="HttpResponseData"/> オブジェクト
        /// </summary>
        /// <param name="data">レスポンスデータとして返却するデータ</param>
        /// <param name="mimeType">レスポンスデータのMIMEタイプ</param>
        /// <exception cref="ArgumentNullException">
        /// 下記の場合に発生
        /// ・引数 <paramref name="data"/> がNULLの場合に発生
        /// 　（このメソッドを呼び出した時に発生する）
        /// ・SetStreamに設定するデリゲートの引数（stream）がNULLの場合に発生
        /// 　（SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Stream"/> が閉じた後でメソッドが呼び出された場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="Stream"/> は読み取り/書き込みがサポートされていない環境の場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="IOException">
        /// I/O エラーが発生した場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <returns>
        /// 引数 <paramref name="data"/> で指定されたデータをレスポンスンスデータとして扱う
        /// <see cref="HttpResponseData"/> オブジェクト
        /// </returns>
        public static HttpResponseData GetDataResponse(byte[] data, string mimeType)
        {
            // NULLチェック
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return new HttpResponseData(
                mimeType: mimeType,
                setStream: (Stream stream, HttpListenerRequest request) =>
                {
                    SetDataToStream(stream, data);
                    return data.LongLength;
                });
        }

        /// <summary>
        /// 引数（<paramref name="filePath"/>）で指定されたパスのファイルを、
        /// レスポンスデータとして扱う <see cref="HttpResponseData"/> オブジェクト
        /// </summary>
        /// <param name="filePath">レスポンスデータとして返却するファイルのパス</param>
        /// <exception cref="ArgumentNullException">
        /// 下記の場合に発生
        /// ・引数 <paramref name="filePath"/> がNULLの場合
        /// 　（このメソッドを呼び出した時に発生する）
        /// ・SetStreamに設定するデリゲートの引数（stream）がNULLの場合
        /// 　（SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 引数の <paramref name="filePath"/> が下記の場合に発生
        /// ・長さ 0 の文字列
        /// ・空白のみで構成される
        /// ・<see cref="Path.InvalidPathChars"/> で定義される 1 つ以上の正しくない文字を含む
        /// （このメソッドを呼び出した時に発生する）
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Stream"/> が閉じた後でメソッドが呼び出された場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// 引数の <paramref name="filePath"/> の形式が正しくない場合 又は、
        /// <see cref="Stream"/> は読み取り/書き込みがサポートされていない環境の場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="IOException">
        /// 下記の場合に発生
        /// ・引数の <paramref name="filePath"/> がシステム定義の最大長を超えている場合
        /// 　[<see cref="PathTooLongException"/>]
        /// 　（たとえば、Windowsでは、パスは 248文字未満、ファイル名は 260 文字未満である必要がある）
        ///   （このメソッドを呼び出した時に発生する）
        /// ・引数の <paramref name="filePath"/> が存在しないディレクトリを示している場合
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// 　（このメソッドを呼び出した時に発生する）
        /// ・引数の <paramref name="filePath"/> で指定されたファイルが存在しない場合
        /// 　[<see cref="FileNotFoundException"/>]
        /// 　（このメソッドを呼び出した時に発生する）
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// 　（このメソッドを呼び出した時、SetStreamに設定するデリゲート実行時の両方で発生する）
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 引数の <paramref name="filePath"/> がファイルを指定しないない（ディレクトリを指定）場合、
        /// 又は、呼び出し元に必要なアクセス許可がない場合に発生
        /// （このメソッドを呼び出した時に発生する）
        /// </exception>
        /// <exception cref="SecurityException">
        /// 呼び出し元に必要なアクセス許可がない場合に発生（セキュリティエラー）
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <returns>
        /// ファイルをレスポンスデータとして扱う <see cref="HttpResponseData"/> オブジェクト
        /// </returns>
        public static HttpResponseData GetHttpFileDataResponse(string filePath)
        {
            // パスで指定されたファイルのデータをバイトデータとして取得
            byte[] data = File.ReadAllBytes(filePath);

            // 取得したバイトデータをもとにレスポンスデータの処理情報を取得し返却
            return GetDataResponse(data, "text/html");
        }

        /// <summary>
        /// 引数（<paramref name="icon"/>）のアイコンデータレスポンスデータを扱う
        /// <see cref="HttpResponseData"/> オブジェクト
        /// </summary>
        /// <param name="icon">レスポンスデータとして返却するアイコンデータ</param>
        /// <exception cref="ArgumentNullException">
        /// 下記の場合に発生
        /// ・引数 <paramref name="icon"/> がNULLの場合に発生
        /// 　（このメソッドを呼び出した時に発生する）
        /// ・SetStreamに設定するデリゲートの引数（stream）がNULLの場合に発生
        /// 　（SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Stream"/> が閉じた後でメソッドが呼び出された場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="Stream"/> は読み取り/書き込みがサポートされていない環境の場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <exception cref="IOException">
        /// I/O エラーが発生した場合に発生
        /// （SetStreamに設定するデリゲート実行時に発生する）
        /// </exception>
        /// <returns>
        /// 引数 <paramref name="icon"/> で指定されたデータをレスポンスンスデータとして扱う
        /// <see cref="HttpResponseData"/> オブジェクト
        /// </returns>
        public static HttpResponseData GetIconDataResponse(Icon icon)
        {
            // NULLチェック
            if (icon == null)
            {
                throw new ArgumentNullException(nameof(icon));
            }

            // アイコンをバイトデータで取得する
            byte[] data;
            MemoryStream memoryStream;
            using (memoryStream = new MemoryStream())
            {
                icon.ToBitmap().Save(memoryStream, ImageFormat.Png);
                data = memoryStream.ToArray();
            }

            // 取得したバイトデータをもとにレスポンスデータの処理情報を取得し返却
            return GetDataResponse(data, "image/x-icon");
        }

        #endregion

        #region LocalHttpServerの共通処理

        /// <summary>
        /// POSTされたデータを <see cref="NameValueCollection"/> として取得する
        /// </summary>
        /// <param name="request">HTTPリクエスト</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="request"/> がNULLの場合に発生
        /// </exception>
        /// <returns>
        /// POSTされたデータを格納した <see cref="NameValueCollection"/> オブジェクト
        /// </returns>
        public static NameValueCollection GetPostData(HttpListenerRequest request)
        {
            // NULLチェック
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            NameValueCollection collection = new NameValueCollection();
            StreamReader reader;
            using (reader = new StreamReader(request.InputStream))
            {
                // StreamからPOSTデータを読み込み
                string postData = reader.ReadToEnd();

                // 読み込んだデータを'&'で区切り、ループする
                foreach (string parameter in postData.Split('&'))
                {
                    // 読み込んだデータを'='で区切り、パラメータ名と値を取得する
                    string[] data = parameter.Split('=');
                    if (data.Length > 0)
                    {
                        string name = data[0];
                        string value = data.Length > 1 ? WebUtility.UrlDecode(data[1]) : string.Empty;
                        collection.Add(name, value);
                    }
                }
            }

            // POSTデータを格納したコレクションを返却
            return collection;
        }

        /// <summary>
        /// 引数の <paramref name="inputData"/> のバイトデータを <paramref name="stream"/> に設定する
        /// </summary>
        /// <param name="stream">データ設定先の <see cref="Stream"/></param>
        /// <param name="inputData">データ設定元のバイトデータ</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="stream"/> 又は <paramref name="inputData"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Stream"/> が閉じた後でメソッドが呼び出された場合に発生
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="Stream"/> は読み取り/書き込みがサポートされていない環境の場合に発生
        /// </exception>
        /// <exception cref="IOException">
        /// I/O エラーが発生した場合に発生
        /// </exception>
        public static void SetDataToStream(Stream stream, byte[] inputData)
        {
            // NULLチェック
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            else if (inputData == null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }

            // メモリーStreamを生成して、設定先のStreamにデータを設定する
            MemoryStream inputStream;
            using (inputStream = new MemoryStream(inputData))
            {
                SetDataToStream(stream, inputStream);
            }
        }

        /// <summary>
        /// 引数の <paramref name="inputStream"/> のデータを <paramref name="stream"/> に設定する
        /// </summary>
        /// <param name="stream">データ設定先の <see cref="Stream"/></param>
        /// <param name="inputStream">データ設定元の <see cref="Stream"/></param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="stream"/> 又は <paramref name="inputStream"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="Stream"/> が閉じた後でメソッドが呼び出された場合に発生
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="Stream"/> は読み取り/書き込みがサポートされていない環境の場合に発生
        /// </exception>
        /// <exception cref="IOException">
        /// I/O エラーが発生した場合に発生
        /// </exception>
        public static void SetDataToStream(Stream stream, Stream inputStream)
        {
            // NULLチェック
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            else if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            // データの読み書き用のバッファ生成
            int bufferSize = 1024 * 32;
            byte[] buffer = new byte[bufferSize];

            // ループにてバッファのサイズ分読み込んで書き込む処理を行う
            int readCount;
            inputStream.Position = 0;
            while ((readCount = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, readCount);
            }
        }

        #endregion
    }
}