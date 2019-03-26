namespace MisaCommon.Utility.Win32Api.NativeMethod.Capture
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// 【注意：このクラスのメソッドは直接呼び出さず、<see cref="CaptureOperate"/> クラスを経由して呼び出すこと】
    /// プラットフォーム呼び出しサービスを使用してアンマネージ コードへのアクセスを提供するためのクラス
    /// このクラスではWin32APIのウィンドウに関する機能を扱う
    /// </summary>
    /// <remarks>
    /// このクラスは、アンマネージ コード アクセス許可のスタック ウォークを出力する
    /// そのため <see cref="SuppressUnmanagedCodeSecurityAttribute"/> の提供は不可とする
    /// 再利用できるライブラリにする場合は「SafeNativeMethods」クラス、
    /// 又は「UnsafeNativeMethods 」クラスで定義することを検討すること
    /// </remarks>
    internal static class NativeMethods
    {
        #region カーソルの情報を取得

        /// <summary>
        /// グローバルのカーソル情報を取得する
        /// </summary>
        /// <param name="cursorInfo">
        /// カーソル情報を受け取る <see cref="Cursor.CURSORINFO"/> 構造体
        /// この関数を呼び出す前に、<see cref="Cursor.CURSORINFO.StructureSize"/> メンバに
        /// sizeof(<see cref="Cursor.CURSORINFO.StructureSize"/>) を設定する必要がある。
        /// </param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorInfo(ref Cursor.CURSORINFO cursorInfo);

        #endregion

        #region アイコンの情報を取得

        /// <summary>
        /// グローバルのカーソル情報を取得する
        /// </summary>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <param name="iconInfo">アイコン情報を受け取る <see cref="Icon.ICONINFO"/> 構造体</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetIconInfo(IntPtr iconCursorHandle, out Icon.ICONINFO iconInfo);

        #endregion

        #region アイコンの複製

        /// <summary>
        /// 引数で指定された他のモジュールのアイコンへのハンドル（<paramref name="iconCursorHandle"/>）を
        /// 現在のモジュールのアイコンへのハンドルに複製する
        /// （複製したアイコンが不要になった場合、<see cref="DestroyIcon(IntPtr)"/> で破棄する必要がある）
        /// </summary>
        /// <remarks>
        /// この機能は、別のモジュールが所有しているアイコンを、現在のモジュールへの独自のハンドルで取得する
        /// その結果、他のモジュールが解放されてもアプリケーションアイコンはアイコンとして使用することができる
        /// </remarks>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <returns>
        /// 複製したアイコンへのハンドル
        /// （処理失敗時は NULL（<see cref="IntPtr.Zero"/>） を返却）
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr CopyIcon(IntPtr iconCursorHandle);

        #endregion

        #region デバイスコンテキストの作成・選択

        /// <summary>
        /// 指定されたデバイスと互換性のあるメモリデバイスコンテキスト（DC）を作成する
        /// （作成したデバイスコンテキスト（DC）が不要になった場合、<see cref="DeleteObject(IntPtr)"/> で破棄する必要がある）
        /// </summary>
        /// <param name="targetDCHandle">
        /// 既存のデバイスコンテキスト（DC）へのハンドル
        /// 指定したデバイスコンテキスト（DC）関連するデバイスと互換性のあるメモリデバイスコンテキスト（DC）を作成する
        /// NULL（<see cref="IntPtr.Zero"/>）を指定した場合、アプリケーションの現在の画面と互換性のある
        /// メモリデバイスコンテキストを作成する
        /// </param>
        /// <returns>
        /// 作成したメモリデバイスコンテキスト（DC）へのハンドル
        /// （処理失敗時は NULL（<see cref="IntPtr.Zero"/>） を返却）
        /// </returns>
        [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr CreateCompatibleDC(IntPtr targetDCHandle);

        /// <summary>
        /// 指定されたデバイスコンテキスト（DC）のオブジェクトを選択する
        /// （新しいオブジェクトを指定した場合は、同じタイプの前のオブジェクトを置き換える
        /// 　新しいオブジェクトの描画が終了した場合、元のデフォルトオブジェクトに戻す必要がある）
        /// </summary>
        /// <param name="targetDCHandle">
        /// 対象とするデバイスコンテキスト（DC）へのハンドル
        /// </param>
        /// <param name="gdiObjectHandle">
        /// 選択する 論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットへのハンドル
        /// </param>
        /// <returns>
        /// 選択するオブジェクトがリージョンでない場合：
        /// 　指定されたタイプの以前に選択されていたオブジェクトへのハンドル（元のオブジェクトへのハンドル）
        /// 選択するオブジェクトがリージョンの場合：
        /// 　下記のいずれかの値を返却
        /// 　・NULLREGION   ：1 リージョンが空
        ///   ・SIMPLEREGION ：2 リージョンが単一の長方形
        ///   ・COMPLEXREGION：3 リージョンが単一の長方形よりも複雑な形
        /// （処理失敗時：リージョンでない場合は NULL（<see cref="IntPtr.Zero"/>） を返却、
        /// 　リージョンの場合は GDI_ERROR（0xFFFFFFFF）を返却）
        /// </returns>
        [DllImport("gdi32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr SelectObject(IntPtr targetDCHandle, IntPtr gdiObjectHandle);

        #endregion

        #region 画像データのビットブロック転送（BitBlt）

        /// <summary>
        /// 送信元のデバイスコンテキストに設定されたビットマップ情報（ピクセル情報）を
        /// 送信先のデバイスコンテキスにビットブロック転送する
        /// </summary>
        /// <param name="destDCHandle">送信先のデバイスコンテキストへのハンドル</param>
        /// <param name="destPointX">送信先の描画位置：X（左上が基準）</param>
        /// <param name="destPointY">送信先の描画位置：Y（左上が基準）</param>
        /// <param name="width">転送元 及び、転送先の幅（転送するデータの幅）</param>
        /// <param name="height">転送元 及び、転送先の高さ（転送するデータの高さ）</param>
        /// <param name="sourceDCHandle">送信元のデバイスコンテキストへのハンドル</param>
        /// <param name="sourcePointX">送信元の基準位置：X（左上が基準）</param>
        /// <param name="sourcePointY">送信元の基準位置：Y（左上が基準）</param>
        /// <param name="ropCode">合成方法を定めるラスタオペレーションコード</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool BitBlt(
            IntPtr destDCHandle,
            int destPointX,
            int destPointY,
            int width,
            int height,
            IntPtr sourceDCHandle,
            int sourcePointX,
            int sourcePointY,
            uint ropCode);

        #endregion

        #region リソースの解放

        /// <summary>
        /// アイコンを破棄する
        /// （<see cref="CopyIcon(IntPtr)"/> で複製したアイコンは必ずこのメソッドで破棄する必要がある）
        /// </summary>
        /// <param name="iconCursorHandle">アイコン／カーソルへのハンドル</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DestroyIcon(IntPtr iconCursorHandle);

        /// <summary>
        /// 指定されたデバイスコンテキスト（DC）を破棄する
        /// （<see cref="CreateCompatibleDC(IntPtr)"/> で複製したデバイスコンテキスト（DC）は必ずこのメソッドで破棄する必要がある）
        /// </summary>
        /// <param name="targetDCHandle">デバイスコンテキスト（DC）へのハンドル</param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("gdi32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteDC(IntPtr targetDCHandle);

        /// <summary>
        /// オブジェクトに関連付けられているすべてのシステムリソースを解放する
        /// （論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットを破棄する
        /// 　オブジェクトが破棄されると、指定されたハンドルは無効になる）
        /// </summary>
        /// <param name="objectHandle">
        /// 論理ペン、ブラシ、フォント、ビットマップ、リージョン、または、パレットへのハンドル
        /// </param>
        /// <returns>正常終了：True、異常終了：False</returns>
        [DllImport("gdi32.dll", SetLastError = false, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr objectHandle);

        #endregion

        #region CopyIcon メソッドで使用するパラメータの定義

        /// <summary>
        /// <see cref="CopyIcon"/> メソッドで使用するパラメータの定義
        /// </summary>
        internal static class CopyIconParameter
        {
            /// <summary>
            /// <see cref="CopyIcon"/> の実行が成功したか判定する
            /// </summary>
            /// <remarks>
            /// <see cref="CopyIcon"/> の場合、失敗時はアイコンへのハンドルが NULL（<see cref="IntPtr.Zero"/>） で返却
            /// </remarks>
            /// <param name="result"><see cref="CopyIcon"/> を実行した際の戻り値を指定</param>
            /// <returns>正常終了：True、異常終了：False</returns>
            public static bool IsSuccess(IntPtr result)
            {
                return result != IntPtr.Zero;
            }
        }

        #endregion

        #region CreateCompatibleDC メソッドで使用するパラメータの定義

        /// <summary>
        /// <see cref="CreateCompatibleDC"/> メソッドで使用するパラメータの定義
        /// </summary>
        internal static class CreateCompatibleDCParameter
        {
            /// <summary>
            /// <see cref="CreateCompatibleDC"/> の実行が成功したか判定する
            /// </summary>
            /// <remarks>
            /// <see cref="CreateCompatibleDC"/> の場合、
            /// 失敗時はアイコンへのハンドルが NULL（<see cref="IntPtr.Zero"/>） で返却
            /// </remarks>
            /// <param name="result"><see cref="CreateCompatibleDC"/> を実行した際の戻り値を指定</param>
            /// <returns>正常終了：True、異常終了：False</returns>
            public static bool IsSuccess(IntPtr result)
            {
                return result != IntPtr.Zero;
            }
        }

        #endregion

        #region SelectObject メソッドで使用するパラメータの定義

        /// <summary>
        /// <see cref="SelectObject"/> メソッドで使用するパラメータの定義
        /// </summary>
        internal static class SelectObjectParameter
        {
            /// <summary>
            /// <see cref="SelectObject"/> の実行が成功したか判定する
            /// </summary>
            /// <remarks>
            /// <see cref="SelectObject"/> の場合、失敗時は戻り値が下記のとおりとなる
            /// ・リージョンでない場合は NULL（<see cref="IntPtr.Zero"/>） で返却
            /// ・リージョンの場合は GDI_ERROR（0xFFFFFFFF）で返却
            /// </remarks>
            /// <param name="result">
            /// <see cref="SelectObject"/> を実行した際の戻り値を指定
            /// </param>
            /// <param name="isRegion">
            /// リージョンを選択したかどうかのフラグ
            /// リージョンを選択した場合：True、リージョン以外の選択した場合：False
            /// </param>
            /// <returns>正常終了：True、異常終了：False</returns>
            public static bool IsSuccess(IntPtr result, bool isRegion)
            {
                if (!isRegion)
                {
                    // リージョンでないの場合
                    return result != IntPtr.Zero;
                }
                else
                {
                    // リージョンの場合
                    return result.ToInt32() != (int)ErrorCode.GDI_ERROR;
                }
            }
        }

        #endregion
    }
}
