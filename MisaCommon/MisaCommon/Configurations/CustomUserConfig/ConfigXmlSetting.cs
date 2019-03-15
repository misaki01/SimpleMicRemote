namespace MisaCommon.Configurations.CustomUserConfig
{
    using System.Xml.Serialization;

    /// <summary>
    /// ユーザコンフィグのユーザ設定Element配下の設定Elementの定義を行うクラス
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
    [XmlRoot("setting")]
    public class ConfigXmlSetting
    {
        #region クラス変数・定数

        /// <summary>
        /// 名称
        /// </summary>
        private string _name;

        /// <summary>
        /// シリアル化の型
        /// </summary>
        private string _serializeAs;

        /// <summary>
        /// 値
        /// </summary>
        private string _value;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各プロパティを <see cref="string.Empty"/> で初期化する
        /// </summary>
        public ConfigXmlSetting()
        {
            Name = string.Empty;
            SerializeAs = string.Empty;
            Value = string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを引数の値で初期化する
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="serializeAs">シリアル化の型</param>
        /// <param name="value">値</param>
        public ConfigXmlSetting(string name, string serializeAs, string value)
        {
            Name = name ?? string.Empty;
            SerializeAs = serializeAs ?? string.Empty;
            Value = value ?? string.Empty;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 名称を取得・設定する
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get => _name;
            set => _name = value ?? string.Empty;
        }

        /// <summary>
        /// シリアル化の型を取得・設定する
        /// </summary>
        [XmlAttribute("serializeAs")]
        public string SerializeAs
        {
            get => _serializeAs;
            set => _serializeAs = value ?? string.Empty;
        }

        /// <summary>
        /// 値を取得・設定する
        /// </summary>
        [XmlElement("value")]
        public string Value
        {
            get => _value;
            set => _value = value ?? string.Empty;
        }

        #endregion
    }
}
