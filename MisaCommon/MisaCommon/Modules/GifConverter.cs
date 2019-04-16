namespace MisaCommon.Modules
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;

    using MisaCommon.Exceptions;
    using MisaCommon.Utility;
    using MisaCommon.Utility.StaticMethod;

    /// <summary>
    /// Gif形式への変換処理に関するクラス
    /// </summary>
    public class GifConverter
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// 引数の値で各プロパティを初期化する
        /// </summary>
        /// <param name="savePath">
        /// 変換処理で生成したGifファイルの保存先パス
        /// </param>
        /// <param name="progressAction">
        /// 進捗率の表示を行う処理
        /// （当クラスの変換処理において、0～100の進捗率を設定する）
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の保存先パス（<paramref name="savePath"/>）がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 引数の保存先パス（<paramref name="savePath"/>）が 空 または、空白の場合に発生
        /// </exception>
        public GifConverter(string savePath, Action<int> progressAction)
        {
            // 引数チェック
            if (savePath == null)
            {
                throw new ArgumentNullException(nameof(savePath));
            }
            else if (string.IsNullOrWhiteSpace(savePath))
            {
                throw new ArgumentException(
                    CommonMessage.ArgumentExceptionMessageEmpty, nameof(savePath));
            }

            SavePath = savePath;
            ProgressAction = progressAction;
        }

        /// <summary>
        /// コンストラクタ：進捗率の表示なしパターン
        /// 引数の値で各プロパティを初期化する
        /// </summary>
        /// <param name="savePath">
        /// 変換処理で生成したGifファイルの保存先パス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の保存先パス（<paramref name="savePath"/>）がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 引数の保存先パス（<paramref name="savePath"/>）が 空 または、空白の場合に発生
        /// </exception>
        public GifConverter(string savePath)
            : this(savePath, null)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 変換処理が実行中かどうかを示すフラグを取得する
        /// </summary>
        public bool IsRunning
        {
            get
            {
                // 変換処理が増える度にORで繋げる
                return IsRunningCombineImages;
            }
        }

        /// <summary>
        /// 変換処理で生成したGifファイルの保存先パスを取得する
        /// </summary>
        public string SavePath { get; private set; }

        /// <summary>
        /// 進捗率の表示を行う処理を取得する
        /// （当クラスの変換処理において、0～100の進捗率を設定する）
        /// </summary>
        private Action<int> ProgressAction { get; }

        /// <summary>
        /// 画像連結の変換処理が実行中がどうかを示すフラグを取得・設定する
        /// </summary>
        private bool IsRunningCombineImages { get; set; } = false;

        #endregion

        #region メソッド

        #region 変換処理を停止する

        /// <summary>
        /// 変換処理を停止する
        /// </summary>
        public void Stop()
        {
            // 全ての停止処理を実行する
            // 変換処理が増える度に停止処理を増やす
            StopCombineImages();
        }

        #endregion

        #region 画像連携⇒Gif

        /// <summary>
        /// 指定されたパスの画像を連結し、Gifを生成する処理を開始する
        /// </summary>
        /// <param name="imageFilePaths">
        /// 画像ファイルのパス配列
        /// （配列の順番で連結する、
        /// 　指定されたパスのファイルが画像ファイルでない場合、そのファイルは無視する）
        /// </param>
        /// <param name="frameRate">
        /// 生成するGifのフレームレート
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の画像ファイルのパス配列（<paramref name="imageFilePaths"/>）がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 引数のフレームレート（<paramref name="frameRate"/>）が 0以下の値の場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 変換処理で生成したGifファイルの保存先パス（<see cref="SavePath"/>）に
        /// <see cref="Path.GetInvalidPathChars"/> で定義される無効な文字が含まれている場合に発生
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// 変換処理で生成したGifファイルの保存先パス（<see cref="SavePath"/>）に
        /// ドライブラベル（C:\）の一部ではないコロン文字（:）が含まれている場合
        /// </exception>
        /// <exception cref="IOException">
        /// 変換処理で生成したGifファイルの保存先パス（<see cref="SavePath"/>）が以下の場合に発生する
        /// ・保存先パスがシステム定義の最大長を超えている場合
        /// 　（Windowsでは、パスは 248 文字未満、ファイル名は 260 文字未満にする必要がある）
        /// 　[<see cref="PathTooLongException"/>]
        /// ・保存先パスが示すディレクトリが正しくない場合
        /// 　(マップされていないドライブ名が指定されている場合等)
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 変換処理で生成したGifファイルの保存先パス（<see cref="SavePath"/>）において、
        /// 隠しファイル等のアクセスできないファイルが既に存在している場合に発生
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// 呼び出し元に、必要なアクセス許可がない場合に発生
        /// </exception>
        /// <exception cref="GifEncoderException">
        /// Gifデータへのエンコードに失敗した場合に発生
        /// </exception>
        /// <returns>処理が正常終了した場合 True、中断した場合 False</returns>
        public bool StartCombineImages(string[] imageFilePaths, decimal frameRate)
        {
            // 引数チェック
            if (imageFilePaths == null)
            {
                throw new ArgumentNullException(nameof(imageFilePaths));
            }
            else if (frameRate < 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(frameRate),
                    actualValue: frameRate,
                    message: string.Format(
                        CultureInfo.InvariantCulture,
                        CommonMessage.ArgumentOutOfRangeExceptionMessageFormatOrLess,
                        0));
            }

            // 変換処理を実行する
            try
            {
                // 実行中フラグを ON にする
                IsRunningCombineImages = true;

                // Gifエンコーダー生成
                GifEncoder gifEncoder;
                using (gifEncoder = new GifEncoder(SavePath, true, 0))
                {
                    // 都度都度保存する
                    gifEncoder.IsEachTimeSave = true;

                    // 設定するディレイ用のパラメータの初期値を設定
                    int remainder = 0;
                    int delay;

                    // イメージデータを読み込みGifエンコーダに追加していく
                    int count = 1;
                    foreach (string imagePath in imageFilePaths)
                    {
                        // 実行中でない場合、処理を中断する
                        if (!IsRunningCombineImages)
                        {
                            return false;
                        }

                        // 読み込み可能な画像データのみ追加していく
                        Image image = null;
                        try
                        {
                            if (ImageTransform.TryImageLoad(imagePath, out image))
                            {
                                // 設定するディレイを計算
                                delay = decimal.ToInt32((GifEncoder.GifDelayUnit + remainder) / frameRate);
                                remainder = decimal.ToInt32((GifEncoder.GifDelayUnit + remainder) % frameRate);

                                // 画像データをGifに追加
                                try
                                {
                                    gifEncoder.AddImage(image, (short)delay);
                                }
                                catch (ExternalException)
                                {
                                    // 画像データ不正のエラーの場合は無視する
                                }
                            }
                        }
                        finally
                        {
                            // 読み込んだ画像リソースの解放
                            image?.Dispose();
                        }

                        // 進捗を進める
                        int progressRate = count * 100 / imageFilePaths.Length;
                        ProgressAction?.Invoke(progressRate);
                        count++;
                    }

                    // 生成したGifを保存する
                    gifEncoder.Save();
                }

                // 正常終了：True を返す
                return true;
            }
            finally
            {
                // 実行中フラグを OFF にする
                IsRunningCombineImages = false;
            }
        }

        /// <summary>
        /// 指定されたパスの画像を連結し、Gifを生成する処理を停止する
        /// </summary>
        public void StopCombineImages()
        {
            // 実行中フラグを OFF にする
            IsRunningCombineImages = false;
        }

        #endregion

        #endregion
    }
}
