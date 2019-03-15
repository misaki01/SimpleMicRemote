namespace MisaCommon.CustomType
{
    /// <summary>
    /// プロセスの情報に対するクラス
    /// </summary>
    public class ProcessInfo
    {
        #region クラス変数・定数

        /// <summary>
        /// 起動したプロセスに関するウィンドウ情報
        /// </summary>
        private WindowInfo _windowInfo;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="startNumId">
        /// 起動した順の番号ID
        /// （起動の指示のタイミングと起動したプロセス情報の関連をつけるIDとして使用する）
        /// </param>
        /// <param name="processId">
        /// 起動したプロセスのプロセスID
        /// </param>
        /// <param name="processName">
        /// 起動したプロセスのプロセス名
        /// </param>
        /// <param name="windowInfo">
        /// 起動したプロセスに関するウィンドウ情報
        /// </param>
        public ProcessInfo(
            long startNumId,
            int processId,
            string processName,
            WindowInfo windowInfo)
        {
            // 各プロパティに引数の値を設定
            StartNumId = startNumId;
            ProcessId = processId;
            ProcessName = processName;
            WindowInfo = windowInfo;
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="startNumId">
        /// 起動した順の番号ID
        /// （起動の指示のタイミングと起動したプロセス情報の関連をつけるIDとして使用する）
        /// </param>
        /// <param name="processId">
        /// 起動したプロセスのプロセスID
        /// </param>
        /// <param name="processName">
        /// 起動したプロセスのプロセス名
        /// </param>
        public ProcessInfo(
            long startNumId,
            int processId,
            string processName)
            : this(startNumId, processId, processName, null)
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 起動した順の番号IDを取得・設定する
        /// （起動の指示のタイミングと起動したプロセス情報の関連をつけるIDとして使用する）
        /// </summary>
        public long StartNumId { get; set; }

        /// <summary>
        /// 起動したプロセスのプロセスIDを取得・設定する
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// 起動したプロセスのプロセス名を取得・設定する
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// ウィンドウ情報が存在するかのフラグを取得・設定する
        /// </summary>
        public bool HasWindowInfo { get; private set; }

        /// <summary>
        /// 起動したプロセスに関するウィンドウ情報を取得・設定する
        /// 起動したプロセスがウィンドウを持つ場合に設定する、
        /// ウィンドウを持たないプロセスの場合はNULL
        /// </summary>
        public WindowInfo WindowInfo
        {
            get => _windowInfo;
            set
            {
                HasWindowInfo = value != null;
                _windowInfo = value;
            }
        }

        #endregion

        #region メソッド

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public ProcessInfo DeepCopy()
        {
            return new ProcessInfo(
                startNumId: StartNumId,
                processId: ProcessId,
                processName: ProcessName,
                windowInfo: WindowInfo?.DeepCopy());
        }

        #endregion
    }
}
