namespace MisaCommon.CustomType
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Text;

    using MisaCommon.CustomType.Converter;
    using MisaCommon.MessageResources.Type;
    using MisaCommon.Utility.StaticMethod;

    /// <summary>
    /// キーボード、マウス以外の入力デバイスの操作の情報を扱うクラス
    /// </summary>
    /// <remarks>
    /// このクラスの公開プロパティにおける表示名と説明は、
    /// <see cref="LocalizableTypeConverter{T, TResouces}"/>にてマッピングしている
    /// <list type="bullet">
    ///     <item>
    ///         <term>string型への変換</term>
    ///         <description>
    ///         各パラメータを下記の順番でカンマ区切りの16進文字列で表現する
    ///         １．入力するメッセージ
    ///         ２．メッセージに対するlParamパラメータ（下位）を設定
    ///         ３．メッセージに対するlParamパラメータ（上位）を設定
    ///         【例】0x0001, 0x0002, 0x0003
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Serializable]
    [TypeConverter(typeof(LocalizableTypeConverter<HardwareInput, HardwareInputPropertyMessage>))]
    public class HardwareInput : ITypeConvertable<HardwareInput>
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="message">入力するメッセージ</param>
        /// <param name="lowlParam"><paramref name="message"/> に対するlParamパラメータ（下位）</param>
        /// <param name="highlParam"><paramref name="message"/> に対するlParamパラメータ（上位）</param>
        public HardwareInput(int message, short lowlParam, short highlParam)
        {
            Message = message;
            LowlParam = lowlParam;
            HighlParam = highlParam;
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        public HardwareInput()
            : this(0, 0, 0)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="data">このクラスに変換可能な文字列</param>
        public HardwareInput(string data)
        {
            HardwareInput input = ConvertFromString(data);
            Message = input.Message;
            LowlParam = input.LowlParam;
            HighlParam = input.HighlParam;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 入力するメッセージを設定を取得・設定する
        /// </summary>
        [DefaultValue(0)]
        public int Message { get; set; }

        /// <summary>
        /// <see cref="Message"/> に対するlParamパラメータ（下位）を取得・設定する
        /// </summary>
        [DefaultValue(0)]
        public short LowlParam { get; set; }

        /// <summary>
        /// <see cref="Message"/> に対するlParamパラメータ（上位）を取得・設定する
        /// </summary>
        [DefaultValue(0)]
        public short HighlParam { get; set; }

        #endregion

        #region メソッド

        #region ITypeConvertableの実装

        /// <summary>
        /// 文字列を <see cref="HardwareInput"/> クラスのインスタンスに変換する
        /// 変換できない場合はNULLを返却する
        /// </summary>
        /// <param name="value">
        /// <see cref="HardwareInput"/> クラスのインスタンスに変換する文字列
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>
        /// 文字列から生成した<see cref="HardwareInput"/> クラスのインスタンス
        /// 変換できない場合はNULLを返却する
        /// </returns>
        public HardwareInput ConvertFromString(string value)
        {
            // 引数の型チェック
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // カンマ区切りをSplitして各パラメータを取得する
            string[] param = value.Split(',');

            // Splitした結果、要素数3未満の配列の場合は過不足があり変換ができないためNULLを返却する
            if (param.Length < 3)
            {
                return null;
            }

            // 変換処理を行う、変換に失敗した場合はNULLを返却
            if (!CustomConvert.TryHexStringToInt(param[0], out int message)
                || !CustomConvert.TryHexStringToShort(param[1], out short lowlParam)
                || !CustomConvert.TryHexStringToShort(param[2], out short highlParam))
            {
                return null;
            }

            // HardwareInputクラスのインスタンスを生成し返却
            return new HardwareInput(message, lowlParam, highlParam);
        }

        /// <summary>
        /// <see cref="HardwareInput"/> クラスのインスタンスを文字列に変換する
        /// </summary>
        /// <param name="value">
        /// 文字列に変換する <see cref="HardwareInput"/> クラスのインスタンス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>変換した文字列</returns>
        public string ConvertToString(HardwareInput value)
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
        public HardwareInput DeepCopy()
        {
            return new HardwareInput(
                message: Message,
                lowlParam: LowlParam,
                highlParam: HighlParam);
        }

        /// <summary>
        /// このインスタンスの値を <see cref="string"/> に変換する
        /// </summary>
        /// <returns>このインスタンスと同じ値の文字列</returns>
        public new string ToString()
        {
            // 文字列に変換し返却
            string message = Message > 0xFFFF
                ? Message.ToString("X8", CultureInfo.InvariantCulture)
                : Message.ToString("X4", CultureInfo.InvariantCulture);
            StringBuilder convertValue = new StringBuilder();
            convertValue.Append("0x").Append(message);
            convertValue.Append(", ").Append("0x").Append(LowlParam.ToString("X4", CultureInfo.InvariantCulture));
            convertValue.Append(", ").Append("0x").Append(HighlParam.ToString("X4", CultureInfo.InvariantCulture));
            return convertValue.ToString();
        }

        #endregion
    }
}
