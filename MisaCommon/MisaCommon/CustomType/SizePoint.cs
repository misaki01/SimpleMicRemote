namespace MisaCommon.CustomType
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Globalization;
    using System.Text;

    using MisaCommon.CustomType.Converter;
    using MisaCommon.CustomType.UiEditor;
    using MisaCommon.MessageResources.Type;

    /// <summary>
    /// フォームのサイズ（幅、高さ）と位置（X座標、Y座標）をまとめて扱うクラス
    /// </summary>
    /// <remarks>
    /// このクラスの公開プロパティにおける表示名と説明は、
    /// <see cref="LocalizableTypeConverter{T, TResouces}"/>にてマッピングしている
    /// <list type="bullet">
    ///     <item>
    ///         <term>string型への変換</term>
    ///         <description>
    ///         ４つの数値をカンマ区切りの文字列で表現する
    ///         【例】サイズ‗幅：100、サイズ‗高さ：200、位置‗X：300、位置Y：400 の場合 ⇒ 「100, 200, 300, 400」と表現する
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [Serializable]
    [TypeConverter(typeof(LocalizableTypeConverter<SizePoint, SizePointPropertyMessage>))]
    [Editor(typeof(SizePointUIEditor), typeof(UITypeEditor))]
    public class SizePoint : ITypeConvertable<SizePoint>
    {
        #region クラス変数・定数

        /// <summary>
        /// フォームのサイズ_幅
        /// </summary>
        private int _sizeWidth;

        /// <summary>
        /// フォームのサイズ_高さ
        /// </summary>
        private int _sizeHeight;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 引数の値で初期化する
        /// </summary>
        /// <param name="sizeWidth">フォームのサイズ‗幅</param>
        /// <param name="sizeHeight">フォームのサイズ‗高さ</param>
        /// <param name="positionX">フォームの位置‗X</param>
        /// <param name="positionY">フォームの位置‗Y</param>
        public SizePoint(int sizeWidth, int sizeHeight, int positionX, int positionY)
        {
            SizeWidth = sizeWidth;
            SizeHeight = sizeHeight;
            PositionX = positionX;
            PositionY = positionY;
        }

        /// <summary>
        /// コンストラクタ
        /// サイズ、位置を0で初期化する
        /// </summary>
        public SizePoint()
            : this(0, 0, 0, 0)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の値で初期化する
        /// </summary>
        /// <param name="size">フォームのサイズ</param>
        /// <param name="point">フォームの位置</param>
        public SizePoint(Size size, Point point)
            : this(size.Width, size.Height, point.X, point.Y)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の値で初期化する
        /// </summary>
        /// <param name="size">フォームのサイズ</param>
        /// <param name="positionX">フォームの位置‗X</param>
        /// <param name="positionY">フォームの位置‗Y</param>
        public SizePoint(Size size, int positionX, int positionY)
            : this(size.Width, size.Height, positionX, positionY)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の値で初期化する
        /// </summary>
        /// <param name="sizeWidth">フォームのサイズ‗幅</param>
        /// <param name="sizeHeight">フォームのサイズ‗高さ</param>
        /// <param name="point">フォームの位置</param>
        public SizePoint(int sizeWidth, int sizeHeight, Point point)
            : this(sizeWidth, sizeHeight, point.X, point.Y)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// 各プロパティを初期化する
        /// </summary>
        /// <param name="data">このクラスに変換可能な文字列</param>
        public SizePoint(string data)
        {
            SizePoint input = ConvertFromString(data);
            SizeWidth = input.SizeWidth;
            SizeHeight = input.SizeHeight;
            PositionX = input.PositionX;
            PositionY = input.PositionY;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// フォームのサイズ_幅を取得・設定する
        /// </summary>
        [DefaultValue(0)]
        public int SizeWidth
        {
            get => _sizeWidth;
            set => _sizeWidth = value < 0 ? 0 : value;
        }

        /// <summary>
        /// フォームのサイズ_高さを取得・設定する
        /// </summary>
        [DefaultValue(0)]
        public int SizeHeight
        {
            get => _sizeHeight;
            set => _sizeHeight = value < 0 ? 0 : value;
        }

        /// <summary>
        /// フォームの位置_Xを取得・設定する
        /// </summary>
        [DefaultValue(0)]
        public int PositionX { get; set; }

        /// <summary>
        /// フォームの位置_Yを取得・設定する
        /// </summary>
        [DefaultValue(0)]
        public int PositionY { get; set; }

        /// <summary>
        /// フォームのサイズを取得・設定する
        /// </summary>
        /// <remarks>
        /// プログラムで使用する用のプロパティのためプロパティグリッドには表示しない
        /// </remarks>
        [Browsable(false)]
        public Size Size
        {
            get => new Size(SizeWidth, SizeHeight);
            set
            {
                SizeWidth = value.Width;
                SizeHeight = value.Height;
            }
        }

        /// <summary>
        /// フォームの位置を取得・設定する
        /// </summary>
        /// <remarks>
        /// プログラムで使用する用のプロパティのためプロパティグリッドには表示しない
        /// </remarks>
        [Browsable(false)]
        public Point Point
        {
            get => new Point(PositionX, PositionY);
            set
            {
                PositionX = value.X;
                PositionY = value.Y;
            }
        }

        #endregion

        #region メソッド

        #region ITypeConvertableの実装

        /// <summary>
        /// 文字列を <see cref="SizePoint"/> クラスのインスタンスに変換する
        /// </summary>
        /// <param name="value">
        /// <see cref="SizePoint"/> クラスのインスタンスに変換する文字列
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>文字列から生成した<see cref="SizePoint"/> クラスのインスタンス</returns>
        public SizePoint ConvertFromString(string value)
        {
            // NULLチェック
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // カンマ区切りでSplitして各パラメータを取得する
            string[] param = value.Split(',');

            // Splitした結果、要素数4以外の配列の場合、パラメータに過不足があるため補正する
            // Sizeパラメータの設定
            Size size = new Size(0, 0);
            if (param.Length >= 2)
            {
                if (int.TryParse(param[0], out int width) && int.TryParse(param[1], out int height))
                {
                    // 幅、高さの両方のパラメータがINT型に変換可能な場合のみ設定を行う
                    size = new Size(width, height);
                }
            }

            // Pointパラメータの設定
            Point point = new Point(0, 0);
            if (param.Length >= 4)
            {
                // 要素数が4以上の場合、Pointパラメータの設定が可能なため設定を行う
                if (int.TryParse(param[2], out int pointX) && int.TryParse(param[3], out int pointY))
                {
                    // 位置‗X、位置‗Yの両方のパラメータがINT型に変換可能な場合のみ設定を行う
                    point = new Point(pointX, pointY);
                }
            }

            // サイズと位置からSizePointクラスのインスタンスを生成し返却
            return new SizePoint(size, point);
        }

        /// <summary>
        /// <see cref="SizePoint"/> クラスのインスタンスを文字列に変換する
        /// </summary>
        /// <param name="value">
        /// 文字列に変換する <see cref="SizePoint"/> クラスのインスタンス
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="value"/> がNULLの場合に発生
        /// </exception>
        /// <returns>変換した文字列</returns>
        public string ConvertToString(SizePoint value)
        {
            // NULLチェック
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
        public SizePoint DeepCopy()
        {
            return new SizePoint(
                sizeWidth: SizeWidth,
                sizeHeight: SizeHeight,
                positionX: PositionX,
                positionY: PositionY);
        }

        /// <summary>
        /// このインスタンスの値を <see cref="string"/> に変換する
        /// </summary>
        /// <returns>このインスタンスと同じ値の文字列</returns>
        public new string ToString()
        {
            // 文字列に変換し返却
            StringBuilder convertValue = new StringBuilder();
            convertValue.Append(SizeWidth.ToString(CultureInfo.InvariantCulture));
            convertValue.Append(", ").Append(SizeHeight.ToString(CultureInfo.InvariantCulture));
            convertValue.Append(", ").Append(PositionX.ToString(CultureInfo.InvariantCulture));
            convertValue.Append(", ").Append(PositionY.ToString(CultureInfo.InvariantCulture));
            return convertValue.ToString();
        }

        #endregion
    }
}
