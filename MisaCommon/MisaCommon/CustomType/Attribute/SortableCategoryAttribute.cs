namespace MisaCommon.CustomType.Attribute
{
    using System;
    using System.ComponentModel;
    using System.Text;

    /// <summary>
    /// ソートキーを設定可能とするプロパティまたはイベントの表示をグループ化するための
    /// <see cref="CategoryAttribute"/> の派生クラス
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SortableCategoryAttribute : CategoryAttribute
    {
        /// <summary>
        /// ソートキーの最大数
        /// </summary>
        /// <remarks>
        /// カテゴリ名の先頭にタブを付与することで、タブの数の違いでカテゴリ名のソートを実現している。
        /// そのためこの最大数を無駄に増やすと意味のないタブが常に付与されることになり無駄な処理となる。
        /// ちょうどいい数にする必要がある
        /// </remarks>
        public const int MaxSortKey = 20;

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 基底クラス（<see cref="CategoryAttribute"/>）にて定義されるデフォルトのカテゴリ名を使用して初期化を行う
        /// （ソート順：最下位固定）
        /// </summary>
        public SortableCategoryAttribute()
            : base()
        {
            CategoryName = null;
            SortKey = 0;
        }

        /// <summary>
        /// コンストラクタ
        /// 引数のカテゴリ名を使用して初期化を行う
        /// （ソート順：最下位固定）
        /// </summary>
        /// <param name="categoryName">カテゴリ名</param>
        public SortableCategoryAttribute(string categoryName)
            : base(categoryName)
        {
            CategoryName = categoryName;
            SortKey = 0;
        }

        /// <summary>
        /// コンストラクタ
        /// 引数からソートを考慮したカテゴリ名を生成し、そのカテゴリ名を使用して初期化を行う
        /// </summary>
        /// <param name="categoryName">カテゴリ名</param>
        /// <param name="sortKey">
        /// ソートキー
        /// 0～最大値までの値を設定（0を除いて設定した値の昇順でソートする）、0の場合はソート順が最下位となる
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// sortKeyが0～MAXの範囲の数値でない場合に発生
        /// </exception>
        public SortableCategoryAttribute(string categoryName, int sortKey)
            : base(GetSortableCategoryName(categoryName, sortKey))
        {
            CategoryName = categoryName;
            SortKey = sortKey;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// カテゴリ名を取得する
        /// コンストラクタで指定されたかった場合はNULL
        /// </summary>
        public string CategoryName { get; }

        /// <summary>
        /// ソートキーを取得する
        /// 0～最大値までの値を設定（0を除いて設定した値の昇順でソートする）、0の場合はソート順が最下位となる
        /// コンストラクタで指定されたかった場合は0
        /// </summary>
        public int SortKey { get; }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// ソートを可能とするカテゴリ名を取得する
        /// </summary>
        /// <param name="categoryName">元となるカテゴリ名</param>
        /// <param name="sortKey">
        /// ソートキー
        /// 0～最大値までの値を設定（0を除いて設定した値の昇順でソートする）、0の場合はソート順が最下位となる
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// sortKeyが0～MAXの範囲の数値でない場合に発生
        /// </exception>
        /// <returns>ソートを可能とするカテゴリ名</returns>
        private static string GetSortableCategoryName(string categoryName, int sortKey)
        {
            // 引数の範囲チェック
            if (sortKey < 0 || sortKey > MaxSortKey)
            {
                // ソートキーの値が範囲外の場合は例外をスローする
                throw new ArgumentOutOfRangeException(nameof(sortKey));
            }

            // カテゴリ名でソート可能とするため名称の先頭にタブを付与する
            StringBuilder sortableCategory = new StringBuilder();
            int roopCount = sortKey == 0 ? 0 : MaxSortKey - sortKey + 1;
            for (int i = 0; i < roopCount; i++)
            {
                sortableCategory.Append("\t");
            }

            // カテゴリ名を取得
            // カテゴリ名がNULLの場合はデフォルトのカテゴリ名を取得し設定
            string name = categoryName ?? new CategoryAttribute().Category;

            // ソートキーを付与してカテゴリ名を返却
            return sortableCategory.Append(name).ToString();
        }

        #endregion
    }
}
