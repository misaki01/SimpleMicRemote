namespace MisaCommon.CustomType
{
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;

    using MisaCommon.CustomType.Converter;
    using MisaCommon.CustomType.UiEditor;
    using MisaCommon.MessageResources.Type;
    using MisaCommon.Utility.StaticMethod;

    /// <summary>
    /// キーボードのキー入力の情報を扱うクラス
    /// </summary>
    /// <remarks>
    /// このクラスの公開プロパティにおける表示名と説明は、
    /// <see cref="LocalizableTypeConverter{T, TResouces}"/>にてマッピングしている
    /// <list type="bullet">
    ///     <item>
    ///         <term>string型への変換</term>
    ///         <description>
    ///         Shift、Ctrl、Alt、Win、キーコード及び押しっぱなしの６つをパイプ区切りの文字列で表現する
    ///         Shift、Ctrl、Alt、Winについては False の場合は出力しない
    ///         キーコードは「16進形式の数値:表示名」の形式で出力する
    ///         【例1】Ctrl+A（押しっぱなし）の場合   ⇒ 「Ctrl|0x41:A|KeepPressing」
    ///         【例2】Shift+Ctrl+Alt+Wi+A の場合     ⇒ 「Shift|Ctrl|Alt|Win|0x41:A」
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Serializable]
    [TypeConverter(typeof(LocalizableTypeConverter<InputKey, InputKeyPropertyMessage>))]
    [Editor(typeof(InputKeyUIEditor), typeof(UITypeEditor))]
    public class InputKey : ITypeConvertable<InputKey>
    {
        /// <summary>
        /// キーを押しっぱなしにする場合であることを示すための文字
        /// <see cref="string"/>型に変換した際に付与する固定文言
        /// </summary>
        private const string _keepPressingText = "KeepPressing";

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数の値で初期化する
        /// </summary>
        /// <param name="keyCode">キーコード</param>
        /// <param name="shift">Shiftキーを押している場合：True、押していない場合：False</param>
        /// <param name="ctrl">Ctrlキーを押している場合：True、押していない場合：False</param>
        /// <param name="alt">Altキーを押している場合：True、押していない場合：False</param>
        /// <param name="win">Windowsロゴキーを押している場合：True、押していない場合：False</param>
        /// <param name="isKeepPressing">キーを押しっぱなしにする場合：True、押しっぱなしにしない場合：False</param>
        public InputKey(Keys keyCode, bool shift, bool ctrl, bool alt, bool win, bool isKeepPressing)
        {
            KeyCode = keyCode;
            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
            Win = win;
            IsKeepPressing = isKeepPressing;
        }

        /// <summary>
        /// コンストラクタ
        /// 初期化する
        /// </summary>
        public InputKey()
            : this(Keys.None, false, false, false, false, false)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の値で初期化する
        /// </summary>
        /// <param name="keyCode">キーコード</param>
        public InputKey(Keys keyCode)
            : this(keyCode, false, false, false, false, false)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数のキーの値からキーコード、Shift、Ctrl、Altの押下を取得し初期化する
        /// （Windowsロゴキー修飾子として存在しないため False で設定する）
        /// </summary>
        /// <param name="keyEventData">キーイベントのデータ</param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="keyEventData"/> がNULLの場合に発生
        /// </exception>
        public InputKey(KeyEventArgs keyEventData)
        {
            // 引数の型チェック
            if (keyEventData == null)
            {
                throw new ArgumentNullException(nameof(keyEventData));
            }

            KeyCode = (Keys)keyEventData.KeyValue;
            Shift = keyEventData.Shift;
            Ctrl = keyEventData.Control;
            Alt = keyEventData.Alt;
            Win = false;
            IsKeepPressing = false;
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="data">このクラスに変換可能な文字列</param>
        public InputKey(string data)
        {
            InputKey input = ConvertFromString(data);
            KeyCode = input.KeyCode;
            Shift = input.Shift;
            Ctrl = input.Ctrl;
            Alt = input.Alt;
            Win = input.Win;
            IsKeepPressing = input.IsKeepPressing;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// キーコードを取得・設定する
        /// </summary>
        [DefaultValue(Keys.None)]
        public Keys KeyCode { get; set; }

        /// <summary>
        /// Shiftキーを押しているかどうか取得・設定する
        /// 押している場合：True、押していない場合：False
        /// </summary>
        [DefaultValue(false)]
        public bool Shift { get; set; }

        /// <summary>
        /// Ctrlキーを押しているかどうか取得・設定する
        /// 押している場合：True、押していない場合：False
        /// </summary>
        [DefaultValue(false)]
        public bool Ctrl { get; set; }

        /// <summary>
        /// Altキーを押しているかどうか取得・設定する
        /// 押している場合：True、押していない場合：False
        /// </summary>
        [DefaultValue(false)]
        public bool Alt { get; set; }

        /// <summary>
        /// Windowsロゴキーを押しているかどうか取得・設定する
        /// 押している場合：True、押していない場合：False
        /// </summary>
        [DefaultValue(false)]
        public bool Win { get; set; }

        /// <summary>
        /// キーを押しっぱなしにするかどうか取得・設定する
        /// 押しっぱなしにする場合：True、押しっぱなしにしない場合：False
        /// </summary>
        [DefaultValue(false)]
        public bool IsKeepPressing { get; set; }

        #endregion

        #region メソッド

        #region ITypeConvertableの実装

        /// <summary>
        /// 文字列を <see cref="InputKey"/> クラスのインスタンスに変換する
        /// </summary>
        /// <param name="value">
        /// <see cref="InputKey"/> クラスのインスタンスに変換する文字列
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>文字列から生成した<see cref="InputKey"/> クラスのインスタンス</returns>
        public InputKey ConvertFromString(string value)
        {
            // NULLチェック
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // パイプ区切りでSplitして各パラメータを取得する
            string[] param = value.Split('|');

            // パイプ区切りのデータに対して、各プロパティの値の判定を行う
            Keys keyCode = Keys.None;
            bool shift = false;
            bool ctrl = false;
            bool alt = false;
            bool win = false;
            bool isKeepPressing = false;
            int count = 0;
            foreach (string data in param)
            {
                // カウントをインクリメント
                count++;

                // 前後の空白を除去
                string trimData = data.Trim();

                // Shiftか判定
                if (trimData.Equals(nameof(Shift)))
                {
                    shift = true;
                }

                // Ctrlか判定
                if (trimData.Equals(nameof(Ctrl)))
                {
                    ctrl = true;
                }

                // Altか判定
                if (trimData.Equals(nameof(Alt)))
                {
                    alt = true;
                }

                // Winか判定
                if (trimData.Equals(nameof(Win)))
                {
                    win = true;
                }

                // キーコードか判定
                if (trimData.Length > 2 && trimData.Substring(0, 2).Equals("0x"))
                {
                    // キーコードから16進のコード部分を抽出する
                    string tmpCode = trimData.Split(':')[0].Trim();
                    tmpCode = tmpCode.Substring(2, tmpCode.Length - 2);

                    // INT型に変換する
                    if (CustomConvert.TryHexStringToInt(tmpCode, out int code))
                    {
                        keyCode = (Keys)code;
                    }
                }
                else if (param.Length == count && !string.IsNullOrEmpty(trimData))
                {
                    // キーコードでない かつ 最後のデータであり値が存在する場合
                    // 押しっぱなしの設定であると判定する
                    isKeepPressing = true;
                }
            }

            // 取得した各プロパティの値から当クラスのインスタンスを生成し返却
            return new InputKey(keyCode, shift, ctrl, alt, win, isKeepPressing);
        }

        /// <summary>
        /// <see cref="InputKey"/> クラスのインスタンスを文字列に変換する
        /// </summary>
        /// <param name="value">
        /// 文字列に変換する <see cref="InputKey"/> クラスのインスタンス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>変換した文字列</returns>
        public string ConvertToString(InputKey value)
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
        public InputKey DeepCopy()
        {
            return new InputKey(
                keyCode: KeyCode,
                shift: Shift,
                ctrl: Ctrl,
                alt: Alt,
                win: Win,
                isKeepPressing: IsKeepPressing);
        }

        /// <summary>
        /// このインスタンスの値を <see cref="string"/> に変換する
        /// </summary>
        /// <returns>このインスタンスと同じ値の文字列</returns>
        public new string ToString()
        {
            // 文字列に変換し返却
            StringBuilder convertValue = new StringBuilder();

            // Shiftキー
            if (Shift)
            {
                convertValue.Append(nameof(Shift)).Append("|");
            }

            // Ctrlキー
            if (Ctrl)
            {
                convertValue.Append(nameof(Ctrl)).Append("|");
            }

            // Altキー
            if (Alt)
            {
                convertValue.Append(nameof(Alt)).Append("|");
            }

            // Windowsロゴキー
            if (Win)
            {
                convertValue.Append(nameof(Win)).Append("|");
            }

            // キーコード
            string keyCode = ((int)KeyCode).ToString("X2", CultureInfo.InvariantCulture);
            convertValue.Append("0x").Append(keyCode);
            convertValue.Append(":").Append(KeyNameMapping.GetName(KeyCode));

            // 押しっぱなしにするかのフラグ
            // （押しっぱなしにの場合のみ付与する）
            if (IsKeepPressing)
            {
                convertValue.Append("|");
                convertValue.Append(_keepPressingText);
            }

            // 生成した文字列を返却
            return convertValue.ToString();
        }

        #endregion
    }
}
