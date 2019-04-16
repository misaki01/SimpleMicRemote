namespace MisaCommon.CustomType.Attribute
{
    using System.ComponentModel;

    /// <summary>
    /// <see cref="CategoryAttribute"/> にて既に定義されているカテゴリの列挙
    /// ここに登録されているカテゴリは <see cref="CategoryAttribute"/> にて、
    /// ローカライズの対応がされてるためリソースファイルの設定不要
    /// </summary>
    public enum DefinedCategory
    {
        /// <summary>
        /// 使用可能なアクションに関連するカテゴリ
        /// 日本語表示：アクション
        /// </summary>
        Action,

        /// <summary>
        /// エンティティの表示方法に関連するカテゴリ
        /// 日本語表示：表示
        /// </summary>
        Appearance,

        /// <summary>
        /// 非同期に関連するカテゴリ
        /// 日本語表示：非同期
        /// </summary>
        Asynchronous,

        /// <summary>
        /// エンティティの動作に関連するカテゴリ
        /// 日本語表示：動作
        /// </summary>
        Behavior,

        /// <summary>
        /// データとデータソースの管理に関連するカテゴリ
        /// 日本語表示：データ
        /// </summary>
        Data,

        /// <summary>
        /// 既定のカテゴリ
        /// 日本語表示：その他
        /// </summary>
        Default,

        /// <summary>
        /// デザイン時にのみ使用できるプロパティに関連するカテゴリ
        /// 日本語表示：デザイン
        /// </summary>
        Design,

        /// <summary>
        /// ドラッグアンドドロップ操作に関連するカテゴリ
        /// 日本語表示：ドラッグ アンド ドロップ
        /// </summary>
        DragDrop,

        /// <summary>
        /// ドフォーカスに関連するカテゴリ
        /// 日本語表示：フォーカス
        /// </summary>
        Focus,

        /// <summary>
        /// 書式設定に関連するカテゴリ
        /// 日本語表示：書式
        /// </summary>
        Format,

        /// <summary>
        /// キーボード操作に関連するカテゴリ
        /// 日本語表示：キー
        /// </summary>
        Key,

        /// <summary>
        /// レイアウトに関連するカテゴリ
        /// 日本語表示：レイアウト
        /// </summary>
        Layout,

        /// <summary>
        /// マウス操作に関連するカテゴリ
        /// 日本語表示：マウス
        /// </summary>
        Mouse,

        /// <summary>
        /// ウィンドウスタイルに関連するカテゴリ
        /// 日本語表示：ウィンドウ スタイル
        /// </summary>
        WindowStyle
    }

    /// <summary>
    /// <see cref="CategoryAttribute"/> にて既に定義されているカテゴリの文字列の定義クラス
    /// ここに登録されているカテゴリは <see cref="CategoryAttribute"/> にて、
    /// ローカライズの対応がされてるためリソースファイルの設定不要
    /// </summary>
    public static class DefinedCategoryString
    {
        /// <summary>
        /// 使用可能なアクションに関連するカテゴリ
        /// 日本語表示：アクション
        /// </summary>
        public const string Action = "Action";

        /// <summary>
        /// エンティティの表示方法に関連するカテゴリ
        /// 日本語表示：表示
        /// </summary>
        public const string Appearance = "Appearance";

        /// <summary>
        /// 非同期に関連するカテゴリ
        /// 日本語表示：非同期
        /// </summary>
        public const string Asynchronous = "Asynchronous";

        /// <summary>
        /// エンティティの動作に関連するカテゴリ
        /// 日本語表示：動作
        /// </summary>
        public const string Behavior = "Behavior";

        /// <summary>
        /// データとデータソースの管理に関連するカテゴリ
        /// 日本語表示：データ
        /// </summary>
        public const string Data = "Data";

        /// <summary>
        /// 既定のカテゴリ
        /// 日本語表示：その他
        /// </summary>
        public const string Default = "Default";

        /// <summary>
        /// デザイン時にのみ使用できるプロパティに関連するカテゴリ
        /// 日本語表示：デザイン
        /// </summary>
        public const string Design = "Design";

        /// <summary>
        /// ドラッグアンドドロップ操作に関連するカテゴリ
        /// 日本語表示：ドラッグ アンド ドロップ
        /// </summary>
        public const string DragDrop = "DragDrop";

        /// <summary>
        /// ドフォーカスに関連するカテゴリ
        /// 日本語表示：フォーカス
        /// </summary>
        public const string Focus = "Focus";

        /// <summary>
        /// 書式設定に関連するカテゴリ
        /// 日本語表示：書式
        /// </summary>
        public const string Format = "Format";

        /// <summary>
        /// キーボード操作に関連するカテゴリ
        /// 日本語表示：キー
        /// </summary>
        public const string Key = "Key";

        /// <summary>
        /// レイアウトに関連するカテゴリ
        /// 日本語表示：レイアウト
        /// </summary>
        public const string Layout = "Layout";

        /// <summary>
        /// マウス操作に関連するカテゴリ
        /// 日本語表示：マウス
        /// </summary>
        public const string Mouse = "Mouse";

        /// <summary>
        /// ウィンドウスタイルに関連するカテゴリ
        /// 日本語表示：ウィンドウ スタイル
        /// </summary>
        public const string WindowStyle = "WindowStyle";

        /// <summary>
        /// 定義済みカテゴリに紐づく文字列を取得する
        /// </summary>
        /// <param name="category">文字列を取得する定義済みカテゴリ</param>
        /// <returns>
        /// 引数（<paramref name="category"/>）で指定された定義済みカテゴリに紐づく文字列
        /// </returns>
        public static string GetString(DefinedCategory category)
        {
            // 定義済みカテゴリに紐づく文字列を取得
            string value;
            switch (category)
            {
                case DefinedCategory.Action:
                    value = Action;
                    break;
                case DefinedCategory.Appearance:
                    value = Appearance;
                    break;
                case DefinedCategory.Asynchronous:
                    value = Asynchronous;
                    break;
                case DefinedCategory.Behavior:
                    value = Behavior;
                    break;
                case DefinedCategory.Data:
                    value = Data;
                    break;
                case DefinedCategory.Default:
                    value = Default;
                    break;
                case DefinedCategory.Design:
                    value = Design;
                    break;
                case DefinedCategory.DragDrop:
                    value = DragDrop;
                    break;
                case DefinedCategory.Focus:
                    value = Focus;
                    break;
                case DefinedCategory.Format:
                    value = Format;
                    break;
                case DefinedCategory.Key:
                    value = Key;
                    break;
                case DefinedCategory.Layout:
                    value = Layout;
                    break;
                case DefinedCategory.Mouse:
                    value = Mouse;
                    break;
                case DefinedCategory.WindowStyle:
                    value = WindowStyle;
                    break;
                default:
                    value = Default;
                    break;
            }

            // 取得した文字列を返却
            return value;
        }
    }
}
