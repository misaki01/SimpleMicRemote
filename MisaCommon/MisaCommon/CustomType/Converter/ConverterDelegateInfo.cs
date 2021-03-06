﻿namespace MisaCommon.CustomType.Converter
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    // TypeConverterの派生クラスにて実装する型変換に対する処理を行うデリゲートの定義
    #region デリゲートの定義

    /// <summary>
    /// コンバーターが対象とする型に変換する機能を提供するデリゲートの定義
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
    /// <returns>変換後の値を表すオブジェクト</returns>
    public delegate object ConverterFrom(
        ITypeDescriptorContext context, CultureInfo culture, object value);

    /// <summary>
    /// 指定した型（<paramref name="destinationType"/>）に変換する機能を提供するデリゲートの定義
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
    /// <returns>変換後の値を表すオブジェクト</returns>
    public delegate object ConverterTo(
        ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType);

    #endregion

    /// <summary>
    /// <see cref="TypeConverter"/> の派生クラスにて実装する型変換に対する処理を扱うクラス
    /// </summary>
    public class ConverterDelegateInfo
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数の値で初期化を行う
        /// </summary>
        /// <param name="from">コンバーターが対象とする型に変換する機能</param>
        /// <param name="to">指定の型に変換する機能</param>
        public ConverterDelegateInfo(ConverterFrom from, ConverterTo to)
        {
            From = from;
            To = to;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// コンバーターが対象とする型に変換する機能を取得する
        /// </summary>
        public ConverterFrom From { get; private set; }

        /// <summary>
        /// 指定の型に変換する機能を取得する
        /// </summary>
        public ConverterTo To { get; private set; }

        #endregion
    }
}
