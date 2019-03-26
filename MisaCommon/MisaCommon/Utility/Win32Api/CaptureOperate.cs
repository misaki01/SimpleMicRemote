namespace MisaCommon.Utility.Win32Api
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.Utility.Win32Api.NativeMethod;
    using MisaCommon.Utility.Win32Api.NativeMethod.Capture;

    using Icon = System.Drawing.Icon;
    using IconInfo = NativeMethod.Capture.Icon;
    using ROPCode = NativeMethod.Capture.RasterOperation.CommonCode;
    using Win32Api = NativeMethod.Capture.NativeMethods;

    /// <summary>
    /// Win32APIの機能を使用してアイコン、カーソルに対する操作を行うクラス
    /// </summary>
    public static class CaptureOperate
    {
        #region メソッド

        #region カーソルのキャプチャ

        /// <summary>
        /// 現在のカーソルをキャプチャする（画像、座標情報を取得）
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetCursorInfo」
        /// ・「DLL：user32.dll、メソッド：CopyIcon」
        /// ・「DLL：user32.dll、メソッド：GetIconInfo」
        /// ・「DLL：gdi32.dll、メソッド：CreateCompatibleDC」
        /// ・「DLL：gdi32.dll、メソッド：SelectObject」
        /// ・「DLL：gdi32.dll、メソッド：BitBlt」
        /// ・「DLL：gdi32.dll、メソッド：DeleteObject」
        /// ・「DLL：gdi32.dll、メソッド：DeleteDC」
        /// ・「DLL：user32.dll、メソッド：DestroyIcon」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetCursorInfo」
        /// ・「DLL：user32.dll、メソッド：CopyIcon」
        /// ・「DLL：user32.dll、メソッド：GetIconInfo」
        /// ・「DLL：gdi32.dll、メソッド：CreateCompatibleDC」
        /// ・「DLL：gdi32.dll、メソッド：SelectObject」
        /// ・「DLL：gdi32.dll、メソッド：BitBlt」
        /// ・「DLL：gdi32.dll、メソッド：DeleteObject」
        /// ・「DLL：gdi32.dll、メソッド：DeleteDC」
        /// ・「DLL：user32.dll、メソッド：DestroyIcon」
        /// </exception>
        /// <returns>
        /// 現在のカーソルの画像、座標情報
        /// （カーソルが取得できない場合はNULLを返却する）
        /// </returns>
        public static CursorInfo CaptureCurrentCursor()
        {
            return CaptureCurrentCursor(null, new Point(0, 0));
        }

        /// <summary>
        /// 現在のカーソルをキャプチャする（画像、座標情報を取得）
        /// </summary>
        /// <param name="backgroundImage">
        /// Iビームカーソル等の背景に応じで色が変化するカーソルを、
        /// 正確に描画するために使用するカーソルの下にある背景画像
        /// （NULLを指定した場合、白一色の背景としてカーソルをキャプチャする）
        /// </param>
        /// <param name="backgroundImageScreenPoint">
        /// 背景画像の画面上の座標（画像の左上の絶対座標）
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetCursorInfo」
        /// ・「DLL：user32.dll、メソッド：CopyIcon」
        /// ・「DLL：user32.dll、メソッド：GetIconInfo」
        /// ・「DLL：gdi32.dll、メソッド：CreateCompatibleDC」
        /// ・「DLL：gdi32.dll、メソッド：SelectObject」
        /// ・「DLL：gdi32.dll、メソッド：BitBlt」
        /// ・「DLL：gdi32.dll、メソッド：DeleteObject」
        /// ・「DLL：gdi32.dll、メソッド：DeleteDC」
        /// ・「DLL：user32.dll、メソッド：DestroyIcon」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetCursorInfo」
        /// ・「DLL：user32.dll、メソッド：CopyIcon」
        /// ・「DLL：user32.dll、メソッド：GetIconInfo」
        /// ・「DLL：gdi32.dll、メソッド：CreateCompatibleDC」
        /// ・「DLL：gdi32.dll、メソッド：SelectObject」
        /// ・「DLL：gdi32.dll、メソッド：BitBlt」
        /// ・「DLL：gdi32.dll、メソッド：DeleteObject」
        /// ・「DLL：gdi32.dll、メソッド：DeleteDC」
        /// ・「DLL：user32.dll、メソッド：DestroyIcon」
        /// </exception>
        /// <returns>
        /// 現在のカーソルの画像、座標情報
        /// （カーソルが取得できない場合はNULLを返却する）
        /// </returns>
        public static CursorInfo CaptureCurrentCursor(Bitmap backgroundImage, Point backgroundImageScreenPoint)
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
            Image cursorImage;
            Point screenPoint;
            Point imagePoint;
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
                // 絶対座標（カーソルのホットスポットの分値を補正する）
                screenPoint = new Point(
                    x: cursorInfo.ScreenPosition.X - iconInfo.HotspotX,
                    y: cursorInfo.ScreenPosition.Y - iconInfo.HotspotY);

                // 背景画像からの相対座標
                imagePoint = new Point(
                    x: screenPoint.X - backgroundImageScreenPoint.X,
                    y: screenPoint.Y - backgroundImageScreenPoint.Y);

                // カーソルの画像を取得
                if (backgroundImage == null)
                {
                    // 背景画像を使用しない場合
                    cursorImage = GetCursorImage(iconHandle, iconInfo);
                }
                else
                {
                    // 背景画像を使用する場合
                    cursorImage = GetCursorImage(iconHandle, iconInfo, backgroundImage, imagePoint);
                }

                // カーソルの画像が取得できない場合は NULL を返す
                if (cursorImage == null)
                {
                    return null;
                }
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

            // カーソル情報を生成して返す
            return new CursorInfo(cursorImage, screenPoint, imagePoint);
        }

        #endregion

        #endregion

        #region プライベートメソッド

        #region Win32Apiを直接呼び出しているメソッド

        #region カーソルの情報を取得

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
            // Win32ApiのWindou共通の呼び出し機能を用いて、グローバルのカーソルの情報を取得処理を呼び出す
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

        #endregion

        #region アイコンの情報を取得

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
            // Win32ApiのWindou共通の呼び出し機能を用いて、アイコンの情報を取得処理を呼び出す
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

        #endregion

        #region アイコンの複製

        /// <summary>
        /// 引数で指定された他のモジュールのアイコンへのハンドル（<paramref name="iconCursorHandle"/>）を
        /// 現在のモジュールのアイコンへのハンドルに複製する
        /// </summary>
        /// <remarks>
        /// この機能は、別のモジュールが所有しているアイコンを、現在のモジュールへの独自のハンドルで取得する
        /// その結果、他のモジュールが解放されてもアプリケーションアイコンはアイコンとして使用することができる
        /// </remarks>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：CopyIcon」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：CopyIcon」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>複製したアイコンへのハンドル</returns>
        private static IntPtr CopyIcon(IntPtr iconCursorHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、アイコンの複製処理を呼び出す
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

        #endregion

        #region デバイスコンテキストの作成・選択

        /// <summary>
        /// 指定されたデバイスと互換性のあるメモリデバイスコンテキスト（DC）を作成する
        /// （作成したデバイスコンテキスト（DC）が不要になった場合、<see cref="DeleteObject(IntPtr)"/> で破棄する必要がある）
        /// </summary>
        /// <param name="targetDCHandle">
        /// 既存のデバイスコンテキスト（DC）へのハンドル
        /// 指定したデバイスコンテキスト（DC）関連するデバイスと互換性のあるメモリデバイスコンテキスト（DC）を作成する
        /// NULL（<see cref="IntPtr.Zero"/>）を指定した場合、アプリケーションの現在の画面と互換性のある
        /// メモリデバイスコンテキストを作成する
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：CreateCompatibleDC」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：CreateCompatibleDC」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>作成したメモリデバイスコンテキスト（DC）へのハンドル</returns>
        private static IntPtr CreateCompatibleDC(IntPtr targetDCHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、メモリデバイスコンテキストの作成処理を呼び出す
            Win32ApiResult function()
            {
                IntPtr win32ReturnValue = Win32Api.CreateCompatibleDC(targetDCHandle);
                int win32ErrorCode = Marshal.GetLastWin32Error();
                bool win32Result = Win32Api.CreateCompatibleDCParameter.IsSuccess(win32ReturnValue);

                return new Win32ApiResult(win32ReturnValue, win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "gdi32.dll";
            string methodName = nameof(Win32Api.CreateCompatibleDC);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }

            // 作成したメモリデバイスコンテキスト（DC）へのハンドルを返却
            return (IntPtr)result.ReturnValue;
        }

        /// <summary>
        /// 指定されたデバイスコンテキスト（DC）においてビットマップを選択する
        /// （新しいビットマップを指定した場合は、前のビットマップを置き換える
        /// 　新しいビットマップの描画が終了した場合、元のデフォルトのビットマップに戻す必要がある）
        /// </summary>
        /// <param name="targetDCHandle">
        /// 対象とするデバイスコンテキスト（DC）へのハンドル
        /// </param>
        /// <param name="bitmapHandle">
        /// 選択するビットマップへのハンドル
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：SelectObject」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：SelectObject」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>
        /// 以前に選択されていたビットマップオブジェクトへのハンドル（元のビットマップオブジェクトへのハンドル）
        /// </returns>
        private static IntPtr SelectBitmap(IntPtr targetDCHandle, IntPtr bitmapHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、オブジェクト選択処理を呼び出す
            Win32ApiResult function()
            {
                IntPtr win32ReturnValue = Win32Api.SelectObject(targetDCHandle, bitmapHandle);
                bool win32Result = Win32Api.SelectObjectParameter.IsSuccess(win32ReturnValue, false);

                return new Win32ApiResult(win32ReturnValue, win32Result);
            }

            // 実行
            string dllName = "gdi32.dll";
            string methodName = nameof(Win32Api.SelectObject);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }

            // 以前に選択されていたビットマップオブジェクトへのハンドルを返却
            return (IntPtr)result.ReturnValue;
        }

        #endregion

        #region 画像データのビットブロック転送（BitBlt）

        /// <summary>
        /// 送信元のデバイスコンテキストに設定されたビットマップ情報（ピクセル情報）を
        /// 送信先のデバイスコンテキスにビットブロック転送する
        /// </summary>
        /// <param name="destDCHandle">送信先のデバイスコンテキストへのハンドル</param>
        /// <param name="destPointX">送信先の描画位置：X（左上が基準）</param>
        /// <param name="destPointY">送信先の描画位置：Y（左上が基準）</param>
        /// <param name="width">転送元 及び、転送先の幅（転送するデータの幅）</param>
        /// <param name="height">転送元 及び、転送先の高さ（転送するデータの高さ）</param>
        /// <param name="sourceDCHandle">送信元のデバイスコンテキストへのハンドル</param>
        /// <param name="sourcePointX">送信元の基準位置：X（左上が基準）</param>
        /// <param name="sourcePointY">送信元の基準位置：Y（左上が基準）</param>
        /// <param name="ropCode">合成方法を定めるラスタオペレーションコード</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：BitBlt」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：BitBlt」の処理に失敗した場合に発生
        /// </exception>
        private static void BitBlt(
            IntPtr destDCHandle,
            int destPointX,
            int destPointY,
            int width,
            int height,
            IntPtr sourceDCHandle,
            int sourcePointX,
            int sourcePointY,
            RasterOperation.CommonCode ropCode)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ビットブロック転送処理を呼び出す
            Win32ApiResult function()
            {
                bool win32Result = Win32Api.BitBlt(
                    destDCHandle: destDCHandle,
                    destPointX: destPointX,
                    destPointY: destPointY,
                    width: width,
                    height: height,
                    sourceDCHandle: sourceDCHandle,
                    sourcePointX: sourcePointX,
                    sourcePointY: sourcePointY,
                    ropCode: (uint)ropCode);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "gdi32.dll";
            string methodName = nameof(Win32Api.BitBlt);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        #endregion

        #region リソースの解放

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
            // Win32ApiのWindou共通の呼び出し機能を用いて、アイコンの破棄処理を呼び出す
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
        /// 指定されたデバイスコンテキスト（DC）を破棄する
        /// （<see cref="CreateCompatibleDC(IntPtr)"/> で複製したデバイスコンテキスト（DC）は必ずこのメソッドで破棄する必要がある）
        /// </summary>
        /// <param name="targetDCHandle">デバイスコンテキスト（DC）へのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：DeleteDC」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：DeleteDC」の処理に失敗した場合に発生
        /// </exception>
        private static void DeleteDC(IntPtr targetDCHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、デバイスコンテキスト（DC）の破棄処理を呼び出す
            Win32ApiResult function()
            {
                bool win32Result = Win32Api.DeleteDC(targetDCHandle);

                return new Win32ApiResult(win32Result);
            }

            // 実行
            string dllName = "gdi32.dll";
            string methodName = nameof(Win32Api.DeleteDC);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName);
            }
        }

        /// <summary>
        /// オブジェクトに関連付けられているすべてのシステムリソースを解放する
        /// （論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットを破棄する
        /// 　オブジェクトが破棄されると、指定されたハンドルは無効になる）
        /// </summary>
        /// <param name="objectHandle">
        /// 論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットへのハンドル
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：DeleteObject」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：gdi32.dll、メソッド：DeleteObject」の処理に失敗した場合に発生
        /// </exception>
        private static void DeleteObject(IntPtr objectHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、オブジェクトの破棄処理を呼び出す
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

        #region カーソル関連のメソッド

        /// <summary>
        /// カーソルの画像を取得する
        /// </summary>
        /// <param name="iconHandle">
        /// カーソルアイコンへのハンドル
        /// </param>
        /// <param name="iconInfo">
        /// カーソルアイコン情報
        /// </param>
        /// <param name="backgroundImage">
        /// カーソルの背景画像（NULLを指定した場合は白一色の背景として描画する）
        /// </param>
        /// <param name="drawPoint">
        /// 背景画像に対してカーソルを描画する座標
        /// （背景画像を使用しない場合は NULL を指定する）</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：gdi32.dll、メソッド：CreateCompatibleDC」
        /// ・「DLL：gdi32.dll、メソッド：SelectObject」
        /// ・「DLL：gdi32.dll、メソッド：BitBlt」
        /// ・「DLL：gdi32.dll、メソッド：DeleteObject」
        /// ・「DLL：gdi32.dll、メソッド：DeleteDC」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：gdi32.dll、メソッド：CreateCompatibleDC」
        /// ・「DLL：gdi32.dll、メソッド：SelectObject」
        /// ・「DLL：gdi32.dll、メソッド：BitBlt」
        /// ・「DLL：gdi32.dll、メソッド：DeleteObject」
        /// ・「DLL：gdi32.dll、メソッド：DeleteDC」
        /// </exception>
        /// <returns>カーソル画像（取得できない場合はNULLを返却する）</returns>
        private static Image GetCursorImage(
            IntPtr iconHandle, IconInfo.ICONINFO iconInfo, Bitmap backgroundImage = null, Point? drawPoint = null)
        {
            // カラー・マスク情報が存在するか判定
            bool hasColor = IsBitmap(iconInfo.ColorBitmapHandle);
            bool hasMask = IsBitmap(iconInfo.MaskBitmapHandle);

            // アイコンのカラー、マスクの両方が取得できない場合
            // 画像情報は取得できないため NULL を返す
            if (!hasColor && !hasMask)
            {
                return null;
            }

            // アイコンのハンドルからカーソルアイコンを取得する
            Icon cursorIcon;
            try
            {
                cursorIcon = Icon.FromHandle(iconHandle);
            }
            catch (ExternalException)
            {
                // アイコンのハンドルからカーソルアイコンを生成できない場合、NULL を返す
                return null;
            }

            // カーソルがモノクロかカラーでそれぞれ取得を行う
            if (hasColor)
            {
                // カラーの場合
                // アイコン画像をビットマップ形式に変換して、そのまま返す
                return cursorIcon.ToBitmap();
            }

            // モノクロの場合
            // カーソル画像取得処理で使用し破棄の処理が必要な変数を宣言
            Bitmap baseImage = null;
            Bitmap cursorImage = null;
            bool isCreateBase = false;
            try
            {
                // 引数で背景画像が与えられていない場合、白一色の背景画像を生成する
                if (backgroundImage == null)
                {
                    baseImage = new Bitmap(cursorIcon.Width, cursorIcon.Height);
                    isCreateBase = true;
                    using (Graphics graphics = Graphics.FromImage(baseImage))
                    {
                        graphics.FillRectangle(Brushes.White, graphics.VisibleClipBounds);
                    }
                }

                // カーソル画像の元データを生成
                cursorImage = new Bitmap(cursorIcon.Width, cursorIcon.Height);

                // アイコン情報のマスク情報を使用してカーソル画像を描画する
                using (Graphics cursorImageGraphics = Graphics.FromImage(cursorImage))
                using (Bitmap maskImage = Bitmap.FromHbitmap(iconInfo.MaskBitmapHandle))
                {
                    // リソースの解放処理が必要な変数の宣言
                    IntPtr cursorImageHdc = IntPtr.Zero;
                    IntPtr baseHdc = IntPtr.Zero;
                    IntPtr beforeBase = IntPtr.Zero;
                    IntPtr maskHdc = IntPtr.Zero;
                    IntPtr beforeMask = IntPtr.Zero;
                    try
                    {
                        // カーソル画像のデバイスコンテキストを取得
                        cursorImageHdc = cursorImageGraphics.GetHdc();

                        // ベース画像のデバイスコンテキストを取得
                        baseHdc = CreateCompatibleDC(IntPtr.Zero);
                        IntPtr baseHBitmap = backgroundImage != null
                            ? backgroundImage.GetHbitmap() : baseImage.GetHbitmap();
                        beforeBase = SelectBitmap(baseHdc, baseHBitmap);

                        // マスク画像のデバイスコンテキストを取得
                        maskHdc = CreateCompatibleDC(IntPtr.Zero);
                        beforeMask = SelectBitmap(maskHdc, maskImage.GetHbitmap());

                        // 画像の合成処理
                        int width = cursorImage.Width;
                        int height = cursorImage.Height;
                        Point base1Point = drawPoint ?? new Point(0, 0);
                        Point mask1Point = new Point(0, 0);
                        Point mask2Point = new Point(0, maskImage.Height / 2);
                        BitBlt(cursorImageHdc, 0, 0, width, height, baseHdc, base1Point.X, base1Point.Y, ROPCode.SRCCOPY);
                        BitBlt(cursorImageHdc, 0, 0, width, height, maskHdc, mask1Point.X, mask1Point.Y, ROPCode.SRCAND);
                        BitBlt(cursorImageHdc, 0, 0, width, height, maskHdc, mask2Point.X, mask2Point.Y, ROPCode.SRCINVERT);
                    }
                    finally
                    {
                        // リソースの解放処理
                        // カーソル画像のデバイスコンテキストを破棄
                        if (cursorImageHdc != IntPtr.Zero)
                        {
                            cursorImageGraphics?.ReleaseHdc(cursorImageHdc);
                        }

                        try
                        {
                            // ベース画像のデバイスコンテキストの破棄
                            IntPtr baseHandle = IntPtr.Zero;
                            if (baseHdc != IntPtr.Zero)
                            {
                                baseHandle = SelectBitmap(baseHdc, beforeBase);
                            }

                            if (baseHandle != IntPtr.Zero)
                            {
                                DeleteObject(baseHandle);
                            }

                            if (baseHdc != IntPtr.Zero)
                            {
                                DeleteDC(baseHdc);
                            }
                        }
                        finally
                        {
                            // マスク画像のデバイスコンテキストの破棄
                            IntPtr maskHandle = IntPtr.Zero;
                            if (maskHdc != IntPtr.Zero)
                            {
                                maskHandle = SelectBitmap(maskHdc, beforeMask);
                            }

                            if (maskHandle != IntPtr.Zero)
                            {
                                DeleteObject(maskHandle);
                            }

                            if (maskHdc != IntPtr.Zero)
                            {
                                DeleteDC(maskHdc);
                            }
                        }
                    }
                }

                // 背景画像を白一色で生成した場合、背景を透過する
                if (isCreateBase)
                {
                    cursorImage.MakeTransparent(Color.White);
                }

                // カーソル画像を返す
                return cursorImage;
            }
            catch
            {
                // 例外発生時はカーソル画像を破棄する
                cursorImage?.Dispose();

                // 発生した例外はそのままスローする
                throw;
            }
            finally
            {
                // このメソッドでベースイメージを作成した場合は、画像データを破棄
                baseImage?.Dispose();
            }
        }

        /// <summary>
        /// ビットマップハンドルが示す情報がビットマップデータであるか判定する
        /// （カラービットマップ 又は、マスクビットマップ情報が存在するのチェックに使用する）
        /// </summary>
        /// <remarks>
        /// 当メソッドはパフォーマンスに懸念があるため、
        /// カーソルのような小さい画像でない場合の呼び出しについては十分考慮すること
        /// </remarks>
        /// <param name="bitmapHandle">ビットマップへのハンドル</param>
        /// <returns>
        /// 戻り値：カラー 又は、マスク情報が存在する場合：True、存在しない場合：False
        /// </returns>
        private static bool IsBitmap(IntPtr bitmapHandle)
        {
            // ハンドルを持っているかチェック
            bool hasBitmap = bitmapHandle != IntPtr.Zero;
            if (hasBitmap)
            {
                // ハンドルを持っている場合、ハンドルが示す情報がビットマップか判定する
                try
                {
                    Image.FromHbitmap(bitmapHandle)?.Dispose();

                    // ビットマップとして取得できるため True を返す
                    return true;
                }
                catch (ExternalException)
                {
                    // ビットマップデータではないため False を返す
                    return false;
                }
            }
            else
            {
                // ハンドルを持っていないため False を返す
                return false;
            }
        }

        #endregion

        #endregion
    }
}
