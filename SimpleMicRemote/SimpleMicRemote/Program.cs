namespace SimpleMicRemote
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;

    using MisaCommon.Modules;
    using MisaCommon.Utility.StaticMethod;

    /// <summary>
    /// アプリケーションのメイン エントリ ポイント
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        public static void Main()
        {
            // アプリケーションの初期設定
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // アセンブリ名を取得
            string assamblyName = ExecuteEnvironment.AssemblyName ?? "SimpleMicRemote";

            // アプリケーション起動
            CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
            EntryPoint.ApplicationStart(() => new SimpleForm(), culture, true, assamblyName);
        }
    }
}
