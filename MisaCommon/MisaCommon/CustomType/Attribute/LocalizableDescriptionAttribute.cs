namespace MisaCommon.CustomType.Attribute
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// ローカライズを可能とするプロパティまたはイベントの表示をグループ化するための
    /// <see cref="CategoryAttribute"/> の派生クラス
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class LocalizableDescriptionAttribute : DescriptionAttribute
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数からローカライズを考慮したプロパティの説明を生成し、その値を使用して初期化を行う
        /// リソースから値を取得できなかった場合はキー（<paramref name="resourceKey"/>）をそのまま説明とする
        /// </summary>
        /// <param name="resourceKey">リソースから値を取得するためのキー</param>
        /// <param name="resource">取得元となるリソースクラスの <see cref="Type"/></param>
        /// <exception cref="AmbiguousMatchException">
        /// キー（<paramref name="resourceKey"/>）に対して複数の値が存在した場合（キー重複の場合）に発生
        /// </exception>
        public LocalizableDescriptionAttribute(string resourceKey, Type resource)
            : base(CommonLocalizable.GetResourceValue(resource, resourceKey) ?? resourceKey)
        {
            ResourceKey = resourceKey;
            Resource = resource;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// リソースから値を取得するためのキーを取得する
        /// コンストラクタで指定されたかった場合はNULL
        /// </summary>
        public string ResourceKey { get; }

        /// <summary>
        /// 値の取得元となるリソースクラスの <see cref="Type"/> を取得する
        /// コンストラクタで指定されたかった場合はNULL
        /// </summary>
        public Type Resource { get; }

        #endregion
    }
}
