using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERP_Budget.Common;
using System.Threading;

namespace DebitArticle
{
    public partial class frmDebitArticle : DevExpress.XtraEditors.XtraForm
    {
        #region Переменные, Свойства, Константы
        private UniXP.Common.CProfile m_objProfile;
        private DevExpress.XtraTreeList.Nodes.TreeListNode draggedNodeParent;
        
        private frmImportDataInDebitArticleList m_frmImportDebitArticleList;
        private frmDebitArticleEditor m_frmDebitArticleEditor;

        private List<CBudgetDep> m_objBudgetDepList;
        private List<CBudgetExpenseType> m_objBudgetExpenseTypeList;
        private List<CBudgetProject> m_objBudgetProjectList;
        private List<CAccountPlan> m_objAccountPlanList;

        private System.String m_strXLSImportFilePath;
        private System.Int32 m_iXLSSheetImport;
        private List<System.String> m_SheetList;

        public delegate void LoadComboBoxForEditorDelegate(List<CBudgetDep> objBudgetDepList, List<CBudgetExpenseType> objBudgetExpenseTypeList, 
            List<CBudgetProject> objBudgetProjectList,  List<CAccountPlan> objAccountPlanList);
        public LoadComboBoxForEditorDelegate m_LoadComboBoxForEditorDelegate;
        public System.Threading.Thread ThreadComboBoxForEditor { get; set; }

        private const System.Int32 iThreadSleepTime = 1000;
        private System.Boolean m_bThreadFinishJob;

        #endregion

        #region Конструктор
        public frmDebitArticle( UniXP.Common.CProfile objProfile )
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_bDREditRootDebitArticle = false;
            draggedNodeParent = null;

            m_objBudgetDepList = null;
            m_objBudgetExpenseTypeList = null;
            m_objBudgetProjectList = null;
            m_objAccountPlanList = null;
            m_bThreadFinishJob = false;

            m_frmImportDebitArticleList = new frmImportDataInDebitArticleList(m_objProfile);
            m_frmDebitArticleEditor = new frmDebitArticleEditor(m_objProfile);
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if( components != null )
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #endregion

