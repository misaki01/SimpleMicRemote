namespace MisaCommon.CustomType.Converter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// オブジェクトのプロパティ情報オブジェクトをローカライズに対応したものに変換する <see cref="TypeConverter"/> の派生クラス
    /// ※注意：プロパティのみを対象としており、イベント定義等のプロパティ以外のものについては使用できない
    /// </summary>
    /// <remarks>
    /// <para>
    /// プロパティ情報をローカライズ対応したいクラスにおいて、このクラスを属性指定することでローカライズを可能とする
    /// ソートは定義順に行っているためこのコンバータクラスを使用するとアルファベット順にはならない
    /// </para>
    /// <para>
    /// ローカライズさせる文言はリソースファイルにて設定を行う、各属性において設定するリソースのキーは下記の設定とする
    /// ・Category属性：任意の名称
    /// ・Description属性：任意の名称　又は　[プロパティ名]＋_Description（例：Nameの場合、Name_Description）※
    /// ・DisplayName属性：任意の名称　又は　[プロパティ名]＋_DisplayName（例：Nameの場合、Name_DisplayName）※
    /// ※[プロパティ名]＋_Description、_DisplayNameの設定はリソース名の指定を省略した場合に自動的に使用する
    /// </para>
    /// </remarks>
    /// <example>
    /// このクラスの使用例
    /// <code>
    /// <![CDATA[
    /// [TypeConverter(typeof(LocalizableConverter<[リソースクラス]>))]
    /// [アクセス修飾子] class [クラス名]
    /// {
    ///     // パターン１
    ///     [Category("[リソースのキー（任意の名称）]")]     // リソースで該当なしの場合は、「その他」
    ///     [Description("[リソースのキー（任意の名称）]")]  // リソースで該当なしの場合は、[リソースのキー（任意の名称）]の値をそのまま出力
    ///     [DisplayName("[リソースのキー（任意の名称）]")]  // リソースで該当なしの場合は、[プロパティ名]をそのまま出力
    ///     public [プロパティの型] [プロパティ名] { get; set; }
    ///
    ///     // パターン２
    ///     [Category()]    // 省略した場合は「その他」
    ///     [Description()] // 省略した場合は [プロパティ名]+"_Description"をキーとしリソースを検索、該当なしの場合は空欄
    ///     [DisplayName()] // 省略した場合は [プロパティ名]+"_DisplayName"をキーとしリソースを検索、該当なしの場合は[プロパティ名]をそのまま出力
    ///     public [プロパティの型] [プロパティ名] { get; set; }
    ///
    ///     // パターン３
    ///     [SortableCategory("[リソースの名前]", 1)]  // ソート可能なカテゴリ属性の使用も可
    ///     public [プロパティの型] [プロパティ名] { get; set; }
    ///
    ///     …
    /// ]]>
    /// </code>
    /// </example>
    /// <typeparam name="TResouces">
    /// プロパティのカテゴリ、説明、表示名に使用するリソースクラスを指定
    /// </typeparam>
    public class LocalizableConverter<TResouces> : TypeConverter
    {
        #region メソッド

        /// <summary>
        /// 引数で指定されたコンテキスト（<paramref name="context"/>）と属性（<paramref name="context"/>）の条件を使用して、
        /// プロパティ情報（<paramref name="value"/>）からプロパティのコレクションを取得する
        /// そのプロパティコレクションのプロパティをローカライズに対応した（<see cref="LocalizablePropertyDescriptor{TResources}"/>）型に変換して返却する
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="value">
        /// プロパティ情報（<see cref="PropertyInfo"/>）の取得対象となる配列のオブジェクト
        /// </param>
        /// <param name="attributes">
        /// フィルターとして使用される、<see cref="Attribute"/> 型の配列
        /// </param>
        /// <exception cref="NotSupportedException">
        /// 引数の<paramref name="value"/> がプロセス間のリモートオブジェクトの場合に発生
        /// </exception>
        /// <returns>
        /// 指定されたデータ型に対して公開されているプロパティを格納している、ローカライズに対応した <see cref="PropertyDescriptor"/> のコレクション
        /// プロパティが格納されていない場合はNULL
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            // PropertyDescriptorコレクションを取得し返却する
            return GetPropertyDescriptorCollection(value, attributes);
        }

        /// <summary>
        /// オブジェクトが <see cref="GetPropertiesSupported(ITypeDescriptorContext)"/> メソッドをサポートしているかどうかを示す値を返す
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <returns>
        /// オブジェクトのプロパティを見つけるために <see cref="GetPropertiesSupported(ITypeDescriptorContext)"/> メソッドを呼び出す必要がある場合は True、
        /// それ以外の場合は False
        /// </returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            // 常にGetProperties(Object)を呼び出す必要があるためtrueを返却
            return true;
        }

        /// <summary>
        /// 引数の属性（<paramref name="attributes"/>）の条件を使用してプロパティ情報（<paramref name="value"/>）からプロパティのコレクションを取得、
        /// そのプロパティコレクションをローカライズに対応した（<see cref="LocalizablePropertyDescriptor{TResources}"/>）型に変換して返却する
        /// </summary>
        /// <param name="value">
        /// プロパティ情報（<see cref="PropertyInfo"/>）の取得対象となる配列のオブジェクト
        /// </param>
        /// <param name="attributes">
        /// フィルターとして使用される、<see cref="Attribute"/> 型の配列
        /// </param>
        /// <exception cref="NotSupportedException">
        /// 引数の<paramref name="value"/> がプロセス間のリモートオブジェクトの場合に発生
        /// </exception>
        /// <returns>
        /// 指定されたデータ型に対して公開されているプロパティを格納している、ローカライズに対応した <see cref="PropertyDescriptor"/> のコレクション
        /// プロパティが格納されていない場合はNULL
        /// </returns>
        private static PropertyDescriptorCollection GetPropertyDescriptorCollection(
            object value, Attribute[] attributes)
        {
            // 基底クラスのメソッドからPropertyDescriptorコレクションを取得
            PropertyDescriptorCollection properies = TypeDescriptor.GetProperties(value, attributes, true);
            if (properies == null)
            {
                // 取得できない場合はNULLを返却
                return null;
            }

            // ローカライズ可能なPropertyDescriptorに変換する
            PropertyDescriptorCollection returnCollection = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor property in properies)
            {
                returnCollection.Add(new LocalizablePropertyDescriptor<TResouces>(property));
            }

            // リフレクションが定義順に取得することを利用し、定義順のソート条件を生成する
            List<string> sortList = new List<string>();
            foreach (PropertyInfo propertyInfo in value.GetType().GetProperties())
            {
                sortList.Add(propertyInfo.Name);
            }

            // 定義順にソートして返却
            return returnCollection.Sort(sortList.ToArray());
        }

        #endregion
    }
}
