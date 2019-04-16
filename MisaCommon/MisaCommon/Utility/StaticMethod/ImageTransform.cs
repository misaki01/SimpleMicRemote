namespace MisaCommon.Utility.StaticMethod
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 画像に関する処理を行うクラス
    /// </summary>
    public static class ImageTransform
    {
        #region クラス変数・定数

        /// <summary>
        /// Bitmap形式として扱うことが可能な拡張子
        /// </summary>
        private static string[] canConvertBitmapExtensions = null;

        #endregion

        #region 画像形式への読み込み

        /// <summary>
        /// このクラスで扱うことが可能な画像データの拡張子を取得する
        /// </summary>
        /// <remarks>
        /// シングルトンパターンで実装
        /// </remarks>
        /// <returns>このクラスで扱うことが可能な画像データの拡張子</returns>
        public static string[] GetCanConvertImageExtensions()
        {
            if (canConvertBitmapExtensions == null)
            {
                // 拡張子が未設定の場合は扱える拡張子を設定する
                List<string> extensionList = new List<string>();
                foreach (ImageCodecInfo codecInfo in ImageCodecInfo.GetImageDecoders())
                {
                    // 拡張子の情報はセミコロン「;」区切りのデータとなっているためばらす
                    foreach (string extemstion in codecInfo.FilenameExtension.Split(';'))
                    {
                        // 拡張子情報の最初の１文字目は「*」が設定されている
                        // それを除外しリストに追加する
                        if (extemstion.Length > 1)
                        {
                            extensionList.Add(extemstion.Substring(1));
                        }
                    }
                }

                // 取得した拡張子リストを設定する
                canConvertBitmapExtensions = extensionList.ToArray();
            }

            return canConvertBitmapExtensions;
        }

        /// <summary>
        /// 入力されたパスから<see cref="Image"/>型の画像データを読み込む
        /// </summary>
        /// <param name="path">
        /// 画像ファイルへのパス
        /// </param>
        /// <param name="image">
        /// 読み込んだ<see cref="Image"/>型の画像データ
        /// （変換できなかった場合はNULL）
        /// </param>
        /// <returns>
        /// <see cref="Image"/>型として読み込める場合：True、読み込めない場合：False
        /// </returns>
        public static bool TryImageLoad(string path, out Image image)
        {
            // outパラメータの初期値を設定
            image = null;

            // 各種チェック
            if (string.IsNullOrWhiteSpace(path))
            {
                // NG：空文字チェック
                return false;
            }

            // 拡張子チェック
            string extension = Path.GetExtension(path).ToUpperInvariant();
            if (!GetCanConvertImageExtensions().Any(canExtension
                => canExtension.ToUpperInvariant().Equals(extension, StringComparison.Ordinal)))
            {
                // NG：読み込み可能な拡張子かチェック
                return false;
            }

            // ファイル存在チェック
            if (!File.Exists(path))
            {
                // NG：
                return false;
            }

            // このクラスではBitmapクラスを使用してImageの複製を行っている、
            // それが行えるか判定する
            try
            {
                image = new Bitmap(path);
            }
            catch (ArgumentException)
            {
                // 下記のエラーが発生する可能性がある
                // [ArgumentException]
                // ・Streamに画像データが含まれていない場合
                // ・画像ファイルが画像データでない場合（破損している場合も含む）
                // ・65535 ピクセルよりも大きいイメージの場合

                // このクラスで扱える画像データでない場合は False を返す
                image?.Dispose();
                image = null;
                return false;
            }

            // 全てのチェックを通過したため True を返す
            return true;
        }

        /// <summary>
        /// 入力されたストリームから<see cref="Image"/>型の画像データを読み込む
        /// </summary>
        /// <param name="stream">
        /// 画像ファイルへのストリーム
        /// </param>
        /// <param name="image">
        /// 読み込んだ<see cref="Image"/>型の画像データ
        /// （変換できなかった場合はNULL）
        /// </param>
        /// <returns>
        /// <see cref="Image"/>型として読み込める場合：True、読み込めない場合：False
        /// </returns>
        public static bool TryImageLoad(Stream stream, out Image image)
        {
            // outパラメータの初期値を設定
            image = null;

            // 各種チェックチェック
            if (stream == null)
            {
                // NG：NULLチェック
                return false;
            }

            // このクラスではBitmapクラスを使用してImageの複製を行っている、
            // そのためBitmap型で読み込めるか判定する
            try
            {
                image = new Bitmap(stream);
            }
            catch (ArgumentException)
            {
                // 下記のエラーが発生する可能性がある
                // [ArgumentException]
                // ・Streamに画像データが含まれていない場合
                // ・画像ファイルが画像データでない場合（破損している場合も含む）
                // ・65535 ピクセルよりも大きいイメージの場合

                // このクラスで扱える画像データでない場合は False を返す
                image?.Dispose();
                image = null;
                return false;
            }

            // 全てのチェックを通過したため True を返す
            return true;
        }

        /// <summary>
        /// 入力されたパスから<see cref="Image"/>型の画像データを読み込むことが可能か判定する
        /// </summary>
        /// <param name="path">画像ファイルへのパス</param>
        /// <returns>
        /// <see cref="Image"/>型として読み込める場合：True、読み込めない場合：False
        /// </returns>
        public static bool CanImageLoad(string path)
        {
            Image image = null;
            try
            {
                return TryImageLoad(path, out image);
            }
            finally
            {
                image?.Dispose();
            }
        }

        /// <summary>
        /// 入力されたストリームから<see cref="Image"/>型の画像データを読み込むことが可能か判定する
        /// </summary>
        /// <param name="stream">画像ファイルへのストリーム</param>
        /// <returns>
        /// <see cref="Image"/>型として読み込める場合：True、読み込めない場合：False
        /// </returns>
        public static bool CanImageLoad(Stream stream)
        {
            Image image = null;
            try
            {
                return TryImageLoad(stream, out image);
            }
            finally
            {
                image?.Dispose();
            }
        }

        #endregion

        #region キャンパスサイズ変更

        /// <summary>
        /// 対象の画像データのキャンパスを引数で指定したサイズ（<paramref name="changeSize"/>）の
        /// キャンパスに変更する
        /// </summary>
        /// <param name="image">対象の画像データ</param>
        /// <param name="changeSize">変更するサイズ</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の画像データがNULLの場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// 変更後のサイズの <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// 又は引数の画像データがインデックス付きピクセル形式かの形式が定義されていない場合に発生
        /// </exception>
        /// <returns>キャンパスサイズを変更した画像データ</returns>
        public static Image ChangeCanvas(Image image, Size changeSize)
        {
            // NULLチェック
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            Point center = new Point(image.Size.Width / 2, image.Size.Height / 2);
            return ChangeCanvas(image, changeSize, center);
        }

        /// <summary>
        /// 対象の画像データのキャンパスを引数で指定した中心位置（<paramref name="changeCenterPoint"/>）を
        /// 中心とするのキャンパスに変更する
        /// </summary>
        /// <param name="image">対象の画像データ</param>
        /// <param name="changeCenterPoint">変更する中心の座標</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の画像データがNULLの場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// 変更後のサイズの <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// 又は引数の画像データがインデックス付きピクセル形式かの形式が定義されていない場合に発生
        /// </exception>
        /// <returns>キャンパスサイズを変更した画像データ</returns>
        public static Image ChangeCanvas(Image image, Point changeCenterPoint)
        {
            // NULLチェック
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            return ChangeCanvas(image, image.Size, changeCenterPoint);
        }

        /// <summary>
        /// 対象の画像データのキャンパスを引数で指定したサイズ（<paramref name="changeSize"/>）
        /// 及び、中心座標（<paramref name="changeCenterPoint"/>）を中心とするキャンパスに変更する
        /// </summary>
        /// <param name="image">対象の画像データ</param>
        /// <param name="changeSize">変更するサイズ</param>
        /// <param name="changeCenterPoint">変更する中心の座標</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の画像データがNULLの場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// 変更後のサイズの <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// 又は引数の画像データがインデックス付きピクセル形式かの形式が定義されていない場合に発生
        /// </exception>
        /// <returns>キャンパスサイズを変更した画像データ</returns>
        public static Image ChangeCanvas(Image image, Size changeSize, Point changeCenterPoint)
        {
            // NULLチェック
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            // 透明な背景に対して引数の画像を合成することで、キャンパスサイズを変更する
            Image combinedImage = new Bitmap(changeSize.Width, changeSize.Height);
            try
            {
                using (Graphics graphics = Graphics.FromImage(combinedImage))
                {
                    // 透明画像と元画像の中心点が重なるように合成する
                    graphics.DrawImage(
                        image: image,
                        x: (int)Math.Truncate((combinedImage.Size.Width / (float)2) - changeCenterPoint.X),
                        y: (int)Math.Truncate((combinedImage.Size.Height / (float)2) - changeCenterPoint.Y),
                        width: image.Size.Width,
                        height: image.Size.Height);
                }

                // 合成したイメージを返す
                return combinedImage;
            }
            catch
            {
                combinedImage.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 対象の画像データのキャンパスのサイズを対角線の長さに拡大したキャンパスに変更する
        /// </summary>
        /// <remarks>
        /// 画像を回転させたときにキャンパスから画像がはみ出ないようにするため、
        /// 対角線の長さにキャンパスサイズを拡大する処理
        /// </remarks>
        /// <param name="image">対象の画像データ</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の画像データがNULLの場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// 変更後のサイズの <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// 又は引数の画像データがインデックス付きピクセル形式かの形式が定義されていない場合に発生
        /// </exception>
        /// <returns>キャンパスサイズを変更した画像データ</returns>
        public static Image ChangeCanvasToDiagonalSize(Image image)
        {
            // NULLチェック
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            // 画像を回転させたときに切れないようにするため
            // 高さ、幅を対角線の長さ（正方形）のサイズにする
            double diagonal = Math.Sqrt(Math.Pow(image.Size.Width, 2) + Math.Pow(image.Size.Height, 2));
            int resize = (int)Math.Ceiling(diagonal);
            Size changeSize = new Size(resize, resize);

            // キャンパスサイズを変更する
            return ChangeCanvas(image, changeSize);
        }

        /// <summary>
        /// 対象の画像データのキャンパスのサイズを引数の中心位置（<paramref name="changeCenterPoint"/>）を
        /// 考慮した対角線の長さに拡大したキャンパスに変更する
        /// </summary>
        /// <remarks>
        /// 画像を回転させたときにキャンパスから画像がはみ出ないようにするため、
        /// 対角線の長さにキャンパスサイズを拡大する処理
        /// </remarks>
        /// <param name="image">対象の画像データ</param>
        /// <param name="changeCenterPoint">変更する中心の座標</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の画像データがNULLの場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// 変更後のサイズの <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// 又は引数の画像データがインデックス付きピクセル形式かの形式が定義されていない場合に発生
        /// </exception>
        /// <returns>キャンパスサイズを変更した画像データ</returns>
        public static Image ChangeCanvasToDiagonalSize(Image image, Point changeCenterPoint)
        {
            // NULLチェック
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            // 中心点から長さが最大となる対角線の２倍の長さに拡大する

            // 中心点から最も長い対角線を検索
            // 左上
            double diagonalLength;
            double leftTop = Math.Pow(changeCenterPoint.X, 2) + Math.Pow(changeCenterPoint.Y, 2);
            diagonalLength = leftTop;

            // 右上
            double rightTop =
                Math.Pow(image.Size.Width - changeCenterPoint.X, 2)
                + Math.Pow(changeCenterPoint.Y, 2);
            diagonalLength = rightTop > diagonalLength ? rightTop : diagonalLength;

            // 左下
            double leftBottom =
                Math.Pow(changeCenterPoint.X, 2)
                + Math.Pow(image.Size.Height - changeCenterPoint.Y, 2);
            diagonalLength = leftBottom > diagonalLength ? leftBottom : diagonalLength;

            // 右下
            double rightBottom =
                Math.Pow(image.Size.Width - changeCenterPoint.X, 2)
                + Math.Pow(image.Size.Height - changeCenterPoint.Y, 2);
            diagonalLength = rightBottom > diagonalLength ? rightBottom : diagonalLength;

            // 中心点から一番長い対角線の２倍のサイズにする
            // 少数点以下は切り上げ
            diagonalLength = Math.Sqrt(diagonalLength);
            int resize = (int)Math.Ceiling(diagonalLength * 2);

            // 画像を回転させたときに切れないようにするため
            // 高さ、幅を対角線の長さ（正方形）にする
            Size changeSize = new Size(resize, resize);

            // キャンパスサイズを変更する
            return ChangeCanvas(image, changeSize, changeCenterPoint);
        }

        #endregion

        #region 画像回転

        /// <summary>
        /// 画像を指定した角度に回転する
        /// </summary>
        /// <param name="image">対象の画像データ</param>
        /// <param name="angle">回転角度</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の画像データがNULLの場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// 変更後のサイズの <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// 又は、引数の画像データがインデックス付きピクセル形式かの形式が定義されていない場合に発生
        /// </exception>
        /// <returns>回転した画像データ</returns>
        public static Image RotateImage(Image image, float angle)
        {
            // NULLチェック
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            // 画像の真ん中を中心点とする
            float cnterPointX = image.Size.Width / 2;
            float cnterPointY = image.Size.Height / 2;

            // 画像を回転
            return RotateImage(image, angle, cnterPointX, cnterPointY);
        }

        /// <summary>
        /// 画像を指定した角度に回転する
        /// </summary>
        /// <param name="image">対象の画像データ</param>
        /// <param name="angle">回転角度</param>
        /// <param name="cnterPointX">中心点の座標X（未指定の場合は画像の中心を適用する）</param>
        /// <param name="cnterPointY">中心点の座標Y（未指定の場合は画像の中心を適用する）</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の画像データがNULLの場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// 変更後のサイズの <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// 又は、引数の画像データがインデックス付きピクセル形式かの形式が定義されていない場合に発生
        /// </exception>
        /// <returns>回転した画像データ</returns>
        public static Image RotateImage(Image image, float angle, float cnterPointX, float cnterPointY)
        {
            // NULLチェック
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            // 回転する画像
            Image rotateImage = new Bitmap(image.Size.Width, image.Size.Height);
            try
            {
                using (Graphics graphics = Graphics.FromImage(rotateImage))
                {
                    // 背景色を透明に指定
                    graphics.Clear(Color.Transparent);

                    // 画像を変形する際の保管モードを指定
                    graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;

                    // 1.回転させるため中心点を左上に移動
                    graphics.TranslateTransform(-cnterPointX, -cnterPointY);

                    // 2.指定した角度に回転
                    graphics.RotateTransform(angle, MatrixOrder.Append);

                    // 3.ずらした中心点を元に戻す
                    graphics.TranslateTransform(cnterPointX, cnterPointY, MatrixOrder.Append);

                    // 画像を描画
                    Rectangle rect = new Rectangle(0, 0, image.Size.Width, image.Size.Height);
                    graphics.DrawImageUnscaledAndClipped(image, rect);
                }

                // 回転させた画像を返却
                return rotateImage;
            }
            catch
            {
                rotateImage.Dispose();
                throw;
            }
        }

        #endregion
    }
}
