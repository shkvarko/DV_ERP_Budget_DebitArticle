using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using ERP_Budget.Common;
using UniXP.Common;

namespace DebitArticle
{
    public partial class frmDebitArticleEditor : DevExpress.XtraEditors.XtraForm
    {
        #region Переменные, свойства, константы
        private CProfile m_objProfile;
        private List<CBudgetDep> m_objBudgetDepList;
        private List<CBudgetExpenseType> m_objBudgetExpenseTypeList;
        private List<CBudgetProject> m_objBudgetProjectList;
        private List<CAccountPlan> m_objAccountPlanList;
        private ERP_Budget.Common.CDebitArticle m_objDebitArticle;
        public ERP_Budget.Common.CDebitArticle DebitArticle
        {
            get { return m_objDebitArticle; }
        }

        public CDebitArticle DebitArticleParent { get; set; }

        private System.Boolean m_bNewDebitArticle;
        private System.Boolean m_bIsCanModifWarningParam;

        private const System.Int32 iWarningPanelHeight = 62;
        private const System.String m_strDebitArticleParentCaption = "Статья расходов";
        private const System.String m_strDebitArticleCaption = "Подcтатья расходов";
        #endregion

        #region Конструктор
        public frmDebitArticleEditor( UniXP.Common.CProfile objProfile )
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objDebitArticle = null;
            DebitArticleParent = null;
            m_objBudgetDepList = null;
            m_objBudgetExpenseTypeList = null;
            m_objBudgetProjectList = null;
            m_objAccountPlanList = null;
            m_bCancelEvents = false;
            m_bNewDebitArticle = false;
            m_bIsCanModifWarningParam = true;
            dateEditBegin.DateTime = new DateTime(System.DateTime.Today.Year, 1, 1);
            dateEditEnd.DateTime = new DateTime(System.DateTime.Today.Year, 12, 31);
        }
        #endregion

        #region Редактирование статьи расходов

