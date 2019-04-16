namespace MisaCommon.Configurations.CustomUserConfig
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Reflection;

    /// <summary>
    /// アプリケーションコンフィグ（App.config）に関する処理を提供するクラス
    /// </summary>
    public class AppConfig
    {
        #region メソッド

        /// <summary>
        /// Appコンフィグからアプリケーション設定を取得する
        /// （キー：名称、値：設定値）
        /// </summary>
        /// <remarks>
        /// 読み込みに失敗はAppコンフィグが存在しない または、壊れている場合を想定している
        /// その場合、コンフィグは無いものとして動作させる設計としているため、空のディクショナリを返却する
        /// </remarks>
        /// <param name="context">
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <returns>
        /// Appコンフィグのアプリケーション設定の情報（キー：名称、値：設定値）
        /// 取得できない場合、空のディクショナリを返却する
        /// </returns>
        public IDictionary<string, string> GetApplicationSettings(SettingsContext context)
        {
            return GetAppConfigSettings(context, false);
        }

        /// <summary>
        /// Appコンフィグからユーザ設定の情報を取得する
        /// （キー：名称、値：設定値）
        /// </summary>
        /// <remarks>
        /// 読み込みに失敗はAppコンフィグが存在しない または、壊れている場合を想定している
        /// その場合、コンフィグは無いものとして動作させる設計としているため、空のディクショナリを返却する
        /// </remarks>
        /// <param name="context">
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <returns>
        /// Appコンフィグのユーザ設定の情報（キー：名称、値：設定値）
        /// 取得できない場合、空のディクショナリを返却する
        /// </returns>
        public IDictionary<string, string> GetUserSettings(SettingsContext context)
        {
            return GetAppConfigSettings(context, true);
        }

        /// <summary>
        /// Appコンフィグからアプリケーション設定 または、ユーザ設定の情報を取得する
        /// （キー：名称、値：設定値）
        /// </summary>
        /// <remarks>
        /// 読み込みに失敗はAppコンフィグが存在しない または、壊れている場合を想定している
        /// その場合、コンフィグは無いものとして動作させる設計としているため、空のディクショナリを返却する
        /// </remarks>
        /// <param name="context">
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <param name="isUser">
        /// 取得の対象がユーザ設定かどうか（Falseの場合、アプリケーション設定を取得対象とする）
        /// </param>
        /// <returns>
        /// Appコンフィグのアプリケーション設定 または、ユーザ設定の情報（キー：名称、値：設定値）
        /// 取得できない場合、空のディクショナリを返却する
        /// </returns>
        public IDictionary<string, string> GetAppConfigSettings(
            SettingsContext context, bool isUser)
        {
            try
            {
                // Appコンフィグを読み込む
                Configuration config
                    = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // 読み込めた場合はそのデータをディクショナリーに設定する
                IDictionary<string, string> settings = new Dictionary<string, string>();
                string sectionName = GetSectionName(context, isUser);
                if (sectionName != null
                    && config.GetSection(sectionName) is ClientSettingsSection sectionData)
                {
                    foreach (SettingElement setting in sectionData.Settings)
                    {
                        settings.Add(setting.Name, setting?.Value?.ValueXml?.InnerXml);
                    }
                }

                // 取得した設定情報を返却
                return settings;
            }
            catch (ConfigurationErrorsException)
            {
                // Appコンフィグの読み込みに失敗した場合は、空のディクショナリを返却
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// ユーザ設定情報のデフォルト値（キー：名称、値：デフォルト値）を取得する
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
        /// <returns>
        /// ユーザ設定情報のデフォルト値（キー：名称、値：デフォルト値）
        /// </returns>
        public IDictionary<string, string> GetDefaultUserSettings(SettingsContext context)
        {
            // AppコンフィグのuserSettingを読み込む
            IDictionary<string, string> userSettings = GetUserSettings(context);

            // Contextが示すクラスからプロパティリストを取得
            PropertyInfo[] properties;
            if (GetDataFromSettingsContext<TypeInfo>(
                context, "SettingsClassType") is TypeInfo contextTypeInfo)
            {
                BindingFlags dindingAttr
                    = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
                properties = contextTypeInfo.GetProperties(dindingAttr);
            }
            else
            {
                properties = new PropertyInfo[0];
            }

            // 取得したプロパティリスト分ループ
            IDictionary<string, string> defaultSettings = new Dictionary<string, string>();
            foreach (PropertyInfo info in properties)
            {
                // プロパティの属性が「ユーザ設定」のプロパティのみリセットを行う
                if (info.GetCustomAttribute(typeof(UserScopedSettingAttribute)) != null)
                {
                    // プロパティメンバーの名称がNULL又は空の場合は次のループへ行く
                    if (string.IsNullOrEmpty(info.Name))
                    {
                        continue;
                    }

                    // デフォルト値を取得
                    string defaultValue;
                    if (userSettings.TryGetValue(info.Name, out string value)
                        && value != null)
                    {
                        // userSettinsの値が存在する場合、その値を取得
                        defaultValue = value;
                    }
                    else if (info.GetCustomAttribute(typeof(DefaultSettingValueAttribute))
                        is DefaultSettingValueAttribute defaultArrtibute)
                    {
                        // userSettinsに値が存在しない場合かつ、デフォルト属性の指定が存在する場合、
                        // そのデフォルト値を取得
                        defaultValue = defaultArrtibute.Value;
                    }
                    else
                    {
                        // 上記以外の場合、デフォルト値はNULLとする
                        defaultValue = null;
                    }

                    // ユーザコンフィグの値を既定値のデータを追加する
                    defaultSettings.Add(info.Name, defaultValue);
                }
            }

            // 取得した設定情報を返却
            return defaultSettings;
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 引数 <paramref name="context"/> から、
        /// アプリケーションの構成情報を格納しているセクションの名称を取得する
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <param name="isUser">
        /// ユーザ設定かどうか（Falseの場合、アプリケーション設定）
        /// </param>
        /// <returns>
        /// アプリケーションの構成情報を格納しているセクションの名称
        /// セクションの名称が取得できない場合はNULL
        /// </returns>
        private string GetSectionName(SettingsContext context, bool isUser)
        {
            // 引数のcontextがNULLでない場合、セクション名の設定を行う
            // グループ名、設定キーを取得
            string groupName = GetDataFromSettingsContext<string>(context, "GroupName");
            string settingsKey = GetDataFromSettingsContext<string>(context, "SettingsKey");

            // セクション名を設定する
            string sectionName = string.IsNullOrEmpty(groupName) ? null : groupName;
            if (sectionName != null && !string.IsNullOrEmpty(settingsKey))
            {
                sectionName += "." + settingsKey;
            }

            // ユーザ設定又はアプリケーション設定のセクション名を付与し返却する
            string sectionRoot = isUser ? "userSettings" : "applicationSettings";
            return string.IsNullOrEmpty(sectionName) ? null : sectionRoot + "/" + sectionName;
        }

        /// <summary>
        /// 引数 <paramref name="context"/> から指定したキー（<paramref name="key"/>）のデータを、
        /// 指定した型（<typeparamref name="T"/> ）で取得する
        /// </summary>
        /// <typeparam name="T">
        /// 取得するデータの型
        /// </typeparam>
        /// <param name="context">
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <param name="key">
        /// 取得するデータのキー
        /// </param>
        /// <returns>
        /// 指定したキー（<paramref name="key"/>）に該当する、<typeparamref name="T"/> 型のデータ
        /// 取得できない場合はNULL
        /// </returns>
        private T GetDataFromSettingsContext<T>(SettingsContext context, string key)
            where T : class
        {
            // NULLチェック
            if (context == null || key == null)
            {
                // 引数がNULLの場合、データを取得できないためNULLを返却
                return null;
            }

            // キーに対応するデータを取得
            object data = context.ContainsKey(key) ? context[key] : null;

            // 取得したデータが指定された型である場合はその型にキャストして返却
            // そうでない場合はNULLを返却
            return data is T ? (T)data : null;
        }

        #endregion
    }
}
