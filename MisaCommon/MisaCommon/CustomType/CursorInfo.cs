namespace MisaCommon.CustomType
{
    using System.Drawing;

    /// <summary>
    /// カーソルの情報をまとめて扱うクラス
    /// </summary>
    public class CursorInfo
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数でプロパティを初期化する
        /// </summary>
        /// <param name="cursorImage">カーソルの画像</param>
        /// <param name="screenPoint">カーソルのスクリーン上の座標</param>
        public CursorInfo(Image cursorImage, Point screenPoint)
        {
            CursorImage = cursorImage;
            ScreenPoint = screenPoint;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// カーソルの画像
        /// </summary>
        public Image CursorImage { get; set; }

        /// <summary>
        /// カーソルのスクリーン上の座標
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
        public CursorInfo DeepCopy()
        {
            return new CursorInfo(
                cursorImage: CursorImage,
                screenPoint: ScreenPoint);
        }

        #endregion
    }
}
