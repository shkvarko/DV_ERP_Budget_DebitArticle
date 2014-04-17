namespace DebitArticle
{
    partial class frmBudgetDep
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBudgetDep));
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barBtnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnAddNode = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnAddChildNode = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnDeleteNode = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnMaagerList = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDown = new DevExpress.XtraEditors.SimpleButton();
            this.btnLeft = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnDrop = new DevExpress.XtraEditors.SimpleButton();
            this.btnUp = new DevExpress.XtraEditors.SimpleButton();
            this.btnRight = new DevExpress.XtraEditors.SimpleButton();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.colGuid_ID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colParentGuid_ID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colBudgetDepName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colBudgetDeclrn = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colReadOnly = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemCheckEdit_ReadOnly = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colBudgetManager = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemUsersList = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.dsDirect = new System.Data.DataSet();
            this.dtBudgetDep = new System.Data.DataTable();
            this.BUDGETDEP_GUID_ID = new System.Data.DataColumn();
            this.BUDGETDEP_NAME = new System.Data.DataColumn();
            this.BUDGETDEP_PARENT_GUID_ID = new System.Data.DataColumn();
            this.BUDGETDEP_MANAGER_ID = new System.Data.DataColumn();
            this.BUDGETDEP_DECLRN = new System.Data.DataColumn();
            this.READONLY = new System.Data.DataColumn();
            this.dtUser = new System.Data.DataTable();
            this.USER_ID = new System.Data.DataColumn();
            this.USER_NAME = new System.Data.DataColumn();
            this.USER_DESCRIPTION = new System.Data.DataColumn();
            this.USER_LOGONNAME = new System.Data.DataColumn();
            this.dtBudgetDepDeclrn = new System.Data.DataTable();
            this.BD_BUDGETDEP_GUID_ID = new System.Data.DataColumn();
            this.BD_USER_ID = new System.Data.DataColumn();
            this.BD_USER_NAME = new System.Data.DataColumn();
            this.repositoryItemMemoExEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.repItemlkpBudgetDep = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.checklboxBudgetDepDclrn = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mitemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemAddRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemAddChild = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mitemManagerList = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCheckEdit_ReadOnly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemUsersList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsDirect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBudgetDep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBudgetDepDeclrn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemlkpBudgetDep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checklboxBudgetDepDclrn)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barBtnAddNode,
            this.barBtnRefresh,
            this.barBtnDeleteNode,
            this.barBtnPrint,
            this.barBtnAddChildNode,
            this.barBtnMaagerList});
            this.barManager.MaxItemId = 7;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 1";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnRefresh),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnAddNode),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnAddChildNode),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnDeleteNode),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnPrint),
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnMaagerList)});
            this.bar1.Text = "Custom 1";
            // 
            // barBtnRefresh
            // 
            this.barBtnRefresh.Caption = "Обновить";
            this.barBtnRefresh.Glyph = global::DebitArticle.Properties.Resources.refresh;
            this.barBtnRefresh.Hint = "Обновить";
            this.barBtnRefresh.Id = 1;
            this.barBtnRefresh.Name = "barBtnRefresh";
            this.barBtnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnRefresh_ItemClick);
            // 
            // barBtnAddNode
            // 
            this.barBtnAddNode.Caption = "Добавить";
            this.barBtnAddNode.Glyph = global::DebitArticle.Properties.Resources.treenode_add16_h_root2;
            this.barBtnAddNode.Hint = "Добавить подразделение";
            this.barBtnAddNode.Id = 0;
            this.barBtnAddNode.Name = "barBtnAddNode";
            this.barBtnAddNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnAddNode_ItemClick);
            // 
            // barBtnAddChildNode
            // 
            this.barBtnAddChildNode.Caption = "Добавить подстатью";
            this.barBtnAddChildNode.Glyph = global::DebitArticle.Properties.Resources.treenode_add16_h_child2;
            this.barBtnAddChildNode.Hint = "Добавить дочернее подразделение";
            this.barBtnAddChildNode.Id = 5;
            this.barBtnAddChildNode.Name = "barBtnAddChildNode";
            this.barBtnAddChildNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnAddChildNode_ItemClick);
            // 
            // barBtnDeleteNode
            // 
            this.barBtnDeleteNode.Caption = "Удалить";
            this.barBtnDeleteNode.Glyph = global::DebitArticle.Properties.Resources.treenode_delete16_h2;
            this.barBtnDeleteNode.Hint = "Удалить подразделение";
            this.barBtnDeleteNode.Id = 2;
            this.barBtnDeleteNode.Name = "barBtnDeleteNode";
            this.barBtnDeleteNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnDeleteNode_ItemClick);
            // 
            // barBtnPrint
            // 
            this.barBtnPrint.Caption = "Печать";
            this.barBtnPrint.Glyph = global::DebitArticle.Properties.Resources.printer2;
            this.barBtnPrint.Hint = "Печать";
            this.barBtnPrint.Id = 3;
            this.barBtnPrint.Name = "barBtnPrint";
            this.barBtnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnPrint_ItemClick);
            // 
            // barBtnMaagerList
            // 
            this.barBtnMaagerList.Glyph = global::DebitArticle.Properties.Resources.IMAGES_BUDGETDEPSMALL_PNG;
            this.barBtnMaagerList.Hint = "Дополнительные распорядители";
            this.barBtnMaagerList.Id = 6;
            this.barBtnMaagerList.Name = "barBtnMaagerList";
            this.barBtnMaagerList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnMaagerList_ItemClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.splitContainerControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(644, 400);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel2.Controls.Add(this.btnDown, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnLeft, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDrop, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnUp, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRight, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 366);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(638, 31);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(35, 3);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(25, 24);
            this.btnDown.TabIndex = 2;
            this.btnDown.ToolTip = "Перемещение узла вниз";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(3, 3);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(25, 24);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.ToolTip = "Перемещение узла влево";
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = global::DebitArticle.Properties.Resources.disk_blue_ok;
            this.btnSave.Location = new System.Drawing.Point(346, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(141, 25);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Сохранить изменения";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = global::DebitArticle.Properties.Resources.undo;
            this.btnCancel.Location = new System.Drawing.Point(494, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отменить изменения";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDrop
            // 
            this.btnDrop.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDrop.Image = ((System.Drawing.Image)(resources.GetObject("btnDrop.Image")));
            this.btnDrop.Location = new System.Drawing.Point(67, 3);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(25, 24);
            this.btnDrop.TabIndex = 3;
            this.btnDrop.ToolTipTitle = "Удаление узла";
            this.btnDrop.Click += new System.EventHandler(this.mitemDelete_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(99, 3);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(25, 24);
            this.btnUp.TabIndex = 4;
            this.btnUp.ToolTipTitle = "Перемещение узла вверх";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnRight
            // 
            this.btnRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.Location = new System.Drawing.Point(131, 3);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(25, 24);
            this.btnRight.TabIndex = 5;
            this.btnRight.ToolTipTitle = "Перемещение узла вправо";
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Location = new System.Drawing.Point(3, 3);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.splitContainerControl1.Panel1.Controls.Add(this.treeList);
            this.splitContainerControl1.Panel1.MinSize = 100;
            this.splitContainerControl1.Panel1.Text = "splitContainerControl1_Panel1";
            this.splitContainerControl1.Panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.splitContainerControl1.Panel2.Controls.Add(this.checklboxBudgetDepDclrn);
            this.splitContainerControl1.Panel2.MinSize = 100;
            this.splitContainerControl1.Panel2.Text = "splitContainerControl1_Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(638, 357);
            this.splitContainerControl1.SplitterPosition = 165;
            this.splitContainerControl1.TabIndex = 2;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // treeList
            // 
            this.treeList.AllowDrop = true;
            this.treeList.Appearance.EvenRow.BackColor = System.Drawing.SystemColors.Info;
            this.treeList.Appearance.EvenRow.Options.UseBackColor = true;
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colGuid_ID,
            this.colParentGuid_ID,
            this.colBudgetDepName,
            this.colBudgetDeclrn,
            this.colReadOnly,
            this.colBudgetManager});
            this.treeList.DataMember = "dtBudgetDep";
            this.treeList.DataSource = this.dsDirect;
            this.treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList.KeyFieldName = "BUDGETDEP_GUID_ID";
            this.treeList.Location = new System.Drawing.Point(0, 0);
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
            this.repItemUsersList});
            this.treeList.Size = new System.Drawing.Size(467, 357);
            this.treeList.TabIndex = 1;
            this.treeList.BeforeFocusNode += new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler(this.treeList_BeforeFocusNode);
            this.treeList.AfterFocusNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList_AfterFocusNode);
            this.treeList.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_FocusedNodeChanged);
            this.treeList.CellValueChanged += new DevExpress.XtraTreeList.CellValueChangedEventHandler(this.treeList_CellValueChanged);
            this.treeList.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeList_DragDrop);
            this.treeList.DragOver += new System.Windows.Forms.DragEventHandler(this.treeList_DragOver);
            this.treeList.DragLeave += new System.EventHandler(this.treeList_DragLeave);
            this.treeList.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.treeList_GiveFeedback);
            this.treeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeList_KeyDown);
            this.treeList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList_MouseClick);
            // 
            // colGuid_ID
            // 
            this.colGuid_ID.Caption = "BUDGETDEP_GUID_ID";
            this.colGuid_ID.FieldName = "BUDGETDEP_GUID_ID";
            this.colGuid_ID.Name = "colGuid_ID";
            this.colGuid_ID.Width = 48;
            // 
            // colParentGuid_ID
            // 
            this.colParentGuid_ID.Caption = "BUDGETDEP_PARENT_GUID_ID";
            this.colParentGuid_ID.FieldName = "BUDGETDEP_PARENT_GUID_ID";
            this.colParentGuid_ID.Name = "colParentGuid_ID";
            this.colParentGuid_ID.Width = 47;
            // 
            // colBudgetDepName
            // 
            this.colBudgetDepName.Caption = "Наименование";
            this.colBudgetDepName.FieldName = "BUDGETDEP_NAME";
            this.colBudgetDepName.Name = "colBudgetDepName";
            this.colBudgetDepName.Visible = true;
            this.colBudgetDepName.VisibleIndex = 0;
            this.colBudgetDepName.Width = 220;
            // 
            // colBudgetDeclrn
            // 
            this.colBudgetDeclrn.Caption = "Состав";
            this.colBudgetDeclrn.FieldName = "BUDGETDEP_DECLRN";
            this.colBudgetDeclrn.Name = "colBudgetDeclrn";
            this.colBudgetDeclrn.OptionsColumn.AllowEdit = false;
            this.colBudgetDeclrn.OptionsColumn.ReadOnly = true;
            this.colBudgetDeclrn.Width = 154;
            // 
            // colReadOnly
            // 
            this.colReadOnly.Caption = "READONLY";
            this.colReadOnly.ColumnEdit = this.repItemCheckEdit_ReadOnly;
            this.colReadOnly.FieldName = "READONLY";
            this.colReadOnly.Name = "colReadOnly";
            this.colReadOnly.OptionsColumn.AllowEdit = false;
            this.colReadOnly.OptionsColumn.ReadOnly = true;
            // 
            // repItemCheckEdit_ReadOnly
            // 
            this.repItemCheckEdit_ReadOnly.AutoHeight = false;
            this.repItemCheckEdit_ReadOnly.Name = "repItemCheckEdit_ReadOnly";
            // 
            // colBudgetManager
            // 
            this.colBudgetManager.Caption = "Руководитель";
            this.colBudgetManager.ColumnEdit = this.repItemUsersList;
            this.colBudgetManager.FieldName = "BUDGETDEP_MANAGER_ID";
            this.colBudgetManager.Format.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colBudgetManager.Name = "colBudgetManager";
            this.colBudgetManager.Visible = true;
            this.colBudgetManager.VisibleIndex = 1;
            this.colBudgetManager.Width = 197;
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
            // dsDirect
            // 
            this.dsDirect.DataSetName = "dsDebitArticle";
            this.dsDirect.Relations.AddRange(new System.Data.DataRelation[] {
            new System.Data.DataRelation("rlBudgetDeclrn", "dtBudgetDep", "dtBudgetDepDeclrn", new string[] {
                        "BUDGETDEP_GUID_ID"}, new string[] {
                        "BD_BUDGETDEP_GUID_ID"}, false)});
            this.dsDirect.Tables.AddRange(new System.Data.DataTable[] {
            this.dtBudgetDep,
            this.dtUser,
            this.dtBudgetDepDeclrn});
            // 
            // dtBudgetDep
            // 
            this.dtBudgetDep.Columns.AddRange(new System.Data.DataColumn[] {
            this.BUDGETDEP_GUID_ID,
            this.BUDGETDEP_NAME,
            this.BUDGETDEP_PARENT_GUID_ID,
            this.BUDGETDEP_MANAGER_ID,
            this.BUDGETDEP_DECLRN,
            this.READONLY});
            this.dtBudgetDep.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.UniqueConstraint("Constraint1", new string[] {
                        "BUDGETDEP_GUID_ID"}, true)});
            this.dtBudgetDep.PrimaryKey = new System.Data.DataColumn[] {
        this.BUDGETDEP_GUID_ID};
            this.dtBudgetDep.TableName = "dtBudgetDep";
            // 
            // BUDGETDEP_GUID_ID
            // 
            this.BUDGETDEP_GUID_ID.AllowDBNull = false;
            this.BUDGETDEP_GUID_ID.ColumnName = "BUDGETDEP_GUID_ID";
            this.BUDGETDEP_GUID_ID.DataType = typeof(System.Guid);
            // 
            // BUDGETDEP_NAME
            // 
            this.BUDGETDEP_NAME.ColumnName = "BUDGETDEP_NAME";
            // 
            // BUDGETDEP_PARENT_GUID_ID
            // 
            this.BUDGETDEP_PARENT_GUID_ID.ColumnName = "BUDGETDEP_PARENT_GUID_ID";
            this.BUDGETDEP_PARENT_GUID_ID.DataType = typeof(System.Guid);
            // 
            // BUDGETDEP_MANAGER_ID
            // 
            this.BUDGETDEP_MANAGER_ID.ColumnName = "BUDGETDEP_MANAGER_ID";
            this.BUDGETDEP_MANAGER_ID.DataType = typeof(int);
            // 
            // BUDGETDEP_DECLRN
            // 
            this.BUDGETDEP_DECLRN.ColumnName = "BUDGETDEP_DECLRN";
            // 
            // READONLY
            // 
            this.READONLY.ColumnName = "READONLY";
            this.READONLY.DataType = typeof(bool);
            // 
            // dtUser
            // 
            this.dtUser.Columns.AddRange(new System.Data.DataColumn[] {
            this.USER_ID,
            this.USER_NAME,
            this.USER_DESCRIPTION,
            this.USER_LOGONNAME});
            this.dtUser.TableName = "dtUser";
            // 
            // USER_ID
            // 
            this.USER_ID.ColumnName = "USER_ID";
            this.USER_ID.DataType = typeof(int);
            // 
            // USER_NAME
            // 
            this.USER_NAME.ColumnName = "USER_NAME";
            // 
            // USER_DESCRIPTION
            // 
            this.USER_DESCRIPTION.ColumnName = "USER_DESCRIPTION";
            // 
            // USER_LOGONNAME
            // 
            this.USER_LOGONNAME.ColumnName = "USER_LOGONNAME";
            // 
            // dtBudgetDepDeclrn
            // 
            this.dtBudgetDepDeclrn.Columns.AddRange(new System.Data.DataColumn[] {
            this.BD_BUDGETDEP_GUID_ID,
            this.BD_USER_ID,
            this.BD_USER_NAME});
            this.dtBudgetDepDeclrn.Constraints.AddRange(new System.Data.Constraint[] {
            new System.Data.ForeignKeyConstraint("rlBudgetDeclrn", "dtBudgetDep", new string[] {
                        "BUDGETDEP_GUID_ID"}, new string[] {
                        "BD_BUDGETDEP_GUID_ID"}, System.Data.AcceptRejectRule.None, System.Data.Rule.Cascade, System.Data.Rule.Cascade)});
            this.dtBudgetDepDeclrn.TableName = "dtBudgetDepDeclrn";
            // 
            // BD_BUDGETDEP_GUID_ID
            // 
            this.BD_BUDGETDEP_GUID_ID.ColumnName = "BD_BUDGETDEP_GUID_ID";
            this.BD_BUDGETDEP_GUID_ID.DataType = typeof(System.Guid);
            // 
            // BD_USER_ID
            // 
            this.BD_USER_ID.ColumnName = "BD_USER_ID";
            this.BD_USER_ID.DataType = typeof(int);
            // 
            // BD_USER_NAME
            // 
            this.BD_USER_NAME.ColumnName = "BD_USER_NAME";
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
            this.repItemlkpBudgetDep.DataSource = this.dsDirect;
            this.repItemlkpBudgetDep.DisplayMember = "BUDGETDEP_NAME";
            this.repItemlkpBudgetDep.Name = "repItemlkpBudgetDep";
            this.repItemlkpBudgetDep.NullText = "";
            this.repItemlkpBudgetDep.ValueMember = "BUDGETDEP_GUID_ID";
            // 
            // checklboxBudgetDepDclrn
            // 
            this.checklboxBudgetDepDclrn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checklboxBudgetDepDclrn.Location = new System.Drawing.Point(0, 0);
            this.checklboxBudgetDepDclrn.Name = "checklboxBudgetDepDclrn";
            this.checklboxBudgetDepDclrn.Size = new System.Drawing.Size(165, 357);
            this.checklboxBudgetDepDclrn.TabIndex = 0;
            this.checklboxBudgetDepDclrn.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.checklboxBudgetDepDclrn_ItemCheck);
            this.checklboxBudgetDepDclrn.ValueMemberChanged += new System.EventHandler(this.checklboxBudgetDepDclrn_ValueMemberChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitemRefresh,
            this.mitemAddRoot,
            this.mitemAddChild,
            this.mitemDelete,
            this.toolStripMenuItem1,
            this.mitemManagerList});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(259, 120);
            // 
            // mitemRefresh
            // 
            this.mitemRefresh.Image = global::DebitArticle.Properties.Resources.refresh;
            this.mitemRefresh.Name = "mitemRefresh";
            this.mitemRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mitemRefresh.Size = new System.Drawing.Size(258, 22);
            this.mitemRefresh.Text = "Обновить";
            this.mitemRefresh.Click += new System.EventHandler(this.mitemRefresh_Click);
            // 
            // mitemAddRoot
            // 
            this.mitemAddRoot.AutoToolTip = true;
            this.mitemAddRoot.Image = global::DebitArticle.Properties.Resources.treenode_add16_h_root2;
            this.mitemAddRoot.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mitemAddRoot.Name = "mitemAddRoot";
            this.mitemAddRoot.Size = new System.Drawing.Size(258, 22);
            this.mitemAddRoot.Text = "Добавить подразделение";
            this.mitemAddRoot.Click += new System.EventHandler(this.mitemAddRoot_Click);
            // 
            // mitemAddChild
            // 
            this.mitemAddChild.AutoToolTip = true;
            this.mitemAddChild.Image = global::DebitArticle.Properties.Resources.treenode_add16_h_child2;
            this.mitemAddChild.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mitemAddChild.Name = "mitemAddChild";
            this.mitemAddChild.Size = new System.Drawing.Size(258, 22);
            this.mitemAddChild.Text = "Добавить дочернее подразделение";
            this.mitemAddChild.ToolTipText = "Добавить дочернее подразделение";
            this.mitemAddChild.Click += new System.EventHandler(this.mitemAddChild_Click);
            // 
            // mitemDelete
            // 
            this.mitemDelete.AutoToolTip = true;
            this.mitemDelete.Image = global::DebitArticle.Properties.Resources.treenode_delete16_h2;
            this.mitemDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mitemDelete.Name = "mitemDelete";
            this.mitemDelete.Size = new System.Drawing.Size(258, 22);
            this.mitemDelete.Text = "Удалить подразделение";
            this.mitemDelete.Click += new System.EventHandler(this.mitemDelete_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(255, 6);
            // 
            // mitemManagerList
            // 
            this.mitemManagerList.Name = "mitemManagerList";
            this.mitemManagerList.Size = new System.Drawing.Size(258, 22);
            this.mitemManagerList.Text = "Дополнительные распорядители";
            this.mitemManagerList.Click += new System.EventHandler(this.mitemManagerList_Click);
            // 
            // frmBudgetDep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 426);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmBudgetDep";
            this.Text = "Бюджетные подразделения";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBudgetDep_FormClosing);
            this.Load += new System.EventHandler(this.frmDebitArticle_Load);
            this.Shown += new System.EventHandler(this.frmBudgetDep_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCheckEdit_ReadOnly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemUsersList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsDirect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBudgetDep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBudgetDepDeclrn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemlkpBudgetDep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checklboxBudgetDepDclrn)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem barBtnAddNode;
        private DevExpress.XtraBars.BarButtonItem barBtnRefresh;
        private DevExpress.XtraBars.BarButtonItem barBtnDeleteNode;
        private DevExpress.XtraBars.BarButtonItem barBtnPrint;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.SimpleButton btnRight;
        private DevExpress.XtraEditors.SimpleButton btnDrop;
        private DevExpress.XtraEditors.SimpleButton btnDown;
        private DevExpress.XtraEditors.SimpleButton btnLeft;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnUp;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colBudgetDepName;
        private System.Data.DataSet dsDirect;
        private DevExpress.XtraBars.BarButtonItem barBtnAddChildNode;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mitemRefresh;
        private System.Windows.Forms.ToolStripMenuItem mitemAddRoot;
        private System.Windows.Forms.ToolStripMenuItem mitemAddChild;
        private System.Windows.Forms.ToolStripMenuItem mitemDelete;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colGuid_ID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentGuid_ID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colReadOnly;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repItemCheckEdit_ReadOnly;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit repositoryItemMemoExEdit1;
        private System.Data.DataTable dtBudgetDep;
        private System.Data.DataColumn BUDGETDEP_GUID_ID;
        private System.Data.DataColumn BUDGETDEP_NAME;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemlkpBudgetDep;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colBudgetDeclrn;
        private DevExpress.XtraEditors.CheckedListBoxControl checklboxBudgetDepDclrn;
        private System.Data.DataColumn BUDGETDEP_PARENT_GUID_ID;
        private System.Data.DataColumn BUDGETDEP_MANAGER_ID;
        private System.Data.DataTable dtUser;
        private System.Data.DataColumn USER_ID;
        private System.Data.DataColumn USER_NAME;
        private System.Data.DataColumn USER_DESCRIPTION;
        private System.Data.DataColumn USER_LOGONNAME;
        private System.Data.DataTable dtBudgetDepDeclrn;
        private System.Data.DataColumn BD_BUDGETDEP_GUID_ID;
        private System.Data.DataColumn BD_USER_ID;
        private System.Data.DataColumn BD_USER_NAME;
        private System.Data.DataColumn BUDGETDEP_DECLRN;
        private System.Data.DataColumn READONLY;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colBudgetManager;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemUsersList;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mitemManagerList;
        private DevExpress.XtraBars.BarButtonItem barBtnMaagerList;
    }
}