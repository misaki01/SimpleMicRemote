namespace MisaCommon.CustomType.Converter
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 型変換処理の共通処理を扱うクラス
    /// </summary>
    public static class ConverterCommon
    {
        #region 型変換が可能かの判定

        /// <summary>
        /// 指定した型（<paramref name="sourceType"/>）のオブジェクトを
        /// コンバーターが対象とする型に変換できるかどうかを示す値を返却する
        /// </summary>
        /// <param name="sourceType">
        /// 変換元のデータ型
        /// </param>
        /// <param name="supportTypeList">
        /// コンバーターで変換をサポートしている型の定義
        /// </param>
        /// <returns>
        /// コンバーターで変換処理が実装している型の場合は True、それ以外の場合は False
        /// </returns>
        public static bool CanConvertFrom(
            Type sourceType, IReadOnlyDictionary<Type, ConverterDelegateInfo> supportTypeList)
        {
            // サポート対象の型かチェックする
            if (sourceType == null
                || supportTypeList == null
                || !supportTypeList.ContainsKey(sourceType))
            {
                // サポート対象外の型の場合はFalseを返却
                return false;
            }

            // 変換処理の実装が存在するかチェック
            if (supportTypeList[sourceType].From == null)
            {
                // 変換処理が存在しない場合はFalseを返却
                return false;
            }

            // チェックが全てOKとなったためTrueを返却
            return true;
        }

        /// <summary>
        /// コンバーターが対象とする型のオブジェクトを
        /// 指定した型（<paramref name="destinationType"/>）型に変換できるかどうかを示す値を返却する
        /// </summary>
        /// <param name="destinationType">
        /// 変換後のデータ型
        /// </param>
        /// <param name="supportTypeList">
        /// コンバーターで変換をサポートしている型の定義
        /// </param>
        /// <returns>
        /// コンバーターで変換処理が実装している型の場合は True、それ以外の場合は False
        /// </returns>
        public static bool CanConvertTo(
            Type destinationType, IReadOnlyDictionary<Type, ConverterDelegateInfo> supportTypeList)
        {
            // サポート対象の型かチェックする
            if (destinationType == null
                || supportTypeList == null
                || !supportTypeList.ContainsKey(destinationType))
            {
                // サポート対象外の型の場合はFalseを返却
                return false;
            }

            // 変換処理の実装が存在するかチェック
            if (supportTypeList[destinationType].To == null)
            {
                // 変換処理が存在しない場合はFalseを返却
                return false;
            }

            // チェックが全てOKとなったためTrueを返却
            return true;
        }

        #endregion
    }
}
