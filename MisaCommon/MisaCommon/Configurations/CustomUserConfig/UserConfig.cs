namespace MisaCommon.Configurations.CustomUserConfig
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using MisaCommon.Utility.StaticMethod;

    /// <summary>
    /// ユーザコンフィグの定義クラス
    /// </summary>
    /// <example>
    /// 下記の形式のユーザコンフィグを定義する
    /// <code>
    /// <![CDATA[
    /// <?xml version="1.0" encoding="utf-8"?>
    /// <configuration>
    ///     <userSettings>
    ///         <setting name="[指定したプロパティの名称]" serializeAs="String">
    ///             <value>[プロパティの設定値]</value>
    ///         </setting>
    ///         ・・・以降settingタグの繰り返し・・・
    ///     </userSettings>
    /// </configuration>
    /// ]]>
    /// </code>
    /// </example>
    public class UserConfig
    {
        #region クラス変数・定数

        /// <summary>
        /// ユーザコンフィグのファイル名のフォーマット（{0}：アセンブリのファイル名）
        /// </summary>
        private const string UserConfigFileNameFormat = "{0}.user.config";

        /// <summary>
        /// ファイルへの書き込み処理の排他制御を行うロックオブジェクト
        /// </summary>
        private static readonly object LockFileWriteObject = new object();

        /// <summary>
        /// ユーザコンフィグのファイル名
        /// </summary>
        private static string userConfigFileName = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// 既定のユーザコンフィグファイルを読み込み、各プロパティの初期化を行う
        /// </summary>
        public UserConfig()
        {
            Load();
        }

        /// <summary>
        /// コンストラクタ
        /// 引数のユーザコンフィグ（XML形式）で、初期化を行う
        /// </summary>
        /// <param name="userConfigXmlData">ユーザコンフィグのXML形式のデータ</param>
        public UserConfig(ConfigXmlRoot userConfigXmlData)
        {
            XmlData = userConfigXmlData;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ユーザコンフィグのファイル名を取得する
        /// </summary>
        /// <remarks>
        /// ユーザコンフィグのファイル名をシングルトンパターンで実装
        /// </remarks>
        public static string FileName
        {
            get
            {
                if (userConfigFileName == null)
                {
                    // アセンブリのファイル名を取得
                    string assemblyFileName = ExecuteEnvironment.AssemblyFileName;

                    // アセンブリ名+ユーザコンフィグのファイル名を結合して設定
                    userConfigFileName = string.Format(
                        CultureInfo.InvariantCulture,
                        UserConfigFileNameFormat,
                        assemblyFileName ?? string.Empty);
                }

                // ファイル名を返却
                return userConfigFileName;
            }
        }

        /// <summary>
        /// ユーザコンフィグのXML形式のデータを取得する
        /// </summary>
        public ConfigXmlRoot XmlData { get; private set; }

        /// <summary>
        /// ユーザコンフィグの設定情報リストを取得する
        /// </summary>
        public IDictionary<string, ConfigXmlSetting> SettingData
            => XmlData.UserSettings.Settings.ToDictionary((setting) => setting.Name);

        #endregion

        #region メソッド

        /// <summary>
        /// デフォルト値のユーザコンフィグを生成する
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <exception cref="AmbiguousMatchException">
        /// 引数のコンテキスト情報（<see cref="SettingsContext"/>）から取得したプロパティにおいて、
        /// 下記の属性が重複して設定されている場合に発生
        /// ・<see cref="UserScopedSettingAttribute"/> 属性
        /// ・<see cref="DefaultSettingValueAttribute"/> 属性
        /// </exception>
        /// <exception cref="TypeLoadException">
        /// 引数のコンテキスト情報（<see cref="SettingsContext"/>）から取得したプロパティが、
        /// カスタム属性の型であり読み込むことができない場合に発生
        /// </exception>
        /// <returns>デフォルト値のユーザコンフィグのインスタンス</returns>
        public static UserConfig CreateDefaultValueConfig(SettingsContext context)
        {
            // アプリケーション設定からユーザ設定のデフォルト値を取得する
            AppConfig appConfig = new AppConfig();
            IDictionary<string, string> defaultUserSettings = appConfig.GetDefaultUserSettings(context);

            // デフォルト値の設定情報を生成する
            IList<ConfigXmlSetting> xmlSettings = defaultUserSettings.Select(
                (setting) => new ConfigXmlSetting(setting.Key, "String", setting.Value)).ToList();

            // ユーザコンフィグのインスタンスを生成して返す
            ConfigXmlUserSettings settings
                = new ConfigXmlUserSettings(new Collection<ConfigXmlSetting>(xmlSettings));
            return new UserConfig(new ConfigXmlRoot(settings));
        }

        /// <summary>
        /// ユーザコンフィグを読み込む
        /// </summary>
        public void Load()
        {
            // ユーザーコンフィグのファイル名を取得
            string userConfigFileName = FileName;

            // ユーザコンフィグの存在チェック
            if (!File.Exists(userConfigFileName))
            {
                // 新規作成のパターン
                // ユーザコンフィグが存在しない場合、初期値（空のユーザコンフィグデータ）を設定
                XmlData = new ConfigXmlRoot();
                return;
            }

            // 同時実行でファイルIOのエラーを防ぐため排他制御を行う
            lock (LockFileWriteObject)
            {
                // ユーザコンフィグを逆シリアル化して読み込む
                TextReader reader;
                using (reader = new StreamReader(userConfigFileName, new UTF8Encoding(false)))
                {
                    // （事前に File.Exists で存在チェックを行っているため
                    // 　TextReader の生成では例外は発生しない）
                    XmlData = DeserializeRead(reader);
                }
            }
        }

        /// <summary>
        /// ユーザコンフィグを保存する
        /// </summary>
        /// <exception cref="IOException">
        /// 下記の場合に発生
        /// ・コンフィグの保存パスが、システム定義の最大長を超えている場合
        /// 　[<see cref="PathTooLongException"/>]
        /// 　（Windowsでは、パスは248文字以下、ファイル名は260文字以下にする必要がある）
        /// ・コンフィグの保存パスが正しくない場合（マップされていないドライブ等）
        /// 　[<see cref="DirectoryNotFoundException"/>]
        /// ・コンフィグの保存パスにおいてファイル、ディレクトリ、または、ボリュームラベルが正しくない場合
        /// 　[<see cref="IOException"/>]
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// コンフィグの保存先フォルダへのアクセスが拒否された場合に発生
        /// </exception>
        /// <exception cref="SecurityException">
        /// コンフィグの保存先フォルダへの必要なアクセス許可がない場合に発生
        /// </exception>
        public void Write()
        {
            // コンフィグデータが存在するかチェック
            if (XmlData.UserSettings.Settings.Count == 0)
            {
                // 保存するべき情報が存在しない場合は処理を終了する
                return;
            }

            // 同時実行でファイルIOのエラーを防ぐため排他制御を行う
            lock (LockFileWriteObject)
            {
                // ユーザコンフィグをシリアライズ化して保存する
                StreamWriter writer;
                using (writer = new StreamWriter(FileName, false, new UTF8Encoding(false)))
                {
                    SerializeWrite(writer, XmlData);
                }
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// ユーザコンフィグのXMLファイルをデシリアライズして読み込む
        /// </summary>
        /// <param name="reader">
        /// ユーザコンフィグのXMLファイルの <see cref="TextReader"/> ストリーム
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="reader"/> が NULL の場合に発生
        /// </exception>
        /// <returns>読み込んだユーザコンフィグのデータ</returns>
        private static ConfigXmlRoot DeserializeRead(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            try
            {
                // 逆シリアル化して読む込む
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigXmlRoot));
                using (XmlReader xmlReader = XmlReader.Create(
                    reader, new XmlReaderSettings() { XmlResolver = null }))
                {
                    return serializer.Deserialize(xmlReader) as ConfigXmlRoot;
                }
            }
            catch (InvalidOperationException)
            {
                // ユーザコンフィグ破損パターン
                // 逆シリアル化に失敗し読み込めない場合、初期値（空のユーザコンフィグデータ）を返却
                // （ユーザコンフィグのデータが不正の場合は、現状のコンフィグを廃棄して再作成する）
                return new ConfigXmlRoot();
            }
        }

        /// <summary>
        /// ユーザコンフィグのデータをシリアライズ化を行いXMLファイル形式で書き込む
        /// </summary>
        /// <param name="writer">
        /// 書き込み用の <see cref="TextWriter"/> ストリーム
        /// </param>
        /// <param name="xmlData">
        /// 書き込むユーザコンフィグのデータ
        /// </param>
        private static void SerializeWrite(TextWriter writer, ConfigXmlRoot xmlData)
        {
            // シリアル化して書き込む
            XmlSerializer serializer = new XmlSerializer(typeof(ConfigXmlRoot));
            serializer.Serialize(writer, xmlData);
        }

        #endregion
    }
}
