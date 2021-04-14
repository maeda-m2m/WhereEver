using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing.Printing;
using WhereEver.DATASET;
using WhereEver.Class;

namespace WhereEver
{
    public partial class Chat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Create();
            }
        }

        private void Create()
        {
            DATASET.DataSet.T_ChatDataTable dt = WhereEver.Class.Chat.GetChatDataTable(Global.GetConnection());
            ChatArea.DataSource = dt;
            ChatArea.DataBind();
        }


        protected void Chat_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DATASET.DataSet.T_ChatRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_ChatRow;

                Label No = e.Item.FindControl("No") as Label;
                Label Name = e.Item.FindControl("ID") as Label;
                Label Date = e.Item.FindControl("Date") as Label;
                Label Naiyou = e.Item.FindControl("Naiyou") as Label;

                No.Text = dr.No.ToString();

                Name.Text = "<font color=#16ba00>" + dr.Name + "</font>";

                Date.Text = "<font size=1px>" + dr.Date.ToShortTimeString() + "</font>";

                Naiyou.Text = dr.Naiyou;

                Label1.Text = SessionManager.User.M_User.name1;
            }
        }

        protected void Send_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_ChatDataTable dt = new DATASET.DataSet.T_ChatDataTable();
            DATASET.DataSet.T_ChatRow dr = dt.NewT_ChatRow();

            dr.Date = DateTime.Now;
            if (TextBox1.Text == "")
            {
                string.Format("if (!confirm('{0}')) return false;", "本文が入力されていません");
            }
            dr.Name = Label1.Text;
            dr.Naiyou = TextBox1.Text;

            DATASET.DataSet.T_ChatRow dl = Class2.MaxNoRow(Global.GetConnection());
            int sl = dl.No;
            dr.No = sl + 1;
            dt.AddT_ChatRow(dr);
            Class2.InsertList(dt, Global.GetConnection());
            Create();

         
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            Create();
        }

        protected void ChatArea_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Label Cno = (Label)e.Item.Cells[0].FindControl("No");
            string Cid = Cno.Text;
            switch (((LinkButton)e.CommandSource).CommandName)
            {

                case "Delete":
                    Class.Chat.DeleteChat(Cid);
                    break;

                // Add other cases here, if there are multiple ButtonColumns in 
                // the DataGrid control.

                default:
                    // Do nothing.
                    break;

            }
            Create();
        }
    }
}