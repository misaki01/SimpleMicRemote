namespace MisaCommon.CustomType.Attribute
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// プロパティまたはイベントの名称をローカライズ可能とするための
    /// <see cref="DisplayNameAttribute"/> の派生クラス
    /// </summary>
    /// <example>
    /// このクラスの使用例（下記のリソースクラスが存在している前提での使用例）
    /// 【リソースクラス】
    /// ・クラス名：LocalizeResources
    /// ・設定値
    /// 　キー：keyA   値：プロパティの名前_Ａ
    /// 　キー：keyB   値：プロパティの名前_Ｂ
    /// 　キー：keyC   値：プロパティの名前_Ｃ
    ///
    /// 【コード】
    /// パターン１：リソースクラスを使用
    /// <code>
    /// <![CDATA[
    ///     // 任意のプロパティ
    ///     [LocalizableDisplayName("keyA", typeof(LocalizeResources))]
    ///     public string SampleProperty { get; set; }
    /// ]]>
    /// </code>
    /// パターン２：リソースクラスを使用し、キーに該当するデータが存在しない場合
    /// <code>
    /// <![CDATA[
    ///     // 任意のプロパティ
    ///     [LocalizableDisplayName("存在しないキー", typeof(LocalizeResources))]
    ///     public string SampleProperty { get; set; }
    /// ]]>
    /// </code>
    ///
    /// 【出力】
    /// パターン１ ⇒ プロパティの名前_Ａ
    /// パターン２ ⇒ 存在しないキー
    /// </example>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class LocalizableDisplayNameAttribute : DisplayNameAttribute
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// 引数からローカライズを考慮した表示名を生成し、その表示名を使用して初期化を行う
        /// （リソースから値を取得できなかった場合は、
        /// 　キー（<paramref name="resourceKey"/>）をそのまま表示名とする）
        /// </summary>
        /// <param name="resourceKey">リソースから値を取得するためのキー</param>
        /// <param name="resource">取得元となるリソースクラスの <see cref="Type"/></param>
        /// <exception cref="AmbiguousMatchException">
        /// キー（<paramref name="resourceKey"/>）に対して複数の値が存在した場合（キー重複の場合）に発生
        /// </exception>
        public LocalizableDisplayNameAttribute(string resourceKey, Type resource)
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
