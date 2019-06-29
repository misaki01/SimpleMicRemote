namespace MisaCommon.Modules
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.Utility.Win32Api;

    /// <summary>
    /// プロセスの起動と起動したプロセスの情報を保持するクラス
    /// </summary>
    public static class StartProcess
    {
        #region クラス変数・定数

        /// <summary>
        /// プロセス起動後、処理を待つデフォルトの間隔（ミリ秒）
        /// 値を小さくしすぎると起動するまえに検出処理が実行されてしまうため。
        /// 起動したプロセスのプロセスID、ウィンドウハンドル等の情報が取得できない
        /// 値を大きくしすぎると他の操作で起動したプロセスを誤って検出してしまう
        /// ちょうどいい値にすることが必要
        /// </summary>
        public const int DefaultWaitDelay = 500;

        /// <summary>
        /// 起動したプロセスの情報リストへの書き込み処理の排他制御を行うロックオブジェクト
        /// </summary>
        private static readonly object LockStartProcessListObject = new object();

        /// <summary>
        /// 新しい起動した順の番号IDの取得処理の排他制御を行うロックオブジェクト
        /// </summary>
        private static readonly object LockGetNewStartNumId = new object();

        /// <summary>
        /// プロセス起動後、処理を待つデフォルトの間隔（ミリ秒）
        /// </summary>
        private static int? waitDelay = null;

        /// <summary>
        /// このクラスで起動したプロセスの情報リスト
        /// （シングルトンパターンで実装）
        /// </summary>
        private static IList<ProcessInfo> processInfoList = null;

        /// <summary>
        /// このクラスでプロセスを起動した回数のカウント
        /// （起動の指示のタイミングと起動したプロセス情報の関連をつけるIDの生成に使用する）
        /// </summary>
        private static long startCount = 0;

        #endregion

        #region Staticの公開プロパティ

        /// <summary>
        /// 共通で使用するプロセス起動後、処理を待つの間隔（ミリ秒）の取得・設定する
        /// 値を小さくしすぎると起動するまえに検出処理が実行されてしまうため。
        /// 起動したプロセスのプロセスID、ウィンドウハンドル等の情報が取得できない
        /// 値を大きくしすぎると他の操作で起動したプロセスを誤って検出してしまう
        /// ちょうどいい値にすることが必要
        /// </summary>
        public static int WaitDelay
        {
            get => waitDelay ?? DefaultWaitDelay;
            set => waitDelay = value;
        }

        /// <summary>
        /// このクラスで起動したプロセスの情報リストを取得する
        /// </summary>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        public static IReadOnlyList<ProcessInfo> StartProcessList
        {
            get
            {
                // 停止・閉じたプロセスの情報をリストから削除する
                // （排他制御を行い、処理中に他の処理でプロセス情報が操作されないようにする）
                lock (LockStartProcessListObject)
                {
                    RemoveStopProcess(ProcessInfoList);
                }

                // このクラスで起動したプロセスの情報をコピー
                List<ProcessInfo> copyList = new List<ProcessInfo>();
                foreach (ProcessInfo info in ProcessInfoList)
                {
                    copyList.Add(info.DeepCopy());
                }

                // コピーしたリストを読み取り専用にして返却
                return copyList.AsReadOnly();
            }
        }

        /// <summary>
        /// 現在実行中のプロセス情報（プロセスID、プロセス名）のリストを取得する
        /// キー：プロセスID、値：プロセス名
        /// </summary>
        public static IDictionary<int, string> NowProcessList => GetNowProcessList();

        #endregion

        #region プライベートプロパティ

        /// <summary>
        /// このクラスで起動したプロセスの情報リストを取得する
        /// </summary>
        /// <remarks>
        /// シングルトンパターンで実装
        /// </remarks>
        private static IList<ProcessInfo> ProcessInfoList
        {
            get
            {
                // シングルトンの処理、NULLならオブジェクトを生成する
                if (processInfoList == null)
                {
                    processInfoList = new List<ProcessInfo>();
                }

                // このクラスで起動したウインドウ情報を返却
                return processInfoList;
            }
        }

        #endregion

        #region メソッド

        #region プロセスの起動

        /// <summary>
        /// 引数（<paramref name="processPath"/>）で指定されたパスのファイルを起動する
        /// （起動したプロセスの情報を保持せずに単純に起動する）
        /// </summary>
        /// <param name="processPath">
        /// 起動するexeファイルのパス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="processPath"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// 指定されたファイルが存在しない、又は PATH 環境変数に引用符を含む文字列があり、
        /// 実行ファイルをできない場合に発生
        /// </exception>
        public static void SimpleStart(string processPath)
        {
            SimpleStart(processPath, null);
        }

        /// <summary>
        /// 引数（<paramref name="processPath"/>）で指定されたパスのファイルを起動する
        /// （起動したプロセスの情報を保持せずに単純に起動する）
        /// </summary>
        /// <param name="processPath">
        /// 起動するexeファイルのパス
        /// </param>
        /// <param name="startupParam">
        /// 起動パラメータ
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="processPath"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// 指定されたファイルが存在しない、又は PATH 環境変数に引用符を含む文字列があり、
        /// 実行ファイルをできない場合に発生
        /// </exception>
        public static void SimpleStart(string processPath, string startupParam)
        {
            // NULLチェック
            if (processPath == null)
            {
                throw new ArgumentNullException(nameof(processPath));
            }

            // 対象のプロセスを実行
            Process.Start(processPath, startupParam ?? string.Empty);
        }

        /// <summary>
        /// 引数（<paramref name="startProcessInfo"/>）で指定されたパスのファイルを起動する
        /// （プロパティに設定された待ち時間を使用する）
        /// </summary>
        /// <param name="startProcessInfo">
        /// プロセスの実行に関する情報
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="startProcessInfo"/> の
        /// <see cref="StartProcessInfo.ProcessPath"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// 指定されたファイルが存在しない、又は PATH 環境変数に引用符を含む文字列があり、
        /// 実行ファイルをできない場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        /// <returns>
        /// 起動した順の番号IDを返却
        /// 起動したプロセスが存在しない場合はNULLを返却
        /// </returns>
        public static long? Start(StartProcessInfo startProcessInfo)
        {
            // NULLチェック
            if (startProcessInfo == null)
            {
                throw new ArgumentNullException(nameof(startProcessInfo));
            }

            return Start(
                processPath: startProcessInfo.ProcessPath,
                startupParam: startProcessInfo.StartupParam,
                sizePoint: startProcessInfo.SizePoint,
                waitDelay: startProcessInfo.WaitDelay ?? WaitDelay);
        }

        /// <summary>
        /// 引数（<paramref name="processPath"/>）で指定されたパスのファイルを起動する
        /// （プロパティに設定された待ち時間を使用する）
        /// </summary>
        /// <param name="processPath">
        /// 起動するexeファイルのパス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="processPath"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// 指定されたファイルが存在しない、又は PATH 環境変数に引用符を含む文字列があり、
        /// 実行ファイルをできない場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        /// <returns>
        /// 起動した順の番号IDを返却
        /// 起動したプロセスが存在しない場合はNULLを返却
        /// </returns>
        public static long? Start(string processPath)
        {
            return Start(processPath, null, null, WaitDelay);
        }

        /// <summary>
        /// 引数（<paramref name="processPath"/>）で指定されたパスのファイルを起動する
        /// （プロパティに設定された待ち時間を使用する）
        /// </summary>
        /// <param name="processPath">
        /// 起動するexeファイルのパス
        /// </param>
        /// <param name="startupParam">
        /// 起動パラメータ
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="processPath"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// 指定されたファイルが存在しない、又は PATH 環境変数に引用符を含む文字列があり、
        /// 実行ファイルをできない場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        /// <returns>
        /// 起動した順の番号IDを返却
        /// 起動したプロセスが存在しない場合はNULLを返却
        /// </returns>
        public static long? Start(string processPath, string startupParam)
        {
            return Start(processPath, startupParam, null, WaitDelay);
        }

        /// <summary>
        /// 引数（<paramref name="processPath"/>）で指定されたパスのファイルを起動する
        /// （プロパティに設定された待ち時間を使用する）
        /// </summary>
        /// <param name="processPath">
        /// 起動するexeファイルのパス
        /// </param>
        /// <param name="startupParam">
        /// 起動パラメータ
        /// </param>
        /// <param name="sizePoint">
        /// 起動したプロセスのウィンドウのサイズと位置
        /// NULLを指定した場合はサイズと位置は起動した各プロセスに依存したデフォルト値となる
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="processPath"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// 指定されたファイルが存在しない、又は PATH 環境変数に引用符を含む文字列があり、
        /// 実行ファイルをできない場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        /// <returns>
        /// 起動した順の番号IDを返却
        /// 起動したプロセスが存在しない場合はNULLを返却
        /// </returns>
        public static long? Start(string processPath, string startupParam, SizePoint sizePoint)
        {
            return Start(processPath, startupParam, sizePoint, WaitDelay);
        }

        /// <summary>
        /// 引数（<paramref name="processPath"/>）で指定されたパスのファイルを起動する
        /// </summary>
        /// <param name="processPath">
        /// 起動するexeファイルのパス
        /// </param>
        /// <param name="startupParam">
        /// 起動パラメータ
        /// </param>
        /// <param name="sizePoint">
        /// 起動したプロセスのウィンドウのサイズと位置
        /// NULLを指定した場合はサイズと位置は起動した各プロセスに依存したデフォルト値となる
        /// </param>
        /// <param name="waitDelay">
        /// プロセス起動後、処理を待つ間隔（ミリ秒）
        /// 値を小さくしすぎると起動したプロセスのプロセスID、ウィンドウハンドル等の情報が取得できない
        /// 値を大きくしすぎると他の操作で起動したプロセスを誤って検出してしまう
        /// 起動が遅いプロセスについてのみ、大きい値を設定すること
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="processPath"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// 下記の要因で。指定ファイルを開いているときにエラーが発生した場合に発生
        /// ・指定ファイルへの完全パスの長さと起動パラメータの長さの合計が、2080 文字を超えている場合
        /// ・指定ファイルへのアクセスが拒否された場合
        /// </exception>
        /// <exception cref="FileNotFoundException">
        /// 指定されたファイルが存在しない、又は PATH 環境変数に引用符を含む文字列があり、
        /// 実行ファイルをできない場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        /// <returns>
        /// 起動した順の番号IDを返却
        /// 起動したプロセスが存在しない場合はNULLを返却
        /// </returns>
        public static long? Start(
            string processPath, string startupParam, SizePoint sizePoint, int waitDelay)
        {
            // NULLチェック
            if (processPath == null)
            {
                throw new ArgumentNullException(nameof(processPath));
            }

            // 起動した順の番号ID取得
            long startNumId = GetNewStartNumId();

            // 起動前のウィンドウリスト、プロセスリストを取得する
            IDictionary<int, string> beforeProcessList = NowProcessList;
            ICollection<IntPtr> beforeHandleList = WindowOperate.WindowHandleList();

            // 対象のプロセスを実行
            string processName;
            using (Process process = Process.Start(processPath, startupParam ?? string.Empty))
            {
                // フォルダを指定して起動した場合はプロセスがNULLとなる
                // その場合はNULLを返却する
                if (process == null)
                {
                    return null;
                }

                // プロセスが存在する場合はプロセス名を取得
                processName = process.ProcessName;
            }

            // 引数で指定された待ち時間分、プロセスの起動を待つ
            Thread.Sleep(waitDelay < 0 ? 0 : waitDelay);

            // 起動後のウィンドウリスト、プロセスリストを取得する
            ICollection<IntPtr> afterHandleList = WindowOperate.WindowHandleList();
            IDictionary<int, string> afterProcessList = NowProcessList;

            // 起動したプロセスの情報を取得
            IList<ProcessInfo> processInfo
                = GetStartProcessInfo(
                    startNumId: startNumId,
                    processName: processName ?? string.Empty,
                    beforeProcessList: beforeProcessList,
                    beforeHandleList: beforeHandleList,
                    afterProcessList: afterProcessList,
                    afterHandleList: afterHandleList);

            // プロセス情報が取得できた場合は、起動情報に追加する
            // （排他制御を行い、処理中に他の処理でプロセス情報が操作されないようにする）
            if (processInfo.Count > 0)
            {
                lock (LockStartProcessListObject)
                {
                    // 起動したプロセスの情報を追加する
                    foreach (ProcessInfo info in processInfo)
                    {
                        ProcessInfoList.Add(info);
                    }

                    // 停止しているプロセスの情報を削除する
                    RemoveStopProcess(ProcessInfoList);
                }
            }

            // ウィンドウが存在するプロセスの場合、サイズと位置を引数の値に変更する
            if (sizePoint != null)
            {
                // 起動キーから対象のプロセスリストを取得しループする
                IReadOnlyList<ProcessInfo> processInfoList = StartProcessList;
                foreach (ProcessInfo info in processInfoList.Where((x) => x.StartNumId == startNumId))
                {
                    // ウィンドウ情報を持っているもののみ処理を行う
                    if (info.HasWindowInfo)
                    {
                        WindowOperate.SetWindowSizeLocation(info.WindowInfo.WindowHandle, sizePoint);
                    }
                }
            }

            // 起動した順の番号IDを返却
            return startNumId;
        }

        #endregion

        #region プロセスの終了

        /// <summary>
        /// 引数（<paramref name="startNumId"/>）に該当するウィンドウを閉じる
        /// </summary>
        /// <param name="startNumId">起動した順の番号ID</param>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        public static void CloseWindow(long startNumId)
        {
            // 起動した順の番号IDに該当する起動中のプロセス情報を取得する
            IList<ProcessInfo> processInfoList
                = ProcessInfoList.Where((x) => x.StartNumId == startNumId).ToList();

            // ウィンドウを閉じる処理を実行
            CloseWindow(processInfoList);
        }

        /// <summary>
        /// 引数（<paramref name="processInfo"/>）に該当するウィンドウを閉じる
        /// </summary>
        /// <param name="processInfo">閉じるウィンドウに関するプロセス情報</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="processInfo"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        public static void CloseWindow(ProcessInfo processInfo)
        {
            // NULLチェック
            if (processInfo == null)
            {
                throw new ArgumentNullException(nameof(processInfo));
            }

            // プロセス情報のリストを生成し、引数のプロセス情報を設定する
            IList<ProcessInfo> processInfoList = new List<ProcessInfo>
            {
                processInfo,
            };

            // ウィンドウを閉じる処理を実行
            CloseWindow(processInfoList);
        }

        /// <summary>
        /// 引数（<paramref name="processInfoList"/>）に該当するウィンドウを閉じる
        /// </summary>
        /// <param name="processInfoList">閉じるウィンドウに関するプロセス情報のリスト</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="processInfoList"/> がNULLの場合に発生
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        public static void CloseWindow(IList<ProcessInfo> processInfoList)
        {
            // NULLチェック
            if (processInfoList == null)
            {
                throw new ArgumentNullException(nameof(processInfoList));
            }

            // 削除対象行を一時的に保持するリストを生成
            IList<ProcessInfo> removeList = new List<ProcessInfo>();

            // 引数の閉じる対象のリスト分ループする
            foreach (ProcessInfo info in processInfoList)
            {
                // ウィンドウ情報が存在しない行は無視する
                if (info.HasWindowInfo)
                {
                    // ウィンドウを閉じる
                    WindowOperate.CloseWindow(info.WindowInfo.WindowHandle);

                    // 閉じたプロセス情報を削除対象として保持する
                    removeList.Add(info);
                }
            }

            // 起動中のプロセス情報のリストから閉じたウィンドウのプロセス情報を削除する
            // （排他制御を行い、処理中に他の処理で起動情報が操作されないようにする）
            lock (LockStartProcessListObject)
            {
                // 削除対象のリスト分ループする
                foreach (ProcessInfo info in removeList)
                {
                    // 閉じたウィンドウのプロセス情報をリストから削除する
                    ProcessInfoList.Remove(info);
                }

                // 停止しているプロセスの情報を削除する
                RemoveStopProcess(ProcessInfoList);
            }
        }

        #endregion

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 新しい起動した順の番号IDを取得する
        /// （起動の指示のタイミングと起動したプロセス情報の関連をつけるIDとして使用する）
        /// </summary>
        /// <returns>新しく生成した起動した順の番号ID</returns>
        private static long GetNewStartNumId()
        {
            // 排他制御を行いインクリメント処理が同時に発生しないよう制御する
            long newId;
            lock (LockGetNewStartNumId)
            {
                newId = ++startCount;
            }

            // 生成したIDを返却
            return newId;
        }

        /// <summary>
        /// 現在実行中のプロセス情報（プロセスID、プロセス名）のリストを取得する
        /// </summary>
        /// <returns>
        /// 現在実行中のプロセス情報（プロセスID、プロセス名）のリスト
        /// キー：プロセスID、値：プロセス名
        /// </returns>
        private static IDictionary<int, string> GetNowProcessList()
        {
            Process[] processes = null;
            try
            {
                // 現在実行中のプロセスを取得
                processes = Process.GetProcesses();

                // 戻り値のプロセス情報を格納するリストを生成
                IDictionary<int, string> processList = new Dictionary<int, string>();

                // プロセスからプロセスIDとプロセス名を取得しリストに設定していく
                foreach (Process process in processes)
                {
                    processList.Add(process.Id, process.ProcessName ?? string.Empty);
                }

                // 値を設定したプロセス情報を返却
                return processList;
            }
            finally
            {
                // プロセスのリストのDispose
                if (processes != null)
                {
                    for (int i = 0; i < processes.Length; i++)
                    {
                        processes[i]?.Dispose();
                    }

                    Array.Clear(processes, 0, processes.Length);
                }
            }
        }

        /// <summary>
        /// 前後のリストを比較して、起動したプロセスのウィンドウ情報を取得する
        /// </summary>
        /// <param name="startNumId">
        /// 起動した順の番号ID
        /// </param>
        /// <param name="processName">
        /// 起動したプロセスの名称
        /// </param>
        /// <param name="beforeProcessList">
        /// プロセス起動前のプロセス情報リスト
        /// </param>
        /// <param name="beforeHandleList">
        /// プロセス起動前のウィンドウハンドルリスト
        /// </param>
        /// <param name="afterProcessList">
        /// プロセス起動後のプロセス情報リスト
        /// </param>
        /// <param name="afterHandleList">
        /// プロセス起動後のウィンドウハンドルリスト
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 下記の引数がNULLの場合に発生
        /// ・プロセス名（<paramref name="processName"/>）
        /// ・プロセス起動前のプロセス情報リスト（<paramref name="beforeProcessList"/>）
        /// ・プロセス起動前のウィンドウハンドルリスト（<paramref name="beforeHandleList"/>）
        /// ・プロセス起動後のプロセス情報リスト（<paramref name="afterProcessList"/>）
        /// ・プロセス起動後のウィンドウハンドルリスト（<paramref name="afterHandleList"/>）
        /// </exception>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        /// <returns>起動したプロセスのウィンドウ情報</returns>
        private static IList<ProcessInfo> GetStartProcessInfo(
            long startNumId,
            string processName,
            IDictionary<int, string> beforeProcessList,
            ICollection<IntPtr> beforeHandleList,
            IDictionary<int, string> afterProcessList,
            ICollection<IntPtr> afterHandleList)
        {
            // NULLチェック
            if (processName == null)
            {
                throw new ArgumentNullException(nameof(processName));
            }
            else if (beforeProcessList == null)
            {
                throw new ArgumentNullException(nameof(beforeProcessList));
            }
            else if (beforeHandleList == null)
            {
                throw new ArgumentNullException(nameof(beforeHandleList));
            }
            else if (afterProcessList == null)
            {
                throw new ArgumentNullException(nameof(afterProcessList));
            }
            else if (afterHandleList == null)
            {
                throw new ArgumentNullException(nameof(afterHandleList));
            }

            // 起動後に増えたウィンドウハンドル、プロセスリストを取得
            IEnumerable<IntPtr> handleList
                = afterHandleList.Where((x) => !beforeHandleList.Contains(x));
            IDictionary<int, string> processList
                = afterProcessList.Where((x) => !beforeProcessList.ContainsKey(x.Key))
                    .ToDictionary(x => x.Key, x => x.Value);

            // ウィンドウハンドルからウィンドウ情報を取得する
            IList<WindowInfo> windowInfoList = new List<WindowInfo>();
            foreach (IntPtr handle in handleList)
            {
                // ウィンドウが存在しないデータは無視する
                if (!WindowOperate.IsWindow(new HandleRef(0, handle)))
                {
                    continue;
                }

                // ウィンドウ情報を取得
                WindowInfo info = WindowOperate.GetWindowThreadProcessId(new HandleRef(0, handle));

                // 起動したプロセスのIDかどうか判定
                if (processList.ContainsKey(info.ProcessId))
                {
                    // 増えたプロセスIDのリストに対象のウィンドウハンドルに紐づくプロセスIDが存在する場合、
                    // そのリストからプロセス名を取得し設定する
                    info.ProcessName = processList[info.ProcessId];
                    windowInfoList.Add(info);
                }
                else if (afterProcessList.ContainsKey(info.ProcessId)
                         && afterProcessList[info.ProcessId].Equals(processName, StringComparison.Ordinal))
                {
                    // 増えたプロセスIDのリストに対象のウィンドウハンドルに紐づくプロセスIDが存在しない場合、
                    // 起動中の全プロセスからウィンドウハンドルに紐づくプロセスIDと、
                    // 起動したプロセス名に紐づくデータが存在するか存在するかチェックしプロセス名を設定する
                    info.ProcessName = afterProcessList[info.ProcessId];
                    windowInfoList.Add(info);
                }
            }

            // ウィンドウ情報の有無を判定する
            IList<ProcessInfo> processInfoList = new List<ProcessInfo>();
            if (windowInfoList.Count > 0)
            {
                // ウィンドウ情報が存在する場合、
                // その情報からプロセス情報を生成する
                foreach (WindowInfo info in windowInfoList)
                {
                    processInfoList.Add(new ProcessInfo(
                        startNumId: startNumId,
                        processId: info.ProcessId,
                        processName: info.ProcessName,
                        windowInfo: info));
                }
            }
            else
            {
                // ウィンドウ情報が存在しない場合、
                // 増えたプロセスをプロセス情報として生成する
                foreach (KeyValuePair<int, string> info in processList)
                {
                    if (info.Value.Equals(processName, StringComparison.Ordinal))
                    {
                        processInfoList.Add(new ProcessInfo(
                            startNumId: startNumId,
                            processId: info.Key,
                            processName: info.Value));
                    }
                }
            }

            // 生成したプロセス情報を返却
            return processInfoList;
        }

        /// <summary>
        /// 停止・閉じたプロセスの情報をリストから削除する
        /// </summary>
        /// <param name="processInfoList">削除処理を行う対象のリスト</param>
        /// <exception cref="PlatformInvokeException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// <see cref="WindowOperate"/> クラスのWin32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        private static void RemoveStopProcess(IList<ProcessInfo> processInfoList)
        {
            // 引数のリストがNULLの場合処理を抜ける
            if (processInfoList == null)
            {
                return;
            }

            // 現在実行中のプロセスリスト取得
            IDictionary<int, string> nowProcessList = NowProcessList;

            // 削除対象の要素を取得する
            IList<ProcessInfo> removeList = new List<ProcessInfo>();
            foreach (ProcessInfo info in processInfoList)
            {
                // 当該行が削除対象かのフラグ
                bool isRemove = false;

                // ウィンドウ情報が存在するか判定
                if (info.HasWindowInfo)
                {
                    // ウィンドウ情報が存在しウィンドウが存在しない場合は削除対象とする
                    if (!WindowOperate.IsWindow(info.WindowInfo.WindowHandle))
                    {
                        isRemove = true;
                    }
                }
                else
                {
                    // ウィンドウ情報が存在しない場合、
                    // プロセスIDが現在実行中のプロセスリストに存在しない場合は削除対象とする
                    if (!nowProcessList.ContainsKey(info.ProcessId))
                    {
                        isRemove = true;
                    }
                }

                // 削除フラグが立っている場合、削除対象リストに追加する
                if (isRemove)
                {
                    removeList.Add(info);
                }
            }

            // 引数のリストから削除対象とマークされたものを削除する
            foreach (ProcessInfo removeItem in removeList)
            {
                processInfoList.Remove(removeItem);
            }
        }

        #endregion
    }
}
