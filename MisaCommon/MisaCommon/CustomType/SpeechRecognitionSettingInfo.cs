namespace MisaCommon.CustomType
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    using MisaCommon.CustomType.Converter;
    using MisaCommon.MessageResources.Type;
    using MisaCommon.UserControls;

    /// <summary>
    /// 音声認識に対する操作設定のタイプ
    /// </summary>
    /// <remarks>
    /// <see cref="SpeechRecognitionSettingInfo.TypeControlMapping"/>にてタイプ毎のユーザコントロールを
    /// マッピングし、<see cref="SpeechRecognitionSettingInfo.SetPropertyForControl(Control)"/>にて、
    /// 各ユーザコントロール毎のプロパティの設定を行う
    /// </remarks>
    [Flags]
    public enum SpeechRecognitionSettingType
    {
        /// <summary>
        /// 操作なし
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// キー入力操作
        /// </summary>
        InputKey = 0x0001,

        /// <summary>
        /// プロセススタート
        /// </summary>
        StartProcess = 0x0002,
    }

    /// <summary>
    /// 音声認識に対するマッチングパターンと操作の設定情報をまとめて扱うクラス
    /// </summary>
    /// <remarks>
    /// このクラスの公開プロパティにおける表示名と説明は、
    /// <see cref="LocalizableTypeConverter{T, TResouces}"/>にてマッピングしている
    /// <list type="bullet">
    /// <item>
    /// <term>string型への変換</term>
    /// <description>
    /// 各パラメータを下記の順番でパイプ区切りの文字列で表現する
    /// １．音声認識に対する操作設定のタイプ
    /// ２．音声認識に対するマッチングメッセージ
    /// ３．音声認識に対するマッチングパターン
    /// ４．設定データ
    /// 　【例1】未設定の場合
    /// 　　　　　⇒　StartProcess|||
    /// 　【例2】プロセススタートの場合
    /// 　　　　　⇒　StartProcess|マッチングメッセージ|exact|c\xxx\xxx\abc.exe
    ///         　　　|0, 0, 1200, 800|500|--abe="xxxx"
    /// 　　　　　　　※３個目のパイプ以降は設定データとなる
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    [Serializable]
    [TypeConverter(typeof(LocalizableTypeConverter<
        SpeechRecognitionSettingInfo, SpeechRecognitionSettingInfoPropertyMessage>))]
    public class SpeechRecognitionSettingInfo : ITypeConvertable<SpeechRecognitionSettingInfo>
    {
        /// <summary>
        /// <see cref="SpeechRecognitionSettingType"/>に対応するユーザコントロールとのマッピング定義
        /// </summary>
        private static readonly IReadOnlyDictionary<SpeechRecognitionSettingType, Type> TypeControlMapping
            = new ReadOnlyDictionary<SpeechRecognitionSettingType, Type>(
                new Dictionary<SpeechRecognitionSettingType, Type>()
                {
                    { SpeechRecognitionSettingType.None, null },
                    { SpeechRecognitionSettingType.InputKey, typeof(KeyInputSetting) },
                    { SpeechRecognitionSettingType.StartProcess, typeof(StartProcessSetting) },
                });

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="settingType">音声認識に対する操作の設定のタイプ</param>
        /// <param name="matchMessage">音声認識に対するマッチングメッセージ</param>
        /// <param name="matchPattern">音声認識に対するマッチングパターン（ラジオボタンのタグデータ）</param>
        /// <param name="settingData">設定データ</param>
        public SpeechRecognitionSettingInfo(
            SpeechRecognitionSettingType settingType,
            string matchMessage,
            string matchPattern,
            string settingData)
        {
            SettingType = settingType;
            MatchMessage = matchMessage;
            MatchPattern = matchPattern;
            SettingData = settingData;
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="settingType">音声認識に対する操作の設定のタイプ</param>
        public SpeechRecognitionSettingInfo(SpeechRecognitionSettingType settingType)
            : this(settingType, null, null, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        public SpeechRecognitionSettingInfo()
            : this(SpeechRecognitionSettingType.None, null, null, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="data">このクラスに変換可能な文字列</param>
        public SpeechRecognitionSettingInfo(string data)
        {
            SpeechRecognitionSettingInfo input = ConvertFromString(data);
            SettingType = input.SettingType;
            MatchMessage = input.MatchMessage;
            MatchPattern = input.MatchPattern;
            SettingData = input.SettingData;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 音声認識に対する操作設定のタイプを取得・設定する
        /// </summary>
        [DefaultValue(SpeechRecognitionSettingType.None)]
        public SpeechRecognitionSettingType SettingType { get; set; }

        /// <summary>
        /// 音声認識に対するマッチングメッセージを取得・設定する
        /// </summary>
        [DefaultValue(null)]
        public string MatchMessage { get; set; }

        /// <summary>
        /// 音声認識に対するマッチングパターン（ラジオボタンのタグデータ）を取得・設定する
        /// </summary>
        [DefaultValue(null)]
        public string MatchPattern { get; set; }

        /// <summary>
        /// 設定データを取得・設定する
        /// </summary>
        [DefaultValue(null)]
        public string SettingData { get; set; }

        /// <summary>
        /// 設定データの概要を示すテキストを取得する
        /// 設定データが未設定の場合はNULL
        /// </summary>
        [ReadOnly(true)]
        [Browsable(false)]
        public string SummaryText
        {
            get
            {
                // 設定データが存在しない場合はNULLを返却
                if (string.IsNullOrEmpty(SettingData))
                {
                    return null;
                }

                // ユーザコントロールのインスタンスを生成し概要のテキストを取得、返却する
                ISpeechRecognitionSettingControl control = GetSettingControlInstance(SettingType);
                return control?.GetSummaryText(SettingData);
            }
        }

        /// <summary>
        /// 音声認識に対する操作設定のタイプに紐づく設定用のコントロールを取得する
        /// <see cref="SpeechRecognitionSettingType.None"/>の場合はNULLを返却
        /// </summary>
        [ReadOnly(true)]
        [Browsable(false)]
        public Control SettingControl
        {
            get
            {
                // ユーザコントロールのインスタンスを生成、初期化を行う
                ISpeechRecognitionSettingControl settingControl = GetSettingControlInstance(SettingType);
                Control control = settingControl?.GetInitializeControl(SettingData);

                // 生成したユーザコントロールのプロパティ設定を行う
                SetPropertyForControl(control);

                // 生成したユーザコントロールを返却する
                return control;
            }
        }

        #endregion

        #region メソッド

        #region ITypeConvertableの実装

        /// <summary>
        /// 文字列を <see cref="SpeechRecognitionSettingInfo"/> クラスのインスタンスに変換する
        /// </summary>
        /// <param name="value">
        /// <see cref="SpeechRecognitionSettingInfo"/> クラスのインスタンスに変換する文字列
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>文字列から生成した<see cref="SpeechRecognitionSettingInfo"/> クラスのインスタンス</returns>
        public SpeechRecognitionSettingInfo ConvertFromString(string value)
        {
            // NULLチェック
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // パイプ区切りでSplitして各パラメータを取得する
            string[] param = value.Split('|');

            // 音声認識に対する操作設定のタイプ
            SpeechRecognitionSettingType settingType
                = !string.IsNullOrWhiteSpace(param[0])
                && Enum.TryParse(param[0].Trim(), out SpeechRecognitionSettingType tmpType)
                ? tmpType : SpeechRecognitionSettingType.None;

            // 音声認識に対するマッチングメッセージ
            string matchMessage = param.Length > 1 && !string.IsNullOrWhiteSpace(param[1])
                ? param[1].Trim() : null;

            // 音声認識に対するマッチングパターン
            string matchPattern = param.Length > 2 && !string.IsNullOrWhiteSpace(param[2])
                ? param[2].Trim() : null;

            // 設定データ
            string settingData = param.Length > 3 && !string.IsNullOrWhiteSpace(param[3])
                ? value.Substring(param[0].Length + param[1].Length + param[2].Length + 3)
                : null;

            // 取得した各プロパティの値から当クラスのインスタンスを生成し返却
            return new SpeechRecognitionSettingInfo(
                settingType: settingType,
                matchMessage: matchMessage,
                matchPattern: matchPattern,
                settingData: string.IsNullOrWhiteSpace(settingData) ? null : settingData);
        }

        /// <summary>
        /// <see cref="SpeechRecognitionSettingInfo"/> クラスのインスタンスを文字列に変換する
        /// </summary>
        /// <param name="value">
        /// 文字列に変換する <see cref="SpeechRecognitionSettingInfo"/> クラスのインスタンス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>変換した文字列</returns>
        public string ConvertToString(SpeechRecognitionSettingInfo value)
        {
            // 引数の型チェック
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // このクラスにおいて、ToStringを実装しているため、
            // その機能を利用し文字列に変換する
            return value.ToString();
        }

        #endregion

        /// <summary>
        /// このクラスのインスタンスの複製を生成する
        /// </summary>
        /// <returns>
        /// このクラスのインスタンスのコピーである新しいインスタンス
        /// </returns>
        public SpeechRecognitionSettingInfo DeepCopy()
        {
            return new SpeechRecognitionSettingInfo(
                settingType: SettingType,
                matchMessage: MatchMessage,
                matchPattern: MatchPattern,
                settingData: SettingData);
        }

        /// <summary>
        /// このインスタンスの値を <see cref="string"/> に変換する
        /// </summary>
        /// <returns>このインスタンスと同じ値の文字列</returns>
        public new string ToString()
        {
            // 文字列に変換し返却
            StringBuilder convertValue = new StringBuilder();

            // 音声認識に対する操作設定のタイプ
            convertValue.Append(Enum.GetName(typeof(SpeechRecognitionSettingType), SettingType));
            convertValue.Append("|");

            // 音声認識に対するマッチングメッセージ
            convertValue.Append(string.IsNullOrEmpty(MatchMessage) ? string.Empty : MatchMessage);
            convertValue.Append("|");

            // 音声認識に対するマッチングパターン
            convertValue.Append(string.IsNullOrEmpty(MatchPattern) ? string.Empty : MatchPattern);
            convertValue.Append("|");

            // 設定データ
            convertValue.Append(string.IsNullOrEmpty(SettingData) ? string.Empty : SettingData);

            return convertValue.ToString();
        }

        /// <summary>
        /// 引数の音声認識に対する操作設定のタイプ（<paramref name="settingType"/>）に紐づく
        /// 音声認識に対するマッチングパターンと操作を設定するためのユーザコントロールを取得する
        /// </summary>
        /// <param name="settingType">音声認識に対する操作設定のタイプ</param>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// ・引数の型（<paramref name="settingType"/>）が RuntimeType でない場合
        /// ・オープンジェネリック型の場合
        /// 　（<see cref="Type.ContainsGenericParameters"/>のプロパティがTrueの場合）
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// 引数の型（<paramref name="settingType"/>）を <see cref="TypeBuilder"/> にすることができない場合に発生
        /// 具体的には下記の場合が該当する
        /// ・引数のタイプ（<paramref name="settingType"/>）のアセンブリが <see cref="AssemblyBuilderAccess.Save"/>
        /// 　　を使用して作成された動的アセンブリの場合
        /// ・下記のサポートされていない型に該当する場合
        /// 　1. <see cref="TypedReference"/>型
        /// 　2. <see cref="ArgIterator"/>型
        /// 　3. <see cref="void"/>型
        /// 　4. <see cref="RuntimeArgumentHandle"/>型
        /// 　5. 上記型の配列型
        /// </exception>
        /// <exception cref="MemberAccessException">
        /// 下記の場合に発生
        /// ・音声認識に対する操作設定コントロールに引数なしパブリックコンストラクターが存在しない場合
        /// 　[<see cref="MissingMethodException"/>]
        /// ・呼び出し元に音声認識に対する操作設定コントロールの引数なしのコンストラクタを
        /// 　呼び出すアクセス許可がない場合
        /// 　[<see cref="MethodAccessException"/>]
        /// ・音声認識に対する操作設定コントロールが抽象クラスでインスタンスを作成することができない場合、
        /// 　または、対象のメンバーが遅延バインドメカニズムでの呼び出しの場合
        /// 　[<see cref="MemberAccessException"/>]
        /// </exception>
        /// <exception cref="TypeLoadException">
        /// 音声認識に対する操作設定コントロールが有効な型でない場合に発生
        /// </exception>
        /// <exception cref="COMException">
        /// 音声認識に対する操作設定コントロールが COMオブジェクトの場合において、
        /// 型を取得するために使用されるクラスIDが有効でない
        /// または、識別されたクラスが登録されていない場合に発生
        /// </exception>
        /// <exception cref="InvalidComObjectException">
        /// 音声認識に対する操作設定コントロールが <see cref="Type.GetTypeFromCLSID(Guid)"/>を通じて、
        /// COM型が取得できない場合に発生
        /// </exception>
        /// <exception cref="TargetInvocationException">
        /// 引数の型（<paramref name="settingType"/>）において呼び出されるコンストラクタで、
        /// 例外がスローされた場合に発生
        /// </exception>
        /// <returns>
        /// 引数の音声認識に対する操作設定のタイプ（<paramref name="settingType"/>）に紐づく
        /// 音声認識に対するマッチングパターンと操作を設定するためのユーザコントロールのインスタンス
        /// </returns>
        private static ISpeechRecognitionSettingControl GetSettingControlInstance(
            SpeechRecognitionSettingType settingType)
        {
            // マッピングから操作設定のタイプに紐づくユーザコントロールのタイプを取得する
            Type controlType = TypeControlMapping[settingType];

            // 取得したユーザコントロールのタイプがNULLの場合はNULLを返却
            // （操作設定のタイプがNoneの場合が該当する）
            if (controlType == null)
            {
                return null;
            }

            // ユーザコントロールのタイプからインスタンスを生成する
            // 引数なしのデフォルトコンストラクタを使用してインスタンスを生成し返却する
            return Activator.CreateInstance(controlType) as ISpeechRecognitionSettingControl;
        }

        /// <summary>
        /// 各操作設定用のユーザコントロールに応じたプロパティの設定を行う
        /// </summary>
        /// <param name="control">設定対象のコントロール</param>
        private static void SetPropertyForControl(Control control)
        {
            // NULLチェック
            if (control == null)
            {
                // NULLの場合はなにもしない
                return;
            }

            // 共通のプロパティ設定
            control.Dock = DockStyle.Top;

            // 「キー入力」コントロールの場合
            if (control is KeyInputSetting keyInputSetting)
            {
                keyInputSetting.EnabledMouse = false;
                keyInputSetting.EnabledSpecialKey = false;
            }
        }

        #endregion
    }
}
