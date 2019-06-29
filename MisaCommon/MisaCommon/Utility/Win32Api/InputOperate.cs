namespace MisaCommon.Utility.Win32Api
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.Utility.Win32Api.NativeMethod;
    using MisaCommon.Utility.Win32Api.NativeMethod.Input;

    using Win32Api = MisaCommon.Utility.Win32Api.NativeMethod.Input.NativeMethods;

    /// <summary>
    /// Win32APIの機能を使用して入力操作に関する操作を行うクラス
    /// </summary>
    public static class InputOperate
    {
        #region クラス変数・定数

        /// <summary>
        /// 共通的に使用する入力から次の入力までの間隔のデフォルト値（ミリ秒単位）
        /// </summary>
        public const int DefaultInputInterval = 50;

        /// <summary>
        /// マウスの入力から次の入力までの間隔のデフォルト値（ミリ秒単位）
        /// </summary>
        private static int mouseInputInterval = DefaultInputInterval;

        /// <summary>
        /// キーボード及び文字の入力から次の入力までの間隔のデフォルト値（ミリ秒単位）
        /// （同じ文字を高速で連続してシステムに送信すると、
        /// 　1回の入力として判定されてしまうためそれを防ぐために空ける間隔）
        /// </summary>
        private static int keybordInputInterval = DefaultInputInterval;

        #endregion

        #region マウスに関するキーの定義

        /// <summary>
        /// マウスに関するキーの定義
        /// </summary>
        public enum MouseKeys : int
        {
            /// <summary>
            /// キーなし
            /// </summary>
            None,

            /// <summary>
            /// マウスの左ボタン
            /// </summary>
            LButton,

            /// <summary>
            /// マウスの右ボタン
            /// </summary>
            RButton,

            /// <summary>
            /// マウスの中央ボタン（3ボタンマウス）
            /// </summary>
            MButton,

            /// <summary>
            /// マウスの第1拡張ボタン（5ボタンマウス）
            /// </summary>
            XButton1,

            /// <summary>
            /// マウスの第2拡張ボタン（5ボタンマウス）
            /// </summary>
            XButton2,

            /// <summary>
            /// マウスホイールを前後方向に移動
            /// </summary>
            Wheel,

            /// <summary>
            /// マウスホイールを左右方向に移動
            /// </summary>
            HWheel,
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// マウスの入力から次の入力までの間隔のデフォルト値（ミリ秒単位）を取得・設定する
        /// </summary>
        /// <remarks>
        /// 0未満の値を設定した場合は0として扱う
        /// </remarks>
        public static int MouseInputInterval
        {
            get => mouseInputInterval;
            set => mouseInputInterval = value < 0 ? 0 : value;
        }

        /// <summary>
        /// キーボード及び文字の入力から次の入力までの間隔のデフォルト値（ミリ秒単位）を取得・設定する
        /// （同じ文字を高速で連続してシステムに送信すると、
        /// 　1回の入力として判定されてしまうためそれを防ぐために空ける間隔）
        /// </summary>
        /// <remarks>
        /// 0未満の値を設定した場合は0として扱う
        /// </remarks>
        public static int KeybordInputInterval
        {
            get => keybordInputInterval;
            set => keybordInputInterval = value < 0 ? 0 : value;
        }

        /// <summary>
        /// 押下したままのキーを保持するためのハッシュを取得する
        /// </summary>
        private static HashSet<VirtualKey.Keys> KeepingKeys { get; } = new HashSet<VirtualKey.Keys>();

        #endregion

        #region マウスの操作

        /// <summary>
        /// 引数の <paramref name="point"/> 座標をクリック（左）する
        /// </summary>
        /// <param name="point">クリックする座標</param>
        /// <param name="screenSize">ディスプレイのサイズ</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void MouseClick(Point point, Size screenSize)
        {
            // マウス操作の座標に変換する
            Point mousePoint = Mouse.ToMousePoint(point, screenSize);

            // マウス操作情報の構造体を生成
            Mouse.INPUT[] input = new Mouse.INPUT[0];
            Mouse.OperateFlag flag;

            // マウスのカーソル移動
            flag = Mouse.OperateFlag.MOUSEEVENTF_MOVE | Mouse.OperateFlag.MOUSEEVENTF_ABSOLUTE;
            SetMouseData(mousePoint.X, mousePoint.Y, 0, flag, ref input[0]);
            SendInput(input);

            // マウスをクリックする
            // マウスのクリックを行う
            MouseClick(
                inputMouseKey: MouseKeys.LButton,
                wheelAmount: 0,
                isShift: false,
                isCtrl: false,
                isAlt: false,
                isWindowsLogo: false,
                isKeepPressing: false,
                inputInterval: MouseInputInterval);
        }

        /// <summary>
        /// 引数の <paramref name="inputKey"/> のマウスのボタンをクリックする
        /// （Shift、Ctrl、Alt、Winキーの修飾子も含んでマウスをクリックする、
        /// 引数の <paramref name="inputKey"/> がマウスに関するボタンでない場合は何もしない）
        /// </summary>
        /// <param name="inputKey">マウスのキー入力の情報</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputKey"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void MouseClick(InputKey inputKey)
        {
            MouseClick(inputKey, MouseInputInterval);
        }

        /// <summary>
        /// 引数の <paramref name="inputKey"/> のマウスのボタンをクリックする
        /// （Shift、Ctrl、Alt、Winキーの修飾子も含んでマウスをクリックする、
        /// 引数の <paramref name="inputKey"/> がマウスに関するボタンでない場合は何もしない）
        /// </summary>
        /// <param name="inputKey">マウスのキー入力の情報</param>
        /// <param name="inputInterval">マウスの入力から次の入力までの間隔（ミリ秒単位）</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputKey"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void MouseClick(InputKey inputKey, int inputInterval)
        {
            // NULLチェック
            if (inputKey == null)
            {
                throw new ArgumentNullException(nameof(inputKey));
            }

            // マウスのキー定義に変換する
            // 変換できない場合は対象外のキーであるため処理をせずに終了する
            if (!TryToMouseKey(inputKey.KeyCode, out MouseKeys mouseKey))
            {
                return;
            }

            // マウスのクリックを行う
            MouseClick(
                inputMouseKey: mouseKey,
                wheelAmount: 0,
                isShift: inputKey.Shift,
                isCtrl: inputKey.Ctrl,
                isAlt: inputKey.Alt,
                isWindowsLogo: inputKey.Win,
                isKeepPressing: inputKey.IsKeepPressing,
                inputInterval: inputInterval);
        }

        #endregion

        #region キーボード操作

        /// <summary>
        /// 引数の <paramref name="inputKey"/> のキーを押下する
        /// （Shift、Ctrl、Alt、Winキーの修飾子も含んでキーを押下する）
        /// </summary>
        /// <param name="inputKey">キーボードのキー入力の情報</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputKey"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void KeybordInput(InputKey inputKey)
        {
            // NULLチェック
            if (inputKey == null)
            {
                throw new ArgumentNullException(nameof(inputKey));
            }

            // 仮想キーに変換する
            // 変換できない場合は対象外のキーであるため処理をせずに終了する
            if (!VirtualKey.TryToVirtualKey(inputKey.KeyCode, out VirtualKey.Keys key))
            {
                return;
            }

            // キーボード操作情報の構造体を生成する
            Keyboard.INPUT[] input = GetKeybordInputData(
                key: key,
                isShift: inputKey.Shift,
                isCtrl: inputKey.Ctrl,
                isAlt: inputKey.Alt,
                isWindowsLogo: inputKey.Win,
                isKeepPressing: inputKey.IsKeepPressing);

            // キーボード操作情報が存在する場合、
            // システムにキーボードの入力操作情報を送信する
            if (input.Length > 0)
            {
                SendInput(input);
            }
        }

        /// <summary>
        /// 押下したままのキーを放す
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void KeybordReleaseKey()
        {
            // 押下したままのキーが存在しない場合は処理を終了する
            if (KeepingKeys.Count == 0)
            {
                return;
            }

            // 押下したままのキーを離す入力操作のデータを生成する
            Keyboard.INPUT[] input = new Keyboard.INPUT[KeepingKeys.Count];
            int count = 0;
            foreach (VirtualKey.Keys key in KeepingKeys)
            {
                SetKeyboardInputData(key, false, ref input[count]);
                count++;
            }

            // システムにキーボードの入力操作情報を送信する
            SendInput(input);
        }

        #endregion

        #region マウス・キーボードの共通操作

        /// <summary>
        /// 引数の <paramref name="inputKey"/> の情報に該当するキーを押下する
        /// （Shift、Ctrl、Alt、Winキーの修飾子も含んでキーを押下する）
        /// </summary>
        /// <param name="inputKey">キー入力の情報</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputKey"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void MouseKeybordInput(InputKey inputKey)
        {
            MouseKeybordInput(inputKey, MouseInputInterval);
        }

        /// <summary>
        /// 引数の <paramref name="inputKey"/> の情報に該当するキーを押下する
        /// （Shift、Ctrl、Alt、Winキーの修飾子も含んでキーを押下する）
        /// </summary>
        /// <param name="inputKey">
        /// キー入力の情報
        /// </param>
        /// <param name="mouseInputInterval">
        /// マウスの操作の場合において、マウスの入力から次の入力までの間隔（ミリ秒単位）
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputKey"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void MouseKeybordInput(InputKey inputKey, int mouseInputInterval)
        {
            // NULLチェック
            if (inputKey == null)
            {
                throw new ArgumentNullException(nameof(inputKey));
            }

            // 入力対象のキーコードがマウスに関するものか判定
            if (TryToMouseKey(inputKey.KeyCode, out MouseKeys _))
            {
                // マウスの場合
                // マウスの操作処理を行う
                MouseClick(inputKey, mouseInputInterval);
            }
            else
            {
                // マウス以外の場合
                // キーボードの操作処理を行う
                KeybordInput(inputKey);
            }
        }

        #endregion

        #region キーボード操作：文字入力操作

        /// <summary>
        /// 引数の <paramref name="inputText"/> の文字を入力する
        /// </summary>
        /// <param name="inputText">入力する文字列</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputText"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void TextInput(string inputText)
        {
            TextInput(inputText, false, true, KeybordInputInterval);
        }

        /// <summary>
        /// 引数の <paramref name="inputText"/> の文字を入力する
        /// </summary>
        /// <param name="inputText">
        /// 入力する文字列
        /// </param>
        /// <param name="isEnter">
        /// 入力後にEnterキーを押下する：True、押下しない：False
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputText"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void TextInput(string inputText, bool isEnter)
        {
            TextInput(inputText, isEnter, true, KeybordInputInterval);
        }

        /// <summary>
        /// 引数の <paramref name="inputText"/> の文字を入力する
        /// </summary>
        /// <param name="inputText">
        /// 入力する文字列
        /// </param>
        /// <param name="inputInterval">
        /// キーボード及び文字の入力から次の入力までの間隔のデフォルト値（ミリ秒単位）
        /// （同じ文字を高速で連続してシステムに送信すると、
        /// 　1回の入力として判定されてしまうためそれを防ぐために空ける間隔）
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputText"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void TextInput(string inputText, int inputInterval)
        {
            TextInput(inputText, false, true, inputInterval);
        }

        /// <summary>
        /// 引数の <paramref name="inputText"/> の文字を入力する
        /// </summary>
        /// <param name="inputText">
        /// 入力する文字列
        /// </param>
        /// <param name="isEnter">
        /// 入力後にEnterキーを押下する場合：True、押下しない場合：False
        /// </param>
        /// <param name="isConfirmConvert">
        /// 入力後に変換を確定する場合：True、確定しない場合：False
        /// </param>
        /// <param name="inputInterval">
        /// キーボード及び文字の入力から次の入力までの間隔のデフォルト値（ミリ秒単位）
        /// （同じ文字を高速で連続してシステムに送信すると、
        /// 　1回の入力として判定されてしまうためそれを防ぐために空ける間隔）
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="inputText"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void TextInput(
            string inputText, bool isEnter, bool isConfirmConvert, int inputInterval)
        {
            // 引数チェック
            if (inputText == null)
            {
                // NULLチェック
                throw new ArgumentNullException(nameof(inputText));
            }
            else if (string.IsNullOrEmpty(inputText))
            {
                // 入力文字が空の場合は処理をせずに終了する
                return;
            }

            // 入力用の構造体を生成
            Keyboard.INPUT[] input = new Keyboard.INPUT[1];
            input[0].InputType = Keyboard.InputType;
            input[0].Keyboard.VirtualKeyCode = 0;
            input[0].Keyboard.OperateFlag = (int)Keyboard.OperateFlag.KEYEVENTF_UNICODE;
            input[0].Keyboard.Time = 0;
            input[0].Keyboard.ExtraInfo = IntPtr.Zero;

            // 一文字づつ入力用のデータを設定し
            // システムにUnicode文字の入力情報を送信する
            short inputCharacter;
            short? beforeInputCharacter = null;
            for (int i = 0; i < inputText.Length; i++)
            {
                // 入力する文字を取得
                inputCharacter = (short)inputText[i];

                // 一つ前の文字と入力する文字が同じ場合はちょっと待つ
                if (beforeInputCharacter.HasValue && beforeInputCharacter == inputCharacter)
                {
                    Thread.Sleep(inputInterval);
                }

                // システムに入力する文字を送信する
                input[0].Keyboard.ScanCode = inputCharacter;
                SendInput(input);

                // 現在の文字を一つ前の文字として保持
                beforeInputCharacter = inputCharacter;
            }

            // 文字を入力後、Enterキーを押下又は変換を確定する場合
            if (isEnter || isConfirmConvert)
            {
                // 変換の確定及びEnterキー入力用の構造体を生成する
                Keyboard.INPUT[] enterInput = new Keyboard.INPUT[2];

                // 変換確定のため「半角/全角」キーを押下をシステムに送信する
                SetKeyboardInputData(VirtualKey.Keys.VK_OEM_AUTO, true, ref enterInput[0]);
                SetKeyboardInputData(VirtualKey.Keys.VK_OEM_AUTO, false, ref enterInput[1]);
                SendInput(enterInput);
                Thread.Sleep(inputInterval);

                // 「半角/全角」のもとに戻すためにもう一度押下をシステムに送信する
                SetKeyboardInputData(VirtualKey.Keys.VK_OEM_AUTO, true, ref enterInput[0]);
                SetKeyboardInputData(VirtualKey.Keys.VK_OEM_AUTO, false, ref enterInput[1]);
                SendInput(enterInput);

                // Enterキーを押下する場合はエンターキー押下をシステムに送信する
                // （変換確定後にEnterキーを押下する）
                if (isEnter)
                {
                    Thread.Sleep(inputInterval);
                    SetKeyboardInputData(VirtualKey.Keys.VK_RETURN, true, ref enterInput[0]);
                    SetKeyboardInputData(VirtualKey.Keys.VK_RETURN, false, ref enterInput[1]);
                    SendInput(enterInput);
                }
            }
        }

        #endregion

        #region マウス、キーボード以外の入力デバイスの操作

        /// <summary>
        /// マウス、キーボード以外の入力デバイスの操作を行う
        /// </summary>
        /// <param name="inputs">
        /// キーボード、マウス以外の入力デバイスの操作の情報の配列
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        public static void HardwareInput(params HardwareInput[] inputs)
        {
            // 入力操作の情報が存在しない場合は処理を終了する
            if (inputs == null || inputs.Length == 0)
            {
                return;
            }

            // システムに送信する入力操作情報の構造体を生成する
            Hardware.INPUT[] inputData = new Hardware.INPUT[inputs.Length];
            int index = 0;
            foreach (HardwareInput input in inputs)
            {
                inputData[index].InputType = Hardware.InputType;
                inputData[index].Hardware.Message = input.Message;
                inputData[index].Hardware.LowlParam = input.LowlParam;
                inputData[index].Hardware.HighlParam = input.HighlParam;
            }

            // システムに入力操作情報を送信する
            SendInput(inputData);
        }

        #endregion

        #region プライベートメソッド

        #region Win32ApiのSendInputを呼び出すメソッド

        /// <summary>
        /// マウスの操作を行う
        /// </summary>
        /// <param name="input">入力操作の情報</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        private static void SendInput(Mouse.INPUT[] input)
        {
            // 入力操作の情報が存在しない場合は処理を終了する
            if (input == null || input.Length == 0)
            {
                return;
            }

            // 入力データの数とサイズを取得
            uint count = decimal.ToUInt32(new decimal(input.Length));
            int size = Marshal.SizeOf(typeof(Mouse.INPUT));

            // Win32Apiの実行処理
            // Win32ApiのInput共通の呼び出し機能を用いて、入力操作の送信処理を呼び出す
            Win32ApiResult Function()
            {
                uint tmpResult = Win32Api.SendInput(count, input, size);
                int win32ErrorCode = Marshal.GetLastWin32Error();
                bool win32Result = Win32Api.SendInputParameter.IsSuccess(tmpResult);

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.SendInput);
            Win32ApiResult result = Win32ApiCommon.Run(Function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        /// <summary>
        /// キーボードの操作を行う
        /// </summary>
        /// <param name="input">入力操作の情報</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        private static void SendInput(Keyboard.INPUT[] input)
        {
            // 入力操作の情報が存在しない場合は処理を終了する
            if (input == null || input.Length == 0)
            {
                return;
            }

            // 入力データの数とサイズを取得
            uint count = decimal.ToUInt32(new decimal(input.Length));
            int size = Marshal.SizeOf(typeof(Keyboard.INPUT));

            // Win32Apiの実行処理
            // Win32ApiのInput共通の呼び出し機能を用いて、入力操作の送信処理を呼び出す
            Win32ApiResult Function()
            {
                uint tmpResult = Win32Api.SendInput(count, input, size);
                int win32ErrorCode = Marshal.GetLastWin32Error();
                bool win32Result = Win32Api.SendInputParameter.IsSuccess(tmpResult);

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.SendInput);
            Win32ApiResult result = Win32ApiCommon.Run(Function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        /// <summary>
        /// マウス、キーボード以外の入力デバイスの操作を行う
        /// </summary>
        /// <param name="input">入力操作の情報</param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理「DLL：user32.dll、メソッド：SendInput」の処理に失敗した場合に発生
        /// </exception>
        private static void SendInput(Hardware.INPUT[] input)
        {
            // 入力操作の情報が存在しない場合は処理を終了する
            if (input == null || input.Length == 0)
            {
                return;
            }

            // 入力データの数とサイズを取得
            uint count = decimal.ToUInt32(new decimal(input.Length));
            int size = Marshal.SizeOf(typeof(Hardware.INPUT));

            // Win32Apiの実行処理
            // Win32ApiのInput共通の呼び出し機能を用いて、入力操作の送信処理を呼び出す
            Win32ApiResult Function()
            {
                uint tmpResult = Win32Api.SendInput(count, input, size);
                int win32ErrorCode = Marshal.GetLastWin32Error();
                bool win32Result = Win32Api.SendInputParameter.IsSuccess(tmpResult);

                return new Win32ApiResult(win32Result, win32ErrorCode);
            }

            // 実行
            string dllName = "user32.dll";
            string methodName = nameof(Win32Api.SendInput);
            Win32ApiResult result = Win32ApiCommon.Run(Function, dllName, methodName);

            // 正常終了したかチェック
            if (!result.Result && result.ErrorCode != (int)ErrorCode.NO_ERROR)
            {
                throw Win32ApiCommon.GetWin32OperateException(dllName, methodName, result.ErrorCode);
            }
        }

        #endregion

        #region マウス操作

        /// <summary>
        /// <see cref="System.Windows.Forms.Keys"/> のキーをマウスキー（<see cref="MouseKeys"/>）へ変換する
        /// </summary>
        /// <param name="key">
        /// <see cref="System.Windows.Forms.Keys"/> で定義されるキー
        /// 複数のキーの情報（Ctrl+クリック等）をもっている入力は不可とする、入力された場合は変換不可とする
        /// </param>
        /// <param name="mouseKey">
        /// 変換した仮想キーの値、変換できない場合は <see cref="MouseKeys.None"/> を返却
        /// </param>
        /// <returns>変換に正解した場合は True、変換できない場合は False を返却</returns>
        private static bool TryToMouseKey(Keys key, out MouseKeys mouseKey)
        {
            // 引数のキー毎にマウスのキーであるか判定する
            MouseKeys convertKey;
            bool isMouseKey;
            if (key == Keys.LButton)
            {
                // 左クリック
                convertKey = MouseKeys.LButton;
                isMouseKey = true;
            }
            else if (key == Keys.RButton)
            {
                // 右クリック
                convertKey = MouseKeys.RButton;
                isMouseKey = true;
            }
            else if (key == Keys.MButton)
            {
                // 中央ボタンクリック
                convertKey = MouseKeys.MButton;
                isMouseKey = true;
            }
            else if (key == Keys.XButton1)
            {
                // 第1_Xボタンクリック
                convertKey = MouseKeys.XButton2;
                isMouseKey = true;
            }
            else if (key == Keys.XButton2)
            {
                // 第2_Xボタンクリック
                convertKey = MouseKeys.XButton2;
                isMouseKey = true;
            }
            else
            {
                // 上記以外の場合
                // マウスのキーではないため変換不可を設定
                convertKey = MouseKeys.None;
                isMouseKey = false;
            }

            // 変換結果を返却
            mouseKey = convertKey;
            return isMouseKey;
        }

        /// <summary>
        /// 引数の <paramref name="inputMouseKey"/> のマウスのボタンをクリックする
        /// （Shift、Ctrl、Alt、Winキーの修飾子も含んでマウスをクリックする）
        /// </summary>
        /// <param name="inputMouseKey">
        /// マウスのキー入力の情報
        /// </param>
        /// <param name="wheelAmount">
        /// ホイール量（120単位設定する（1ホイールが120））
        /// </param>
        /// <param name="isShift">
        /// Shiftキーを押している場合：True、押していない場合：False
        /// </param>
        /// <param name="isCtrl">
        /// Ctrlキーを押している場合：True、押していない場合：False
        /// </param>
        /// <param name="isAlt">
        /// Altキーを押している場合：True、押していない場合：False
        /// </param>
        /// <param name="isWindowsLogo">
        /// Windowsロゴキーを押している場合：True、押していない場合：False
        /// </param>
        /// <param name="isKeepPressing">
        /// キーを押しっぱなしにする場合：True、押しっぱなしにしない場合：False
        /// </param>
        /// <param name="inputInterval">
        /// マウスの入力から次の入力までの間隔（ミリ秒単位）
        /// </param>
        private static void MouseClick(
            MouseKeys inputMouseKey,
            int wheelAmount,
            bool isShift,
            bool isCtrl,
            bool isAlt,
            bool isWindowsLogo,
            bool isKeepPressing,
            int inputInterval)
        {
            // 修飾キーを押下するための入力キー情報を生成する
            InputKey modifierKey = new InputKey(Keys.None, isShift, isCtrl, isAlt, isWindowsLogo, true);

            // 修飾キーを押しっぱなしにする
            KeybordInput(modifierKey);

            // マウスの対象ボタンをクリック（押し込む）操作情報を生成
            Mouse.INPUT[] input = CreateMouseInputData(inputMouseKey, wheelAmount, true);

            // マウスの操作情報が存在する場合は、システムにマウスの操作情報を送信する
            if (input.Length > 0)
            {
                SendInput(input);
            }

            // 押しっぱなしにしない場合はキーを離す動作を行う
            if (!isKeepPressing)
            {
                // 指定間隔分待機する
                Thread.Sleep(inputInterval);

                // マウスの対象ボタンをクリック（離す）操作情報を生成
                input = CreateMouseInputData(inputMouseKey, wheelAmount, false);

                // マウスの操作情報が存在する場合は、システムにマウスの操作情報を送信する
                if (input.Length > 0)
                {
                    SendInput(input);
                }

                // 修飾キーの押しっぱなしを解除する
                modifierKey.IsKeepPressing = false;
                KeybordInput(modifierKey);
            }
        }

        /// <summary>
        /// 引数を元にマウスの操作情報を生成する
        /// （ホイール動作においてはキーを押下の場合のみデータを生成し、離す場合は要素0の配列を返却する）
        /// </summary>
        /// <param name="mouseKey">操作するマウスのキー</param>
        /// <param name="wheelAmount">ホイール量（120単位設定する（1ホイールが120））</param>
        /// <param name="isPress">押下する場合：True、離す場合：False</param>
        /// <returns>マウスの操作情報</returns>
        private static Mouse.INPUT[] CreateMouseInputData(
            MouseKeys mouseKey, int wheelAmount, bool isPress)
        {
            // マウス情報を格納する配列を宣言
            Mouse.INPUT[] mouseInputData;

            // 押下するマウスのキー毎にデータの生成を行う
            Mouse.OperateFlag flag;
            int data;
            switch (mouseKey)
            {
                case MouseKeys.None:
                    // 操作をしない
                    // 要素0の配列を設定する
                    mouseInputData = new Mouse.INPUT[0];
                    break;
                case MouseKeys.LButton:
                    // 左クリック
                    mouseInputData = new Mouse.INPUT[1];
                    flag = isPress
                        ? Mouse.OperateFlag.MOUSEEVENTF_LEFTDOWN
                        : Mouse.OperateFlag.MOUSEEVENTF_LEFTUP;
                    data = 0;
                    SetMouseData(0, 0, data, flag, ref mouseInputData[0]);
                    break;
                case MouseKeys.RButton:
                    // 右クリック
                    mouseInputData = new Mouse.INPUT[1];
                    flag = isPress
                        ? Mouse.OperateFlag.MOUSEEVENTF_RIGHTDOWN
                        : Mouse.OperateFlag.MOUSEEVENTF_RIGHTUP;
                    data = 0;
                    SetMouseData(0, 0, data, flag, ref mouseInputData[0]);
                    break;
                case MouseKeys.MButton:
                    // 中央ボタンクリック
                    mouseInputData = new Mouse.INPUT[1];
                    flag = isPress
                        ? Mouse.OperateFlag.MOUSEEVENTF_MIDDLEDOWN
                        : Mouse.OperateFlag.MOUSEEVENTF_MIDDLEUP;
                    data = 0;
                    SetMouseData(0, 0, data, flag, ref mouseInputData[0]);
                    break;
                case MouseKeys.XButton1:
                    // １番目のXボタンクリック
                    mouseInputData = new Mouse.INPUT[1];
                    flag = isPress
                        ? Mouse.OperateFlag.MOUSEEVENTF_XDOWN
                        : Mouse.OperateFlag.MOUSEEVENTF_XUP;
                    data = (int)Mouse.DataXButton.XBUTTON1;
                    SetMouseData(0, 0, data, flag, ref mouseInputData[0]);
                    break;
                case MouseKeys.XButton2:
                    // ２番目のXボタンクリック
                    mouseInputData = new Mouse.INPUT[1];
                    flag = isPress
                        ? Mouse.OperateFlag.MOUSEEVENTF_XDOWN
                        : Mouse.OperateFlag.MOUSEEVENTF_XUP;
                    data = (int)Mouse.DataXButton.XBUTTON2;
                    SetMouseData(0, 0, data, flag, ref mouseInputData[0]);
                    break;
                case MouseKeys.Wheel:
                    // マウスホイールを前後方向に移動
                    if (isPress)
                    {
                        // キー押下の場合
                        mouseInputData = new Mouse.INPUT[1];
                        flag = Mouse.OperateFlag.MOUSEEVENTF_WHEEL;
                        data = wheelAmount;
                        SetMouseData(0, 0, data, flag, ref mouseInputData[0]);
                    }
                    else
                    {
                        // キーを離す場合
                        // ホイールは押下処理のみ生成する
                        mouseInputData = new Mouse.INPUT[0];
                    }

                    break;
                case MouseKeys.HWheel:
                    // マウスホイールを左右方向に移動
                    if (isPress)
                    {
                        // キー押下の場合
                        mouseInputData = new Mouse.INPUT[1];
                        flag = Mouse.OperateFlag.MOUSEEVENTF_HWHEEL;
                        data = wheelAmount;
                        SetMouseData(0, 0, data, flag, ref mouseInputData[0]);
                    }
                    else
                    {
                        // キーを離す場合
                        // ホイールは押下処理のみ生成する
                        mouseInputData = new Mouse.INPUT[0];
                    }

                    break;
                default:
                    // 上記以外
                    // 不正なデータのため要素0の配列を設定する
                    mouseInputData = new Mouse.INPUT[0];
                    break;
            }

            // 生成したマウスの操作情報を返却する
            return mouseInputData;
        }

        /// <summary>
        /// マウスの操作情報を設定する
        /// </summary>
        /// <param name="x">設定するマウスの座標データ_X</param>
        /// <param name="y">設定するマウスの座標データ_Y</param>
        /// <param name="data">ホイール及びXボタンに関する場合に設定するデータ</param>
        /// <param name="setOperateFlag">設定するマウス操作</param>
        /// <param name="mouseInput">設定対象のマウス操作情報の構造体</param>
        private static void SetMouseData(
            int x, int y, int data, Mouse.OperateFlag setOperateFlag, ref Mouse.INPUT mouseInput)
        {
            mouseInput.InputType = Mouse.InputType;
            mouseInput.Mouse.X = x;
            mouseInput.Mouse.Y = y;
            mouseInput.Mouse.Data = data;
            mouseInput.Mouse.OperateFlag = (int)setOperateFlag;
            mouseInput.Mouse.Time = 0;
            mouseInput.Mouse.ExtraInfo = IntPtr.Zero;
        }

        #endregion

        #region キーボード操作

        /// <summary>
        /// 引数のキーボード操作の内容をキーボード操作情報の構造体に設定し返却する
        /// </summary>
        /// <param name="key">
        /// 押下するキー
        /// </param>
        /// <param name="isShift">
        /// Shiftキーを押している場合：True、押していない場合：False
        /// </param>
        /// <param name="isCtrl">
        /// Ctrlキーを押している場合：True、押していない場合：False
        /// </param>
        /// <param name="isAlt">
        /// Altキーを押している場合：True、押していない場合：False
        /// </param>
        /// <param name="isWindowsLogo">
        /// Windowsロゴキーを押している場合：True、押していない場合：False
        /// </param>
        /// <param name="isKeepPressing">
        /// キーを押しっぱなしにする場合：True、押しっぱなしにしない場合：False
        /// </param>
        /// <returns>引数のキーボード操作の内容を設定したキーボード操作情報の構造体の配列</returns>
        private static Keyboard.INPUT[] GetKeybordInputData(
            VirtualKey.Keys key,
            bool isShift,
            bool isCtrl,
            bool isAlt,
            bool isWindowsLogo,
            bool isKeepPressing)
        {
            // 引数のキーが押下するキーか判定
            bool isPressKey = key != VirtualKey.Keys.VK_NONE;

            // 引数の押下するキーが、「Shift」「Ctrl」「Alt」「Windowsロゴ」キーである場合
            // 対象のキーのフラグをOFFにする
            if (IsShiftKey(key))
            {
                // Shiftキー
                isShift = false;
            }
            else if (IsCtrlKey(key))
            {
                // Ctrlキー
                isCtrl = false;
            }
            else if (IsAltKey(key))
            {
                // Altキー
                isAlt = false;
            }
            else if (IsWinKey(key))
            {
                // Windowsロゴキー
                isWindowsLogo = false;
            }

            // 押下するキーのカウントを取る
            int count = 0;
            count = isPressKey ? ++count : count;
            count = isShift ? ++count : count;
            count = isCtrl ? ++count : count;
            count = isAlt ? ++count : count;
            count = isWindowsLogo ? ++count : count;

            // キーボード操作の情報を設定する構造体を生成
            // １つのキーに対して押す離すの動作が必要な場合はカウントの2倍のデータ量とする
            // （押しっぱなしにする場合は、話す動作がないためカウントのデータ量とする）
            Keyboard.INPUT[] input = new Keyboard.INPUT[isKeepPressing ? count : count * 2];

            // 設定対象行を示すインデックス
            int index = 0;

            // Shiftキーを押す操作の設定
            if (isShift)
            {
                SetKeyboardInputData(VirtualKey.Keys.VK_SHIFT, true, ref input[index]);
                index++;
            }

            // Ctrlキーを押す操作の設定
            if (isCtrl)
            {
                SetKeyboardInputData(VirtualKey.Keys.VK_CONTROL, true, ref input[index]);
                index++;
            }

            // Altキーを押す操作の設定
            if (isAlt)
            {
                SetKeyboardInputData(VirtualKey.Keys.VK_MENU, true, ref input[index]);
                index++;
            }

            // Windowsロゴキーを押す操作の設定
            if (isWindowsLogo)
            {
                SetKeyboardInputData(VirtualKey.Keys.VK_LWIN, true, ref input[index]);
                index++;
            }

            // 引数の押下したキーを押す操作の設定
            if (isPressKey)
            {
                SetKeyboardInputData(key, true, ref input[index]);
                index++;
            }

            // 押しっぱなしにしない場合にキーを話す動作を行う
            if (!isKeepPressing)
            {
                // 引数の押下したキーを離す操作の設定
                if (isPressKey)
                {
                    SetKeyboardInputData(key, false, ref input[index]);
                    index++;
                }

                // Windowsロゴキーを離す操作の設定
                if (isWindowsLogo)
                {
                    SetKeyboardInputData(VirtualKey.Keys.VK_LWIN, false, ref input[index]);
                    index++;
                }

                // Altキーを離す操作の設定
                if (isAlt)
                {
                    SetKeyboardInputData(VirtualKey.Keys.VK_MENU, false, ref input[index]);
                    index++;
                }

                // Ctrlキーを離す操作の設定
                if (isCtrl)
                {
                    SetKeyboardInputData(VirtualKey.Keys.VK_CONTROL, false, ref input[index]);
                    index++;
                }

                // Shiftキーを離す操作の設定
                if (isShift)
                {
                    SetKeyboardInputData(VirtualKey.Keys.VK_SHIFT, false, ref input[index]);
                }
            }
            else
            {
                // 押しっぱなしの場合、現在の押下キーをハッシュに保持する
                for (int i = 0; i < input.Length; i++)
                {
                    KeepingKeys.Add((VirtualKey.Keys)input[i].Keyboard.VirtualKeyCode);
                }
            }

            // 設定したキーボード操作情報の構造体の配列を返却する
            return input;
        }

        /// <summary>
        /// 押下するキーがShiftキーか判定する
        /// </summary>
        /// <param name="key">押下するキー</param>
        /// <returns>Shiftキーの場合：True、Shiftキーでない場合：False</returns>
        private static bool IsShiftKey(VirtualKey.Keys key)
        {
            return key == VirtualKey.Keys.VK_SHIFT
                || key == VirtualKey.Keys.VK_LSHIFT
                || key == VirtualKey.Keys.VK_RSHIFT;
        }

        /// <summary>
        /// 押下するキーがCtrlキーか判定する
        /// </summary>
        /// <param name="key">押下するキー</param>
        /// <returns>Ctrlキーの場合：True、Ctrlキーでない場合：False</returns>
        private static bool IsCtrlKey(VirtualKey.Keys key)
        {
            // 引数の押下するキーが、「Shift」「Ctrl」「Alt」「Windowsロゴ」キーである場合
            return key == VirtualKey.Keys.VK_CONTROL
                || key == VirtualKey.Keys.VK_LCONTROL
                || key == VirtualKey.Keys.VK_RCONTROL;
        }

        /// <summary>
        /// 押下するキーがAltキーか判定する
        /// </summary>
        /// <param name="key">押下するキー</param>
        /// <returns>Altキーの場合：True、Altキーでない場合：False</returns>
        private static bool IsAltKey(VirtualKey.Keys key)
        {
            return key == VirtualKey.Keys.VK_MENU
                || key == VirtualKey.Keys.VK_LMENU
                || key == VirtualKey.Keys.VK_RMENU;
        }

        /// <summary>
        /// 押下するキーがWinキーか判定する
        /// </summary>
        /// <param name="key">押下するキー</param>
        /// <returns>Winキーの場合：True、Winキーでない場合：False</returns>
        private static bool IsWinKey(VirtualKey.Keys key)
        {
            return key == VirtualKey.Keys.VK_LWIN
                || key == VirtualKey.Keys.VK_RWIN;
        }

        /// <summary>
        /// キーボード操作の情報を設定する
        /// </summary>
        /// <param name="inputKey">対象とするキー</param>
        /// <param name="isPress">押下する場合：True、離す場合：False</param>
        /// <param name="keyboardInput">設定対象のキーボード操作情報の構造体</param>
        private static void SetKeyboardInputData(
            VirtualKey.Keys inputKey, bool isPress, ref Keyboard.INPUT keyboardInput)
        {
            keyboardInput.InputType = Keyboard.InputType;
            keyboardInput.Keyboard.VirtualKeyCode = (short)inputKey;
            keyboardInput.Keyboard.ScanCode = 0;
            keyboardInput.Keyboard.OperateFlag = isPress ? 0 : (int)Keyboard.OperateFlag.KEYEVENTF_KEYUP;
            keyboardInput.Keyboard.Time = 0;
            keyboardInput.Keyboard.ExtraInfo = IntPtr.Zero;
        }

        #endregion

        #endregion
    }
}
