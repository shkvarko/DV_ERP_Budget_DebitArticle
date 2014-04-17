namespace DebitArticle
{
    partial class frmDebitArticle
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDebitArticle));
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barBtnRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnAddNode = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnAddChildNode = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnDeleteNode = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDown = new DevExpress.XtraEditors.SimpleButton();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mitemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemAddRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemAddChild = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemRecalcDebitArticleNum = new System.Windows.Forms.ToolStripMenuItem();
            this.mitemRecalcAllDebitArticleNum = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mitemImportDebitArticleList = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanelDebitArticleList = new System.Windows.Forms.TableLayoutPanel();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.colGuid_ID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colParentGuid_ID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDebitArticleNum = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colAccountPlan = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colReadOnly = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repItemCheckEdit_ReadOnly = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.treeListBudgetDept = new DevExpress.XtraTreeList.TreeList();
            this.colBudgetDep = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colExpenseType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colProject = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemMemoExEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.repItemlkpBudgetDep = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repItemUsersList = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lblDebitArticleNum = new DevExpress.XtraEditors.LabelControl();
            this.btnLeft = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnDrop = new DevExpress.XtraEditors.SimpleButton();
            this.btnUp = new DevExpress.XtraEditors.SimpleButton();
            this.btnRight = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.tableLayoutPanelDebitArticleList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCheckEdit_ReadOnly)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListBudgetDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemlkpBudgetDep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemUsersList)).BeginInit();
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
            this.barBtnAddChildNode});
            this.barManager.MaxItemId = 6;
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
            new DevExpress.XtraBars.LinkPersistInfo(this.barBtnPrint)});
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
            this.barBtnAddNode.Hint = "Добавить статью";
            this.barBtnAddNode.Id = 0;
            this.barBtnAddNode.Name = "barBtnAddNode";
            this.barBtnAddNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnAddNode_ItemClick);
            // 
            // barBtnAddChildNode
            // 
            this.barBtnAddChildNode.Caption = "Добавить подстатью";
            this.barBtnAddChildNode.Glyph = global::DebitArticle.Properties.Resources.treenode_add16_h_child2;
            this.barBtnAddChildNode.Hint = "Добавить подстатью";
            this.barBtnAddChildNode.Id = 5;
            this.barBtnAddChildNode.Name = "barBtnAddChildNode";
            this.barBtnAddChildNode.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnAddChildNode_ItemClick);
            // 
            // barBtnDeleteNode
            // 
            this.barBtnDeleteNode.Caption = "Удалить";
            this.barBtnDeleteNode.Glyph = global::DebitArticle.Properties.Resources.treenode_delete16_h2;
            this.barBtnDeleteNode.Hint = "Удалить статью/подстатью";
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
            // barDockControlTop
            // 
            this.toolTipController.SetSuperTip(this.barDockControlTop, null);
            // 
            // barDockControlBottom
            // 
            this.toolTipController.SetSuperTip(this.barDockControlBottom, null);
            // 
            // barDockControlLeft
            // 
            this.toolTipController.SetSuperTip(this.barDockControlLeft, null);
            // 
            // barDockControlRight
            // 
            this.toolTipController.SetSuperTip(this.barDockControlRight, null);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanelDebitArticleList, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 26);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(801, 480);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel1, null);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tableLayoutPanel2.Controls.Add(this.btnDown, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnLeft, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDrop, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnUp, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRight, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(1, 446);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(799, 33);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel2, null);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(31, 4);
            this.btnDown.Margin = new System.Windows.Forms.Padding(1);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(25, 25);
            this.btnDown.TabIndex = 2;
            this.btnDown.ToolTip = "Перемещение узла вниз";
            this.btnDown.ToolTipController = this.toolTipController;
            this.btnDown.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mitemRefresh,
            this.mitemAddRoot,
            this.mitemAddChild,
            this.mitemDelete,
            this.mitemRecalcDebitArticleNum,
            this.mitemRecalcAllDebitArticleNum,
            this.toolStripMenuItem1,
            this.mitemImportDebitArticleList});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(244, 164);
            this.toolTipController.SetSuperTip(this.contextMenuStrip, null);
            // 
            // mitemRefresh
            // 
            this.mitemRefresh.Image = global::DebitArticle.Properties.Resources.refresh;
            this.mitemRefresh.Name = "mitemRefresh";
            this.mitemRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mitemRefresh.Size = new System.Drawing.Size(243, 22);
            this.mitemRefresh.Text = "Обновить";
            this.mitemRefresh.Click += new System.EventHandler(this.mitemRefresh_Click);
            // 
            // mitemAddRoot
            // 
            this.mitemAddRoot.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mitemAddRoot.Name = "mitemAddRoot";
            this.mitemAddRoot.Size = new System.Drawing.Size(243, 22);
            this.mitemAddRoot.Text = "Добавить статью";
            this.mitemAddRoot.Click += new System.EventHandler(this.mitemAddRoot_Click);
            // 
            // mitemAddChild
            // 
            this.mitemAddChild.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mitemAddChild.Name = "mitemAddChild";
            this.mitemAddChild.Size = new System.Drawing.Size(243, 22);
            this.mitemAddChild.Text = "Добавить подстатью";
            this.mitemAddChild.Click += new System.EventHandler(this.mitemAddChild_Click);
            // 
            // mitemDelete
            // 
            this.mitemDelete.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.mitemDelete.Name = "mitemDelete";
            this.mitemDelete.Size = new System.Drawing.Size(243, 22);
            this.mitemDelete.Text = "Удалить статью/подстатью";
            this.mitemDelete.Click += new System.EventHandler(this.mitemDelete_Click);
            // 
            // mitemRecalcDebitArticleNum
            // 
            this.mitemRecalcDebitArticleNum.Name = "mitemRecalcDebitArticleNum";
            this.mitemRecalcDebitArticleNum.Size = new System.Drawing.Size(243, 22);
            this.mitemRecalcDebitArticleNum.Text = "Пересчитать номера подстатей";
            // 
            // mitemRecalcAllDebitArticleNum
            // 
            this.mitemRecalcAllDebitArticleNum.Name = "mitemRecalcAllDebitArticleNum";
            this.mitemRecalcAllDebitArticleNum.Size = new System.Drawing.Size(243, 22);
            this.mitemRecalcAllDebitArticleNum.Text = "Пересчитать номера всех статей";
            this.mitemRecalcAllDebitArticleNum.Click += new System.EventHandler(this.mitemRecalcAllDebitArticleNum_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(240, 6);
            // 
            // mitemImportDebitArticleList
            // 
            this.mitemImportDebitArticleList.Name = "mitemImportDebitArticleList";
            this.mitemImportDebitArticleList.Size = new System.Drawing.Size(243, 22);
            this.mitemImportDebitArticleList.Text = "Импорт статей расходов...";
            this.mitemImportDebitArticleList.Click += new System.EventHandler(this.mitemImportDebitArticleList_Click);
            // 
            // tableLayoutPanelDebitArticleList
            // 
            this.tableLayoutPanelDebitArticleList.ColumnCount = 2;
            this.tableLayoutPanelDebitArticleList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.05243F));
            this.tableLayoutPanelDebitArticleList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.94757F));
            this.tableLayoutPanelDebitArticleList.Controls.Add(this.treeList, 0, 0);
            this.tableLayoutPanelDebitArticleList.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanelDebitArticleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDebitArticleList.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDebitArticleList.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelDebitArticleList.Name = "tableLayoutPanelDebitArticleList";
            this.tableLayoutPanelDebitArticleList.RowCount = 1;
            this.tableLayoutPanelDebitArticleList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDebitArticleList.Size = new System.Drawing.Size(801, 445);
            this.toolTipController.SetSuperTip(this.tableLayoutPanelDebitArticleList, null);
            this.tableLayoutPanelDebitArticleList.TabIndex = 2;
            // 
            // treeList
            // 
            this.treeList.AllowDrop = true;
            this.treeList.Appearance.EvenRow.BackColor = System.Drawing.SystemColors.Info;
            this.treeList.Appearance.EvenRow.Options.UseBackColor = true;
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colGuid_ID,
            this.colParentGuid_ID,
            this.colDebitArticleNum,
            this.colAccountPlan,
            this.colReadOnly});
            this.treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList.Location = new System.Drawing.Point(3, 3);
            this.treeList.Name = "treeList";
            this.treeList.OptionsBehavior.AutoChangeParent = false;
            this.treeList.OptionsBehavior.AutoNodeHeight = false;
            this.treeList.OptionsBehavior.DragNodes = true;
            this.treeList.OptionsBehavior.ImmediateEditor = false;
            this.treeList.OptionsBehavior.KeepSelectedOnClick = false;
            this.treeList.OptionsBehavior.SmartMouseHover = false;
            this.treeList.OptionsPrint.PrintPreview = true;
            this.treeList.OptionsView.EnableAppearanceEvenRow = true;
            this.treeList.OptionsView.ShowPreview = true;
            this.treeList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItemCheckEdit_ReadOnly});
            this.treeList.Size = new System.Drawing.Size(458, 439);
            this.treeList.TabIndex = 1;
            this.treeList.ToolTipController = this.toolTipController;
            this.treeList.AfterDragNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList_AfterDragNode);
            this.treeList.AfterFocusNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList_AfterFocusNode);
            this.treeList.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.treeList_CustomDrawNodeCell);
            this.treeList.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeList_DragDrop);
            this.treeList.DragOver += new System.Windows.Forms.DragEventHandler(this.treeList_DragOver);
            this.treeList.DragLeave += new System.EventHandler(this.treeList_DragLeave);
            this.treeList.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.treeList_GiveFeedback);
            this.treeList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeList_KeyDown);
            this.treeList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeList_MouseClick);
            this.treeList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeList_MouseDoubleClick);
            // 
            // colGuid_ID
            // 
            this.colGuid_ID.Caption = "DEBITARTICLE_GUID_ID";
            this.colGuid_ID.FieldName = "DEBITARTICLE_GUID_ID";
            this.colGuid_ID.Name = "colGuid_ID";
            this.colGuid_ID.Width = 48;
            // 
            // colParentGuid_ID
            // 
            this.colParentGuid_ID.Caption = "DEBITARTICLE_PARENT_GUID_ID";
            this.colParentGuid_ID.FieldName = "DEBITARTICLE_PARENT_GUID_ID";
            this.colParentGuid_ID.Name = "colParentGuid_ID";
            this.colParentGuid_ID.Width = 47;
            // 
            // colDebitArticleNum
            // 
            this.colDebitArticleNum.Caption = "Номер";
            this.colDebitArticleNum.FieldName = "Номер";
            this.colDebitArticleNum.MinWidth = 27;
            this.colDebitArticleNum.Name = "colDebitArticleNum";
            this.colDebitArticleNum.OptionsColumn.AllowEdit = false;
            this.colDebitArticleNum.OptionsColumn.AllowFocus = false;
            this.colDebitArticleNum.OptionsColumn.AllowSort = false;
            this.colDebitArticleNum.OptionsColumn.ReadOnly = true;
            this.colDebitArticleNum.Visible = true;
            this.colDebitArticleNum.VisibleIndex = 0;
            this.colDebitArticleNum.Width = 251;
            // 
            // colAccountPlan
            // 
            this.colAccountPlan.Caption = "План счетов";
            this.colAccountPlan.FieldName = "План счетов";
            this.colAccountPlan.Name = "colAccountPlan";
            this.colAccountPlan.OptionsColumn.AllowEdit = false;
            this.colAccountPlan.OptionsColumn.AllowFocus = false;
            this.colAccountPlan.OptionsColumn.ReadOnly = true;
            this.colAccountPlan.Visible = true;
            this.colAccountPlan.VisibleIndex = 1;
            this.colAccountPlan.Width = 215;
            // 
            // colReadOnly
            // 
            this.colReadOnly.Caption = "colReadOnly";
            this.colReadOnly.ColumnEdit = this.repItemCheckEdit_ReadOnly;
            this.colReadOnly.FieldName = "colReadOnly";
            this.colReadOnly.Name = "colReadOnly";
            // 
            // repItemCheckEdit_ReadOnly
            // 
            this.repItemCheckEdit_ReadOnly.AutoHeight = false;
            this.repItemCheckEdit_ReadOnly.Name = "repItemCheckEdit_ReadOnly";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.treeListBudgetDept, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblDebitArticleNum, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(464, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(337, 445);
            this.toolTipController.SetSuperTip(this.tableLayoutPanel3, null);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // treeListBudgetDept
            // 
            this.treeListBudgetDept.AllowDrop = true;
            this.treeListBudgetDept.Appearance.EvenRow.BackColor = System.Drawing.SystemColors.Info;
            this.treeListBudgetDept.Appearance.EvenRow.Options.UseBackColor = true;
            this.treeListBudgetDept.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colBudgetDep,
            this.colExpenseType,
            this.colProject});
            this.treeListBudgetDept.DataMember = "dtBudgetDep";
            this.treeListBudgetDept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListBudgetDept.KeyFieldName = "";
            this.treeListBudgetDept.Location = new System.Drawing.Point(3, 23);
            this.treeListBudgetDept.Name = "treeListBudgetDept";
            this.treeListBudgetDept.OptionsBehavior.Editable = false;
            this.treeListBudgetDept.ParentFieldName = "";
            this.treeListBudgetDept.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemMemoExEdit1,
            this.repItemlkpBudgetDep,
            this.repItemUsersList});
            this.treeListBudgetDept.Size = new System.Drawing.Size(331, 419);
            this.treeListBudgetDept.TabIndex = 14;
            // 
            // colBudgetDep
            // 
            this.colBudgetDep.Caption = "Служба";
            this.colBudgetDep.FieldName = "Служба";
            this.colBudgetDep.Name = "colBudgetDep";
            this.colBudgetDep.OptionsColumn.AllowEdit = false;
            this.colBudgetDep.OptionsColumn.AllowFocus = false;
            this.colBudgetDep.OptionsColumn.ReadOnly = true;
            this.colBudgetDep.Visible = true;
            this.colBudgetDep.VisibleIndex = 0;
            this.colBudgetDep.Width = 187;
            // 
            // colExpenseType
            // 
            this.colExpenseType.Caption = "Тип расходов";
            this.colExpenseType.FieldName = "Тип расходов";
            this.colExpenseType.Name = "colExpenseType";
            this.colExpenseType.OptionsColumn.AllowEdit = false;
            this.colExpenseType.OptionsColumn.AllowFocus = false;
            this.colExpenseType.OptionsColumn.ReadOnly = true;
            this.colExpenseType.Visible = true;
            this.colExpenseType.VisibleIndex = 1;
            this.colExpenseType.Width = 212;
            // 
            // colProject
            // 
            this.colProject.Caption = "Проект";
            this.colProject.FieldName = "Проект";
            this.colProject.Name = "colProject";
            this.colProject.OptionsColumn.AllowEdit = false;
            this.colProject.OptionsColumn.AllowFocus = false;
            this.colProject.OptionsColumn.ReadOnly = true;
            this.colProject.Visible = true;
            this.colProject.VisibleIndex = 2;
            this.colProject.Width = 148;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
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
            // lblDebitArticleNum
            // 
            this.lblDebitArticleNum.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDebitArticleNum.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDebitArticleNum.Appearance.Options.UseFont = true;
            this.lblDebitArticleNum.Location = new System.Drawing.Point(3, 3);
            this.lblDebitArticleNum.Name = "lblDebitArticleNum";
            this.lblDebitArticleNum.Size = new System.Drawing.Size(10, 13);
            this.lblDebitArticleNum.TabIndex = 15;
            this.lblDebitArticleNum.Text = "[]";
            // 
            // btnLeft
            // 
            this.btnLeft.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(1, 4);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(1);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(25, 25);
            this.btnLeft.TabIndex = 1;
            this.btnLeft.ToolTip = "Перемещение узла влево";
            this.btnLeft.ToolTipController = this.toolTipController;
            this.btnLeft.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.Image = global::DebitArticle.Properties.Resources.disk_blue_ok;
            this.btnSave.Location = new System.Drawing.Point(511, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(141, 25);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Сохранить изменения";
            this.btnSave.ToolTip = "Сохранить изменения";
            this.btnSave.ToolTipController = this.toolTipController;
            this.btnSave.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Image = global::DebitArticle.Properties.Resources.undo;
            this.btnCancel.Location = new System.Drawing.Point(657, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(141, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Отменить изменения";
            this.btnCancel.ToolTip = "Отменить изменения";
            this.btnCancel.ToolTipController = this.toolTipController;
            this.btnCancel.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDrop
            // 
            this.btnDrop.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDrop.Image = ((System.Drawing.Image)(resources.GetObject("btnDrop.Image")));
            this.btnDrop.Location = new System.Drawing.Point(61, 4);
            this.btnDrop.Margin = new System.Windows.Forms.Padding(1);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(25, 25);
            this.btnDrop.TabIndex = 3;
            this.btnDrop.ToolTip = "Удаление узла";
            this.btnDrop.ToolTipController = this.toolTipController;
            this.btnDrop.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Warning;
            this.btnDrop.Click += new System.EventHandler(this.mitemDelete_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(91, 4);
            this.btnUp.Margin = new System.Windows.Forms.Padding(1);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(25, 25);
            this.btnUp.TabIndex = 4;
            this.btnUp.ToolTip = "Перемещение узла вверх";
            this.btnUp.ToolTipController = this.toolTipController;
            this.btnUp.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnRight
            // 
            this.btnRight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.Location = new System.Drawing.Point(121, 4);
            this.btnRight.Margin = new System.Windows.Forms.Padding(1);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(25, 25);
            this.btnRight.TabIndex = 5;
            this.btnRight.ToolTip = "Перемещение узла вправо";
            this.btnRight.ToolTipController = this.toolTipController;
            this.btnRight.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // frmDebitArticle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 506);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmDebitArticle";
            this.toolTipController.SetSuperTip(this, null);
            this.Text = "Статьи расходов";
            this.Load += new System.EventHandler(this.frmDebitArticle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.tableLayoutPanelDebitArticleList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemCheckEdit_ReadOnly)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListBudgetDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemlkpBudgetDep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemUsersList)).EndInit();
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
        private DevExpress.XtraBars.BarButtonItem barBtnAddChildNode;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mitemRefresh;
        private System.Windows.Forms.ToolStripMenuItem mitemAddRoot;
        private System.Windows.Forms.ToolStripMenuItem mitemAddChild;
        private System.Windows.Forms.ToolStripMenuItem mitemDelete;
        private DevExpress.Utils.ToolTipController toolTipController;
        private System.Windows.Forms.ToolStripMenuItem mitemRecalcDebitArticleNum;
        private System.Windows.Forms.ToolStripMenuItem mitemRecalcAllDebitArticleNum;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colGuid_ID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colParentGuid_ID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDebitArticleNum;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colAccountPlan;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repItemCheckEdit_ReadOnly;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDebitArticleList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colReadOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mitemImportDebitArticleList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private DevExpress.XtraTreeList.TreeList treeListBudgetDept;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colBudgetDep;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colExpenseType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colProject;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit repositoryItemMemoExEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemlkpBudgetDep;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemUsersList;
        private DevExpress.XtraEditors.LabelControl lblDebitArticleNum;
    }
}