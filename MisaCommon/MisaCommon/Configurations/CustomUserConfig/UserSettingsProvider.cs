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
    /// <see cref="IApplicationSettingsProvider"/> インターフェースを実装する
    /// <see cref="SettingsProvider"/> の派生クラス
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
        private string applicationName;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// <see cref="ExecuteEnvironment"/> からアプリケーションの名称を取得し初期化する
        /// </summary>
        public UserSettingsProvider()
        {
            // アセンブリのファイル名を設定
            applicationName = ExecuteEnvironment.AssemblyFileNameWithoutExtension ?? string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の <paramref name="applicationName"/> を使用して初期化する
        /// </summary>
        /// <param name="applicationName">現在実行中のアプリケーションの名前</param>
        public UserSettingsProvider(string applicationName)
        {
            this.applicationName = string.IsNullOrEmpty(applicationName) ? string.Empty : applicationName;
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
        public override string ApplicationName { get => applicationName; set => applicationName = value; }

        #endregion

        #region メソッド

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
                // 下記のエラーの場合は想定しているエラーのため個別に処理を行う
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
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <param name="collection">
        /// 値の取得対象となる設定プロパティ（<see cref="SettingsProperty"/>）のコレクション
        /// </param>
        /// <returns>
        /// 対象のアプリケーションの app.config、user.config から生成した
        /// <see cref="SettingsPropertyValue"/> のコレクション
        /// </returns>
        public override SettingsPropertyValueCollection GetPropertyValues(
            SettingsContext context, SettingsPropertyCollection collection)
        {
            // 引数の設定プロパティのコレクションが NULL の場合は、空のコレクションを返却する
            if (collection == null)
            {
                return new SettingsPropertyValueCollection();
            }

            // App.config からアプリケーション設定セクション、ユーザ設定セクションの情報を読み込む
            AppConfig appConfig = new AppConfig();
            IDictionary<string, string> applicationSettings = appConfig.GetApplicationSettings(context);
            IDictionary<string, string> userSettings = appConfig.GetUserSettings(context);

            // ユーザコンフィグを読み込む
            // （Exeファイルが存在するフォルダに存在するuser.configを使用する）
            IDictionary<string, ConfigXmlSetting> userConfigData = new UserConfig().SettingData;

            // 戻り値のコレクション生成
            SettingsPropertyValueCollection propertyValueCollection = new SettingsPropertyValueCollection();

            try
            {
                // 引数の設定プロパティのコレクションでループし、
                // Appコンフィグ、ユーザコンフィグから読み込んだ値を設定する
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
                    if (setting.Attributes?.Contains(
                        typeof(UserScopedSettingAttribute)) ?? false)
                    {
                        // ユーザの設定の場合
                        if (userConfigData.TryGetValue(setting.Name, out ConfigXmlSetting userConfigElement)
                            && userConfigElement != null)
                        {
                            // ユーザコンフィグに値が存在する場合、その値を設定する
                            settingsPropertyValue.SerializedValue = userConfigElement.Value;
                        }
                        else if (userSettings.TryGetValue(setting.Name, out string userSettingValue)
                            && userSettingValue != null)
                        {
                            // ユーザコンフィグに値が存在しないかつ
                            // AppコンフィグのuserSettinsの値が存在する場合、
                            // その値を設定する
                            settingsPropertyValue.SerializedValue = userSettingValue;
                        }
                        else
                        {
                            // ユーザコンフィグ、AppコンフィグのuserSettinsに値が存在しない場合、
                            // デフォルト値を設定する
                            settingsPropertyValue.SerializedValue = setting.DefaultValue;
                        }
                    }
                    else if (setting.Attributes?.Contains(
                        typeof(ApplicationScopedSettingAttribute)) ?? false)
                    {
                        // アプリケーション設定の場合
                        if (applicationSettings.TryGetValue(setting.Name, out string appSettingValue)
                            && appSettingValue != null)
                        {
                            // AppコンフィグのapplicationSettingに値が存在する場合、
                            // その値を設定する
                            settingsPropertyValue.SerializedValue = appSettingValue;
                        }
                        else
                        {
                            // AppコンフィグのapplicationSettingに値が存在しない場合、
                            // デフォルト値を設定する
                            settingsPropertyValue.SerializedValue = setting.DefaultValue;
                        }
                    }

                    // 初期取得処理であり値を変更していないため False を設定
                    settingsPropertyValue.IsDirty = false;

                    // 設定した値をコレクションに格納する
                    propertyValueCollection.Add(settingsPropertyValue);
                }
            }
            catch (ArgumentException ex)
            {
                // 下記のエラーの場合は想定しているエラーのため個別に処理を行う
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
        /// 引数（<paramref name="collection"/>）の内容を設定プロパティに設定する
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <param name="collection">
        /// 構成ファイルに設定する内容を含む、
        /// 設定プロパティ（<see cref="SettingsPropertyValue"/>）のコレクション
        /// </param>
        public override void SetPropertyValues(
            SettingsContext context, SettingsPropertyValueCollection collection)
        {
            // 引数の設定プロパティのコレクションが NULL の場合は処理を抜ける
            if (collection == null)
            {
                return;
            }

            // ユーザコンフィグを読み込む
            // （Exeファイルが存在するフォルダに存在するuser.configを使用する）
            UserConfig userConfig = new UserConfig();

            // 引数の設定プロパティのコレクションでループする
            foreach (SettingsPropertyValue setting in collection.OfType<SettingsPropertyValue>())
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
                    if (userConfig.SettingData.TryGetValue(
                        setting.Name, out ConfigXmlSetting userConfigElement)
                        && userConfigElement != null)
                    {
                        // ユーザコンフィグに値が存在する場合、その値を更新する

                        // ユーザコンフィグの対象行のインデックスを取得
                        int index = userConfig.XmlData.UserSettings.Settings.IndexOf(userConfigElement);

                        // 値の更新
                        userConfig.XmlData.UserSettings.Settings[index].SerializeAs
                            = setting.Property.SerializeAs.ToString();
                        userConfig.XmlData.UserSettings.Settings[index].Value
                            = setting.SerializedValue?.ToString();
                    }
                    else
                    {
                        // ユーザコンフィグに値が存在しない場合、設定を追加する
                        userConfig.XmlData.UserSettings.Settings.Add(
                            new ConfigXmlSetting(
                                name: setting.Name,
                                serializeAs: setting.Property.SerializeAs.ToString(),
                                value: setting.SerializedValue?.ToString()));
                    }
                }
            }

            try
            {
                // ユーザコンフィグの保存を行う
                userConfig.Write();
            }
            catch (Exception ex)
                when (ex is IOException
                    || ex is UnauthorizedAccessException
                    || ex is SecurityException)
            {
                // 下記のエラーの場合は想定しているエラーのため個別に処理を行う
                // [IOException]
                // ・コンフィグの保存パスが、システム定義の最大長を超えている場合に発生
                // 　[PathTooLongException]
                // 　（Windowsでは、パスは248文字以下、ファイル名は260文字以下にする必要がある）
                // ・コンフィグの保存パスが正しくない場合に発生（マップされていないドライブ等）
                // 　[DirectoryNotFoundException]
                // ・コンフィグの保存パスにおいてファイル名、ディレクトリ名、またはボリューム ラベル構文が、
                // 　正しくないか無効な構文の場合に発生
                // 　[IOException]
                // [UnauthorizedAccessException]
                // ・コンフィグの保存先フォルダへのアクセスが拒否された場合に発生
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
        /// 【未使用のため未実装】引数 <paramref name="property"/> で指定したプロパティにおいて、
        /// 同じアプリケーションの前のバージョンの値を取得する
        /// </summary>
        /// <param name="context">
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
        /// </param>
        /// <param name="property">
        /// 取得対象とする設定プロパティ（<see cref="SettingsProperty"/>）
        /// </param>
        /// <exception cref="NotImplementedException">
        /// このメソッドを呼び出した時に発生
        /// </exception>
        /// <returns>
        /// 引数 <paramref name="property"/> で指定したプロパティにおいて、
        /// 前のバージョンのアプリケーションで最後に設定されたときの値を格納している値を含む
        /// 設定プロパティ（<see cref="SettingsPropertyValue"/>）を返却
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
        public void Reset(SettingsContext context)
        {
            try
            {
                // ユーザコンフィグのデフォルトの設定情報を取得する
                UserConfig defaultConfig = UserConfig.CreateDefaultValueConfig(context);

                // デフォルトの設定を保存する
                defaultConfig.Write();
            }
            catch (Exception ex)
                when (ex is IOException
                    || ex is UnauthorizedAccessException
                    || ex is SecurityException)
            {
                // 下記のエラーの場合は想定しているエラーのため個別に処理を行う
                // [IOException]
                // ・コンフィグの保存パスが、システム定義の最大長を超えている場合に発生
                // 　[PathTooLongException]
                // 　（Windowsでは、パスは248文字以下、ファイル名は260文字以下にする必要がある）
                // ・コンフィグの保存パスが正しくない場合に発生（マップされていないドライブ等）
                // 　[DirectoryNotFoundException]
                // ・コンフィグの保存パスにおいてファイル名、ディレクトリ名、またはボリューム ラベル構文が、
                // 　正しくないか無効な構文の場合に発生
                // 　[IOException]
                // [UnauthorizedAccessException]
                // ・コンフィグの保存先フォルダへのアクセスが拒否された場合に発生
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
        /// 現在のアプリケーションの設定情報を格納した、コンテキストオブジェクト
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
    }
}
