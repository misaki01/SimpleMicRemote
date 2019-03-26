namespace MisaCommon.CustomType
{
    using System;
    using System.Drawing;

    /// <summary>
    /// カーソルをキャプチャする際に用いる背景情報をまとめて扱うクラス
    /// （Iビームカーソル等の背景に応じで色が変化するカーソルを、
    /// 　正確に描画するために使用するカーソルの下にある背景画像等の情報）
    /// </summary>
    public class CursorBackgroundInfo : IDisposable
    {
        #region クラス変数・定数

        /// <summary>
        /// Dispose処理済みかどうかのフラグ
        /// </summary>
        private bool _isDisposed = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数でプロパティを初期化する
        /// </summary>
        /// <param name="backgroundImage">カーソルの下にある背景画像</param>
        /// <param name="screenPoint">背景画像の画面上の座標（画像の左上の絶対座標）</param>
        public CursorBackgroundInfo(Bitmap backgroundImage, Point screenPoint)
        {
            BackgroundImage = backgroundImage;
            ScreenPoint = screenPoint;
        }

        #endregion

        #region ファイナライザー

        /// <summary>
        /// ファイナライザー
        /// リソースを解放する
        /// </summary>
        ~CursorBackgroundInfo()
        {
            // リソースの解放処理は「Dispose(bool disposing)」にて実装する
            // ここでは解放処理は行わないこと
            Dispose(false);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// カーソルの下にある背景画像
        /// （Iビームカーソル等の背景に応じで色が変化するカーソルを、
        /// 　正確に描画するために使用するカーソルの下にある背景画像）
        /// </summary>
        public Bitmap BackgroundImage { get; set; }

        /// <summary>
        /// 背景画像の画面上の座標（画像の左上の絶対座標）
        /// </summary>
        public Point ScreenPoint { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public CursorBackgroundInfo DeepCopy()
        {
            return new CursorBackgroundInfo(
                backgroundImage: (Bitmap)BackgroundImage.Clone(),
                screenPoint: new Point(ScreenPoint.X, ScreenPoint.Y));
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
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // マネージドオブジェクトの解放
                    // 背景画像の解放
                    BackgroundImage?.Dispose();
                }

                // アンマネージドオブジェクトの解放

                // 大きなフィールドの解放（NULLの設定）

                // Dispose済みのフラグを立てる
                _isDisposed = true;
            }
        }

        #endregion

        #endregion
    }
}