        public void LoadComboBox(List<CBudgetDep> objBudgetDepList, List<CBudgetExpenseType> objBudgetExpenseTypeList,
           List<CBudgetProject> objBudgetProjectList, List<CAccountPlan> objAccountPlanList)
        {
            try
            {
                m_objBudgetDepList = objBudgetDepList;
                m_objBudgetExpenseTypeList = objBudgetExpenseTypeList;
                m_objBudgetProjectList = objBudgetProjectList;
                m_objAccountPlanList = objAccountPlanList;

                chklstBudgetDep.Items.Clear();
                checkedListBoxExpenseType.Items.Clear();
                checkedListBoxBudgetProject.Items.Clear();
                cboxAccountPlan.Properties.Items.Clear();

                if (m_objBudgetExpenseTypeList != null)
                {
                    foreach (CBudgetExpenseType objExpenseType in m_objBudgetExpenseTypeList)
                    {
                        checkedListBoxExpenseType.Items.Add(objExpenseType, false);
                    }
                }
                if (m_objBudgetProjectList != null)
                {
                    foreach (CBudgetProject objBudgetProject in m_objBudgetProjectList)
                    {
                        checkedListBoxBudgetProject.Items.Add(objBudgetProject, false);
                    }
                }
                if (m_objAccountPlanList != null)
                {
                    cboxAccountPlan.Properties.Items.AddRange(m_objAccountPlanList);
                }

                txtDebitArticleName.Text = "";
                txtDebitArticleNum.Text = "";
                txtDebitArticleDscrpn.Text = "";
                checkTranspRest.Checked = false;
                checkDontChange.Checked = false;
                dateEditBegin.DateTime = System.DateTime.MinValue;
                dateEditEnd.DateTime = System.DateTime.MinValue;


            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadComboBox.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }


        /// <summary>
        /// Редактирование статьи расходов
        /// </summary>
        /// <param name="objDebitArticle">статья расходов</param>
        /// <returns>true - успешное завершение; false - ошибка</returns>
        public void EditDebitArticle( ERP_Budget.Common.CDebitArticle objDebitArticle,
            ERP_Budget.Common.CDebitArticle objDebitArticleParent)
        {
            try
            {
                m_objDebitArticle = CDebitArticle.Copy( objDebitArticle );
                DebitArticleParent = CDebitArticle.Copy( objDebitArticleParent );

                m_bCancelEvents = true;
                m_bNewDebitArticle = false;

                // свойства статьи
                LoadBudgetDepListForDebitArticle();

                txtDebitArticleName.Text = DebitArticle.Name;
                txtDebitArticleNum.Text = DebitArticle.ArticleNum;
                txtDebitArticleDscrpn.Text = DebitArticle.ArticleDescription;
                dateEditBegin.DateTime = new DateTime( DebitArticle.FinancislYear, 1, 1 );
                dateEditEnd.DateTime = new DateTime( DebitArticle.FinancislYear, 12, 31 );
                checkTranspRest.Checked = DebitArticle.TransprtRest;
                checkDontChange.Checked = DebitArticle.DontChange;
                cboxAccountPlan.SelectedItem = (DebitArticle.AccountPlan == null) ? null : (cboxAccountPlan.Properties.Items.Cast<CAccountPlan>().SingleOrDefault<CAccountPlan>(x => x.uuidID.Equals(DebitArticle.AccountPlan.uuidID)));

                dateEditBegin.Properties.ReadOnly = true;
                dateEditEnd.Properties.ReadOnly = true;
                treeList.OptionsBehavior.Editable = false;
                tabPageBudgetDepCollectionEditor.PageVisible = (DebitArticleParent.uuidID.Equals(DebitArticle.uuidID));

                // Службы
                ERP_Budget.Common.CBudgetDep objItem = null;
                foreach (ERP_Budget.Common.CBudgetDep objBudgDep in m_objBudgetDepList)
                {
                    objItem = null;
                    objBudgDep.BudgetExpenseType = null;

                    if (DebitArticleParent.BudgetDepList != null)
                    {
                        objItem = DebitArticleParent.BudgetDepList.SingleOrDefault<ERP_Budget.Common.CBudgetDep>(x => x.uuidID.Equals(objBudgDep.uuidID));
                        if (objItem != null)
                        {
                            chklstBudgetDep.Items.Add(objItem, true);
                        }
                        else
                        {
                            chklstBudgetDep.Items.Add(objBudgDep, false);
                        }
                    }
                    else
                    {
                        chklstBudgetDep.Items.Add(objBudgDep, false);
                    }
                }

                // заголовок формы
                this.Text = ( DebitArticleParent.uuidID.Equals( DebitArticle.uuidID ) ? m_strDebitArticleParentCaption : m_strDebitArticleParentCaption );
                lblParentDebitArticle.Text = DebitArticleParent.ArticleFullName;

                m_bCancelEvents = false;
                SetModified( false );

                this.ShowDialog();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( null, "Ошибка редактирования статьи расходов.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
            }
            return;
        }

        private void LoadExpenseTypeListForBudgetDep(ERP_Budget.Common.CBudgetDep objBudgetDep)
        {
            try
            {
                tableLayoutPanelExpenseType.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxExpenseType)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxBudgetProject)).BeginInit();

                for (System.Int32 i = 0; i < checkedListBoxExpenseType.Items.Count; i++)
                {
                    if (checkedListBoxExpenseType.Items[i].CheckState != CheckState.Unchecked)
                    {
                        checkedListBoxExpenseType.Items[i].CheckState = CheckState.Unchecked;
                    }
                }

                if ((objBudgetDep != null) && (objBudgetDep.BudgetExpenseType != null))
                {
                    for (System.Int32 i2 = 0; i2 < checkedListBoxExpenseType.Items.Count; i2++)
                    {
                        if (((ERP_Budget.Common.CBudgetExpenseType)checkedListBoxExpenseType.Items[i2].Value).uuidID.CompareTo(objBudgetDep.BudgetExpenseType.uuidID) == 0)
                        {
                            checkedListBoxExpenseType.Items[i2].CheckState = CheckState.Checked;
                            break;
                        }
                    }
                }


                for (System.Int32 i = 0; i < checkedListBoxBudgetProject.Items.Count; i++)
                {
                    if (checkedListBoxBudgetProject.Items[i].CheckState != CheckState.Unchecked)
                    {
                        checkedListBoxBudgetProject.Items[i].CheckState = CheckState.Unchecked;
                    }
                }

                if ((objBudgetDep != null) && (objBudgetDep.BudgetProject != null))
                {
                    for (System.Int32 i2 = 0; i2 < checkedListBoxBudgetProject.Items.Count; i2++)
                    {
                        if (((CBudgetProject)checkedListBoxBudgetProject.Items[i2].Value).uuidID.CompareTo(objBudgetDep.BudgetProject.uuidID) == 0)
                        {
                            checkedListBoxBudgetProject.Items[i2].CheckState = CheckState.Checked;
                            break;
                        }
                    }
                }

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, String.Format("Ошибка LoadExpenseTypeListForBudgetDep.\n\nТекст ошибки:\n{0}", f.Message), "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.tableLayoutPanelExpenseType.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxExpenseType)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxBudgetProject)).EndInit();
            }
            return;
        }

        private void LoadBudgetDepListForDebitArticle()
        {
            try
            {
                this.tableLayoutPanel2.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                treeList.Nodes.Clear();

                if( (DebitArticleParent == null) || ( DebitArticleParent.BudgetDepList == null) ) { return; }

                foreach (CBudgetDep objBudgetDep in DebitArticleParent.BudgetDepList)
                {
                    treeList.AppendNode(new object[] { objBudgetDep.Name, 
                        ((objBudgetDep.BudgetExpenseType == null) ? "" : objBudgetDep.BudgetExpenseType.Name), 
                        ((objBudgetDep.BudgetProject == null) ? "" : objBudgetDep.BudgetProject.Name) }, null);
                }

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, String.Format("Ошибка LoadExpenseTypeListForBudgetDep.\n\nТекст ошибки:\n{0}", f.Message), "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.tableLayoutPanel2.ResumeLayout(false);
                this.tableLayoutPanel2.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

            }
            return;
        }

        private void checkedListBoxExpenseType_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (checkedListBoxExpenseType.ItemCount == 0) { return; }

                if ((chklstBudgetDep.ItemCount > 0) && (chklstBudgetDep.Items[chklstBudgetDep.SelectedIndex].CheckState == CheckState.Checked))
                {
                    if (e.State == CheckState.Checked)
                    {
                        // снимем галочки с остальных типов расходов
                        for (System.Int32 i = 0; i < checkedListBoxExpenseType.Items.Count; i++)
                        {
                            if (i == e.Index) { continue; }
                            checkedListBoxExpenseType.Items[i].CheckState = CheckState.Unchecked;
                        }

                        ((ERP_Budget.Common.CBudgetDep)chklstBudgetDep.Items[chklstBudgetDep.SelectedIndex].Value).BudgetExpenseType = (ERP_Budget.Common.CBudgetExpenseType)checkedListBoxExpenseType.Items[e.Index].Value; 
                    }
                    else
                    {
                        ((CBudgetDep)chklstBudgetDep.Items[chklstBudgetDep.SelectedIndex].Value).BudgetExpenseType = null;
                    }

                    SetModified(true);
                }

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this,
                String.Format("Ошибка функции checkedListBoxExpenseType_ItemCheck.\nОбъект : {0}\n\nТекст ошибки: {1}", ((Control)sender).Name, f.Message), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void checkedListBoxAccountPlan_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (checkedListBoxBudgetProject.ItemCount == 0) { return; }

                if ((chklstBudgetDep.ItemCount > 0) && (chklstBudgetDep.Items[chklstBudgetDep.SelectedIndex].CheckState == CheckState.Checked))
                {
                    if (e.State == CheckState.Checked)
                    {
                        // снимем галочки с остальных счетов
                        for (System.Int32 i = 0; i < checkedListBoxBudgetProject.Items.Count; i++)
                        {
                            if (i == e.Index) { continue; }
                            checkedListBoxBudgetProject.Items[i].CheckState = CheckState.Unchecked;
                        }

                        ((CBudgetDep)chklstBudgetDep.Items[chklstBudgetDep.SelectedIndex].Value).BudgetProject = (CBudgetProject)checkedListBoxBudgetProject.Items[e.Index].Value; ;
                    }
                    else
                    {
                        ((CBudgetDep)chklstBudgetDep.Items[chklstBudgetDep.SelectedIndex].Value).BudgetProject = null;
                    }

                    SetModified(true);
                }


            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this,
                String.Format("Ошибка функции checkedListBoxAccountPlan_ItemCheck.\nОбъект : {0}\n\nТекст ошибки: {1}", ((Control)sender).Name, f.Message), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void chklstBudgetDep_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadExpenseTypeListForBudgetDep((ERP_Budget.Common.CBudgetDep)chklstBudgetDep.Items[chklstBudgetDep.SelectedIndex].Value);
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this,
                String.Format("Ошибка функции chklstBudgetDep_SelectedIndexChanged.\nОбъект : {0}\n\nТекст ошибки: {1}", ((Control)sender).Name, f.Message), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void chklstBudgetDep_ItemCheck_1(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (this.m_bCancelEvents == false)
                {
                    ERP_Budget.Common.CBudgetDep objSelectedBudgetDep = (ERP_Budget.Common.CBudgetDep)chklstBudgetDep.Items[chklstBudgetDep.SelectedIndex].Value;
                    if (objSelectedBudgetDep != null)
                    {
                        if (e.State == CheckState.Unchecked)
                        {
                            objSelectedBudgetDep.BudgetExpenseType = null;
                        }
                        LoadExpenseTypeListForBudgetDep(objSelectedBudgetDep);
                    }

                    SetModified(true);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this,
                String.Format("Ошибка функции chklstBudgetDep_SelectedIndexChanged.\nОбъект : {0}\n\nТекст ошибки: {1}", ((Control)sender).Name, f.Message), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }


        #endregion

        #region Новая статья
        /// <summary>
        /// Создание новой статьи расходов
        /// </summary>
        public void AddDebitArticle( System.Int32 iArticleNum, System.Int32 iFinancialYear,
            ERP_Budget.Common.CDebitArticle objDebitArticleParent, System.Int32 iArticleChildNum)
        {
            try
            {
                this.m_bNewDebitArticle = true;
                this.m_objDebitArticle = new ERP_Budget.Common.CDebitArticle();
                this.m_objDebitArticle.uuidID = System.Guid.NewGuid();
                this.m_objDebitArticle.ArticleNum = iArticleNum.ToString();
                this.m_objDebitArticle.ArticleID = iArticleNum;
                this.m_objDebitArticle.FinancislYear = iFinancialYear;

                if (objDebitArticleParent != null)
                {
                    DebitArticleParent = CDebitArticle.Copy(objDebitArticleParent);
                    this.m_objDebitArticle.ParentID = DebitArticleParent.uuidID;
                    this.m_objDebitArticle.ArticleNum = String.Format("{0}.{1}", DebitArticleParent.ArticleNum, iArticleChildNum);
                    this.m_objDebitArticle.ArticleID = iArticleChildNum;
                    this.m_objDebitArticle.DontChange = DebitArticleParent.DontChange;
                    this.m_objDebitArticle.FinancislYear = DebitArticleParent.FinancislYear;
                    this.m_objDebitArticle.TransprtRest = DebitArticleParent.TransprtRest;
                    this.m_objDebitArticle.AccountPlan = DebitArticleParent.AccountPlan;
                }

                m_bCancelEvents = true;

                LoadBudgetDepListForDebitArticle();

                txtDebitArticleName.Text = DebitArticle.Name;
                txtDebitArticleNum.Text = DebitArticle.ArticleNum;
                txtDebitArticleDscrpn.Text = DebitArticle.ArticleDescription;
                dateEditBegin.DateTime = new DateTime(DebitArticle.FinancislYear, 1, 1);
                dateEditEnd.DateTime = new DateTime(DebitArticle.FinancislYear, 12, 31);
                checkTranspRest.Checked = DebitArticle.TransprtRest;
                checkDontChange.Checked = DebitArticle.DontChange;
                cboxAccountPlan.SelectedItem = (DebitArticle.AccountPlan == null) ? null : (cboxAccountPlan.Properties.Items.Cast<CAccountPlan>().SingleOrDefault<CAccountPlan>(x => x.uuidID.Equals(DebitArticle.AccountPlan.uuidID)));

                dateEditBegin.Properties.ReadOnly = (objDebitArticleParent != null);
                dateEditEnd.Properties.ReadOnly = (objDebitArticleParent != null);
                txtDebitArticleNum.Properties.ReadOnly = (objDebitArticleParent != null);

                treeList.OptionsBehavior.Editable = false;
                tabPageBudgetDepCollectionEditor.PageVisible = ( DebitArticleParent == null);

                // Службы
                ERP_Budget.Common.CBudgetDep objItem = null;
                foreach (ERP_Budget.Common.CBudgetDep objBudgDep in m_objBudgetDepList)
                {
                    objItem = null;
                    objBudgDep.BudgetExpenseType = null;

                    if( ( DebitArticleParent != null ) && (DebitArticleParent.BudgetDepList != null) )
                    {
                        objItem = DebitArticleParent.BudgetDepList.SingleOrDefault<ERP_Budget.Common.CBudgetDep>(x => x.uuidID.Equals(objBudgDep.uuidID));
                        if (objItem != null)
                        {
                            chklstBudgetDep.Items.Add(objItem, true);
                        }
                        else
                        {
                            chklstBudgetDep.Items.Add(objBudgDep, false);
                        }
                    }
                    else
                    {
                        chklstBudgetDep.Items.Add(objBudgDep, false);
                    }
                }

                // заголовок формы
                this.Text = (this.m_objDebitArticle.ParentID.CompareTo(System.Guid.Empty) == 0) ? "Статья расходов" : "Подстатья расходов";
                lblParentDebitArticle.Text = ((DebitArticleParent == null) ? "" : DebitArticleParent.ArticleFullName);

                m_bCancelEvents = false;
                SetModified( false );
                this.ShowDialog();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( null, "Ошибка создания статьи расходов.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
            }
            return;
        }


        #endregion

        #region Индикация изменений

        private System.Boolean m_bIsModified;
        private System.Boolean m_bCancelEvents;
        /// <summary>
        /// Устанавливает индикатор "изменена запись"
        /// </summary>
        private void SetModified( System.Boolean bModified )
        {
            try
            {
                m_bIsModified = bModified;
                btnSave.Enabled = bModified;
                btnCancel.Enabled = bModified;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка метода SetModified().\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// Возвращает признак того, изменялась ли запись
        /// </summary>
        /// <returns>true - изменялась; false - не изменялась</returns>
        private System.Boolean IsModified()
        {
            return m_bIsModified;
        }

        private void EditValueChanged( object sender, EventArgs e )
        {
            try
            {
                if( this.m_bCancelEvents == false )
                {
                    SetModified( true );
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                String.Format("Ошибка функции EditValueChanged.\nОбъект : {0}\n\nТекст ошибки: {1}", ((Control)sender).Name, f.Message), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (this.m_bCancelEvents == false)
                {
                    SetModified(true);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this,
                String.Format("EditValueChanging.\nОбъект : {0}\n\nТекст ошибки: {1}", ((Control)sender).Name, f.Message), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Сохранение свойств статьи расходов
        /// <summary>
        /// Присваивает свойствам объекта значения из элементов управления
        /// </summary>
        /// <returns>true - операция прошла успешно;false - ошибка</returns>
        private System.Boolean bInitDebitArticleParams( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                this.m_objDebitArticle.Name = txtDebitArticleName.Text;
                this.m_objDebitArticle.ArticleNum = txtDebitArticleNum.Text;
                this.m_objDebitArticle.ArticleDescription = txtDebitArticleDscrpn.Text;
                this.m_objDebitArticle.TransprtRest = checkTranspRest.Checked;
                this.m_objDebitArticle.DontChange = checkDontChange.Checked;
                this.m_objDebitArticle.FinancislYear = dateEditBegin.DateTime.Year;
                this.m_objDebitArticle.AccountPlan = (CAccountPlan)cboxAccountPlan.SelectedItem;

                if( this.m_bIsCanModifWarningParam == true )
                {
                    // определим список служб
                    this.m_objDebitArticle.BudgetDepList.Clear();

                    // в том случае, если редактируется подстатья, то список служб не сохраняется
                    if ((DebitArticleParent != null) && ( DebitArticleParent.uuidID.Equals(DebitArticle.uuidID) ))
                    {
                        for (System.Int32 i = 0; i < chklstBudgetDep.Items.Count; i++)
                        {
                            if (chklstBudgetDep.Items[i].CheckState == CheckState.Checked)
                            {
                                m_objDebitArticle.BudgetDepList.Add((ERP_Budget.Common.CBudgetDep)chklstBudgetDep.Items[i].Value);
                            }
                        }
                    }

                }

                bRet = true;
            }
            catch( System.Exception f )
            {
                bRet = false;
                strErr += (String.Format("\nОшибка инициализации свойств объекта \"Статья расходов\".\nТекст ошибки:\n{0}", f.Message));
            }

            return bRet;
        }

        /// <summary>
        /// Сохраняет типа бюджетной проводки
        /// </summary>
        /// <returns>true - успешное завершение; false - ошибка</returns>
        private System.Boolean bSaveDebitArticle( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                if( this.m_objDebitArticle == null )
                {
                    strErr += ("Не удалось создать объект \"Статья расходов\".");
                    return bRet;
                }
                // теперь присвоим свойствам объекта "Статья расходов" значения из элементов управления
                if (bInitDebitArticleParams(ref strErr) == true)
                {
                    // теперь проверим обязательные параметры
                    if (this.m_objDebitArticle.IsValidateProperties( ref strErr ) == true)
                    {
                        // все свойства определены, можно сохраняться
                        if (this.m_bNewDebitArticle == true)
                        {
                            // новая статья
                            bRet = this.m_objDebitArticle.Add(this.m_objProfile, ref strErr);
                            if (bRet == true) { m_bNewDebitArticle = false; }
                        }
                        else
                        {
                            bRet = this.m_objDebitArticle.Update(this.m_objProfile, this.m_bIsCanModifWarningParam, ref strErr );
                        }
                    }
                }
            }
            catch( System.Exception f )
            {
                strErr += String.Format("\nОшибка сохранения свойств объекта \"Статья расходов\".\nТекст ошибки: {0}", f.Message);
            }

            return bRet;
        }

        private void btnSave_Click( object sender, EventArgs e )
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                if (IsModified() == false) { return; }

                System.String strErr = "";
                if (bSaveDebitArticle( ref strErr ) == true)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Ошибка сохранения",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка сохранения свойств статьи расходов.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }

        #endregion

        #region Отмена
        private void btnCancel_Click( object sender, EventArgs e )
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( null, "Ошибка отмены изменений.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        #endregion

        private void frmDebitArticleEditor_Shown( object sender, EventArgs e )
        {
            try
            {
                txtDebitArticleName.Focus();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка отображения формы.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }




    }
}