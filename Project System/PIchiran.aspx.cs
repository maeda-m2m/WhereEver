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
            DgPIchiran.EditCommand +=
             new DataGridCommandEventHandler(this.DgPIchiran_EditCommand);
            DgPIchiran.CancelCommand +=
                new DataGridCommandEventHandler(this.DgPIchiran_CancelCommand);
            DgPIchiran.UpdateCommand +=
                new DataGridCommandEventHandler(this.DgPIchiran_UpdateCommand);
            DgPIchiran.ItemCommand +=
                new DataGridCommandEventHandler(this.DgPIchiran_ItemCommand);
        }

        protected void DgPIchiran_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                DATASET.DataSet.T_PdbRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_PdbRow;
                e.Item.Cells[0].Text = dr.Pname.ToString();
                e.Item.Cells[1].Text = dr.Pcustomer.ToString();
                e.Item.Cells[2].Text = dr.Presponsible.ToString();
                e.Item.Cells[3].Text = dr.Pcategory.ToString();
                e.Item.Cells[4].Text = dr.Pstarttime.ToShortDateString();
                e.Item.Cells[5].Text = dr.Povertime.ToShortDateString();

            }
        }
        public static DATASET.DataSet.T_PdbDataTable GetPdbDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Pdb order by Pid";
            DATASET.DataSet.T_PdbDataTable dt = new DATASET.DataSet.T_PdbDataTable();
            da.Fill(dt);
            return dt;
        }

        protected void DgPIchiran_EditCommand(object source, DataGridCommandEventArgs e)
        {
            //string txtPname = e.Item.Cells[0].Text;
            //DATASET.DataSet.T_PdbRow dr = GetT_PdbRow(txtPname, Global.GetConnection());
            //SessionManager.Project(dr);
            DgPIchiran.EditItemIndex = e.Item.ItemIndex;
            DgPIchiran.DataSource = GetPdbDataTable(Global.GetConnection());
            DgPIchiran.DataBind();
        }

        protected void DgPIchiran_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            DgPIchiran.EditItemIndex = -1;
            DgPIchiran.DataSource = GetPdbDataTable(Global.GetConnection());
            DgPIchiran.DataBind();
        }

        protected void DgPIchiran_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
        }
        
        protected void DgPIchiran_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            //string txtPname = e.Item.Cells[0].Text;
            //switch (((LinkButton)e.CommandSource).CommandName)
            //{

            //    case "Delete":
            //        Delete(txtPname);
            //        break;
            //        // Add other cases here, if there are multiple ButtonColumns in 
            //        // the DataGrid control.
                    
            //    default:
            //        // Do nothing.
            //        break;

            //}
        }

        //public void Update(string Pname)
        //{
        //    string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
        //    using (SqlConnection connection = new SqlConnection(cstr))
        //    {
        //        string sql = "DELETE FROM T_Pdb WHERE PName = @i";
        //        SqlDataAdapter da = new SqlDataAdapter(sql, connection);

        //        da.SelectCommand.Parameters.AddWithValue("@i", Pname);

        //        connection.Open();
        //        int cnt = da.SelectCommand.ExecuteNonQuery();
        //        connection.Close();
        //    }
        //}
        //public void Delete(string Pname)
        //{
        //    string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
        //    using (SqlConnection connection = new SqlConnection(cstr))
        //    {
        //        string sql = "DELETE FROM T_Pdb WHERE PName = @i";
        //        SqlDataAdapter da = new SqlDataAdapter(sql, connection);

        //        da.SelectCommand.Parameters.AddWithValue("@i", Pname);

        //        connection.Open();
        //        int cnt = da.SelectCommand.ExecuteNonQuery();
        //        connection.Close();
        //    }
        //}

        protected void btnNewP_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_PdbDataTable t_Pdbs = new DATASET.DataSet.T_PdbDataTable();
            DATASET.DataSet.T_PdbRow dr = t_Pdbs.NewT_PdbRow();

            DATASET.DataSet.T_PdbRow dl = GetMaxPidRow(Global.GetConnection());
            int sl = dl.Pid;
            dr.Pid = sl + 1;
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
            ddlResponsible.Text = "";
        }

        internal static DATASET.DataSet.T_PdbRow GetMaxPidRow(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select MAX(Pid) as Pid from T_Pdb";
            DATASET.DataSet.T_PdbDataTable dt = new DATASET.DataSet.T_PdbDataTable();
            da.Fill(dt);
            return dt[0];
        }
        //internal static DATASET.DataSet.T_PdbRow GetT_PdbRow(string name,SqlConnection sqlConnection)
        //{
        //    SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
        //    da.SelectCommand.CommandText =
        //        "select * from T_Pdb where Pname is @i";
        //    da.SelectCommand.Parameters.AddWithValue("@i", name);
        //    DATASET.DataSet.T_PdbDataTable dt = new DATASET.DataSet.T_PdbDataTable();
        //    da.Fill(dt);
        //    return dt[0];
        //}

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