namespace MisaCommon.Utility.Win32Api.NativeMethod.Capture
{
    using System;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;

    using MisaCommon.Exceptions;

    /// <summary>
    /// <see cref="NativeMethods.CopyIcon(IntPtr)"/> メソッドで生成するハンドルに対する
    /// <see cref="SafeHandle"/> の派生クラス
    /// </summary>
    internal class SafeCopyIconHandle : SafeHandle
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
        private SafeCopyIconHandle()
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

        /// <summary>
        /// ハンドルを取得する
        /// </summary>
        public IntPtr Handle => handle;

        #endregion

        #region メソッド

        /// <summary>
        /// アイコンのハンドルを解放する
        /// </summary>
        /// <remarks>
        /// 下記の例外が発生する状況はコードのバグであるため例外はスローする
        /// ・<see cref="PlatformInvokeException"/>
        /// ・<see cref="Win32OperateException"/>
        /// </remarks>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：DestroyIcon」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：DestroyIcon」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>正常に解放できた場合：True、深刻なエラーが発生した場合：False</returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、アイコンの破棄処理を呼び出す
            Win32ApiResult function()
            {
                bool win32Result = NativeMethods.DestroyIcon(handle);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(NativeMethods.DestroyIcon);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
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
