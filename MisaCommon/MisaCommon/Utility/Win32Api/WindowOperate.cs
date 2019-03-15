namespace MisaCommon.Utility.Win32Api
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.InteropServices;

    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.Utility.Win32Api.NativeMethod;
    using Win32Api = NativeMethod.Window.NativeMethods;
    using Win32MessageApi = NativeMethod.Message.NativeMethods;

    /// <summary>
    /// Win32APIの機能を使用してウィンドウに対する操作を行うクラス
    /// </summary>
    public static class WindowOperate
    {
        #region ウィンドウのハンドルを取得

        /// <summary>
        /// 画面上の全てのトップレベルウィンドウのウィンドウハンドルのリストを取得
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：EnumWindows」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：EnumWindows」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>画面上の全てのトップレベルウィンドウのウィンドウハンドルのリスト</returns>
        public static ICollection<IntPtr> WindowHandleList()
        {
            // ウィンドウハンドルリストを生成する
            ICollection<IntPtr> windowHandleList = new Collection<IntPtr>();

            // コールバック関数に、一時リストにウィンドウハンドル格納する処理を指定して列挙処理を実行
            // トップレベルウィンドウのウィンドウハンドルリストを取得する
            EnumWindows(
                callBack: (windowHandle, value) =>
                    {
                        windowHandleList.Add(windowHandle);
                        return true;
                    },
                callBackValue: IntPtr.Zero);

            // ウィンドウハンドルを格納したリストを返す
            return windowHandleList;
        }

        /// <summary>
        /// 画面上の全てのトップレベルウィンドウを列挙する
        /// </summary>
        /// <remarks>
        /// このメソッドを呼び出すと、下記の動作を繰り返し実行しトップレベルウィンドウの列挙を行う
        /// 　1) トップレベルウィンドウを取得
        /// 　2) 取得したトップレベルウィンドウのハンドルを引数のコールバック関数（<paramref name="callBack"/>）に設定する
        /// 　3) コールバック関数（<paramref name="callBack"/>）を実行する
        ///
        /// 上記処理は全てのトップレベルウィンドウを列挙し終えるか、
        /// コールバック関数（<paramref name="callBack"/>）から False が返却されるまで繰り返す
        /// </remarks>
        /// <param name="callBack">
        /// <see cref="Func{T1, T2, TResult}"/> で定義されるコールバック関数を指定
        /// コールバック関数はコールバック元から受け取った引数に対して処理を行い、戻り値を返却するメソッドである
        /// 引数及び、戻り値の詳細は下記のとおり
        /// 　・引数1（型：IntPtr）：コールバック元から引き渡されるトップレベルウィンドウのハンドル
        /// 　・引数2（型：IntPtr）：コールバック元から引き渡される値
        /// 　・戻り値（型：bool）：コールバック元の列挙処理を継続する場合は True、中断する場合は False を返却
        /// </param>
        /// <param name="callBackValue">
        /// コールバック関数に渡す値を指定
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：EnumWindows」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：EnumWindows」の処理に失敗した場合に発生
        /// </exception>
        public static void EnumWindows(Func<IntPtr, IntPtr, bool> callBack, IntPtr callBackValue)
        {
            // 引数のコールバック関数をデリゲートに変換するためのローカル関数
            bool CallBackDelegate(IntPtr windowHandle, IntPtr value)
            {
                return callBack(windowHandle, value);
            }

            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウ列挙処理を呼び出し列挙を行う
            Win32ApiResult function()
            {
                bool win32Result = Win32Api.EnumWindows(CallBackDelegate, callBackValue);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.EnumWindows);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        /// <summary>
        /// 現在ユーザーが作業している最前面ウィンドウのハンドルを取得する
        /// 取得できなかった場合は <see cref="IntPtr.Zero"/> を返却する
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetForegroundWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindowVisible」
        /// ・「DLL：user32.dll、メソッド：IsWindowEnabled」
        /// ・「DLL：user32.dll、メソッド：GetDesktopWindow」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetForegroundWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindowVisible」
        /// ・「DLL：user32.dll、メソッド：IsWindowEnabled」
        /// ・「DLL：user32.dll、メソッド：GetDesktopWindow」
        /// （例外発生フラグが True の場合で、取得したウィンドウハンドルの値が相応しくない場合もこの例外を発生させる）
        /// </exception>
        /// <returns>
        /// 現在ユーザーが作業している最前面ウィンドウのハンドル
        /// 取得できなかった場合は<see cref="IntPtr.Zero"/> を返却する
        /// </returns>
        public static IntPtr GetForegroundWindowHandle()
        {
            return GetForegroundWindowHandle(false);
        }

        /// <summary>
        /// 現在ユーザーが作業している最前面ウィンドウのハンドルを取得する
        /// 取得できなかった場合は <see cref="IntPtr.Zero"/> を返却する
        /// </summary>
        /// <param name="isThrowExceptionGetFail">
        /// 取得したウィンドウハンドルに該当するウィンドウが下記の場合に例外を発生させるかどうかを指定するフラグ
        /// ・アクティブウィンドウが存在しない
        /// ・取得したウィンドウハンドルのウィンドウがウィンドウではない
        /// ・表示されない（非表示）ウィンドウ
        /// ・無効（入力を受け付けない）なウィンドウ
        /// ・ディスクトップを示すウィンドウ
        /// このフラグを True にした場合、上記状態の場合に例外をスローする
        /// このフラグを False にした場合、上記状態の場合に <see cref="IntPtr.Zero"/> を返却する
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetForegroundWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindowVisible」
        /// ・「DLL：user32.dll、メソッド：IsWindowEnabled」
        /// ・「DLL：user32.dll、メソッド：GetDesktopWindow」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetForegroundWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindowVisible」
        /// ・「DLL：user32.dll、メソッド：IsWindowEnabled」
        /// ・「DLL：user32.dll、メソッド：GetDesktopWindow」
        /// （例外発生フラグが True の場合で、取得したウィンドウハンドルの値が相応しくない場合もこの例外を発生させる）
        /// </exception>
        /// <returns>
        /// 現在ユーザーが作業している最前面ウィンドウのハンドル
        /// 取得できなかった場合は<see cref="IntPtr.Zero"/> を返却する
        /// </returns>
        public static IntPtr GetForegroundWindowHandle(bool isThrowExceptionGetFail)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、最前面のウィンドウのハンドルを取得する
            Win32ApiResult function()
            {
                // 最前面のウィンドウのハンドルを取得
                IntPtr windowHandle = Win32Api.GetForegroundWindow();

                // 取得したウィンドウのチェック
                bool win32Result = windowHandle != IntPtr.Zero
                    && Win32Api.IsWindow(windowHandle)
                    && Win32Api.IsWindowVisible(windowHandle)
                    && Win32Api.IsWindowEnabled(windowHandle)
                    && windowHandle != Win32Api.GetDesktopWindow();

                // チェックOKの場合は取得したウィンドウハンドルを、NGの場合はIntPtr.Zeroを返却
                return new Win32ApiResult(win32Result ? windowHandle : IntPtr.Zero);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.GetForegroundWindow);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);
            IntPtr handle = (IntPtr)result.ReturnValue;

            // 例外発生フラグが立っている場合、取得成功か判定
            if (isThrowExceptionGetFail && handle == IntPtr.Zero)
            {
                // 取得失敗の場合、例外をスローする
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName);
            }

            // 取得したウインドウハンドルを返却
            return handle;
        }

        #endregion

        #region ウインドウが表示されている状態の取得（表示されてるか、最小化されてるか等）

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウが、
        /// 存在しているかチェックする
        /// </summary>
        /// <param name="windowHandle">チェック対象のウィンドウのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：IsWindow」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：IsWindow」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>ウィンドウが存在する場合：True、存在しない場合：False</returns>
        public static bool IsWindow(IntPtr windowHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの存在チェック処理を呼び出す
            Win32ApiResult function()
            {
                bool win32ReturnValue = Win32Api.IsWindow(windowHandle);

                return new Win32ApiResult(win32ReturnValue);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.IsWindow);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 取得したウインドウハンドルを返却
            return (bool)result.ReturnValue;
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウが、
        /// 有効か無効か（マウスやキーボードの入力を受け付けるか受け付けないか）をチェックする
        /// </summary>
        /// <param name="windowHandle">チェック対象のウィンドウのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：IsWindowEnabled」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：IsWindowEnabled」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>ウィンドウが有効な場合：True、無効な場合：False</returns>
        public static bool IsWindowEnabled(IntPtr windowHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの有効無効チェック処理を呼び出す
            Win32ApiResult function()
            {
                bool win32ReturnValue = Win32Api.IsWindowEnabled(windowHandle);

                return new Win32ApiResult(win32ReturnValue);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.IsWindowEnabled);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 取得したウインドウハンドルを返却
            return (bool)result.ReturnValue;
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウが、
        /// 表示されているかチェックする
        /// </summary>
        /// <param name="windowHandle">チェック対象のウィンドウのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：IsWindowVisible」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：IsWindowVisible」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>ウィンドウが表示されている場合：True、表示されていない場合：False</returns>
        public static bool IsWindowVisible(IntPtr windowHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの表示有無チェック処理を呼び出す
            Win32ApiResult function()
            {
                bool win32ReturnValue = Win32Api.IsWindowVisible(windowHandle);

                return new Win32ApiResult(win32ReturnValue);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.IsWindowVisible);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 取得したウインドウハンドルを返却
            return (bool)result.ReturnValue;
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウが、
        /// 最小化（アイコン化）されているかチェックする
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：IsIconic」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：IsIconic」の処理に失敗した場合に発生
        /// </exception>
        /// <param name="windowHandle">チェック対象のウィンドウのハンドル</param>
        /// <returns>ウィンドウが最小化されている場合：True、最小化されていない場合：False</returns>
        public static bool IsIconic(IntPtr windowHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの最小化チェック処理を呼び出す
            Win32ApiResult function()
            {
                bool win32ReturnValue = Win32Api.IsIconic(windowHandle);

                return new Win32ApiResult(win32ReturnValue);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.IsIconic);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 取得したウインドウハンドルを返却
            return (bool)result.ReturnValue;
        }

        #endregion

        #region ウィンドウに対する情報を取得

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウにおいて
        /// そのウィンドウを作成した、スレッドのID 及び プロセスID等の情報を取得する
        /// </summary>
        /// <param name="windowHandle">取得対象のウィンドウのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：GetWindowThreadProcessId」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：GetWindowThreadProcessId」の処理に失敗した場合に発生
        /// </exception>
        /// <returns>ウィンドウを作成したスレッドIDとプロセスID等のウィンドウの情報</returns>
        public static WindowInfo GetWindowThreadProcessId(IntPtr windowHandle)
        {
            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、スレッドID、プロセスIDの取得処理を呼び出す
            Win32ApiResult function()
            {
                int threadId = Win32Api.GetWindowThreadProcessId(windowHandle, out int processId);
                int win32ErrorCode = Marshal.GetLastWin32Error();
                bool win32Result = Win32Api.GetWindowThreadProcessIdParameter.IsSuccess(threadId);

                WindowInfo windowInfo = new WindowInfo(windowHandle, threadId, processId);
                return new Win32ApiResult(windowInfo, win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.GetWindowThreadProcessId);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }

            // 取得したスレッドIDとプロセスID等のウィンドウの情報を返却
            return (WindowInfo)result.ReturnValue;
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウにおいて、
        /// そのウィンドウのサイズ位置情報を取得する
        /// </summary>
        /// <param name="windowHandle">取得対象のウィンドウのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsIconic」
        /// ・「DLL：user32.dll、メソッド：GetWindowRect」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsIconic」
        /// ・「DLL：user32.dll、メソッド：GetWindowRect」
        /// </exception>
        /// <returns>
        /// ウィンドウのサイズ位置情報
        /// ウィンドウが存在しない、非表示、最小化状態の場合はNULLを返却
        /// </returns>
        public static SizePoint GetWindowRect(IntPtr windowHandle)
        {
            // ウィンドウが存在しない、非表示、最小化状態の場合は処理をせずに終了する
            if (!IsWindow(windowHandle)
                || !IsWindowVisible(windowHandle)
                || IsIconic(windowHandle))
            {
                return null;
            }

            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、上下左右の座標情報の取得処理を呼び出す
            Win32ApiResult function()
            {
                bool win32Result = Win32Api.GetWindowRect(windowHandle, out Win32Api.RECT rect);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                int positionX = rect.Left;
                int positionY = rect.Top;
                SizePoint sizePoint = new SizePoint(width, height, positionX, positionY);

                return new Win32ApiResult(sizePoint, win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.GetWindowRect);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }

            // 取得したウィンドウのサイズ位置情報を返却
            return (SizePoint)result.ReturnValue;
        }

        #endregion

        #region ウインドウに対する設定

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウを最小化する
        /// </summary>
        /// <param name="windowHandle">最小化対象のウィンドウのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsIconic」
        /// ・「DLL：user32.dll、メソッド：IconicWindow」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsIconic」
        /// ・「DLL：user32.dll、メソッド：IconicWindow」
        /// </exception>
        public static void IconicWindow(IntPtr windowHandle)
        {
            // 既に最小化状態の場合は処理をせずに終了する
            if (IsIconic(windowHandle))
            {
                return;
            }

            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウを最小化する
            Win32ApiResult function()
            {
                bool win32Result = Win32Api.CloseWindow(windowHandle);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.CloseWindow);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウを最小化状態から元に戻す
        /// 元に戻したウィンドウはアクティブにする
        /// </summary>
        /// <param name="windowHandle">最小化状態を戻す対象のウィンドウのハンドル</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsIconic」
        /// ・「DLL：user32.dll、メソッド：OpenIcon」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsIconic」
        /// ・「DLL：user32.dll、メソッド：OpenIcon」
        /// </exception>
        public static void OpenIconicWindow(IntPtr windowHandle)
        {
            // 既に最小化状態でないの場合は処理をせずに終了する
            if (!IsIconic(windowHandle))
            {
                return;
            }

            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウの最小化状態を元に戻す
            Win32ApiResult function()
            {
                bool win32Result = Win32Api.OpenIcon(windowHandle);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.OpenIcon);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウのサイズ、位置を変更する
        /// </summary>
        /// <param name="windowHandle">変更対象のウィンドウのハンドル</param>
        /// <param name="sizePoint">設定するウィンドウのサイズと位置</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="sizePoint"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SetWindowSizeLocation」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SetWindowSizeLocation」
        /// </exception>
        public static void SetWindowSizeLocation(IntPtr windowHandle, SizePoint sizePoint)
        {
            // 対象のウィンドウが存在しない場合は処理を終了する
            if (!IsWindow(windowHandle))
            {
                return;
            }

            // NULLチェック
            if (sizePoint == null)
            {
                throw new ArgumentNullException(nameof(sizePoint));
            }

            // ウィンドウを閉じるコマンドメッセージをの定義
            // Zオーダーの配置順序の変更なしを指定
            IntPtr order = IntPtr.Zero;
            uint option = (uint)Win32Api.SetWindowPosParameter.OptionFlag.SWP_NOZORDER;

            // Win32Apiの実行処理
            // Win32ApiのWindou共通の呼び出し機能を用いて、ウィンドウのサイズ、位置を変更する
            Win32ApiResult function()
            {
                // 実行
                bool win32Result = Win32Api.SetWindowPos(
                    windowHandle: windowHandle,
                    windowHandleOrder: order,
                    pointX: sizePoint.PositionX,
                    pointY: sizePoint.PositionY,
                    sizeWidth: sizePoint.SizeWidth,
                    sizeHeight: sizePoint.SizeHeight,
                    optionFlag: option);
                int win32ErrorCode = Marshal.GetLastWin32Error();

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.SetWindowPos);
            Win32ApiResult result = Win32ApiCommon.Run(function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        #endregion

        #region ウィンドウを閉じる

        /// <summary>
        /// 現在ユーザーが作業している最前面ウィンドウを閉じる
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetForegroundWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindowVisible」
        /// ・「DLL：user32.dll、メソッド：IsWindowEnabled」
        /// ・「DLL：user32.dll、メソッド：GetDesktopWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：GetForegroundWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：IsWindowVisible」
        /// ・「DLL：user32.dll、メソッド：IsWindowEnabled」
        /// ・「DLL：user32.dll、メソッド：GetDesktopWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        public static void CloseActiveWindow()
        {
            // 現在ユーザーが作業している最前面ウィンドウのハンドルを取得
            IntPtr handle = GetForegroundWindowHandle();
            if (handle == IntPtr.Zero)
            {
                // ウィンドウハンドルが取得できなかった場合は処理を終了する
                return;
            }

            // ウィンドウを閉じる
            CloseWindow(handle);
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウを閉じる
        /// （タイムアウト時に関する例外は発生させない）
        /// </summary>
        /// <param name="windowHandle">
        /// 送信対象のウィンドウのハンドル
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        public static void CloseWindow(IntPtr windowHandle)
        {
            CloseWindow(windowHandle, false, true);
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウを閉じる
        /// （引数（<paramref name="isThrowExceptionCloseFail"/>）にて例外の発生有無を制御する）
        /// </summary>
        /// <param name="windowHandle">
        /// 送信対象のウィンドウのハンドル
        /// </param>
        /// <param name="isThrowExceptionCloseFail">
        /// ウィンドウを閉じる処理に失敗した場合に例外を発生させるかどうかを指定するフラグ
        /// 閉じるだけの処理であるため失敗しても良いと判断される場合は False を設定、
        /// 閉じる処理の成功の保証がいる場合は Ture を設定
        /// 失敗するパターンとしては下記のパターンが考えられる
        /// ・メモ帳等を閉じる際に表示される「保存しますか？」のダイアログが表示され待機が発生しタイムアウトが発生した
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        public static void CloseWindow(IntPtr windowHandle, bool isThrowExceptionCloseFail)
        {
            CloseWindow(windowHandle, isThrowExceptionCloseFail, false);
        }

        /// <summary>
        /// 引数（<paramref name="windowHandle"/>）のウインドウハンドルを持つウィンドウを閉じる
        /// </summary>
        /// <param name="windowHandle">
        /// 送信対象のウィンドウのハンドル
        /// </param>
        /// <param name="isThrowExceptionCloseFail">
        /// ウィンドウを閉じる処理に失敗した場合に例外を発生させるかどうかを指定するフラグ
        /// 閉じるだけの処理であるため失敗しても良いと判断される場合は False を設定、
        /// 閉じる処理の成功の保証がいる場合は Ture を設定
        /// 失敗するパターンとしては下記のパターンが考えられる
        /// ・メモ帳等を閉じる際に表示される「保存しますか？」のダイアログが表示され待機が発生しタイムアウトが発生した
        /// </param>
        /// <param name="isExcludeTimeoutExceptions">
        /// 発生させる例外のうち、タイムアウトに関する例外は除外するかどうかを指定するフラグ
        /// 引数 <paramref name="isExcludeTimeoutExceptions"/> が True の場合のみ有効
        /// このフラグを True した場合、タイムアウトに関するもの以外の例外のみが発生する
        /// このフラグを False にした場合、タイムアウトに関するものも含めて例外が発生する
        /// 保存の確認ダイアログ等の待ちでタイムアウトが発生する場合があるため、
        /// その際に例外を発生させるかどうかの制御として使用
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの下記の処理の呼び出しに失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの下記の処理に失敗した場合に発生
        /// ・「DLL：user32.dll、メソッド：IsWindow」
        /// ・「DLL：user32.dll、メソッド：SendMessage」
        /// </exception>
        public static void CloseWindow(IntPtr windowHandle, bool isThrowExceptionCloseFail, bool isExcludeTimeoutExceptions)
        {
            // 閉じる操作のコマンドを取得
            int message = Win32MessageApi.WM_CLOSE.MESSAGE;
            IntPtr paramW = Win32MessageApi.WM_CLOSE.WPARAM;
            IntPtr paramL = Win32MessageApi.WM_CLOSE.LPARAM;
            Func<IntPtr, bool> isSucessFunc = Win32MessageApi.WM_CLOSE.IsSuccess;

            // 閉じるコマンドのメッセージを送信
            MessageOperate.SendMessage(
                windowHandle: windowHandle,
                message: message,
                wParam: paramW,
                lParam: paramL,
                isSucessFunc: isSucessFunc,
                isThrowExceptionCloseFail: isThrowExceptionCloseFail,
                isExcludeTimeoutExceptions: isExcludeTimeoutExceptions);
        }

        #endregion
    }
}
