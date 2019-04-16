namespace MisaCommon.CustomType.Converter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// オブジェクトのプロパティ情報オブジェクトをローカライズに対応したものに変換する
    /// <see cref="TypeConverter"/> の派生クラス
    /// ※注意：プロパティのみを対象としており、イベント定義等のプロパティ以外のものについては使用できない
    /// </summary>
    /// <remarks>
    /// <para>
    /// ローカライズ対応したいクラスに、このクラスを属性指定することで、
    /// そのクラスのプロパティ情報をローカライズを可能とする
    /// </para>
    /// <para>
    /// ローカライズさせる文言はリソースファイルにて設定を行う、
    /// 各属性において設定するリソースのキーは下記の設定とする
    /// ・Category属性：任意の名称
    /// ・DisplayName属性：任意の名称　OR　[プロパティ名]＋_DisplayName
    /// 　（例：Nameの場合、Name_DisplayName）※
    /// ・Description属性：任意の名称　OR　[プロパティ名]＋_Description
    /// 　（例：Nameの場合、Name_Description）※
    /// ※[プロパティ名]＋_DisplayName、_Descriptionの設定は、
    /// 　リソース名の指定を省略した場合に自動的に使用する設定である
    /// </para>
    /// <para>
    /// ソートについてはプロパティの定義順に行われる。
    /// そのためこのコンバータクラスを使用するとアルファベット順にはならない
    /// </para>
    /// </remarks>
    /// <example>
    /// このクラスの使用例（下記のリソースクラスが存在している前提での使用例）
    /// 【リソースクラス】
    /// ・クラス名：LocalizeResources
    /// ・設定値
    /// 　キー：CategoryA                       値：カテゴリＡ
    /// 　キー：CategoryB                       値：カテゴリＢ
    /// 　キー：DisplayNameA                    値：プロパティ名Ａ
    /// 　キー：DescriptionA                    値：プロパティの説明Ａ
    /// 　キー：SampleProperty1_DisplayName     値：サンプル１プロパティ
    /// 　キー：SampleProperty1_Description     値：サンプル１の説明
    /// 　キー：SampleProperty2_DisplayName     値：サンプル２プロパティ
    /// 　キー：SampleProperty2_Description     値：サンプル２の説明
    /// 　キー：SampleProperty3_DisplayName     値：サンプル３プロパティ
    /// 　キー：SampleProperty3_Description     値：サンプル３の説明
    /// 　キー：SampleProperty4_DisplayName     値：サンプル４プロパティ
    /// 　キー：SampleProperty4_Description     値：サンプル４の説明
    ///
    /// 【コード】
    /// <code>
    /// <![CDATA[
    /// [TypeConverter(typeof(LocalizableConverter<LocalizeResources>))]
    /// public class SampleClass
    /// {
    ///     // パターン１：リソースのキーを使用する場合
    ///     [Category("CategoryA")]
    ///     [DisplayName("DisplayNameA")]
    ///     [Description("DescriptionA")]
    ///     public string SampleProperty1 { get; set; }
    ///
    ///     // パターン２：リソースのキーを使用する場合、キーに該当するデータが存在しない場合
    ///     [Category("カテゴリ_該当なし")]
    ///     [DisplayName("表示名_該当なし")]
    ///     [Description("説明_該当なし")]
    ///     public string SampleProperty2 { get; set; }
    ///
    ///     // パターン３：キーを省略した場合
    ///     [Category()]
    ///     [Description()]
    ///     [DisplayName()]
    ///     public string SampleProperty3 { get; set; }
    ///
    ///     // パターン４：属性指定を省略した場合
    ///     public string SampleProperty4 { get; set; }
    ///
    ///     // パターン５：属性指定を省略した場合で、該当キーも存在しない場合
    ///     public string SampleProperty5 { get; set; }
    ///
    ///     // パターン６：ソート可能カテゴリを使用した場合
    ///     [SortableCategory("CategoryB", 1)]
    ///     public string SampleProperty6 { get; set; }
    ///
    ///     …
    /// ]]>
    /// </code>
    ///
    /// 【出力】
    /// パターン１：リソースのキーを使用する場合
    /// 　・カテゴリ  ：カテゴリＡ
    /// 　・表示名    ：プロパティ名Ａ
    /// 　・説明      ：プロパティの説明Ａ
    ///
    /// パターン２：リソースのキーを使用する場合、キーに該当するデータが存在しない場合
    /// 　・カテゴリ  ：カテゴリ_該当なし    （←該当するリソースが存在しないためキーをそのまま出力）
    /// 　・表示名    ：表示名_該当なし      （←該当するリソースが存在しないためキーをそのまま出力）
    /// 　・説明      ：説明_該当なし        （←該当するリソースが存在しないためキーをそのまま出力）
    ///
    /// パターン３：キーを省略した場合
    /// 　・カテゴリ  ：その他　             （←キーが空のためデフォルト値を出力）
    /// 　・表示名    ：サンプル３プロパティ （←キーが空のためプロパティ名＋_DisplayNameのキーで取得）
    /// 　・説明      ：サンプル３の説明     （←キーが空のためプロパティ名＋_Descriptionのキーで取得）
    ///
    /// パターン４：属性指定を省略した場合
    /// 　・カテゴリ  ：その他　             （←省略されたためデフォルト値を出力）
    /// 　・表示名    ：サンプル４プロパティ （←省略されたためプロパティ名＋_DisplayNameのキーで取得）
    /// 　・説明      ：サンプル４の説明     （←省略されたためプロパティ名＋_Descriptionのキーで取得）
    ///
    /// パターン５：属性指定を省略した場合で、該当キーも存在しない場合
    /// 　・カテゴリ  ：その他　             （←省略されたためデフォルト値を出力）
    /// 　・表示名    ：SampleProperty5      （←リソースが取得できないためプロパティ名をそのまま出力）
    /// 　・説明      ：[空欄]               （←リソースが取得できないため空欄となる）
    ///
    /// パターン６：ソート可能カテゴリを使用した場合
    /// 　・カテゴリ  ：カテゴリＢ           （←ソートに１が設定されているため一番上になる）
    /// 　・表示名    ：SampleProperty6
    /// 　・説明      ：[空欄]
    /// </example>
    /// <typeparam name="TResouces">
    /// プロパティのカテゴリ、説明、表示名に使用するリソースクラスを指定
    /// </typeparam>
    public class LocalizableConverter<TResouces> : TypeConverter
    {
        #region メソッド

        /// <summary>
        /// プロパティ情報（<paramref name="value"/>）からプロパティのコレクションを取得し、
        /// ローカライズ対応のための（<see cref="LocalizablePropertyDescriptor{TResources}"/>）型に
        /// 変換して返却する
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
        /// 指定されたデータ型に対して公開されているプロパティを格納している、
        /// ローカライズに対応した <see cref="PropertyDescriptor"/> のコレクション
        /// プロパティが格納されていない場合はNULL
        /// </returns>
        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context, object value, Attribute[] attributes)
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
            foreach (PropertyInfo propertyInfo in value?.GetType().GetProperties() ?? new PropertyInfo[0])
            {
                sortList.Add(propertyInfo.Name);
            }

            // ローカライズ可能なPropertyDescriptorをプロパティの定義順にソートして返却する
            return returnCollection.Sort(sortList.ToArray());
        }

        /// <summary>
        /// オブジェクトが <see cref="GetProperties(ITypeDescriptorContext, object, Attribute[])"/>
        /// メソッドをサポートしているかどうかを示す値を返す
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <returns>
        /// オブジェクトのプロパティを見つけるために <see cref="GetProperties(ITypeDescriptorContext, object, Attribute[])"/>
        /// メソッドを呼び出す必要がある場合は True、それ以外の場合は False
        /// </returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            // 常にGetProperties(ITypeDescriptorContext, object, Attribute[])を
            // 呼び出す必要があるためtrueを返却
            return true;
        }

        #endregion
    }
}
