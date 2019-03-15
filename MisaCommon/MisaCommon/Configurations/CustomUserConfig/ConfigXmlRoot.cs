namespace MisaCommon.Configurations.CustomUserConfig
{
    using System.Xml.Serialization;

    /// <summary>
    /// ユーザコンフィグのルートElementの定義を行うクラス
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
    [XmlRoot("configuration")]
    public class ConfigXmlRoot
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各プロパティの初期化を行う
        /// </summary>
        public ConfigXmlRoot()
        {
            UserSettings = new ConfigXmlUserSettings();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// ユーザ設定Elementを取得・設定する
        /// </summary>
        [XmlElement("userSettings")]
        public ConfigXmlUserSettings UserSettings { get; set; }

        #endregion
    }
}
