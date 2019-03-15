namespace MisaCommon.Utility.ExtendMethod
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// <see cref="TableLayoutRowStyleCollection"/> に拡張メソッドを追加するクラス
    /// </summary>
    public static class TableLayoutRowStyleCollectionExtend
    {
        #region ToArrayの追加

        /// <summary>
        /// <see cref="TableLayoutRowStyleCollection"/> の要素を配列に変換する
        /// </summary>
        /// <param name="collection">
        /// 拡張機能を追加する <see cref="TableLayoutRowStyleCollection"/> オブジェクト
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <see cref="TableLayoutRowStyleCollection"/> オブジェクトがNULLの場合に発生
        /// </exception>
        /// <returns>
        /// <see cref="TableLayoutRowStyleCollection"/> の要素のシャドウコピーを格納した配列
        /// </returns>
        public static RowStyle[] ToArray(this TableLayoutRowStyleCollection collection)
        {
            // NULLチェック
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            // スタイルのリストの配列を生成する
            RowStyle[] rowStyles = new RowStyle[collection.Count];

            // 生成した配列にコレクションの要素をコピーしながら格納する
            int styleIndex = 0;
            foreach (RowStyle style in collection)
            {
                rowStyles[styleIndex] = new RowStyle(style.SizeType, style.Height);
                styleIndex++;
            }

            // 取得した配列を返却する
            return rowStyles;
        }

        #endregion
    }
}
