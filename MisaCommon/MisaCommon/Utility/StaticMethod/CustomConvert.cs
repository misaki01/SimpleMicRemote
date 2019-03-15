namespace MisaCommon.Utility.StaticMethod
{
    using System;

    /// <summary>
    /// 共通的な変換機能を提供するクラス
    /// </summary>
    public static class CustomConvert
    {
        #region 文字⇒数値の変換処理

        /// <summary>
        /// 引数（<paramref name="value"/>）で与えられた16進形式の文字列を、
        /// それと等価な<see cref="int"/> 型の整数に変換する
        /// </summary>
        /// <param name="value"><see cref="int"/> 型の整数に変換する16進形式の文字列</param>
        /// <param name="result">変換した<see cref="int"/> 型の整数、変換に失敗した場合は 0を返却</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>変換に成功した場合：True、失敗した場合：False</returns>
        public static bool TryHexStringToInt(string value, out int result)
        {
            try
            {
                // 引数の16進数の文字列の補正
                string tmpValue = CorrectHexString(value);

                // 空白の場合は変換失敗を返却
                if (string.IsNullOrWhiteSpace(tmpValue))
                {
                    result = 0;
                    return false;
                }

                // INTに変換する
                result = Convert.ToInt32(tmpValue, 16);
                return true;
            }
            catch (Exception ex)
                when (ex is FormatException || ex is InvalidCastException || ex is OverflowException)
            {
                result = 0;
                return false;
            }
        }

        /// <summary>
        /// 引数（<paramref name="value"/>）で与えられた16進形式の文字列を、
        /// それと等価な<see cref="short"/> 型の整数に変換する
        /// </summary>
        /// <param name="value"><see cref="short"/> 型の整数に変換する16進形式の文字列</param>
        /// <param name="result">変換した<see cref="short"/> 型の整数、変換に失敗した場合は 0を返却</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>変換に成功した場合：True、失敗した場合：False</returns>
        public static bool TryHexStringToShort(string value, out short result)
        {
            try
            {
                // 引数の16進数の文字列の補正
                string tmpValue = CorrectHexString(value);

                // 空白の場合は変換失敗を返却
                if (string.IsNullOrWhiteSpace(tmpValue))
                {
                    result = 0;
                    return false;
                }

                // SHORTに変換する
                result = Convert.ToInt16(tmpValue, 16);
                return true;
            }
            catch (Exception ex)
                when (ex is FormatException || ex is InvalidCastException || ex is OverflowException)
            {
                result = 0;
                return false;
            }
        }

        #endregion

        #region プライベートメソッド

        #region 16進数の文字列の補正

        /// <summary>
        /// 引数（<paramref name="value"/>）で与えられた16進形式の文字列に対して下記の補正を行う
        /// ・NULLチェック：NULLの場合は例外をスロー
        /// ・前後の空白除去
        /// ・文字列の先頭が「0x」、「0X」の場合その文字を除去
        /// </summary>
        /// <param name="value"><see cref="short"/> 型の整数に変換する16進形式の文字列</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>補正した16進数形式の文字列</returns>
        private static string CorrectHexString(string value)
        {
            // 引数のチェック
            if (value == null)
            {
                // NULLの場合は例外をスロー
                throw new ArgumentNullException(nameof(value));
            }

            // 前後の空白を除去
            string correctValue = value.Trim();

            // 引数の16進数の文字が「0x」で始まっている場合はその文字を削除する
            bool is0X = correctValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase);
            correctValue = is0X ? correctValue.Remove(0, 2) : correctValue;

            // 補正した文字列を返却する
            return correctValue;
        }

        #endregion

        #endregion
    }
}
