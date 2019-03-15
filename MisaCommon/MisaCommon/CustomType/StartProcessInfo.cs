namespace MisaCommon.CustomType
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;

    using MisaCommon.CustomType.Converter;
    using MisaCommon.MessageResources.Type;
    using MisaCommon.Modules;

    /// <summary>
    /// プロセスの実行に関する情報をまとめて扱うクラス
    /// </summary>
    /// <remarks>
    /// このクラスの公開プロパティにおける表示名と説明は、
    /// <see cref="LocalizableTypeConverter{T, TResouces}"/>にてマッピングしている
    /// <list type="bullet">
    ///     <item>
    ///         <term>string型への変換</term>
    ///         <description>
    ///         各パラメータを下記の順番でパイプ区切りの文字列で表現する
    ///         １．起動するexeファイルのパス
    ///         ２．起動したプロセスのウィンドウのサイズと位置
    ///         ３．プロセス起動後、処理を待つ間隔（ミリ秒）
    ///         ４．起動パラメータ
    ///         【例】c\xxx\xxx\abc.exe|0, 0, 1200, 800|500|--abe="xxxx"
    ///         ※省略したパラメータは空欄となる
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Serializable]
    [TypeConverter(typeof(LocalizableTypeConverter<StartProcessInfo, StartProcessInfoPropertyMessage>))]
    public class StartProcessInfo : ITypeConvertable<StartProcessInfo>
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各プロパティを初期化する
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
        /// 通常はNULLを指定する、NULLを指定した場合はデフォルトの値を使用する
        /// 値を小さくしすぎると起動したプロセスのプロセスID、ウィンドウハンドル等の情報が取得できない
        /// 値を大きくしすぎると他の操作で起動したプロセスを誤って検出してしまう
        /// 起動が遅いプロセスについてのみ、大きい値を設定すること
        /// </param>
        public StartProcessInfo(string processPath, string startupParam, SizePoint sizePoint, int? waitDelay)
        {
            ProcessPath = processPath;
            StartupParam = startupParam;
            SizePoint = sizePoint;
            WaitDelay = waitDelay;
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
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
        public StartProcessInfo(string processPath, string startupParam, SizePoint sizePoint)
            : this(processPath, startupParam, sizePoint, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="processPath">
        /// 起動するexeファイルのパス
        /// </param>
        /// <param name="startupParam">
        /// 起動パラメータ
        /// </param>
        /// <param name="waitDelay">
        /// プロセス起動後、処理を待つ間隔（ミリ秒）
        /// 通常はNULLを指定する、NULLを指定した場合はデフォルトの値を使用する
        /// 値を小さくしすぎると起動したプロセスのプロセスID、ウィンドウハンドル等の情報が取得できない
        /// 値を大きくしすぎると他の操作で起動したプロセスを誤って検出してしまう
        /// 起動が遅いプロセスについてのみ、大きい値を設定すること
        /// </param>
        public StartProcessInfo(string processPath, string startupParam, int? waitDelay)
            : this(processPath, startupParam, null, waitDelay)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="processPath">
        /// 起動するexeファイルのパス
        /// </param>
        /// <param name="startupParam">
        /// 起動パラメータ
        /// </param>
        public StartProcessInfo(string processPath, string startupParam)
            : this(processPath, startupParam, null, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        public StartProcessInfo()
            : this(null, null, null, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="data">このクラスに変換可能な文字列</param>
        public StartProcessInfo(string data)
        {
            StartProcessInfo input = ConvertFromString(data);
            ProcessPath = input.ProcessPath;
            StartupParam = input.StartupParam;
            SizePoint = input.SizePoint;
            WaitDelay = input.WaitDelay;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 起動するexeファイルのパスを取得・設定する
        /// </summary>
        [DefaultValue(null)]
        public string ProcessPath { get; set; }

        /// <summary>
        /// 起動パラメータを取得・設定する
        /// </summary>
        [DefaultValue(null)]
        public string StartupParam { get; set; }

        /// <summary>
        /// 起動したプロセスのウィンドウのサイズと位置を取得・設定する
        /// NULLを指定した場合はサイズと位置は起動した各プロセスに依存したデフォルト値となる
        /// </summary>
        [DefaultValue(null)]
        public SizePoint SizePoint { get; set; }

        /// <summary>
        /// プロセス起動後、処理を待つ間隔（ミリ秒）を取得・設定する
        /// 通常はNULLを指定する、NULLを指定した場合はデフォルトの値を使用する
        /// 値を小さくしすぎると起動したプロセスのプロセスID、ウィンドウハンドル等の情報が取得できない
        /// 値を大きくしすぎると他の操作で起動したプロセスを誤って検出してしまう
        /// 起動が遅いプロセスについてのみ、大きい値を設定すること
        /// </summary>
        [DefaultValue(StartProcess.DefaultWaitDelay)]
        public int? WaitDelay { get; set; }

        #endregion

        #region メソッド

        #region ITypeConvertableの実装

        /// <summary>
        /// 文字列を <see cref="StartProcessInfo"/> クラスのインスタンスに変換する
        /// </summary>
        /// <param name="value">
        /// <see cref="StartProcessInfo"/> クラスのインスタンスに変換する文字列
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>文字列から生成した<see cref="StartProcessInfo"/> クラスのインスタンス</returns>
        public StartProcessInfo ConvertFromString(string value)
        {
            // NULLチェック
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // パイプ区切りでSplitして各パラメータを取得する
            string[] param = value.Split('|');

            // 起動するexeファイルのパス
            string processPath = param[0].Trim();

            // 起動したプロセスのウィンドウのサイズと位置
            SizePoint sizePoint = param.Length > 1 && !string.IsNullOrWhiteSpace(param[1])
                ? new SizePoint(param[1]) : null;

            // プロセス起動後、処理を待つ間隔
            int? waitDelay = param.Length > 2
                && !string.IsNullOrWhiteSpace(param[2])
                && int.TryParse(param[2], out int tmpWaitDelay)
                ? tmpWaitDelay : (int?)null;

            // 起動パラメータ
            string startupParam = param.Length > 3 && !string.IsNullOrWhiteSpace(param[3])
                ? value.Substring(param[0].Length + param[1].Length + param[2].Length + 3)
                : null;

            // 取得した各プロパティの値から当クラスのインスタンスを生成し返却
            return new StartProcessInfo(processPath, startupParam, sizePoint, waitDelay);
        }

        /// <summary>
        /// <see cref="StartProcessInfo"/> クラスのインスタンスを文字列に変換する
        /// </summary>
        /// <param name="value">
        /// 文字列に変換する <see cref="StartProcessInfo"/> クラスのインスタンス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>変換した文字列</returns>
        public string ConvertToString(StartProcessInfo value)
        {
            // 引数の型チェック
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // このクラスにおいて、ToStringを実装しているため、
            // その機能を利用し文字列に変換する
            return value.ToString();
        }

        #endregion

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public StartProcessInfo DeepCopy()
        {
            return new StartProcessInfo(
                processPath: ProcessPath,
                startupParam: StartupParam,
                waitDelay: WaitDelay,
                sizePoint: SizePoint);
        }

        /// <summary>
        /// このインスタンスの値を <see cref="string"/> に変換する
        /// </summary>
        /// <returns>このインスタンスと同じ値の文字列</returns>
        public new string ToString()
        {
            // 文字列に変換し返却
            StringBuilder convertValue = new StringBuilder();

            // 起動するexeファイルのパス
            convertValue.Append(string.IsNullOrEmpty(ProcessPath) ? string.Empty : ProcessPath);
            convertValue.Append("|");

            // 起動したプロセスのウィンドウのサイズと位置
            convertValue.Append(SizePoint == null ? string.Empty : SizePoint.ToString());
            convertValue.Append("|");

            // プロセス起動後、処理を待つ間隔
            convertValue.Append(WaitDelay.HasValue ? WaitDelay.Value.ToString(CultureInfo.InvariantCulture) : string.Empty);
            convertValue.Append("|");

            // 起動パラメータ
            convertValue.Append(string.IsNullOrEmpty(StartupParam) ? string.Empty : StartupParam);

            return convertValue.ToString();
        }

        #endregion
    }
}
