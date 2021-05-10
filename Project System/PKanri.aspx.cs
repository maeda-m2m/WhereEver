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
    public partial class PKanri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateDropDownList();
                CreateDataGrid();
                DgPKanri.EditCommand +=
                    new DataGridCommandEventHandler(this.DgPKanri_EditCommand);
                DgPKanri.CancelCommand +=
                    new DataGridCommandEventHandler(this.DgPKanri_CancelCommand);
                DgPKanri.UpdateCommand +=
                    new DataGridCommandEventHandler(this.DgPKanri_UpdateCommand);
                DgPKanri.ItemCommand +=
                    new DataGridCommandEventHandler(this.DgPKanri_ItemCommand);

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
                "SELECT * FROM T_PdbKanri where PMiddleid != 0 order by PBigid , PMiddleid";
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
                e.Item.Cells[1].Text = dr.PMiddleid.ToString();
                e.Item.Cells[2].Text = dr.PMiddlename.ToString();
                e.Item.Cells[3].Text = dr.PMiddlestart.ToShortDateString();
                e.Item.Cells[4].Text = dr.PMiddleover.ToShortDateString();
                if (dr.PMiddleover < DateTime.Now)
                {
                    e.Item.Cells[5].Text = "完了";
                }
                else
                {
                    if (dr.PMiddleover == DateTime.Parse("2100/01/01 00:00:00"))
                    {
                        e.Item.Cells[4].Text = "";
                    }
                }
                e.Item.Cells[6].Text = dr.PTorokutime.ToShortDateString();
                e.Item.Cells[7].Text = dr.PTorokusya.ToString();
                
            }
        }

        protected void btnToroku_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = new DATASET.DataSet.T_PdbKanriDataTable();
            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow = t_PdbKanris.NewT_PdbKanriRow();

            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow1 = Insert.GetMaxPBigidRow(Global.GetConnection());

            
            if(t_PdbKanriRow1.IsNull("PBigid"))
            {
                t_PdbKanriRow.PBigid = 1;
            }
            else
            {
                t_PdbKanriRow.PBigid = t_PdbKanriRow1.PBigid + 1;
                
            }
            t_PdbKanriRow.PBigname = txtPBig.Text;
            t_PdbKanriRow.PMiddleid = 0;
            t_PdbKanriRow.PTorokutime = DateTime.Today;
            t_PdbKanriRow.PTorokusya = SessionManager.User.M_User.id.Trim();

            t_PdbKanris.Rows.Add(t_PdbKanriRow);
            Insert.InsertPBig(t_PdbKanris, Global.GetConnection());

            ddlPBigList.Items.Clear();
            ddlPBigList.Items.Add("");
            CreateDropDownList();
            ddlPBigList.Text = txtPBig.Text;
            txtPBig.Text = "";
        }
        internal static int GetPBigidNow(SqlConnection sqlConnection,string name)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select PBigid from T_PdbKanri where PBigname like @name";
            da.SelectCommand.Parameters.AddWithValue("@name", name);
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);
            return dt[0].PBigid;
        }
        protected void btnPMiddle_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = new DATASET.DataSet.T_PdbKanriDataTable();
            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow = t_PdbKanris.NewT_PdbKanriRow();

            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow1 = Insert.GetMaxPMiddleidRow(Global.GetConnection(), ddlPBigList.SelectedItem.ToString());
            if (ddlPBigList.Text!="")
            {
                if (date1.Value != "")
                {
                    lblStart.Text = "開始";
                    if (date2.Value !="")
                    {
                        if (DateTime.Parse(date2.Value) >= DateTime.Parse(date1.Value))
                        {
                            lblCalendarError.Text = "";
                            lblStart.Text = "開始";
                            if (t_PdbKanriRow1.PMiddleid == 0)
                            {
                                t_PdbKanriRow.PMiddleid = 1;
                                t_PdbKanriRow.PMiddlename = txtPMiddle.Text;
                                t_PdbKanriRow.PMiddlestart = DateTime.Parse(date1.Value);
                                t_PdbKanriRow.PMiddleover = DateTime.Parse(date2.Value);
                                t_PdbKanriRow.PTorokutime = DateTime.Now;
                                t_PdbKanriRow.PTorokusya = SessionManager.User.M_User.id.Trim();
                                Update.UpdateMiddleNew(t_PdbKanriRow, ddlPBigList.SelectedItem.ToString());
                            }
                            else
                            {
                                t_PdbKanriRow.PBigname = ddlPBigList.SelectedItem.Text;
                                t_PdbKanriRow.PBigid = GetPBigidNow(Global.GetConnection(), ddlPBigList.SelectedItem.Text);
                                t_PdbKanriRow.PMiddleid = t_PdbKanriRow1.PMiddleid + 1;
                                t_PdbKanriRow.PMiddlename = txtPMiddle.Text;
                                t_PdbKanriRow.PMiddlestart = DateTime.Parse(date1.Value);
                                
                                t_PdbKanriRow.PTorokutime = DateTime.Now;
                                t_PdbKanriRow.PTorokusya = SessionManager.User.M_User.id.Trim();
                                t_PdbKanris.Rows.Add(t_PdbKanriRow);
                                Insert.InsertPBig(t_PdbKanris, Global.GetConnection());
                            }
                            ddlPBigList.Text = "";
                            txtPMiddle.Text = "";
                            date1.Value = null;
                            date2.Value = null;
                            lblAisatu1.Text = "を選択してから、中項目入力をお願い致します。";
                            CreateDataGrid();
                        }
                        else
                        {
                            lblCalendarError.Text = "<font color=red>カレンダーに誤りがあります。<font>";
                        }
                    }
                    else
                    {
                        lblCalendarError.Text = "";
                        lblStart.Text = "開始";
                        if (t_PdbKanriRow1.PMiddleid == 0)
                        {
                            t_PdbKanriRow.PMiddleid = 1;
                            t_PdbKanriRow.PMiddlename = txtPMiddle.Text;
                            t_PdbKanriRow.PMiddlestart = DateTime.Parse(date1.Value);
                            t_PdbKanriRow.PMiddleover = DateTime.Parse("2100/01/01 00:00:00");
                            t_PdbKanriRow.PTorokutime = DateTime.Now;
                            t_PdbKanriRow.PTorokusya = SessionManager.User.M_User.id.Trim();
                            Update.UpdateMiddleNew(t_PdbKanriRow, ddlPBigList.SelectedItem.ToString());
                        }
                        else
                        {
                            t_PdbKanriRow.PBigname = ddlPBigList.SelectedItem.Text;
                            t_PdbKanriRow.PBigid = GetPBigidNow(Global.GetConnection(), ddlPBigList.SelectedItem.Text);
                            t_PdbKanriRow.PMiddleid = t_PdbKanriRow1.PMiddleid + 1;
                            t_PdbKanriRow.PMiddlename = txtPMiddle.Text;
                            t_PdbKanriRow.PMiddlestart = DateTime.Parse(date1.Value);
                            t_PdbKanriRow.PMiddleover = DateTime.Parse("2100/01/01 00:00:00");
                            t_PdbKanriRow.PTorokutime = DateTime.Now;
                            t_PdbKanriRow.PTorokusya = SessionManager.User.M_User.id.Trim();
                            t_PdbKanris.Rows.Add(t_PdbKanriRow);
                            Insert.InsertPBig(t_PdbKanris, Global.GetConnection());
                        }
                        ddlPBigList.Text = "";
                        txtPMiddle.Text = "";
                        date1.Value = null;
                        date2.Value = null;
                        lblAisatu1.Text = "を選択してから、中項目入力をお願い致します。";
                        CreateDataGrid();
                    }
                    
                }
                else
                {
                    lblStart.Text = "開始<font color=red>(必須)<font>";
                    lblAisatu1.Text = "を選択してから、中項目入力をお願い致します。";
                }
            }
            else
            {
                lblAisatu1.Text = "<font color=red>を選択してから、中項目入力をお願い致します。<font>";
            } 
        }

        protected void DgPKanri_EditCommand(object source, DataGridCommandEventArgs e)
        {
            DgPKanri.EditItemIndex = e.Item.ItemIndex;
            CreateDataGrid();
        }

        protected void DgPKanri_CancelCommand(object source, DataGridCommandEventArgs e)
        {

            DgPKanri.EditItemIndex = -1;
            CreateDataGrid();
        }

        protected void DgPKanri_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            TextBox txtPMiddlename = (TextBox)e.Item.Cells[2].Controls[0];
            TextBox txtPMiddlestart = (TextBox)e.Item.Cells[3].Controls[0];
            TextBox txtPMiddleover = (TextBox)e.Item.Cells[4].Controls[0];
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = new DATASET.DataSet.T_PdbKanriDataTable();
            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow = t_PdbKanris.NewT_PdbKanriRow();

            t_PdbKanriRow[3] = txtPMiddlename.Text;
            t_PdbKanriRow[4] = txtPMiddlestart.Text;
            t_PdbKanriRow[5] = txtPMiddleover.Text;
            t_PdbKanriRow[6] = SessionManager.User.M_User.id.Trim();
            t_PdbKanriRow[7] = DateTime.Now;
            Update.UpdateMiddle(t_PdbKanriRow, e.Item.Cells[0].Text, e.Item.Cells[1].Text);
            DgPKanri.EditItemIndex = -1;
            CreateDataGrid();
        }

        protected void DgPKanri_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            string bigname = e.Item.Cells[0].Text;
            string Middleid = e.Item.Cells[1].Text;
            switch (((LinkButton)e.CommandSource).CommandName)
            {

                case "Delete":
                    Delete.DeleteMiddle(bigname, Middleid);
                    break;

                // Add other cases here, if there are multiple ButtonColumns in 
                // the DataGrid control.

                default:
                    // Do nothing.
                    break;

            }
            DgPKanri.EditItemIndex = -1;
            CreateDataGrid();
        }

        protected void btnDeleteBig_Click(object sender, EventArgs e)
        {
            Delete.DeleteBig(ddlPBigList.Text);
            CreateDataGrid();
            ddlPBigList.Items.Clear();
            ddlPBigList.Items.Add("");
            CreateDropDownList();
            ddlPBigList.Text = "";
        }

        protected void btnWBS_Click(object sender, EventArgs e)
        {
            DateTime Time1 = WBS.GetPMiddleTimeRow(Global.GetConnection()).PMiddlestart;
            DateTime Time2 = WBS.GetPMiddleTimeRow(Global.GetConnection()).PMiddleover;

            int interval = (int)(Time2 - Time1).TotalDays+1;
            ar = new DateTime[interval];

            for (int i = 0; i < interval; i++)
            {
                ar[i] = Time1.AddDays(i);
            }
            DATASET.DataSet.T_PdbKanriDataTable dt = GetT_PdbKanriDataTable(Global.GetConnection());
            wbs.DataSource = dt;
            wbs.DataBind();
        }
        public DateTime[] ar;
        protected void wbs_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            for (int i = 0; i<ar.Length;i++)
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    TableCell cell = new TableCell();
                    cell.Controls.Add(new LiteralControl(ar[i].Day.ToString()));//ヘッダー
                    e.Item.Cells.Add(cell);
                }
            }

            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DATASET.DataSet.T_PdbKanriRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_PdbKanriRow;
                e.Item.Cells[0].Text = dr.PBigname.ToString();
                e.Item.Cells[1].Text = dr.PMiddlename.ToString();
                for (int i = 0; i < ar.Length; i++)
                {
                    TableCell cell = new TableCell();
                    if (dr.PMiddlestart <= ar[i] && dr.PMiddleover >= ar[i])
                    {
                        cell.BackColor = Color.Black;
                        cell.BorderColor = Color.White;
                    }
                    DayOfWeek dow = ar[i].DayOfWeek;

                    switch (dow)
                    {
                        case DayOfWeek.Saturday:
                            cell.BackColor = Color.Gray;
                            cell.BorderColor = Color.White;
                            break;
                        case DayOfWeek.Sunday:
                            cell.BackColor = Color.Gray;
                            cell.BorderColor = Color.White;
                            break;
                    }
                    cell.Width = 1620/ar.Length;
                    e.Item.Cells.Add(cell);
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtPBig.Text = "";
            ddlPBigList.Text = "";
            txtPMiddle.Text = "";
            lblStart.Text = "開始";
            date1.Value = "";
            date2.Value = "";
            lblCalendarError.Text = "";
            lblAisatu1.Text = "を選択してから、中項目入力をお願い致します。";
            wbs.DataSource = null;
        }
        
    }
}