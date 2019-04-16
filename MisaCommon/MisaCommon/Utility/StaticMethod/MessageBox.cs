namespace MisaCommon.Utility.StaticMethod
{
    using System;
    using System.Globalization;
    using System.Windows.Forms;

    using MisaCommon.MessageResources;

    using Original = System.Windows.Forms.MessageBox;

    /// <summary>
    /// ユーザーに対してメッセージを表示するメッセージウィンドウを表示する機能に関するクラス
    /// このメッセージウィンドウはモーダルウィンドウであり、
    /// ユーザーが閉じるまでこのアプリケーションでの他の操作をブロックする
    /// </summary>
    public static class MessageBox
    {
        #region メソッド_Owner付き

        /// <summary>
        /// 情報のメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowInfo(IWin32Window owner, string message)
        {
            // 情報のメッセージボックスを表示する
            Show(
                owner: owner,
                text: message,
                caption: ErrorMessage.TitleInfo,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Information);
        }

        /// <summary>
        /// 確認のメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult ShowConfirm(IWin32Window owner, string message)
        {
            // 確認のメッセージボックスを表示する
            return Show(
                owner: owner,
                text: message,
                caption: ErrorMessage.TitleConfirm,
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Question,
                defaultButton: MessageBoxDefaultButton.Button2);
        }

        /// <summary>
        /// 注意のメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowAttention(IWin32Window owner, string message)
        {
            // 注意のメッセージボックスを表示する
            Show(
                owner: owner,
                text: message,
                caption: ErrorMessage.TitleAttention,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// ワーニングのメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowWarning(IWin32Window owner, string message)
        {
            // ワーニングメッセージを表示する
            Show(
                owner: owner,
                text: message,
                caption: ErrorMessage.TitleWarning,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Warning);
        }

        /// <summary>
        /// エラーのメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowError(IWin32Window owner, string message)
        {
            // エラーのメッセージボックスを表示する
            Show(
                owner: owner,
                text: message,
                caption: ErrorMessage.TitleError,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Error);
        }

        /// <summary>
        /// クリティカルエラーのメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowCriticalError(IWin32Window owner, string message)
        {
            // クリティカルエラーのメッセージボックスを表示
            Show(
                owner: owner,
                text: message,
                caption: ErrorMessage.TitleCriticalError,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Stop);
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 情報のメッセージボックスを表示する
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowInfo(string message)
        {
            // 情報のメッセージボックスを表示する
            Show(
                text: message,
                caption: ErrorMessage.TitleInfo,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Information);
        }

        /// <summary>
        /// 確認のメッセージボックスを表示する
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult ShowConfirm(string message)
        {
            // 確認のメッセージボックスを表示する
            return Show(
                text: message,
                caption: ErrorMessage.TitleConfirm,
                buttons: MessageBoxButtons.YesNo,
                icon: MessageBoxIcon.Question,
                defaultButton: MessageBoxDefaultButton.Button2);
        }

        /// <summary>
        /// 注意のメッセージボックスを表示する
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowAttention(string message)
        {
            // 注意のメッセージボックスを表示する
            Show(
                text: message,
                caption: ErrorMessage.TitleAttention,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// ワーニングのメッセージボックスを表示する
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowWarning(string message)
        {
            // ワーニングメッセージを表示する
            Show(
                text: message,
                caption: ErrorMessage.TitleWarning,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Warning);
        }

        /// <summary>
        /// エラーのメッセージボックスを表示する
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowError(string message)
        {
            // エラーのメッセージボックスを表示する
            Show(
                text: message,
                caption: ErrorMessage.TitleError,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Error);
        }

        /// <summary>
        /// クリティカルエラーのメッセージボックスを表示する
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        public static void ShowCriticalError(string message)
        {
            // クリティカルエラーのメッセージボックスを表示
            Show(
                text: message,
                caption: ErrorMessage.TitleCriticalError,
                buttons: MessageBoxButtons.OK,
                icon: MessageBoxIcon.Stop);
        }

        #endregion

        #region MessageBoxのメソッド_Owner付き

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び指定したヘルプファイル、HelpNavigator、ヘルプトピックを使用する [ヘルプ] ボタンを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="helpFilePath">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプファイルのパスと名前</param>
        /// <param name="navigator"><see cref="HelpNavigator"/> 値のいずれか</param>
        /// <param name="param">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプトピックの数値ID</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
        {
            return Original.Show(owner, text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), helpFilePath, navigator, param);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び指定したヘルプファイル、HelpNavigatorを使用する [ヘルプ] ボタンを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="helpFilePath">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプファイルのパスと名前</param>
        /// <param name="navigator"><see cref="HelpNavigator"/> 値のいずれか</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return Original.Show(owner, text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), helpFilePath, navigator);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び指定したヘルプファイルととヘルプキーワードを使用する [ヘルプ] ボタンを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="helpFilePath">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプファイルのパスと名前</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプキーワード</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return Original.Show(owner, text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), helpFilePath, keyword);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び指定したヘルプファイルを使用する [ヘルプ] ボタンを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="helpFilePath">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプファイルのパスと名前</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            return Original.Show(
                owner, text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), helpFilePath);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、及びオプションを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return Original.Show(
                owner, text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options));
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、及び既定のボタンを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Original.Show(
                owner, text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption());
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、及びアイコンを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Original.Show(
                owner, text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(), GetOption());
        }

        /// <summary>
        /// 指定したテキスト、キャプション、及びボタンを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return Original.Show(
                owner, text, GetCaption(caption), GetButtons(buttons), GetIcon(), GetDefaultButton(), GetOption());
        }

        /// <summary>
        /// 指定したテキストとキャプションをメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return Original.Show(
                owner, text, GetCaption(caption), GetButtons(), GetIcon(), GetDefaultButton(), GetOption());
        }

        /// <summary>
        /// 指定したテキストを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="owner">メッセージボックスを所有する <see cref="IWin32Window"/> のインスタンス</param>
        /// <param name="text">表示するテキスト</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return Original.Show(
                owner, text, GetCaption(), GetButtons(), GetIcon(), GetDefaultButton(), GetOption());
        }

        #endregion

        #region MessageBoxのメソッド

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び指定したヘルプファイル、HelpNavigator、ヘルプトピックを使用する [ヘルプ] ボタンを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="helpFilePath">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプファイルのパスと名前</param>
        /// <param name="navigator"><see cref="HelpNavigator"/> 値のいずれか</param>
        /// <param name="param">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプトピックの数値ID</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), helpFilePath, navigator, param);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び指定したヘルプファイル、HelpNavigatorを使用する [ヘルプ] ボタンを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="helpFilePath">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプファイルのパスと名前</param>
        /// <param name="navigator"><see cref="HelpNavigator"/> 値のいずれか</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), helpFilePath, navigator);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び指定したヘルプファイルととヘルプキーワードを使用する [ヘルプ] ボタンを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="helpFilePath">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプファイルのパスと名前</param>
        /// <param name="keyword">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプキーワード</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), helpFilePath, keyword);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び指定したヘルプファイルを使用する [ヘルプ] ボタンを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="helpFilePath">ユーザーが [ヘルプ] ボタンをクリックしたときに表示されるヘルプファイルのパスと名前</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), helpFilePath);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、オプション、
        /// 及び [ヘルプ] ボタンを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <param name="displayHelpButton">[ヘルプ] ボタンを表示する場合は True、それ以外の場合は False</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options), displayHelpButton);
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、既定のボタン、及びオプションを表示する
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <param name="options">表示オプションと関連付けオプションの指定 <see cref="MessageBoxOptions"/> 値の1つ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// 　・<paramref name="options"/> に 無効な組み合わせの <see cref="MessageBoxOptions"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption(options));
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、アイコン、及び既定のボタンを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <param name="defaultButton">既定のボタンの指定 <see cref="MessageBoxDefaultButton"/> 値の1つ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// 　・<paramref name="defaultButton"/> が <see cref="MessageBoxDefaultButton"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(defaultButton), GetOption());
        }

        /// <summary>
        /// 指定したテキスト、キャプション、ボタン、及びアイコンを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <param name="icon">表示するアイコン <see cref="MessageBoxIcon"/> 値の1つ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// 　・<paramref name="icon"/> が <see cref="MessageBoxIcon"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(
            string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(icon), GetDefaultButton(), GetOption());
        }

        /// <summary>
        /// 指定したテキスト、キャプション、及びボタンを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <param name="buttons">表示するボタン <see cref="MessageBoxButtons"/> 値の1つ</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 下記の場合に発生
        /// １．下記の引数に定義されていない不正な値が指定された場合
        /// 　・<paramref name="buttons"/> が <see cref="MessageBoxButtons"/>のメンバーでない値の場合
        /// ２．下記の引数に無効な組み合わせの値が指定された場合に発生
        /// 　・<paramref name="buttons"/> に 無効な組み合わせの <see cref="MessageBoxButtons"/> を指定した場合
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(buttons), GetIcon(), GetDefaultButton(), GetOption());
        }

        /// <summary>
        /// 指定したテキストとキャプションをメッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <param name="caption">タイトルバーに表示するテキスト</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(string text, string caption)
        {
            return Original.Show(
                text, GetCaption(caption), GetButtons(), GetIcon(), GetDefaultButton(), GetOption());
        }

        /// <summary>
        /// 指定したテキストを表示するメッセージボックスを表示する
        /// </summary>
        /// <param name="text">表示するテキスト</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="System.Windows.Forms.MessageBox"/> がユーザー対話モードで実行されていない場合に発生
        /// </exception>
        /// <returns><see cref="DialogResult"/> 値のいずれか</returns>
        public static DialogResult Show(string text)
        {
            return Original.Show(
                text, GetCaption(), GetButtons(), GetIcon(), GetDefaultButton(), GetOption());
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// メッセージボックスのタイトルバーに表示するテキストを取得する
        /// </summary>
        /// <param name="caption">
        /// メッセージボックスのタイトルバーに表示するテキスト
        /// </param>
        /// <returns>
        /// メッセージボックスのタイトルバーに表示するテキストを返却
        /// </returns>
        private static string GetCaption(string caption = null)
        {
            return caption ?? string.Empty;
        }

        /// <summary>
        /// メッセージボックスに表示するボタンを取得する
        /// </summary>
        /// <param name="buttons">
        /// メッセージボックスに表示するボタン
        /// </param>
        /// <returns>
        /// メッセージボックスに表示するボタンを返却
        /// </returns>
        private static MessageBoxButtons GetButtons(MessageBoxButtons? buttons = null)
        {
            return buttons ?? MessageBoxButtons.OK;
        }

        /// <summary>
        /// メッセージボックスに表示するアイコンを取得する
        /// </summary>
        /// <param name="icon">
        /// メッセージボックスに表示するアイコン
        /// </param>
        /// <returns>
        /// メッセージボックスに表示するアイコンを返却
        /// </returns>
        private static MessageBoxIcon GetIcon(MessageBoxIcon? icon = null)
        {
            return icon ?? MessageBoxIcon.None;
        }

        /// <summary>
        /// メッセージボックスの既定のボタンを取得する
        /// </summary>
        /// <param name="defaultButton">
        /// メッセージボックスの既定のボタン
        /// </param>
        /// <returns>
        /// メッセージボックスの既定のボタンを返却
        /// </returns>
        private static MessageBoxDefaultButton GetDefaultButton(
            MessageBoxDefaultButton? defaultButton = null)
        {
            return defaultButton ?? MessageBoxDefaultButton.Button1;
        }

        /// <summary>
        /// メッセージ ボックスで使用する表示オプションと関連付けオプションを取得する
        /// </summary>
        /// <remarks>
        /// 現在の言語が右から左へ向かう言語の場合は、オプションにその設定を行う
        /// </remarks>
        /// <param name="options">
        /// メッセージ ボックスで使用する表示オプションと関連付けオプション
        /// 既定値は 0
        /// </param>
        /// <returns>
        /// メッセージ ボックスで使用する表示オプションと関連付けオプションを返却
        /// </returns>
        private static MessageBoxOptions GetOption(MessageBoxOptions? options = null)
        {
            MessageBoxOptions returnOptions = options ?? 0;
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                returnOptions |= MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign;
            }

            return returnOptions;
        }

        #endregion
    }
}
