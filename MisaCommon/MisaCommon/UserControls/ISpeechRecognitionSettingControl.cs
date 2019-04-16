namespace MisaCommon.UserControls
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// 音声認識に対するマッチングパターンと操作を設定するためのユーザコントロール
    /// （<see cref="SpeechRecognitionSetting"/>）において、
    /// 操作設定のコントロールとして使用可能とするためのインターフェース
    /// </summary>
    public interface ISpeechRecognitionSettingControl
    {
        /// <summary>
        /// 設定データを変更した際に発生させるイベント
        /// </summary>
        event EventHandler SettingDataChanged;

        /// <summary>
        /// 操作設定の設定データ（文字列形式）を取得する
        /// </summary>
        /// <returns>操作設定の設定データ（文字列形式）</returns>
        string GetSettingData { get; }

        /// <summary>
        /// 設定データの概要を示すテキストを取得する
        /// </summary>
        /// <param name="settingData">設定データ</param>
        /// <returns>設定データの概要を示すテキスト</returns>
        string GetSummaryText(string settingData);

        /// <summary>
        /// 操作設定のコントロールを引数の設定データ（<paramref name="settingData"/>）で
        /// 初期化したコントロールを取得する
        /// </summary>
        /// <param name="settingData">設定データ</param>
        /// <returns>
        /// 操作設定のコントロールを引数の設定データ（<paramref name="settingData"/>）で
        /// 初期化したコントロール
        /// </returns>
        Control GetInitializeControl(string settingData);
    }
}
