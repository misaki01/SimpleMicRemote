namespace MisaCommon.Utility.Win32Api
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.Utility.Win32Api.NativeMethod;
    using MisaCommon.Utility.Win32Api.NativeMethod.IconCursor;

    using Icon = System.Drawing.Icon;
    using IconInfo = NativeMethod.IconCursor.Icon;
    using Win32Api = NativeMethod.IconCursor.NativeMethods;

    /// <summary>
    /// Win32APIの機能を使用してアイコン、カーソルに対する操作を行うクラス
    /// </summary>
    public static class IconCursorOperate
    {
        #region メソッド

        /// <summary>
        /// 現在のカーソルをキャプチャする（画像、座標情報を取得）
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetCursorInfo」
        /// ・「DLL：user32.dll、メソッド：CopyIcon」
        /// ・「DLL：user32.dll、メソッド：GetIconInfo」
        /// ・「DLL：user32.dll、メソッド：DeleteObject」
        /// ・「DLL：user32.dll、メソッド：DestroyIcon」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetCursorInfo」
        /// ・「DLL：user32.dll、メソッド：CopyIcon」
        /// ・「DLL：user32.dll、メソッド：GetIconInfo」
        /// ・「DLL：user32.dll、メソッド：DeleteObject」
        /// ・「DLL：user32.dll、メソッド：DestroyIcon」
        /// </exception>
        /// <returns>現在のカーソルの画像、座標情報</returns>
        public static CursorInfo CaptureCurrentCursor()
        {
            // カーソル情報を取得
            Cursor.CURSORINFO cursorInfo = GetCursorInfo();
            if (cursorInfo.Flag != (int)Cursor.State.CURSOR_SHOWING)
            {
                // カーソルを表示していない場合、NULL を返却する
                return null;
            }

            // アイコン用のハンドルを宣言
            IntPtr iconHandle = IntPtr.Zero;
            IconInfo.ICONINFO iconInfo = default(IconInfo.ICONINFO);
            try
            {
                // システムからアイコンのハンドルをコピーしておく
                iconHandle = CopyIcon(cursorInfo.CursorHandle);

                // アイコン情報を取得
                iconInfo = GetIconInfo(iconHandle);

                // 取得したアイコン情報がカーソルでない場合、NULL を返却する
                if (iconInfo.IsIcon)
                {
                    return null;
                }

                // カーソルの座標を取得
                // （カーソルのホットスポットの分値を補正する）
                int x = cursorInfo.ScreenPosition.X - iconInfo.HotspotX;
                int y = cursorInfo.ScreenPosition.Y - iconInfo.HotspotY;
                Point point = new Point(x, y);

                // カーソルのイメージを取得
                Image cursorImage = Icon.FromHandle(iconHandle).ToBitmap();

                // 取得成功を返す
                return new CursorInfo(cursorImage, point);
            }
            finally
            {
                try
                {
                    // アイコン情報のマスクと画像情報を破棄する
                    if (!iconInfo.Equals(default(IconInfo.ICONINFO)))
                    {
                        DeleteObject(iconInfo.MaskBitmapHandle);
                        DeleteObject(iconInfo.ColorBitmapHandle);
                    }
                }
                finally
                {
                    // アイコンを破棄する
                    if (iconHandle != IntPtr.Zero)
                    {
                        DestroyIcon(iconHandle);
                    }
                }
            }
        }

        #endregion

        #region プライベートメソッド

        #region Win32Apiを直接呼び出しているメソッド

        /// <summary>
        /// グローバルのカーソルの情報を取得する
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：GetCursorInfo」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：GetCursorInfo」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>取得したカーソル情報</returns>
        private static Cursor.CURSORINFO GetCursorInfo()
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの存在チェック処理を呼び出す
            Win32ApiResult function()
            {
                Cursor.CURSORINFO info = new Cursor.CURSORINFO
                {
                    StructureSize = Marshal.SizeOf(typeof(Cursor.CURSORINFO))
                };
                bool win32Result = Win32Api.GetCursorInfo(ref info);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(info, win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.GetCursorInfo);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }

            // 取得したカーソル情報を返却
            return (Cursor.CURSORINFO)result.ReturnValue;
        }

        /// <summary>
        /// アイコンの情報を取得する
        /// </summary>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：GetIconInfo」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：GetIconInfo」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>取得したアイコン情報</returns>
        private static IconInfo.ICONINFO GetIconInfo(IntPtr iconCursorHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの存在チェック処理を呼び出す
            Win32ApiResult function()
            {
                bool win32Result
                    = Win32Api.GetIconInfo(iconCursorHandle, out IconInfo.ICONINFO info);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(info, win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.GetIconInfo);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }

            // 取得したアイコン情報を返却
            return (IconInfo.ICONINFO)result.ReturnValue;
        }

        /// <summary>
        /// 引数で指定された他のモジュールのアイコンへのハンドル（<paramref name="iconCursorHandle"/>）を
        /// 現在のモジュールのアイコンへのハンドルに複製する
        /// </summary>
        /// <remarks>
        /// <see cref="CopyIcon(IntPtr)"/>の機能は、別のモジュールが所有しているアイコンを、
        /// アプリケーション または、DLLへの独自のハンドルで取得することを可能にします。
        /// この結果、他のモジュールが解放されてもアプリケーションアイコンはアイコンとして使用できます。
        /// </remarks>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：CopyIcon」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：CopyIcon」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>ウィンドウが存在する場合：True、存在しない場合：False</returns>
        private static IntPtr CopyIcon(IntPtr iconCursorHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの存在チェック処理を呼び出す
            Win32ApiResult function()
            {
                IntPtr win32ReturnValue = Win32Api.CopyIcon(iconCursorHandle);
                int win32ErrorCode = Marshal.GetLastWin32Error();
                bool win32Result = Win32Api.CopyIconParameter.IsSuccess(win32ReturnValue);

                return new Win32ApiResult(win32ReturnValue, win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.CopyIcon);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }

            // 複製したアイコンへのハンドルを返却
            return (IntPtr)result.ReturnValue;
        }

        /// <summary>
        /// アイコンを破棄する
        /// （<see cref="CopyIcon(IntPtr)"/> で複製したアイコンは必ずこのメソッドで破棄する必要がある）
        /// </summary>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：DestroyIcon」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：DestroyIcon」の処理に失敗した場合に発生
        /// </exception>
        private static void DestroyIcon(IntPtr iconCursorHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの存在チェック処理を呼び出す
            Win32ApiResult function()
            {
                bool win32Result = Win32Api.DestroyIcon(iconCursorHandle);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.DestroyIcon);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        /// <summary>
        /// オブジェクトに関連付けられているすべてのシステムリソースを解放する
        /// （論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットを削除する
        /// 　オブジェクトが削除されると、指定されたハンドルは無効になる）
        /// </summary>
        /// <param name="objectHandle">
        /// 論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットへのハンドル
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：DeleteObject」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：DeleteObject」の処理に失敗した場合に発生
        /// </exception>
        private static void DeleteObject(IntPtr objectHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの存在チェック処理を呼び出す
            Win32ApiResult function()
            {
                bool win32Result = Win32Api.DeleteObject(objectHandle);

                return new Win32ApiResult(win32Result);
            }

            // 実行
            string dllName = "gdi32.dll";
            string methodName = nameof(Win32Api.DeleteObject);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName);
            }
        }

        #endregion

        #endregion
    }
}
