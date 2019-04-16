namespace MisaCommon.Network.Http
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.NetworkInformation;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using MisaCommon.Exceptions;
    using MisaCommon.MessageResources;

    /// <summary>
    /// ローカルのHttpサーバを起動・停止するための機能を提供するクラス
    /// </summary>
    public class LocalHttpServer : IDisposable
    {
        #region クラス変数・定数

        /// <summary>
        /// ローカルHTTPサーバのURLのフォーマット定義（{0}：ポート番号）
        /// </summary>
        private const string LocalUrlFormat = "http://localhost:{0}/";

        /// <summary>
        /// 設定情報への書き込み処理の排他制御を行うロックオブジェクト
        /// </summary>
        private readonly object lockSettingObject = new object();

        /// <summary>
        /// Dispose処理済みかどうかのフラグ
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// ポート番号を生成するために使用する乱数
        /// </summary>
        private Random randomPort = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 初期化を行う
        /// </summary>
        public LocalHttpServer()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の情報でHttpレスポンスの処理設定の初期化を行う
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
        public LocalHttpServer(
            HttpResponseData defaultProcess,
            HttpResponseData faviconProcess,
            params LocalHttpServerResponceProcess[] responceProcesses)
        {
            // Httpレスポンスの処理の設定クラスを生成しプロパティに設定する
            SetResponseProcessSetting(defaultProcess, faviconProcess, responceProcesses);
        }

        #endregion

        #region ファイナライザー

        /// <summary>
        /// ファイナライザー
        /// リソースを解放する
        /// </summary>
        ~LocalHttpServer()
        {
            // リソースの解放処理は「Dispose(bool disposing)」にて実装する
            // ここでは解放処理は行わないこと
            Dispose(false);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// Httpレスポンスの処理の設定情報を取得する
        /// （参照渡しにならないよう複製を返却）
        /// </summary>
        public LocalHttpServerResponseSetting ResponseSetting
        {
            get
            {
                // 設定処理とコピーが競合しないように排他制御をかける
                LocalHttpServerResponseSetting setting;
                lock (lockSettingObject)
                {
                    setting = Setting?.DeepCopy();
                }

                return setting;
            }
        }

        /// <summary>
        /// ローカルHTTPサーバのURLを取得する
        /// （HTTPサーバが起動していない場合はNULL）
        /// </summary>
        public Uri Url { get; private set; } = null;

        /// <summary>
        /// ローカルHTTPサーバが起動しているかどうかのフラグを取得する
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// Httpレスポンスの処理の設定情報を取得・設定する
        /// </summary>
        private LocalHttpServerResponseSetting Setting { get; set; } = null;

        /// <summary>
        /// ローカルHTTPサーバのリスナーオブジェクトを取得・設定する
        /// </summary>
        private HttpListener Listener { get; set; } = null;

        /// <summary>
        /// ポート番号を生成するために使用する乱数を取得する
        /// </summary>
        /// <remarks>
        /// シングルトンパターンで実装
        /// </remarks>
        private Random RandomPort
        {
            get
            {
                if (randomPort == null)
                {
                    randomPort = new Random();
                }

                return randomPort;
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// Httpレスポンスの処理を行うデリゲートを設定する
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
        public void SetResponseProcessSetting(
            HttpResponseData defaultProcess,
            HttpResponseData faviconProcess,
            params LocalHttpServerResponceProcess[] responceProcesses)
        {
            // Httpレスポンスの処理の設定クラスを生成しプロパティに設定する
            lock (lockSettingObject)
            {
                Setting = new LocalHttpServerResponseSetting(
                    defaultProcess, faviconProcess, responceProcesses);
            }
        }

        /// <summary>
        /// ローカルのHTTPサーバを起動する
        /// </summary>
        /// <exception cref="LocalHttpServerException">
        /// 下記の場合に発生
        /// ・現在のオペレーティングシステムにおいて、<see cref="HttpListener"/> の使用が不可の場合
        /// ・使用可能なポートが存在せず、サーバを起動できない場合に発生
        /// ・<see cref="HttpListener"/> の処理において、Win32関数の呼び出しが失敗した場合に発生
        /// 　例外の原因は <see cref="HttpListenerException.ErrorCode"/> プロパティを参照
        /// ・<see cref="HttpListener"/> の処理において、<see cref="HttpListenerException"/> 以外の
        /// 　例外が発生した場合に発生
        /// 　（<see cref="ObjectDisposedException"/> が発生した場合であるが、通常は起こりえない）
        /// </exception>
        public void Start()
        {
            Start(null);
        }

        /// <summary>
        /// ローカルのHTTPサーバを起動する
        /// </summary>
        /// <param name="port">
        /// ポート番号を指定する場合に設定するポート番号
        /// 指定しない場合（NULLの場合）または、指定したポート番号が使用できない場合は
        /// 乱数からポート番号を取得し使用する
        /// </param>
        /// <exception cref="LocalHttpServerException">
        /// 下記の場合に発生
        /// ・現在のオペレーティングシステムにおいて、<see cref="HttpListener"/> の使用が不可の場合
        /// ・使用可能なポートが存在せず、サーバを起動できない場合に発生
        /// ・<see cref="HttpListener"/> の処理において、Win32関数の呼び出しが失敗した場合に発生
        /// 　例外の原因は <see cref="HttpListenerException.ErrorCode"/> プロパティを参照
        /// ・<see cref="HttpListener"/> の処理において、<see cref="HttpListenerException"/> 以外の
        /// 　例外が発生した場合に発生
        /// 　（<see cref="ObjectDisposedException"/> が発生した場合であるが、通常は起こりえない）
        /// </exception>
        public void Start(int? port)
        {
            // HttpListenerが使用可能かチェック
            if (!HttpListener.IsSupported)
            {
                // 使用不可の場合、例外をスロー
                throw new LocalHttpServerException(ErrorMessage.LocalHttpServerErrorNotSupported);
            }

            // HTTPサーバを生成し開始する
            HttpListenerStart(port);

            // ローカルHTTPサーバが起動しているかどうかのフラグをONにする
            IsRunning = true;

            // 非同期でHttpListenerの受信処理を実行する
            // （非同期で実行するが、待たないで処理を終了する）
            _ = Task.Run(() => ReceiveResponseProcess(Listener));
        }

        /// <summary>
        /// ローカルのHTTPサーバを終了する
        /// </summary>
        public void Stop()
        {
            // ローカルHTTPサーバが起動しているかどうかのフラグをOFFにする
            IsRunning = false;

            // HttpListenerを強制終了する
            HttpListenerAbort();
        }

        #endregion

        #region IDisposable インターフェースの Dispoase メソッド

        /// <summary>
        /// リソースを解放する
        /// </summary>
        public void Dispose()
        {
            // リソースの解放処理は「Dispose(bool disposing)」にて実装する
            // ここでは解放処理は行わないこと
            Dispose(true);

            // 不要なファイナライザーを呼び出さないようにする
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースを解放する
        /// </summary>
        /// <param name="disposing">
        /// マネージドオブジェクトを解放するかのフラグ
        /// 下記の用途で使い分ける
        /// ・True：<see cref="Dispose()"/> メソッドからの呼び出し
        /// ・False：デストラクタからの呼び出し
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // マネージドオブジェクトの解放
                    HttpListenerAbort();
                }

                // アンマネージドオブジェクトの解放
                // （現状使用していないので解放処理はなし）

                // 大きなフィールドの解放（NULLの設定）
                Setting = null;

                // Dispose済みのフラグを立てる
                isDisposed = true;
            }
        }

        #endregion

        #region プライベートメソッド

        #region 使用していないポート番号を取得

        /// <summary>
        /// 使用していないポート番号を取得する
        /// </summary>
        /// <exception cref="LocalHttpServerException">
        /// 使用していないポート番号の取得に失敗した場合に発生
        /// </exception>
        /// <returns>現状、使用していないポート番号</returns>
        private int GetUnusedPort()
        {
            // ポート番号の取得に使用する定数を宣言
            int minPort = 49152;
            int maxPort = 65535;
            int maxTryCount = maxPort - minPort;

            // 使用済みのポート情報の配列を取得する
            TcpConnectionInformation[] usedPorts;
            try
            {
                usedPorts = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();
            }
            catch (NetworkInformationException ex)
            {
                // 取得に失敗した場合は、例外をExceptionLocalHttpServerに詰めなおしスローする
                throw new LocalHttpServerException(ErrorMessage.LocalHttpServerErrorNoUnusedPort, ex);
            }

            // 動的・プライベート ポート番号(49152–65535)の範囲で未使用のポート番号を探索する
            int? port = null;
            int tryCount = 0;
            while (tryCount <= maxTryCount)
            {
                // 乱数からポート番号の候補を取得
                int searchPort = RandomPort.Next(minPort, maxPort);

                // 使用済みポートの配列に検索対象のポート番号が含まれていない場合、
                // そのボート番号を戻り値に設定しループを終了する
                if (usedPorts == null
                    || !usedPorts.Any((x) => x.LocalEndPoint.Port == searchPort))
                {
                    // 乱数から取得したポート番号が未使用ポートである場合、
                    // ポート番号を保持しループを抜ける
                    port = searchPort;
                    break;
                }

                // 乱数から取得したポートが使用済みポートの場合
                tryCount++;
            }

            // ポート番号が取得できない場合は例外をスローする
            if (port == null)
            {
                throw new LocalHttpServerException(ErrorMessage.LocalHttpServerErrorNoUnusedPort);
            }

            // 取得したポート番号を返却
            return port.Value;
        }

        #endregion

        #region HTTPサーバを生成し開始（再起処理で開始を繰り返し試みる）

        /// <summary>
        /// HTTPサーバを生成し開始する
        /// </summary>
        /// <remarks>
        /// 再帰的にHTTPサーバの開始の試みを繰り返すためのメソッド
        /// </remarks>
        /// <param name="port">
        /// ポート番号を指定する場合に設定するポート番号
        /// 指定しない場合、または指定したポート番号が使用できない場合は乱数からポート番号を取得し使用する
        /// </param>
        /// <param name="tryCount">
        /// 再起処理でHTTPサーバの開始を試みた回数（初回はNULLを指定）
        /// </param>
        private void HttpListenerStart(int? port = null, int? tryCount = null)
        {
            // 開始処理の最大試行回数を定義
            int maxTryCount = 5;

            // 引数にポート番号の指定がある場合はそれを使用、
            // 指定がない場合は利用可能なポート番号を使用しURLを設定
            Url = new Uri(string.Format(
                CultureInfo.InvariantCulture, LocalUrlFormat, port ?? GetUnusedPort()));

            // 試行回数を設定
            int nowTryCount = tryCount ?? 0;
            nowTryCount++;

            // HTTPサーバを生成し開始する
            try
            {
                Listener = new HttpListener();
                Listener.Prefixes.Add(Url.ToString());
                Listener.Start();
            }
            catch (HttpListenerException ex)
            {
                // エラーがHttpListenerExceptionの場合

                // HttpListenerを強制終了する
                HttpListenerAbort();

                // エラーの内容が競合の場合、
                // 異なるポート番号で実行を行うため、再起で再びHTTPサーバの開始処理を行う
                // ただし最大試行回数を超えている場合は再起は行わずエラーとして処理する
                if (ex.ErrorCode == 183
                    && nowTryCount <= maxTryCount)
                {
                    HttpListenerStart(null, nowTryCount);

                    // 正常に開始できた場合は処理を終了する
                    return;
                }

                // 競合のエラーでない、または最大試行回数を超えた場合は
                // ExceptionLocalHttpServerに詰めなおしてスローする
                string message = string.Format(
                        CultureInfo.InvariantCulture,
                        ErrorMessage.LocalHttpServerErrorNotStartHttpListenerErrorFormat,
                        ex.ErrorCode.ToString(CultureInfo.InvariantCulture));

                throw new LocalHttpServerException(message, ex);
            }
            catch (PlatformNotSupportedException ex)
            {
                // HTTPサーバが起動できない場合

                // HttpListenerを強制終了する
                HttpListenerAbort();

                // ExceptionLocalHttpServerに詰めなおしてスローする
                throw new LocalHttpServerException(ErrorMessage.LocalHttpServerErrorNotStart, ex);
            }
            catch
            {
                // 上記以外の例外の場合
                // HttpListenerを強制終了して、そのままスローする
                HttpListenerAbort();
                throw;
            }
        }

        #endregion

        #region HttpListenerの強制終了

        /// <summary>
        /// プロパティ（<see cref="Listener"/>）の <see cref="HttpListener"/> オブジェクトを強制終了する
        /// </summary>
        private void HttpListenerAbort()
        {
            // ローカルHTTPサーバが起動しているかどうかのフラグをOFFにする
            // （エラー処理で呼び出された時のためフラグ設定処理）
            IsRunning = false;

            // サーバが停止するためURLをNULLにする
            Url = null;

            // HttpListenerを強制終了する
            try
            {
                // サーバ自体を止めるため要求キューを処理せずに終了する
                Listener?.Abort();
            }
            finally
            {
                // HttpListenerを初期化する
                Listener = null;
            }
        }

        #endregion

        #region 非同期で行うHttpListenerの受信返信処理

        /// <summary>
        /// <see cref="HttpListener"/> の受信返信処理を非同期で行う
        /// </summary>
        /// <param name="listener">ローカルHTTPサーバのリスナーオブジェクト</param>
        /// <returns>非同期処理のタスクオブジェクト</returns>
        private async Task ReceiveResponseProcess(HttpListener listener)
        {
            try
            {
                // ローカルHTTPサーバが起動しているかどうかのフラグがOFFになるまで無限ループ
                while (IsRunning)
                {
                    // 受信が完了するまで待ち、要求オブジェクトを取得する
                    HttpListenerContext context = await listener.GetContextAsync().ConfigureAwait(false);
                    HttpListenerRequest request = context.Request;

                    // レスポンスデータ用のStreamオブジェクトを生成
                    using (Stream responseDataStream = new MemoryStream())
                    {
                        // レスポンスデータをStreamに設定する
                        SetResponseData(
                            request: request,
                            responseDataStream: responseDataStream,
                            dataSize: out long dataSize,
                            mimeType: out string mimeType,
                            statusCode: out int statusCode);

                        // レスポンスオブジェクトを取得する
                        using (HttpListenerResponse response = context.Response)
                        {
                            // ステータスコードの設定
                            response.StatusCode = statusCode;

                            // コンテンツタイプの設定
                            response.ContentType = mimeType;

                            // データサイズの設定
                            response.ContentLength64 = dataSize;

                            // レスポンス処理
                            LocalHttpServerCommon.SetDataToStream(
                                response.OutputStream, responseDataStream);
                        }
                    }
                }
            }
            catch (Exception ex)
                when (ex is HttpListenerException
                    || ex is ArgumentException
                    || ex is RegexMatchTimeoutException
                    || ex is InvalidOperationException
                    || ex is NotSupportedException
                    || ex is IOException)
            {
                // 下記の例外が発生する可能性がある
                // [HttpListenerException]
                // ・Win32 関数の呼び出しが失敗しました。
                // 　エラーの詳細は HttpListenerException.ErrorCode プロパティを参照
                //
                // [ArgumentException]
                // ・レスポンスで設定するContentTypeの値がNULL又は空文字の場合に発生
                // 　（HttpResponseDelegateの生成時に値のチェックを行っているので
                //     基本的に発生しない発生しないはず、発生したらバグ）
                // 　[ArgumentNullException]
                // 　[ArgumentException]
                // ・レスポンスで設定するContentLength64の値が0未満の場合に発生
                // 　（基本的に発生しない発生しないはず、
                // 　　コマンドラインユーザーインターフェース発生した場合は
                // 　　レスポンスデータ取得用のデリゲートにバグがあり不正な値を返却している）
                // 　[ArgumentOutOfRangeException]
                //
                // [RegexMatchTimeoutException]
                // ・対象のレスポンス処理を取得する際に、
                // 　正規表現のマッチング処理にてタイムアウトが発生した場合に発生
                //
                // [InvalidOperationException]
                // １．下記の場合に発生
                // 　[ObjectDisposedException]
                // 　・処理中にHttpListenerオブジェクトがDisposedされていた場合
                // 　・処理中にHttpListenerResponseオブジェクトがDisposedされていた場合
                // 　・処理中にレスポンスデータを読み込むStreamオブジェクトがDisposedされていた場合
                // 　・処理中にレスポンスを出力するStreamオブジェクトがDisposedされていた場合
                // ２．下記の場合に発生
                // 　[InvalidOperationException]
                // 　・HttpListenerオブジェクトが開始されていないか、停止中の場合
                // 　・レスポンスで設定するContentLength64の処理中に既にレスポンスが返却済みの場合
                //
                // [NotSupportedException]
                // ・レスポンスデータ読み込み用のStreamが読み込みをサポートしていない場合に発生
                // ・レスポンスデータ書き込み用のStreamが書き込みをサポートしていない場合に発生
                //
                // [IO.IOException]
                // ・レスポンスデータの読み書きでI/O エラーが発生した場合に発生

                // エラーに対する処理を行う
                ErrorHandling(ex);
            }
            finally
            {
                // HttpListenerを強制終了する
                HttpListenerAbort();
            }
        }

        /// <summary>
        /// 引数のHTTPリクエスト（<paramref name="request"/>）に対するレスポンスデータを取得する
        /// </summary>
        /// <remarks>
        /// 【CA1031】の抑止について
        /// 任意のデリゲートの指定ができ発生する例外が推測不可能 かつ、
        /// HTTPサーバの内部エラーとして処理をさせたいため Exception によるCatchが必要である
        /// そのためこのメソッドにおいては【CA1031】の警告を抑止している
        /// </remarks>
        /// <param name="request">HTTPリクエストデータ</param>
        /// <param name="responseDataStream">レスポンスデータを格納するためのストリーム</param>
        /// <param name="dataSize">レスポンスデータのデータサイズ</param>
        /// <param name="mimeType">レスポンスデータの MimeType</param>
        /// <param name="statusCode">レスポンスデータのステータスコード</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "任意のデリゲートの指定ができ発生する例外が推測不可能、かつ、HTTPサーバの内部エラーとして処理をさせたいため Exception によるCatchを実装するため抑止")]
        private void SetResponseData(
            HttpListenerRequest request,
            Stream responseDataStream,
            out long dataSize,
            out string mimeType,
            out int statusCode)
        {
            try
            {
                // レスポンスデータを取得するためのデリゲート情報を取得する
                HttpResponseData responseData = GetTargetResponseData(request);
                dataSize = responseData.SetStream(responseDataStream, request);
                mimeType = responseData.MimeType;
                statusCode = 200;
            }
            catch (Exception ex)
            {
                // レスポンスデータ取得処理でエラーが発生した場合

                // スタックトレース付きのエラーメッセージを取得
                string errorMessage = ExceptionHandling.GetErrorMessageWithStackTrace(ex);

                // エラー用のHTMLレスポンスデータを生成
                string errorResponse = string.Format(
                    CultureInfo.InvariantCulture,
                    ErrorMessage.LocalHttpServerErrorResponseHtmlFormat,
                    errorMessage);
                byte[] responseData = System.Text.Encoding.UTF8.GetBytes(errorResponse);

                // 500:Internal Server Errorレスポンスを返却
                LocalHttpServerCommon.SetDataToStream(responseDataStream, responseData);
                dataSize = responseData.LongLength;
                mimeType = "text/html";
                statusCode = 500;
            }
        }

        /// <summary>
        /// 引数（<paramref name="request"/>）に紐づくレスポンスデータ情報を取得する
        /// </summary>
        /// <param name="request">HTTPリクエスト</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="request"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="RegexMatchTimeoutException">
        /// 正規表現のマッチング処理にてタイムアウトが発生した場合に発生
        /// </exception>
        /// <returns>レスポンスデータ情報</returns>
        private HttpResponseData GetTargetResponseData(HttpListenerRequest request)
        {
            // NULLチェック
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // favicon.iconのリクエストかチェック
            if (request.HttpMethod.Equals(
                    HttpMethod.Get.ToString(), StringComparison.OrdinalIgnoreCase)
                && request.RawUrl.Equals(
                    LocalHttpServerResponseSetting.FaviconUrl, StringComparison.OrdinalIgnoreCase))
            {
                // favicon.iconのリクエストの場合は、それfavicon.icon用のレスポンスデータ情報を返却
                return Setting?.FaviconResponse ?? LocalHttpServerCommon.EmptyDataResponse;
            }

            // HttpMethod、URLに紐づくレスポンスデータ情報を取得
            LocalHttpServerResponceProcess responceProcess
                = Setting?.Find(request.HttpMethod, request.RawUrl);

            // 取得したレスポンスデータ情報を返却
            // 取得できなかった場合はデフォルト用のレスポンスデータ情報を返却
            HttpResponseData defaultProcess
                = Setting?.DefaultResponse ?? LocalHttpServerCommon.EmptyDataResponse;
            return responceProcess?.ResponseData ?? defaultProcess;
        }

        /// <summary>
        /// <see cref="HttpListener"/> の受信返信処理におけるエラー処理を行う
        /// </summary>
        /// <param name="ex">発生した例外</param>
        private void ErrorHandling(Exception ex)
        {
            // 実行中のエラーか判定する
            // （既に終了している場合は強制終了によるエラーのため無視する）
            if (IsRunning)
            {
                // 実行中のエラーの場合
                // エラーメッセージを生成する
                string message;
                if (ex is HttpListenerException listenerException)
                {
                    // HttpListenerException の場合、エラーコード付きのメッセージを生成する
                    message = string.Format(
                        CultureInfo.InvariantCulture,
                        ErrorMessage.LocalHttpServerErrorHttpListenerErrorFormat,
                        listenerException.ErrorCode);
                }
                else
                {
                    // 上記以外の場合、共通のエラーメッセージを生成する
                    message = ErrorMessage.LocalHttpServerError;
                }

                // 非同期処理のため例外をスローしても戻り先が存在しない
                // そのため例外をスローせず、ここでエラーメッセージを表示する
                // （単独で実行させるためメインスレッドから切り離している）
                ExceptionHandling.Error(ex, message);
            }
        }

        #endregion

        #endregion
    }
}
