namespace MisaCommon.Utility.Win32Api.NativeMethod
{
    /// <summary>
    /// Win32のエラーコード
    /// </summary>
    internal enum ErrorCode : int
    {
        /// <summary>
        /// 正常終了
        /// 「この操作を正しく終了しました。」のエラーコード
        /// </summary>
        NO_ERROR = 0x00000000,

        /// <summary>
        /// 異常終了
        /// 「タイムアウト期間が経過したため、この操作は終了しました。」のエラーコード
        /// </summary>
        ERROR_TIMEOUT = 0x000005B4,
    }
}
