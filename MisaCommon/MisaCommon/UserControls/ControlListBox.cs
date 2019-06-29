namespace MisaCommon.UserControls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    using MisaCommon.CustomType.Attribute;
    using MisaCommon.MessageResources.UserControl;
    using MisaCommon.Utility.ExtendMethod;

    using MessageBox = MisaCommon.Utility.StaticMethod.MessageBox;

    /// <summary>
    /// 任意のコントロールをリスト表示するためのユーザコントロール
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1501",
        Justification = "元々のUserControlの継承が深過ぎであり、自作クラスにおいては深い継承はしていないため抑止")]
    public partial class ControlListBox : UserControl
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトコンストラクタ
        /// 各コントロールを初期化する
        /// </summary>
        public ControlListBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// コンストラクタ
        /// 初期化処理にて、引数の <paramref name="tableControlList"/> に存在する全てのコントロールを、
        /// <see cref="TableLayoutPanel"/> に初期設定する
        /// </summary>
        /// <param name="tableControlList">
        /// 初期設定対象のコントロールのリスト
        /// （NULL 又は、要素0行の場合、<see cref="TableLayoutPanel"/> の初期化（クリア）のみを行う）
        /// </param>
        /// <exception cref="Exception">
        /// 設定するコントロールがトップレベルのコントロール 又は、設定によって循環参照になる場合に発生
        /// </exception>
        public ControlListBox(IList<Control> tableControlList)
            : this()
        {
            // コントロールの設定を行う
            SetTableControlCollection(tableControlList);
        }

        #endregion

        #region イベント定義

        /// <summary>
        /// コントロール追加ボタンがクリックされた場合に発生するイベント
        /// </summary>
        [LocalizableCategory(DefinedCategory.Action)]
        [LocalizableDescription("PlusClickDescription", typeof(ControlListBoxMessage))]
        public event EventHandler PlusClick;

        #endregion

        #region PlListTableのカラムのenum定義

        /// <summary>
        /// コントロールを配置する <see cref="PlListTable"/> のカラムの定義
        /// </summary>
        private enum ListTableColumn : int
        {
            /// <summary>
            /// ラジオボタンを配置するカラムインデックス
            /// </summary>
            Radio = 0,

            /// <summary>
            /// コントロールを配置するカラムインデックス
            /// </summary>
            Control = 1,
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 選択された行に対する背景色をを取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Appearance)]
        [LocalizableDescription("SelectRowBackColorDescription", typeof(ControlListBoxMessage))]
        [DefaultValue(typeof(Color), nameof(SystemColors.Control))]
        public Color SelectRowBackColor { get; set; } = SystemColors.Control;

        /// <summary>
        /// 削除ボタン（[ー] ボタン）押下時の削除確認メッセージを表示するかのフラグを取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("IsShowDeleteConfirmMessageDescription", typeof(ControlListBoxMessage))]
        [DefaultValue(true)]
        public bool IsShowDeleteConfirmMessage { get; set; } = true;

        /// <summary>
        /// 削除ボタン（[ー] ボタン）押下時において、
        /// 削除対象有無チェックでNGとなった場合にメッセージを表示するかのフラグを取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("IsShowDeleteNoChekedMessageDescription", typeof(ControlListBoxMessage))]
        [DefaultValue(true)]
        public bool IsShowDeleteNoChekedMessage { get; set; } = true;

        /// <summary>
        /// 移動ボタン（[↑]、[↓] ボタン）押下時において、
        /// 移動対象有無チェックでNGとなった場合にメッセージを表示するかのフラグを取得・設定する
        /// </summary>
        [LocalizableCategory(DefinedCategory.Behavior)]
        [LocalizableDescription("IsShowMoveNoChekedMessageDescription", typeof(ControlListBoxMessage))]
        [DefaultValue(true)]
        public bool IsShowMoveNoChekedMessage { get; set; } = true;

        /// <summary>
        /// <see cref="TableLayoutPanel"/> に配置されているコントロールを取得する
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public IList<Control> TableControlList
        {
            get
            {
                // TableLayoutPanelに登録されているコントロールを取得する
                IList<Control> tableControlList = new List<Control>();
                for (int i = 0; i < PlListTable.RowCount; i++)
                {
                    Control control
                        = PlListTable.GetControlFromPosition((int)ListTableColumn.Control, i);
                    tableControlList.Add(control);
                }

                // 取得したコントロールリストを返却する
                return tableControlList;
            }
        }

        /// <summary>
        /// <see cref="TableLayoutPanel"/> においてラジオボタンがチェックされている行の
        /// インデックスを取得する
        /// （チェックされている行が存在しない場合はNULLを返却）
        /// </summary>
        /// <remarks>
        /// デザイン時は操作しないプロパティのため読み取り専用属性を付与する
        /// </remarks>
        [ReadOnly(true)]
        [Browsable(false)]
        public int? CheckedRowIndex
        {
            get
            {
                // 対象のTableLayoutPanelコントロール取得
                TableLayoutPanel tablePanel = PlListTable;

                // PlTableListの行でループし、ラジオボタン設置用のカラムに存在する
                // ラジオボタンがチェックされているか判定
                int? index = null;
                for (int row = 0; row < tablePanel.RowCount; row++)
                {
                    if (tablePanel.GetControlFromPosition(
                        (int)ListTableColumn.Radio, row) is TableRadioButton radio
                        && radio.Checked)
                    {
                        index = row;
                        break;
                    }
                }

                // チェックされている行のインデックスが取得できた場合はその値、取得できない場合はNULL
                return index;
            }
        }

        /// <summary>
        /// <see cref="PlListTable"/> に設置しているラジオボタンのリスト
        /// </summary>
        private Collection<TableRadioButton> TableRadioButtons { get; }
            = new Collection<TableRadioButton>();

        /// <summary>
        /// <see cref="PlListTable"/> に設置しているコントロールを含むパネルのリスト
        /// </summary>
        private Collection<TableControlPanel> TableControlPanels { get; }
            = new Collection<TableControlPanel>();

        #endregion

        #region 公開メソッド

        #region ラジオボタンのチェック：CheckedRadio

        /// <summary>
        /// 引数（<paramref name="rowIndex"/>）で指定された行のラジオボタンをチェックする
        /// 対象行にラジオボタンが存在しない場合はなにもしない
        /// </summary>
        /// <param name="rowIndex">チェックを行う行インデックス</param>
        public void CheckedRadio(int rowIndex)
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // 指定された行インデックスが、TableLayoutPanelの行の範囲にない場合、
            // チェック対象が存在しないため処理を終了する
            if (rowIndex < 0 || rowIndex >= tablePanel.RowCount)
            {
                return;
            }

            // 対象行のラジオボタンのコントロールを取得しチェックを入れる
            // ラジオボタンのコントロールを取得できなかった場合はなにもしない
            if (tablePanel.GetControlFromPosition(
                (int)ListTableColumn.Radio, rowIndex) is TableRadioButton radio)
            {
                radio.Checked = true;
            }
        }

        #endregion

        #region 設定：SetTableControlCollection

        /// <summary>
        /// <see cref="TableLayoutPanel"/> の内容を一旦クリアしてから、
        /// 引数（<paramref name="tableControlList"/>）のコントロールを全て設定する
        /// </summary>
        /// <param name="tableControlList">
        /// 設定対象のコントロールのリスト
        /// （NULL 又は、要素0行の場合、<see cref="TableLayoutPanel"/> の初期化（クリア）のみを行う）
        /// </param>
        /// <exception cref="Exception">
        /// 設定するコントロールがトップレベルのコントロール または、
        /// 設定によって循環参照になる場合に発生
        /// </exception>
        public void SetTableControlCollection(IList<Control> tableControlList)
        {
            // 設定はテーブルのコントロールを全消去して、追加する
            Clear();
            Add(tableControlList);
        }

        #endregion

        #region 追加：Add

        /// <summary>
        /// 引数の <paramref name="control"/> コントロールを、
        /// <see cref="TableLayoutPanel"/> に追加する（先頭行に追加する）
        /// </summary>
        /// <param name="control">
        /// 追加対象のコントロール（NULLの場合、追加対象なしとして処理を終了する）
        /// </param>
        /// <exception cref="Exception">
        /// 追加するコントロールがトップレベルのコントロール または、
        /// 追加によって循環参照になる場合に発生
        /// </exception>
        public void Add(Control control)
        {
            // 引数のコントロールに対応するコレクションを生成する
            IList<Control> tableControlCollection
                = control == null ? null : new List<Control> { control };

            // 追加処理を行う
            Add(tableControlCollection);
        }

        /// <summary>
        /// 引数の <paramref name="tableControlList"/> に存在する全てのコントロールを、
        /// <see cref="TableLayoutPanel"/> に追加する（先頭行に追加する）
        /// </summary>
        /// <param name="tableControlList">
        /// 追加対象のコントロールのリスト
        /// （NULL 又は、要素0の場合、追加対象なしとして処理を終了する）
        /// </param>
        /// <exception cref="Exception">
        /// 追加するコントロールがトップレベルのコントロール または、
        /// 追加によって循環参照になる場合に発生
        /// </exception>
        public void Add(IList<Control> tableControlList)
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // 引数のコレクションがNULL 又は、0行の場合、追加対象なしとして処理を終了する
            if (tableControlList == null || tableControlList.Count == 0)
            {
                return;
            }

            // 描画処理を一時中断
            tablePanel.SuspendLayout();

            try
            {
                // 現在の行数が1以下かつコントロールが未配置の状態であるかの判定を取得
                bool isNoRow = IsNoRow();

                // 現状の行数を取得
                // （現在の行数が1以下かつコントロールが未配置の状態の場合、現在の行数は0とする）
                int beforeRowCount = isNoRow ? 0 : tablePanel.RowCount;

                // 追加する行数
                int addRowCount = tableControlList.Count;

                // 行カウントを増やす
                tablePanel.RowCount = beforeRowCount + addRowCount;

                // スタイル情報を配列で取得し、追加する行数分配列を拡張する
                RowStyle[] rowStyles = tablePanel.RowStyles.ToArray();
                Array.Resize(ref rowStyles, beforeRowCount + addRowCount);

                // 追加行数分コントロール及びスタイルを下に移動させる
                for (int row = beforeRowCount - 1; row >= 0; row--)
                {
                    MoveRow(
                        toRowIndex: row + addRowCount,
                        getControl: (column) => tablePanel.GetControlFromPosition(column, row),
                        rowStyles: ref rowStyles,
                        getRowStyle: () => rowStyles[row]);
                }

                // 追加対象のコントロールとそのスタイルを追加
                int rowIndex = 0;
                foreach (Control control in tableControlList)
                {
                    // コントロールの設定
                    // ラジオボタン
                    TableRadioButton radioButton
                        = new TableRadioButton(tablePanel, (int)ListTableColumn.Radio, SelectRowBackColor);
                    TableRadioButtons.Add(radioButton);
                    tablePanel.Controls.Add(radioButton, (int)ListTableColumn.Radio, rowIndex);

                    // パネル
                    TableControlPanel controlPanel = new TableControlPanel(control);
                    TableControlPanels.Add(controlPanel);
                    tablePanel.Controls.Add(controlPanel, (int)ListTableColumn.Control, rowIndex);

                    // スタイルを設定
                    rowStyles[rowIndex] = new RowStyle();

                    // 行インデックスをインクリメント
                    rowIndex++;
                }

                // スタイルを再設定
                SetRowStyle(rowStyles);

                // Tabインデックスの再設定を行う
                SetTabIndex();
            }
            finally
            {
                // 描画処理を再開
                tablePanel.ResumeLayout(false);
                tablePanel.PerformLayout();
            }
        }

        #endregion

        #region 削除：Remove＆Clear

        /// <summary>
        /// <see cref="TableLayoutPanel"/> の指定の行を削除する
        /// </summary>
        /// <param name="removeRowIndex">
        /// 削除対象の行インデックス
        /// （引数の削除対象行の指定が0未満 または、
        /// 　最大行数を超えている場合、削除対象なしとして処理を終了する）
        /// </param>
        public void Remove(int removeRowIndex)
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // 引数の削除対象行の指定が0未満 又は、最大行数を超えている場合、
            // 削除対象なしとして処理を終了する
            if (removeRowIndex < 0 || removeRowIndex >= tablePanel.RowCount)
            {
                return;
            }

            // 描画処理を一時中断
            tablePanel.SuspendLayout();

            try
            {
                // 現状の行数とカラム数を取得
                int beforeRowCount = tablePanel.RowCount;

                // 削除対象行のコントロールを削除
                RemoveRow(removeRowIndex, true);

                // 削除対象行より先の行のコントロールを一つ上に移動させる
                for (int row = removeRowIndex + 1; row < beforeRowCount; row++)
                {
                    MoveRow(
                        toRowIndex: row - 1,
                        getControl: (column) => tablePanel.GetControlFromPosition(column, row));
                }

                // 行カウントを減らす
                // （TableLayoutPanelの仕様上、1未満にはならないよう制御する）
                tablePanel.RowCount = beforeRowCount <= 1 ? 1 : beforeRowCount - 1;

                // 行スタイルを削除する
                // （TableLayoutPanelの仕様上、1未満にはならないよう制御する）
                if (removeRowIndex < tablePanel.RowStyles.Count)
                {
                    tablePanel.RowStyles.RemoveAt(removeRowIndex);
                    if (tablePanel.RowStyles.Count == 0)
                    {
                        tablePanel.RowStyles.Add(new RowStyle());
                    }
                }

                // Tabインデックスの再設定を行う
                SetTabIndex();
            }
            finally
            {
                // 描画処理を再開
                tablePanel.ResumeLayout(false);
                tablePanel.PerformLayout();
            }
        }

        /// <summary>
        /// <see cref="TableLayoutPanel"/> の内容をクリアする
        /// </summary>
        public void Clear()
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // 描画処理を一時中断
            tablePanel.SuspendLayout();

            try
            {
                // 全てのコントロールをクリア
                // （下からクリアしていく）
                for (int row = tablePanel.RowCount - 1; row > -1; row--)
                {
                    RemoveRow(row, true);
                }

                // 最初の1行目を生成（TableLayoutPanelは0行を認めていないため1行目を生成する）
                tablePanel.RowCount = 1;

                // 行のスタイル設定を初期化（引数にNULLを指定することで初期化を行わせる）
                SetRowStyle(null);
            }
            finally
            {
                // 描画処理を再開
                tablePanel.ResumeLayout(false);
                tablePanel.PerformLayout();
            }
        }

        #endregion

        #region 移動：MoveRow

        /// <summary>
        /// <see cref="TableLayoutPanel"/> の行を移動する
        /// （引数の行インデックスの指定が0未満の場合は 0、
        /// 　最大行数を超えている場合は最大行インデックスとして処理を行う）
        /// </summary>
        /// <param name="fromRowIndex">移動元の行インデックス</param>
        /// <param name="toRowIndex">移動先の行インデックス</param>
        public void MoveRow(int fromRowIndex, int toRowIndex)
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // 引数の行インデックスの指定が0未満の場合は 0、
            // 最大行数を超えている場合は最大行インデックスとして処理を行う
            int fromRow = GetIndex(fromRowIndex, tablePanel.RowCount);
            int toRow = GetIndex(toRowIndex, tablePanel.RowCount);

            // 上記行インデックスの設定で使用するローカル関数
            int GetIndex(int inputIndex, int tableRowCount)
            {
                return (inputIndex >= 0 && inputIndex < tableRowCount)
                    ? inputIndex
                    : inputIndex < 0 ? 0 : tableRowCount - 1;
            }

            // 移動元と移動先が同じ場合は移動を行わないため処理を終了する
            if (fromRow == toRow)
            {
                return;
            }

            // 描画処理を一時中断
            tablePanel.SuspendLayout();

            try
            {
                // 各種のカウントを取得
                int columnCount = tablePanel.ColumnCount;
                int styleCount = tablePanel.RowStyles.Count;

                // スタイル情報を配列で取得する
                RowStyle[] rowStyles = tablePanel.RowStyles.ToArray();

                // 移動先のコントロールを取得する
                Control[] toRowControls = new Control[columnCount];
                for (int column = 0; column < tablePanel.ColumnCount; column++)
                {
                    Control toControl = tablePanel.GetControlFromPosition(column, toRow);
                    if (toControl != null)
                    {
                        toRowControls[column] = toControl;
                    }
                }

                // 移動先のスタイルを取得する
                RowStyle toRowStyle = toRow < styleCount ? rowStyles[toRow] : new RowStyle();

                // 移動先のコントロールをTableLayoutPanelからは削除する
                RemoveRow(toRow, false);

                // 移動元のコントロール・スタイルを移動先に移動する
                MoveRow(
                    toRowIndex: toRow,
                    getControl: (column) => tablePanel.GetControlFromPosition(column, fromRow),
                    rowStyles: ref rowStyles,
                    getRowStyle: () => fromRow < styleCount ? rowStyles[fromRow] : new RowStyle());

                // 上への移動か下への移動かによってfrom～toの間の
                // コントロール・スタイルの移動方法を切り分ける
                if ((fromRow - toRow) > 0)
                {
                    // 下に移動の場合
                    // 上から順にコントロール・スタイルを移動する
                    for (int row = fromRow + 1; row < toRow; row++)
                    {
                        MoveRow(
                            toRowIndex: row - 1,
                            getControl: (column) => tablePanel.GetControlFromPosition(column, row),
                            rowStyles: ref rowStyles,
                            getRowStyle: () => row < styleCount ? rowStyles[row] : new RowStyle());
                    }

                    // 保持していた移動先のコントロールを再設定
                    for (int column = 0; column < tablePanel.ColumnCount; column++)
                    {
                        Control control = toRowControls[column];
                        if (control != null)
                        {
                            tablePanel.Controls.Add(control, column, toRow + 1);
                        }
                    }

                    // 保持していた移動先のスタイルを再設定
                    rowStyles[toRow + 1] = toRowStyle;
                }
                else
                {
                    // 上に移動の場合
                    // 下から順にコントロール・スタイルを移動する
                    for (int row = fromRow - 1; row > toRow; row--)
                    {
                        MoveRow(
                            toRowIndex: row + 1,
                            getControl: (column) => tablePanel.GetControlFromPosition(column, row),
                            rowStyles: ref rowStyles,
                            getRowStyle: () => row < styleCount ? rowStyles[row] : new RowStyle());
                    }

                    // 保持していた移動先のスタイルを再設定
                    for (int column = 0; column < tablePanel.ColumnCount; column++)
                    {
                        Control control = toRowControls[column];
                        if (control != null)
                        {
                            tablePanel.Controls.Add(control, column, toRow - 1);
                        }
                    }

                    // 保持していた移動先のスタイルを再設定
                    rowStyles[toRow - 1] = toRowStyle;
                }

                // スタイルを再設定
                SetRowStyle(rowStyles);

                // Tabインデックスの再設定を行う
                SetTabIndex();
            }
            finally
            {
                // 描画処理を再開
                tablePanel.ResumeLayout(false);
                tablePanel.PerformLayout();
            }
        }

        #endregion

        #endregion

        #region イベントで呼び出されるメソッド

        /// <summary>
        /// コントロールの追加ボタン押下のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void PlPlus_Click(object sender, EventArgs e)
        {
            // 当イベントは親コントロールへのイベント通知のみである、
            // そのためエラー処理は親コントロールにて行う
            PlusClick?.Invoke(this, null);
        }

        /// <summary>
        /// コントロールの削除ボタン押下のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtMinus_Click(object sender, EventArgs e)
        {
            // チェックされている行インデックスを取得する
            int? checkedIndex = CheckedRowIndex;

            // 削除対象がチェックされていない場合はメッセージを表示し処理を終了する
            if (!checkedIndex.HasValue)
            {
                // メッセージ表示フラグが立っている場合のみ表示する
                if (IsShowDeleteNoChekedMessage)
                {
                    MessageBox.ShowInfo(ControlListBoxMessage.BtMinusNoChekedMessage);
                }

                return;
            }

            // 削除確認メッセージの表示
            // メッセージ表示フラグが立っている場合のみ表示する
            if (IsShowDeleteConfirmMessage)
            {
                string confirmMessage = ControlListBoxMessage.BtMinusConfirmDeleteMessage;
                if (!(DialogResult.OK | DialogResult.Yes).HasFlag(MessageBox.ShowConfirm(confirmMessage)))
                {
                    // 確認でOK・Yes以外を押下した場合は処理を終了する
                    return;
                }
            }

            // チェックされた行を削除する
            Remove(checkedIndex.Value);
        }

        /// <summary>
        /// コントロールの上／下へ移動ボタン押下のイベント
        /// </summary>
        /// <param name="sender">センダーオブジェクト</param>
        /// <param name="e">イベントデータ</param>
        private void BtUpDown_Click(object sender, EventArgs e)
        {
            // チェックされている行インデックスを取得する
            int? checkedIndex = CheckedRowIndex;

            // 削除対象がチェックされていない場合はメッセージを表示し処理を終了する
            if (!checkedIndex.HasValue)
            {
                // メッセージ表示フラグが立っている場合のみ表示する
                if (IsShowMoveNoChekedMessage)
                {
                    MessageBox.ShowInfo(ControlListBoxMessage.BtUpDownNoChekedMessage);
                }

                return;
            }

            // 上に移動か下に移動か判定する
            int moveVale = 0;
            if (sender == BtUp)
            {
                // 上に移動
                moveVale = -1;
            }
            else if (sender == BtDown)
            {
                // 下に移動
                moveVale = 1;
            }

            // コントロールの移動
            MoveRow(checkedIndex.Value, checkedIndex.Value + moveVale);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// <see cref="PlListTable"/> に行及びコントロールが存在しないかの判定
        /// </summary>
        /// <returns>
        /// <see cref="PlListTable"/>  に行及びコントロールが存在しない場合 True、
        /// 行及びコントロールが存在する場合 False
        /// </returns>
        private bool IsNoRow()
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // 行が1行以下かつコントロールが存在しない場合はデータなしと判定する
            return tablePanel.RowCount <= 1 && tablePanel.Controls.Count == 0;
        }

        /// <summary>
        /// <see cref="PlListTable"/> の対象行に行単位にコントロール及びスタイルを移動する
        /// </summary>
        /// <param name="toRowIndex">
        /// 移動先の行インデックス
        /// </param>
        /// <param name="getControl">
        /// 移動元のコントロールを取得する処理
        /// ・引数1 int：カラムのインデックス
        /// ・戻り値 Control：移動先に設定するコントロール
        /// 　（NULLを返却した場合はそのカラムでは移動を行わない）
        /// NULLを指定した場合はコントロールの移動処理を行わない
        /// </param>
        /// <param name="rowStyles">
        /// コントロールの移動に伴い同じく移動する <see cref="RowStyle"/> の配列
        /// </param>
        /// <param name="getRowStyle">
        /// 移動元の <see cref="RowStyle"/> を取得する処理
        /// ・戻り値 RowStyle：移動先に設定する <see cref="RowStyle"/>
        /// 　（NULLを返却した場合はデフォルト値を設定する）
        /// NULLを指定した場合はスタイルの移動処理を行わない
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 引数の <paramref name="toRowIndex"/> が <see cref="PlListTable"/> コントロールの
        /// 行の範囲（0以上、行カウント未満の範囲）の値でない場合に発生
        /// </exception>
        private void MoveRow(
            int toRowIndex,
            Func<int, Control> getControl,
            ref RowStyle[] rowStyles,
            Func<RowStyle> getRowStyle)
        {
            // コントロールの移動処理を行う（必要な引数チェックもこのメソッドで実行している）
            MoveRow(toRowIndex, getControl);

            // スタイルの移動
            if (getRowStyle != null)
            {
                if (toRowIndex < rowStyles.Length)
                {
                    rowStyles[toRowIndex] = getRowStyle() ?? new RowStyle();
                }
            }
        }

        /// <summary>
        /// <see cref="PlListTable"/> の対象行のコントロールを行単位に移動する
        /// </summary>
        /// <param name="toRowIndex">
        /// 移動先の行インデックス
        /// </param>
        /// <param name="getControl">
        /// 移動元のコントロールを取得する処理
        /// ・引数1 int：カラムのインデックス
        /// ・戻り値 Control：移動先に設定するコントロール
        /// 　（NULLを返却した場合はそのカラムでは移動を行わない）
        /// NULLを指定した場合はコントロールの移動処理を行わない
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 引数の <paramref name="toRowIndex"/> が <see cref="PlListTable"/> コントロールの
        /// 行の範囲（0以上、行カウント未満の範囲）の値でない場合に発生
        /// </exception>
        private void MoveRow(int toRowIndex, Func<int, Control> getControl)
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // 引数のチェック
            if (toRowIndex < 0 || toRowIndex >= tablePanel.RowCount)
            {
                // 引数のtoRowIndexがtablePanelの行の範囲にない場合
                throw new ArgumentOutOfRangeException(nameof(toRowIndex));
            }

            // コントロールの移動
            if (getControl != null)
            {
                for (int column = 0; column < tablePanel.ColumnCount; column++)
                {
                    Control control = getControl(column);
                    if (control != null)
                    {
                        tablePanel.SetCellPosition(
                            control: control,
                            position: new TableLayoutPanelCellPosition(column, toRowIndex));
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="PlListTable"/> の対象行に存在する子コントロールを削除する
        /// </summary>
        /// <param name="removeRowIndex">
        /// 削除対象の行インデックス
        /// </param>
        /// <param name="isDelete">
        /// 削除処理かどうかのフラグ
        /// 削除の場合：True、移動の場合：False
        /// （削除の場合は対象のコントロールの Dispose を行う）
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 引数の <paramref name="removeRowIndex"/> が <see cref="PlListTable"/> コントロールの
        /// 行の範囲（0以上、行カウント未満の範囲）の値でない場合に発生
        /// </exception>
        private void RemoveRow(int removeRowIndex, bool isDelete)
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // 引数のチェック
            if (removeRowIndex < 0 || removeRowIndex >= tablePanel.RowCount)
            {
                // 引数のremoveRowIndexがtablePanelの行の範囲にない場合
                throw new ArgumentOutOfRangeException(nameof(removeRowIndex));
            }

            // 削除対象行に存在する全てのコントロールを削除する
            for (int column = 0; column < tablePanel.ColumnCount; column++)
            {
                Control removeControl = tablePanel.GetControlFromPosition(column, removeRowIndex);
                if (removeControl != null)
                {
                    tablePanel.Controls.Remove(removeControl);

                    // 削除の場合は対象のコントロールを破棄する
                    if (isDelete)
                    {
                        if (removeControl is TableRadioButton radio)
                        {
                            TableRadioButtons.Remove(radio);
                        }
                        else if (removeControl is TableControlPanel panel)
                        {
                            TableControlPanels.Remove(panel);
                        }

                        removeControl.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="PlListTable"/> の行スタイルを、
        /// 引数の <paramref name="rowStyles"/> の値で設定する
        /// </summary>
        /// <param name="rowStyles">
        /// コントロールの移動に伴い同じく移動する <see cref="RowStyle"/> の配列
        /// NULL又は要素0の配列を指定した場合はスタイルを初期化する
        /// </param>
        private void SetRowStyle(RowStyle[] rowStyles)
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // TableLayoutPanelの行スタイルをクリア
            tablePanel.RowStyles.Clear();

            if (rowStyles != null || rowStyles.Length > 0)
            {
                // 引数のスタイルの指定が存在する場合、引数の配列の要素で設定を行う
                foreach (RowStyle style in rowStyles)
                {
                    tablePanel.RowStyles.Add(new RowStyle(style.SizeType, style.Height));
                }
            }
            else
            {
                // 引数のスタイルの指定がNULL又は要素0の場合、初期設定を行う
                // 最初の1行目を生成（TableLayoutPanelは0行を認めていないため1行目を生成する）
                tablePanel.RowStyles.Add(new RowStyle());
            }
        }

        /// <summary>
        /// <see cref="PlListTable"/> の子コントロールのTabインデックスを設定する
        /// </summary>
        private void SetTabIndex()
        {
            // 対象のTableLayoutPanelコントロール取得
            TableLayoutPanel tablePanel = PlListTable;

            // TableLayoutPanelを左上から右、下に向かってTabインデックスを設定する
            int tabIndex = 0;
            for (int row = 0; row < tablePanel.RowCount; row++)
            {
                for (int column = 0; column < tablePanel.ColumnCount; column++)
                {
                    Control control = tablePanel.GetControlFromPosition(column, row);
                    if (control != null)
                    {
                        control.TabIndex = tabIndex++;
                    }
                }
            }
        }

        #endregion

        #region 内部プライベートクラス

        #region PlListTableに設置するラジオボタン

        /// <summary>
        /// <see cref="PlListTable"/> に設置するラジオボタン
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1501",
            Justification = "元々のRadioButtonの継承が深過ぎであり、自作クラスにおいては深い継承はしていないため抑止")]
        private class TableRadioButton : RadioButton
        {
            #region コンストラクタ

            /// <summary>
            /// デフォルトコンストラクタ
            /// コントロールの初期化を行う
            /// </summary>
            /// <param name="owner">
            /// このコントロールを配置する、オーナ <see cref="TableLayoutPanel"/> コントロール
            /// </param>
            /// <param name="columnIndex">
            /// オーナ <see cref="TableLayoutPanel"/> コントロールにおいて、
            /// このコントロールを配置するカラムのインデックス
            /// </param>
            /// <param name="selectBackColor">
            /// チェックボックスがチェックされた場合、
            /// オーナ <see cref="TableLayoutPanel"/> コントロールの対象行に設定する色を指定する
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// 引数の <paramref name="owner"/> がNULLの場合に発生
            /// </exception>
            /// <exception cref="ArgumentOutOfRangeException">
            /// 引数の <paramref name="columnIndex"/> が、
            /// オーナ <see cref="TableLayoutPanel"/> コントロールの
            /// カラムの範囲（0以上、カラムカウント未満の範囲）の値でない場合に発生
            /// </exception>
            public TableRadioButton(
                TableLayoutPanel owner,
                int columnIndex,
                Color selectBackColor)
            {
                // 引数のチェック
                if (owner == null)
                {
                    // オーナTableLayoutPanelがNULLの場合
                    throw new ArgumentNullException(nameof(owner));
                }
                else if (columnIndex < 0 || columnIndex >= owner.ColumnCount)
                {
                    // カラムインデックスがオーナTableLayoutPanelのカラムの範囲にない場合
                    throw new ArgumentOutOfRangeException(nameof(columnIndex));
                }

                // 引数をプロパティに設定して保持する
                Owner = owner;
                ColumnIndex = columnIndex;
                SelectBackColor = selectBackColor;

                // 初期化処理
                InitializeComponent();
            }

            #endregion

            #region プロパティ

            /// <summary>
            /// このコントロールを配置する、オーナ <see cref="TableLayoutPanel"/> コントロール
            /// </summary>
            private TableLayoutPanel Owner { get; }

            /// <summary>
            /// オーナ <see cref="TableLayoutPanel"/> コントロールおける、
            /// このコントロールを配置するカラムのインデックス
            /// </summary>
            private int ColumnIndex { get; }

            /// <summary>
            /// チェックボックスがチェックされた場合、
            /// オーナ <see cref="TableLayoutPanel"/> コントロールの、対象行に設定する背景色を指定する
            /// </summary>
            private Color SelectBackColor { get; }

            #endregion

            #region イベントから直接呼び出されるメソッド

            /// <summary>
            /// ラジオボタンのチェックに変更があった場合のイベント
            /// </summary>
            /// <param name="sender">センダーオブジェクト</param>
            /// <param name="e">イベントデータ</param>
            private void RadioCheckedChanged(object sender, EventArgs e)
            {
                // センダーオブジェクトから変更があったチェックの内容を取得する
                // センダーオブジェクトがラジオボタンでない場合、
                // 後続の処理で全てのチェックを外すため True を設定
                bool isChecked = (sender as RadioButton)?.Checked ?? true;

                // オーナのTableLayoutPanelパネルの行数毎にループを行う
                for (int row = 0; row < Owner.RowCount; row++)
                {
                    // ラジオボタンコントロールを取得
                    if (!(Owner.GetControlFromPosition(ColumnIndex, row) is TableRadioButton radio))
                    {
                        // ラジオボタンコントロールが取得できない行は処理を飛ばす
                        continue;
                    }

                    // 対象行がチェックの変更があったコントロールか判定する
                    if (radio == sender)
                    {
                        // チェックの変更があったコントロールの場合
                        // 背景色の設定を行う
                        for (int column = 0; column < Owner.ColumnCount; column++)
                        {
                            Control control = Owner.GetControlFromPosition(column, row);
                            if (control != null)
                            {
                                control.BackColor = isChecked ? SelectBackColor : Owner.BackColor;
                            }
                        }
                    }
                    else if (isChecked && radio.Checked)
                    {
                        // チェックの変更があったコントロールが True かつ、それ以外のラジオボタンで、
                        // チェックが True のコントロールが存在する場合、それのチェックを外す
                        radio.Checked = false;
                    }
                }
            }

            #endregion

            #region 各コントロールの初期化処理

            /// <summary>
            /// 各コントロールの初期化処理
            /// </summary>
            private void InitializeComponent()
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                AutoSize = true;
                CheckAlign = ContentAlignment.MiddleCenter;
                Margin = new Padding(0);
                Padding = new Padding(10);
                TabIndex = 0;
                TabStop = true;
                Text = ControlListBoxMessage.TableRadioButtonText;
                UseVisualStyleBackColor = true;
                CheckedChanged += new EventHandler(RadioCheckedChanged);
            }

            #endregion
        }

        #endregion

        #region PlListTableに設置するコントロールを含むパネル

        /// <summary>
        /// <see cref="PlListTable"/> に設置するコントロールを含むパネル
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1501",
            Justification = "元々のPanelの継承が深過ぎであり、自作クラスにおいては深い継承はしていないため抑止")]
        private class TableControlPanel : Panel
        {
            #region コンストラクタ

            /// <summary>
            /// デフォルトコンストラクタ
            /// 各コントロールの初期化を行う
            /// </summary>
            /// <param name="control">
            /// このパネルに配置するコントロールオブジェクト
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// 引数の <paramref name="control"/> がNULLの場合に発生
            /// </exception>
            /// <exception cref="Exception">
            /// 追加するコントロールがトップレベルのコントロール または、
            /// 追加によって循環参照になる場合に発生
            /// </exception>
            public TableControlPanel(Control control)
            {
                // 引数をプロパティに設定して保持する
                Control = control ?? throw new ArgumentNullException(nameof(control));

                // 初期化処理
                InitializeComponent();
            }

            #endregion

            #region プロパティ

            /// <summary>
            /// このパネルに配置されるコントロールオブジェクト
            /// </summary>
            public Control Control { get; }

            #endregion

            #region 各コントロールの初期化処理

            /// <summary>
            /// 各コントロールの初期化処理
            /// </summary>
            /// <exception cref="Exception">
            /// 追加するコントロールがトップレベルのコントロール または、
            /// 追加によって循環参照になる場合に発生
            /// </exception>
            private void InitializeComponent()
            {
                // 描画処理を一時中断
                SuspendLayout();

                try
                {
                    // このパネルに配置するコントロール
                    Control.Dock = DockStyle.Top;

                    // このパネル
                    Anchor = AnchorStyles.Top
                        | AnchorStyles.Bottom
                        | AnchorStyles.Left
                        | AnchorStyles.Right;
                    AutoSize = true;
                    AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    Controls.Add(Control);
                    Margin = new Padding(0);
                }
                finally
                {
                    // 描画処理を再開
                    ResumeLayout(false);
                    PerformLayout();
                }
            }

            #endregion
        }

        #endregion

        #endregion
    }
}
