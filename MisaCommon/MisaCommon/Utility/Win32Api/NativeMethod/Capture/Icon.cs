namespace MisaCommon.Utility.Win32Api.NativeMethod.Capture
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// アイコンに関する情報を扱うクラス
    /// </summary>
    internal static class Icon
    {
        #region システムのデフォルトのカーソル／アイコンの定義

        /// <summary>
        /// システムでのデフォルトのカーソルの定義
        /// （<see cref="NativeMethods.GetIconInfo(SafeCopyIconHandle, out ICONINFO)"/> において、
        /// 　システムのデフォルトのカーソル情報を取得したい場合に、第１引数に設定してする）
        /// </summary>
        public enum SYSTEM_CURSOR : uint
        {
            /// <summary>
            /// 標準の矢印のカーソル
            /// </summary>
            IDC_ARROW = 32512,

            /// <summary>
            /// Iビームのカーソル
            /// </summary>
            IDC_IBEAM = 32513,

            /// <summary>
            /// 砂時計のカーソル
            /// </summary>
            IDC_WAIT = 32514,

            /// <summary>
            /// 十字のカーソル
            /// </summary>
            IDC_CROSS = 32515,

            /// <summary>
            /// 垂直の上向き矢印のカーソル
            /// </summary>
            IDC_UPARROW = 32516,

            /// <summary>
            /// 左上と右下を指す斜めの両矢印のカーソル
            /// </summary>
            IDC_SIZENWSE = 32642,

            /// <summary>
            /// 右上と左下を指す斜めの両矢印のカーソル
            /// </summary>
            IDC_SIZENESW = 32643,

            /// <summary>
            /// 左右を刺す水平の両矢印のカーソル
            /// </summary>
            IDC_SIZEWE = 32644,

            /// <summary>
            /// 上下を刺す垂直の両矢印のカーソル
            /// </summary>
            IDC_SIZENS = 32645,

            /// <summary>
            /// 上下左右を指す四方向矢印のカーソル
            /// </summary>
            IDC_SIZEALL = 32646,

            /// <summary>
            /// 禁止マーク（🚫：円にスラッシュ）のカーソル
            /// </summary>
            IDC_NO = 32648,

            /// <summary>
            /// ハンドのカーソル
            /// </summary>
            IDC_HAND = 32649,

            /// <summary>
            /// バックグランドで作業中：標準の矢印と小さな砂時計のカーソル
            /// </summary>
            IDC_APPSTARTING = 32650,

            /// <summary>
            /// ヘルプの選択：矢印と疑問符（？）のカーソル
            /// </summary>
            IDC_HELP = 32651,
        }

        /// <summary>
        /// システムでのデフォルトのアイコンの定義
        /// （<see cref="NativeMethods.GetIconInfo(SafeCopyIconHandle, out ICONINFO)"/> において、
        /// 　システムのデフォルトのアイコン情報を取得したい場合に、第１引数に設定してする）
        /// </summary>
        public enum SYSTEM_ICON : uint
        {
            /// <summary>
            /// 既定のアプリケーションのアイコン
            /// </summary>
            IDI_APPLICATION = 32512,

            /// <summary>
            /// 一時停止の標識のアイコン
            /// （エラー <see cref="IDI_ERROR"/> と同じアイコンに統合されている）
            /// </summary>
            IDI_HAND = 32513,

            /// <summary>
            /// エラーのアイコン（赤の円に×）
            /// </summary>
            IDI_ERROR = IDI_HAND,

            /// <summary>
            /// 疑問符のアイコン（青の円に？）
            /// </summary>
            IDI_QUESTION = 32514,

            /// <summary>
            /// 感嘆符のアイコン（黄の三角に！）
            /// （警告 <see cref="IDI_WARNING"/> と同じアイコンに統合されている）
            /// </summary>
            IDI_EXCLAMATION = 32515,

            /// <summary>
            /// 警告のアイコン（黄の三角に！）
            /// </summary>
            IDI_WARNING = IDI_EXCLAMATION,

            /// <summary>
            /// アスタリスクのアイコン（青の円に！）
            /// （情報 <see cref="IDI_INFORMATION"/> と同じアイコンに統合されている）
            /// </summary>
            IDI_ASTERISK = 32516,

            /// <summary>
            /// 情報のアイコン（青の円に！）
            /// </summary>
            IDI_INFORMATION = IDI_ASTERISK,

            /// <summary>
            /// 既定のアプリケーションのアイコン
            /// （Windows2000の場合：Windowsのロゴアイコン）
            /// </summary>
            IDI_WINLOGO = 32517,
        }

        #endregion

        #region 構造体定義

        /// <summary>
        /// アイコンまたはカーソルに関する情報を扱う構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            /// <summary>
            /// この構造体がアイコンを定義しているかのフラグ
            /// Trueの場合：アイコン
            /// Falseの場合：カーソル
            /// </summary>
            public bool IsIcon;

            /// <summary>
            /// カーソルのホットスポットの X 座標
            /// （この構造体がアイコンを定義している場合：
            /// 　ホットスポットは常にアイコンの中央であり、このメンバーは無視する）
            /// </summary>
            public int HotspotX;

            /// <summary>
            /// カーソルのホットスポットの Y 座標
            /// （この構造体がアイコンを定義している場合：
            /// 　ホットスポットは常にアイコンの中央であり、このメンバーは無視する）
            /// </summary>
            public int HotspotY;

            /// <summary>
            /// アイコンのマスク（透過用）のビットマップハンドル
            /// （白黒のアイコンの場合:上半分が ANDマスク、下半分が XORマスク、
            /// 　カラーアイコンの場合：全てがXORマスク）
            /// </summary>
            public IntPtr MaskBitmapHandle;

            /// <summary>
            /// アイコンのカラーのビットマップハンドル
            /// （白黒のアイコンの場合:このメンバーはオプションとなる）
            /// </summary>
            public IntPtr ColorBitmapHandle;
        }

        #endregion
    }
}
