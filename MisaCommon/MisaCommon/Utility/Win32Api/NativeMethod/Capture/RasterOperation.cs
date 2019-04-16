namespace MisaCommon.Utility.Win32Api.NativeMethod.Capture
{
    /// <summary>
    /// ラスタオペレーションに関する情報を扱うクラス
    /// （<see cref="NativeMethods.BitBlt(SafeDCHandle, int, int, int, int, SafeDCHandle, int, int, uint)"/>
    /// 　メソッド等で指定するラスタオペレーションに関する情報を扱う）
    /// </summary>
    internal static class RasterOperation
    {
        #region 一般的なラスタオペレーションコードの定義

        /// <summary>
        /// 一般的なラスタオペレーションコードの定義
        /// </summary>
        public enum CommonCode : uint
        {
            /// <summary>
            /// [ dest = BLACK ]
            /// 黒（※）で「転送先」の領域を塗りつぶす
            /// ※：正確には物理パレットのインデックス：0 に関連付けられている色、既定は黒
            /// </summary>
            BLACKNESS = 0x00000042,

            /// <summary>
            /// [ dest = WHITE ]
            /// 白（※）で「転送先」の領域を塗りつぶす
            /// ※：正確には物理パレットのインデックス：1 に関連付けられている色、既定は白
            /// </summary>
            WHITENESS = 0x00FF0062,

            /// <summary>
            /// [ dest = source ]
            /// 「転送元」を「転送先」の領域にそのままコピーする
            /// </summary>
            SRCCOPY = 0x00CC0020,

            /// <summary>
            /// [ dest = source OR dest ]
            /// OR 演算子を使用して、「転送元」と「転送先」の色を合成した結果を、
            /// 「転送先」の領域に描画する
            /// </summary>
            SRCPAINT = 0x00EE0086,

            /// <summary>
            /// [ dest = source AND dest ]
            /// AND 演算子を使用して、「転送元」と「転送先」の色を合成した結果を、
            /// 「転送先」の領域に描画する
            /// </summary>
            SRCAND = 0x008800C6,

            /// <summary>
            /// [ dest = source XOR dest ]
            /// XOR 演算子を使用して、「転送元」と「転送先」の色を合成した結果を、
            /// 「転送先」の領域に描画する
            /// </summary>
            SRCINVERT = 0x00660046,

            /// <summary>
            /// [ dest = source AND (NOT dest) ]
            /// AND 演算子を使用して、「転送元」と「色を反転させた転送先」の色を合成した結果を、
            /// 「転送先」の領域に描画する
            /// </summary>
            SRCERASE = 0x00440328,

            /// <summary>
            /// [ dest = NOT dest ]
            /// 「転送先」の色を反転させる
            /// </summary>
            DSTINVERT = 0x00550009,

            /// <summary>
            /// [ dest = NOT source ]
            /// 「色を反転させた転送元」を「転送先」にコピーする
            /// </summary>
            NOTSRCCOPY = 0x00330008,

            /// <summary>
            /// [ dest = NOT(source OR dest) ]
            /// OR 演算子を使用して、「転送元」と「転送先」の色を合成し、その結果の色を反転した結果を、
            /// 「転送先」の領域に描画する
            /// </summary>
            NOTSRCERASE = 0x001100A6,

            /// <summary>
            /// [ dest = source AND (BRUSH selected in dest) ]
            /// AND 演算子を使用して、「転送元の色」と「転送先で現在選択しているブラシ」をマージし、
            /// その結果を「転送先」の領域に描画する
            /// </summary>
            MERGECOPY = 0x00C000CA,

            /// <summary>
            /// [ dest = (NOT source) OR dest ]
            /// OR 演算子を使用して、「色を反転させた転送元」と「転送先」の色をマージし、
            /// その結果を「転送先」の領域に描画する
            /// </summary>
            MERGEPAINT = 0x00BB0226,

            /// <summary>
            /// [ dest = (BRUSH selected in dest)) ]
            /// 「転送先で現在選択しているブラシ」を使用して「転送先」の領域を描画する
            /// </summary>
            PATCOPY = 0x00F00021,

            /// <summary>
            /// [ dest = dest OR (BRUSH selected in dest) ]
            /// OR 演算子を使用して、「転送先で現在選択しているブラシの色」と「転送先の色」を組み合わせ、
            /// その結果を「転送先」の領域に描画する
            /// </summary>
            PATPAINT = 0x00FB0A09,

            /// <summary>
            /// [ dest = dest XOR (BRUSH selected in dest) ]
            /// XOR 演算子を使用して、「転送先で現在選択しているブラシの色」と「転送先の色」を組み合わせ、
            /// その結果を「転送先」の領域に描画する
            /// </summary>
            PATINVERT = 0x005A0049,
        }

        #endregion
    }
}
