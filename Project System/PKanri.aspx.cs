using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver.Project_System
{
    public partial class PKanri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DgPKanri_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }

        protected void btnToroku_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = new DATASET.DataSet.T_PdbKanriDataTable();
            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow = t_PdbKanris.NewT_PdbKanriRow();

            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow1 = Insert.GetMaxPBigidRow(Global.GetConnection());

            int sl = t_PdbKanriRow1.PBigid;
            t_PdbKanriRow.PBigid = sl +1;
            t_PdbKanriRow.PBigname = txtPBig.Text;

            t_PdbKanris.Rows.Add(t_PdbKanriRow);
            Insert.InsertPBig(t_PdbKanris, Global.GetConnection());
        }
    }
}