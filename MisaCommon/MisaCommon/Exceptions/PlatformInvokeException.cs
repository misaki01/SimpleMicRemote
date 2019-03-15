namespace MisaCommon.Exceptions
{
    using System;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    /// <summary>
    /// P/Invoke（プラットフォーム呼び出し）に関する例外を扱う <see cref="Exception"/> の派生クラス
    /// </summary>
    /// <remarks>
    /// <see cref="DllImportAttribute"/> 属性に指定されたDLLが見つからない、
    /// 指定されたメソッド名に該当するメソッドが存在しない等の場合に発生する例外をラップする
    /// </remarks>
    [Serializable]
    public class PlatformInvokeException : Exception
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 初期化を行う
        /// </summary>
        public PlatformInvokeException()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数を用いて初期化を行う
        /// </summary>
        /// <param name="message">例外メッセージ</param>
        public PlatformInvokeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数を用いて初期化を行う
        /// </summary>
        /// <param name="message">例外メッセージ</param>
        /// <param name="innerException">内包する例外</param>
        public PlatformInvokeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数のシリアル化したデータを用いて初期化を行う
        /// </summary>
        /// <param name="info">
        /// スローされている例外に関するシリアル化済みオブジェクトのデータを保持している
        /// <see cref="SerializationInfo"/> オブジェクト
        /// </param>
        /// <param name="context">
        /// 転送元または転送先についてのコンテキスト情報を含む <see cref="StreamingContext"/> オブジェクト
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="info"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="SerializationException">
        /// クラス名がNULL又は、<see cref="Exception.HResult"/> が 0 の場合に発生
        /// </exception>
        protected PlatformInvokeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
