namespace MisaCommon.Utility.ExtendMethod
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    /// 【使用禁止】未整理のUtilityクラス
    /// 作成したが使わないStaticメソッドの寄せ集め
    /// そのうちどこかに統合するが、それまでは参考ソースの用途
    /// </summary>
    public static class Z0UnfinishedUtility
    {
        #region 汎用のDeepコピー処理

        /// <summary>
        /// 引数（<paramref name="original"/>）のシリアライズ可能なオブジェクトの複製を生成する
        /// </summary>
        /// <typeparam name="T">コピー対象のシリアライズ可能なオブジェクトの型</typeparam>
        /// <param name="original">コピー対象のシリアライズ可能なオブジェクト</param>
        /// <exception cref="ArgumentNullException">
        /// 引数（<paramref name="original"/>）がNULLの場合に発生
        /// </exception>
        /// <exception cref="SerializationException">
        /// 引数（<paramref name="original"/>）がシリアル化可能でない等の理由で、
        /// シリアル化、逆シリアル化処理においてエラーが発生した場合に発生
        /// </exception>
        /// <exception cref="SecurityException">
        /// 呼び出し元に、必要なアクセス許可がない場合に発生
        /// </exception>
        /// <returns>
        /// 引数（<paramref name="original"/>）コピーである新しいオブジェクト
        /// </returns>
        public static T DeepCopySerializableClass<T>(T original)
        {
            // NULLチェック
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            // コピー元のオブジェクトをメモリに書き込み、それを別のオブジェクトに読み込むことで複製する
            object copyData = null;

            // ワーニングがでるためコメントアウト、使っておらず、参考用のコードだから問題ない
            /*
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter
                = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream stream;
            using (stream = new MemoryStream())
            {
                // コピー元のオブジェクトをシリアル化していったんメモリに書き込む
                formatter.Serialize(stream, original);

                // メモリに書き込んだデータを読み込み、逆シリアル化する
                stream.Seek(0, SeekOrigin.Begin);
                copyData = formatter.Deserialize(stream);
            }
            */

            // コピーしたオブジェクトを元の型にキャストして返却
            return (T)copyData;
        }

        #endregion

        #region ファイルをbyte型の配列形式のデータとして取得

        /// <summary>
        /// ファイルを <see cref="byte"/> 型の配列形式のデータとして取得する
        /// </summary>
        /// <param name="path">取得対象のファイルパス（相対パス）</param>
        /// <exception cref="ArgumentException">
        /// 引数の <paramref name="path"/> が下記の場合に発生
        /// ・長さ 0 の文字列
        /// ・空白のみで構成される
        /// ・<see cref="Path.InvalidPathChars"/> で定義される 1 つ以上の正しくない文字を含む
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="path"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// 引数の <paramref name="path"/> がシステム定義の最大長を超えている場合に発生
        /// たとえば、Windowsでは、パスは 248文字未満、ファイル名は 260 文字未満である必要がある
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        /// 引数の <paramref name="path"/> が存在しないディレクトリを示している場合に発生
        /// </exception>
        /// <exception cref="IOException">
        /// ファイルを開くときに、I/O エラーが発生した場合に発生
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// 引数の <paramref name="path"/> がファイルを指定しないない（ディレクトリを指定）場合、
        /// 又は、呼び出し元に必要なアクセス許可がない場合に発生
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// 引数の <paramref name="path"/> で指定されたファイルが存在しない場合に発生
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// 引数の <paramref name="path"/> の形式が正しくない場合に発生
        /// </exception>
        /// <exception cref="SecurityException">
        /// 呼び出し元に必要なアクセス許可がない場合に発生（セキュリティエラー）
        /// </exception>
        /// <returns>
        /// 対象のファイルの <see cref="byte"/> 型配列形式のデータ
        /// </returns>
        public static byte[] GetFileData(string path)
        {
            // ファイル内容を返却
            return File.ReadAllBytes(path);
        }

        #endregion
    }
}
