namespace MisaCommon.CustomType
{
    using System;
    using System.Drawing;

    /// <summary>
    /// カーソルの情報をまとめて扱うクラス
    /// </summary>
    public class CursorInfo : IDisposable
    {
        #region クラス変数・定数

        /// <summary>
        /// Dispose処理済みかどうかのフラグ
        /// </summary>
        private bool isDisposed = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// 引数でプロパティを初期化する
        /// </summary>
        /// <param name="cursorImage">カーソルの画像</param>
        /// <param name="screenPoint">カーソルが存在する画面上の座標（絶対座標）</param>
        /// <param name="imagePoint">カーソルが存在する背景イメージ上の座標（相対座標）</param>
        public CursorInfo(Image cursorImage, Point screenPoint, Point imagePoint)
        {
            CursorImage = cursorImage;
            ScreenPoint = screenPoint;
            ImagePoint = imagePoint;
        }

        /// <summary>
        /// コンストラクタ
        /// 引数でプロパティを初期化する
        /// </summary>
        /// <param name="cursorImage">カーソルの画像</param>
        /// <param name="screenPoint">カーソルが存在する画面上の座標（絶対座標）</param>
        public CursorInfo(Image cursorImage, Point screenPoint)
        {
            CursorImage = cursorImage;
            ScreenPoint = screenPoint;
            ImagePoint = screenPoint;
        }

        #endregion

        #region ファイナライザー

        /// <summary>
        /// ファイナライザー
        /// リソースを解放する
        /// </summary>
        ~CursorInfo()
        {
            // リソースの解放処理は「Dispose(bool disposing)」にて実装する
            // ここでは解放処理は行わないこと
            Dispose(false);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// カーソルの画像
        /// </summary>
        public Image CursorImage { get; set; }

        /// <summary>
        /// カーソルが存在する画面上の座標（絶対座標）
        /// </summary>
        public Point ScreenPoint { get; set; }

        /// <summary>
        /// カーソルが存在する画面上の座標（絶対座標）
        /// （背景イメージを指定してカーソルを取得した場合：背景イメージ上にカーソルを描写する座標、
        /// 　背景イメージを指定しないでカーソルを取得した場合：画面上の座標（絶対座標）と同じ値
        /// </summary>
        public Point ImagePoint { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public CursorInfo DeepCopy()
        {
            return new CursorInfo(
                cursorImage: (Image)CursorImage.Clone(),
                screenPoint: new Point(ScreenPoint.X, ScreenPoint.Y),
                imagePoint: new Point(ImagePoint.X, ImagePoint.Y));
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
                    // 背景画像の解放
                    CursorImage?.Dispose();
                }

                // アンマネージドオブジェクトの解放

                // 大きなフィールドの解放（NULLの設定）

                // Dispose済みのフラグを立てる
                isDisposed = true;
            }
        }

        #endregion

        #endregion
    }
}
