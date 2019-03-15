namespace MisaCommon.Utility.Win32Api
{
    /// <summary>
    /// Win32Apiの実行結果に関する情報を扱うクラス
    /// </summary>
    public class Win32ApiResult
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        /// <param name="returnValue">実行したWin32Apiの戻り値（戻り値が存在しない場合はNULL）</param>
        /// <param name="result">正常終了したかどうかを表すフラグ 正常終了：True、異常終了：False</param>
        /// <param name="errorCode">エラーコード</param>
        public Win32ApiResult(object returnValue, bool result, int errorCode)
        {
            ReturnValue = returnValue;
            Result = result;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// コンストラクタ（戻り値なしパターン）
        /// 戻り値の値はNULLで初期化する
        /// </summary>
        /// <param name="result">正常終了したかどうかを表すフラグ 正常終了：True、異常終了：False</param>
        /// <param name="errorCode">エラーコード</param>
        public Win32ApiResult(bool result, int errorCode)
        {
            ReturnValue = null;
            Result = result;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// コンストラクタ（戻り値のみで実行結果のチェックを行わないパターン）
        /// 実行結果、エラーコードは正常終了の値で初期化する
        /// </summary>
        /// <param name="returnValue">実行したWin32Apiの戻り値（戻り値が存在しない場合はNULL）</param>
        public Win32ApiResult(object returnValue)
        {
            ReturnValue = returnValue;
            Result = true;
            ErrorCode = (int)NativeMethod.ErrorCode.NO_ERROR;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 実行したWin32Apiの戻り値を取得・設定する
        /// （戻り値が存在しない場合はNULL）
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// 正常終了したかどうかを表すフラグを取得・設定する
        /// 正常終了：True、異常終了：False
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// エラーコードを取得・設定する
        /// </summary>
        public int ErrorCode { get; set; }

        #endregion
    }
}
