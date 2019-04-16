namespace MisaCommon.CustomType.Attribute
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// ローカライズを可能とするプロパティまたはイベントの表示をグループ化するための
    /// <see cref="CategoryAttribute"/> の派生クラス
    /// </summary>
    /// <example>
    /// このクラスの使用例（下記のリソースクラスが存在している前提での使用例）
    /// 【リソースクラス】
    /// ・クラス名：LocalizeResources
    /// ・設定値
    /// 　キー：keyA   値：カテゴリＡ
    /// 　キー：keyB   値：カテゴリＢ
    /// 　キー：keyC   値：カテゴリＣ
    ///
    /// 【コード】
    /// パターン１：定義済みカテゴリを使用
    /// <code>
    /// <![CDATA[
    ///     // 任意のプロパティ
    ///     [LocalizableCategory(DefinedCategory.Action)]
    ///     public string SampleProperty { get; set; }
    /// ]]>
    /// </code>
    /// パターン２：リソースクラスを使用
    /// <code>
    /// <![CDATA[
    ///     // 任意のプロパティ
    ///     [LocalizableCategory("keyB", typeof(LocalizeResources))]
    ///     public string SampleProperty { get; set; }
    /// ]]>
    /// </code>
    /// パターン３：リソースクラスを使用し、キーに該当するデータが存在しない場合
    /// <code>
    /// <![CDATA[
    ///     // 任意のプロパティ
    ///     [LocalizableCategory("存在しないキー", typeof(LocalizeResources))]
    ///     public string SampleProperty { get; set; }
    /// ]]>
    /// </code>
    ///
    /// 【出力】
    /// パターン１ ⇒ アクション
    /// パターン２ ⇒ カテゴリＢ
    /// パターン３ ⇒ 存在しないキー
    /// </example>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class LocalizableCategoryAttribute : CategoryAttribute
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// 引数からローカライズを考慮したカテゴリ名を生成し、そのカテゴリ名を使用して初期化を行う
        /// </summary>
        /// <param name="definedCategory">定義済みカテゴリ</param>
        public LocalizableCategoryAttribute(DefinedCategory definedCategory)
            : base(DefinedCategoryString.GetString(definedCategory))
        {
            DefinedCategory = definedCategory;
        }

        /// <summary>
        /// コンストラクタ
        /// 引数からローカライズを考慮したカテゴリ名を生成し、そのカテゴリ名を使用して初期化を行う
        /// （リソースから値を取得できなかった場合は、
        /// 　キー（<paramref name="resourceKey"/>）をそのままカテゴリ名とする）
        /// </summary>
        /// <param name="resourceKey">リソースから値を取得するためのキー</param>
        /// <param name="resource">取得元となるリソースクラスの <see cref="Type"/></param>
        /// <exception cref="AmbiguousMatchException">
        /// キー（<paramref name="resourceKey"/>）に対して複数の値が存在した場合（キー重複の場合）に発生
        /// </exception>
        public LocalizableCategoryAttribute(string resourceKey, Type resource)
            : base(CommonLocalizable.GetResourceValue(resource, resourceKey) ?? resourceKey)
        {
            ResourceKey = resourceKey;
            Resource = resource;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 定義済みカテゴリを取得する
        /// コンストラクタで指定されたかった場合は、デフォルトのカテゴリとする
        /// </summary>
        public DefinedCategory DefinedCategory { get; } = DefinedCategory.Default;

        /// <summary>
        /// リソースから値を取得するためのキーを取得する
        /// コンストラクタで指定されたかった場合はNULL
        /// </summary>
        public string ResourceKey { get; } = null;

        /// <summary>
        /// 値の取得元となるリソースクラスの <see cref="Type"/> を取得する
        /// コンストラクタで指定されたかった場合はNULL
        /// </summary>
        public Type Resource { get; } = null;

        #endregion
    }
}
