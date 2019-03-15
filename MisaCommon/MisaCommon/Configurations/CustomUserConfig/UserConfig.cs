namespace MisaCommon.Configurations.CustomUserConfig
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Security;
    using System.Text;
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
        private static string _userConfigFileName = null;

        /// <summary>
        /// ユーザコンフィグのXML形式のデータ
        /// </summary>
        private ConfigXmlRoot _configXmlData;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 初期化を行う
        /// </summary>
        public UserConfig()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ユーザコンフィグのファイル名を取得する
        /// </summary>
        /// <remarks>
        /// ユーザコンフィグのファイル名をシングルトンパターンで実装
        /// </remarks>
        public static string UserConfigFileName
        {
            get
            {
                if (_userConfigFileName == null)
                {
                    // アセンブリのファイル名を取得
                    string assemblyFileName = ExecuteEnvironment.AssemblyFileName;

                    // アセンブリ名+ユーザコンフィグのファイル名を結合して設定
                    _userConfigFileName = string.Format(
                        CultureInfo.InvariantCulture,
                        UserConfigFileNameFormat,
                        assemblyFileName ?? string.Empty);
                }

                // ファイル名を返却
                return _userConfigFileName;
            }
        }

        /// <summary>
        /// ユーザコンフィグのXML形式のデータを取得する
        /// </summary>
        public ConfigXmlRoot ConfigXmlData
        {
            get
            {
                if (_configXmlData == null)
                {
                    _configXmlData = Load();
                }

                return _configXmlData;
            }
        }

        /// <summary>
        /// ユーザコンフィグの設定情報リストを取得する
        /// </summary>
        public IDictionary<string, ConfigXmlSetting> ConfigXmlSettingData => GetSettingData();

        #endregion

        #region メソッド

        /// <summary>
        /// ユーザコンフィグを保存する
        /// </summary>
        /// <exception cref="UnauthorizedAccessException">
        /// コンフィグの保存先フォルダへのアクセスが拒否された場合に発生
        /// </exception>
        /// <exception cref="IOException">
        /// コンフィグの保存パスにおいてファイル名、ディレクトリ名、またはボリューム ラベル構文の正しくないか無効な構文の場合に発生
        /// </exception>
        /// <exception cref="PathTooLongException">
        /// コンフィグの保存パスにがシステム定義の最大長を超えている場合に発生
        /// Windows ベースのプラットフォームでは、パスは248文字以下、ファイル名は260文字以下にする必要がある
        /// </exception>
        /// <exception cref="SecurityException">
        /// コンフィグの保存先フォルダへの必要なアクセス許可がない場合に発生
        /// </exception>
        public void Write()
        {
            // コンフィグデータが存在するかチェック
            if (ConfigXmlData.UserSettings.Settings.Count == 0)
            {
                // 保存するべき情報が存在しない場合は処理を終了する
                return;
            }

            // 同時実行でファイルIOのエラーを防ぐため排他制御を行う
            lock (LockFileWriteObject)
            {
                // ユーザコンフィグを保存する
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigXmlRoot));
                using (StreamWriter stream = new StreamWriter(UserConfigFileName, false, new UTF8Encoding(false)))
                {
                    serializer.Serialize(stream, ConfigXmlData);
                }
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// ユーザコンフィグを読み込む
        /// </summary>
        /// <returns>ユーザコンフィグのXML形式のデータ</returns>
        private static ConfigXmlRoot Load()
        {
            // ユーザーコンフィグのファイル名を取得
            string userConfigFileName = UserConfigFileName;

            // ユーザコンフィグの存在チェック
            if (!File.Exists(userConfigFileName))
            {
                // ユーザコンフィグが存在しない場合、初期値（空のユーザコンフィグデータ）を返却
                // 新規作成のパターン
                return new ConfigXmlRoot();
            }

            // ユーザコンフィグを逆シリアル化して読む込む
            ConfigXmlRoot config = null;
            using (StreamReader stream = new StreamReader(userConfigFileName, new UTF8Encoding(false)))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ConfigXmlRoot));
                    config = serializer.Deserialize(stream) as ConfigXmlRoot;
                }
                catch (InvalidOperationException)
                {
                    // 逆シリアル化に失敗し読み込めない場合、初期値（空のユーザコンフィグデータ）を返却
                    // （ユーザコンフィグのデータが不正：壊れている場合は、現状のコンフィグを廃棄して再作成する）
                    return new ConfigXmlRoot();
                }
            }

            // 読み込んだユーザコンフィグを返却
            return config;
        }

        /// <summary>
        /// ユーザコンフィグから設定情報リストを取得する
        /// </summary>
        /// <returns>
        /// ユーザコンフィグの設定情報を返却
        /// <see cref="string"/>：設定の名称（キー）
        /// <see cref="ConfigXmlSetting"/>：ユーザコンフィグの「setting」要素の内容を格納したのオブジェクト
        /// </returns>
        private IDictionary<string, ConfigXmlSetting> GetSettingData()
        {
            // ユーザコンフィグの「setting」要素の情報分ループし、
            // ユーザコンフィグの設定情報を生成する
            IDictionary<string, ConfigXmlSetting> returnList = new Dictionary<string, ConfigXmlSetting>();
            foreach (ConfigXmlSetting setting in ConfigXmlData.UserSettings.Settings)
            {
                returnList.Add(setting.Name, setting);
            }

            // ユーザコンフィグの設定情報を返す
            return returnList;
        }

        #endregion
    }
}
