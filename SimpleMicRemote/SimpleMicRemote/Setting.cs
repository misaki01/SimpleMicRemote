namespace SimpleMicRemote
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;

    using MisaCommon.Configurations.CustomUserConfig;
    using MisaCommon.CustomType;
    using MisaCommon.CustomType.Attribute;
    using MisaCommon.CustomType.Converter;
    using MisaCommon.Utility.ExtendMethod;

    /// <summary>
    /// 起動モード（アプリケーション起動の際のモード）
    /// </summary>
    public enum StartupMode
    {
        /// <summary>
        /// 設定（メイン画面：表示、　音声認識：停止）
        /// </summary>
        Setting,

        /// <summary>
        /// 開始（メイン画面：最小化、音声認識：起動）
        /// </summary>
        StartupMinimize,

        /// <summary>
        /// 開始（メイン画面：表示、　音声認識：起動）
        /// </summary>
        StartupNormal,
    }

    /// <summary>
    /// 入力モード（音声認識した文字をそのまま入力するモード）
    /// </summary>
    public enum InputMode
    {
        /// <summary>
        /// 入力モード：Off
        /// </summary>
        Off,

        /// <summary>
        /// 入力モード：On
        /// （<see cref="OnEnter"/>と異なり入力後にEnterキーの押下はなし）
        /// </summary>
        On,

        /// <summary>
        /// 入力モード：On
        /// （音声認識文字を入力した後にEnterキーを押下するモード）
        /// </summary>
        OnEnter,
    }

    /// <summary>
    /// 設定情報を扱うクラス
    /// </summary>
    [TypeConverter(typeof(LocalizableConverter<Properties.Resources>))]
    public class Setting
    {
        #region クラス変数・定数

        /// <summary>
        /// ユーザコンフィグのデフォルトの設定情報
        /// </summary>
        private static IReadOnlyDictionary<string, string> _defaultUserSettings = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        /// <exception cref="SettingException">
        /// 下記の場合に発生
        /// ・引数で指定された名称のプロパティが設定情報（<see cref="Properties.Settings"/>）に存在しない場合
        /// ・デフォルト値を使用する場合において、デフォルト値属性が存在しない場合
        /// ・デフォルト値を使用する場合において、デフォルト値が指定のEnum型に変換できない場合
        /// </exception>
        public Setting()
        {
            // アプリケーション起動の際の起動モード
            StartStartupMode
                = GetSettingPropertyValue<StartupMode>(nameof(Properties.Settings.Default.StartStartupMode));

            // アプリケーション起動の際の入力モード
            StartInputMode
                = GetSettingPropertyValue<InputMode>(nameof(Properties.Settings.Default.StartInputMode));

            // 音声認識で使用するChromeウィンドウのサイズ
            ChromeSizePoint = new SizePoint(Properties.Settings.Default.ChromeSizePoint);

            // キー入力 1～7 の入力キーの設定
            InputKey1 = new InputKey(Properties.Settings.Default.InputKey1);
            InputKey2 = new InputKey(Properties.Settings.Default.InputKey2);
            InputKey3 = new InputKey(Properties.Settings.Default.InputKey3);
            InputKey4 = new InputKey(Properties.Settings.Default.InputKey4);
            InputKey5 = new InputKey(Properties.Settings.Default.InputKey5);
            InputKey6 = new InputKey(Properties.Settings.Default.InputKey6);
            InputKey7 = new InputKey(Properties.Settings.Default.InputKey7);

            // 起動する対象のパス 1～6 の設定
            StartTargetPath1 = Properties.Settings.Default.StartTargetPath1;
            StartTargetPath2 = Properties.Settings.Default.StartTargetPath2;
            StartTargetPath3 = Properties.Settings.Default.StartTargetPath3;
            StartTargetPath4 = Properties.Settings.Default.StartTargetPath4;
            StartTargetPath5 = Properties.Settings.Default.StartTargetPath5;
            StartTargetPath6 = Properties.Settings.Default.StartTargetPath6;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// アプリケーション起動の際の起動モードを取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [DefaultValue(StartupMode.Setting)]
        public StartupMode StartStartupMode { get; set; }

        /// <summary>
        /// アプリケーション起動の際の起動モードを<see cref="string"/>型で取得する
        /// </summary>
        [ReadOnly(true)]
        [Browsable(false)]
        public string StartStartupModeString
        {
            get
            {
                return Enum.GetName(typeof(StartupMode), StartStartupMode);
            }
        }

        /// <summary>
        /// アプリケーション起動の際の入力モードを取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [DefaultValue(InputMode.Off)]
        public InputMode StartInputMode { get; set; }

        /// <summary>
        /// アプリケーション起動の際の入力モードを<see cref="string"/>型でを取得する
        /// </summary>
        [ReadOnly(true)]
        [Browsable(false)]
        public string StartInputModeString
        {
            get
            {
                return Enum.GetName(typeof(InputMode), StartInputMode);
            }
        }

        /// <summary>
        /// 音声認識で使用するChromeウィンドウのサイズと位置を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        public SizePoint ChromeSizePoint { get; set; }

        /// <summary>
        /// キー入力 1 の入力キーの設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        public InputKey InputKey1 { get; set; }

        /// <summary>
        /// キー入力 2 の入力キーの設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        public InputKey InputKey2 { get; set; }

        /// <summary>
        /// キー入力 3 の入力キーの設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        public InputKey InputKey3 { get; set; }

        /// <summary>
        /// キー入力 4 の入力キーの設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        public InputKey InputKey4 { get; set; }

        /// <summary>
        /// キー入力 5 の入力キーの設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        public InputKey InputKey5 { get; set; }

        /// <summary>
        /// キー入力 6 の入力キーの設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        public InputKey InputKey6 { get; set; }

        /// <summary>
        /// キー入力 7 の入力キーの設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        public InputKey InputKey7 { get; set; }

        /// <summary>
        /// 起動する対象のパス 1 の設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [DefaultValue(typeof(string), "")]
        public string StartTargetPath1 { get; set; }

        /// <summary>
        /// 起動する対象のパス 2 の設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [DefaultValue(typeof(string), "")]
        public string StartTargetPath2 { get; set; }

        /// <summary>
        /// 起動する対象のパス 3 の設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [DefaultValue(typeof(string), "")]
        public string StartTargetPath3 { get; set; }

        /// <summary>
        /// 起動する対象のパス 4 の設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [DefaultValue(typeof(string), "")]
        public string StartTargetPath4 { get; set; }

        /// <summary>
        /// 起動する対象のパス 5 の設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [DefaultValue(typeof(string), "")]
        public string StartTargetPath5 { get; set; }

        /// <summary>
        /// 起動する対象のパス 6 の設定を取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [DefaultValue(typeof(string), "")]
        public string StartTargetPath6 { get; set; }

        /// <summary>
        /// ユーザコンフィグのデフォルトの設定情報を取得する
        /// </summary>
        /// <remarks>
        /// シングルトンパターンで実装
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        private static IReadOnlyDictionary<string, string> DefaultUserSettings
        {
            get
            {
                if (_defaultUserSettings == null)
                {
                    SettingsContext context = Properties.Settings.Default.Context;
                    _defaultUserSettings
                        = new ReadOnlyDictionary<string, string>(UserSettingsProvider.GetDefaultUserSettings(context));
                }

                return _defaultUserSettings;
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 設定情報（<see cref="Properties.Settings"/>）から、引数のプロパティの名称（<paramref name="propertyName"/>）に該当する
        /// プロパティの値を<typeparamref name="T"/>のEnum型で取得する
        /// </summary>
        /// <typeparam name="T">取得対象のプロパティに該当するEnumの型</typeparam>
        /// <param name="propertyName">プロパティの名称</param>
        /// <exception cref="ArgumentNullException">
        /// 引数のプロパティの名称（<paramref name="propertyName"/>）がNULLの場合に発生
        /// </exception>
        /// <exception cref="SettingException">
        /// 下記の場合に発生
        /// ・引数で指定された名称のプロパティが設定情報（<see cref="Properties.Settings"/>）に存在しない場合
        /// ・デフォルト値を使用する場合において、デフォルト値属性が存在しない場合
        /// ・デフォルト値を使用する場合において、デフォルト値が指定のEnum型に変換できない場合
        /// </exception>
        /// <returns>
        /// <typeparamref name="T"/>のEnum型のプロパティの設定値、取得できない場合はデフォルト値を返却
        /// </returns>
        private static T GetSettingPropertyValue<T>(string propertyName)
            where T : struct
        {
            // NULLチェック
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            // 引数に該当するプロパティの値を取得
            string value = GetSettingProperty(propertyName).GetValue(Properties.Settings.Default)?.ToString();

            // 該当するEnumが存在するかチェック
            if (string.IsNullOrEmpty(value)
                || !Enum.TryParse(value, out T enumValue))
            {
                // 該当するEnumが存在しない場合、デフォルト値を返却
                // デフォルト値を取得
                string defaultValue = GetSettingPropertyDefaultValue(propertyName);

                // デフォルト値に該当するEnumが存在するかチェック
                if (!string.IsNullOrEmpty(defaultValue)
                    && Enum.TryParse(defaultValue, out T defaultEnumValue))
                {
                    // デフォルト値に該当するEnumが存在する場合、デフォルトの起動モードを返却
                    return defaultEnumValue;
                }
                else
                {
                    // デフォルト値に該当するEnumが存在しない場合、例外をスローする
                    throw new SettingException(string.Format(
                        CultureInfo.InvariantCulture,
                        Properties.Resources.SettingErrorMessageInvalidPropertyValue,
                        propertyName,
                        defaultValue ?? "null"));
                }
            }

            // 該当するEnumが存在する場合、該当する値を返却
            return enumValue;
        }

        /// <summary>
        /// 設定情報（<see cref="Properties.Settings"/>）から、
        /// 引数のプロパティの名称（<paramref name="propertyName"/>）に該当するプロパティのデフォルト値を取得する
        /// </summary>
        /// <param name="propertyName">プロパティの名称</param>
        /// <exception cref="ArgumentNullException">
        /// 引数のプロパティの名称（<paramref name="propertyName"/>）がNULLの場合に発生
        /// </exception>
        /// <exception cref="SettingException">
        /// 引数で指定された名称のプロパティが設定情報（<see cref="Properties.Settings"/>）に存在しない、
        /// または デフォルト値属性が存在しない場合に発生
        /// </exception>
        /// <returns>
        /// 引数のプロパティの名称（<paramref name="propertyName"/>）に該当する
        /// 設定情報（<see cref="Properties.Settings"/>）で定義されたプロパティのデフォルト値
        /// </returns>
        private static string GetSettingPropertyDefaultValue(string propertyName)
        {
            // NULLチェック
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            // 対象のプロパティの属性情報を取得する
            AttributeCollection attributes = GetSettingProperty(propertyName).Attributes;

            // 取得した属性除法情報からデフォルト値属性を取得
            DefaultSettingValueAttribute defaultValueAttribute
                = attributes == null ? null : attributes[typeof(DefaultSettingValueAttribute)] as DefaultSettingValueAttribute;
            if (defaultValueAttribute == null)
            {
                throw new SettingException(string.Format(
                    CultureInfo.InvariantCulture,
                    Properties.Resources.SettingErrorMessagePropertyNotHaveDefaultAttribute,
                    propertyName));
            }

            // デフォルト値から値を取得して返却
            return defaultValueAttribute.Value?.ToString();
        }

        /// <summary>
        /// 設定情報（<see cref="Properties.Settings"/>）から、
        /// 引数のプロパティの名称（<paramref name="propertyName"/>）に指定されたプロパティを取得する
        /// </summary>
        /// <param name="propertyName">取得対象のプロパティの名称</param>
        /// <exception cref="ArgumentNullException">
        /// 引数のプロパティの名称（<paramref name="propertyName"/>）がNULLの場合に発生
        /// </exception>
        /// <exception cref="SettingException">
        /// 引数で指定された名称のプロパティが設定情報（<see cref="Properties.Settings"/>）に存在しない場合に発生
        /// </exception>
        /// <returns>
        /// 引数のプロパティの名称（<paramref name="propertyName"/>）に該当する
        /// 設定情報（<see cref="Properties.Settings"/>）のプロパティ
        /// </returns>
        private static PropertyDescriptor GetSettingProperty(string propertyName)
        {
            // NULLチェック
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            // 対象のプロパティの属性情報を取得する
            PropertyDescriptor property
                = TypeDescriptor.GetProperties(Properties.Settings.Default)?.Find(propertyName, false);
            if (property == null)
            {
                // 取得できない場合は例外をスローする
                throw new SettingException(string.Format(
                    CultureInfo.InvariantCulture,
                    Properties.Resources.SettingErrorMessageNoTargetProperty,
                    propertyName));
            }

            // 取得したプロパティを返却する
            return property;
        }

        /// <summary>
        /// 音声認識で使用するChromeウィンドウのサイズ、位置の設定が既定値か判定する
        /// </summary>
        /// <returns>既定値でない場合：True、既定値の場合：False</returns>
        private bool ShouldSerializeChromeSizePoint()
        {
            string name = nameof(Properties.Settings.Default.ChromeSizePoint);
            string defaultValue = DefaultUserSettings[name];
            return !defaultValue.EqualsWithNull(ChromeSizePoint?.ConvertToString(ChromeSizePoint));
        }

        /// <summary>
        /// 音声認識で使用するChromeウィンドウのサイズ、位置の設定を既定値にリセットする
        /// </summary>
        private void ResetChromeSizePoint()
        {
            string name = nameof(Properties.Settings.Default.ChromeSizePoint);
            string defaultValue = DefaultUserSettings[name];
            ChromeSizePoint = new SizePoint(defaultValue);
        }

        /// <summary>
        /// キー入力 1 の入力キーの設定が既定値か判定する
        /// </summary>
        /// <returns>既定値でない場合：True、既定値の場合：False</returns>
        private bool ShouldSerializeInputKey1()
        {
            string name = nameof(Properties.Settings.Default.InputKey1);
            string defaultValue = DefaultUserSettings[name];
            return !defaultValue.EqualsWithNull(InputKey1?.ConvertToString(InputKey1));
        }

        /// <summary>
        /// キー入力 1 の入力キーの設定を既定値にリセットする
        /// </summary>
        private void ResetInputKey1()
        {
            string name = nameof(Properties.Settings.Default.InputKey1);
            string defaultValue = DefaultUserSettings[name];
            InputKey1 = new InputKey(defaultValue);
        }

        /// <summary>
        /// キー入力 1 の入力キーの設定が既定値か判定する
        /// </summary>
        /// <returns>既定値でない場合：True、既定値の場合：False</returns>
        private bool ShouldSerializeInputKey2()
        {
            string name = nameof(Properties.Settings.Default.InputKey2);
            string defaultValue = DefaultUserSettings[name];
            return !defaultValue.EqualsWithNull(InputKey2?.ConvertToString(InputKey2));
        }

        /// <summary>
        /// キー入力 2 の入力キーの設定を既定値にリセットする
        /// </summary>
        private void ResetInputKey2()
        {
            string name = nameof(Properties.Settings.Default.InputKey2);
            string defaultValue = DefaultUserSettings[name];
            InputKey2 = new InputKey(defaultValue);
        }

        /// <summary>
        /// キー入力 3 の入力キーの設定が既定値か判定する
        /// </summary>
        /// <returns>既定値でない場合：True、既定値の場合：False</returns>
        private bool ShouldSerializeInputKey3()
        {
            string name = nameof(Properties.Settings.Default.InputKey3);
            string defaultValue = DefaultUserSettings[name];
            return !defaultValue.EqualsWithNull(InputKey3?.ConvertToString(InputKey3));
        }

        /// <summary>
        /// キー入力 3 の入力キーの設定を既定値にリセットする
        /// </summary>
        private void ResetInputKey3()
        {
            string name = nameof(Properties.Settings.Default.InputKey3);
            string defaultValue = DefaultUserSettings[name];
            InputKey3 = new InputKey(defaultValue);
        }

        /// <summary>
        /// キー入力 4 の入力キーの設定が既定値か判定する
        /// </summary>
        /// <returns>既定値でない場合：True、既定値の場合：False</returns>
        private bool ShouldSerializeInputKey4()
        {
            string name = nameof(Properties.Settings.Default.InputKey4);
            string defaultValue = DefaultUserSettings[name];
            return !defaultValue.EqualsWithNull(InputKey4?.ConvertToString(InputKey4));
        }

        /// <summary>
        /// キー入力 4 の入力キーの設定を既定値にリセットする
        /// </summary>
        private void ResetInputKey4()
        {
            string name = nameof(Properties.Settings.Default.InputKey4);
            string defaultValue = DefaultUserSettings[name];
            InputKey4 = new InputKey(defaultValue);
        }

        /// <summary>
        /// キー入力 5 の入力キーの設定が既定値か判定する
        /// </summary>
        /// <returns>既定値でない場合：True、既定値の場合：False</returns>
        private bool ShouldSerializeInputKey5()
        {
            string name = nameof(Properties.Settings.Default.InputKey5);
            string defaultValue = DefaultUserSettings[name];
            return !defaultValue.EqualsWithNull(InputKey5?.ConvertToString(InputKey5));
        }

        /// <summary>
        /// キー入力 5 の入力キーの設定を既定値にリセットする
        /// </summary>
        private void ResetInputKey5()
        {
            string name = nameof(Properties.Settings.Default.InputKey5);
            string defaultValue = DefaultUserSettings[name];
            InputKey5 = new InputKey(defaultValue);
        }

        /// <summary>
        /// キー入力 6 の入力キーの設定が既定値か判定する
        /// </summary>
        /// <returns>既定値でない場合：True、既定値の場合：False</returns>
        private bool ShouldSerializeInputKey6()
        {
            string name = nameof(Properties.Settings.Default.InputKey6);
            string defaultValue = DefaultUserSettings[name];
            return !defaultValue.EqualsWithNull(InputKey6?.ConvertToString(InputKey6));
        }

        /// <summary>
        /// キー入力 6 の入力キーの設定を既定値にリセットする
        /// </summary>
        private void ResetInputKey6()
        {
            string name = nameof(Properties.Settings.Default.InputKey6);
            string defaultValue = DefaultUserSettings[name];
            InputKey6 = new InputKey(defaultValue);
        }

        /// <summary>
        /// キー入力 7 の入力キーの設定が既定値か判定する
        /// </summary>
        /// <returns>既定値でない場合：True、既定値の場合：False</returns>
        private bool ShouldSerializeInputKey7()
        {
            string name = nameof(Properties.Settings.Default.InputKey7);
            string defaultValue = DefaultUserSettings[name];
            return !defaultValue.EqualsWithNull(InputKey7?.ConvertToString(InputKey7));
        }

        /// <summary>
        /// キー入力 7 の入力キーの設定を既定値にリセットする
        /// </summary>
        private void ResetInputKey7()
        {
            string name = nameof(Properties.Settings.Default.InputKey7);
            string defaultValue = DefaultUserSettings[name];
            InputKey7 = new InputKey(defaultValue);
        }

        #endregion
    }
}
