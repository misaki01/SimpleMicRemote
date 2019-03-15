namespace MisaCommon.Utility.ExtendMethod
{
    /// <summary>
    /// <see cref="string"/> に拡張メソッドを追加するクラス
    /// </summary>
    public static class StringExtend
    {
        #region EqualsWithNull

        /// <summary>
        /// この文字列と、指定した文字列の値がNULLも加味して同一かどうかを判定する
        /// </summary>
        /// <param name="thisValue">拡張機能を追加する <see cref="string"/> オブジェクト</param>
        /// <param name="compareObject">この文字列を比較する文字列</param>
        /// <returns>この文字列と同じ場合：True、異なる場合：False</returns>
        public static bool EqualsWithNull(this string thisValue, string compareObject)
        {
            if (thisValue == null)
            {
                return (compareObject == null);
            }
            else if (compareObject == null)
            {
                return false;
            }

            return thisValue.Equals(compareObject);
        }

        #endregion
    }
}
