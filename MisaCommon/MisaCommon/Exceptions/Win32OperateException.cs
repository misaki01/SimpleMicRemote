namespace MisaCommon.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// P/Invoke（プラットフォーム呼び出し）で呼び出したメソッド内で、
    /// 例外が発生又は処理が失敗した場合に関する例外を扱う <see cref="Exception"/> の派生クラス
    /// </summary>
    [Serializable]
    public class Win32OperateException : Exception, ISerializable
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 初期化を行う
        /// </summary>
        public Win32OperateException()
            : base()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数を用いて初期化を行う
        /// </summary>
        /// <param name="message">例外メッセージ</param>
        public Win32OperateException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数を用いて初期化を行う
        /// </summary>
        /// <param name="message">例外メッセージ</param>
        /// <param name="innerException">内包する例外</param>
        public Win32OperateException(string message, Exception innerException)
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
        protected Win32OperateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// <see cref="SerializationInfo"/> に、オブジェクトをシリアル化するために必要なデータを設定する
        /// </summary>
        /// <param name="info">データの読み込み先となる <see cref="SerializationInfo"/></param>
        /// <param name="context">このシリアル化のシリアル化先（<see cref="StreamingContext"/>）</param>
        /// <exception cref="System.Security.SecurityException">
        /// 呼び出し元に、必要なアクセス許可がない場合に発生
        /// </exception>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
