namespace MisaCommon.Utility.Win32Api.NativeMethod.Input
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    /// <summary>
    /// マウス操作の情報を扱うクラス
    /// </summary>
    internal static class Mouse
    {
        #region フラグ定義

        /// <summary>
        /// 入力操作のタイプ：マウス操作を行う
        /// （<see cref="INPUT.InputType"/> に設定する値）
        /// </summary>
        public const int InputType = 0;

        /// <summary>
        /// マウスの操作（移動、ボタンクリック等）を示すビットフラグ
        /// </summary>
        [Flags]
        public enum OperateFlag : int
        {
            /// <summary>
            /// マウスを移動
            /// </summary>
            MOUSEEVENTF_MOVE = 0x0001,

            /// <summary>
            /// 左ボタンを押す
            /// </summary>
            MOUSEEVENTF_LEFTDOWN = 0x0002,

            /// <summary>
            /// 左ボタンを離す
            /// </summary>
            MOUSEEVENTF_LEFTUP = 0x0004,

            /// <summary>
            /// 右ボタンを押す
            /// </summary>
            MOUSEEVENTF_RIGHTDOWN = 0x0008,

            /// <summary>
            /// 右ボタンを離す
            /// </summary>
            MOUSEEVENTF_RIGHTUP = 0x0010,

            /// <summary>
            /// 中央ボタンを押す
            /// </summary>
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,

            /// <summary>
            /// 中央ボタンを離す
            /// </summary>
            MOUSEEVENTF_MIDDLEUP = 0x0040,

            /// <summary>
            /// Xボタンを押す
            /// </summary>
            MOUSEEVENTF_XDOWN = 0x0080,

            /// <summary>
            /// Xボタンを離す
            /// </summary>
            MOUSEEVENTF_XUP = 0x0100,

            /// <summary>
            /// マウスホイールを前後方向に移動
            /// （移動量は <see cref="MOUSEINPUT.Data"/> で指定する）
            /// </summary>
            MOUSEEVENTF_WHEEL = 0x0800,

            /// <summary>
            /// マウスホイールを左右方向に移動
            /// （移動量は <see cref="MOUSEINPUT.Data"/> で指定する）
            /// </summary>
            MOUSEEVENTF_HWHEEL = 0x1000,

            /// <summary>
            /// マウスの操作で WM_MOUSEMOVE のメッセージを発生させない
            /// （既定の動作は、WM_MOUSEMOVE  のメッセージを発生させる）
            /// </summary>
            MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,

            /// <summary>
            /// 絶対位置の座標モードを使用する
            /// （<see cref="MOUSEEVENTF_ABSOLUTE"/> と一緒に使用する必要がある）
            /// ・絶対位置：ディスクトップの座標にマッピングされる値
            /// ・相対位置：最後のマウス操作イベント発生位置からの移動ピクセル数の値
            /// </summary>
            MOUSEEVENTF_VIRTUALDESK = 0x4000,

            /// <summary>
            /// 絶対位置の座標モードを使用する
            /// このフラグを設定していない場合は、相対位置の座標モードを使用する、
            /// ・絶対位置：ディスクトップの座標（左上（0,0）～右下（65535, 65535））にマッピングされる値
            /// ・相対位置：最後のマウス操作イベント発生位置からの移動ピクセル数の値
            /// </summary>
            MOUSEEVENTF_ABSOLUTE = 0x8000,
        }

        /// <summary>
        /// マウスのXボタンが押下の際に指定する使用するXボタンを指定するビットフラグ
        /// </summary>
        public enum DataXButton : int
        {
            /// <summary>
            /// 1番目のXボタン
            /// （Xボタンが押下処理の場合に使用する）
            /// </summary>
            XBUTTON1 = 0x0001,

            /// <summary>
            /// 2番目のXボタン
            /// （Xボタンが押下処理の場合に使用する）
            /// </summary>
            XBUTTON2 = 0x0002,
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 引数の <paramref name="point"/> 座標をマウス操作で使用する座標に変換する
        /// </summary>
        /// <param name="point">変換対象の座標データ</param>
        /// <param name="screenSize">対象のディスプレイ</param>
        /// <returns>変換したマウス操作で使用する座標</returns>
        public static Point ToMousePoint(Point point, Size screenSize)
        {
            int x = point.X * (65535 / screenSize.Width);
            int y = point.Y * (65535 / screenSize.Height);
            return new Point(x, y);
        }

        #endregion

        #region 構造体定義

        /// <summary>
        /// マウス操作の情報を扱う構造体
        /// （Win32Api：SendInputの引数に設定するINPUT構造体の定義）
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            /// <summary>
            /// 入力操作のタイプ
            /// </summary>
            public int InputType;

            /// <summary>
            /// マウス操作の情報を扱う構造体
            /// </summary>
            public MOUSEINPUT Mouse;
        }

        /// <summary>
        /// マウス操作の情報を扱う構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8, Size = 24)]
        public struct MOUSEINPUT
        {
            /// <summary>
            /// マウスの絶対位置の X 座標 または、
            /// 最後のマウス操作イベント発生位置からの X 方向の移動ピクセル数
            /// <see cref="OperateFlag"/> の値に応じて、絶対位置 または、相対位置のどちらかの値を設定する
            /// ・絶対位置の場合、ディスクトップの座標（0～65535）
            /// ・相対位置の場合、正の値は右方向の移動、負の値は左方向の移動を示す
            /// </summary>
            /// <remarks>
            /// マウスの相対的な移動はOSで設定しているマウスの速度や移動距離の閾値により影響を受ける
            /// 移動距離が第1の閾値よりも大きくマウス速度がゼロでない場合、システムは移動距離を2倍にする
            /// 移動距離が第2の閾値よりも大きくマウス速度が2に等しい場合、システムは移動距離を4倍にする
            /// （第1の閾値で2倍、第2の閾値で2倍の計4倍）
            /// 詳細は「MOUSEINPUT structure」の定義情報を参照
            /// </remarks>
            public int X;

            /// <summary>
            /// マウスの絶対位置の Y 座標 または、
            /// 最後のマウス操作イベント発生位置からの Y 方向の移動ピクセル数
            /// <see cref="OperateFlag"/> の値に応じて、絶対位置 または、相対位置のどちらかの値を設定する
            /// ・絶対位置の場合、ディスクトップの座標（0～65535）
            /// ・相対位置の場合、正の値は下方向の移動、負の値は上方向の移動を示す
            /// </summary>
            /// <remarks>
            /// マウスの相対的な移動はOSで設定しているマウスの速度や移動距離の閾値により影響を受ける
            /// 移動距離が第1の閾値よりも大きくマウス速度がゼロでない場合、システムは移動距離を2倍にする
            /// 移動距離が第2の閾値よりも大きくマウス速度が2に等しい場合、システムは移動距離を4倍にする
            /// （第1の閾値で2倍、第2の閾値で2倍の計4倍）
            /// 詳細は「MOUSEINPUT structure」の定義情報を参照
            /// </remarks>
            public int Y;

            /// <summary>
            /// <see cref="OperateFlag"/> に下記が含まれている場合のみに設定する値
            /// （下記以外の場合は 0 を設定する）
            /// ・<see cref="OperateFlag.MOUSEEVENTF_WHEEL"/>、<see cref="OperateFlag.MOUSEEVENTF_HWHEEL"/>
            /// 　ホリールの回転量を「WHEEL_DELTA：120」単位設定する（1ホイールが120）
            /// 　正の値は前（右）方向への回転、負の値は後（左）方向への回転を示す
            /// ・<see cref="OperateFlag.MOUSEEVENTF_XDOWN"/>、<see cref="OperateFlag.MOUSEEVENTF_XUP"/>
            /// 　<see cref="DataXButton"/> で定義されているXボタンのフラグを設定する
            /// </summary>
            public int Data;

            /// <summary>
            /// マウスの操作（移動、ボタンクリック等）を示す
            /// ビットフラグ（<see cref="Mouse.OperateFlag"/>）を設定する
            /// フラグの設定は状態の変化を示すように設定する
            /// </summary>
            /// <remarks>
            /// 下記のフラグは <see cref="Data"/> フィールドを使用するため同時に指定することは不可
            /// ・<see cref="OperateFlag.MOUSEEVENTF_WHEEL"/>
            /// ・<see cref="OperateFlag.MOUSEEVENTF_HWHEEL"/>
            /// ・<see cref="OperateFlag.MOUSEEVENTF_XDOWN"/>
            /// ・<see cref="OperateFlag.MOUSEEVENTF_XUP"/>
            /// </remarks>
            public int OperateFlag;

            /// <summary>
            /// マウスイベントのタイムスタンプ（ミリ秒単位）を設定する
            /// 0 を指定した場合はシステムは独自のタイムスタンプを使用する
            /// </summary>
            public int Time;

            /// <summary>
            /// マウスイベントに関連する追加情報を設定する
            /// 詳細は「GetMessageExtraInfo」を参照
            /// </summary>
            public IntPtr ExtraInfo;
        }

        #endregion
    }
}
