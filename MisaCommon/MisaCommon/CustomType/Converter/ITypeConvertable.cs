namespace MisaCommon.CustomType.Converter
{
    using System;

    /// <summary>
    /// <see cref="LocalizableTypeConverter{T, TResouces}"/>を適用可能とするインターフェース
    /// </summary>
    /// <typeparam name="T">Converterを適用するクラスを指定</typeparam>
    public interface ITypeConvertable<T>
    {
        /// <summary>
        /// 文字列を <typeparamref name="T"/> クラスのインスタンスに変換する
        /// </summary>
        /// <param name="value">
        /// <typeparamref name="T"/> クラスのインスタンスに変換する文字列
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>文字列から生成した <typeparamref name="T"/> クラスのインスタンス</returns>
        T ConvertFromString(string value);

        /// <summary>
        /// <typeparamref name="T"/> クラスのインスタンスを文字列に変換する
        /// </summary>
        /// <param name="value">
        /// 文字列に変換する <typeparamref name="T"/> クラスのインスタンス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>変換した文字列</returns>
        string ConvertToString(T value);
    }
}
