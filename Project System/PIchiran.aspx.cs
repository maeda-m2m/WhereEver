using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver.Project_System
{
    public partial class PIchiran : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DATASET.DataSet.T_PdbDataTable dt = GetPdbDataTable(Global.GetConnection());

                DgPIchiran.DataSource = dt;
                DgPIchiran.DataBind();
            }
        }
        public static DATASET.DataSet.T_PdbDataTable GetPdbDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Pdb";
            DATASET.DataSet.T_PdbDataTable dt = new DATASET.DataSet.T_PdbDataTable();
            da.Fill(dt);
            return dt;
        }

        protected void DgPIchiran_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                DATASET.DataSet.T_PdbRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_PdbRow;
                LinkButton lbPName = e.Item.FindControl("lbPName") as LinkButton;
                Label lblCustomer = e.Item.FindControl("lblCustomer") as Label;
                LinkButton lbResponsible = e.Item.FindControl("lbResponsible") as LinkButton;
                Label lblCategory = e.Item.FindControl("lblCategory") as Label;
                Label lblStartTime = e.Item.FindControl("lblStartTime") as Label;
                Label lblOverTime = e.Item.FindControl("lblOverTime") as Label;

                lbPName.Text = dr.Pname.ToString();
                lblCustomer.Text = dr.Pcustomer.ToString();
                lbResponsible.Text = dr.Presponsible.ToString();
                lblCategory.Text = dr.Pcategory.ToString();
                lblStartTime.Text = dr.Pstarttime.ToShortDateString();
                lblOverTime.Text = dr.Povertime.ToShortDateString();
            }
        }

        protected void btnNewP_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_PdbDataTable t_Pdbs = new DATASET.DataSet.T_PdbDataTable();
            DATASET.DataSet.T_PdbRow dr = t_Pdbs.NewT_PdbRow();

            dr.Pname = txtNewPName.Text.Trim();
            dr.Pcustomer = txtNewCustomer.Text.Trim();
            dr.Presponsible = ddlResponsible.SelectedItem.Text.Trim();
            dr.Pcategory = txtNewCategory.Text.Trim();
            dr.Pstarttime = Calendar1.SelectedDate;
            dr.Povertime = Calendar2.SelectedDate;

            t_Pdbs.Rows.Add(dr);

            InsertProject(t_Pdbs, Global.GetConnection());

            DATASET.DataSet.T_PdbDataTable dt = GetPdbDataTable(Global.GetConnection());

            DgPIchiran.DataSource = dt;
            DgPIchiran.DataBind();

            txtNewPName.Text = "";
            txtNewCustomer.Text = "";
            txtNewCategory.Text = "";
        }

        public static void InsertProject(DATASET.DataSet.T_PdbDataTable dt, SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Pdb";
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();

            SqlTransaction sql = null;

            try
            {
                sqlConnection.Open();
                sql = sqlConnection.BeginTransaction();

                da.SelectCommand.Transaction = da.InsertCommand.Transaction = sql;

                da.Update(dt);

                sql.Commit();
            }
            catch (Exception ex)
            {
                if (sql != null)
                    sql.Rollback();
            }
            finally
            {
                sqlConnection.Close();
            }
        }

    }
}