namespace MisaCommon.CustomType
{
    using System;

    /// <summary>
    /// ウィンドウに関する情報をまとめて扱うクラス
    /// </summary>
    public class WindowInfo
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数でプロパティを初期化する
        /// </summary>
        /// <param name="windowHandle">ウインドウのハンドル</param>
        /// <param name="threadId">スレッドID</param>
        /// <param name="processId">プロセスID</param>
        /// <param name="processName">プロセス名</param>
        public WindowInfo(IntPtr windowHandle, int threadId, int processId, string processName)
        {
            WindowHandle = windowHandle;
            ThreadId = threadId;
            ProcessId = processId;
            ProcessName = processName;
        }

        /// <summary>
        /// コンストラクタ
        /// 引数でプロパティを初期化する
        /// </summary>
        /// <param name="windowHandle">ウインドウのハンドル</param>
        /// <param name="threadId">スレッドID</param>
        /// <param name="processId">プロセスID</param>
        public WindowInfo(IntPtr windowHandle, int threadId, int processId)
        {
            WindowHandle = windowHandle;
            ThreadId = threadId;
            ProcessId = processId;
            ProcessName = null;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ウインドウのハンドルを取得・設定する
        /// </summary>
        public IntPtr WindowHandle { get; set; }

        /// <summary>
        /// スレッドIDを取得・設定する
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// プロセスIDを取得・設定する
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// プロセス名を取得・設定する
        /// </summary>
        public string ProcessName { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public WindowInfo DeepCopy()
        {
            return new WindowInfo(
                windowHandle: WindowHandle,
                threadId: ThreadId,
                processId: ProcessId,
                processName: ProcessName);
        }

        #endregion
    }
}
