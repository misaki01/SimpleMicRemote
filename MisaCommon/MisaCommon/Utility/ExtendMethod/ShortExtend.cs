namespace MisaCommon.Utility.ExtendMethod
{
    using System;

    /// <summary>
    /// <see cref="short"/> に拡張メソッドを追加するクラス
    /// </summary>
    public static class ShortExtend
    {
        #region GetByte

        /// <summary>
        /// この値をbyte配列（2byte）で取得する
        /// </summary>
        /// <param name="input">byteを取得する数値</param>
        /// <returns>取得したbyte配列（2byte）</returns>
        public static byte[] GetByte(this short input)
        {
            // 引数のbyteを取得
            byte[] output = BitConverter.GetBytes(input);

            // ビックエイディアンの場合、配列をひっくり返す
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(output);
            }

            // 生成したbyteを返す
            return output;
        }

        #endregion
    }
}
