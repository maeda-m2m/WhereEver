﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            if (!IsPostBack)
            {
                CreateDropDownList();
                CreateDataGrid();
                
            }
        }
        private void CreateDataGrid()
        {
            DATASET.DataSet.T_PdbKanriDataTable dt = GetT_PdbKanriDataTable(Global.GetConnection());

            DgPKanri.DataSource = dt;
            DgPKanri.DataBind();
        }
        public static DATASET.DataSet.T_PdbKanriDataTable GetT_PdbKanriDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_PdbKanri order by PBigid";
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);
            return dt;
        }
        private void CreateDropDownList()
        {
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = Insert.GetT_PdbKanri(Global.GetConnection());
            DataView dtview = dtview = new DataView(t_PdbKanris);
            DataTable dt1 = dtview.ToTable(false, "PBigname");
            for (int rowindex = 0; rowindex < dt1.Rows.Count; rowindex++)
            {
                for (int colindex = 0; colindex < dt1.Rows[rowindex].ItemArray.Length; colindex++)
                {
                    ddlPBigList.Items.Add(dt1.Rows[rowindex][colindex].ToString());
                }
            }
        }
        protected void DgPKanri_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DATASET.DataSet.T_PdbKanriRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_PdbKanriRow;
                e.Item.Cells[0].Text = dr.PBigname.ToString();
                e.Item.Cells[1].Text = dr.PMiddlename.ToString();
                e.Item.Cells[2].Text = dr.PMiddlestart.ToString();
                e.Item.Cells[3].Text = dr.PMiddleover.ToString();
                e.Item.Cells[4].Text = dr.PTorokutime.ToString();
                e.Item.Cells[5].Text = dr.PTorokusya.ToString();
            }
        }

        protected void btnToroku_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = new DATASET.DataSet.T_PdbKanriDataTable();
            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow = t_PdbKanris.NewT_PdbKanriRow();

            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow1 = Insert.GetMaxPBigidRow(Global.GetConnection());

            t_PdbKanriRow.PBigid = t_PdbKanriRow1.PBigid + 1;
            t_PdbKanriRow.PBigname = txtPBig.Text;

            t_PdbKanris.Rows.Add(t_PdbKanriRow);
            Insert.InsertPBig(t_PdbKanris, Global.GetConnection());
        }

        protected void DgPKanri_ItemCommand(object source, DataGridCommandEventArgs e)
        {

        }
    }
}