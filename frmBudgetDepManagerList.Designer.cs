namespace DebitArticle
{
    partial class frmBudgetDepManagerList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.colChecked = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemChecked = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colManagerName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemCheckEdit_ReadOnly = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemMemoExEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.repItemlkpBudgetDep = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repItemUsersList = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemChecked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCheckEdit_ReadOnly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemlkpBudgetDep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemUsersList)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.treeList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 296);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // treeList
            // 
            this.treeList.AllowDrop = true;
            this.treeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeList.Appearance.EvenRow.BackColor = System.Drawing.SystemColors.Info;
            this.treeList.Appearance.EvenRow.Options.UseBackColor = true;
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colChecked,
            this.colManagerName});
            this.treeList.KeyFieldName = "BUDGETDEP_GUID_ID";
            this.treeList.Location = new System.Drawing.Point(3, 29);
            this.treeList.Name = "treeList";
            this.treeList.OptionsBehavior.AutoChangeParent = false;
            this.treeList.OptionsBehavior.AutoFocusNewNode = true;
            this.treeList.OptionsBehavior.AutoNodeHeight = false;
            this.treeList.OptionsBehavior.DragNodes = true;
            this.treeList.OptionsBehavior.ImmediateEditor = false;
            this.treeList.OptionsBehavior.KeepSelectedOnClick = false;
            this.treeList.OptionsBehavior.ShowEditorOnMouseUp = true;
            this.treeList.OptionsBehavior.SmartMouseHover = false;
            this.treeList.OptionsPrint.PrintPreview = true;
            this.treeList.OptionsView.EnableAppearanceEvenRow = true;
            this.treeList.OptionsView.ShowPreview = true;
            this.treeList.ParentFieldName = "BUDGETDEP_PARENT_GUID_ID";
            this.treeList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItemCheckEdit_ReadOnly,
            this.repositoryItemMemoExEdit1,
            this.repItemlkpBudgetDep,
            this.repItemUsersList,
            this.repItemChecked});
            this.treeList.Size = new System.Drawing.Size(442, 225);
            this.treeList.TabIndex = 3;
            this.treeList.CellValueChanged += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.treeList_CellValueChanged);
            // 
            // colChecked
            // 
            this.colChecked.Caption = "Вкл.";
            this.colChecked.ColumnEdit = this.repItemChecked;
            this.colChecked.FieldName = "Вкл.";
            this.colChecked.MinWidth = 30;
            this.colChecked.Name = "colChecked";
            this.colChecked.OptionsColumn.AllowSort = false;
            this.colChecked.Visible = true;
            this.colChecked.VisibleIndex = 0;
            this.colChecked.Width = 55;
            // 
            // repItemChecked
            // 
            this.repItemChecked.AutoHeight = false;
            this.repItemChecked.Name = "repItemChecked";
            // 
            // colManagerName
            // 
            this.colManagerName.Caption = "Сотрудники с правом \"Распорядитель бюджета\"";
            this.colManagerName.FieldName = "Фамилия Имя";
            this.colManagerName.Name = "colManagerName";
            this.colManagerName.OptionsColumn.AllowEdit = false;
            this.colManagerName.OptionsColumn.ReadOnly = true;
            this.colManagerName.Visible = true;
            this.colManagerName.VisibleIndex = 1;
            this.colManagerName.Width = 366;
            // 
            // repItemCheckEdit_ReadOnly
            // 
            this.repItemCheckEdit_ReadOnly.AutoHeight = false;
            this.repItemCheckEdit_ReadOnly.Name = "repItemCheckEdit_ReadOnly";
            // 
            // repositoryItemMemoExEdit1
            // 
            this.repositoryItemMemoExEdit1.AutoHeight = false;
            this.repositoryItemMemoExEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemMemoExEdit1.Name = "repositoryItemMemoExEdit1";
            this.repositoryItemMemoExEdit1.ShowIcon = false;
            // 
            // repItemlkpBudgetDep
            // 
            this.repItemlkpBudgetDep.AutoHeight = false;
            this.repItemlkpBudgetDep.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemlkpBudgetDep.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("BUDGETDEP_NAME", "Бюджетное подразделение", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.repItemlkpBudgetDep.DisplayMember = "BUDGETDEP_NAME";
            this.repItemlkpBudgetDep.Name = "repItemlkpBudgetDep";
            this.repItemlkpBudgetDep.NullText = "";
            this.repItemlkpBudgetDep.ValueMember = "BUDGETDEP_GUID_ID";
            // 
            // repItemUsersList
            // 
            this.repItemUsersList.AutoHeight = false;
            this.repItemUsersList.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemUsersList.Name = "repItemUsersList";
            this.repItemUsersList.NullText = "";
            this.repItemUsersList.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.DoubleClick;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 260);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(442, 33);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::DebitArticle.Properties.Resources.undo;
            this.btnCancel.Location = new System.Drawing.Point(351, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 27);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отменить";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::DebitArticle.Properties.Resources.disk_blue_ok;
            this.btnSave.Location = new System.Drawing.Point(257, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 27);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // frmBudgetDepManagerList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 296);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBudgetDepManagerList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Список дополнительных распорядителей";
            this.Shown += new System.EventHandler(this.frmBudgetDepManagerList_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemChecked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCheckEdit_ReadOnly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemlkpBudgetDep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemUsersList)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colChecked;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repItemChecked;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colManagerName;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repItemCheckEdit_ReadOnly;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit repositoryItemMemoExEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemlkpBudgetDep;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemUsersList;
        private System.Windows.Forms.Label label1;
    }
}