namespace MisaCommon.Modules
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using MisaCommon.CustomType;
    using MisaCommon.Exceptions;
    using MisaCommon.MessageResources;
    using MisaCommon.Utility.Win32Api;

    /// <summary>
    /// ウィンドウキャプチャに関する処理を行うクラス
    /// </summary>
    public static class WindowCapture
    {
        #region メソッド

        /// <summary>
        /// アクティブウィンドウをキャプチャする
        /// </summary>
        /// <param name="isCursorCapture">
        /// マウスカーソルをキャプチャするかどうかのフラグ
        /// キャプチャする場合：True、しない場合：False
        /// </param>
        /// <exception cref="WindowCaptureException">
        /// キャプチャ処理が失敗した場合に発生
        /// </exception>
        /// <returns>キャプチャした画像データ</returns>
        public static Image ActiveWindowCapture(bool isCursorCapture)
        {
            Bitmap windowImage = null;
            try
            {
                // アクティブウィンドウのキャプチャを取得
                windowImage = ActiveWindowCapture(out SizePoint windowSizePoint);

                // カーソルを描画する場合は、カーソル描画処理を行う
                if (isCursorCapture)
                {
                    // マウスカーソルのキャプチャを取得し、クリップボードから取得した画像に書き込む
                    using (CursorInfo cursolInfo
                        = CaptureOperate.CaptureCurrentCursor(windowImage, windowSizePoint.Point))
                    {
                        // カーソル情報を取得できた場合、
                        // キャプチャした画像にカーソルを描画する
                        if (cursolInfo != null)
                        {
                            using (Graphics graphics = Graphics.FromImage(windowImage))
                            {
                                graphics.DrawImage(
                                    image: (Bitmap)cursolInfo.CursorImage,
                                    x: cursolInfo.ImagePoint.X,
                                    y: cursolInfo.ImagePoint.Y,
                                    width: cursolInfo.CursorImage.Size.Width,
                                    height: cursolInfo.CursorImage.Size.Height);
                            }
                        }
                    }
                }

                // 取得した画像を返却する
                return windowImage;
            }
            catch (Exception ex)
                when (ex is PlatformInvokeException
                    || ex is Win32OperateException
                    || ex is Win32Exception)
            {
                // キャプチャ処理で想定される例外の場合
                // 例外発生時は、戻り値の画像データを破棄する
                windowImage?.Dispose();

                // 発生した例外を CaptureException に格納してスローする
                throw new WindowCaptureException(ErrorMessage.WindowCaptureError, ex);
            }
            catch
            {
                // 上記以外の例外の場合
                // 戻り値の画像データを破棄し、再スローする
                windowImage?.Dispose();
                throw;
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// アクティブウィンドウのキャプチャを行う
        /// </summary>
        /// <param name="targetWindowSizePoint">
        /// キャプチャ対象となるアクティブウィンドウのサイズ位置情報を返却
        /// </param>
        /// <exception cref="PlatformInvokeException">
        /// Win32Apiの処理において、処理の呼び出しに失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32OperateException">
        /// Win32Apiの処理において、処理に失敗した場合に発生
        /// </exception>
        /// <exception cref="Win32Exception">
        /// キャプチャデータ取得に用いる <see cref="Graphics.CopyFromScreen(Point, Point, Size, CopyPixelOperation)"/>
        /// において、Win32Apiの操作に失敗した場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// キャプチャデータを格納する <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// </exception>
        /// <returns>
        /// アクティブウィンドウをキャプチャした画像データ
        /// （キャプチャできない場合は NULL を返却）
        /// </returns>
        private static Bitmap ActiveWindowCapture(out SizePoint targetWindowSizePoint)
        {
            // アクティブウィンドウのサイズと位置を取得
            SizePoint windowSizePoint = null;
            IntPtr windowHandle = WindowOperate.GetForegroundWindowHandle();
            if (windowHandle != IntPtr.Zero)
            {
                // ウィンドウハンドルが取得できた場合、
                // ウィンドウサイズ位置、クライアント領域のサイズ位置を取得
                HandleRef handle = new HandleRef(0, windowHandle);
                SizePoint window = WindowOperate.GetWindowRect(handle);
                SizePoint client = WindowOperate.GetClientRect(handle);

                // ウィンドウ全体とクライアント領域のサイズ差から、
                // 左右下のバーの幅を取得し全体のサイズ位置を補正する
                if (window != null && client != null)
                {
                    float barWidth = (window.SizeWidth - client.SizeWidth) / 2;
                    windowSizePoint = new SizePoint(
                        sizeWidth: window.SizeWidth - (int)((barWidth * 2)),
                        sizeHeight: window.SizeHeight - (int)Math.Floor(barWidth + 1),
                        positionX: window.PositionX + (int)Math.Floor(barWidth),
                        positionY: window.PositionY + 1);
                }
            }

            // ウィンドウサイズが取得できない場合はディスクトップのサイズ位置を取得する
            if (windowSizePoint == null)
            {
                HandleRef handle = new HandleRef(0, WindowOperate.GetDesktopWindow());
                windowSizePoint = WindowOperate.GetWindowRect(handle);
            }

            // 上記ウィンドウの存在するScreenを取得
            Screen screen;
            if (windowSizePoint != null)
            {
                screen = Screen.FromPoint(windowSizePoint.Point);
            }
            else
            {
                // ウィンドウサイズが取得できない場合は
                // プライマリウィンドウの情報からディスクトップのウィンドウサイズを設定する
                screen = Screen.PrimaryScreen;
                windowSizePoint = new SizePoint(screen.Bounds.Size, screen.Bounds.Location);
            }

            // ディスクトップの対象領域のキャプチャを取得
            Bitmap captureImage = DesktopWindowCapture(
                windowSizePoint, screen, out SizePoint displayedSizePoint);

            // ウィンドウのサイズ位置 及び、画面情報、画像を返却
            targetWindowSizePoint = displayedSizePoint;
            return captureImage;
        }

        /// <summary>
        /// ディスクトップをキャプチャする
        /// </summary>
        /// <param name="windowSizePoint">
        /// 対象のウィンドウのサイズ位置情報
        /// </param>
        /// <param name="targetScreen">
        /// 対象のウィンドウが存在する画面
        /// </param>
        /// <param name="displayedSizePoint">
        /// 対象のウィンドウにおいて画面に表示している領域の情報を返却
        /// </param>
        /// <exception cref="Win32Exception">
        /// キャプチャデータ取得に用いる <see cref="Graphics.CopyFromScreen(Point, Point, Size, CopyPixelOperation)"/>
        /// において、Win32Apiの操作に失敗した場合に発生
        /// </exception>
        /// <exception cref="Exception">
        /// キャプチャデータを格納する <see cref="Bitmap"/> オブジェクトが生成できない場合に発生
        /// </exception>
        /// <returns>キャプチャした画像データ（キャプチャできない場合は NULL を返却）</returns>
        private static Bitmap DesktopWindowCapture(
            SizePoint windowSizePoint, Screen targetScreen, out SizePoint displayedSizePoint)
        {
            // キャプチャ用のBitmapの変数を宣言
            Bitmap desktopImage = null;
            try
            {
                // 画面に表示されている領域の画面サイズを取得
                SizePoint displayed = GetDisplayedSizePoint(windowSizePoint, targetScreen);
                if (displayed.SizeWidth <= 0 || displayed.SizeHeight <= 0)
                {
                    // 表示領域が存在しない場合は対象スクリーン全体を対象とする
                    displayed = new SizePoint(targetScreen.Bounds.Size, targetScreen.Bounds.Location);
                }

                // キャプチャ用のBitmapを生成
                desktopImage = new Bitmap(displayed.Size.Width, displayed.Size.Height);

                // 画面のキャプチャを取得
                using (Graphics graphics = Graphics.FromImage(desktopImage))
                {
                    // 背景を黒にする
                    graphics.CopyFromScreen(
                        upperLeftSource: new Point(0, 0),
                        upperLeftDestination: new Point(0, 0),
                        blockRegionSize: displayed.Size,
                        copyPixelOperation: CopyPixelOperation.Blackness);

                    // 画面のキャプチャを描画
                    graphics.CopyFromScreen(
                        upperLeftSource: displayed.Point,
                        upperLeftDestination: new Point(0, 0),
                        blockRegionSize: displayed.Size,
                        copyPixelOperation: CopyPixelOperation.SourceCopy);
                }

                // キャプチャした画像データを返却
                displayedSizePoint = displayed;
                return desktopImage;
            }
            catch (Exception)
            {
                // 例外発生時、キャプチャ用のBitmapオブジェクトを破棄する
                desktopImage?.Dispose();

                // 発生した例外はそのままスロー
                throw;
            }
        }

        /// <summary>
        /// 対象のウィンドウにおいて画面に表示されている領域のサイズ位置を取得する
        /// </summary>
        /// <param name="windowSizePoint">対象のウィンドウのサイズ位置情報</param>
        /// <param name="targetScreen">対象のウィンドウが存在する画面</param>
        /// <returns>画面に表示されている領域のサイズ位置</returns>
        private static SizePoint GetDisplayedSizePoint(SizePoint windowSizePoint, Screen targetScreen)
        {
            // ウインドウと画面の四隅の情報を取得
            Rectangle windowRect = new Rectangle(
                windowSizePoint.Point, windowSizePoint.Size);
            Rectangle screenRect = new Rectangle(
                targetScreen.WorkingArea.Location, targetScreen.WorkingArea.Size);

            // ウィンドウの左上の座標を取得
            Point windowLocation = windowSizePoint.Point;

            // 画面からはみ出した領域を計算
            // 幅
            int overWidth;
            if (windowRect.Left < screenRect.Left)
            {
                // 左にはみ出している場合、はみ出した分を取得
                overWidth = screenRect.Left - windowRect.Left;

                // 左にはみ出した分、左上の座標を補正する
                windowLocation.X += overWidth;
            }
            else if (windowRect.Right > screenRect.Right)
            {
                // 右にはみ出している場合、はみ出した分を取得
                overWidth = windowRect.Right - screenRect.Right;
            }
            else
            {
                // 上記以外の場合、はみ出していないため 0 を設定
                overWidth = 0;
            }

            // 高さ
            int overHeight;
            if (windowRect.Top < screenRect.Top)
            {
                // 上にはみ出している場合、はみ出した分を取得
                overHeight = screenRect.Top - windowRect.Top;

                // 上にはみ出した分、左上の座標を補正する
                windowLocation.Y += overHeight;
            }
            else if (windowRect.Bottom > screenRect.Bottom)
            {
                // 下にはみ出している場合、はみ出した分を取得
                overHeight = windowRect.Bottom - screenRect.Bottom;
            }
            else
            {
                // 上記以外の場合、はみ出していないため 0 を設定
                overHeight = 0;
            }

            // 画面に表示されている領域のサイズ位置を設定
            SizePoint displayedSizePoint = new SizePoint(
                sizeWidth: windowSizePoint.SizeWidth - overWidth,
                sizeHeight: windowSizePoint.SizeHeight - overHeight,
                positionX: windowLocation.X,
                positionY: windowLocation.Y);

            // 生成したサイズ位置情報を返す
            return displayedSizePoint;
        }

        #endregion
    }
}
