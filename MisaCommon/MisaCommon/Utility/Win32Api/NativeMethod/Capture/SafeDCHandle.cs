namespace MisaCommon.Utility.Win32Api.NativeMethod.Capture
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;

    using MisaCommon.Exceptions;

    /// <summary>
    /// <see cref="NativeMethods.CreateCompatibleDC(IntPtr)"/> メソッドで生成する
    /// デバイスコンテキストのハンドルに対する <see cref="SafeHandle"/> の派生クラス
    /// </summary>
    internal class SafeDCHandle : SafeHandle
    {
        #region コンストラクタ

        /// <summary>
        /// プライベートコンストラクタ
        /// 無効なハンドルの値を 0 とし、ハンドルを確実に解放する設定で初期化を行う
        /// </summary>
        /// <exception cref="TypeLoadException">
        /// アンマネージコードへのアクセス許可がない場合に発生
        /// </exception>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        private SafeDCHandle()
            : base(IntPtr.Zero, true)
        {
            // base の第２引数について
            // 終了処理中にハンドルを確実にリリースする場合は true、
            // 信頼性の高いリリースを実行しない場合は false (お勧めしません)
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ハンドルが無効かどうかを示す値を取得する
        /// </summary>
        public override bool IsInvalid => handle == IntPtr.Zero;

        #endregion

        #region メソッド

        /// <summary>
        /// デバイスコンテキスト（DC）を解放する
        /// </summary>
        /// <remarks>
        /// 下記の例外が発生する状況はコードのバグであるため例外はスローする
        /// ・<see cref="PlatformInvokeException"/>
        /// ・<see cref="Win32OperateException"/>
        /// </remarks>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：DeleteDC」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：DeleteDC」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>正常に解放できた場合：True、深刻なエラーが発生した場合：False</returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、デバイスコンテキスト（DC）の破棄処理を呼び出す
            Win32ApiResult function()
            {
                bool win32Result = NativeMethods.DeleteDC(handle);

                return new Win32ApiResult(win32Result);
            }

            // 実行
            string dllName = "gdi32.dll";
            string methodName = nameof(NativeMethods.DeleteDC);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result)
            {
                // 解放が失敗した場合、False を返す
                return false;
            }

            // 解放が成功した場合、True を返す
            return true;
        }

        #endregion
    }
}
