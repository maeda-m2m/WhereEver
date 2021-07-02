using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WhereEver.Project_System
{
    
    public partial class PKanri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int spId = SessionManager.project.PdbRow.Pid;

                int dt = WBS.GetT_PdbKanriDataTable(Global.GetConnection(), spId);
                if (dt == 0)
                {
                    DgPKanri.Visible = false;
                    wbs.Visible = false;
                    btnWBS.Visible = false;
                }
                else
                {
                    DgPKanri.Visible = true;
                    wbs.Visible = true;
                    btnWBS.Visible = true;
                }
                CreateDropDownList(spId);
                CreateDataGrid(spId);
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
        private void CreateDataGrid(int spId)
        {
            DATASET.DataSet.T_PdbKanriDataTable dt = GetT_PdbKanriDataTable(Global.GetConnection(), spId);

            DgPKanri.DataSource = dt;
            DgPKanri.DataBind();

            wbs.Visible = false;
        }
        public static DATASET.DataSet.T_PdbKanriDataTable GetT_PdbKanriDataTable(SqlConnection sqlConnection,int spId)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_PdbKanri where PMiddleid != 0 and Pid = @spId order by PBigid , PMiddleid";
            da.SelectCommand.Parameters.AddWithValue("@spId", spId);
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);
            return dt;
        }
        private void CreateDropDownList(int spId)
        {
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = Insert.GetT_PdbKanri(Global.GetConnection(), spId);
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

        public string bigname = "";
        protected void DgPKanri_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DATASET.DataSet.T_PdbKanriRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_PdbKanriRow;
                
                if (dr.PBigname != bigname && dr.PMiddleid==1)
                {
                    e.Item.Cells[0].Text = "<font color = white>"+dr.PBigname.ToString();
                    bigname = dr.PBigname;
                    if (dr.PBigid == 1)
                    {
                        Button uebig = e.Item.FindControl("uebig") as Button;
                        uebig.CssClass = "visibility_hidden";
                        Button sitabig = e.Item.FindControl("sitabig") as Button;
                        sitabig.CssClass = "jyunban";
                    }
                    else if (dr.PBigid == Insert.GetMaxPBigidRow(Global.GetConnection(), SessionManager.project.PdbRow.Pid).PBigid)
                    {
                        Button uebig = e.Item.FindControl("uebig") as Button;
                        uebig.CssClass = "jyunban";
                        Button sitabig = e.Item.FindControl("sitabig") as Button;
                        sitabig.CssClass = "visibility_hidden";
                    }
                    else
                    {
                        Button uebig = e.Item.FindControl("uebig") as Button;
                        uebig.CssClass = "jyunban";
                        Button sitabig = e.Item.FindControl("sitabig") as Button;
                        sitabig.CssClass = "jyunban";
                    }
                }
                else
                {
                    e.Item.Cells[0].Text = "<font color = black>"+dr.PBigname.ToString();

                }
                e.Item.Cells[2].Text = dr.PMiddleid.ToString();
                if (dr.PMiddleid == 1)
                {
                    Button uemiddle = e.Item.FindControl("uemiddle") as Button;
                    uemiddle.CssClass = "visibility_hidden";
                    Button sitamiddle = e.Item.FindControl("sitamiddle") as Button;
                    sitamiddle.CssClass = "jyunban";
                }
                else if (dr.PMiddleid == Insert.GetMaxPMiddleidRow(Global.GetConnection(), dr.PBigname).PMiddleid)
                {
                    Button uemiddle = e.Item.FindControl("uemiddle") as Button;
                    uemiddle.CssClass = "jyunban";
                    Button sitamiddle = e.Item.FindControl("sitamiddle") as Button;
                    sitamiddle.CssClass = "visibility_hidden";
                    Button uebig = e.Item.FindControl("uebig") as Button;
                    uebig.CssClass = "visibility_hidden";
                    Button sitabig = e.Item.FindControl("sitabig") as Button;
                    sitabig.CssClass = "visibility_hidden";
                }
                else
                {
                    Button uemiddle = e.Item.FindControl("uemiddle") as Button;
                    uemiddle.CssClass = "jyunban";
                    Button sitamiddle = e.Item.FindControl("sitamiddle") as Button;
                    sitamiddle.CssClass = "jyunban";
                    Button uebig = e.Item.FindControl("uebig") as Button;
                    uebig.CssClass = "visibility_hidden";
                    Button sitabig = e.Item.FindControl("sitabig") as Button;
                    sitabig.CssClass = "visibility_hidden";
                }
                if (Insert.GetMaxPMiddleidRow(Global.GetConnection(), dr.PBigname).PMiddleid==1)
                {
                    Button uemiddle = e.Item.FindControl("uemiddle") as Button;
                    uemiddle.CssClass = "visibility_hidden";
                    Button sitamiddle = e.Item.FindControl("sitamiddle") as Button;
                    sitamiddle.CssClass = "visibility_hidden";
                }
                
                e.Item.Cells[3].Text = dr.PMiddlename.ToString();

                e.Item.Cells[4].Text = "<progress max = 100 value = " + dr.PShintyoku+ " class = progress ></ progress >";
                                            
                e.Item.Cells[6].Text = dr.PMiddlestart.ToShortDateString();
                e.Item.Cells[7].Text = dr.PMiddleover.ToShortDateString();
                if (dr.PMiddleover < DateTime.Now)
                {
                    e.Item.Cells[8].Text = "完了";
                }
                else
                {
                    if (dr.PMiddleover == DateTime.Parse("2100/01/01 00:00:00"))
                    {
                        e.Item.Cells[7].Text = "";
                    }
                }
                e.Item.Cells[9].Text = dr.PTorokutime.ToShortDateString();
                e.Item.Cells[10].Text = dr.PTorokusya.ToString();

            }
        }

        protected void btnToroku_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = new DATASET.DataSet.T_PdbKanriDataTable();
            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow = t_PdbKanris.NewT_PdbKanriRow();

            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow1 = Insert.GetMaxPBigidRow(Global.GetConnection(), SessionManager.project.PdbRow.Pid);


            if (t_PdbKanriRow1.IsNull("PBigid"))
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
            t_PdbKanriRow.Pid = SessionManager.project.PdbRow.Pid;

            t_PdbKanris.Rows.Add(t_PdbKanriRow);
            Insert.InsertPBig(t_PdbKanris, Global.GetConnection());

            ddlPBigList.Items.Clear();
            ddlPBigList.Items.Add("");
            CreateDropDownList(SessionManager.project.PdbRow.Pid);
            ddlPBigList.Text = txtPBig.Text;
            txtPBig.Text = "";
        }
        internal static int GetPBigidNow(SqlConnection sqlConnection, string name,int Pid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select PBigid from T_PdbKanri where PBigname like @name and Pid like @Pid";
            da.SelectCommand.Parameters.AddWithValue("@name", name);
            da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);
            return dt[0].PBigid;
        }
        protected void btnPMiddle_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = new DATASET.DataSet.T_PdbKanriDataTable();
            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow = t_PdbKanris.NewT_PdbKanriRow();

            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow1 = Insert.GetMaxPMiddleidRow(Global.GetConnection(), ddlPBigList.SelectedItem.ToString());
            if (ddlPBigList.Text != "")
            {
                if (date1.Value != ""&& date2.Value != "")
                {
                    lblStart.Text = "開始";
                    lblOver.Text = "終了";
                    if (DateTime.Parse(date2.Value) >= DateTime.Parse(date1.Value))
                    {
                        lblCalendarError.Text = "";
                        if (t_PdbKanriRow1.PMiddleid == 0)
                        {
                            t_PdbKanriRow.PMiddleid = 1;
                            t_PdbKanriRow.PMiddlename = txtPMiddle.Text;
                            t_PdbKanriRow.PMiddlestart = DateTime.Parse(date1.Value);
                            t_PdbKanriRow.PMiddleover = DateTime.Parse(date2.Value);
                            t_PdbKanriRow.PTorokutime = DateTime.Now;
                            t_PdbKanriRow.PTorokusya = SessionManager.User.M_User.id.Trim();
                            t_PdbKanriRow.Pid = SessionManager.project.PdbRow.Pid;
                            t_PdbKanriRow.PShintyoku = int.Parse(ddpShintyoku.SelectedItem.Value);
                            Update.UpdateMiddleNew(t_PdbKanriRow, ddlPBigList.SelectedItem.ToString());
                        }
                        else
                        {
                            t_PdbKanriRow.PBigname = ddlPBigList.SelectedItem.Text;
                            t_PdbKanriRow.PBigid = GetPBigidNow(Global.GetConnection(), ddlPBigList.SelectedItem.Text, SessionManager.project.PdbRow.Pid);
                            t_PdbKanriRow.PMiddleid = t_PdbKanriRow1.PMiddleid + 1;
                            t_PdbKanriRow.PMiddlename = txtPMiddle.Text;
                            t_PdbKanriRow.PMiddlestart = DateTime.Parse(date1.Value);
                            t_PdbKanriRow.PMiddleover = DateTime.Parse(date2.Value);
                            t_PdbKanriRow.PTorokutime = DateTime.Now;
                            t_PdbKanriRow.PTorokusya = SessionManager.User.M_User.id.Trim();
                            t_PdbKanriRow.Pid = SessionManager.project.PdbRow.Pid;
                            t_PdbKanriRow.PShintyoku = int.Parse(ddpShintyoku.SelectedItem.Value);
                            t_PdbKanris.Rows.Add(t_PdbKanriRow);
                            Insert.InsertPBig(t_PdbKanris, Global.GetConnection());
                        }
                        ddpShintyoku.Text = "";
                        ddlPBigList.Text = "";
                        txtPMiddle.Text = "";
                        date1.Value = null;
                        date2.Value = null;
                        lblAisatu1.Text = "を選択してから、中項目入力をお願い致します。";
                        CreateDataGrid(SessionManager.project.PdbRow.Pid);
                    }
                    else
                    {
                        lblCalendarError.Text = "<font color=red>カレンダーに誤りがあります。<font>";
                    }
                    
                }
                else
                {
                    if (date1.Value == ""&& date2.Value != "")
                    {
                        lblStart.Text = "開始<font color=red>(必須)<font>";
                        lblOver.Text = "終了";
                    }
                    if (date2.Value == ""&& date1.Value != "")
                    {
                        lblStart.Text = "開始";
                        lblOver.Text = "終了<font color=red>(必須)<font>";
                    }
                    if (date2.Value == "" && date1.Value == "")
                    {
                        lblStart.Text = "開始<font color=red>(必須)<font>";
                        lblOver.Text = "終了<font color=red>(必須)<font>";
                    }
                    lblAisatu1.Text = "を選択してから、中項目入力をお願い致します。";
                }
            }
            else
            {
                lblAisatu1.Text = "<font color=red>を選択してから、中項目入力をお願い致します。<font>";
            }
            DgPKanri.Visible = true;
            wbs.Visible = true;
            btnWBS.Visible = true;
        }

        protected void DgPKanri_EditCommand(object source, DataGridCommandEventArgs e)
        {
            DgPKanri.EditItemIndex = e.Item.ItemIndex;
            CreateDataGrid(SessionManager.project.PdbRow.Pid);
        }

        protected void DgPKanri_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            DgPKanri.EditItemIndex = -1;
            CreateDataGrid(SessionManager.project.PdbRow.Pid);
        }

        protected void DgPKanri_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            TextBox txtPMiddlename = (TextBox)e.Item.FindControl("PMiddlename");
            string xx = e.Item.Cells[6].Text;
            string yy = e.Item.Cells[7].Text;
            //TextBox txtPMiddleover = (TextBox)e.Item.FindControl("txtPMiddleover");
            DropDownList dropDownListChange = e.Item.FindControl("ddpShintyokuChange") as DropDownList;
            DATASET.DataSet.T_PdbKanriDataTable t_PdbKanris = new DATASET.DataSet.T_PdbKanriDataTable();
            DATASET.DataSet.T_PdbKanriRow t_PdbKanriRow = t_PdbKanris.NewT_PdbKanriRow();

            t_PdbKanriRow[3] = txtPMiddlename.Text;
            //t_PdbKanriRow[4] = txtPMiddlestart;
            //t_PdbKanriRow[5] = txtPMiddleover.Text;
            t_PdbKanriRow[6] = SessionManager.User.M_User.id.Trim();
            t_PdbKanriRow[7] = DateTime.Now;
            t_PdbKanriRow[9] = dropDownListChange.SelectedItem.Value;
            Update.UpdateMiddle(t_PdbKanriRow, e.Item.Cells[0].Text, e.Item.Cells[2].Text);
            DgPKanri.EditItemIndex = -1;
            CreateDataGrid(SessionManager.project.PdbRow.Pid);
        }

        protected void DgPKanri_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName== "Delete"|| e.CommandName == "uebig" || e.CommandName == "sitabig" || e.CommandName == "uemiddle" || e.CommandName == "sitamiddle")
            {
                string name = e.Item.Cells[0].Text;
                string[] arr = name.Split('>');
                string bigname = arr[1];
                string Middleid = e.Item.Cells[2].Text;
                switch (e.CommandName)
                {
                    case "Delete":
                        Delete.DeleteMiddle(bigname, Middleid, SessionManager.project.PdbRow.Pid);
                        break;
                    case "uebig":
                        Narabi.uebig(Global.GetConnection(), bigname, SessionManager.project.PdbRow.Pid);
                        CreateDataGrid(SessionManager.project.PdbRow.Pid);
                        break;
                    case "sitabig":
                        Narabi.sitabig(Global.GetConnection(), bigname, SessionManager.project.PdbRow.Pid);
                        CreateDataGrid(SessionManager.project.PdbRow.Pid);
                        break;
                    case "uemiddle":
                        Narabi.uemiddle(Global.GetConnection(), Middleid, bigname, SessionManager.project.PdbRow.Pid);
                        CreateDataGrid(SessionManager.project.PdbRow.Pid);
                        break;
                    case "sitamiddle":
                        Narabi.sitamiddle(Global.GetConnection(), Middleid, bigname, SessionManager.project.PdbRow.Pid);
                        CreateDataGrid(SessionManager.project.PdbRow.Pid);
                        break;
                    default:
                        // Do nothing.
                        break;

                }
                DgPKanri.EditItemIndex = -1;
                CreateDataGrid(SessionManager.project.PdbRow.Pid);
            }
                
        }

        protected void btnDeleteBig_Click(object sender, EventArgs e)
        {
            Delete.DeleteBig(ddlPBigList.Text, SessionManager.project.PdbRow.Pid);
            CreateDataGrid(SessionManager.project.PdbRow.Pid);
            ddlPBigList.Items.Clear();
            ddlPBigList.Items.Add("");
            CreateDropDownList(SessionManager.project.PdbRow.Pid);
            ddlPBigList.Text = "";
        }

        protected void btnWBS_Click(object sender, EventArgs e)
        {
            DateTime Time1 = WBS.GetPMiddleTimeRow(Global.GetConnection(), SessionManager.project.PdbRow.Pid).PMiddlestart;
            DateTime Time2 = WBS.GetPMiddleTimeRow(Global.GetConnection(), SessionManager.project.PdbRow.Pid).PMiddleover;

            int interval = (int)(Time2 - Time1).TotalDays + 1;
            ar = new DateTime[interval];

            for (int i = 0; i < interval; i++)
            {
                ar[i] = Time1.AddDays(i);
            }
            DATASET.DataSet.T_PdbKanriDataTable dt = GetT_PdbKanriDataTable(Global.GetConnection(), SessionManager.project.PdbRow.Pid);
            wbs.DataSource = dt;
            wbs.DataBind();
            wbs.Visible = true;
        }
        public DateTime[] ar;
        protected void wbs_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            for (int i = 0; i < ar.Length; i++)
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    TableCell cell = new TableCell();
                    if (ar[i].Day<10)
                    {
                        if (ar[i].Day==1)
                        {
                            cell.Height = 50;
                            cell.BorderWidth = 3;
                            cell.BorderColor = Color.Black;
                            cell.Controls.Add(new LiteralControl("<font size = 1px>" + ar[i].Month+ "月"+ Environment.NewLine + "0" + ar[i].Day));
                        }
                        else
                        {
                            cell.Controls.Add(new LiteralControl("<font size = 1px>" +"0"+ ar[i].Day.ToString()));
                        }
                    }
                    else
                    {
                        cell.Controls.Add(new LiteralControl("<font size = 1px>" + ar[i].Day.ToString()));//ヘッダー
                    }
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
                    e.Item.Cells.Add(cell);
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtPBig.Text = "";
            ddlPBigList.Text = "";
            ddpShintyoku.Text = "";
            txtPMiddle.Text = "";
            lblStart.Text = "開始";
            lblOver.Text = "開始";
            date1.Value = "";
            date2.Value = "";
            lblCalendarError.Text = "";
            lblAisatu1.Text = "を選択してから、中項目入力をお願い致します。";
            wbs.DataSource = null;
            DgPKanri.Visible = false;
            wbs.Visible = false;
            btnWBS.Visible = false;
        }
    }
}