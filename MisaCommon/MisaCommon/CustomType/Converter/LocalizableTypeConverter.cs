namespace MisaCommon.CustomType.Converter
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// 共通のローカライズ可能な変換をサポートする <see cref="TypeConverter"/> の派生クラス
    /// <see cref="LocalizableConverter{TResources}"/> を経由して継承しており、ローカライズも対応している
    /// </summary>
    /// <typeparam name="T">
    /// Converterを適用する <see cref="ITypeConvertable{T}"/> インターフェースを実装したクラスを指定
    /// </typeparam>
    /// <typeparam name="TResouces">
    /// プロパティのカテゴリ、説明、表示名に使用するリソースクラスを指定
    /// </typeparam>
    public class LocalizableTypeConverter<T, TResouces> : LocalizableConverter<TResouces>
        where T : ITypeConvertable<T>, new()
    {
        /// <summary>
        /// このコンバータで変換をサポートしている型と変換処理デリゲートのリスト
        /// </summary>
        private IReadOnlyDictionary<Type, ConverterDelegateInfo> SupportTypeList =>
            new ReadOnlyDictionary<Type, ConverterDelegateInfo>(
                new Dictionary<Type, ConverterDelegateInfo>()
            {
                // 型：string
                { typeof(string),   new ConverterDelegateInfo(ConverterFromString,     ConverterToString) },
            });

        #region メソッド

        /// <summary>
        /// 指定した型（<paramref name="sourceType"/>）を
        /// <typeparamref name="T"/> 型に変換できるかどうかを示す値を返却する
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="sourceType">
        /// 変換元のデータ型
        /// </param>
        /// <returns>
        /// コンバーターで変換が可能な場合は True、それ以外の場合は False
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            // サポート対象の型かチェックする
            if (ConverterCommon.CanConvertFrom(sourceType, SupportTypeList))
            {
                // サポート対象の型の場合、変換処理を実装しているのでTrueを返却
                return true;
            }

            // 基底のメソッドを呼び出しデフォルトの処理で変換可能であるかをチェックしその結果を返却する
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// <typeparamref name="T"/> 型を、指定した型（<paramref name="destinationType"/>）に
        /// 変換できるかどうかを示す値を返却する
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="destinationType">
        /// 変換後のデータ型
        /// </param>
        /// <returns>
        /// コンバーターで変換が可能な場合は True、それ以外の場合は False
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            // サポート対象の型かチェックする
            if (ConverterCommon.CanConvertTo(destinationType, SupportTypeList))
            {
                // サポート対象の型の場合、変換処理を実装しているのでTrueを返却
                return true;
            }

            // 基底のメソッドを呼び出しデフォルトの処理で変換可能であるかをチェックしその結果を返却する
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 指定されたオブジェクト（<paramref name="value"/>）を、<typeparamref name="T"/> 型に変換する
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="culture">
        /// 現在のカルチャとして使用する <see cref="CultureInfo"/> オブジェクト
        /// </param>
        /// <param name="value">
        /// 変換対象のオブジェクト
        /// </param>
        /// <exception cref="NotSupportedException">
        /// 変換が実行できない場合に発生
        /// </exception>
        /// <returns>
        /// 変換後の値を表すオブジェクト
        /// </returns>
        public override object ConvertFrom(
            ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // 引数の型がサポート対象の型の場合、実装している変換処理を呼び出しその結果を返却
            if (value != null)
            {
                if (SupportTypeList.ContainsKey(value.GetType()))
                {
                    object convertValue = SupportTypeList[value.GetType()]
                        .From(context, culture, value);
                    if (convertValue != null)
                    {
                        // 返還後の値がNULLでない場合、値を返却する
                        // NULLの場合はこの後の基底のメソッドの変換処理にまかせる
                        return convertValue;
                    }
                }
            }

            // 基底のメソッドを呼び出しデフォルトの処理で変換を行い、その結果を返却する
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// <typeparamref name="T"/> 型のオブジェクト（<paramref name="value"/>）を、
        /// 指定した型（<paramref name="destinationType"/>）に変換する
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="culture">
        /// 現在のカルチャとして使用する <see cref="CultureInfo"/> オブジェクト
        /// </param>
        /// <param name="value">
        /// 変換対象のコンバーターが対象とする型のオブジェクト
        /// </param>
        /// <param name="destinationType">
        /// 変換先の型
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="destinationType"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// 変換が実行できない場合に発生
        /// </exception>
        /// <returns>
        /// 変換後の値を表すオブジェクト
        /// </returns>
        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // 引数の型がサポート対象の型の場合、実装している変換処理を呼び出しその結果を返却
            if (destinationType != null)
            {
                if (SupportTypeList.ContainsKey(destinationType))
                {
                    object convertValue = SupportTypeList[destinationType]
                        .To(context, culture, value, destinationType);
                    if (convertValue != null)
                    {
                        // 返還後の値がNULLでない場合、値を返却する
                        // NULLの場合はこの後の基底のメソッドの変換処理にまかせる
                        return convertValue;
                    }
                }
            }

            // 基底のメソッドを呼び出しデフォルトの処理で変換を行い、その結果を返却する
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// コンバーターを利用しているオブジェクトの値が変更された場合において、
        /// 新しいインスタンスを生成する必要があるかどうかを示す値を返却する
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <returns>
        /// 新しい値を生成する必要があるか場合は True、それ以外の場合は False
        /// </returns>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            // 常にCreateInstance(ITypeDescriptorContext, IDictionary)を
            // 呼び出す必要があるため true を返却
            return true;
        }

        /// <summary>
        /// コンバーターを利用している型（<typeparamref name="T"/>）のインスタンスを生成する
        /// </summary>
        /// <param name="context">
        /// 書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="propertyValues">
        /// 新しいプロパティ値のディクショナリ
        /// </param>
        /// <returns>
        /// 引数の <paramref name="propertyValues"/> の値を格納した
        /// <typeparamref name="T"/>型のインスタンス
        /// オブジェクトを作成できない場合はNULLを返却
        /// </returns>
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            // NULLチェック
            if (propertyValues == null)
            {
                return null;
            }

            // インスタンスを生成
            T instance = new T();

            // 生成したインスタンスのプロパティを取得する
            PropertyDescriptorCollection descriptorCollection = TypeDescriptor.GetProperties(instance);
            if (descriptorCollection == null)
            {
                // プロパティが取得できない場合はNULLを返却
                return null;
            }

            // 取得したプロパティでループする
            foreach (PropertyDescriptor descriptor in descriptorCollection)
            {
                // プロパティの名称を取得
                string name = descriptor.Name;

                // プロパティ名称に該当するデータが存在する場合、概要のプロパティにデータを設定する
                if (propertyValues.Contains(name))
                {
                    descriptor.SetValue(instance, propertyValues[name]);
                }
            }

            // 値を設定したインスタンスを返却
            return instance;
        }

        #endregion

        #region 型変換のメソッド

        /// <summary>
        /// <see cref="string"/> 型のオブジェクトを <typeparamref name="T"/> 型に変換する
        /// </summary>
        /// <remarks>
        /// このメソッドは <see cref="ConverterFrom"/> デリゲートの実装として使用する
        /// </remarks>
        /// <param name="context">
        /// 【未使用】書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="culture">
        /// 【未使用】現在のカルチャとして使用する <see cref="CultureInfo"/> オブジェクト
        /// </param>
        /// <param name="value">
        /// 変換対象のオブジェクト
        /// </param>
        /// <returns>
        /// 変換後の値を表すオブジェクト
        /// 変換できない場合はNULLを返却
        /// </returns>
        private object ConverterFromString(
            ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // 引数の型チェック
            if (!(value is string stringValue))
            {
                // 型が対象外の場合、NULLを返却
                return null;
            }

            // Tクラスのインスタンスに変換し返却
            return new T().ConvertFromString(stringValue);
        }

        /// <summary>
        /// <typeparamref name="T"/> 型のオブジェクトを <see cref="string"/> 型に変換する
        /// </summary>
        /// <remarks>
        /// このメソッドは <see cref="ConverterTo"/> デリゲートの実装として使用する
        /// </remarks>
        /// <param name="context">
        /// 【未使用】書式指定コンテキストを提供する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="culture">
        /// 【未使用】現在のカルチャとして使用する <see cref="CultureInfo"/> オブジェクト
        /// </param>
        /// <param name="value">
        /// 変換対象のコンバーターが対象とする型のオブジェクト
        /// </param>
        /// <param name="destinationType">
        /// 【未使用】変換先の型
        /// </param>
        /// <returns>
        /// 変換後の値を表すオブジェクト
        /// 変換できない場合はNULLを返却
        /// </returns>
        private object ConverterToString(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // 引数の型チェック
            if (!(value is T typeValue))
            {
                // 型が対象外の場合、NULLを返却
                return null;
            }

            // 文字列に変換し返却
            return new T().ConvertToString(typeValue);
        }

        #endregion
    }
}
