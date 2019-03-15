namespace MisaCommon.Configurations.CustomUserConfig
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;

    using MisaCommon.Exceptions;
    using MisaCommon.MessageResources;
    using MisaCommon.Utility.StaticMethod;

    /// <summary>
    /// Exe実行パスにてユーザに関するアプリケーション設定情報を管理するための、
    /// <see cref="IApplicationSettingsProvider"/> インターフェースを実装する <see cref="SettingsProvider"/> の派生クラス
    /// </summary>
    public class UserSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        #region クラス変数・定数

        /// <summary>
        /// このクラスが定義する <see cref="SettingsProvider"/> の名称
        /// </summary>
        private const string ProviderName = "UserSettingsProvider";

        /// <summary>
        /// 現在実行中のアプリケーションの名前
        /// </summary>
        private string _applicationName;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// <see cref="ExecuteEnvironment"/> からアプリケーションの名称を取得し初期化する
        /// </summary>
        public UserSettingsProvider()
        {
            // アセンブリのファイル名を設定
            _applicationName = ExecuteEnvironment.AssemblyFileNameWithoutExtension ?? string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の <paramref name="applicationName"/> を使用して初期化する
        /// </summary>
        /// <param name="applicationName">現在実行中のアプリケーションの名前</param>
        public UserSettingsProvider(string applicationName)
        {
            _applicationName = string.IsNullOrEmpty(applicationName) ? string.Empty : applicationName;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 現在実行中のアプリケーションの名前を取得・設定する
        /// （コンストラクタにて値を設定）
        /// </summary>
        /// <remarks>
        /// 基底クラスとの競合を避けるため、このプロパティは自動プロパティにしない
        /// </remarks>
        public override string ApplicationName { get => _applicationName; set => _applicationName = value; }

        #endregion

        #region メソッド

        /// <summary>
        /// ユーザコンフィグのデフォルトの設定情報（ディクショナリ_Key：名称、Value：設定値）を取得する
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
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
        /// ユーザコンフィグのデフォルトの設定情報（ディクショナリ_Key：名称、Value：設定値）
        /// </returns>
        public static IDictionary<string, string> GetDefaultUserSettings(SettingsContext context)
        {
            IDictionary<string, string> settings = new Dictionary<string, string>();

            // AppコンフィグのuserSettingを読み込む
            IDictionary<string, SettingElement> userSettings = GetAppConfigSettings(context, true);

            // Contextが示すクラスからプロパティリストを取得
            PropertyInfo[] properties;
            if (GetSettingsContextData<TypeInfo>(context, "SettingsClassType") is TypeInfo contextTypeInfo)
            {
                BindingFlags dindingAttr = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
                properties = contextTypeInfo.GetProperties(dindingAttr);
            }
            else
            {
                properties = new PropertyInfo[0];
            }

            // 取得したプロパティリスト分ループ
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
                    string defaultValue = string.Empty;
                    if (userSettings.TryGetValue(info.Name, out SettingElement element)
                        && element?.Value?.ValueXml?.InnerXml != null)
                    {
                        // userSettinsの値が存在する場合、その値を取得
                        defaultValue = userSettings[info.Name].Value.ValueXml.InnerXml;
                    }
                    else if (info.GetCustomAttribute(typeof(DefaultSettingValueAttribute))
                        is DefaultSettingValueAttribute defaultArrtibute)
                    {
                        // userSettinsに値が存在しない場合かつ、デフォルト属性の指定が存在する場合、そのデフォルト値を取得
                        defaultValue = defaultArrtibute.Value;
                    }

                    // ユーザコンフィグの値を既定値のデータを追加する
                    settings.Add(info.Name, defaultValue);
                }
            }

            // 取得した設定情報を返却
            return settings;
        }

        #region SettingsProviderの実装

        /// <summary>
        /// 初期化を行う
        /// </summary>
        /// <param name="name">
        /// プロバイダーの名称
        /// </param>
        /// <param name="config">
        /// このプロバイダーの構成で指定された、プロバイダー固有の属性を表す名前と値のペアのコレクション
        /// </param>
        public override void Initialize(string name, NameValueCollection config)
        {
            try
            {
                // 引数のプロバイダ名が未設定の場合、このクラスのプロバイダ名を設定する
                if (string.IsNullOrEmpty(name))
                {
                    name = ProviderName;
                }

                // 基底クラスの初期化処理を行う
                base.Initialize(name, config);
            }
            catch (InvalidOperationException ex)
            {
                // [InvalidOperationException]
                // ・プロバイダーが既に初期化されていた場合に発生（Initializeの2重実行の場合）

                // エラー処理
                // 初期化処理に失敗した場合、そもそも起動しないためアプリケーションを終了する
                ExceptionHandling.CriticalError(ex, ErrorMessage.UserSettingsProviderErrorInitialize);
            }
        }

        /// <summary>
        /// 対象のアプリケーションの構成ファイルから取得した値を元にして、
        /// 値を含む設定プロパティ（<see cref="SettingsPropertyValue"/>）のコレクションを生成して返却
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
        /// </param>
        /// <param name="collection">
        /// 値の取得対象となる設定プロパティ（<see cref="SettingsProperty"/>）のコレクション
        /// </param>
        /// <returns>
        /// 対象のアプリケーションの app.config、user.config から生成した <see cref="SettingsPropertyValue"/> のコレクション
        /// </returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            // 戻り値のコレクション生成
            SettingsPropertyValueCollection propertyValueCollection = new SettingsPropertyValueCollection();

            try
            {
                // AppコンフィグのapplicationSettingを読み込む
                IDictionary<string, SettingElement> applicationSettings = GetAppConfigSettings(context, false);

                // AppコンフィグのuserSettingsを読み込む
                IDictionary<string, SettingElement> userSettings = GetAppConfigSettings(context, true);

                // ユーザコンフィグを読み込む（Exeファイルが存在するフォルダに存在するuser.configを使用する）
                IDictionary<string, ConfigXmlSetting> userConfigSettings = new UserConfig().ConfigXmlSettingData;

                // 引数の設定プロパティのコレクションでループし、Appコンフィグ、ユーザコンフィグから読み込んだ値を設定する
                if (collection != null)
                {
                    foreach (SettingsProperty setting in collection)
                    {
                        // 設定対象のSettingsPropertyがNULL又は設定値の名称が存在しない場合は、
                        // 設定対象が存在しないため無視する
                        if (setting.Name == null)
                        {
                            continue;
                        }

                        // 引数から設定プロパティオブジェクトを生成
                        SettingsPropertyValue settingsPropertyValue = new SettingsPropertyValue(setting);

                        // 値の設定を行う
                        if (setting.Attributes?.Contains(typeof(UserScopedSettingAttribute)) ?? false)
                        {
                            // ユーザの設定の場合、読み込んだユーザコンフィグ、AppコンフィグのuserSettinsの値を設定する
                            if (userConfigSettings.TryGetValue(setting.Name, out ConfigXmlSetting userConfigElement)
                                && userConfigElement != null)
                            {
                                // ユーザコンフィグに値が存在する場合、その値を設定する
                                settingsPropertyValue.SerializedValue = userConfigElement.Value;
                            }
                            else if (userSettings.TryGetValue(setting.Name, out SettingElement userSettingElement)
                                && userSettingElement?.Value?.ValueXml?.InnerXml != null)
                            {
                                // ユーザコンフィグに値が存在しないかつ、AppコンフィグのuserSettinsの値が存在する場合、その値を設定する
                                settingsPropertyValue.SerializedValue = userSettingElement.Value.ValueXml.InnerXml;
                            }
                            else
                            {
                                // ユーザコンフィグ、AppコンフィグのuserSettinsに値が存在しない場合、デフォルト値を設定する
                                settingsPropertyValue.SerializedValue = setting.DefaultValue;
                            }
                        }
                        else if (setting.Attributes?.Contains(typeof(ApplicationScopedSettingAttribute)) ?? false)
                        {
                            // アプリケーション設定の場合、読み込んだAppコンフィグのapplicationSettingの値を設定する
                            if (applicationSettings.TryGetValue(setting.Name, out SettingElement appSettingElement)
                                && appSettingElement?.Value?.ValueXml?.InnerXml != null)
                            {
                                // AppコンフィグのapplicationSettingに値が存在する場合、その値を設定する
                                settingsPropertyValue.SerializedValue = appSettingElement.Value.ValueXml.InnerXml;
                            }
                            else
                            {
                                // AppコンフィグのapplicationSettingに値が存在しない場合、デフォルト値を設定する
                                settingsPropertyValue.SerializedValue = setting.DefaultValue;
                            }
                        }

                        // 初期取得処理であり値を変更していないため False を設定
                        settingsPropertyValue.IsDirty = false;

                        // 設定した値をコレクションに格納する
                        propertyValueCollection.Add(settingsPropertyValue);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                // [ArgumentException]
                // ・SerializedValueプロパティへの値の設定処理において
                // 　TypeConverterにおいて文字列から元の型への変換機能が提供されていない場合に発生

                // エラー処理
                // 読み込み処理に失敗した場合、そもそも起動しないためアプリケーションを終了する
                ExceptionHandling.CriticalError(ex, ErrorMessage.UserSettingsProviderErrorGetProperty);
            }

            // 戻り値を返却する
            return propertyValueCollection;
        }

        /// <summary>
        /// 引数（<paramref name="collection"/>）の内容をアプリケーションの構成ファイルに設定する
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
        /// </param>
        /// <param name="collection">
        /// 構成ファイルに設定する内容を含む、設定プロパティ（<see cref="SettingsPropertyValue"/>）のコレクション
        /// </param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            try
            {
                // ユーザコンフィグを読み込む（Exeファイルが存在するフォルダに存在するuser.configを使用する）
                UserConfig userConfig = new UserConfig();
                IDictionary<string, ConfigXmlSetting> userConfigSettings = userConfig.ConfigXmlSettingData;

                // 引数の設定プロパティのコレクションでループする
                if (collection != null)
                {
                    // collectionが2回破棄されるという警告の対応のため一旦配列にコピーしてからループ
                    SettingsPropertyValue[] settinArray = new SettingsPropertyValue[collection.Count];
                    collection.CopyTo(settinArray, 0);
                    foreach (SettingsPropertyValue setting in settinArray)
                    {
                        // 設定プロパティの属性が「ユーザ設定」と指定されているプロパティのみ保存対象とする
                        if (setting.Property?.Attributes?.Contains(typeof(UserScopedSettingAttribute)) ?? false)
                        {
                            // プロパティメンバーの名称がNULL又は空の場合は次のループへ行く
                            if (string.IsNullOrEmpty(setting.Name))
                            {
                                continue;
                            }

                            // ユーザコンフィグから名称が一致するデータが存在するかチェック
                            if (userConfigSettings.TryGetValue(setting.Name, out ConfigXmlSetting userConfigElement)
                                && userConfigElement != null)
                            {
                                // ユーザコンフィグに値が存在する場合、その値を更新する

                                // ユーザコンフィグの対象行のインデックスを取得
                                int index = userConfig.ConfigXmlData.UserSettings.Settings.IndexOf(userConfigElement);

                                // 値の更新
                                userConfig.ConfigXmlData.UserSettings.Settings[index].SerializeAs
                                    = setting.Property.SerializeAs.ToString();
                                userConfig.ConfigXmlData.UserSettings.Settings[index].Value
                                    = setting.SerializedValue?.ToString();
                            }
                            else
                            {
                                // ユーザコンフィグに値が存在しない場合、設定を追加する
                                userConfig.ConfigXmlData.UserSettings.Settings.Add(
                                    new ConfigXmlSetting(
                                        name: setting.Name,
                                        serializeAs: setting.Property.SerializeAs.ToString(),
                                        value: setting.SerializedValue?.ToString()));
                            }
                        }
                    }

                    // 使い終わったコピー領域を解放
                    settinArray = null;
                }

                // ユーザコンフィグの保存を行う
                userConfig.Write();
            }
            catch (Exception ex)
                when (ex is UnauthorizedAccessException
                    || ex is IOException
                    || ex is SecurityException)
            {
                // 下記のエラーの場合は想定しているエラーのため個別に処理を行う
                // [UnauthorizedAccessException]
                // ・コンフィグの保存先フォルダへのアクセスが拒否された場合に発生
                // [IOException],[PathTooLongException]
                // ・コンフィグの保存パスにおいてファイル名、ディレクトリ名、またはボリューム ラベル構文の正しくないか無効な構文の場合に発生
                // ・コンフィグの保存パスにがシステム定義の最大長を超えている場合に発生
                // 　Windows ベースのプラットフォームでは、パスは248文字以下、ファイル名は260文字以下にする必要がある
                // [SecurityException]
                // ・コンフィグの保存先フォルダへの必要なアクセス許可がない場合に発生

                // エラー処理
                // 値設定処理は継続可能なためエラーメッセージを表示し処理は続行する
                ExceptionHandling.Error(ex, ErrorMessage.UserSettingsProviderErrorSetProperty);
            }
        }

        #endregion

        #region IApplicationSettingsProviderの実装

        /// <summary>
        /// 【未使用のため未実装】引数 <paramref name="property"/> で指定したプロパティにおいて、同じアプリケーションの前のバージョンの値を取得する
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
        /// </param>
        /// <param name="property">
        /// 取得対象とする設定プロパティ（<see cref="SettingsProperty"/>）
        /// </param>
        /// <exception cref="NotImplementedException">
        /// このメソッドを呼び出した時に発生
        /// </exception>
        /// <returns>
        /// 引数 <paramref name="property"/> で指定したプロパティにおいて、前のバージョンのアプリケーションで最後に設定されたときの値を格納している
        /// 値を含む設定プロパティ（<see cref="SettingsPropertyValue"/>）を返却
        /// 設定が存在しない場合はNULLを返却
        /// </returns>
        [Obsolete("このメソッドは実装されていないため使用できません。", true)]
        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            // 使用していないため実装しない
            throw new NotImplementedException();
        }

        /// <summary>
        /// アプリケーション構成ファイルを既定値にリセットする
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
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
        public void Reset(SettingsContext context)
        {
            try
            {
                // ユーザコンフィグを生成する
                UserConfig userConfig = new UserConfig();

                // リセットの処理のためユーザコンフィグの設定情報をクリアする
                userConfig.ConfigXmlData.UserSettings.Settings.Clear();

                // ユーザコンフィグのデフォルトの設定情報を取得する
                IDictionary<string, string> defaultUserSettings = GetDefaultUserSettings(context);

                // 取得したプロパティリスト分ループ
                foreach (string name in defaultUserSettings.Keys.ToArray())
                {
                    // ユーザコンフィグの値を既定値のデータを追加する
                    string value = defaultUserSettings[name];
                    userConfig.ConfigXmlData.UserSettings.Settings.Add(
                        new ConfigXmlSetting(name, "String", value));
                }

                // リセットしたユーザコンフィグを保存する
                userConfig.Write();
            }
            catch (Exception ex)
                when (ex is UnauthorizedAccessException
                    || ex is IOException
                    || ex is SecurityException)
            {
                // 下記のエラーの場合は想定しているエラーのため個別に処理を行う
                // [UnauthorizedAccessException]
                // ・コンフィグの保存先フォルダへのアクセスが拒否された場合に発生
                // [IOException],[PathTooLongException]
                // ・コンフィグの保存パスにおいてファイル名、ディレクトリ名、またはボリューム ラベル構文の正しくないか無効な構文の場合に発生
                // ・コンフィグの保存パスにがシステム定義の最大長を超えている場合に発生
                // 　Windows ベースのプラットフォームでは、パスは248文字以下、ファイル名は260文字以下にする必要がある
                // [SecurityException]
                // ・コンフィグの保存先フォルダへの必要なアクセス許可がない場合に発生

                // エラー処理
                // リセット処理は継続可能なためエラーメッセージを表示し処理は続行する
                ExceptionHandling.Error(ex, ErrorMessage.UserSettingsProviderErrorReset);
            }
        }

        /// <summary>
        /// 【未使用のため未実装】アプリケーションがアップグレードされたことをプロバイダーに示す
        /// これにより、プロバイダーは格納している値を必要に応じてアップグレード可能とする
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
        /// </param>
        /// <param name="properties">
        /// 値の取得対象となる設定プロパティ（<see cref="SettingsProperty"/>）のコレクション
        /// </param>
        /// <exception cref="NotImplementedException">
        /// このメソッドを呼び出した時に発生
        /// </exception>
        [Obsolete("このメソッドは実装されていないため使用できません。", true)]
        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            // 使用していないため実装しない
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// Appコンフィグからアプリケーション設定セクション又はユーザ設定セクションの設定情報を取得する
        /// </summary>
        /// <remarks>
        /// 読み込みに失敗はAppコンフィグが存在しない又は壊れている場合を想定している
        /// その場合、コンフィグは無いものとして動作させる設計としているため、要素0の設定情報リストを返却する
        /// </remarks>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
        /// </param>
        /// <param name="isUser">
        /// ユーザ設定かどうか（Falseの場合、アプリケーション設定）
        /// </param>
        /// <returns>
        /// Appコンフィグからアプリケーション設定セクション又はユーザ設定セクションの設定情報
        /// 取得できない場合はデータなしとして要素0の設定情報リストを返却する
        /// </returns>
        private static IDictionary<string, SettingElement> GetAppConfigSettings(
            SettingsContext context, bool isUser)
        {
            try
            {
                // Appコンフィグを読み込む
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // 読み込めた場合はそのデータをディクショナリーに設定する
                IDictionary<string, SettingElement> settings = new Dictionary<string, SettingElement>();
                if (config.GetSection(GetSectionName(context, isUser) ?? string.Empty) is ClientSettingsSection sectionData)
                {
                    foreach (SettingElement setting in sectionData.Settings)
                    {
                        settings.Add(setting.Name, setting);
                    }
                }

                // 取得した設定情報を返却
                return settings;
            }
            catch (ConfigurationErrorsException)
            {
                // Appコンフィグの読み込みに失敗した場合は、データなしとして要素0の設定情報リストを返却
                // 補足：読み込みに失敗はAppコンフィグが存在しない又は壊れている場合を想定している
                // 　　　その場合、コンフィグは無いものとして動作させる設計としているため、
                // 　　　要素0の設定情報リストを返却する
                return new Dictionary<string, SettingElement>();
            }
        }

        /// <summary>
        /// 引数 <paramref name="context"/> からアプリケーションの構成情報を格納しているセクションの名称を取得する
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
        /// </param>
        /// <param name="isUser">
        /// ユーザ設定かどうか（Falseの場合、アプリケーション設定）
        /// </param>
        /// <returns>
        /// アプリケーションの構成情報を格納しているセクションの名称
        /// セクションの名称が取得できない場合はNULL
        /// </returns>
        private static string GetSectionName(SettingsContext context, bool isUser)
        {
            // 引数のcontextがNULLでない場合、セクション名の設定を行う
            // グループ名、設定キーを取得
            string groupName = GetSettingsContextData<string>(context, "GroupName");
            string settingsKey = GetSettingsContextData<string>(context, "SettingsKey");

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
        /// 引数 <paramref name="context"/> から指定したキー（<paramref name="key"/>）、
        /// のデータをobject指定した型（<typeparamref name="T"/>）の条件でデータを取得する
        /// 取得できない場合はNULLを返却する
        /// </summary>
        /// <typeparam name="T">
        /// 取得するデータの型
        /// </typeparam>
        /// <param name="context">
        /// 現在のアプリケーションのコンテキスト情報を格納した <see cref="SettingsContext"/> オブジェクト
        /// </param>
        /// <param name="key">
        /// 取得するデータのキー
        /// </param>
        /// <returns>
        /// 引数 <paramref name="context"/> から指定したキー（<paramref name="key"/>）、
        /// 指定した型（<typeparamref name="T"/>）の条件に合致するデータ
        /// 取得できない場合はNULL
        /// </returns>
        private static T GetSettingsContextData<T>(SettingsContext context, string key)
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
