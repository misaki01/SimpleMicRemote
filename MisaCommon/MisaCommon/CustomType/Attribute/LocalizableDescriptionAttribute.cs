namespace MisaCommon.CustomType.Attribute
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// プロパティまたはイベントの説明をローカライズ可能とするための
    /// <see cref="DescriptionAttribute"/> の派生クラス
    /// </summary>
    /// <example>
    /// このクラスの使用例（下記のリソースクラスが存在している前提での使用例）
    /// 【リソースクラス】
    /// ・クラス名：LocalizeResources
    /// ・設定値
    /// 　キー：keyA   値：プロパティの説明_Ａ
    /// 　キー：keyB   値：プロパティの説明_Ｂ
    /// 　キー：keyC   値：プロパティの説明_Ｃ
    ///
    /// 【コード】
    /// パターン１：リソースクラスを使用
    /// <code>
    /// <![CDATA[
    ///     // 任意のプロパティ
    ///     [LocalizableDescription("keyA", typeof(LocalizeResources))]
    ///     public string SampleProperty { get; set; }
    /// ]]>
    /// </code>
    /// パターン２：リソースクラスを使用し、キーに該当するデータが存在しない場合
    /// <code>
    /// <![CDATA[
    ///     // 任意のプロパティ
    ///     [LocalizableDescription("存在しないキー", typeof(LocalizeResources))]
    ///     public string SampleProperty { get; set; }
    /// ]]>
    /// </code>
    ///
    /// 【出力】
    /// パターン１ ⇒ プロパティの説明_Ａ
    /// パターン２ ⇒ 存在しないキー
    /// </example>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class LocalizableDescriptionAttribute : DescriptionAttribute
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数からローカライズを考慮したプロパティの説明を生成し、その値を使用して初期化を行う
        /// （リソースから値を取得できなかった場合は、
        /// 　キー（<paramref name="resourceKey"/>）をそのまま説明とする）
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
