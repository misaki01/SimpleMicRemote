namespace MisaCommon.CustomType.Attribute
{
    using System;
    using System.Reflection;

    /// <summary>
    /// ローカライズに関する属性における共通処理を行うクラス
    /// </summary>
    internal static class CommonLocalizable
    {
        #region メソッド

        /// <summary>
        /// 引数で指定されたリソースクラス（<paramref name="resource"/>）から、
        /// 引数のキー（<paramref name="key"/>）に該当する値をローカライズを考慮して取得する
        /// 値が取得できなかった場合はNULLを返却
        /// </summary>
        /// <remarks>
        /// このメソッドが呼ばれる前に言語の設定を行う必要がある
        /// 設定していない場合はOSでしている規定の言語を使用する
        /// </remarks>
        /// <param name="resource">取得元となるリソースクラスの <see cref="Type"/></param>
        /// <param name="key">リソースから値を取得するためのキー</param>
        /// <exception cref="AmbiguousMatchException">
        /// キー（<paramref name="key"/>）に対して複数の値が存在した場合（キー重複の場合）に発生
        /// </exception>
        /// <returns>
        /// リソースクラスからローカライズを考慮して取得した値
        /// 値が取得できなかった場合はNULLを返却
        /// </returns>
        internal static string GetResourceValue(Type resource, string key)
        {
            // NULLチェック
            if (resource == null || key == null)
            {
                return null;
            }

            // リソースクラスからキーに紐づくプロパティ情報を取得する
            BindingFlags flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            PropertyInfo property = resource.GetProperty(key, flags);

            // プロパティ情報から値を取得
            string value = property?.GetValue(property) as string;

            // 取得した値を返却（値が取得できなかった場合はNULLを返却）
            return value;
        }

        #endregion
    }
}
