using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERP_Budget.Common;

namespace DebitArticle
{
    public class ViewDebitArticle : PlugIn.IClassTypeView
    {
        public override void Run(UniXP.Common.MENUITEM objMenuItem, System.String strCaption)
        {
            frmDebitArticle obj = new frmDebitArticle(objMenuItem.objProfile) { Text = strCaption, MdiParent = objMenuItem.objProfile.m_objMDIManager.MdiParent, Visible = true };
        }
    }
}
