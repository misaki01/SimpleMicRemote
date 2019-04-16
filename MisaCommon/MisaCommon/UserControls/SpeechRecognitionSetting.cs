namespace MisaCommon.UserControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using MisaCommon.CustomType;
    using MisaCommon.MessageResources.UserControl;

    /// <summary>
    /// 音声認識に対するマッチングパターンと操作を設定するためのユーザコントロール
    /// </summary>
    public partial class SpeechRecognitionSetting : UserControl
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各コントロールの初期化を行う
        /// </summary>
        public SpeechRecognitionSetting()
        {
            InitializeComponent();

            // マッチングメッセージのテキストボックスの初期値、色を保持する
            DefaultMatchMessage = TxtMatchMessage.Text;
            DefaultMatchMessageForeColor = TxtMatchMessage.ForeColor;

            // マッチングメッセージのテキストボックスの入力フラグ、文字色を初期化
            IsSetMatchMessage = false;
            SetMatchMessageForeColor(IsSetMatchMessage);

            // マッチングパターンのラジオボタン領域は非表示
            PlMatchPattern.Visible = false;

            // 拡大縮小ボタンの初期化
            IsSummary = true;
            SetBackgroundImageForBtMinMax(IsSummary);

            // 概要ラベルの初期値設定
            DefaultSummaryTextForeColor = LbSummaryText.ForeColor;
            SetSummaryText();
            LbSummaryText.Visible = true;

            // 操作設定用のコントロールの初期設定
            SettingControl = null;
        }

        /// <summary>
        /// コンストラクタ
        /// 各コントロールの初期化を行う
        /// </summary>
        /// <param name="settingInfo">
        /// 音声認識に対するマッチングパターンと操作の設定情報
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// 引数の <paramref name="settingInfo"/> がNULLの場合に発生
        /// </exception>
        public SpeechRecognitionSetting(SpeechRecognitionSettingInfo settingInfo)
            : this()
        {
            // NULLチェック
            if (settingInfo == null)
            {
                throw new ArgumentNullException(nameof(settingInfo));
            }

            // 各コントロールの初期化を行う
            Initialize(settingInfo);
        }

        #endregion

        #region 公開プロパティ

        /// <summary>
        /// 現在の音声認識に対するマッチングパターンと操作を設定情報を取得・設定する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public SpeechRecognitionSettingInfo CurrentSpeechRecognitionSettingInfo { get; set; }

        /// <summary>
        /// このコントロールで設定した音声認識に対するマッチングパターンと操作を設定情報を取得・設定する
        /// （未設定、キャンセルの場合はNULL）
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public SpeechRecognitionSettingInfo SettingSpeechRecognitionSettingInfo => SettingInfo?.DeepCopy();

        #endregion

        #region プライぺートプロパティ

        /// <summary>
        /// マッチングパターンが未入力時に表示するデフォルト文言の文字色を取得する
        /// </summary>
        private static Color NotInputMatchMessageForeColor => Color.Gray;

        /// <summary>
        /// マッチングメッセージとパターンの入力がエラーの場合に、
        /// 概要ラベルに表示するエラーメッセージの文字色を取得する
        /// （マッチングパターンが正規表現の場合で、メッセージが正規表現として不正な場合に使用する）
        /// </summary>
        private static Color ErrorSummaryTextForeColor => Color.Red;

        /// <summary>
        /// マッチングメッセージテキストボックスのデフォルト値を取得する
        /// 値はコンストラクタでデザイナで指定された文言を設定する
        /// </summary>
        private string DefaultMatchMessage { get; }

        /// <summary>
        /// マッチングメッセージテキストボックスのデフォルトの文字色を取得する
        /// 値はコンストラクタでデザイナで指定された文字色を設定する
        /// </summary>
        private Color DefaultMatchMessageForeColor { get; }

        /// <summary>
        /// マッチングメッセージテキストボックスに入力があるか判定するフラグを取得・設定する
        /// </summary>
        private bool IsSetMatchMessage { get; set; }

        /// <summary>
        /// 概要表示ラベルのデフォルトの文字色を取得する
        /// 値はコンストラクタでデザイナで指定された文字色を設定する
        /// </summary>
        private Color DefaultSummaryTextForeColor { get; }

        /// <summary>
        /// 概要表示しているかどうかを示すフラグを取得・設定する
        /// </summary>
        private bool IsSummary { get; set; }

        /// <summary>
        /// 操作設定用のコントロールを取得・設定する
        /// 縮小（概要）表示の場合コントロールオブジェクトを解放するためNULLとする
        /// </summary>
        private Control SettingControl { get; set; }

        /// <summary>
        /// 音声認識に対応する文言と操作の設定情報を取得・設定する
        /// </summary>
        private SpeechRecognitionSettingInfo SettingInfo { get; set; } = new SpeechRecognitionSettingInfo();

        #endregion

        #region 公開メソッド

        /// <summary>
        /// 各コントロールの初期化を行う
        /// </summary>
        /// <param name="settingInfo">
        /// 音声認識に対するマッチングパターンと操作の設定情報
        /// </param>
        public void Initialize(SpeechRecognitionSettingInfo settingInfo)
        {
            // 引数の音声認識に対するマッチングパターンと操作の設定情報を保持する
            // NULLが指定された場合は初期値を設定する
            CurrentSpeechRecognitionSettingInfo = settingInfo;
            SettingInfo = settingInfo ?? new SpeechRecognitionSettingInfo();
            SpeechRecognitionSettingInfo info = SettingInfo.DeepCopy();

            // マッチングメッセージのテキストボックスを設定
            if (!string.IsNullOrEmpty(info.MatchMessage))
            {
                TxtMatchMessage.Text = info.MatchMessage;
                IsSetMatchMessage = true;
            }
            else
            {
                // マッチングメッセージが存在しない場合はデフォルトの値を設定する
                TxtMatchMessage.Text = DefaultMatchMessage;
                IsSetMatchMessage = false;
            }

            // マッチングメッセージの文字色を初期化
            SetMatchMessageForeColor(IsSetMatchMessage);

            // マッチングパターンのラジオボタン領域は非表示
            PlMatchPattern.Visible = false;

            // 設定情報のマッジングパターンに紐づくラジオボタンコントロールを取得
            RadioButton radio = GetRadio(info.MatchPattern);

            // ラジオボタンコントロールが取得できた場合はそのラジオボタンをチェックする
            if (radio != null)
            {
                radio.Checked = true;
            }
            else if ((radio = GetCheckedRadio()) != null)
            {
                // チェック対象のラジオボタンが存在しない場合は、
                // チェックされているラジオボタンのチェックを外し、すべてが未チェック状態にする
                radio.Checked = false;
            }

            // 拡大縮小ボタンの初期化
            IsSummary = true;
            SetBackgroundImageForBtMinMax(IsSummary);

            // 概要ラベルを設定
            SetSummaryText(info);
            LbSummaryText.Visible = true;

            // 操作設定用のコントロールを設定
            // （非表示にするためコントロールオブジェクトを解放する）
            if (SettingControl != null)
            {
                PlControl.Controls.Remove(SettingControl);
                SettingControl.Dispose();
                SettingControl = null;
            }
        }

        #endregion

        #region イベントで呼び出されるメソッド

        /// <summary>
        /// マッチングメッセージテキストボックスのフォーカスEnterのイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void TxtMatchMessage_Enter(object sender, EventArgs e)
        {
            // マッチングメッセージが未設定の場合、
            // メッセージ設定フラグをONにし、デフォルトのマッチングメッセージをクリアする
            if (!IsSetMatchMessage)
            {
                IsSetMatchMessage = true;
                TxtMatchMessage.Text = string.Empty;
            }

            // マッチングメッセージ設定有無に応じた文字色を設定する
            SetMatchMessageForeColor(IsSetMatchMessage);
        }

        /// <summary>
        /// マッチングメッセージテキストボックスのフォーカスLeaveのイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void TxtMatchMessage_Leave(object sender, EventArgs e)
        {
            // マッチングメッセージが入力されたか判定する
            if (string.IsNullOrEmpty(TxtMatchMessage.Text))
            {
                // 入力されていない場合
                // 設定フラグをOFFにしデフォルトメッセージを設定する
                IsSetMatchMessage = false;
                TxtMatchMessage.Text = DefaultMatchMessage;
            }
            else
            {
                // 入力されている場合
                // 設定フラグをONにする
                IsSetMatchMessage = true;
            }

            // マッチングメッセージ設定有無に応じた文字色を設定する
            SetMatchMessageForeColor(IsSetMatchMessage);

            // 概要表示用のラベルの表示を更新する
            SetSummaryText(SettingInfo);
        }

        /// <summary>
        /// マッチングメッセージを変更のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void TxtMatchMessage_TextChanged(object sender, EventArgs e)
        {
            // マッチングメッセージ設定フラグがONの場合
            // 設定情報のマッチングメッセージを入力値に更新する
            if (IsSetMatchMessage)
            {
                SettingInfo.MatchMessage = string.IsNullOrEmpty(TxtMatchMessage.Text)
                    ? null : TxtMatchMessage.Text;
            }
        }

        /// <summary>
        /// マッチングパターンラジオボタンのチェック変更のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void MatchPatternRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            // チェック状態のラジオボタンコントロールを取得する
            // （チェック情報のラジオボタンが存在しない場合はNULLとなる）
            RadioButton radio = GetCheckedRadio();

            // 設定情報のマッチングパターンを更新する
            SettingInfo.MatchPattern = radio?.Tag.ToString();

            // 概要表示用のラベルの表示を更新する
            SetSummaryText(SettingInfo);
        }

        /// <summary>
        /// 拡大縮小表示の切り替えボタン押下のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtMinMax_Click(object sender, EventArgs e)
        {
            // 概要表示フラグを切り替える
            IsSummary = !IsSummary;

            // マッチングパターンのラジオボタン領域の表示を切り替える
            PlMatchPattern.Visible = !IsSummary;

            // 拡大縮小ボタンの表示を切り替える
            SetBackgroundImageForBtMinMax(IsSummary);

            // 操作設定エリアの表示を切り替える
            if (IsSummary)
            {
                // 縮小の場合
                // 概要ラベルを表示する
                LbSummaryText.Visible = true;
                SetSummaryText(SettingInfo);

                // 操作設定用のコントロールを設定
                // （非表示にするためコントロールオブジェクトを解放する）
                if (SettingControl != null)
                {
                    PlControl.Controls.Remove(SettingControl);
                    SettingControl.Dispose();
                    SettingControl = null;
                }
            }
            else
            {
                // 拡大の場合
                // 操作設定用のコントロールを取得する
                SettingControl?.Dispose();
                SettingControl = SettingInfo.SettingControl;
                if (SettingControl != null)
                {
                    // 操作設定用のコントロールが存在する場合
                    // 概要ラベルを設定
                    LbSummaryText.Visible = false;

                    // 設定変更イベントを設定
                    (SettingControl as ISpeechRecognitionSettingControl).SettingDataChanged
                        += new EventHandler(SettingControl_SettingDataChanged);

                    // 操作設定用のコントロールを設定
                    PlControl.Controls.Add(SettingControl);
                }
                else
                {
                    // 操作設定用のコントロールが存在しない場合
                    // 概要ラベルを表示する
                    LbSummaryText.Visible = true;
                    SetSummaryText(SettingInfo);
                }
            }
        }

        /// <summary>
        /// 設定情報の設定データ変更のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void SettingControl_SettingDataChanged(object sender, EventArgs e)
        {
            // 設定情報の設定データを更新する
            SettingInfo.SettingData
                = (SettingControl as ISpeechRecognitionSettingControl)?.GetSettingData;

            // 概要表示用のラベルの表示を更新する
            SetSummaryText(SettingInfo);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// マッチングメッセージの文字色を設定する
        /// </summary>
        /// <param name="isSetMatchMessage">マッチングメッセージが設定されているかのフラグ</param>
        private void SetMatchMessageForeColor(bool isSetMatchMessage)
        {
            TxtMatchMessage.ForeColor = isSetMatchMessage
                ? DefaultMatchMessageForeColor : NotInputMatchMessageForeColor;
        }

        /// <summary>
        /// 引数のタグデータ（<paramref name="tagData"/>）に紐づくラジオボタンのコントロールを取得する
        /// </summary>
        /// <returns>
        /// 引数のタグデータ（<paramref name="tagData"/>）に紐づくラジオボタンのコントロール、
        /// 該当するものが存在しない場合はNULLを返却
        /// </returns>
        /// <param name="tagData">ラジオボタンのタグデータ</param>
        private RadioButton GetRadio(string tagData)
        {
            foreach (Control control in PlMatchPattern.Controls)
            {
                if (control is RadioButton radio && radio.Tag.Equals(tagData))
                {
                    return radio;
                }
            }

            return null;
        }

        /// <summary>
        /// チェックされているラジオボタンのコントロールを取得する
        /// </summary>
        /// <returns>
        /// チェックされているラジオボタンのコントロール、
        /// 全てが未チェックの場合はNULLを返却
        /// </returns>
        private RadioButton GetCheckedRadio()
        {
            foreach (Control control in PlMatchPattern.Controls)
            {
                if (control is RadioButton radio && radio.Checked)
                {
                    return radio;
                }
            }

            return null;
        }

        /// <summary>
        /// 拡大縮小ボタンの背景画像を設定する
        /// </summary>
        /// <param name="isSummary">
        /// 縮小表示かどうかのフラグ
        /// 縮小表示：True、拡大表示：False
        /// </param>
        private void SetBackgroundImageForBtMinMax(bool isSummary)
        {
            BtMinMax.BackgroundImage.Dispose();
            BtMinMax.BackgroundImage = isSummary
                ? SpeechRecognitionSettingMessage.TriangleRight
                : SpeechRecognitionSettingMessage.TriangleBottom;
        }

        /// <summary>
        /// 音声認識に対応する文言と操作の設定情報から取得した概要文字列を表示対象のラベルに設定する
        /// </summary>
        /// <param name="settingInfo">音声認識に対応する文言と操作の設定情報</param>
        private void SetSummaryText(SpeechRecognitionSettingInfo settingInfo = null)
        {
            // マッチングメッセージのチェックを行う
            if (!CheckMatchMessage(out string errorMessage))
            {
                // マッチングメッセージがエラーの場合はエラーメッセージを設定して処理を終了する
                LbSummaryText.ForeColor = ErrorSummaryTextForeColor;
                LbSummaryText.Text = errorMessage;
                return;
            }

            // 処理内容のメッセージを生成する
            string summary = settingInfo?.SummaryText;
            if (settingInfo == null || settingInfo.SettingType.Equals(SpeechRecognitionSettingType.None))
            {
                // 設定情報が存在しない または、タイプが「操作なし」の場合
                // 「使用不可」を設定する
                summary = SpeechRecognitionSettingMessage.SummaryTextNotUse;
            }
            else if (string.IsNullOrEmpty(summary))
            {
                // 概要文字列が存在しない場合は、「未設定」を設定する
                summary = SpeechRecognitionSettingMessage.SummaryTextNotSet;
            }

            // フォーマットに従って概要文字列を生成しラベルに設定す
            LbSummaryText.ForeColor = DefaultSummaryTextForeColor;
            LbSummaryText.Text = string.Format(
                CultureInfo.InvariantCulture,
                SpeechRecognitionSettingMessage.SummaryTextFormat,
                GetCheckedRadio()?.Text ?? SpeechRecognitionSettingMessage.SummaryTextNoneMatchPattern,
                summary);
        }

        /// <summary>
        /// マッチングメッセージがエラーか判定します
        /// </summary>
        /// <param name="errorMessage">
        /// エラーの場合、エラーメッセージを返却
        /// 正常の場合はNULLを返却
        /// </param>
        /// <returns>正常の場合：True、エラーの場合：False</returns>
        private bool CheckMatchMessage(out string errorMessage)
        {
            // 正常時のエラーメッセージを設定
            errorMessage = null;

            // マッチングメッセージが未入力の場合は正常を返却
            if (!IsSetMatchMessage || string.IsNullOrEmpty(TxtMatchMessage.Text))
            {
                return true;
            }

            // チェック状態のマッチングパターンのラベルを取得する
            // 取得できない場合（全てのラジオボタンが未チェック状態）は正常を返却
            RadioButton radio = GetCheckedRadio();
            if (radio == null)
            {
                return true;
            }

            // マッチングパターンが正規表現の場合、
            // メッセージが正規表現として問題ないかチェックする
            if (radio.Equals(RdoRegularExpression))
            {
                // 正規表現として生成できるかチェック
                try
                {
                    Regex regex = new Regex(TxtMatchMessage.Text);
                    regex.IsMatch(string.Empty);
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException
                        || ex is RegexMatchTimeoutException)
                    {
                        // 正規表現として異常の場合、異常を返却
                        errorMessage = SpeechRecognitionSettingMessage.SummaryTextErrorRegularExpression;
                        return false;
                    }
                    else
                    {
                        // 上記以外のエラーの場合は想定外のエラーのため例外をスローする
                        throw;
                    }
                }
            }

            // 全てのチェックがOKの場合、正常を返却
            return true;
        }

        #endregion
    }
}
