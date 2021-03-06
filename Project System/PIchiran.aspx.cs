using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
                DgPIchiran.EditCommand +=
                    new DataGridCommandEventHandler(this.DgPIchiran_EditCommand);
                DgPIchiran.CancelCommand +=
                    new DataGridCommandEventHandler(this.DgPIchiran_CancelCommand);
                DgPIchiran.UpdateCommand +=
                    new DataGridCommandEventHandler(this.DgPIchiran_UpdateCommand);
                DgPIchiran.ItemCommand +=
                    new DataGridCommandEventHandler(this.DgPIchiran_ItemCommand);

                DATASET.DataSet.M_UserDataTable m_User = Insert.GetM_User(Global.GetConnection());
                DataView dtview = dtview = new DataView(m_User);
                DataTable dt1 = dtview.ToTable(false, "name1");
                for (int rowindex = 0; rowindex < dt1.Rows.Count; rowindex++)
                {
                    for (int colindex = 0; colindex < dt1.Rows[rowindex].ItemArray.Length; colindex++)
                    {
                        ddlResponsible.Items.Add(dt1.Rows[rowindex][colindex].ToString());
                    }
                }
            }
        }

        protected void DgPIchiran_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                DATASET.DataSet.T_PdbRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_PdbRow;
                e.Item.Cells[0].Text = dr.Pid.ToString();
                e.Item.Cells[1].Text = dr.Pname.ToString();
                e.Item.Cells[2].Text = dr.Pcustomer.ToString();
                e.Item.Cells[3].Text = dr.Presponsible.ToString();
                e.Item.Cells[4].Text = dr.Pcategory.ToString();
                e.Item.Cells[5].Text = dr.Pstarttime.ToShortDateString();
                if (dr.Povertime == DateTime.Parse("2100/01/01"))
                {
                    e.Item.Cells[6].Text = "未定";
                }
                else
                {
                    e.Item.Cells[6].Text = dr.Povertime.ToShortDateString();
                }
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

        public void DgPIchiran_EditCommand(object source, DataGridCommandEventArgs e)
        {
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
            TextBox txtPname = (TextBox)e.Item.Cells[1].Controls[0];
            TextBox txtPcustomer = (TextBox)e.Item.Cells[2].Controls[0];
            TextBox txtPresponsible = (TextBox)e.Item.Cells[3].Controls[0];
            TextBox txtPcategory = (TextBox)e.Item.Cells[4].Controls[0];
            TextBox txtPstartTime = (TextBox)e.Item.Cells[5].Controls[0];
            TextBox txtPoverTime = (TextBox)e.Item.Cells[6].Controls[0];

            string Pid = e.Item.Cells[0].Text;
            string Pname = txtPname.Text;
            string Pcustomer = txtPcustomer.Text;
            string Presponsible = txtPresponsible.Text;
            string Pcategory = txtPcategory.Text;
            string PstartTime = txtPstartTime.Text;
            string PoverTime = txtPoverTime.Text;

            DATASET.DataSet.T_PdbDataTable t_Pdbs = GetPdbDataTable(Global.GetConnection());
            DATASET.DataSet.T_PdbRow dr = t_Pdbs.NewT_PdbRow();

            dr[0] = Pname;
            dr[1] = Pcustomer;
            dr[2] = Presponsible;
            dr[3] = Pcategory;
            dr[4] = PstartTime;
            dr[5] = PoverTime;
            dr[6] = Pid;
            Update.UpdateProject(dr);

            DgPIchiran.DataSource = GetPdbDataTable(Global.GetConnection());
            DgPIchiran.DataBind();
        }

        protected void DgPIchiran_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            switch (((LinkButton)e.CommandSource).CommandName)
            {

                case "Delete":
                    Delete.DeleteProject(id);
                    break;

                // Add other cases here, if there are multiple ButtonColumns in 
                // the DataGrid control.
                case "wbs":
                    DATASET.DataSet.T_PdbRow dr = Class1.GetProjectRow(id, Global.GetConnection());
                    SessionManager.Project(dr);
                    Response.Redirect("PKanri.aspx");
                    break;
                default:
                    // Do nothing.
                    break;

            }
            DgPIchiran.EditItemIndex = -1;
            DgPIchiran.DataSource = GetPdbDataTable(Global.GetConnection());
            DgPIchiran.DataBind();
        }


        protected void btnNewP_Click(object sender, EventArgs e)
        {
            if (date1.Value!="") 
            {
                DATASET.DataSet.T_PdbDataTable t_Pdbs = new DATASET.DataSet.T_PdbDataTable();
                DATASET.DataSet.T_PdbRow dr = t_Pdbs.NewT_PdbRow();

                DATASET.DataSet.T_PdbRow dl = Insert.GetMaxPidRow(Global.GetConnection());
                int sl = dl.Pid;
                dr.Pid = sl + 1;
                dr.Pname = txtNewPName.Text.Trim();
                dr.Pcustomer = txtNewCustomer.Text.Trim();
                dr.Presponsible = ddlResponsible.SelectedItem.Text.Trim();
                dr.Pcategory = txtNewCategory.Text.Trim();
                dr.Pstarttime = DateTime.Parse(date1.Value);

                if (date2.Value != "")
                {
                    dr.Povertime = DateTime.Parse(date2.Value);
                }
                else
                {
                    dr.Povertime = DateTime.Parse("2100/01/01");
                }
                

                t_Pdbs.Rows.Add(dr);

                Insert.InsertProject(t_Pdbs, Global.GetConnection());

                DATASET.DataSet.T_PdbDataTable dt = GetPdbDataTable(Global.GetConnection());

                DgPIchiran.DataSource = dt;
                DgPIchiran.DataBind();

                clear();
            }
            else
            {
                lblStartM.ForeColor = Color.Red;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
        public void clear()
        {
            txtNewPName.Text = "";
            txtNewCustomer.Text = "";
            txtNewCategory.Text = "";
            ddlResponsible.Text = "";
            date1.Value = "";
            date2.Value = "";
            lblStartM.ForeColor = Color.Black;
        }
    }
}