        #region Выпадающие списки
        /// <summary>
        /// Загружает выпадающие списки в редактор
        /// </summary>
        public void StartThreadComboBoxForEditor()
        {
            try
            {
                // инициализируем делегаты
                m_LoadComboBoxForEditorDelegate = new LoadComboBoxForEditorDelegate(LoadComboBoxForEditor);

                // запуск потока
                this.ThreadComboBoxForEditor = new System.Threading.Thread(LoadComboBoxForEditorInThread);
                this.ThreadComboBoxForEditor.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadComboBoxForEditor().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        /// <summary>
        /// Загружает выпадающие списки в редактор (метод, выполняемый в потоке)
        /// </summary>
        public void LoadComboBoxForEditorInThread()
        {
            try
            {
                System.String strErr = System.String.Empty;
                m_objBudgetDepList = CBudgetDep.GetBudgetDepsList(m_objProfile, false);
                m_objBudgetExpenseTypeList = CBudgetExpenseType.GetBudgetExpenseTypeList(m_objProfile);
                m_objBudgetProjectList = CBudgetProjectDataBaseModel.GetBudgetProjectList(m_objProfile, null, ref strErr);
                m_objAccountPlanList = CAccountPlanDataBaseModel.GetAccountPlanList(m_objProfile, null, ref strErr);


                this.Invoke(m_LoadComboBoxForEditorDelegate,
                    new Object[] {
                                    m_objBudgetDepList,  m_objBudgetExpenseTypeList, 
                                    m_objBudgetProjectList,  m_objAccountPlanList
                                  });

                this.Invoke(m_LoadComboBoxForEditorDelegate,
                    new Object[] { null, null, null, null });
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadComboBoxForEditorInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Загрузка выпадающих списков значений в редактор
        /// </summary>
        /// <param name="objBudgetDepList">список бюджетных подразделений</param>
        /// <param name="objBudgetExpenseTypeList">список типов бюджетных расходов</param>
        /// <param name="objBudgetProjectList">список бюджетных проектов</param>
        /// <param name="objAccountPlanList">план счетов</param>
        private void LoadComboBoxForEditor(List<CBudgetDep> objBudgetDepList, List<CBudgetExpenseType> objBudgetExpenseTypeList,
            List<CBudgetProject> objBudgetProjectList, List<CAccountPlan> objAccountPlanList)
        {
            try
            {
                if (
                    (objBudgetDepList != null) && (objBudgetDepList.Count > 0) &&
                    (objBudgetExpenseTypeList != null) && (objBudgetExpenseTypeList.Count > 0) &&
                    (objBudgetProjectList != null) && (objBudgetProjectList.Count > 0) &&
                    (objAccountPlanList != null) && (objAccountPlanList.Count > 0)
                    )
                {
                    if (m_frmImportDebitArticleList != null)
                    {
                        m_frmImportDebitArticleList.LoadComboBox( objBudgetDepList, objBudgetExpenseTypeList, objBudgetProjectList, objAccountPlanList );
                    }
                    if (m_frmDebitArticleEditor != null)
                    {
                        m_frmDebitArticleEditor.LoadComboBox(objBudgetDepList, objBudgetExpenseTypeList, objBudgetProjectList, objAccountPlanList); 
                    }
                }
                else
                {
                    // процесс загрузки данных завершён
                    Thread.Sleep(iThreadSleepTime);
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadComboBoxForDecodeEditor.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Динамические права
        private System.Boolean m_bDREditRootDebitArticle;
        private void SetAccessForDynamicRights()
        {
            try
            {
                // редактирование статей расходов
                m_bDREditRootDebitArticle = ( m_objProfile.GetClientsRight() ).GetState( ERP_Budget.Global.Consts.strDREditRootDebitArticle );
                if( m_bDREditRootDebitArticle == false )
                {
                    // нет прав на работу со статьями расходов
                    barBtnAddNode.Enabled = m_bDREditRootDebitArticle;
                    mitemAddRoot.Enabled = m_bDREditRootDebitArticle;
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка проверки динамических прав\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        #endregion

        #region Индикация изменений в дереве

        private System.Boolean m_bIsTreeModified;
        /// <summary>
        /// Устанавливает индикатор изменения дерева
        /// </summary>
        /// <param name="bModified">значение индикатора изменения дерева</param>
        private void SetModified( System.Boolean bModified )
        {
            try
            {
                m_bIsTreeModified = bModified;
                btnSave.Enabled = m_bIsTreeModified;
                btnCancel.Enabled = m_bIsTreeModified;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка SetModified()\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// Снимает пометку "Изменен" в узле
        /// </summary>
        /// <param name="objTreeNode">узел</param>
        private void DiscardChangesNodes( DevExpress.XtraTreeList.Nodes.TreeListNode objTreeNode )
        {
            try
            {
                if( objTreeNode == null ) { return; }

                if( ( System.Boolean )objTreeNode.GetValue( colReadOnly ) )
                {
                    objTreeNode.SetValue( colReadOnly, false );
                }

                if( objTreeNode.HasChildren )
                {
                    foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objTreeNodeChild in objTreeNode.Nodes )
                    {
                        DiscardChangesNodes( objTreeNodeChild );
                    }
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка отметки пометки \"Изменен\" в узле.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        /// <summary>
        /// Возвращает значение индикатора изменения дерева
        /// </summary>
        /// <returns></returns>
        private System.Boolean bIsModified()
        {
            return m_bIsTreeModified;
        }

        #endregion

        #region Построение дерева статей расходов

        /// <summary>
        /// Загружает дерево статей расходов
        /// </summary>
        private void LoadDebitArticles( ref System.String strErr )
        {
            //System.String strDataMember = treeList.DataMember;
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.tableLayoutPanelDebitArticleList.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                // отключаем дерево
                treeList.Enabled = false;
                treeList.AfterFocusNode -= new DevExpress.XtraTreeList.NodeEventHandler( this.treeList_AfterFocusNode );
                treeList.CustomDrawNodeCell -= new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler( this.treeList_CustomDrawNodeCell );

                CDebitArticle.LoadDebitArticleList(this.m_objProfile, treeList, colDebitArticleNum);

            }//try
            catch( System.Exception f )
            {
                strErr += ("Ошибка чтения списка статей расходов.\nТекст ошибки: " + f.Message);
            }
            finally
            {
                treeList.Enabled = true;
                SetModified( false );
                
                treeList.AfterFocusNode += new DevExpress.XtraTreeList.NodeEventHandler( this.treeList_AfterFocusNode );
                treeList.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler( this.treeList_CustomDrawNodeCell );
                
                if (treeList.Nodes.Count > 0)
                {
                    treeList.Nodes[treeList.Nodes.Count - 1].ExpandAll();
                }
                
                this.tableLayoutPanelDebitArticleList.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return ;
        }
        /// <summary>
        /// Обновляет список статей расходов
        /// </summary>
        private void RefreshDebitArticlesList()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                
                System.String strErr = "";

                LoadDebitArticles(ref strErr);
                if( strErr != "" )
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this,
                    "Ошибка обновления списка статей расходов.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }

        private void barBtnRefresh_ItemClick( object sender, DevExpress.XtraBars.ItemClickEventArgs e )
        {
            try
            {
                RefreshDebitArticlesList();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка обновления списка статей\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void mitemRefresh_Click( object sender, EventArgs e )
        {
            try
            {
                RefreshDebitArticlesList();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка обновления списка статей\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        #endregion

        #region Пересчет номеров подстатей
        /// <summary>
        /// Пересчитывает номер статьи расходов и дочерних подстатей 
        /// </summary>
        /// <param name="objNode">узел дерева</param>
        private void RecalcDebitArticleNumber( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            try
            {
                if( ( objNode == null ) || ( objNode.Tag == null ) ) { return; }

                // Сперва нужно определить новый номер
                System.String strDebitArticleNum = "";
                if( objNode.ParentNode == null )
                {
                    // статья расходов
                    strDebitArticleNum = ( treeList.GetNodeIndex( objNode ) + 1 ).ToString();
                }
                else
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode objParentNode = objNode.ParentNode;
                    // родительским узлом является подстатья
                    strDebitArticleNum = ( ( ERP_Budget.Common.CDebitArticle )objParentNode.Tag ).ArticleNum + 
                        "." + ( treeList.GetNodeIndex( objNode ) + 1 ).ToString();
                }
                // прописываем в узле и статье расходов новый номер
                if( ( ( ERP_Budget.Common.CDebitArticle )objNode.Tag ).ArticleNum != strDebitArticleNum )
                {
                    ( ( ERP_Budget.Common.CDebitArticle )objNode.Tag ).ArticleNum = strDebitArticleNum;
                    ( ( ERP_Budget.Common.CDebitArticle )objNode.Tag ).ArticleID = treeList.GetNodeIndex( objNode );

                    this.tableLayoutPanelDebitArticleList.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                    
                    objNode.SetValue( colDebitArticleNum, ( strDebitArticleNum + " " + ( ( ERP_Budget.Common.CDebitArticle )objNode.Tag ).Name ) );
                    objNode.SetValue( colReadOnly, true );

                    this.tableLayoutPanelDebitArticleList.ResumeLayout(false);
                    ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                }

                if( objNode.HasChildren )
                {
                    // пробежим по дочерним узлам и пересчитаем их номера
                    foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNodeChild in objNode.Nodes )
                    {
                        RecalcDebitArticleNumber( objNodeChild );
                    }
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( null, "Ошибка пересчета номеров статей расходов.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// Пересчет всех номеров статей
        /// </summary>
        private void RecalcAllDebitArticleNumber()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.tableLayoutPanel1.SuspendLayout();
                this.tableLayoutPanel2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                for (System.Int32 i = 0; i < treeList.Nodes.Count; i++)
                {
                    RecalcDebitArticleNumber( treeList.Nodes[ i ] );
                }
                this.tableLayoutPanel1.ResumeLayout(false);
                this.tableLayoutPanel2.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
                SetModified(true);
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( null, "Ошибка пересчета номеров статей расходов.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        private void mitemRecalcDebitArticleNum_Click( object sender, EventArgs e )
        {
            if( treeList.FocusedNode == null ) { return; }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                RecalcDebitArticleNumber( treeList.FocusedNode );
                SetModified( true );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( null, "Ошибка пересчета номеров подстатей расходов.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;

        }
        private void mitemRecalcAllDebitArticleNum_Click( object sender, EventArgs e )
        {
            if( treeList.Nodes.Count == 0 ) { return; }
            try
            {
                RecalcAllDebitArticleNumber();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( null, "Ошибка пересчета номеров статей расходов.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        #endregion

        #region Добавить, удалить узел
        /// <summary>
        /// Добавляет узел в дерево
        /// </summary>
        /// <param name="objParentNode">родительский узел</param>
        private void AddNode( DevExpress.XtraTreeList.Nodes.TreeListNode objParentNode )
        {
            try
            {
                //прописываем значения нового узла
                if ((objParentNode == null) || (objParentNode.ParentNode == null))
                {
                    // статья расходов
                    if( m_bDREditRootDebitArticle == false ) { return; }

                    System.Int32 iFinancialYear = System.DateTime.Today.Year;
                    System.Int32 iDebitArticleCountInYear = 0;

                    // нужно правильно сосчитать номер статьи
                    // статья вернего уровня подключается к узлу с номером года
                    if( (treeList.FocusedNode != null) && ( treeList.FocusedNode.ParentNode == null ) )
                    {
                        iFinancialYear = System.Convert.ToInt32( treeList.FocusedNode.GetValue( colDebitArticleNum ) );
                        iDebitArticleCountInYear = treeList.FocusedNode.Nodes.Count;
                    }

                    m_frmDebitArticleEditor.AddDebitArticle((iDebitArticleCountInYear + 1), iFinancialYear, null, 0);

                    if (m_frmDebitArticleEditor.DialogResult == DialogResult.OK)
                    {
                        this.tableLayoutPanel1.SuspendLayout();
                        this.tableLayoutPanel2.SuspendLayout();
                        ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                        // статья верхнего уровня
                        // нужно проверить, есть ли у нас узел с нужным годом и воткнуть её именно туда
                        DevExpress.XtraTreeList.Nodes.TreeListNode objYearNode = null;
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                        {
                            if (System.Convert.ToInt32(objNode.GetValue(colDebitArticleNum)) == m_frmDebitArticleEditor.DebitArticle.FinancislYear)
                            {
                                objYearNode = objNode;
                                break;
                            }
                        }

                        if (objYearNode == null)
                        {
                            objYearNode = treeList.AppendNode(new object[] { System.Guid.Empty, 
                                null, m_frmDebitArticleEditor.DebitArticle.FinancislYear.ToString(),
                                null, false, null, null }, null);
                        }
                        else
                        {
                            if (System.Convert.ToString(objYearNode.GetValue(colDebitArticleNum)) != m_frmDebitArticleEditor.DebitArticle.FinancislYear.ToString())
                            {
                                System.Boolean bFinancislYearExists = false;
                                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeList.Nodes)
                                {
                                    if (System.Convert.ToString(objItem.GetValue(colDebitArticleNum)) == m_frmDebitArticleEditor.DebitArticle.FinancislYear.ToString())
                                    {
                                        bFinancislYearExists = true;
                                        objYearNode = objItem;
                                        break;
                                    }
                                }
                                if (bFinancislYearExists == false)
                                {
                                    objYearNode = treeList.AppendNode(new object[] { System.Guid.Empty, 
                                        null, m_frmDebitArticleEditor.DebitArticle.FinancislYear.ToString(),
                                        null, false, null, null }, null);
                                }
                            }
                        }

                        DevExpress.XtraTreeList.Nodes.TreeListNode objNewDebitArticleNode = treeList.AppendNode(new object[] { m_frmDebitArticleEditor.DebitArticle.uuidID, 
                            m_frmDebitArticleEditor.DebitArticle.ParentID, 
                            m_frmDebitArticleEditor.DebitArticle.ArticleFullName, 
                            m_frmDebitArticleEditor.DebitArticle.ArticleDescription, false, 
                            m_frmDebitArticleEditor.DebitArticle.GetBudgetDepList(), 
                            m_frmDebitArticleEditor.DebitArticle.TransprtRest }, objYearNode);

                        objNewDebitArticleNode.Tag = m_frmDebitArticleEditor.DebitArticle;

                        this.tableLayoutPanel1.ResumeLayout(false);
                        this.tableLayoutPanel2.ResumeLayout(false);
                        ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                    }
                }
                else
                {
                    // подстатья расходов
                    // сперва определим родительскую статью расходов
                    ERP_Budget.Common.CDebitArticle objDebitArticleParent = null;
                    if( objParentNode.Tag == null )
                    {
                        System.Windows.Forms.MessageBox.Show( this,
                            "Не удалось определить родительскую статью расходов!", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                        return;
                    }
                    else
                    {
                        objDebitArticleParent = ( ERP_Budget.Common.CDebitArticle )objParentNode.Tag;
                    }

                    m_frmDebitArticleEditor.AddDebitArticle(objDebitArticleParent.ArticleID, 
                        objDebitArticleParent.FinancislYear, objDebitArticleParent, (objParentNode.Nodes.Count + 1) );
                    if (m_frmDebitArticleEditor.DialogResult == DialogResult.OK)
                    {
                        this.tableLayoutPanel1.SuspendLayout();
                        this.tableLayoutPanel2.SuspendLayout();
                        ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                        DevExpress.XtraTreeList.Nodes.TreeListNode objNewDebitArticleNode = treeList.AppendNode(new object[] { m_frmDebitArticleEditor.DebitArticle.uuidID, 
                            m_frmDebitArticleEditor.DebitArticle.ParentID, 
                            m_frmDebitArticleEditor.DebitArticle.ArticleFullName, 
                            m_frmDebitArticleEditor.DebitArticle.ArticleDescription, false, 
                            m_frmDebitArticleEditor.DebitArticle.GetBudgetDepList(), 
                            m_frmDebitArticleEditor.DebitArticle.TransprtRest }, objParentNode);

                        objNewDebitArticleNode.Tag = m_frmDebitArticleEditor.DebitArticle;

                        // корректировка списка служб в родительских статьях расходов
                        CorrectBudgetDepListUp( objNewDebitArticleNode );
                        // корректировка списка служб в дочерних статьях расходов
                        CorrectBudgetDepListDown( objNewDebitArticleNode );

                        this.tableLayoutPanel1.ResumeLayout(false);
                        this.tableLayoutPanel2.ResumeLayout(false);
                        ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
                    }

                }

            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка добавления узла.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        /// <summary>
        /// Удаляет узел в дереве
        /// </summary>
        /// <param name="objNode">удаляемый узел</param>
        private void DeleteNode( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            try
            {
                if( objNode == null ) { return; }

                // проверяем, можно ли удалять узел
                ERP_Budget.Common.CDebitArticle objDebitArticle = ( ERP_Budget.Common.CDebitArticle )objNode.Tag;
                //if( objDebitArticle.IsPossibleChangeDebitArticle( this.m_objProfile, true ) == true )
                //{
                //    // узел удалять можно
                if( objDebitArticle.Remove( this.m_objProfile ) == true )
                {
                    this.tableLayoutPanel1.SuspendLayout();
                    this.tableLayoutPanel2.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                    if (objNode.ParentNode == null)
                    {
                        // статья расходов
                        treeList.Nodes.Remove( objNode );
                    }
                    else
                    {
                        // подстатья расходов
                        objNode.ParentNode.Nodes.Remove( objNode );
                    }

                    this.tableLayoutPanel1.ResumeLayout(false);
                    this.tableLayoutPanel2.ResumeLayout(false);
                    ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
                }
                //}
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка удаления узла.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }


        private void barBtnAddNode_ItemClick( object sender, DevExpress.XtraBars.ItemClickEventArgs e )
        {
            try
            {
                AddNode( null );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка добавления статьи\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void barBtnAddChildNode_ItemClick( object sender, DevExpress.XtraBars.ItemClickEventArgs e )
        {
            try
            {
                if( treeList.FocusedNode == null ) { return; }
                AddNode( treeList.FocusedNode );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка добавления статьи\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void mitemAddRoot_Click( object sender, EventArgs e )
        {
            try
            {
                AddNode( null );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка добавления статьи\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void mitemAddChild_Click( object sender, EventArgs e )
        {
            try
            {
                if( treeList.FocusedNode == null ) { return; }
                AddNode( treeList.FocusedNode );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка добавления статьи\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void DeleteNodeClick()
        {
            try
            {
                if( ( treeList.FocusedNode == null ) ) { return; }

                // китайское предупреждение по поводу удаления узла
                System.String strQuestion = "Удалить " + 
                    ( ( treeList.FocusedNode.ParentNode == null ) ? "статью " : "подстатью " ) + 
                    ( System.String )treeList.FocusedNode.GetValue( colDebitArticleNum ) + 
                    "?\nОтмена удаления будет невозможна!";
                if( System.Windows.Forms.MessageBox.Show( this, strQuestion, "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                { return; }

                // удаляем узел
                DeleteNode( treeList.FocusedNode );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка удаления статьи\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void barBtnDeleteNode_ItemClick( object sender, DevExpress.XtraBars.ItemClickEventArgs e )
        {
            try
            {
                DeleteNodeClick();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка удаления статьи\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void mitemDelete_Click( object sender, EventArgs e )
        {
            try
            {
                DeleteNodeClick();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка удаления статьи\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        #endregion

        #region Перетаскивание узлов
        /// <summary>
        /// Устанавливает внешний вид курсора "по-умолчанию"
        /// </summary>
        private void SetDefaultCursor()
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка установки курсора\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void SetDragCursor( DragDropEffects e )
        {
            try
            {
                switch( e )
                {
                    case DragDropEffects.Move:
                    {
                        this.Cursor = Cursors.Default;
                        break;
                    }
                    case DragDropEffects.Copy:
                    {
                        this.Cursor = Cursors.Default;
                        break;
                    }
                    case DragDropEffects.None:
                    {
                        this.Cursor = Cursors.No;
                        break;
                    }
                    default:
                    {
                        this.Cursor = Cursors.Default;
                        break;
                    }
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка 'SetDragCursor'\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// Возвращает перетаскиваемый узел
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DevExpress.XtraTreeList.Nodes.TreeListNode GetDragNode( System.Windows.Forms.IDataObject data )
        {
            return data.GetData( typeof( DevExpress.XtraTreeList.Nodes.TreeListNode ) ) as DevExpress.XtraTreeList.Nodes.TreeListNode;
        }
        /// <summary>
        /// Завершение перетаскивания (отпустили мышку)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_DragDrop( object sender, DragEventArgs e )
        {
            try
            {
                DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo( treeList.PointToClient( new Point( e.X, e.Y ) ) );
                // узел, который перетаскивали
                DevExpress.XtraTreeList.Nodes.TreeListNode draggedNode = 
                    ( DevExpress.XtraTreeList.Nodes.TreeListNode )e.Data.GetData( typeof( DevExpress.XtraTreeList.Nodes.TreeListNode ) );
                if( ( draggedNode != null ) && ( draggedNode.ParentNode != null ) && ( ( e.KeyState & 4 ) != 4 ) )
                {
                    //узел, над которым отпустили мышку
                    DevExpress.XtraTreeList.Nodes.TreeListNode node = hi.Node;
                    // узел из которого выдернут перетаскиваемый узел 
                    this.draggedNodeParent = draggedNode.ParentNode;
                    if( node != null )
                    {
                        // изменяем указатель на родителя у перетащенного узла
                        System.Guid uuidDebitArticleID = ( ( ERP_Budget.Common.CDebitArticle )node.Tag ).uuidID;
                        ( ( ERP_Budget.Common.CDebitArticle )draggedNode.Tag ).ParentID = uuidDebitArticleID;
                        ( ( ERP_Budget.Common.CDebitArticle )draggedNode.Tag ).ReadOnly = true;

                        this.tableLayoutPanel1.SuspendLayout();
                        this.tableLayoutPanel2.SuspendLayout();
                        ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                        
                        draggedNode.SetValue(colParentGuid_ID, uuidDebitArticleID);
                        draggedNode.SetValue( colReadOnly, true );

                        this.tableLayoutPanel1.ResumeLayout(false);
                        this.tableLayoutPanel2.ResumeLayout(false);
                        ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                        // делаем пометку, что изменения произошли
                        SetModified( true );
                    }
                }
                SetDefaultCursor();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка treeList_DragDrop\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        /// <summary>
        /// Объект перетаскивается над treeList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_DragOver( object sender, DragEventArgs e )
        {
            try
            {
                DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo( treeList.PointToClient( new Point( e.X, e.Y ) ) );
                // узел под курсором
                DevExpress.XtraTreeList.Nodes.TreeListNode node = hi.Node;
                // перетаскиваемый узел
                DevExpress.XtraTreeList.Nodes.TreeListNode draggedNode = GetDragNode( e.Data );

                if( ( node == null ) || ( node.Tag == null ) ||                     
                    ( draggedNode == null ) || ( draggedNode.ParentNode == null ) || ( draggedNode.Tag == null ) )
                {
                    e.Effect= DragDropEffects.None;
                }
                else
                {
                    // мы тащим подстатью и под курсором тоже подстатья
                    if( ( node != null ) && ( node.Tag != null ) )
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }

                SetDragCursor( e.Effect );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка 'treeList_DragOver'\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        /// <summary>
        /// Перетаскиваемый объект вышел за границы treeList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_DragLeave( object sender, EventArgs e )
        {
            try
            {
                SetDefaultCursor();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка 'treeList_DragLeave'\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// Узел перетащен
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_AfterDragNode( object sender, DevExpress.XtraTreeList.NodeEventArgs e )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if( e.Node == null ) { return; }

                this.tableLayoutPanel1.SuspendLayout();
                this.tableLayoutPanel2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                
                // пересчет номеров статей у перетащенного узла
                RecalcDebitArticleNumber( e.Node.ParentNode );
                // корректировка списка служб в дочерних статьях расходов
                CorrectBudgetDepListDown( e.Node.ParentNode );
                if( this.draggedNodeParent == null ) { return; }
                // пересчет номеров статей в ветке, откуда перетаскивали узел
                RecalcDebitArticleNumber( this.draggedNodeParent );
                this.draggedNodeParent = null;

                this.tableLayoutPanel1.ResumeLayout(false);
                this.tableLayoutPanel2.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                SetModified(true);

            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка \"treeList_AfterDragNode\"\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }

        private void treeList_GiveFeedback( object sender, GiveFeedbackEventArgs e )
        {
            try
            {
                e.UseDefaultCursors = false;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка 'treeList_GiveFeedback'\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }


        #endregion

        #region Кнопки управления деревом

        /// <summary>
        /// перемещение узла дерева меню влево
        /// </summary>
        private void OnLeft()
        {
            try
            {
                // текущий узел
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( ( objFocusedNode == null ) || ( objFocusedNode.Tag == null ) ) { return; }
                // родительский узел
                if( ( objFocusedNode.ParentNode == null ) || 
                    ( objFocusedNode.ParentNode.ParentNode == null ) || 
                    ( ( objFocusedNode.ParentNode.ParentNode.Tag == null ) ) ) { return; }
                System.Guid uuidDebitArticleID = ( ( ERP_Budget.Common.CDebitArticle )objFocusedNode.ParentNode.ParentNode.Tag ).uuidID;

                // перемещаем узел

                this.tableLayoutPanel1.SuspendLayout();
                this.tableLayoutPanel2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                treeList.MoveNode(objFocusedNode, objFocusedNode.ParentNode.ParentNode);
                ( ( ERP_Budget.Common.CDebitArticle )objFocusedNode.Tag ).ArticleID = treeList.GetNodeIndex( objFocusedNode );
                objFocusedNode.SetValue( colReadOnly, true );

                // изменяем указатель на родителя у перетаскиваемого узла
                ( ( ERP_Budget.Common.CDebitArticle )objFocusedNode.Tag ).ParentID = uuidDebitArticleID;
                objFocusedNode.SetValue( colParentGuid_ID, uuidDebitArticleID );

                // попробуем пересчитать номера подстатей
                RecalcDebitArticleNumber( objFocusedNode.ParentNode );

                this.tableLayoutPanel1.ResumeLayout(false);
                this.tableLayoutPanel2.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                // делаем пометку об изменениях в дереве
                SetModified( true );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnLeft\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void btnLeft_Click( object sender, EventArgs e )
        {
            try
            {
                OnLeft();
                OnSelectItem();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка при перемещении узла влево\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        /// <summary>
        /// перемещение узла дерева меню вправо
        /// </summary>
        private void OnRight()
        {
            try
            {
                // текущий узел
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( ( objFocusedNode == null ) || ( objFocusedNode.Tag == null ) ) { return; }
                // предыдущий узел
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevFocusedNode = objFocusedNode.PrevNode;
                // предыдущий узел должен иметь родителя  и не пустой тэг
                if( ( objPrevFocusedNode == null ) || ( objFocusedNode.ParentNode == null ) || ( objPrevFocusedNode.Tag == null ) ) { return; }
                System.Guid uuidPrevNodeDebitArticleID = ( ( ERP_Budget.Common.CDebitArticle )objPrevFocusedNode.Tag ).uuidID;

                // перемещаем узел
                this.tableLayoutPanel1.SuspendLayout();
                this.tableLayoutPanel2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                treeList.MoveNode(objFocusedNode, objPrevFocusedNode);
                ( ( ERP_Budget.Common.CDebitArticle )objFocusedNode.Tag ).ArticleID = treeList.GetNodeIndex( objFocusedNode );
                objFocusedNode.SetValue( colReadOnly, true );

                // изменяем указатель на родителя у перетаскиваемого узла
                ( ( ERP_Budget.Common.CDebitArticle )objFocusedNode.Tag ).ParentID = uuidPrevNodeDebitArticleID;
                objFocusedNode.SetValue( colParentGuid_ID, uuidPrevNodeDebitArticleID );

                // попробуем пересчитать номера подстатей
                RecalcDebitArticleNumber( objFocusedNode.ParentNode );

                this.tableLayoutPanel1.ResumeLayout(false);
                this.tableLayoutPanel2.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                // делаем пометку об изменениях в дереве
                SetModified( true );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка перемещения узла вправо.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void btnRight_Click( object sender, EventArgs e )
        {
            try
            {
                OnRight();
                OnSelectItem();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка при перемещении узла вправо\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        /// <summary>
        /// перемещение узла дерева меню вниз
        /// </summary>
        private void OnDown()
        {
            try
            {
                // выделенный узел
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode == null ) { return; }
                // следующий узел
                DevExpress.XtraTreeList.Nodes.TreeListNode objNexFocusedNode = objFocusedNode.NextNode;
                if( objNexFocusedNode == null ) { return; }
                
                // перемещаем узел
                this.tableLayoutPanel1.SuspendLayout();
                this.tableLayoutPanel2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                
                System.Int32 iFocusedNodeIndx = treeList.GetNodeIndex(objFocusedNode);
                System.Int32 iNexFocusedNodeIndx = treeList.GetNodeIndex( objNexFocusedNode );
                treeList.SetNodeIndex( objFocusedNode, iNexFocusedNodeIndx );
                ( ( ERP_Budget.Common.CDebitArticle )objFocusedNode.Tag ).ArticleID = iNexFocusedNodeIndx;
                treeList.SetNodeIndex( objNexFocusedNode, iFocusedNodeIndx );
                ( ( ERP_Budget.Common.CDebitArticle )objNexFocusedNode.Tag ).ArticleID = iFocusedNodeIndx;

                objFocusedNode.SetValue( colReadOnly, true );
                objNexFocusedNode.SetValue( colReadOnly, true );

                // попробуем пересчитать номера подстатей
                RecalcDebitArticleNumber( objNexFocusedNode );
                RecalcDebitArticleNumber( objFocusedNode );

                this.tableLayoutPanel1.ResumeLayout(false);
                this.tableLayoutPanel2.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                // делаем пометку об изменениях в дереве
                SetModified( true );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnDown\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void btnDown_Click( object sender, EventArgs e )
        {
            try
            {
                OnDown();
                OnSelectItem();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка при перемещении узла вниз\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        /// <summary>
        /// перемещение узла дерева меню вверх
        /// </summary>
        private void OnUp()
        {
            try
            {
                // выделенный узел
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( ( objFocusedNode == null ) || ( objFocusedNode.Tag == null ) ) { return; }
                // предыдущий узел
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevFocusedNode = objFocusedNode.PrevNode;
                if( ( objPrevFocusedNode == null ) || ( objPrevFocusedNode.Tag == null ) ) { return; }

                // перемещаем узел
                this.tableLayoutPanel1.SuspendLayout();
                this.tableLayoutPanel2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                
                System.Int32 iFocusedNodeIndx = treeList.GetNodeIndex(objFocusedNode);
                System.Int32 iPrevFocusedNodeIndx = treeList.GetNodeIndex( objPrevFocusedNode );

                treeList.SetNodeIndex( objFocusedNode, iPrevFocusedNodeIndx );
                ( ( ERP_Budget.Common.CDebitArticle )objFocusedNode.Tag ).ArticleID = iPrevFocusedNodeIndx;

                treeList.SetNodeIndex( objPrevFocusedNode, iFocusedNodeIndx );
                ( ( ERP_Budget.Common.CDebitArticle )objPrevFocusedNode.Tag ).ArticleID = iFocusedNodeIndx;

                objFocusedNode.SetValue( colReadOnly, true );
                objPrevFocusedNode.SetValue( colReadOnly, true );

                // попробуем пересчитать номера подстатей
                RecalcDebitArticleNumber( objFocusedNode );
                RecalcDebitArticleNumber( objPrevFocusedNode );

                this.tableLayoutPanel1.ResumeLayout(false);
                this.tableLayoutPanel2.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                // делаем пометку об изменениях в дереве
                SetModified( true );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnUp\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void btnUp_Click( object sender, EventArgs e )
        {
            try
            {
                OnUp();
                OnSelectItem();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка при перемещении узла вверх\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// выделение узла дерева меню
        /// </summary>
        private void OnSelectItem()
        {
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                    // узел выделен
                    // разберемся с клавишами управления
                    System.Boolean bIsSelectYear = false;
                    System.Boolean bIsSelectSubDebitArticle = false;
                    System.Boolean bIsSelectSubDebitArticleFirstAfterRoot = false;
                    System.Boolean bIsSelectSubDebitArticleFirstInLine = false;
                    if (objFocusedNode.ParentNode == null) 
                    {
                        // год
                        bIsSelectYear = true;
                        bIsSelectSubDebitArticle = false; 
                    } 
                    else
                    {
                        if (objFocusedNode.ParentNode.ParentNode == null) { bIsSelectSubDebitArticle = false; } // статья
                        else 
                        {
                            //подстатья
                            bIsSelectSubDebitArticle = true;
                            bIsSelectSubDebitArticleFirstAfterRoot = (objFocusedNode.ParentNode.ParentNode.ParentNode == null);
                            bIsSelectSubDebitArticleFirstInLine = (objFocusedNode.PrevNode != null);
                        } 
                    }
                    if ((objFocusedNode == null) || (bIsSelectYear == true))
                    {
                        btnLeft.Enabled = false;
                        btnRight.Enabled = false;
                        btnUp.Enabled = false;
                        btnDown.Enabled = false;
                        btnDrop.Enabled = false;
                    }
                    else
                    {
                        if (bIsSelectSubDebitArticle == false)
                        {
                            btnLeft.Enabled = false;
                            btnRight.Enabled = false;
                            btnDrop.Enabled = (m_bDREditRootDebitArticle && (bIsModified() == false));
                            // статья
                            if (m_bDREditRootDebitArticle == true)
                            {
                                btnUp.Enabled = (objFocusedNode.PrevNode != null);
                                btnDown.Enabled = (objFocusedNode.NextNode != null);
                            }
                            else
                            {
                                btnUp.Enabled = false;
                                btnDown.Enabled = false;
                            }
                        }
                        else
                        {
                            // подстатья
                            btnLeft.Enabled = ( bIsSelectSubDebitArticleFirstAfterRoot == false );
                            btnRight.Enabled = ( bIsSelectSubDebitArticleFirstInLine == false );
                            btnDrop.Enabled = (bIsModified() == false);
                            btnUp.Enabled = (objFocusedNode.PrevNode != null);
                            btnDown.Enabled = (objFocusedNode.NextNode != null);
                        }

                    barBtnDeleteNode.Enabled = btnDrop.Enabled;
                    barBtnAddChildNode.Enabled = true;
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnSelectItem\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void LoadBudgetDepListForDebitArticle( )
        {
            try
            {
                if ((treeList.FocusedNode == null) || (treeList.FocusedNode.Tag == null)) { return; }


                this.tableLayoutPanelDebitArticleList.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListBudgetDept)).BeginInit();

                treeListBudgetDept.Nodes.Clear();

                CDebitArticle DebitArticleParent = GetSelectedDebitArticleParent();

                if ((DebitArticleParent == null) || (DebitArticleParent.BudgetDepList == null)) { return; }

                foreach (CBudgetDep objBudgetDep in DebitArticleParent.BudgetDepList)
                {
                    treeListBudgetDept.AppendNode(new object[] { objBudgetDep.Name, 
                        ((objBudgetDep.BudgetExpenseType == null) ? "" : objBudgetDep.BudgetExpenseType.Name), 
                        ((objBudgetDep.BudgetProject == null) ? "" : objBudgetDep.BudgetProject.Name) }, null);
                }

                lblDebitArticleNum.Text = ((CDebitArticle)treeList.FocusedNode.Tag).ArticleFullName;
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, String.Format("Ошибка LoadExpenseTypeListForBudgetDep.\n\nТекст ошибки:\n{0}", f.Message), "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.tableLayoutPanelDebitArticleList.ResumeLayout(false);
                this.tableLayoutPanelDebitArticleList.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListBudgetDept)).EndInit();

            }
            return;
        }


        private void treeList_MouseClick( object sender, MouseEventArgs e )
        {
            try
            {
                if( e.Button == MouseButtons.Right )
                {
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo( new Point( e.X, e.Y ) );
                    if( hi == null ) { return; }
                    if( hi.Node == null ) { return; }
                    // выделяем узел
                    hi.Node.TreeList.FocusedNode = hi.Node;
                    // запускаем всплывающее меню
                    mitemDelete.Enabled = btnDrop.Enabled;
                    mitemRecalcAllDebitArticleNum.Enabled = ( hi.Node.TreeList.FocusedNode != null );
                    mitemRecalcDebitArticleNum.Enabled = mitemRecalcAllDebitArticleNum.Enabled;
                    contextMenuStrip.Show( treeList, new Point( e.X, e.Y ) );
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка 'treeList_MouseClick'\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void treeList_KeyDown( object sender, KeyEventArgs e )
        {
            try
            {
                if( treeList.Nodes.Count == 0 ) { return; }
                if( treeList.FocusedNode == null ) { return; }
                if( e.Control )
                {
                    switch( e.KeyCode )
                    {
                        // Ctrl - влево
                        case System.Windows.Forms.Keys.Left:
                        if( btnLeft.Enabled )
                        {
                            OnLeft();
                            OnSelectItem();
                        }
                        break;
                        // Ctrl - вправо
                        case System.Windows.Forms.Keys.Right:
                        if( btnRight.Enabled )
                        {
                            OnRight();
                            OnSelectItem();
                        }
                        break;
                        // Ctrl - вверх
                        case System.Windows.Forms.Keys.Up:
                        if( btnUp.Enabled )
                        {
                            OnUp();
                            OnSelectItem();
                        }
                        break;
                        // Ctrl - вниз
                        case System.Windows.Forms.Keys.Down:
                        if( btnDown.Enabled )
                        {
                            OnDown();
                            OnSelectItem();
                        }
                        break;
                        // Ctrl - Delete
                        case System.Windows.Forms.Keys.Delete:
                        if( btnDrop.Enabled )
                        {
                            // китайское предупреждение по поводу удаления узла
                            System.String strQuestion = "Удалить " + ( ( treeList.FocusedNode.ParentNode == null ) ? "статью" : "подстатью" ) + "?";
                            if( System.Windows.Forms.MessageBox.Show( this, strQuestion, "Подтверждение",
                                System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                            { break; }
                            // удаляем узел
                            DeleteNode( treeList.FocusedNode );
                            OnSelectItem();
                        }
                        break;

                        default:
                        break;
                    }
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка обработки нажатия клавиши\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }



        #endregion

        #region Сохранение, отмена изменений в БД
        /// <summary>
        /// Отмена внесенных изменений
        /// </summary>
        private void CancelChanges()
        {
            try
            {
                if( bIsModified() )
                {
                    // если мы утверждаем, что изменения отменяются, 
                    // то в каждом узле нужно снять пометку, что он изменен
                    foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objTreeNode in treeList.Nodes )
                    {
                        DiscardChangesNodes( objTreeNode );
                    }

                    RefreshDebitArticlesList();
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка отмены внесенных изменений.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void btnCancel_Click( object sender, EventArgs e )
        {
            try
            {
                if( System.Windows.Forms.MessageBox.Show( this,
                   "Отменить все сделанные изменения?", "Подтверждение",
                   System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                   System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.Yes )
                {
                    CancelChanges();
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                   "Ошибка отмены внесенных изменений.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// Заполняет список измененных статей расходов для последующего сохранения в БД
        /// </summary>
        /// <param name="objList">список объектов "статья расходов"</param>
        /// <param name="objTreeNode">узел дерева, для которго производится поиск дочерних узлов, подвергнувшихся изменениям </param>
        private void FillChangesNodesList( List<ERP_Budget.Common.CDebitArticle> objList,
            DevExpress.XtraTreeList.Nodes.TreeListNode objTreeNode )
        {
            try
            {
                if( objTreeNode == null ) { return; }
                if( objList == null ) { return; }

                if( ( objTreeNode.GetValue( colReadOnly ) != null ) &&
                    (objTreeNode.GetValue(colReadOnly).GetType().Name == "Boolean") &&
                    ( ( System.Boolean )objTreeNode.GetValue( colReadOnly ) == true ) )
                {
                    objList.Add( ( ERP_Budget.Common.CDebitArticle )objTreeNode.Tag );
                }

                if( objTreeNode.HasChildren )
                {
                    foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objTreeNodeChild in objTreeNode.Nodes )
                    {
                        FillChangesNodesList( objList, objTreeNodeChild );
                    }
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка заполнения списка измененных статей расходов.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        /// <summary>
        /// Сохраняет изменения в списке статей расходов в БД
        /// </summary>
        /// <returns></returns>
        private System.Boolean SaveChangesToDB(ref System.String strErr)
        {
            System.Boolean bRes = false;
            try
            {
                if( treeList.Nodes.Count == 0 )
                {
                    strErr += ("\nСписок статей пуст!");
                    return bRes;
                }

                //сформируем список статей расходов для сохранения в БД
                List<ERP_Budget.Common.CDebitArticle> objDebitArticleChangeList = new List<ERP_Budget.Common.CDebitArticle>();
                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objTreeNodeParent in treeList.Nodes )
                {
                    FillChangesNodesList( objDebitArticleChangeList, objTreeNodeParent );
                }
                if( objDebitArticleChangeList.Count == 0 )
                {
                    strErr += ("\nПрограмме не удалось определить список измененных статей.");
                    return bRes;
                }

                // список заполнен - попробуем сохранить изменения в БД
                bRes = ERP_Budget.Common.CDebitArticle.UpdateList( m_objProfile, objDebitArticleChangeList, false, ref strErr );
                if( bRes == true )
                {
                    // делаем пометку об изменениях в дереве
                    SetModified( false );
                }
                objDebitArticleChangeList = null;
            }
            catch( System.Exception f )
            {
                strErr += String.Format("\nОшибка сохранения изменений в списке статей в БД.\nТекст ошибки: {0}", f.Message);
            }
            return bRes;
        }

        private void btnSave_Click( object sender, EventArgs e )
        {
            Cursor = Cursors.WaitCursor;
            System.String strErr = "";
            try
            {
                if (SaveChangesToDB( ref strErr ) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Ошибка сохранения",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка сохранения изменений.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }

        #endregion

        #region Печать

        private void barBtnPrint_ItemClick( object sender, DevExpress.XtraBars.ItemClickEventArgs e )
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if( DevExpress.XtraPrinting.PrintHelper.IsPrintingAvailable )
                    DevExpress.XtraPrinting.PrintHelper.ShowPreview( treeList );
                else
                    MessageBox.Show( "XtraPrinting Library is not found...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information );
                Cursor.Current = Cursors.Default;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка печати\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        #endregion

        #region Открытие формы
        private void frmDebitArticle_Load( object sender, EventArgs e )
        {
            try
            {
                // загружаем список статей расходов
                System.String strErr = "";
                
                StartThreadComboBoxForEditor();

                LoadDebitArticles(ref strErr);

                if (strErr != "")
                {
                    System.Windows.Forms.MessageBox.Show(this, strErr, "Ошибка",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                
                // проверка динамических прав
                SetAccessForDynamicRights();

                if( treeList.Nodes.Count > 0 )
                {
                    treeList.BestFitColumns();
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка открытия формы\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        #endregion

        #region События дерева статей расходов
        private void treeList_AfterFocusNode( object sender, DevExpress.XtraTreeList.NodeEventArgs e )
        {
            try
            {
                if( treeList.Nodes.Count == 0 ) { return; }
                if( treeList.FocusedNode == null ) { return; }

                LoadBudgetDepListForDebitArticle();

                OnSelectItem();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка treeList_AfterFocusNode\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void treeList_CustomDrawNodeCell( object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e )
        {
            try
            {
                if( ( e.Node == null || e.Column == null ) ) { return; }

                System.Boolean bIsSelectYear = false;
                System.Boolean bIsSelectSubDebitArticle = false;
                System.Boolean bIsSelectSubDebitArticleFirstAfterRoot = false;
                System.Boolean bIsSelectSubDebitArticleFirstInLine = false;
                if (e.Node.ParentNode == null)
                {
                    // год
                    bIsSelectYear = true;
                    bIsSelectSubDebitArticle = false;
                }
                else
                {
                    if (e.Node.ParentNode.ParentNode == null) { bIsSelectSubDebitArticle = false; } // статья
                    else
                    {
                        //подстатья
                        bIsSelectSubDebitArticle = true;
                        bIsSelectSubDebitArticleFirstAfterRoot = (e.Node.ParentNode.ParentNode == null);
                        bIsSelectSubDebitArticleFirstInLine = (e.Node.PrevNode != null);
                    }
                }

                System.Drawing.Brush brushBckgrnd = null; // цвет фона
                System.Drawing.Brush brushFont = null; // цвет текста в ячейке
                System.String strText = ""; // текст в ячейке
                System.Drawing.Color litegrayColor = Color.FromArgb( 255, 250, 250, 250 );
                System.Drawing.Color yelowColor = Color.FromArgb( 255, 255, 250 );

                System.Boolean bNodeFocused = ( e.Node == treeList.FocusedNode );
                System.Boolean bColumnFocused = ( e.Column == treeList.FocusedColumn );

                // цвет фона
                if( bNodeFocused )
                {
                    if( bColumnFocused )
                    {
                        // узел и столбец в фокусе
                        brushBckgrnd = e.Appearance.GetBorderBrush( e.Cache );
                    }
                    else
                    {
                        brushBckgrnd = e.Appearance.GetBackBrush( e.Cache );
                    }
                }
                else
                {
                    brushBckgrnd = new System.Drawing.SolidBrush( yelowColor );
                }
                // цвет текста
                if( bNodeFocused )
                {
                    if( bColumnFocused )
                    {
                        brushFont = System.Drawing.Brushes.Black;
                    }
                    else
                    {
                        brushFont = System.Drawing.Brushes.White;
                    }
                }
                else
                {
                    brushFont = System.Drawing.Brushes.Black;
                }
                // текст в ячейке
                strText = e.CellText;

                // собственно рисование
                // фон
                if( brushBckgrnd != null )
                {
                    e.Graphics.FillRectangle( brushBckgrnd, e.Bounds );
                }
                if( strText != "" )
                {
                    if( e.Column == colDebitArticleNum )
                    {
                        if ((bIsSelectYear == false) && (bIsSelectSubDebitArticle == false) )
                        {
                            System.Drawing.Rectangle ImageRect = new Rectangle( e.Bounds.Location, e.Bounds.Size );
                            ImageRect.Y += 2;
                            ImageRect.X += 2;
                            ImageRect.Width = 16;
                            ImageRect.Height = 16;
                            e.Graphics.DrawImage( DebitArticle.Properties.Resources.TranspRest, ImageRect );

                            // рисуем текст в рамке
                            System.Drawing.RectangleF StringRect = new RectangleF( e.Bounds.Location, e.Bounds.Size );
                            StringRect.Width -= ( 2 + 16 );
                            StringRect.Y += 1;
                            StringRect.X += ( 3 + 16 );
                            StringRect.Height -= 1;
                            e.Graphics.DrawString( strText, e.Appearance.Font, brushFont, StringRect, e.Appearance.GetStringFormat() );

                        }
                        else
                        {
                            e.Appearance.DrawString( e.Cache, strText, new Rectangle( e.Bounds.Location.X + 2,
                                e.Bounds.Location.Y, e.Bounds.Size.Width - 2, e.Bounds.Size.Height ), brushFont );
                        }
                    }
                    else
                    {
                        e.Appearance.DrawString( e.Cache, strText, new Rectangle( e.Bounds.Location.X + 2,
                            e.Bounds.Location.Y, e.Bounds.Size.Width - 2, e.Bounds.Size.Height ), brushFont );

                    }
                }
                // последний штрих, устанавливаем признак того, что мы рисуем сами в True
                e.Handled = true;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( null, "Ошибка отрисовки картинок в узлах\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void treeList_MouseDoubleClick( object sender, MouseEventArgs e )
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                // попробуем определить, что же у нас под мышкой
                DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo(new Point(e.X, e.Y));
                if (hi == null) { return; }
                if ((hi.Node == null) || (hi.Node.Tag == null)) { return; }

                // узел есть и в тэге тоже что-то есть
                m_frmDebitArticleEditor.EditDebitArticle((ERP_Budget.Common.CDebitArticle)hi.Node.Tag,
                    GetSelectedDebitArticleParent());

                if (m_frmDebitArticleEditor.DialogResult == DialogResult.OK)
                {
                    hi.Node.Tag = m_frmDebitArticleEditor.DebitArticle;
                    hi.Node.SetValue(colDebitArticleNum, m_frmDebitArticleEditor.DebitArticle.ArticleFullName);
                    hi.Node.SetValue(colAccountPlan, ((m_frmDebitArticleEditor.DebitArticle.AccountPlan == null) ? "" : m_frmDebitArticleEditor.DebitArticle.AccountPlan.FullName));
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка редактирования статьи расходов.\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }

        /// <summary>
        /// Возвращает ссылку на родительскую статью бюджета
        /// </summary>
        /// <returns></returns>
        private CDebitArticle GetSelectedDebitArticleParent()
        {
            CDebitArticle objRet = null;
            try
            {
                if( (treeList.FocusedNode == null) && (treeList.FocusedNode.Tag == null) ) { return objRet; }

                CDebitArticle objItem = (CDebitArticle)treeList.FocusedNode.Tag;

                if (objItem.ParentID.Equals(System.Guid.Empty))
                {
                    objRet = objItem;
                }
                else
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode objNodeCurrent = treeList.FocusedNode;
                    objItem = (CDebitArticle)objNodeCurrent.Tag;

                    while( objItem.ParentID.Equals( System.Guid.Empty ) == false )
                    {
                        objNodeCurrent = objNodeCurrent.ParentNode;

                        if( (objNodeCurrent == null) || (objNodeCurrent.Tag == null) ) 
                        { 
                            break;
                        }
                        else
                        {
                            objItem = (CDebitArticle)objNodeCurrent.Tag;
                        }
                    }

                    objRet = objItem;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("GetSelectedDebitArticleParent.\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return objRet;
        }

        #endregion

        #region Корректировка списка служб у статьи расходов

        /// <summary>
        /// Рекурсивный метод, корректирующий список служб у родительских статей
        /// </summary>
        /// <param name="objNode">текущий узел со статьей расходов</param>
        private void CorrectBudgetDepListUp( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            if( ( objNode == null ) || ( objNode.Tag == null ) || 
                ( objNode.ParentNode == null ) || ( objNode.ParentNode.Tag == null ) ) { return; }
            try
            {
                ERP_Budget.Common.CDebitArticle objDebitArticle = ( ERP_Budget.Common.CDebitArticle )objNode.Tag;
                if( objDebitArticle.BudgetDepList.Count > 0 )
                {
                    ERP_Budget.Common.CDebitArticle objDebitArticleParent = ( ERP_Budget.Common.CDebitArticle )objNode.ParentNode.Tag;
                    // если в родительской статье нет какого-нибудь подразделения из дочерней статьи,
                    // то мы его добавляем в родительскую статью
                    foreach( ERP_Budget.Common.CBudgetDep objBudgetDep in objDebitArticle.BudgetDepList )
                    {
                        System.Boolean bFind = false;
                        foreach( ERP_Budget.Common.CBudgetDep objBudgetDepParent in objDebitArticleParent.BudgetDepList )
                        {
                            if( objBudgetDepParent.uuidID.CompareTo( objBudgetDep.uuidID ) == 0 )
                            {
                                bFind = true;
                                break;
                            }
                        }

                        if( bFind == false )
                        {
                            // добавляем бюджетную службу
                            objDebitArticleParent.BudgetDepList.Add( new ERP_Budget.Common.CBudgetDep( objBudgetDep.uuidID, objBudgetDep.Name ) );
                            objNode.ParentNode.SetValue( colReadOnly, true );
                            SetModified( true );
                        }
                    }
                    // теперь провернем все тоже самое для дочернего узла
                    CorrectBudgetDepListUp( objNode.ParentNode );
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка изменения списка бюджетных служб.\nСтатья: " + 
                    ( ( ERP_Budget.Common.CDebitArticle )objNode.Tag ).ArticleFullName + 
                    "\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        ///Корректирует список служб у дочерних статей
        /// </summary>
        /// <param name="objNode">текущий узел со статьей расходов</param>
        private void CorrectBudgetDepListDown( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            if( ( objNode == null ) || ( objNode.Tag == null ) || 
                ( objNode.HasChildren == false ) ) { return; }
            try
            {
                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNodeChild in objNode.Nodes )
                {
                    CorrectBudgetDepListChildNode( objNodeChild );
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка изменения списка бюджетных служб.\nСтатья: " + 
                    ( ( ERP_Budget.Common.CDebitArticle )objNode.Tag ).ArticleFullName + 
                    "\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// Рекурсивный метод, корректирующий список служб у дочерних статей
        /// </summary>
        /// <param name="objNode">текущий узел со статьей расходов</param>
        private void CorrectBudgetDepListChildNode( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            if( ( objNode == null ) || ( objNode.Tag == null ) || 
                ( objNode.ParentNode == null ) || ( objNode.ParentNode.Tag == null ) ) { return; }
            try
            {
                ERP_Budget.Common.CDebitArticle objDebitArticle = ( ERP_Budget.Common.CDebitArticle )objNode.Tag;
                // очищаем список бюджетных служб
                objDebitArticle.BudgetDepList.Clear();

                // родительская статья расходов
                ERP_Budget.Common.CDebitArticle objDebitArticleParent = ( ERP_Budget.Common.CDebitArticle )objNode.ParentNode.Tag;

                foreach( ERP_Budget.Common.CBudgetDep objBudgetDepParent in objDebitArticleParent.BudgetDepList )
                {
                    // добавляем бюджетную службу
                    objDebitArticle.BudgetDepList.Add( new ERP_Budget.Common.CBudgetDep( objBudgetDepParent.uuidID, objBudgetDepParent.Name ) );
                    objNode.SetValue( colReadOnly, true );
                    SetModified( true );
                }
                if( objNode.HasChildren )
                {
                    foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNodeChild in objNode.Nodes )
                    {
                        CorrectBudgetDepListChildNode( objNodeChild );
                    }
                }

            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка изменения списка бюджетных служб.\nСтатья: " + 
                    ( ( ERP_Budget.Common.CDebitArticle )objNode.Tag ).ArticleFullName + 
                    "\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        #endregion

        #region Импорт статей расходов
        /// <summary>
        /// импорт данных в справочник статей расходов
        /// </summary>
        private void ImportDebitArticleList()
        {
            try
            {
                if (m_frmImportDebitArticleList != null)
                {
                    m_frmImportDebitArticleList.OpenForImportDataInDebitArticle( null, m_strXLSImportFilePath, m_iXLSSheetImport, m_SheetList);
                    //PlanImportEditor.ShowDialog();

                    DialogResult dlgRes = m_frmImportDebitArticleList.DialogResult;

                    if (dlgRes == System.Windows.Forms.DialogResult.OK)
                    {
                        RefreshDebitArticlesList();
                    }

                    m_strXLSImportFilePath = m_frmImportDebitArticleList.FileFullName;
                    m_iXLSSheetImport = m_frmImportDebitArticleList.SelectedSheetId;
                    m_SheetList = m_frmImportDebitArticleList.SheetList;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("ImportDebitArticleList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void mitemImportDebitArticleList_Click(object sender, EventArgs e)
        {
            ImportDebitArticleList();
        }

        #endregion


    }

}