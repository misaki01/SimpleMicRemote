namespace MisaCommon.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Windows.Media.Imaging;

    using MisaCommon.Exceptions;
    using MisaCommon.MessageResources;
    using MisaCommon.Utility;
    using MisaCommon.Utility.ExtendMethod;

    /// <summary>
    /// Gif形式へのエンコードを行うクラス
    /// </summary>
    public class GifEncoder : IDisposable
    {
        #region クラス変数・定数

        /// <summary>
        /// Gifのディレイの単位
        /// （Gifは1/100秒単位で各フレームのディレイを設定する）
        /// </summary>
        /// <remarks>
        /// Gifのディレイの設定は1/100秒単位で行うため100を指定
        /// </remarks>
        public const int GifDelayUnit = 100;

        /// <summary>
        /// 保存処理で使用する閾値（10M）
        /// 保存していないデータのデータサイズがこの閾値を超えた場合に都度保存を行う
        /// </summary>
        private const long DefaultThresholdForSave = 10 * 1024 * 1024;

        /// <summary>
        /// このクラスで生成したGif形式へエンコードしたデータの出力用ストリーム
        /// （コンストラクタの引数として渡された保存先のパスからコンストラクタで生成）
        /// </summary>
        private Stream thisClassGeneratedOutputStream = null;

        /// <summary>
        /// 出力先のストリームへバイナリデータを書き込むためのWriter
        /// </summary>
        private BinaryWriter writer = null;

        /// <summary>
        /// このクラスに追加された画像データをGif形式に変換したデータ
        /// 1行が 1フレーム（画像1枚分）のデータに相当する
        /// </summary>
        private List<GifFrameData> gifData;

        /// <summary>
        /// Dispose処理済みかどうかのフラグ
        /// </summary>
        private bool isDisposed = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="outputStream">
        /// Gif形式へエンコードしたデータの出力用ストリーム
        /// </param>
        /// <param name="isRoop">
        /// 作成するGifがループするかのフラグ
        /// ループする場合：True、しない場合：False
        /// </param>
        /// <param name="roopCount">
        /// ループする場合に設定するループする回数
        /// 0以下の値を設定した場合は無限ループとする
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の出力用ストリーム（<paramref name="outputStream"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(Stream outputStream, bool isRoop, short roopCount)
        {
            // 出力用のストリームに関する設定
            CallerGeneratedOutputStream
                = outputStream ?? throw new ArgumentNullException(nameof(outputStream));
            SavePath = null;
            IsWriting = false;

            // 各プロパティの初期値を設定
            IsRoop = isRoop;
            RoopCount = roopCount;
        }

        /// <summary>
        /// コンストラクタ（Gifが無限ループするかを指定する場合）
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="outputStream">
        /// Gif形式へエンコードしたデータの出力用ストリーム
        /// </param>
        /// <param name="isInfiniteRoop">
        /// 作成するGifが無限ループするかのフラグ
        /// 無限ループする場合：True、ループしない場合：False
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の出力用ストリーム（<paramref name="outputStream"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(Stream outputStream, bool isInfiniteRoop)
            : this(outputStream, isInfiniteRoop, 0)
        {
        }

        /// <summary>
        /// コンストラクタ（Gifが指定した回数分ループする場合）
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="outputStream">
        /// Gif形式へエンコードしたデータの出力用ストリーム
        /// </param>
        /// <param name="roopCount">
        /// ループする回数
        /// 0以下の値を設定した場合は無限ループとする
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の出力用ストリーム（<paramref name="outputStream"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(Stream outputStream, short roopCount)
            : this(outputStream, true, roopCount)
        {
        }

        /// <summary>
        /// コンストラクタ（ループしないGifの場合）
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="outputStream">
        /// Gif形式へエンコードしたデータの出力用ストリーム
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の出力用ストリーム（<paramref name="outputStream"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(Stream outputStream)
            : this(outputStream, false, 1)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="savePath">
        /// Gif形式へエンコードしたデータの保存先のパス
        /// </param>
        /// <param name="isRoop">
        /// 作成するGifがループするかのフラグ
        /// ループする場合：True、しない場合：False
        /// </param>
        /// <param name="roopCount">
        /// ループする場合に設定するループする回数
        /// 0以下の値を設定した場合は無限ループとする
        /// </param>
        /// <param name="isAppend">
        /// 保存先に既にファイルが存在する場合、
        /// ループする場合に設定するループする回数
        /// 0以下の値を設定した場合は無限ループとする
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の保存先のパス（<paramref name="savePath"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(string savePath, bool isAppend, bool isRoop, short roopCount)
        {
            // 出力用のストリームに関する設定
            CallerGeneratedOutputStream = null;
            SavePath = savePath ?? throw new ArgumentNullException(nameof(savePath));

            // 追記を行う かつ、追記するファイルが存在する場合は、出力中フラグを ON にする
            IsWriting = isAppend && File.Exists(savePath);

            // 各プロパティの初期値を設定
            IsRoop = isRoop;
            RoopCount = roopCount;
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="savePath">
        /// Gif形式へエンコードしたデータの保存先のパス
        /// </param>
        /// <param name="isRoop">
        /// 作成するGifがループするかのフラグ
        /// ループする場合：True、しない場合：False
        /// </param>
        /// <param name="roopCount">
        /// ループする場合に設定するループする回数
        /// 0以下の値を設定した場合は無限ループとする
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の保存先のパス（<paramref name="savePath"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(string savePath, bool isRoop, short roopCount)
            : this(savePath, false, isRoop, roopCount)
        {
        }

        /// <summary>
        /// コンストラクタ（Gifが無限ループするかを指定する場合）
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="savePath">
        /// Gif形式へエンコードしたデータの保存先のパス
        /// </param>
        /// <param name="isInfiniteRoop">
        /// 作成するGifが無限ループするかのフラグ
        /// 無限ループする場合：True、ループしない場合：False
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の保存先のパス（<paramref name="savePath"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(string savePath, bool isInfiniteRoop)
            : this(savePath, isInfiniteRoop, 0)
        {
        }

        /// <summary>
        /// コンストラクタ（Gifが指定した回数分ループする場合）
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="savePath">
        /// Gif形式へエンコードしたデータの保存先のパス
        /// </param>
        /// <param name="roopCount">
        /// ループする回数
        /// 0以下の値を設定した場合は無限ループとする
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の保存先のパス（<paramref name="savePath"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(string savePath, short roopCount)
            : this(savePath, true, roopCount)
        {
        }

        /// <summary>
        /// コンストラクタ（ループしないGifの場合）
        /// 各プロパティの初期化を行う
        /// </summary>
        /// <param name="savePath">
        /// Gif形式へエンコードしたデータの保存先のパス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の保存先のパス（<paramref name="savePath"/>）がNULLの場合に発生
        /// </exception>
        public GifEncoder(string savePath)
            : this(savePath, false, 1)
        {
        }

        #endregion

        #region ファイナライザー

        /// <summary>
        /// ファイナライザー
        /// リソースを解放する
        /// </summary>
        ~GifEncoder()
        {
            // リソースの解放処理は「Dispose(bool disposing)」にて実装する
            // ここでは解放処理は行わないこと
            Dispose(false);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 都度保存を行うかのフラグを取得・設定する（デフォルト：False）
        /// True：
        /// 　メモリの消費を抑えるため、Gifデータのデータサイズが
        /// 　閾値（<see cref="ThresholdForSave"/>）を超える度に保存を行う
        /// 　都度保存する場合でも最後のデータは保存しないため
        /// 　<see cref="Save()"/> メソッドの呼び出しは必要である
        /// 　（閾値のデフォルト値は10M）
        /// False：
        /// 　<see cref="Save()"/>メソッドを明示的に呼び出さないと保存しない
        /// </summary>
        public bool IsEachTimeSave { get; set; } = false;

        /// <summary>
        /// 都度の保存処理に使用する閾値を取得・設定する（デフォルト：10M）
        /// 保存していないGifデータのデータサイズがこの閾値に設定した値を超えた場合に都度保存を行う
        /// </summary>
        public long ThresholdForSave { get; set; } = DefaultThresholdForSave;

        /// <summary>
        /// 現在このクラスで保持している保存していないデータのデータサイズを取得する
        /// </summary>
        public long NotSaveDataSize { get; private set; } = 0;

        /// <summary>
        /// 呼び出し元で生成したGif形式へエンコードしたデータの出力用ストリーム
        /// （コンストラクタの引数として渡される）
        /// </summary>
        private Stream CallerGeneratedOutputStream { get; set; }

        /// <summary>
        /// Gif形式へエンコードしたデータの出力用ストリームを取得する
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、
        /// 保存先のパス（<see name="SavePath"/>）が空文字
        /// または、<see cref="Path.GetInvalidPathChars"/> で定義される無効な文字が含まれている場合に発生
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、
        /// 保存先のパス（<see name="SavePath"/>）にドライブラベル（C:\）の一部ではない
        /// コロン文字（:）が含まれている場合に発生
        /// </exception>
        /// <exception cref="IOException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、下記の場合に発生
        /// ・保存先のパス（<see name="SavePath"/>）がシステム定義の最大長を超えている場合
        /// 　（Windowsでは、パスは 248 文字未満、ファイル名は 260 文字未満にする必要がある）
        /// 　[<see cref="PathTooLongException"/>]
        /// ・保存先のパス（<see name="SavePath"/>）が示すディレクトリが正しくない場合
        /// 　（マップされていないドライブ名が指定されている場合等）
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、
        /// 保存先のパス（<see name="SavePath"/>）に、
        /// 隠しファイル等のアクセスできないファイルが既に存在している場合に発生
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、
        /// 呼び出し元に、必要なアクセス許可がない場合に発生
        /// </exception>
        private Stream OutputStream
        {
            get
            {
                if (CallerGeneratedOutputStream != null)
                {
                    // 呼び出し元から出力ストリームが渡されている場合は、
                    // その出力ストリームを返す
                    return CallerGeneratedOutputStream;
                }
                else if (thisClassGeneratedOutputStream != null)
                {
                    // 呼び出し元から保存先のパスを渡されている場合で、
                    // 既にそのパスから出力ストリームを生成済みの場合
                    // その出力ストリームを返す
                    return thisClassGeneratedOutputStream;
                }

                // 呼び出し元から保存先のパスを渡されている場合で、
                // 出力ストリームを生成していない場合そのパスから出力ストリームを生成し返す
                // （追記する場合は Open、追記しない場合は Create モードでストリームを生成する）
                thisClassGeneratedOutputStream
                    = new FileStream(SavePath, IsWriting ? FileMode.Open : FileMode.Create);

                // 追記の場合は現在位置を最後にする
                if (IsWriting)
                {
                    thisClassGeneratedOutputStream.Seek(
                        thisClassGeneratedOutputStream.Length - 1, SeekOrigin.Current);
                }

                return thisClassGeneratedOutputStream;
            }
        }

        /// <summary>
        /// 出力先のストリームへバイナリデータを書き込むための Writer を取得する
        /// </summary>
        /// <remarks>
        /// シングルトンパターンで実装
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// 以下の場合に発生する
        /// ・出力用ストリーム（<see name="OutputStream"/>）が書き込みをサポートしていない、
        /// 　または、既に閉じられている場合
        /// ・呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、
        /// 　その保存先パスが空文字 または、
        /// 　<see cref="Path.GetInvalidPathChars"/> で定義される無効な文字が含まれている場合
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、
        /// 保存先のパス（<see name="SavePath"/>）にドライブラベル（C:\）の一部ではない
        /// コロン文字（:）が含まれている場合に発生
        /// </exception>
        /// <exception cref="IOException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、下記の場合に発生
        /// ・保存先のパス（<see name="SavePath"/>）がシステム定義の最大長を超えている場合
        /// 　（Windowsでは、パスは 248 文字未満、ファイル名は 260 文字未満にする必要がある）
        /// 　[<see cref="PathTooLongException"/>]
        /// ・保存先のパス（<see name="SavePath"/>）が示すディレクトリが正しくない場合
        /// 　（マップされていないドライブ名が指定されている場合等）
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、
        /// 保存先のパス（<see name="SavePath"/>）に、
        /// 隠しファイル等のアクセスできないファイルが既に存在している場合に発生
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// 呼び出し元から保存先のパス（<see name="SavePath"/>）を渡された場合において、
        /// 呼び出し元に、必要なアクセス許可がない場合に発生
        /// </exception>
        private BinaryWriter Writer
        {
            get
            {
                if (writer == null)
                {
                    writer = new BinaryWriter(OutputStream);
                }

                return writer;
            }
        }

        /// <summary>
        /// Gif形式へエンコードしたデータの保存先のパス
        /// </summary>
        private string SavePath { get; }

        /// <summary>
        /// 出力用ストリームへ出力中かどうかを示すフラグを取得・設定する
        /// 一度でも出力用ストリームへ出力を行った場合：True、
        /// まだ出力用ストリームへの出力を一度でも行っていない場合：False
        /// </summary>
        private bool IsWriting { get; set; }

        /// <summary>
        /// ループするGifを作成するかのフラグを取得する
        /// ループする場合：True、しない場合：False
        /// </summary>
        private bool IsRoop { get; }

        /// <summary>
        /// ループする場合に設定するループ回数を取得する
        /// 0以下の値を設定した場合は無限ループとする
        /// </summary>
        private short RoopCount { get; }

        /// <summary>
        /// このクラスに追加された画像データをGif形式に変換したデータを取得する
        /// 1行が 1フレーム（画像1枚分）のデータに相当する
        /// </summary>
        /// <remarks>
        /// シングルトンパターンで実装
        /// </remarks>
        private List<GifFrameData> GifData
        {
            get
            {
                if (gifData == null)
                {
                    gifData = new List<GifFrameData>();
                }

                return gifData;
            }
        }

        /// <summary>
        /// このクラスに追加された画像データをGif形式に変換したデータが存在するかのフラグを取得する
        /// 存在する場合：True、しない場合：False
        /// </summary>
        private bool HasGifData => GifData != null && GifData.Count > 0;

        #endregion

        #region メソッド

        /// <summary>
        /// このGifエンコーダーに引数の画像データ（<paramref name="image"/>）を追加する
        /// </summary>
        /// <param name="image">追加する画像データ</param>
        /// <param name="delay">1フレームあたりのディレイ（1/100秒単位）</param>
        /// <exception cref="ArgumentNullException">
        /// 追加する画像データ（<paramref name="image"/>）がNULLの場合に発生
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 1フレームあたりのディレイ（<paramref name="delay"/>）が0未満の値の場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・出力用ストリームが書き込みをサポートしていない または、既に閉じられている場合
        /// ・保存先パスが空文字 または、<see cref="Path.GetInvalidPathChars"/> で定義される
        /// 　無効な文字が含まれている場合
        /// </exception>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">
        /// 引数の画像データ（<paramref name="image"/>）が正しくないイメージ形式の場合に発生
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・出力用ストリームが既に閉じられている場合
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・出力用ストリームがシーク・書き込みをサポートしていない場合
        /// ・保存先のパスにドライブラベル（C:\）の一部ではないコロン文字（:）が含まれている場合
        /// </exception>
        /// <exception cref="IOException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・保存先のパスがシステム定義の最大長を超えている場合
        /// 　[<see cref="PathTooLongException"/>]
        /// 　（Windowsでは、パスは 248 文字未満、ファイル名は 260 文字未満にする必要がある）
        /// ・保存先のパスが示すディレクトリが正しくない場合
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// 　(マップされていないドライブ名が指定されている場合等)
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・保存先のパスにおいて、隠しファイル等のアクセスできないファイルが既に存在している場合に発生
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// 呼び出し元に、必要なアクセス許可がない場合に発生
        /// </exception>
        /// <exception cref="GifEncoderException">
        /// Gifデータへのエンコードに失敗した場合に発生
        /// </exception>
        public void AddImage(Image image, short delay)
        {
            // 引数チェック
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }
            else if (delay < 0)
            {
                // 引数のディレイが0未満の値の場合、例外をスローする
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(delay),
                    actualValue: delay,
                    message: string.Format(
                        CultureInfo.InvariantCulture,
                        CommonMessage.ArgumentOutOfRangeExceptionMessageFormatLessThan,
                        0));
            }

            // BitmapFrameを生成するために使用する、画像データ用のStreamを生成
            MemoryStream imageStream;
            MemoryStream gifStream;
            using (imageStream = new MemoryStream())
            using (gifStream = new MemoryStream())
            {
                // 画像データをメモリに書き出す
                image.Save(imageStream, ImageFormat.Png);

                // メモリから呼び出す位置を設定
                imageStream.Seek(0, SeekOrigin.Begin);

                // BitmapFrameを生成
                BitmapFrame bitmapFrame = BitmapFrame.Create(
                    imageStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);

                // エンコーダーにBitmapFrameを追加する
                GifBitmapEncoder gifBitmapEncoder = new GifBitmapEncoder();
                gifBitmapEncoder.Frames.Add(bitmapFrame);

                // Gifデータにエンコードをする
                try
                {
                    // エンコードを行いGif形式のデータをバイナリ形式で取得する
                    // （メモリーストリームを利用してバイナリデータを取得）
                    gifBitmapEncoder.Save(gifStream);
                    byte[] gif = gifStream.ToArray();

                    // Gifデータのbyte配列からGifのフレーム単位のデータを生成する
                    GifFrameData frameData = new GifFrameData(gif, delay);

                    // 生成したデータのデータサイズを保存していないデータのデータサイズに加算する
                    NotSaveDataSize += frameData.DataSize;

                    // Gifデータのリストに追加する
                    GifData.Add(new GifFrameData(gif, delay));

                    // 自動保存を行う場合 かつ 保存していないデータが閾値を超えている場合、
                    // 保存を行う
                    if (IsEachTimeSave && NotSaveDataSize > ThresholdForSave)
                    {
                        Save();
                    }
                }
                catch (Exception ex)
                    when (ex is InvalidOperationException
                        || ex is ArgumentException)
                {
                    // Gifのデータへのエンコードが失敗
                    // または、生成したデータが不正な場合、例外をスローする
                    throw new GifEncoderException(ErrorMessage.GifEncoderErrorEncodingFailed, ex);
                }
                finally
                {
                    // GifBitmapEncoderを破棄
                    gifBitmapEncoder.Frames.Clear();
                }
            }
        }

        /// <summary>
        /// Gif形式へエンコードしたデータを保存する
        /// </summary>
        /// <exception cref="ArgumentException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・出力用ストリームが書き込みをサポートしていない または、既に閉じられている場合
        /// ・保存先パスが空文字 または、<see cref="Path.GetInvalidPathChars"/> で定義される
        /// 　無効な文字が含まれている場合
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・出力用ストリームが既に閉じられている場合
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・出力用ストリームがシーク・書き込みをサポートしていない場合
        /// ・保存先のパスにドライブラベル（C:\）の一部ではないコロン文字（:）が含まれている場合
        /// </exception>
        /// <exception cref="IOException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・保存先のパスがシステム定義の最大長を超えている場合
        /// 　[<see cref="PathTooLongException"/>]
        /// 　（Windowsでは、パスは 248 文字未満、ファイル名は 260 文字未満にする必要がある）
        /// ・保存先のパスが示すディレクトリが正しくない場合
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// 　(マップされていないドライブ名が指定されている場合等)
        /// ・I/O エラーが発生した場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// コンストラクタで指定した 出力用ストリーム または、保存先パスが以下の場合に発生する
        /// ・保存先のパスにおいて、隠しファイル等のアクセスできないファイルが既に存在している場合に発生
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// 呼び出し元に、必要なアクセス許可がない場合に発生
        /// </exception>
        /// <returns>
        /// 保存に成功した場合：True、
        /// データが存在せず保存が行えない場合：False
        /// </returns>
        public bool Save()
        {
            // 保存するGifデータが存在しない場合は False を返す
            if (!HasGifData)
            {
                return false;
            }

            // 初回の出力処理でないかつデータが存在する場合、末端データを削除する
            if (IsWriting && OutputStream.Length >= GifFrameData.LastLength)
            {
                OutputStream.SetLength(OutputStream.Length - GifFrameData.LastLength);
            }

            // Gifデータをファイルに書き出す
            bool isFirst = true;
            foreach (GifFrameData gifFrameData in GifData)
            {
                // 初回の出力処理の場合 かつ 初回データの場合ヘッダー情報を書き込む
                if (!IsWriting && isFirst)
                {
                    // 初回フラグを OFF にする
                    isFirst = false;

                    // ヘッダー情報を書き込む
                    Writer.Write(gifFrameData.GetHeader());

                    // ループをする場合、ループの情報を書き込む
                    if (IsRoop)
                    {
                        Writer.Write(GetRoopSectionData());
                    }
                }

                // ブロックデータの書き込む
                Writer.Write(gifFrameData.GetBlock());
            }

            // 末端データの書き込む
            Writer.Write(GifData[GifData.Count - 1].GetLast());

            // 出力用ストリームへ出力中かどうかを示すフラグを ON にする
            IsWriting = true;

            // 保存した現在のGifデータをクリアする
            GifData.Clear();

            // 保存していないデータのデータサイズをクリアする
            NotSaveDataSize = 0;

            // 成功 True を返す
            return true;
        }

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
                    // バイナリデータを書き込むための Writerを解放する
                    if (writer != null)
                    {
                        writer.Dispose();
                    }

                    // このクラスで生成した出力用ストリームを解放する
                    if (thisClassGeneratedOutputStream != null)
                    {
                        thisClassGeneratedOutputStream.Dispose();
                    }
                }

                // アンマネージドオブジェクトの解放

                // 大きなフィールドの解放（NULLの設定）
                gifData?.Clear();
                gifData = null;

                // Dispose済みのフラグを立てる
                isDisposed = true;
            }
        }

        #endregion

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// Gifにおいてループに関するセクションのデータを取得する
        /// </summary>
        /// <remarks>
        /// このデータはヘッダーの次に設定するデータである
        /// </remarks>
        /// <returns>Gifにおいてループに関するセクションのデータ</returns>
        private byte[] GetRoopSectionData()
        {
            // ループ回数のbyteデータを取得
            byte[] countByte = RoopCount.GetByte();

            // ループするデータの設定
            byte[] roopData = new byte[19];
            int index = 0;
            roopData[index++] = 0x21;
            roopData[index++] = 0xFF;
            roopData[index++] = 0x0B;
            roopData[index++] = 0x4E;
            roopData[index++] = 0x45;
            roopData[index++] = 0x54;
            roopData[index++] = 0x53;
            roopData[index++] = 0x43;
            roopData[index++] = 0x41;
            roopData[index++] = 0x50;
            roopData[index++] = 0x45;
            roopData[index++] = 0x32;
            roopData[index++] = 0x2E;
            roopData[index++] = 0x30;
            roopData[index++] = 0x03;
            roopData[index++] = 0x01;
            roopData[index++] = countByte[0];
            roopData[index++] = countByte[1];
            roopData[index++] = 0x00;

            // ループデータを返却
            return roopData;
        }

        #endregion

        #region Gifのフレーム単位のデータを扱うクラス

        /// <summary>
        /// Gifのフレーム単位のデータを扱うクラス
        /// </summary>
        private class GifFrameData
        {
            #region クラス変数・定数

            /// <summary>
            /// ヘッダーデータの最小データ長
            /// </summary>
            public const long MinHeaderLength = 13;

            /// <summary>
            /// ブロックデータの最小データ長
            /// </summary>
            public const long MinBlockLength = 8;

            /// <summary>
            /// 末端データのデータ長
            /// </summary>
            public const long LastLength = 1;

            /// <summary>
            /// Gifのヘッダー情報
            /// </summary>
            private byte[] header;

            /// <summary>
            /// Gifのヘッダー情報
            /// </summary>
            private byte[] block;

            /// <summary>
            /// Gifの末端データ
            /// （基本的には 1 Byte のデータ）
            /// </summary>
            private byte[] last;

            #endregion

            #region コンストラクタ

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="frameData">Gifのフレーム単位のデータ</param>
            /// <param name="delay">1フレームあたりのディレイ（1/100秒単位）</param>
            /// <exception cref="ArgumentNullException">
            /// 引数のGifのフレーム単位のデータ（<paramref name="frameData"/>）がNULLの場合に発生
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            /// 1フレームあたりのディレイ（<paramref name="delay"/>）が0未満の値の場合に発生
            /// </exception>
            /// <exception cref="ArgumentException">
            /// 引数のGifのフレーム単位のデータ（<paramref name="frameData"/>）が
            /// Gifとして不正な形式の場合に発生
            /// </exception>
            public GifFrameData(byte[] frameData, short delay)
            {
                // 初期化を行う
                Initialize(frameData, delay);
            }

            #endregion

            #region Enum定義

            /// <summary>
            /// Gifのフレーム単位のデータにおけるデータの種類
            /// </summary>
            private enum GifFrameDataType
            {
                /// <summary>
                /// Gifのヘッダー情報
                /// </summary>
                Header,

                /// <summary>
                /// Gifのブロックデータ
                /// </summary>
                Block,

                /// <summary>
                /// Gifの末端データ
                /// </summary>
                Last,
            }

            #endregion

            #region プロパティ

            /// <summary>
            /// 保持しているデータのデータサイズ
            /// </summary>
            public long DataSize { get; private set; }

            #endregion

            #region メソッド

            /// <summary>
            /// Gifのヘッダー情報を取得する
            /// </summary>
            /// <returns>Gifのヘッダー情報</returns>
            public byte[] GetHeader()
            {
                return header;
            }

            /// <summary>
            /// Gifのブロックデータを取得する
            /// </summary>
            /// <returns>Gifのブロックデータ</returns>
            public byte[] GetBlock()
            {
                return block;
            }

            /// <summary>
            /// Gifの末端データを取得する
            /// （基本的には 1 Byte のデータ）
            /// </summary>
            /// <returns>Gifの末端データ</returns>
            public byte[] GetLast()
            {
                return last;
            }

            #endregion

            #region プライベートメソッド

            /// <summary>
            /// 初期化処理を行う
            /// </summary>
            /// <param name="frameData">Gifのフレーム単位のデータ</param>
            /// <param name="delay">1フレームあたりのディレイ（1/100秒単位）</param>
            /// <exception cref="ArgumentNullException">
            /// 引数のGifのフレーム単位のデータ（<paramref name="frameData"/>）がNULLの場合に発生
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            /// 1フレームあたりのディレイ（<paramref name="delay"/>）が0未満の値の場合に発生
            /// </exception>
            /// <exception cref="ArgumentException">
            /// 引数のGifのフレーム単位のデータ（<paramref name="frameData"/>）が
            /// Gifとして不正な形式の場合に発生
            /// </exception>
            private void Initialize(byte[] frameData, short delay)
            {
                // 引数のチェック
                if (frameData == null)
                {
                    throw new ArgumentNullException(nameof(frameData));
                }
                else if (frameData.LongLength < (MinHeaderLength + MinBlockLength + LastLength))
                {
                    // 引数のフレームデータが最小長よりも短い場合、例外をスローする
                    throw new ArgumentException(ErrorMessage.GifEncoderErrorBadData, nameof(frameData));
                }
                else if (delay < 0)
                {
                    // 引数のディレイが0未満の値の場合、例外をスローする
                    throw new ArgumentOutOfRangeException(
                        paramName: nameof(delay),
                        actualValue: delay,
                        message: string.Format(
                            CultureInfo.InvariantCulture,
                            CommonMessage.ArgumentOutOfRangeExceptionMessageFormatLessThan,
                            0));
                }

                // ループで使用する制御変数の定義
                // 現在のデータの種類
                GifFrameDataType nowType = 0;

                // ヘッダーデータ、ブロックデータ、末尾データのインデックス
                long headerCount = 0;
                long blockCount = 0;
                long lastCount = 0;

                // 全データの長さを取得
                long dataLength = frameData.LongLength;

                // Gifのフレーム単位のデータを探索し、
                // データをヘッダー、ブロック、末端の３つに分割する
                for (long i = 0; i < dataLength; i++)
                {
                    // 現在位置が区切りに該当するデータの場合、
                    // 現在のデータの種類を更新する
                    if (i == 0)
                    {
                        // 開始データの場合、
                        // ヘッダーデータと判定する
                        nowType = GifFrameDataType.Header;
                    }
                    else if ((i + 5) < dataLength
                        && frameData[i - 1] == 0x00
                        && frameData[i] == 0x21
                        && frameData[i + 1] == 0xF9
                        && frameData[i + 2] == 0x04)
                    {
                        // 現在の値から 1 byte 前～ 2 byte 先までのデータが「[00][21][F9][04]」の場合
                        // ブロックデータと判定する
                        // （透過、ディレイ設定に 5 byte 先までのデータを使用するため、
                        // 　長さが足りるかの判定も行っている）
                        nowType = GifFrameDataType.Block;

                        // 既にブロックデータが存在する場合、
                        // 引数のデータがフレーム単位のデータでないため例外を発生させる
                        if (blockCount > 0)
                        {
                            throw new ArgumentException(
                                ErrorMessage.GifEncoderErrorNotSingleFrame, nameof(frameData));
                        }
                    }
                    else if (i == (frameData.LongLength - 1))
                    {
                        // 末端データの場合、
                        // 末端データと判定する
                        nowType = GifFrameDataType.Last;
                    }

                    // 現在のデータの種類に応じてデータ数をカウントする
                    switch (nowType)
                    {
                        case GifFrameDataType.Header:
                            headerCount++;
                            break;
                        case GifFrameDataType.Block:
                            blockCount++;
                            break;
                        case GifFrameDataType.Last:
                            lastCount++;
                            break;
                        default:
                            // 上記以外はなにもしない
                            break;
                    }
                }

                // データのカウントをチェックする
                if (headerCount < MinHeaderLength)
                {
                    // ヘッダーデータが最小長よりも短い場合、例外をスローする
                    throw new ArgumentException(ErrorMessage.GifEncoderErrorBadHeader, nameof(frameData));
                }
                else if (blockCount == 0)
                {
                    // ブロックデータが存在しない場合、例外をスローする
                    throw new ArgumentException(ErrorMessage.GifEncoderErrorNoBlock, nameof(frameData));
                }
                else if (blockCount < MinBlockLength)
                {
                    // ブロックデータが最小長よりも短い場合、例外をスローする
                    throw new ArgumentException(ErrorMessage.GifEncoderErrorBadBlock, nameof(frameData));
                }
                else if (lastCount < LastLength)
                {
                    // 末端データが最小長よりも短い場合、例外をスローする
                    throw new ArgumentException(ErrorMessage.GifEncoderErrorBadLast, nameof(frameData));
                }

                // ヘッダーデータを設定
                byte[] headerData = new byte[headerCount];
                for (long i = 0; i < headerCount; i++)
                {
                    headerData[i] = frameData[i];
                }

                // ブロックデータを設定
                byte[] blockData = new byte[blockCount];
                long blockIndex = 0;
                bool isFirst = true;
                for (long i = headerCount; i < (headerCount + blockCount); i++)
                {
                    // 初回ループ時の場合、
                    // ブロック情報のうちのヘッダー情報について設定を行う
                    if (isFirst)
                    {
                        // 初回フラグを OFF にする
                        isFirst = false;

                        // ブロックデータの区切りを示すデータの設定
                        blockData[blockIndex++] = frameData[i++]; // 21
                        blockData[blockIndex++] = frameData[i++]; // F9
                        blockData[blockIndex++] = frameData[i++]; // 04

                        // 透過処理の値を設定
                        blockData[blockIndex++] = 0x09;
                        i++;

                        // ディレイの設定
                        byte[] delayByte = delay.GetByte();
                        blockData[blockIndex++] = delayByte[0];
                        i++;
                        blockData[blockIndex++] = delayByte[1];
                        i++;
                    }

                    blockData[blockIndex] = frameData[i];
                    blockIndex++;
                }

                // 末端データを設定
                byte[] lastData = new byte[lastCount];
                long lastIndex = 0;
                for (long i = (headerCount + blockCount); i < (headerCount + blockCount + lastCount); i++)
                {
                    lastData[lastIndex] = frameData[i];
                    lastIndex++;
                }

                // クラス変数に値を設定し保持する
                header = headerData;
                block = blockData;
                last = lastData;

                // データサイズを保持する
                DataSize = header.LongLength + block.LongLength + last.LongLength;
            }

            #endregion
        }

        #endregion
    }
}
