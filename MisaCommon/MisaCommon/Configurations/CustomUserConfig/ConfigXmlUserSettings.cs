namespace MisaCommon.Configurations.CustomUserConfig
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    /// <summary>
    /// ユーザコンフィグルート直下のユーザ設定Elementの定義を行うクラス
    /// 下記の3つのクラスで１セットのXML形式のユーザコンフィグを定義する
    /// ・<see cref="ConfigXmlRoot"/>
    /// ・<see cref="ConfigXmlUserSettings"/>
    /// ・<see cref="ConfigXmlSetting"/>
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
    [XmlRoot("userSettings")]
    public class ConfigXmlUserSettings
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// 各プロパティの初期化を行う
        /// </summary>
        public ConfigXmlUserSettings()
        {
            Settings = new Collection<ConfigXmlSetting>();
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを引数の値で初期化する
        /// </summary>
        /// <param name="settings">設定Element</param>
        public ConfigXmlUserSettings(Collection<ConfigXmlSetting> settings)
        {
            Settings = settings;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 設定Elementを取得・設定する
        /// </summary>
        /// <remarks>
        /// Xmlでシリアライズするために、リストではなくコレクションである必要がある
        /// </remarks>
        [XmlElement("setting")]
        public Collection<ConfigXmlSetting> Settings { get; }

        #endregion
    }
}
