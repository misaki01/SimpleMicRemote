namespace SimpleMicRemote
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    using MisaCommon.CustomType;
    using MisaCommon.Modules;
    using MisaCommon.Utility.Win32Api;

    /// <summary>
    /// 設定用のパネルコントロールを扱うクラス
    /// コンストラクタの生成は<see cref="SimpleForm"/>のコンストラクタで行うこと
    /// </summary>
    public class SettingPanel
    {
        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// 各プロパティの値を初期化する
        /// </summary>
        /// <param name="panel">
        /// 設定用のパネルコントロール
        /// </param>
        /// <param name="getPropertySetting">
        /// 紐づくプロパティ設定から値を取得するための処理
        /// プロパティ設定は不要な項目の場合はNULLを指定する
        /// ・戻り値：プロパティ設定の設定値
        /// </param>
        /// <param name="getPropertySummaryText">
        /// 紐づくプロパティ設定の設定値の概要文字を取得する（概要文字はラベルに表示する）
        /// プロパティ設定は不要な項目の場合はNULLを指定する
        /// ・引数１：プロパティ設定の設定値
        /// ・戻り値：プロパティ設定の設定値から生成したラベルに表示する文字列
        /// </param>
        /// <param name="checkPropertySetting">
        /// 紐づくプロパティ設定が正しいか判定するための処理
        /// プロパティ設定は不要な項目の場合はNULLを指定する
        /// ・引数１：プロパティ設定の設定値
        /// ・引数２：プロパティ設定からラベルに表示する文字を取得する処理
        /// 　　　　（引数１：プロパティ設定の設定値、戻り値：ラベルに表示する文字列）
        /// ・戻り値：プロパティ設定のステータス（<see cref="PropertySettingStatus"/>）
        /// </param>
        /// <param name="processToMatching">
        /// マッチングした際に実行する処理
        /// ・引数１：プロパティ設定の設定値
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 下記の引数がNULLの場合に発生
        /// ・設定用のパネルコントロール（<paramref name="panel"/>）
        /// ・マッチングした際に実行する処理（<paramref name="processToMatching"/>）
        /// </exception>
        public SettingPanel(
            Panel panel,
            Func<string> getPropertySetting,
            Func<string, string> getPropertySummaryText,
            Func<string, PropertySettingStatus> checkPropertySetting,
            Action<string> processToMatching)
        {
            // NULLチェック
            if (panel == null)
            {
                throw new ArgumentNullException(nameof(panel));
            }
            else if (processToMatching == null)
            {
                throw new ArgumentNullException(nameof(processToMatching));
            }

            // パネル及びパネルの子コントロールのラベル、テキストボックスを取得し設定する
            Panel = panel;
            foreach (Control control in Panel.Controls)
            {
                if (Label == null && control is Label label)
                {
                    Label = label;
                }
                else if (TextBox == null && control is TextBox textBox)
                {
                    TextBox = textBox;
                }

                if (Label != null && TextBox != null)
                {
                    break;
                }
            }

            // ラベルのデフォルト値を保持する
            DefaultLabelText = Label.Text;

            // 紐づくプロパティ設定から値を取得するための処理
            // 引数の値がNULLの場合（プロパティ設定を使用しない場合）はデフォルトの処理を設定する
            GetPropertySettingFunc = getPropertySetting ?? (() => null);

            // 紐づくプロパティ設定の設定値の概要文字を取得する（概要文字はラベルに表示する）
            // 引数の値がNULLの場合（プロパティ設定を使用しない場合）はデフォルトの処理を設定する
            GetPropertySummaryText = getPropertySummaryText ?? ((property) => null);

            // 紐づくプロパティ設定が正しいか判定するための処理
            // 引数の値がNULLの場合（プロパティ設定を使用しない場合）はデフォルトの処理を設定する
            CheckPropertySetting = checkPropertySetting ?? ((property) => PropertySettingStatus.Ok);

            // マッチングした際に実行する処理
            ProcessToMatchingAction = processToMatching;
        }

        #endregion

        #region Enum定義

        /// <summary>
        /// プロパティ設定のステータス
        /// </summary>
        public enum PropertySettingStatus
        {
            /// <summary>
            /// 正常
            /// </summary>
            Ok,

            /// <summary>
            /// 未設定
            /// </summary>
            NotSet,

            /// <summary>
            /// エラー
            /// </summary>
            Error,
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 設定用のパネルコントロールのステータスが[OK]の時のラベルの文字色
        /// </summary>
        public static Color ColorSettingPanelStatusOk { get; set; } = SystemColors.ControlText;

        /// <summary>
        /// 設定用のパネルコントロールのステータスが[NotSet]の時のラベルの文字色
        /// </summary>
        public static Color ColorSettingPanelStatusNotSet { get; set; } = Color.Gray;

        /// <summary>
        /// 設定用のパネルコントロールのステータスが[Error]の時のラベルの文字色
        /// </summary>
        public static Color ColorSettingPanelStatusError { get; set; } = Color.Red;

        /// <summary>
        /// 設定用のパネルコントロールのデフォルトの背景色
        /// </summary>
        public static Color ColorSettingPanelDefault { get; set; } = SystemColors.Control;

        /// <summary>
        /// 設定用のパネルコントロールの使用中の時の背景色
        /// </summary>
        public static Color ColorSettingPanelInUse { get; set; } = Color.FromArgb(192, 255, 192);

        /// <summary>
        /// マッチングメッセージ
        /// テキストボックスが入力されていない、プロパティ設定が不正な場合はNULL
        /// </summary>
        public string MatchMessage =>

                // ※ Statusプロパティの画面設定処理を実行するためStatusのチェック先に行う
                GetStatus() == PropertySettingStatus.Ok && !string.IsNullOrWhiteSpace(TextBox.Text)
                    ? TextBox.Text : null;

        /// <summary>
        /// 紐づくプロパティ設定から値を取得するための処理を取得する
        /// 戻り値：プロパティ設定の設定値
        /// </summary>
        public Func<string> GetPropertySettingFunc { get; }

        /// <summary>
        /// マッチングした際に実行する処理を取得する
        /// 引数１：プロパティ設定の設定値
        /// </summary>
        public Action<string> ProcessToMatchingAction { get; }

        /// <summary>
        /// 設定用のパネルコントロールを取得する
        /// </summary>
        private Panel Panel { get; }

        /// <summary>
        /// 設定用のパネルに配置されているラベルを取得する
        /// </summary>
        private Label Label { get; }

        /// <summary>
        /// 設定用のパネルに配置されているテキストボックスを取得する
        /// </summary>
        private TextBox TextBox { get; }

        /// <summary>
        /// 設定用のパネルに配置されているラベルのデフォルトテキストを取得する
        /// </summary>
        private string DefaultLabelText { get; }

        /// <summary>
        /// 紐づくプロパティ設定の設定値の概要文字を取得する（概要文字はラベルに表示する）
        /// 引数１：プロパティ設定の設定値
        /// 戻り値：プロパティ設定の設定値から生成したラベルに表示する文字列
        /// </summary>
        private Func<string, string> GetPropertySummaryText { get; }

        /// <summary>
        /// 紐づくプロパティ設定が正しいか判定するための処理を取得する
        /// 引数１：プロパティ設定の設定値
        /// 引数２：プロパティ設定からラベルに表示する文字を取得する処理
        /// 　　　（引数１：プロパティ設定の設定値、戻り値：ラベルに表示する文字列）
        /// 戻り値：プロパティ設定のステータス（<see cref="PropertySettingStatus"/>）
        /// </summary>
        private Func<string, PropertySettingStatus> CheckPropertySetting { get; }

        #endregion

        #region static メソッド 設定用のパネルコントロールに関するクラスのインスタンス生成処理

        /// <summary>
        /// 入力キーに該当する設定用のパネルコントロールに関するクラスのインスタンスを生成する
        /// </summary>
        /// <param name="inputKeyPanel">入力キーに該当する設定用のパネルコントロール</param>
        /// <param name="setting">プロパティグリッドコントロールに関連付けされている設定情報</param>
        /// <param name="propertyName">対象とする入力キーに該当する設定情報のプロパティ名</param>
        /// <returns>入力キーに該当する設定用のパネルコントロールに関するクラスのインスタンス</returns>
        public static SettingPanel CreateInstanceKeyInput(
            Panel inputKeyPanel, Setting setting, string propertyName)
        {
            return new SettingPanel(
                inputKeyPanel,
                () =>
                {
                    InputKey inputKey = GetSettingPropertyValue<InputKey>(setting, propertyName);
                    return inputKey.ConvertToString(inputKey);
                },
                (property) => summaryText(new InputKey(property)),
                (property) => CheckKeyInputProperty(property),
                (property) => InputOperate.MouseKeybordInput(new InputKey(property)));

            // ラベルに表示するための概要文字列を取得するローカル関数
            // 引数１：概要文字を生成するInputKeyオブジェクト
            // 戻り値：概要文字
            string summaryText(InputKey inputKey)
            {
                // 文字列に変換し返却
                StringBuilder convertValue = new StringBuilder();

                // Shiftキー（最初の１文字のみ）
                if (inputKey.Shift)
                {
                    convertValue.Append(nameof(inputKey.Shift).Substring(0, 1));
                    convertValue.Append("|");
                }

                // Ctrlキー（最初の１文字のみ）
                if (inputKey.Ctrl)
                {
                    convertValue.Append(nameof(inputKey.Ctrl).Substring(0, 1));
                    convertValue.Append("|");
                }

                // Altキー（最初の１文字のみ）
                if (inputKey.Alt)
                {
                    convertValue.Append(nameof(inputKey.Alt).Substring(0, 1));
                    convertValue.Append("|");
                }

                // Windowsロゴキー（最初の１文字のみ）
                if (inputKey.Win)
                {
                    convertValue.Append(nameof(inputKey.Win).Substring(0, 1));
                    convertValue.Append("|");
                }

                // キーコード
                convertValue.Append(
                    new InputKey(inputKey.KeyCode, false, false, false, false, false).ToString());

                // 押しっぱなしにするかのフラグ（）
                if (inputKey.IsKeepPressing)
                {
                    convertValue.Append("|");
                    convertValue.Append("凹");
                }

                // 生成した文字列を返却
                return convertValue.ToString();
            }

            // 入力キーに該当する設定情報のプロパティの設定値についてチェックを行うローカル関数
            // 引数１：入力キーに該当する設定情報のプロパティ
            // 戻り値：チェック結果のプロパティ設定のステータス
            PropertySettingStatus CheckKeyInputProperty(string property)
            {
                // 入力キーがなしの場合は未設定を返却、それ以外の場合は正常を返却
                return new InputKey(property).KeyCode == Keys.None
                    ? PropertySettingStatus.NotSet : SettingPanel.PropertySettingStatus.Ok;
            }
        }

        /// <summary>
        /// 起動に該当する設定用のパネルコントロールに関するクラスのインスタンスを生成する
        /// </summary>
        /// <param name="exeStartPanel">起動に該当する設定用のパネルコントロール</param>
        /// <param name="setting">プロパティグリッドコントロールに関連付けされている設定情報</param>
        /// <param name="propertyName">対象とする起動に該当する設定情報のプロパティ名</param>
        /// <returns>起動に該当する設定用のパネルコントロールに関するクラスのインスタンス</returns>
        public static SettingPanel CreateInstanceExeStart(
            Panel exeStartPanel, Setting setting, string propertyName)
        {
            return new SettingPanel(
                exeStartPanel,
                () => GetSettingPropertyValue<string>(setting, propertyName),
                (property) => Path.GetFileName(property),
                (property) => CheckExeStartProperty(property),
                (property) => StartProcess.Start(property));

            // 起動に該当する設定情報のプロパティの設定値についてチェックを行う
            // 引数１：起動に該当する設定情報のプロパティ
            // 戻り値：チェック結果のプロパティ設定のステータス
            PropertySettingStatus CheckExeStartProperty(string property)
            {
                // 起動対象のパスが存在しない場合、未設定を返却
                if (string.IsNullOrWhiteSpace(property))
                {
                    return PropertySettingStatus.NotSet;
                }

                // 起動対象のパスのファイル又はフォルダが存在するかチェック
                // 起動対象のパスのファイル又はフォルダが存在しない場合、エラーを返却
                // （ショートカットの場合は拡張子「.lnk」を付与して判定する）
                if (!File.Exists(property)
                    && !Directory.Exists(property)
                    && !File.Exists(property + ".lnk"))
                {
                    return PropertySettingStatus.Error;
                }

                // 上記チェックがOKの場合、正常を返却
                return PropertySettingStatus.Ok;
            }
        }

        /// <summary>
        /// 引数の設定情報（<paramref name="setting"/>）から引数のプロパティ名
        /// （<paramref name="propertyName"/>）に該当する設定値を<typeparamref name="T"/>型で取得する
        /// </summary>
        /// <typeparam name="T">取得するプロパティの型</typeparam>
        /// <param name="setting">プロパティグリッドコントロールに関連付けされている設定情報</param>
        /// <param name="propertyName">入力キーに該当する設定情報のプロパティ名称</param>
        /// <returns>設定情報プロパティの設定値の文字列</returns>
        public static T GetSettingPropertyValue<T>(Setting setting, string propertyName)
        {
            T value = (T)TypeDescriptor.GetProperties(setting).Find(propertyName, false).GetValue(setting);
            return value;
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 読み取り専用の設定を行う
        /// </summary>
        /// <param name="isReadOnly">読み取り専用にするかどうかのフラグ</param>
        public void SetReadOnly(bool isReadOnly)
        {
            // 読み取り専用の設定
            TextBox.ReadOnly = isReadOnly;

            // 読み取り専用の場合において、
            // マッチングメッセージが設定されている（利用する）かどうかで背景色の設定する
            // ※ MatchMessageプロパティのチェックを先の行うことで、
            // 　 Statusプロパティの画面設定処理を実行している
            Panel.BackColor = MatchMessage != null && isReadOnly
                ? ColorSettingPanelInUse : ColorSettingPanelDefault;
        }

        /// <summary>
        /// 紐づくプロパティ設定のステータスを取得する
        /// </summary>
        /// <returns>紐づくプロパティ設定のステータス</returns>
        private PropertySettingStatus GetStatus()
        {
            // プロパティの設定値を取得
            string property = GetPropertySettingFunc();

            // チェック処理を実行しプロパティ設定のステータスを取得する
            PropertySettingStatus status = CheckPropertySetting(property);

            // ステータスに応じた画面表示用のテキストと文字色を設定する
            string text;
            Color foreColor;
            switch (status)
            {
                case PropertySettingStatus.Ok:
                    // 正常の場合
                    text = DefaultLabelText + GetPropertySummaryText(property);
                    foreColor = ColorSettingPanelStatusOk;
                    break;
                case PropertySettingStatus.NotSet:
                    // 未設定の場合
                    text = DefaultLabelText + Properties.Resources.SimpleFormMatchLabelTextNotSet;
                    foreColor = ColorSettingPanelStatusNotSet;
                    break;
                case PropertySettingStatus.Error:
                default:
                    // エラーの場合（デフォルトもエラーとして扱う）
                    text = DefaultLabelText + Properties.Resources.SimpleFormMatchLabelTextError;
                    foreColor = ColorSettingPanelStatusError;
                    break;
            }

            // ラベルの表示設定を行う
            Label.Text = text;
            Label.ForeColor = foreColor;

            // チェック結果を返却
            return status;
        }

        #endregion
    }
}
