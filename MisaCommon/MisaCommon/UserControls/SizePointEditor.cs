namespace MisaCommon.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using MisaCommon.CustomType;

    /// <summary>
    /// サイズ・位置を設定するためのユーザコントロール
    /// </summary>
    public partial class SizePointEditor : UserControl
    {
        #region クラス変数・定数

        /// <summary>
        /// ドロップダウンで表示しているかのフラグ
        /// </summary>
        private readonly bool _isDropDown;

        /// <summary>
        /// このコントロールで設定したサイズと位置の値（未設定、キャンセルの場合はNULL）
        /// </summary>
        private SizePoint _settingSizePoint = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// このユーザコントロールを初期化する
        /// </summary>
        /// <remarks>
        /// .NETのデザイナーで表示するためのコンストラクタ
        /// </remarks>
        public SizePointEditor()
            : this(null, null)
        {
            // 下記のコンストラクタを呼び出しており、そこで処理を行うためここでは処理をしない
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の値でこのユーザコントロールを初期化する
        /// </summary>
        /// <param name="currentSizePoint">設定対象とするFormコントロールの現在のサイズと位置を指定</param>
        public SizePointEditor(SizePoint currentSizePoint)
            : this(currentSizePoint, null)
        {
            // 下記のコンストラクタを呼び出しており、そこで処理を行うためここでは処理をしない
        }

        /// <summary>
        /// コンストラクタ
        /// 引数の値でこのユーザコントロールを初期化する
        /// </summary>
        /// <param name="currentSizePoint">設定対象とするFormコントロールの現在のサイズと位置を指定</param>
        /// <param name="isDropDown">ドロップダウンで表示するかのフラグ</param>
        public SizePointEditor(SizePoint currentSizePoint, bool? isDropDown)
        {
            // デザイナで生成された設定を行う
            InitializeComponent();

            // 現在の設定値を保持
            // （NULLの場合はSizePointオブジェクトの初期値を使用）
            CurrentSizePoint = currentSizePoint ?? new SizePoint();

            // ドロップダウンで表示しているかのフラグを保持
            // （NULLの場合はFalse：ドロップダウンでの表示ではないとする）
            _isDropDown = isDropDown ?? false;

            // 現在の設定値を元に各コントロールに値を設定
            SetCurrentSizePoint(CurrentSizePoint);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 設定対象とする <see cref="Form"/> コントロールの現在のサイズと位置を取得・設定する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public SizePoint CurrentSizePoint { get; set; }

        /// <summary>
        /// このコントロールで設定したサイズと位置の値（未設定、キャンセルの場合はNULL）を取得・設定する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public SizePoint SettingSizePoint
        {
            get => _settingSizePoint?.DeepCopy();
            private set => _settingSizePoint = value;
        }

        #endregion

        #region イベントで呼び出されるメソッド

        #region OK、キャンセルボタン

        /// <summary>
        /// OKボタン押下イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtOk_Click(object sender, EventArgs e)
        {
            try
            {
                // 現在の入力値をプロパティに設定する
                SetSizePointProperty();

                // ドロップダウンでの表示の場合、ドロップダウンを閉じる
                CloseDropDown();
            }
            catch (InvalidOperationException)
            {
                // キーストロークの送信先となるアクティブなアプリケーションが存在しない場合に発生
                // ドロップダウンを閉じるための処理のためエラーが発生してもなにもしない
            }
        }

        /// <summary>
        /// キャンセルボタン押下イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            try
            {
                // プロパティを未設定に戻す
                SettingSizePoint = null;

                // ドロップダウンでの表示の場合、ドロップダウンを閉じる
                CloseDropDown();
            }
            catch (InvalidOperationException)
            {
                // キーストロークの送信先となるアクティブなアプリケーションが存在しない場合に発生
                // ドロップダウンを閉じるための処理のためエラーが発生してもなにもしない
            }
        }

        #endregion

        #region 四隅の座標に設定ボタン

        /// <summary>
        /// 左上ボタン押下イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtTopLeft_Click(object sender, EventArgs e)
        {
            // 左下の座標を設定
            TxtPositionX.Value = 0;
            TxtPositionY.Value = 0;

            // 現在の入力値をプロパティに設定する
            SetSizePointProperty();
        }

        /// <summary>
        /// 右上ボタン押下イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtTopRight_Click(object sender, EventArgs e)
        {
            // 右下の座標を設定
            // 座標はコントロールの左上の座標のためコントロールのサイズ分引く
            Screen screen = Screen.FromControl(this);
            decimal positionX = screen.Bounds.Width - TxtSizeWidth.Value;
            TxtPositionX.Value = positionX < TxtPositionX.Minimum ? TxtPositionX.Minimum : positionX;
            TxtPositionY.Value = 0;

            // 現在の入力値をプロパティに設定する
            SetSizePointProperty();
        }

        /// <summary>
        /// 左下ボタン押下イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtBottomLeft_Click(object sender, EventArgs e)
        {
            // 左下の座標を設定
            // 座標はコントロールの左上の座標のためコントロールのサイズ分引く
            Screen screen = Screen.FromControl(this);
            decimal positionY = screen.Bounds.Height - TxtSizeHeight.Value;
            TxtPositionX.Value = 0;
            TxtPositionY.Value = positionY < TxtPositionY.Minimum ? TxtPositionY.Minimum : positionY;

            // 現在の入力値をプロパティに設定する
            SetSizePointProperty();
        }

        /// <summary>
        /// 右下ボタン押下イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtBottomRight_Click(object sender, EventArgs e)
        {
            // 右下の座標を設定
            // 座標はコントロールの左上の座標のためコントロールのサイズ分引く
            Screen screen = Screen.FromControl(this);
            decimal positionX = screen.Bounds.Width - TxtSizeWidth.Value;
            decimal positionY = screen.Bounds.Height - TxtSizeHeight.Value;
            TxtPositionX.Value = positionX < TxtPositionX.Minimum ? TxtPositionX.Minimum : positionX;
            TxtPositionY.Value = positionY < TxtPositionY.Minimum ? TxtPositionY.Minimum : positionY;

            // 現在の入力値をプロパティに設定する
            SetSizePointProperty();
        }

        #endregion

        #region サイズ・位置入力のテキストボックスでのキー押下

        /// <summary>
        /// サイズ・位置入力のテキストボックスでのキー押下イベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void TxtSizePosition_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter、Escapeが押下されたか判定
            bool isPress = false;
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                case (char)Keys.Escape:
                    isPress = true;
                    break;
                default:
                    isPress = false;
                    break;
            }

            // Enter、Escapeで音が鳴らないようtrueを返す
            if (isPress)
            {
                e.Handled = true;
            }

            // 現在の入力値をプロパティに設定する
            SetSizePointProperty();
        }

        #endregion

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// ドロップダウンでの表示の場合にドロップダウンを閉じる
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// キーストロークの送信先となるアクティブなアプリケーションが存在しない場合に発生
        /// </exception>
        private void CloseDropDown()
        {
            // ドロップダウンでの表示の場合、ESCキーを送信してドロップダウンを閉じる
            if (_isDropDown)
            {
                SendKeys.Send("{ESC}");
            }
        }

        /// <summary>
        /// 現在の設定値を元に各コントロールに値を設定する
        /// </summary>
        /// <param name="currentSizePoint">
        /// 設定対象とする <see cref="Form"/> コントロールの現在のサイズと位置
        /// </param>
        private void SetCurrentSizePoint(SizePoint currentSizePoint)
        {
            // サイズと位置の初期値設定
            TxtSizeWidth.Value = currentSizePoint.SizeWidth;
            TxtSizeHeight.Value = currentSizePoint.SizeHeight;
            TxtPositionX.Value = currentSizePoint.PositionX;
            TxtPositionY.Value = currentSizePoint.PositionY;

            // 画面からはみ出さないようにサイズの最大値を設定
            Screen screen = Screen.FromControl(this);
            decimal maximunWidth = TxtSizeWidth.Value > screen.Bounds.Width ? TxtSizeWidth.Value : screen.Bounds.Width;
            decimal maximunHeight = TxtSizeHeight.Value > screen.Bounds.Height ? TxtSizeHeight.Value : screen.Bounds.Height;
            TxtSizeWidth.Maximum = maximunWidth;
            TxtSizeHeight.Maximum = maximunHeight;

            // 画面から大きくはみ出さないように座標の最大値、最小値を設定
            decimal maximunX = TxtPositionX.Value > screen.Bounds.Width ? TxtPositionX.Value : screen.Bounds.Width;
            decimal maximunY = TxtPositionY.Value > screen.Bounds.Height ? TxtPositionY.Value : screen.Bounds.Height;
            TxtPositionX.Maximum = maximunX;
            TxtPositionX.Minimum = -1 * maximunX;
            TxtPositionY.Maximum = maximunY;
            TxtPositionY.Minimum = -1 * maximunY;
        }

        /// <summary>
        /// 現在の入力値を「このコントロールで設定したサイズと位置の値」プロパティに設定する
        /// </summary>
        private void SetSizePointProperty()
        {
            SettingSizePoint = new SizePoint(
                sizeWidth: (int)TxtSizeWidth.Value,
                sizeHeight: (int)TxtSizeHeight.Value,
                positionX: (int)TxtPositionX.Value,
                positionY: (int)TxtPositionY.Value);
        }

        #endregion
    }
}
