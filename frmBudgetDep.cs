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
        #region ����������, ��������, ���������
        UniXP.Common.CProfile m_objProfile;
        List<ERP_Budget.Common.CUser> m_objUserList;
        #endregion

        public frmBudgetDep( UniXP.Common.CProfile objProfile )
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objUserList = null;
            //m_bDREditRootDebitArticle = false;
            // �������� ������������ ����
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

        #region ������������ �����
        //private System.Boolean m_bDREditRootDebitArticle;
        private void SetAccessForDynamicRights()
        {
            try
            {
                //// �������������� ������ ��������
                //m_bDREditRootDebitArticle = ( m_objProfile.GetClientsRight() ).GetState( ERP_Budget.Global.Consts.strDREditRootDebitArticle );
                //if( m_bDREditRootDebitArticle == false )
                //{
                //    // ��� ���� �� ������ �� �������� ��������
                //    barBtnAddNode.Enabled = m_bDREditRootDebitArticle;
                //    mitemAddRoot.Enabled = m_bDREditRootDebitArticle;
                //}
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "������ �������� ������������ ����\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        #endregion

        #region ��������� ��������� � ������

        private System.Boolean m_bIsTreeModified;
        /// <summary>
        /// ������������� ��������� "�������� ������" � true
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
                System.Windows.Forms.MessageBox.Show( this, "������ SetModifiedToTrue()\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// ������������� ��������� "�������� ������" � false
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
                System.Windows.Forms.MessageBox.Show( this, "������ SetModifiedToFalse()\n" + f.Message, "������",
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
                System.Windows.Forms.MessageBox.Show( this, "������ treeList_CellValueChanged\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        #endregion

        #region ���������� ������ ��������� �������������

        /// <summary>
        /// ��������� ������ ��������� �������������
        /// </summary>
        /// <returns>true - �������; false - ������</returns>
        private System.Boolean bLoadBudgetDeps()
        {
            System.Boolean bRes = false;
            Cursor = Cursors.WaitCursor;
            
            try
            {
                // ���������� �������� ������ ��������� �������������
                List<ERP_Budget.Common.CBudgetDep> objBudgetDepList = ERP_Budget.Common.CBudgetDep.GetBudgetDepsList( m_objProfile, true );
                if( objBudgetDepList == null ) { return bRes; }

                // ��������� ����� ������, ��������� � �������
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
                        // ��������� ������ � �������� ���������� �������������
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
                // ��������� ������ �����������
                bLoadUsers();
                // ������������ ��������� � ������ ������
                dsDirect.AcceptChanges();
                bRes = true;
            }//try
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "������ ������ ������ ��������� �������������\n" + e.Message, "������",
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
        /// ��������� ������ �����������
        /// </summary>
        /// <returns></returns>
        private System.Boolean bLoadUsers()
        {
            System.Boolean bRes = false;
            try
            {
                // ���������� �������� ������ �����������
                ERP_Budget.Common.CUser objUser = new ERP_Budget.Common.CUser();
                m_objUserList = objUser.GetBudgetUserList(m_objProfile, true, false, false);
                if (m_objUserList == null) { return bRes; }

                // ��������� ����� ������, ��������� � �������
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
                // ���������� ������ �����������
                this.repItemUsersList.DataSource = this.dtUser;
                this.repItemUsersList.DisplayMember = "USER_NAME";
                this.repItemUsersList.ValueMember = "USER_ID";
                this.repItemUsersList.Columns.Clear();
                this.repItemUsersList.Columns.AddRange( new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
		            new DevExpress.XtraEditors.Controls.LookUpColumnInfo( "USER_NAME", "���������", 100, 
                        DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, 
                        DevExpress.Data.ColumnSortOrder.None ) } );

                bRes = true;
            }//try
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "������ ������ ������ �����������\n" + e.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            finally
            {
            }

            return bRes;
        }
        /// <summary>
        /// ��������� ������ ��������� �������������
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
                            "� ������ ��������� ������������� ���� ����������� ���������.\n� ������ ���������� ��� ��������� ����� ��������.\n�������� ������?",
                            "��������", System.Windows.Forms.MessageBoxButtons.YesNo,
                            System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                        { return; }
                    }
                }
                bLoadBudgetDeps();
            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "������ ���������� ������ ��������� �������������\n" + e.Message, "������",
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
                    "������ ���������� ������ ��������� �������������\n" + f.Message, "������",
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
                    "������ ���������� ������ ��������� �������������\n" + f.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        #endregion

        #region ��������, ������� ����
        /// <summary>
        /// ��������� ���� � ������
        /// </summary>
        /// <param name="objParentNode">������������ ����</param>
        private void AddNode( DevExpress.XtraTreeList.Nodes.TreeListNode objParentNode )
        {
            try
            {
                // ���� � ������ ���� ���� � ���� �� ��� �������, ���������� ��������� ��� �������� �� ������� ������
                if( treeList.Nodes.Count > 0 )
                {
                    if( ( treeList.FocusedNode != null ) && ( treeList.FocusedNode.Selected ) )
                    {
                        if( bIsNodeValuesValidation( treeList.FocusedNode, true ) == false ) { return; }
                    }
                }
                //����������� �������� ������ ����
                System.Guid newuuidID = System.Guid.NewGuid();
                if( objParentNode == null )
                {
                    System.Data.DataRow newRow = dsDirect.Tables[ "dtBudgetDep" ].NewRow();
                    newRow[ "BUDGETDEP_GUID_ID" ] = newuuidID;
                    newRow[ "BUDGETDEP_PARENT_GUID_ID" ] = System.Guid.Empty;
                    newRow[ "BUDGETDEP_NAME" ] = "�������������";
                    newRow[ "READONLY" ] = false;
                    newRow[ "BUDGETDEP_DECLRN" ] = "";
                    dsDirect.Tables[ "dtBudgetDep" ].Rows.Add( newRow );
                }
                else
                {
                    // ���������� ��������� ���� - ��������� ��������
                    System.Data.DataRow newRow = dsDirect.Tables[ "dtBudgetDep" ].NewRow();
                    newRow[ "BUDGETDEP_GUID_ID" ] = newuuidID;
                    newRow[ "BUDGETDEP_PARENT_GUID_ID" ] = ( System.Guid )objParentNode.GetValue( colGuid_ID );
                    newRow[ "BUDGETDEP_NAME" ] = "�������������";
                    newRow[ "READONLY" ] = false;
                    newRow[ "BUDGETDEP_DECLRN" ] = "";
                    dsDirect.Tables[ "dtBudgetDep" ].Rows.Add( newRow );
                }

                SetModifiedToTrue();
            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "������ ���������� ����\n" + e.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        /// <summary>
        /// ������� ���� � ������
        /// </summary>
        /// <param name="objNode">��������� ����</param>
        private void DeleteNode( DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            try
            {
                if( objNode == null ){ return; }
                // ���������, ����� �� ������� ����
                if( IsPossibleDeleteNode( objNode ) == false )
                {
                    System.Windows.Forms.MessageBox.Show( this,
                        "��������� �������������/�������� ������������� ������� ������.\n�� ���� ���� ������ � ������� ���� ��������� ���������.", "��������!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning );
                    return;
                }
                // ����������� ������ �����, ���������� ��������
                ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList = CreateNodeListForDelete( objNode );
                if( objList.GetCountItems() > 0 )
                {
                    // ������� ���������� �� ���� �� ��
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
                    "������ �������� ����\n" + e.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        /// <summary>
        /// ��������� ����� �� ������� ����,
        /// ��� �� � �������� ����� �������� "������ ������"
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
                // ��������� �������� ����
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
                    "������ �������� ����������� �������� ����\n" + e.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return bRes;
        }
        /// <summary>
        /// ��������� ������ � ��������� "��������� �������������", ���������� ��������
        /// </summary>
        /// <param name="objList">����������� ������</param>
        /// <param name="objNode">���� ������</param>
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
                // ��������� �������� ����
                for( System.Int32 i = 0; i < objNode.Nodes.Count; i++ )
                {
                    FillNodeListForDelete( objList, objNode.Nodes[ i ] );
                }
            }
            catch( System.Exception e )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "������ ���������� ������ ����� ���������� ��������\n" + e.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return ;
        }
        /// <summary>
        /// ������� ������ � ��������� "��������� �������������", ���������� ��������
        /// </summary>
        /// <param name="objDelNode">��������� ����</param>
        /// <returns>������ �������� "��������� �������������"</returns>
        private ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> CreateNodeListForDelete( DevExpress.XtraTreeList.Nodes.TreeListNode objDelNode )
        {
            ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList = new ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep>();
            objList.ClearList();
            try
            {
                if( objDelNode == null ) { return objList; }
                // ��������� ������ ����� ��� ��������
                FillNodeListForDelete( objList, objDelNode );
                // ������ ���������� ����������� ������� � ������ � �������� �������
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
                    "������ �������� ������ ����� ���������� ��������\n" + e.Message, "������",
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
                    "������ ���������� ���������� �������������\n" + f.Message, "������",
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
                    "������ ���������� ���������� �������������\n" + f.Message, "������",
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
                    "������ ���������� ���������� �������������\n" + f.Message, "������",
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
                    "������ ���������� ���������� �������������\n" + f.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        private void DeleteNode()
        {
            try
            {
                if( treeList.FocusedNode == null ) { return; }

                // ��������� �������������� �� ������ �������� ����
                System.String strQuestion = "������� " + ( ( treeList.FocusedNode.ParentNode == null ) ? "��������� �������������" : " �������� ��������� �������������" ) + "?\n������ �������� ����� ����������!";
                if( System.Windows.Forms.MessageBox.Show( this, strQuestion, "�������������",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                { return; }

                // ������� ����
                DeleteNode( treeList.FocusedNode );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "������ �������� ���������� �������������\n" + f.Message, "������",
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
                    "������ �������� ���������� �������������\n" + f.Message, "������",
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
                    "������ �������� ���������� �������������\n" + f.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }

        #endregion

        #region �������������� �����
        /// <summary>
        /// ������������� ������� ��� ������� "��-���������"
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
                    "������ ��������� �������\n" + f.Message, "������",
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
                    "������ 'SetDragCursor'\n" + f.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// ���������� ��������������� ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DevExpress.XtraTreeList.Nodes.TreeListNode GetDragNode( System.Windows.Forms.IDataObject data )
        {
            return data.GetData( typeof( DevExpress.XtraTreeList.Nodes.TreeListNode ) ) as DevExpress.XtraTreeList.Nodes.TreeListNode;
        }
        /// <summary>
        /// ���������� �������������� (��������� �����)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_DragDrop( object sender, DragEventArgs e )
        {
            try
            {
                DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo( treeList.PointToClient( new Point( e.X, e.Y ) ) );
                // ����, ������� �������������
                DevExpress.XtraTreeList.Nodes.TreeListNode draggedNode = 
                    ( DevExpress.XtraTreeList.Nodes.TreeListNode )e.Data.GetData( typeof( DevExpress.XtraTreeList.Nodes.TreeListNode ) );
                if( ( draggedNode != null ) && ( ( e.KeyState & 4) != 4 ) )
                {
                    //����, ��� ������� ��������� �����
                    DevExpress.XtraTreeList.Nodes.TreeListNode node = hi.Node;
                    if( node != null )
                    {
                        System.Guid uuidID = ( System.Guid )draggedNode.GetValue( colGuid_ID );
                        System.Guid uuidParentID = ( System.Guid )node.GetValue( colGuid_ID );

                        // ���������� ����
                        treeList.MoveNode( draggedNode, node );
                        // �������� ��������� �� �������� � ���������������� ����
                        foreach( System.Data.DataRow row in dsDirect.Tables[ "dtBudgetDep" ].Rows )
                        {
                            if( ( ( System.Guid )row[ "BUDGETDEP_GUID_ID" ] ).CompareTo( uuidID ) == 0 )
                            {
                                row[ "BUDGETDEP_PARENT_GUID_ID" ] = uuidParentID;
                                break;
                            }
                        }
                        // ������ �������, ��� ��������� ���������
                        SetModifiedToTrue();
                    }
                }
                SetDefaultCursor();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this,
                    "������ treeList_DragDrop\n" + f.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// ��������������� ������ ����� �� ������� treeList
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
                    "������ 'treeList_DragLeave'\n" + f.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// ������ ��������������� ��� treeList
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
                    "������ 'treeList_DragOver'\n" + f.Message, "������",
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
                    "������ 'treeList_GiveFeedback'\n" + f.Message, "������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        #endregion

        #region ������ ���������� �������

        /// <summary>
        /// ����������� ���� ������ ���� �����
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

                // �������� ��������� �� �������� � ���������������� ����
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
                // ������ ������� �� ���������� � ������
                SetModifiedToTrue();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnLeft\n" + f.Message, "������",
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
                System.Windows.Forms.MessageBox.Show( this, "������ ��� ����������� ���� �����\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        /// <summary>
        /// ����������� ���� ������ ���� ������
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

                // ���������� ����
                treeList.MoveNode( objFocusedNode, objPrevFocusedNode );
                // �������� ��������� �� �������� � ���������������� ����
                foreach( System.Data.DataRow row in dsDirect.Tables[ "dtBudgetDep" ].Rows )
                {
                    if( ( ( System.Guid )row[ "BUDGETDEP_GUID_ID" ] ).CompareTo( uuidID ) == 0 )
                    {
                        row[ "BUDGETDEP_PARENT_GUID_ID" ] = uuidParentID;
                        break;
                    }
                }
                //objFocusedNode.SetValue( colParentGuid_ID, ( System.Guid )objFocusedNode.ParentNode.GetValue( colGuid_ID ) );
                // ������ ������� �� ���������� � ������
                SetModifiedToTrue();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnRight\n" + f.Message, "������",
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
                System.Windows.Forms.MessageBox.Show( this, "������ ��� ����������� ���� ������\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        /// <summary>
        /// ����������� ���� ������ ���� ����
        /// </summary>
        private void OnDown( )
        {
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode == null ) { return; }
                DevExpress.XtraTreeList.Nodes.TreeListNode objNexFocusedNode = objFocusedNode.NextNode;
                if( objNexFocusedNode == null ) { return; }
                // ���������� ����
                System.Int32 iFocusedNodeIndx = treeList.GetNodeIndex( objFocusedNode ); 
                System.Int32 iNexFocusedNodeIndx = treeList.GetNodeIndex( objNexFocusedNode );
                treeList.SetNodeIndex( objFocusedNode, iNexFocusedNodeIndx );
                treeList.SetNodeIndex( objNexFocusedNode, iFocusedNodeIndx );
                treeList.FocusedNode = objFocusedNode;
                // ������ ������� �� ���������� � ������
                SetModifiedToTrue();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnDown\n" + f.Message, "������",
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
                System.Windows.Forms.MessageBox.Show( this, "������ ��� ����������� ���� ����\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        /// <summary>
        /// ����������� ���� ������ ���� �����
        /// </summary>
        private void OnUp( )
        {
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode == null ) { return; }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevFocusedNode = objFocusedNode.PrevNode;
                if( objPrevFocusedNode == null ) { return; }
                // ���������� ����
                System.Int32 iFocusedNodeIndx = treeList.GetNodeIndex( objFocusedNode );
                System.Int32 iPrevFocusedNodeIndx = treeList.GetNodeIndex( objPrevFocusedNode );
                treeList.SetNodeIndex( objFocusedNode, iPrevFocusedNodeIndx );
                treeList.SetNodeIndex( objPrevFocusedNode, iFocusedNodeIndx );
                treeList.FocusedNode = objPrevFocusedNode;
                // ������ ������� �� ���������� � ������
                SetModifiedToTrue();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "OnUp\n" + f.Message, "������",
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
                System.Windows.Forms.MessageBox.Show( this, "������ ��� ����������� ���� �����\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// ��������� ���� ������ ����
        /// </summary>
        private void OnSelectItem()
        {
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objFocusedNode = treeList.FocusedNode;
                if( objFocusedNode == null )
                {
                    // ������ �� ��������
                    btnLeft.Enabled = false;
                    btnRight.Enabled = false;
                    btnUp.Enabled = false;
                    btnDown.Enabled = false;
                    btnDrop.Enabled = false;
                }
                else
                {
                    // ���� �������
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
                    // ���� ��� ���� �� �������������� ������ � �������� ������, ����� ������ "�������"
                    btnDrop.Enabled = true; // ( objFocusedNode.ParentNode == null ) ? false : true;
                    barBtnDeleteNode.Enabled = btnDrop.Enabled;

                    // ������ ������ � ������ ������, ��������������� ���������� ����
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
                System.Windows.Forms.MessageBox.Show( this, "OnSelectItem\n" + f.Message, "������",
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
                    // ��������� ����������, ��� �� � ��� ��� ������
                    DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo( new Point( e.X, e.Y ) );
                    if( hi == null ) { return; }
                    if( hi.Node == null ) { return; }
                    // �������� ����
                    hi.Node.TreeList.FocusedNode = hi.Node;
                    // ��������� ����������� ����
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
                System.Windows.Forms.MessageBox.Show( this, "������ 'treeList_MouseClick'\n" + f.Message, "������",
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
                        // Ctrl - �����
                        case System.Windows.Forms.Keys.Left:
                        if( btnLeft.Enabled )
                        {
                            OnLeft();
                            OnSelectItem();
                        }
                        break;
                        // Ctrl - ������
                        case System.Windows.Forms.Keys.Right:
                        if( btnRight.Enabled )
                        {
                            OnRight();
                            OnSelectItem();
                        }
                        break;
                        // Ctrl - �����
                        case System.Windows.Forms.Keys.Up:
                        if( btnUp.Enabled )
                        {
                            OnUp();
                            OnSelectItem();
                        }
                        break;
                        // Ctrl - ����
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
                            // ��������� �������������� �� ������ �������� ����
                            System.String strQuestion = "������� " + ( ( treeList.FocusedNode.ParentNode == null ) ? "������" : "���������" ) + "?";
                            if( System.Windows.Forms.MessageBox.Show( this, strQuestion, "�������������",
                                System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                            { break; }
                            // ������� ����
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
                System.Windows.Forms.MessageBox.Show( this, "������ ��������� ������� �������\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }



        #endregion

        #region ����������, ������ ��������� � ��
        /// <summary>
        /// ������ ��������� ���������
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
                // ������ ������� �� ���������� � ������
                SetModifiedToFalse();
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "������ ������ ���������\n" + f.Message, "������",
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
                    if( System.Windows.Forms.MessageBox.Show( this, "�������� ��� ���������?", "�������������",
                   System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question ) == DialogResult.Yes )
                    { CancelChanges(); }
                //}
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "������ ������ ���������\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// ��������� ������ ���������� ��������� ������������� ��� ������������ ���������� � ��
        /// </summary>
        /// <param name="dtChanges">������� �� ������� ���������� �������</param>
        /// <param name="objList">������ �������� "��������� �������������"</param>
        /// <param name="objTreeNode">���� ������, ��� ������� ������������ ����� �������� �����, ��������������� ���������� </param>
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
                    // ���������, �������� �� ���� � ������ ���������
                    foreach( System.Data.DataRow row in dtChanges.Rows )
                    {
                        if( ( row.RowState == DataRowState.Added ) || ( row.RowState == DataRowState.Modified ) )
                        {
                            // ����������� ��� ����������� ������
                            if( ( ( System.Guid )row[ "BUDGETDEP_GUID_ID" ] ).CompareTo( ( ( System.Guid )tmpTreeNode.GetValue( colGuid_ID ) ) ) == 0 )
                            {
                                // ���� �������� � ������ ���������
                                // ������� ������ "��������� �������������" � ��������� ��� � ������
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

                                // ������ �����������
                                objBudgetDep.UsesrList.ClearList();
                                if( row[ "BUDGETDEP_DECLRN" ] != System.DBNull.Value )
                                {
                                    if( ( ( System.String )tmpTreeNode.GetValue( colBudgetDeclrn ) ).Length > 0 )
                                    {
                                        ERP_Budget.Common.CUser objUser = null;
                                        // ����� ��������� ������ �� ������������ � ������� �����������
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

                                // ��������� ������ "������ ��������" � ������
                                objList.AddItemToList( objBudgetDep );
                                // ������� �� �����
                                break;
                            }
                        }
                    }
                    FillChangesNodesList( dtChanges, objList, tmpTreeNode.FirstNode );

                    // ������� ��������� ���� ������
                    tmpNextNode = tmpTreeNode.NextNode;
                    tmpTreeNode = tmpNextNode;
                }

            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "������ ������ ���������\n" + f.Message, "������",
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
                    // ���������, �������� �� ���� � ������ ���������
                    foreach (System.Data.DataRow row in dtChanges.Rows)
                    {
                        if ((row.RowState == DataRowState.Added) || (row.RowState == DataRowState.Modified))
                        {
                            // ����������� ��� ����������� ������
                            if (((System.Guid)row["BUDGETDEP_GUID_ID"]).CompareTo(((System.Guid)objTreeNode.GetValue(colGuid_ID))) == 0)
                            {
                                // ���� �������� � ������ ���������
                                // ������� ������ "��������� �������������" � ��������� ��� � ������
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

                                // ������ �����������
                                objBudgetDep.UsesrList.ClearList();
                                if (row["BUDGETDEP_DECLRN"] != System.DBNull.Value)
                                {
                                    if (((System.String)objTreeNode.GetValue(colBudgetDeclrn)).Length > 0)
                                    {
                                        ERP_Budget.Common.CUser objUser = null;
                                        // ����� ��������� ������ �� ������������ � ������� �����������
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

                                // ��������� ������ "������ ��������" � ������
                                objList.AddItemToList(objBudgetDep);
                            }
                        }
                    }
                }

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "������ ������ ���������\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        
        /// <summary>
        /// ���������� ��������������������� ������ "���������" �� ��� �����
        /// </summary>
        /// <param name="strBudgetDepName">��� ����������</param>
        /// <returns>������ "���������"</returns>
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
                System.Windows.Forms.MessageBox.Show( this, "������ ������ ����������\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return objRet;
        }
        /// <summary>
        /// ������� ������ ���������� ��������� ������������� ��� ������������ ���������� � ��
        /// </summary>
        /// <returns></returns>
        private ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> CreateChangesNodesList()
        {
            ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList = new ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep>();
            objList.ClearList();
            try
            {
                // ��������, ���� ��������� � ������ ��� ���
                System.Data.DataTable dtChanges = dsDirect.Tables[ "dtBudgetDep" ].GetChanges();
                if( dtChanges == null ) { return objList; }

                // ��������� ������ ��������� �����
                FillChangesNodesList2( dtChanges, objList, treeList.FocusedNode );

                //// ��������� ������ ��������� �����
                //FillChangesNodesList(dtChanges, objList, treeList.Nodes[0]);
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "������ ������������ ������ ���������� ��������� �������������\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return objList;
        }
             

        /// <summary>
        /// ��������� ��������� � ������ �������� ������������� � ��
        /// </summary>
        /// <returns></returns>
        private System.Boolean SaveChangesToDB()
        {
            System.Boolean bRes = false;
            try
            {
                System.Data.DataTable dtChanges = dsDirect.Tables[ "dtBudgetDep" ].GetChanges();
                if( dtChanges == null ) { return true; }
                // ��������� �������������, ���������� ������ �������� ������������� ��� ���������� � ��
                ERP_Budget.Common.CBaseList<ERP_Budget.Common.CBudgetDep> objList = CreateChangesNodesList();

                // ������ �������� - ��������� ��������� ��������� � ��
                if( ERP_Budget.Common.CBudgetDep.SaveBudgetDepListToDB( objList, m_objProfile ) == true )
                {
                    dsDirect.AcceptChanges();
                    // ������ ������� �� ���������� � ������
                    SetModifiedToFalse();
                    bRes = true;
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "������ ���������� ��������� � ��\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return bRes;
        }

        private void btnSave_Click( object sender, EventArgs e )
        {
            try
            {
                // ���� ��������� ���� ��������� � ������ ��������������, �� �������� ���� �������
                if( treeList.FocusedNode != null )
                {
                    // ����������� ������ ���������� ������������� � ���������� ����
                    SetBudgetDepDeclrn( treeList.FocusedNode );
                    // ��������� ������ � ���������� ����
                    if( bIsNodeValuesValidation( treeList.FocusedNode, true ) == false ) { return; }
                }
                if( SaveChangesToDB() == false )
                {
                    System.Windows.Forms.MessageBox.Show( this, "�� ������� ��������� ��������� � ���� ������", "������",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "������ ���������� ���������\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }

        #endregion

        #region ������

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
                System.Windows.Forms.MessageBox.Show( this, "������ ������\n" + f.Message, "������",
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
                System.Windows.Forms.MessageBox.Show( this, "������ treeList_AfterFocusNode\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }
            return;
        }
        /// <summary>
        /// ��������� ���� �� ������� ������ � ������
        /// </summary>
        /// <param name="objNode">���� ������</param>
        /// <param name="bShowMessage">������� ����, �������� ��������� ��� ���</param>
        /// <returns></returns>
        private System.Boolean bIsNodeValuesValidation( DevExpress.XtraTreeList.Nodes.TreeListNode objNode, 
            System.Boolean bShowMessage )
        {
            System.Boolean bRet = false;
            try
            {
                // ������ ������ � ������ ������, ��������������� ���������� ����
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
                    // ��������, ������� �� ��������
                    if( ( ( System.String )dsDirect.Tables[ "dtBudgetDep" ].Rows[ iRecNum ][ "BUDGETDEP_NAME" ] ) == "" )
                    {
                        if( bShowMessage )
                        {
                            System.Windows.Forms.MessageBox.Show( this, "���������� ������� ������������ ���������� �������������.", "��������������",
                               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning );
                        }
                        return bRet;
                    }
                    // ��������, ������ �� ����
                    if( dsDirect.Tables[ "dtBudgetDep" ].Rows[ iRecNum ][ "BUDGETDEP_MANAGER_ID" ] == System.DBNull.Value )
                    {
                        if( bShowMessage )
                        {
                            System.Windows.Forms.MessageBox.Show( this, "���������� ������� ������������ ���������� �������������.", "��������������",
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
                    System.Windows.Forms.MessageBox.Show( this, "������ �������� �������� � ����\n" + f.Message, "������",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                }
            }
            return bRet;
        }

        private void treeList_BeforeFocusNode( object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e )
        {
            try
            {
                // ����� ��� ��� �������� ����� ���� ��������, ��� �� ��������� � ������� ���������� ����
                if( treeList.Nodes.Count == 0 ) { return; }
                if( e.OldNode == null ) { return; }

                // �������� �������� ������� "������ ���������� �������������"
                SetBudgetDepDeclrn( e.OldNode );

                e.CanFocus = bIsNodeValuesValidation( e.OldNode, true );
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "������ treeList_AfterFocusNode\n" + f.Message, "������",
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
                        "������ ��������� ������������� ��� �������.\n����� �� ����� ��� ���������� ���������?", "��������",
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question ) == System.Windows.Forms.DialogResult.No )
                    // ��������� ������� ����������
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch( System.Exception f )
            {
                System.Windows.Forms.MessageBox.Show( this, "frmBudgetDep_FormClosing\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
            }

            return;
        }
        /// <summary>
        /// ����������� �������� ������� "������ �������������"
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
                System.Windows.Forms.MessageBox.Show( this, "������ SetBudgetDepDeclrn\n" + f.Message, "������",
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
                System.Windows.Forms.MessageBox.Show(this, "frmBudgetDep_FormClosing\n" + f.Message, "������",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void frmBudgetDep_Shown(object sender, EventArgs e)
        {
            try
            {
                // ��������� ������
                bLoadBudgetDeps();

                //if (treeList.Nodes.Count > 0)
                //{
                //    treeList.ExpandAll();
                //    treeList.BestFitColumns();
                //}
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "������ �������� �����\n" + f.Message, "������",
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