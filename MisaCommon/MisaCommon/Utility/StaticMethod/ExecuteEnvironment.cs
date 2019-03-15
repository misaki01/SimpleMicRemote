namespace MisaCommon.Utility.StaticMethod
{
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// 実行環境に関する処理を提供する
    /// </summary>
    public static class ExecuteEnvironment
    {
        #region 実行中のアセンブリに関する情報に関する処理

        /// <summary>
        /// アセンブリの名称を取得する
        /// 取得できない場合はNULLを返却する
        /// </summary>
        public static string AssemblyName
        {
            get
            {
                // 実行中のアセンブリの名称を取得
                string assemblyName = Assembly.GetEntryAssembly()?.GetName()?.Name;

                // 実行中のアセンブリの名称が取得できない場合は、自身のアセンブリの名称を取得
                assemblyName = string.IsNullOrEmpty(assemblyName)
                    ? Assembly.GetExecutingAssembly()?.GetName()?.Name : assemblyName;

                // アセンブリの名称を返却
                return string.IsNullOrEmpty(assemblyName) ? null : assemblyName;
            }
        }

        /// <summary>
        /// アセンブリの場所（絶対パス）を取得する
        /// 取得できない場合はNULLを返却する
        /// </summary>
        public static string AssemblyLocation
        {
            get
            {
                // 実行中のアセンブリのパスを取得
                string assemblyPath = Assembly.GetEntryAssembly()?.Location;

                // 実行中のアセンブリのパスが取得できない場合は、自身のアセンブリのパスを取得
                assemblyPath = string.IsNullOrEmpty(assemblyPath)
                    ? Assembly.GetExecutingAssembly()?.Location : assemblyPath;

                // アセンブリのパスを返却
                return string.IsNullOrEmpty(assemblyPath) ? null : assemblyPath;
            }
        }

        /// <summary>
        /// アセンブリのファイル名を取得する
        /// 取得できない場合はNULLを返却する
        /// </summary>
        public static string AssemblyFileName
        {
            get
            {
                // 実行中のアセンブリパスを取得
                string assemblyPath = AssemblyLocation;

                // アセンブリパスからファイル名を取得
                string fileName = string.IsNullOrEmpty(assemblyPath)
                    ? null : Path.GetFileName(assemblyPath);

                // ファイル名を返却
                return string.IsNullOrEmpty(fileName) ? null : fileName;
            }
        }

        /// <summary>
        /// アセンブリのファイル名（拡張子なし）を取得する
        /// 取得できない場合はNULLを返却する
        /// </summary>
        public static string AssemblyFileNameWithoutExtension
        {
            get
            {
                // 実行中のアセンブリパスを取得
                string assemblyPath = AssemblyLocation;

                // アセンブリパスからファイル名を取得
                string fileName = string.IsNullOrEmpty(assemblyPath)
                    ? null : Path.GetFileNameWithoutExtension(assemblyPath);

                // ファイル名を返却
                return string.IsNullOrEmpty(fileName) ? null : fileName;
            }
        }

        #endregion
    }
}
