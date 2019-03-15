namespace MisaCommon.CustomType.UiEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    using MisaCommon.UserControls;

    /// <summary>
    /// プロパティグリッドにおいて <see cref="InputKey"/> 型の値を編集するためのユーザーインターフェイスを提供する
    /// <see cref="UITypeEditor"/> の派生クラス
    /// </summary>
    internal class InputKeyUIEditor : UITypeEditor
    {
        #region プロパティ

        /// <summary>
        /// ユーザーがドロップダウン エディターのサイズを変更できるかどうかを示す値を取得する
        /// </summary>
        /// <remarks>常に変更を可能とするため Trueのみを返却する</remarks>
        public override bool IsDropDownResizable => true;

        #endregion

        #region メソッド

        /// <summary>
        /// <see cref="GetEditStyle(ITypeDescriptorContext)"/> メソッドで指定されたエディタースタイルを使用して、
        /// 指定したオブジェクトの値を編集する
        /// </summary>
        /// <param name="context">
        /// 追加のコンテキスト情報を取得するために使用する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <param name="provider">
        /// エディターがサービスを取得するために使用する <see cref="IServiceProvider"/> オブジェクト
        /// </param>
        /// <param name="value">
        /// 編集対象のオブジェクト
        /// </param>
        /// <returns>
        /// オブジェクトの新しい値
        /// オブジェクトの値が変更されない場合は、引数の<paramref name="value"/>オブジェクトをそのまま返す
        /// </returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // 編集対象のオブジェクトの型チェック
            if (!(value is InputKey inputKeyValue))
            {
                // 対象の型でない場合は編集を行わず、引数の値をそのまま返す
                return value;
            }

            // IWindowsFormsEditorServiceを使用してドロップダウンのUIのプロパティウィンドウを表示する
            if (!(provider?.GetService(typeof(IWindowsFormsEditorService)) is IWindowsFormsEditorService editorService))
            {
                // IWindowsFormsEditorServiceが取得できない場合は編集を行わず、引数の値をそのまま返す
                return value;
            }

            // エディターのユーザインターフェースを生成しドロップダウンで表示する
            InputKey inputKey = null;
            using (KeyInputSetting editorUI = new KeyInputSetting(inputKeyValue))
            {
                // 表示設定
                editorUI.EnabledImeKey = false;
                editorUI.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                editorUI.AutoSizeWidth = true;
                editorUI.AutoSizeHeight = true;
                editorService.DropDownControl(editorUI);
                inputKey = editorUI.SettingInputKey;
            }

            // エディタで設定した値を返却
            // 設定値がNULLの場合は編集を行わず、引数の値をそのまま返す
            return inputKey ?? value;
        }

        /// <summary>
        /// <see cref="UITypeEditor.EditValue(IServiceProvider, object)"/> メソッドで使用するエディターのスタイルを取得する
        /// </summary>
        /// <param name="context">
        /// 追加のコンテキスト情報を取得するために使用する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <returns>
        /// <see cref="UITypeEditor.EditValue(IServiceProvider, object)"/> メソッドで使用するエディターのスタイル
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            // ドロップダウン形式で表示するため、ドロップダウンのスタイルを返却
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 引数で指定したコンテキスト（<paramref name="context"/>）内で、
        /// オブジェクトの値の視覚的な表現を描画できるかどうかを取得する
        /// </summary>
        /// <param name="context">
        /// 追加のコンテキスト情報を取得するために使用する <see cref="ITypeDescriptorContext"/> オブジェクト
        /// </param>
        /// <returns>
        /// <see cref="UITypeEditor.PaintValue(object, Graphics, Rectangle)"/> が実装されている場合は True
        /// それ以外の場合は False。
        /// </returns>
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            // UITypeEditor.PaintValueは不要のため実装していないそのためFalseを返却
            return false;
        }

        #endregion
    }
}
