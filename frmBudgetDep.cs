using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace DebitArticle
{
    public partial class frmBudgetDep : DevExpress.XtraEditors.XtraForm
    {
        #region Переменные, Свойства, Константы
        UniXP.Common.CProfile m_objProfile;
        List<ERP_Budget.Common.CUser> m_objUserList;
        #endregion

        public frmBudgetDep( UniXP.Common.CProfile objProfile )
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objUserList = null;
            //m_bDREditRootDebitArticle = false;
            // проверка динамических прав
            SetAccessForDynamicRights();
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

        #region Динамические права
        //private System.Boolean m_bDREditRootDebitArticle;
        private void SetAccessForDynamicRights()
        {
            try
            {
                //// редактирование статей расходов
                //m_bDREditRootDebitArticle = ( m_objProfile.GetClientsRight() ).GetState( ERP_Budget.Global.Consts.strDREditRootDebitArticle );
                //if( m_bDREditRootDebitArticle == false )
                //{
                //    // нет прав на работу со статьями расходов
                //    barBtnAddNode.Enabled = m_bDREditRootDebitArticle;
                //    mitemAddRoot.Enabled = m_bDREditRootDebitArticle;
                //}
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
        /// Устанавливает индикатор "изменено дерево" в true
        /// </summary>
        private void SetModifiedToTrue()
        {
            try
            {
                m_bIsTreeModified = true;
                btnSave.Enabled = m_bIsTreeModified;
                btnCancel.Enabled = m_bIsTreeModified;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка SetModifiedToTrue()\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// Устанавливает индикатор "изменено дерево" в false
        /// </summary>
        private void SetModifiedToFalse()
        {
            try
            {
                m_bIsTreeModified = false;
                btnSave.Enabled = m_bIsTreeModified;
                btnCancel.Enabled = m_bIsTreeModified;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка SetModifiedToFalse()\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void treeList_CellValueChanged( object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e )
        {
            try
            {
                SetModifiedToTrue();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка treeList_CellValueChanged\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        #endregion

        #region Построение дерева бюджетных подразделений

        /// <summary>
        /// Загружает дерево бюджетных подразделений
        /// </summary>
        /// <returns>true - успешно; false - ошибка</returns>
        private System.Boolean bLoadBudgetDeps()
        {
            System.Boolean bRes = false;
            Cursor = Cursors.WaitCursor;
            
            try
            {
                // необходимо получить список бюджетных подразделений
                List<ERP_Budget.Common.CBudgetDep> objBudgetDepList = ERP_Budget.Common.CBudgetDep.GetBudgetDepsList( m_objProfile, true );
                if( objBudgetDepList == null ) { return bRes; }

                // заполняем набор данных, связанный с деревом
                treeList.Enabled = false;
                treeList.DataSource = null;

                dsDirect.Tables[ "dtBudgetDep" ].Rows.Clear();
                System.Int32 iItemsCount = objBudgetDepList.Count;
                if( iItemsCount > 0 )
                {
                    System.Data.DataRow newRow = null;
                    ERP_Budget.Common.CBudgetDep objItem = null;
                    System.Int32 iBudgetDepDeclrn = 0;
                    System.Int32 iComaCount = 0;
                    System.String strBudgetDepDeclrn = "";
                    for( System.Int32 i = 0; i< iItemsCount; i++ )
                    {
                        objItem = objBudgetDepList[ i ];
                        newRow = dsDirect.Tables[ "dtBudgetDep" ].NewRow();
                        newRow[ "BUDGETDEP_GUID_ID" ] = objItem.uuidID;
                        newRow[ "BUDGETDEP_PARENT_GUID_ID" ] = objItem.ParentID;
                        newRow[ "BUDGETDEP_NAME" ] = objItem.Name;
                        newRow[ "BUDGETDEP_MANAGER_ID" ] = objItem.Manager.ulID;
                        newRow[ "READONLY" ] = objItem.ReadOnly;
                        // формируем строку с составом бюджетного подразделения
                        iBudgetDepDeclrn = 0;
                        iComaCount = 0;
                        strBudgetDepDeclrn = "";

                        iBudgetDepDeclrn = objItem.UsesrList.GetCountItems();
                        if( iBudgetDepDeclrn > 0 )
                        {
                            iComaCount = ( iBudgetDepDeclrn > 0 ) ? ( iBudgetDepDeclrn - 1 ) : 0;
                            for( System.Int32 i2 = 0; i2 < iBudgetDepDeclrn; i2++ )
                            {
                                strBudgetDepDeclrn += objItem.UsesrList.GetByIndex( i2 ).UserLastName + " " + objItem.UsesrList.GetByIndex( i2 ).UserFirstName;
                                if( iComaCount > 0 )
                                {
                                    strBudgetDepDeclrn += ";";
                                    iComaCount--;
                                }
                            }
                        }
                        newRow[ "BUDGETDEP_DECLRN" ] = strBudgetDepDeclrn;
                        dsDirect.Tables[ "dtBudgetDep" ].Rows.Add( newRow );
                    }
                }
                // загружаем список сотрудников
                bLoadUsers();
                // подтверждаем изменения в наборе данных
                dsDirect.AcceptChanges();
                bRes = true;
            }//try
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка чтения списка бюджетных подразделений\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
                treeList.Enabled = true;
                SetModifiedToFalse();


                this.treeList.DataMember = "dtBudgetDep";
                this.treeList.DataSource = this.dsDirect;
                this.treeList.KeyFieldName = "BUDGETDEP_GUID_ID";
                this.treeList.Name = "treeList";

                treeList.RefreshDataSource();

                Cursor = Cursors.Default;
            }

            return bRes;
        }
        /// <summary>
        /// Загружает список сотрудников
        /// </summary>
        /// <returns></returns>
        private System.Boolean bLoadUsers()
        {
            System.Boolean bRes = false;
            try
            {
                // необходимо получить список сотрудников
                ERP_Budget.Common.CUser objUser = new ERP_Budget.Common.CUser();
                m_objUserList = objUser.GetBudgetUserList(m_objProfile, true, false, false);
                if (m_objUserList == null) { return bRes; }

                // заполняем набор данных, связанный с деревом
                checklboxBudgetDepDclrn.Items.Clear();
                dsDirect.Tables[ "dtUser" ].Rows.Clear();
                System.Int32 iItemsCount = m_objUserList.Count();
                if( iItemsCount > 0 )
                {
                    System.Data.DataRow newRow = null;
                    ERP_Budget.Common.CUser objItem = null;

                    for( System.Int32 i = 0; i< iItemsCount; i++ )
                    {
                        objItem = m_objUserList[i];

                        if (objItem.DynamicRightsList.FindByName(ERP_Budget.Global.Consts.strDRManager).IsEnable == true)
                        {
                            newRow = dsDirect.Tables["dtUser"].NewRow();
                            newRow["USER_ID"] = objItem.ulID;
                            newRow["USER_NAME"] = String.Format("{0} {1}", objItem.UserLastName, objItem.UserFirstName);
                            dsDirect.Tables["dtUser"].Rows.Add(newRow);
                        }

                        if (objItem.IsBlocked == false)
                        {
                            checklboxBudgetDepDclrn.Items.Add(new DevExpress.XtraEditors.Controls.CheckedListBoxItem(objItem.UserLastName + " " + objItem.UserFirstName));
                        }
                        objItem = null;
                    }
                    repItemlkpBudgetDep.DataSource = dsDirect.Tables[ "dtUser" ];
                }
                // выпадающий список сотрудников
                this.repItemUsersList.DataSource = this.dtUser;
                this.repItemUsersList.DisplayMember = "USER_NAME";
                this.repItemUsersList.ValueMember = "USER_ID";
                this.repItemUsersList.Columns.Clear();
                this.repItemUsersList.Columns.AddRange( new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
		            new DevExpress.XtraEditors.Controls.LookUpColumnInfo( "USER_NAME", "Сотрудник", 100, 
                        DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, 
                        DevExpress.Data.ColumnSortOrder.None ) } );

                bRes = true;
            }//try
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка чтения списка сотрудников\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
            }

            return bRes;
        }
        /// <summary>
        /// Обновляет список бюджетных подразделений
        /// </summary>
        private void RefreshBudgetDepsList()
        {
            try
            {
                System.Data.DataSet dsChanges = dsDirect.GetChanges();
                if( dsChanges != null )
                {
                    if( dsChanges.Tables[ "dtBudgetDep" ].Rows.Count > 0 )
                    {
                        if( System.Windows.Forms.MessageBox.Show( this,
                            "В списке бюджетных подразделений были произведены изменения.\nВ случае обновления все изменения будут отменены.\nОбновить список?",
                            "Внимание", System.Windows.Forms.MessageBoxButtons.YesNo,
                            System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                        { return; }
                    }
                }
                bLoadBudgetDeps();
            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка обновления списка бюджетных подразделений\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
            }

            return ;
        }

        private void barBtnRefresh_ItemClick( object sender, DevExpress.XtraBars.ItemClickEventArgs e )
        {
            try
            {
                RefreshBudgetDepsList();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка обновления списка бюджетных подразделений\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void mitemRefresh_Click( object sender, EventArgs e )
        {
            try
            {
                RefreshBudgetDepsList();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка обновления списка бюджетных подразделений\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
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
                // если в дереве есть узлы и один из них выделен, необходимо проверить его значения на предмет ошибок
                if( treeList.Nodes.Count > 0 )
                {
                    if( ( treeList.FocusedNode != null ) && ( treeList.FocusedNode.Selected ) )
                    {
                        if( bIsNodeValuesValidation( treeList.FocusedNode, true ) == false ) { return; }
                    }
                }
                //прописываем значения нового узла
                System.Guid newuuidID = System.Guid.NewGuid();
                if( objParentNode == null )
                {
                    System.Data.DataRow newRow = dsDirect.Tables[ "dtBudgetDep" ].NewRow();
                    newRow[ "BUDGETDEP_GUID_ID" ] = newuuidID;
                    newRow[ "BUDGETDEP_PARENT_GUID_ID" ] = System.Guid.Empty;
                    newRow[ "BUDGETDEP_NAME" ] = "Подразделение";
                    newRow[ "READONLY" ] = false;
                    newRow[ "BUDGETDEP_DECLRN" ] = "";
                    dsDirect.Tables[ "dtBudgetDep" ].Rows.Add( newRow );
                }
                else
                {
                    // добавление дочернего узла - подстатья расходов
                    System.Data.DataRow newRow = dsDirect.Tables[ "dtBudgetDep" ].NewRow();
                    newRow[ "BUDGETDEP_GUID_ID" ] = newuuidID;
                    newRow[ "BUDGETDEP_PARENT_GUID_ID" ] = ( System.Guid )objParentNode.GetValue( colGuid_ID );
                    newRow[ "BUDGETDEP_NAME" ] = "Подразделение";
                    newRow[ "READONLY" ] = false;
                    newRow[ "BUDGETDEP_DECLRN" ] = "";
                    dsDirect.Tables[ "dtBudgetDep" ].Rows.Add( newRow );
                }

                SetModifiedToTrue();
            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка добавления узла\n" + e.Message, "Ошибка",
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
                if( objNode == null ){ return; }
                // проверяем, можно ли удалять узел
                if( IsPossibleDeleteNode( objNode ) == false )
                {
                    System.Windows.Forms.MessageBox.Show( this,
                        "Выбранное подразделение/дочернее подразделение удалить нельзя.\nНа него есть ссылка в бюджете либо бюджетном документе.", "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning );
                    return;
                }
                // запрашиваем список узлов, подлежащих удалению
                ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList = CreateNodeListForDelete( objNode );
                if( objList.GetCountItems() > 0 )
                {
                    // удаляем информацию об узле из БД
                    if( ERP_Budget.Common.CBudgetDep.SaveBudgetDepListToDB( objList, m_objProfile ) == true )
                    {
                        treeList.BeforeFocusNode -= new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler( this.treeList_BeforeFocusNode );
                        treeList.DeleteNode( objNode );
                        treeList.BeforeFocusNode += new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler( this.treeList_BeforeFocusNode );
                    }
                }
            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка удаления узла\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        /// <summary>
        /// Проверяет можно ли удалить узел,
        /// нет ли у дочерних узлов признака "только чтение"
        /// </summary>
        /// <param name="objNode"></param>
        /// <returns></returns>
        private System.Boolean IsPossibleDeleteNode( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            System.Boolean bRes = true;
            try
            {
                if( objNode == null ) { return false; }
                if( ( System.Boolean )objNode.GetValue( colReadOnly ) == true ) { return false; }
                // проверяем дочерние узлы
                for( System.Int32 i = 0; i < objNode.Nodes.Count; i++ )
                {
                    if( objNode.Nodes[ i ].Nodes.Count == 0 )
                    {
                        if( ( System.Boolean )objNode.Nodes[ i ].GetValue( colReadOnly ) == true )
                        {
                            bRes = false;
                            break;
                        }
                    }
                    else
                    {
                        if( IsPossibleDeleteNode( objNode.Nodes[ i ] ) == false )
                        {
                            bRes = false;
                            break;
                        }
                    }
                }
            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка проверки возможности удаления узла\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return bRes;
        }
        /// <summary>
        /// Заполняет список с объектами "бюджетные подразделения", подлежащих удалению
        /// </summary>
        /// <param name="objList">заполняемый список</param>
        /// <param name="objNode">узел дерева</param>
        private void FillNodeListForDelete( ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList,
            DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            try
            {
                if( objNode == null ) { return; }
                ERP_Budget.Common.CBudgetDep objBudgetDep = new ERP_Budget.Common.CBudgetDep();
                objBudgetDep.uuidID = ( System.Guid )objNode.GetValue( colGuid_ID );
                objBudgetDep.Name = ( System.String )objNode.GetValue( colBudgetDepName );
                objBudgetDep.State = DataRowState.Deleted;
                objList.AddItemToList( objBudgetDep );
                // проверяем дочерние узлы
                for( System.Int32 i = 0; i < objNode.Nodes.Count; i++ )
                {
                    FillNodeListForDelete( objList, objNode.Nodes[ i ] );
                }
            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка заполнения списка узлов подлежащих удалению\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return ;
        }
        /// <summary>
        /// Создает список с объектами "бюджетные подразделения", подлежащих удалению
        /// </summary>
        /// <param name="objDelNode">удаляемый узел</param>
        /// <returns>список объектов "бюджетные подразделения"</returns>
        private ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> CreateNodeListForDelete( DevExpress.XtraTreeList.Nodes.TreeListNode objDelNode )
        {
            ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList = new ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep>();
            objList.ClearList();
            try
            {
                if( objDelNode == null ) { return objList; }
                // заполняем список узлов для удаления
                FillNodeListForDelete( objList, objDelNode );
                // теперь необходимо расположить объекты в списке в обратном порядке
                if( objList.GetCountItems() > 0 )
                {
                    System.Collections.ArrayList objNodeList = new System.Collections.ArrayList();
                    objNodeList.Clear();
                    for( System.Int32 i = ( objList.GetCountItems() - 1 ); i >= 0; i-- )
                    {
                        objNodeList.Add( objList.GetByIndex( i ) );
                    }
                    if( objNodeList.Count == ( objList.GetCountItems() ) )
                    {
                        objList.ClearList();
                        for( System.Int32 i2 = 0; i2 < objNodeList.Count; i2++ )
                        {
                            objList.AddItemToList( ( ERP_Budget.Common.CBudgetDep )objNodeList[ i2 ] );
                        }
                    }
                }

            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка создания списка узлов подлежащих удалению\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return objList;
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
                    "Ошибка добавления бюджетного подразделения\n" + f.Message, "Ошибка",
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
                    "Ошибка добавления бюджетного подразделения\n" + f.Message, "Ошибка",
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
                    "Ошибка добавления бюджетного подразделения\n" + f.Message, "Ошибка",
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
                    "Ошибка добавления бюджетного подразделения\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        private void DeleteNode()
        {
            try
            {
                if( treeList.FocusedNode == null ) { return; }

                // китайское предупреждение по поводу удаления узла
                System.String strQuestion = "Удалить " + ( ( treeList.FocusedNode.ParentNode == null ) ? "бюджетное подразделение" : " дочернее бюджетное подразделение" ) + "?\nОтмена удаления будет невозможна!";
                if( System.Windows.Forms.MessageBox.Show( this, strQuestion, "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                { return; }

                // удаляем узел
                DeleteNode( treeList.FocusedNode );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка удаления бюджетного подразделения\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        private void barBtnDeleteNode_ItemClick( object sender, DevExpress.XtraBars.ItemClickEventArgs e )
        {
            try
            {
                DeleteNode();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка удаления бюджетного подразделения\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void mitemDelete_Click( object sender, EventArgs e )
        {
            try
            {
                DeleteNode();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "Ошибка удаления бюджетного подразделения\n" + f.Message, "Ошибка",
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
                if( e == DragDropEffects.Move )
                    this.Cursor = Cursors.Default;
                if( e == DragDropEffects.Copy )
                    this.Cursor = Cursors.Default;
                if( e == DragDropEffects.None )
                    this.Cursor = Cursors.No;
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
                if( ( draggedNode != null ) && ( ( e.KeyState & 4) != 4 ) )
                {
                    //узел, над которым отпустили мышку
                    DevExpress.XtraTreeList.Nodes.TreeListNode node = hi.Node;
                    if( node != null )
                    {
                        System.Guid uuidID = ( System.Guid )draggedNode.GetValue( colGuid_ID );
                        System.Guid uuidParentID = ( System.Guid )node.GetValue( colGuid_ID );

                        // перемещаем узел
                        treeList.MoveNode( draggedNode, node );
                        // изменяем указатель на родителя у перетаскиваемого узла
                        foreach( System.Data.DataRow row in dsDirect.Tables[ "dtBudgetDep" ].Rows )
                        {
                            if( ( ( System.Guid )row[ "BUDGETDEP_GUID_ID" ] ).CompareTo( uuidID ) == 0 )
                            {
                                row[ "BUDGETDEP_PARENT_GUID_ID" ] = uuidParentID;
                                break;
                            }
                        }
                        // делаем пометку, что изменения произошли
                        SetModifiedToTrue();
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
        /// Объект перетаскивается над treeList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_DragOver( object sender, DragEventArgs e )
        {
            try
            {
                DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo( treeList.PointToClient( new Point( e.X, e.Y ) ) );
                DevExpress.XtraTreeList.Nodes.TreeListNode node = GetDragNode( e.Data );
                if( node == null )
                {
                    if( hi.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Empty || hi.Node != null )
                        e.Effect = System.Windows.Forms.DragDropEffects.Copy;
                    else
                        e.Effect = System.Windows.Forms.DragDropEffects.None;
                }
                else
                {
                    if( bIsNodeValuesValidation( node, false ) )
                    {
                        if( node.ParentNode == null )
                        //if( ( node.ParentNode == null ) && ( m_bDREditRootDebitArticle == false ) )
                        { e.Effect = System.Windows.Forms.DragDropEffects.None; }
                    }
                    else
                    {
                        e.Effect = System.Windows.Forms.DragDropEffects.None; 
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
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode == null ) { return; }
                if( objFocusedNode.ParentNode == null ) { return; }
                System.Guid uuidID = (System.Guid)objFocusedNode.GetValue(colGuid_ID);
                System.Guid uuidParentID = ( objFocusedNode.ParentNode.ParentNode == null ) ? System.Guid.Empty : ( System.Guid )objFocusedNode.ParentNode.ParentNode.GetValue( colGuid_ID );

                // изменяем указатель на родителя у перетаскиваемого узла
                foreach (System.Data.DataRow row in dsDirect.Tables["dtBudgetDep"].Rows)
                {
                    if (((System.Guid)row["BUDGETDEP_GUID_ID"]).CompareTo(uuidID) == 0)
                    {
                        if (objFocusedNode.ParentNode.ParentNode != null)
                        {
                            row["BUDGETDEP_PARENT_GUID_ID"] = uuidParentID;
                        }
                        else
                        {
                            row["BUDGETDEP_PARENT_GUID_ID"] = System.DBNull.Value;
                        }
                        break;
                    }
                }

                treeList.RefreshDataSource();
                // делаем пометку об изменениях в дереве
                SetModifiedToTrue();
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
        private void OnRight( )
        {
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode == null ) { return; }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevFocusedNode = objFocusedNode.PrevNode;
                if( objPrevFocusedNode == null ) { return; }
//                if ((objPrevFocusedNode == null) || (objFocusedNode.ParentNode == null)) { return; }
                System.Guid uuidID = (System.Guid)objFocusedNode.GetValue(colGuid_ID);
                System.Guid uuidParentID = ( System.Guid )objPrevFocusedNode.GetValue( colGuid_ID );

                // перемещаем узел
                treeList.MoveNode( objFocusedNode, objPrevFocusedNode );
                // изменяем указатель на родителя у перетаскиваемого узла
                foreach( System.Data.DataRow row in dsDirect.Tables[ "dtBudgetDep" ].Rows )
                {
                    if( ( ( System.Guid )row[ "BUDGETDEP_GUID_ID" ] ).CompareTo( uuidID ) == 0 )
                    {
                        row[ "BUDGETDEP_PARENT_GUID_ID" ] = uuidParentID;
                        break;
                    }
                }
                //objFocusedNode.SetValue( colParentGuid_ID, ( System.Guid )objFocusedNode.ParentNode.GetValue( colGuid_ID ) );
                // делаем пометку об изменениях в дереве
                SetModifiedToTrue();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnRight\n" + f.Message, "Ошибка",
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
        private void OnDown( )
        {
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode == null ) { return; }
                DevExpress.XtraTreeList.Nodes.TreeListNode objNexFocusedNode = objFocusedNode.NextNode;
                if( objNexFocusedNode == null ) { return; }
                // перемещаем узел
                System.Int32 iFocusedNodeIndx = treeList.GetNodeIndex( objFocusedNode ); 
                System.Int32 iNexFocusedNodeIndx = treeList.GetNodeIndex( objNexFocusedNode );
                treeList.SetNodeIndex( objFocusedNode, iNexFocusedNodeIndx );
                treeList.SetNodeIndex( objNexFocusedNode, iFocusedNodeIndx );
                treeList.FocusedNode = objFocusedNode;
                // делаем пометку об изменениях в дереве
                SetModifiedToTrue();
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
        private void OnUp( )
        {
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode == null ) { return; }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevFocusedNode = objFocusedNode.PrevNode;
                if( objPrevFocusedNode == null ) { return; }
                // перемещаем узел
                System.Int32 iFocusedNodeIndx = treeList.GetNodeIndex( objFocusedNode );
                System.Int32 iPrevFocusedNodeIndx = treeList.GetNodeIndex( objPrevFocusedNode );
                treeList.SetNodeIndex( objFocusedNode, iPrevFocusedNodeIndx );
                treeList.SetNodeIndex( objPrevFocusedNode, iFocusedNodeIndx );
                treeList.FocusedNode = objPrevFocusedNode;
                // делаем пометку об изменениях в дереве
                SetModifiedToTrue();
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
                if( objFocusedNode == null )
                {
                    // ничего не выделено
                    btnLeft.Enabled = false;
                    btnRight.Enabled = false;
                    btnUp.Enabled = false;
                    btnDown.Enabled = false;
                    btnDrop.Enabled = false;
                }
                else
                {
                    // узел выделен
                    btnLeft.Enabled = ( objFocusedNode.ParentNode != null );
                    btnRight.Enabled = ( objFocusedNode.PrevNode != null );
                    
                    //btnLeft.Enabled = ((objFocusedNode.ParentNode != null) && (objFocusedNode.ParentNode.ParentNode != null));
                    //btnRight.Enabled = ((objFocusedNode.PrevNode != null) && (objFocusedNode.ParentNode != null));

                    if (objFocusedNode.ParentNode == null)
                    {
                        btnUp.Enabled = ( objFocusedNode.PrevNode != null );
                        btnDown.Enabled = ( objFocusedNode.NextNode != null );
                    }
                    else
                    {
                        btnUp.Enabled = ( objFocusedNode.PrevNode != null );
                        btnDown.Enabled = ( objFocusedNode.NextNode != null );
                    }
                    // если нет прав на редактирование статей и выделена статья, гасим кнопку "удалить"
                    btnDrop.Enabled = true; // ( objFocusedNode.ParentNode == null ) ? false : true;
                    barBtnDeleteNode.Enabled = btnDrop.Enabled;

                    // найдем строку в наборе данных, соответствующую выбранному узлу
                    System.Guid uuidID = ( System.Guid )objFocusedNode.GetValue( colGuid_ID );
                    System.Int32 iRecNum = -1;
                    for( System.Int32 i = 0; i < dsDirect.Tables[ "dtBudgetDep" ].Rows.Count; i++ )
                    {
                        if( dsDirect.Tables[ "dtBudgetDep" ].Rows[ i ].RowState != DataRowState.Deleted )
                        {
                            if( ( ( System.Guid )dsDirect.Tables[ "dtBudgetDep" ].Rows[ i ][ "BUDGETDEP_GUID_ID" ] ).CompareTo( uuidID ) == 0 )
                            {
                                iRecNum = i;
                                break;
                            }
                        }
                    }
                    if( iRecNum >= 0 )
                    {
                        dsDirect.Tables[ "dtBudgetDep" ].Rows[ iRecNum ].EndEdit();
                        System.String strBudgetDepDeclrn = ( System.String )dsDirect.Tables[ "dtBudgetDep" ].Rows[ iRecNum ][ "BUDGETDEP_DECLRN" ];

                        foreach( DevExpress.XtraEditors.Controls.CheckedListBoxItem item in checklboxBudgetDepDclrn.Items )
                            item.CheckState = ( strBudgetDepDeclrn.IndexOf( item.Value.ToString() ) > -1 )  ? CheckState.Checked : CheckState.Unchecked;
                        checklboxBudgetDepDclrn.SelectedIndex = 0;
                    }


                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnSelectItem\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
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
                    if( ( treeList.Nodes.Count > 0 ) && ( treeList.FocusedNode != null ) )
                    {
                        mitemDelete.Enabled = true;
                        //if( treeList.FocusedNode.ParentNode == null )
                        //{
                        //    mitemDelete.Enabled = m_bDREditRootDebitArticle;
                        //}
                        //else
                        //{ mitemDelete.Enabled = true; }
                    }
                    else
                    { mitemDelete.Enabled = false; }
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
                treeList.BeforeFocusNode -= new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler( this.treeList_BeforeFocusNode );
                checklboxBudgetDepDclrn.ItemCheck -= new DevExpress.XtraEditors.Controls.ItemCheckEventHandler( this.checklboxBudgetDepDclrn_ItemCheck );
                dsDirect.RejectChanges();
                treeList.BeforeFocusNode += new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler( this.treeList_BeforeFocusNode );
                checklboxBudgetDepDclrn.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler( this.checklboxBudgetDepDclrn_ItemCheck );
                // делаем пометку об изменениях в дереве
                SetModifiedToFalse();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка отмены изменений\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void btnCancel_Click( object sender, EventArgs e )
        {
            try
            {
                //if( dsDirect.Tables[ "dtBudgetDep" ].GetChanges() != null )
                //{
                    if( System.Windows.Forms.MessageBox.Show( this, "Отменить все изменения?", "Подтверждение",
                   System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question ) == DialogResult.Yes )
                    { CancelChanges(); }
                //}
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка отмены изменений\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// Заполняет список измененных бюджетных подразделений для последующего сохранения в БД
        /// </summary>
        /// <param name="dtChanges">таблица со списком измененных записей</param>
        /// <param name="objList">список объектов "бюджетные подразделения"</param>
        /// <param name="objTreeNode">узел дерева, для которго производится поиск дочерних узлов, подвергнувшихся изменениям </param>
        private void FillChangesNodesList( System.Data.DataTable dtChanges,
            ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList,
            DevExpress.XtraTreeList.Nodes.TreeListNode objTreeNode )
        {
            try
            {
                if( dtChanges.Rows.Count == 0 ) { return; }
                if( objTreeNode == null ) { return; }
                if( objList == null ) { return; }

                DevExpress.XtraTreeList.Nodes.TreeListNode tmpNextNode;
                DevExpress.XtraTreeList.Nodes.TreeListNode tmpTreeNode;
                tmpTreeNode = objTreeNode;
                while( tmpTreeNode != null )
                {
                    // проверяем, числится ли узел в списке изменений
                    foreach( System.Data.DataRow row in dtChanges.Rows )
                    {
                        if( ( row.RowState == DataRowState.Added ) || ( row.RowState == DataRowState.Modified ) )
                        {
                            // добавленный или измененнная запись
                            if( ( ( System.Guid )row[ "BUDGETDEP_GUID_ID" ] ).CompareTo( ( ( System.Guid )tmpTreeNode.GetValue( colGuid_ID ) ) ) == 0 )
                            {
                                // узел числится в списке изменений
                                // создаем объект "бюджетное подразделение" и добавляем его в список
                                ERP_Budget.Common.CBudgetDep objBudgetDep = new ERP_Budget.Common.CBudgetDep();
                                objBudgetDep.uuidID = ( System.Guid )row[ "BUDGETDEP_GUID_ID" ];
                                if( row[ "BUDGETDEP_PARENT_GUID_ID" ] != System.DBNull.Value )
                                {
                                    if( ( ( System.Guid )row[ "BUDGETDEP_PARENT_GUID_ID" ] ).CompareTo( System.Guid.Empty ) != 0 )
                                    { objBudgetDep.ParentID = ( System.Guid )row[ "BUDGETDEP_PARENT_GUID_ID" ]; }
                                }
                                objBudgetDep.State = row.RowState;
                                objBudgetDep.Name = ( System.String )row[ "BUDGETDEP_NAME" ];
                                ERP_Budget.Common.CUser objManager = new ERP_Budget.Common.CUser();
                                objManager.ulID = ( System.Int32 )row[ "BUDGETDEP_MANAGER_ID" ];
                                objBudgetDep.Manager = objManager;

                                // список сотрудников
                                objBudgetDep.UsesrList.ClearList();
                                if( row[ "BUDGETDEP_DECLRN" ] != System.DBNull.Value )
                                {
                                    if( ( ( System.String )tmpTreeNode.GetValue( colBudgetDeclrn ) ).Length > 0 )
                                    {
                                        ERP_Budget.Common.CUser objUser = null;
                                        // нужно разложить строку на состовляющие с именами сотрудников
                                        System.String strUserList = ( System.String )tmpTreeNode.GetValue( colBudgetDeclrn );
                                        System.String strUserName = "";
                                        System.String strAppendix = "";
                                        System.Int32 iIndexOf = 0;
                                        while( strUserList.Length > 0 )
                                        {
                                            objUser = null;
                                            iIndexOf =  strUserList.IndexOf( ";" );
                                            if( iIndexOf > 0 )
                                            {
                                                strUserName = strUserList.Substring( 0, strUserList.IndexOf( ";" ) );
                                            }
                                            else
                                            {
                                                strUserName = strUserList;
                                            }
                                            objUser = GetUserByName( strUserName );
                                            if( objUser != null )
                                            {
                                                objBudgetDep.UsesrList.AddItemToList( objUser );
                                                strAppendix = "";
                                                if( iIndexOf > 0 )
                                                {
                                                    strAppendix = strUserList.Substring( iIndexOf + 1 );
                                                }
                                                else
                                                {
                                                    strAppendix = "";
                                                }
                                                strUserList = strAppendix;
                                            }
                                        }
                                        
                                    }
                                }

                                // добавляем объект "статья расходов" в список
                                objList.AddItemToList( objBudgetDep );
                                // выходим из цикла
                                break;
                            }
                        }
                    }
                    FillChangesNodesList( dtChanges, objList, tmpTreeNode.FirstNode );

                    // находим следующий узел дерева
                    tmpNextNode = tmpTreeNode.NextNode;
                    tmpTreeNode = tmpNextNode;
                }

            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка отмены изменений\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void FillChangesNodesList2(System.Data.DataTable dtChanges,
            ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList,
            DevExpress.XtraTreeList.Nodes.TreeListNode objTreeNode)
        {
            try
            {
                if (dtChanges.Rows.Count == 0) { return; }
                if (objTreeNode == null) { return; }
                if (objList == null) { return; }

                if (objTreeNode != null)
                {
                    // проверяем, числится ли узел в списке изменений
                    foreach (System.Data.DataRow row in dtChanges.Rows)
                    {
                        if ((row.RowState == DataRowState.Added) || (row.RowState == DataRowState.Modified))
                        {
                            // добавленный или измененнная запись
                            if (((System.Guid)row["BUDGETDEP_GUID_ID"]).CompareTo(((System.Guid)objTreeNode.GetValue(colGuid_ID))) == 0)
                            {
                                // узел числится в списке изменений
                                // создаем объект "бюджетное подразделение" и добавляем его в список
                                ERP_Budget.Common.CBudgetDep objBudgetDep = new ERP_Budget.Common.CBudgetDep();
                                objBudgetDep.uuidID = (System.Guid)row["BUDGETDEP_GUID_ID"];
                                if (row["BUDGETDEP_PARENT_GUID_ID"] != System.DBNull.Value)
                                {
                                    if (((System.Guid)row["BUDGETDEP_PARENT_GUID_ID"]).CompareTo(System.Guid.Empty) != 0)
                                    { objBudgetDep.ParentID = (System.Guid)row["BUDGETDEP_PARENT_GUID_ID"]; }
                                }
                                objBudgetDep.State = row.RowState;
                                objBudgetDep.Name = (System.String)row["BUDGETDEP_NAME"];
                                ERP_Budget.Common.CUser objManager = new ERP_Budget.Common.CUser();
                                objManager.ulID = (System.Int32)row["BUDGETDEP_MANAGER_ID"];
                                objBudgetDep.Manager = objManager;

                                // список сотрудников
                                objBudgetDep.UsesrList.ClearList();
                                if (row["BUDGETDEP_DECLRN"] != System.DBNull.Value)
                                {
                                    if (((System.String)objTreeNode.GetValue(colBudgetDeclrn)).Length > 0)
                                    {
                                        ERP_Budget.Common.CUser objUser = null;
                                        // нужно разложить строку на состовляющие с именами сотрудников
                                        System.String strUserList = (System.String)objTreeNode.GetValue(colBudgetDeclrn);
                                        System.String strUserName = "";
                                        System.String strAppendix = "";
                                        System.Int32 iIndexOf = 0;
                                        while (strUserList.Length > 0)
                                        {
                                            objUser = null;
                                            iIndexOf = strUserList.IndexOf(";");
                                            if (iIndexOf > 0)
                                            {
                                                strUserName = strUserList.Substring(0, strUserList.IndexOf(";"));
                                            }
                                            else
                                            {
                                                strUserName = strUserList;
                                            }
                                            objUser = GetUserByName(strUserName);
                                            if (objUser != null)
                                            {
                                                objBudgetDep.UsesrList.AddItemToList(objUser);
                                            }

                                            strAppendix = "";
                                            if (iIndexOf > 0)
                                            {
                                                //strAppendix = strUserList.Substring(iIndexOf + 1);
                                                strAppendix = strUserList.Substring(iIndexOf + 1, (strUserList.Length - (iIndexOf + 1)));
                                            }
                                            else
                                            {
                                                strAppendix = "";
                                            }
                                            strUserList = strAppendix;
                                        }

                                    }
                                }

                                // добавляем объект "статья расходов" в список
                                objList.AddItemToList(objBudgetDep);
                            }
                        }
                    }
                }

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка отмены изменений\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        
        /// <summary>
        /// Возвращает проинициализированный объект "Сотрудник" по его имени
        /// </summary>
        /// <param name="strBudgetDepName">имя сотрудника</param>
        /// <returns>объект "Сотрудник"</returns>
        private ERP_Budget.Common.CUser GetUserByName( System.String strUserName )
        {
            ERP_Budget.Common.CUser objRet = null;
            try
            {
                if (m_objUserList != null)
                {
                    objRet = m_objUserList.SingleOrDefault<ERP_Budget.Common.CUser>(x => x.UserFullName == strUserName);
                }
                
                //if( dsDirect.Tables[ "dtUser" ].Rows.Count > 0 )
                //{
                //    foreach( System.Data.DataRow row in dsDirect.Tables[ "dtUser" ].Rows )
                //    {
                //        if( ( ( System.String )row[ "USER_NAME" ] ).CompareTo( strUserName ) == 0 )
                //        {
                //            objRet = new ERP_Budget.Common.CUser();
                //            objRet.ulID = ( System.Int32 )row[ "USER_ID" ];
                //            objRet.Name = ( System.String )row[ "USER_NAME" ];
                //            break;
                //        }
                //    }
                //}
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка поиска сотрудника\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return objRet;
        }
        /// <summary>
        /// Создает список измененных бюджетных подразделений для последующего сохранения в БД
        /// </summary>
        /// <returns></returns>
        private ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> CreateChangesNodesList()
        {
            ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList = new ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep>();
            objList.ClearList();
            try
            {
                // проверим, были изменения в дереве или нет
                System.Data.DataTable dtChanges = dsDirect.Tables[ "dtBudgetDep" ].GetChanges();
                if( dtChanges == null ) { return objList; }

                // заполняем список измененых узлов
                FillChangesNodesList2( dtChanges, objList, treeList.FocusedNode );

                //// заполняем список измененых узлов
                //FillChangesNodesList(dtChanges, objList, treeList.Nodes[0]);
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка формирования списка измененных бюджетных подразделений\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return objList;
        }
             

        /// <summary>
        /// Сохраняет изменения в списке бюжетных подразделений в БД
        /// </summary>
        /// <returns></returns>
        private System.Boolean SaveChangesToDB()
        {
            System.Boolean bRes = false;
            try
            {
                System.Data.DataTable dtChanges = dsDirect.Tables[ "dtBudgetDep" ].GetChanges();
                if( dtChanges == null ) { return true; }
                // изменения производились, сформируем список бюжетных подразделений для сохранения в БД
                ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList = CreateChangesNodesList();

                // список заполнен - попробуем сохранить изменения в БД
                if( ERP_Budget.Common.CBudgetDep.SaveBudgetDepListToDB( objList, m_objProfile ) == true )
                {
                    dsDirect.AcceptChanges();
                    // делаем пометку об изменениях в дереве
                    SetModifiedToFalse();
                    bRes = true;
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка сохранения изменений в БД\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return bRes;
        }

        private void btnSave_Click( object sender, EventArgs e )
        {
            try
            {
                // если выбранный узел находится в режиме редактирования, то закончим этот процесс
                if( treeList.FocusedNode != null )
                {
                    // прописываем состав бюджетного подразделения в выделенном узле
                    SetBudgetDepDeclrn( treeList.FocusedNode );
                    // проверяем данные в выделенном узле
                    if( bIsNodeValuesValidation( treeList.FocusedNode, true ) == false ) { return; }
                }
                if( SaveChangesToDB() == false )
                {
                    System.Windows.Forms.MessageBox.Show( this, "Не удалось сохранить изменения в базе данных", "Ошибка",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка сохранения изменений\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
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

        private void frmDebitArticle_Load( object sender, EventArgs e )
        {
        }

        private void treeList_AfterFocusNode( object sender, DevExpress.XtraTreeList.NodeEventArgs e )
        {
            try
            {
                if( treeList.Nodes.Count == 0 ) { return; }
                if( treeList.FocusedNode == null ) { return; }
                OnSelectItem();
                treeList.OptionsBehavior.Editable = true;
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка treeList_AfterFocusNode\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// Проверяет узел на предмет ошибок в данных
        /// </summary>
        /// <param name="objNode">узел дерева</param>
        /// <param name="bShowMessage">признак того, выводить сообщения или нет</param>
        /// <returns></returns>
        private System.Boolean bIsNodeValuesValidation( DevExpress.XtraTreeList.Nodes.TreeListNode objNode, 
            System.Boolean bShowMessage )
        {
            System.Boolean bRet = false;
            try
            {
                // найдем строку в наборе данных, соответствующую выбранному узлу
                System.Guid uuidID = ( System.Guid )objNode.GetValue( colGuid_ID );
                System.Int32 iRecNum = -1;
                for( System.Int32 i = 0; i < dsDirect.Tables[ "dtBudgetDep" ].Rows.Count; i++ )
                {
                    if( dsDirect.Tables[ "dtBudgetDep" ].Rows[ i ].RowState != DataRowState.Deleted )
                    {
                        if( ( ( System.Guid )dsDirect.Tables[ "dtBudgetDep" ].Rows[ i ][ "BUDGETDEP_GUID_ID" ] ).CompareTo( uuidID ) == 0 )
                        {
                            iRecNum = i;
                            break;
                        }
                    }
                }
                if( iRecNum >= 0 )
                {
                    dsDirect.Tables[ "dtBudgetDep" ].Rows[ iRecNum ].EndEdit();
                    // проверим, указано ли название
                    if( ( ( System.String )dsDirect.Tables[ "dtBudgetDep" ].Rows[ iRecNum ][ "BUDGETDEP_NAME" ] ) == "" )
                    {
                        if( bShowMessage )
                        {
                            System.Windows.Forms.MessageBox.Show( this, "Необходимо указать наименование бюджетного подразделения.", "Предупреждение",
                               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning );
                        }
                        return bRet;
                    }
                    // проверим, указан ли БОСС
                    if( dsDirect.Tables[ "dtBudgetDep" ].Rows[ iRecNum ][ "BUDGETDEP_MANAGER_ID" ] == System.DBNull.Value )
                    {
                        if( bShowMessage )
                        {
                            System.Windows.Forms.MessageBox.Show( this, "Необходимо указать руководителя бюджетного подразделения.", "Предупреждение",
                               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning );
                        }
                        return bRet;
                    }
                    bRet = true;
                }
            }
            catch( System.Exception f )
            {
                if( bShowMessage )
                {
                    System.Windows.Forms.MessageBox.Show( this, "Ошибка проверки значений в узле\n" + f.Message, "Ошибка",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                }
            }
            return bRet;
        }

        private void treeList_BeforeFocusNode( object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e )
        {
            try
            {
                // перед тем как выделить новый узел проверим, все ли правильно в текущем выделенном узле
                if( treeList.Nodes.Count == 0 ) { return; }
                if( e.OldNode == null ) { return; }

                // пропишем занчение столбца "Состав бюджетного подразделения"
                SetBudgetDepDeclrn( e.OldNode );

                e.CanFocus = bIsNodeValuesValidation( e.OldNode, true );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка treeList_AfterFocusNode\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        private void frmBudgetDep_FormClosing( object sender, FormClosingEventArgs e )
        {
            try
            {
                if( m_bIsTreeModified )
                {
                    if( System.Windows.Forms.MessageBox.Show( this,
                        "Список бюджетных подразделений был изменен.\nВыйти из формы без сохранения изменений?", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                    // запускаем процесс сохранения
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "frmBudgetDep_FormClosing\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// Прописывает значение столбца "Состав подразделения"
        /// </summary>
        /// <param name="objNode"></param>
        private void SetBudgetDepDeclrn( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            try
            {
                System.String strOldBudgetDeclrn = ( System.String )objNode.GetValue( colBudgetDeclrn );
                System.Int32 iCheckedCount = checklboxBudgetDepDclrn.CheckedItems.Count;
                System.Int32 iComaCount = ( iCheckedCount > 0 ) ? ( iCheckedCount - 1 ) : 0;
                System.String strRes = "";
                for( System.Int32 i = 0; i < checklboxBudgetDepDclrn.Items.Count; i++ )
                {
                    if( checklboxBudgetDepDclrn.Items[ i ].CheckState == CheckState.Checked )
                    {
                        strRes += checklboxBudgetDepDclrn.Items[ i ].Value.ToString();
                        if( iComaCount > 0 )
                        {
                            strRes += ";";
                            iComaCount--;
                        }
                    }
                }
                if( strOldBudgetDeclrn != strRes )
                {
                    objNode.SetValue( colBudgetDeclrn, strRes );
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "Ошибка SetBudgetDepDeclrn\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        private void checklboxBudgetDepDclrn_ValueMemberChanged( object sender, EventArgs e )
        {
        }

        private void checklboxBudgetDepDclrn_ItemCheck( object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e )
        {
            SetModifiedToTrue();
        }

        private void mitemManagerList_Click(object sender, EventArgs e)
        {
            ShowManagerList();
        }

        private void treeList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

        }

        private void barBtnMaagerList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowManagerList();

        }

        private void ShowManagerList()
        {
            if (treeList.Nodes.Count == 0) { return; }
            if (treeList.FocusedNode == null) { return; }
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode != null )
                {
                    using (ERP_Budget.Common.frmBudgetDepManagerList objfrmBudgetDepManagerList = new ERP_Budget.Common.frmBudgetDepManagerList(m_objProfile))
                    {
                        if (objfrmBudgetDepManagerList != null)
                        {
                            objfrmBudgetDepManagerList.LoadBudgetDepManagerList((System.Guid)objFocusedNode.GetValue(colGuid_ID), 
                                System.Convert.ToString( objFocusedNode.GetValue( colBudgetDepName ) )); 
                            objfrmBudgetDepManagerList.ShowDialog();
                        }
                    }

                    //frmBudgetDepManagerList objfrmBudgetDepManagerList = new frmBudgetDepManagerList(m_objProfile,
                    //    (System.Guid)objFocusedNode.GetValue(colGuid_ID));
                    //if (objfrmBudgetDepManagerList != null)
                    //{
                    //    objfrmBudgetDepManagerList.ShowDialog();
                    //    objfrmBudgetDepManagerList.Dispose();
                    //}
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "frmBudgetDep_FormClosing\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void frmBudgetDep_Shown(object sender, EventArgs e)
        {
            try
            {
                // загружаем список
                bLoadBudgetDeps();

                //if (treeList.Nodes.Count > 0)
                //{
                //    treeList.ExpandAll();
                //    treeList.BestFitColumns();
                //}
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка открытия формы\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }


    }

    public class ViewBudgetDep : PlugIn.IClassTypeView
    {
        public override void Run( UniXP.Common.MENUITEM objMenuItem, System.String strCaption )
        {
            frmBudgetDep obj = new frmBudgetDep( objMenuItem.objProfile );
            obj.Text = strCaption;
            obj.MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent;
            obj.Visible = true;
        }
    }

}