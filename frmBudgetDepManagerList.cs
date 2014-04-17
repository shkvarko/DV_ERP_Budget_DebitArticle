using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DebitArticle
{
    public partial class frmBudgetDepManagerList : DevExpress.XtraEditors.XtraForm
    {
        #region Переменные, Свойства, Константы
        UniXP.Common.CProfile m_objProfile;
        System.Guid m_uuidBudgetDepID;
        ERP_Budget.Common.CBudgetDep m_objBudgetDep;
        #endregion

        public frmBudgetDepManagerList(UniXP.Common.CProfile objProfile, System.Guid uuidBudgetDepID)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_uuidBudgetDepID = uuidBudgetDepID;
            m_objBudgetDep = new ERP_Budget.Common.CBudgetDep(m_uuidBudgetDepID);
        }

        private void LoadManagerList()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                treeList.CellValueChanged -= new DevExpress.XtraTreeList.CellValueChangedEventHandler(treeList_CellValueChanged);
                m_objBudgetDep.Init(m_objProfile, m_objBudgetDep.uuidID);
                label1.Text = "Распорядитель: " + m_objBudgetDep.Manager.UserFullName;
                ERP_Budget.Common.CBudgetDep.RefreshBudgetDepManagerList(m_objProfile, m_uuidBudgetDepID, treeList);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось обновиь список дополнительных распорядителей.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = false;
                treeList.CellValueChanged += new DevExpress.XtraTreeList.CellValueChangedEventHandler(treeList_CellValueChanged);
                this.Cursor = Cursors.Default;
            }
            return;

        }

        private void frmBudgetDepManagerList_Shown(object sender, EventArgs e)
        {
            try
            {
                LoadManagerList();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка загрузки формы.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка закрытия формы.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void SaveChanges()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (treeList.Nodes.Count > 0)
                {
                    if (ERP_Budget.Common.CBudgetDep.SaveBudgetDepManagerList(m_objProfile, m_uuidBudgetDepID, treeList) == true)
                    {
                        this.Cursor = Cursors.Default;
                        this.Close();
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка сохранения изменений.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveChanges();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка сохранения изменений.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void treeList_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                btnSave.Enabled = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка treeList_CellValueChanged.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
    }
}
