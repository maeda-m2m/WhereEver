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
                txtHenshin.Visible = false;
                lblHenshin.Visible = false;
                btnHenshin.Visible = false;
                txtHozon.Visible = false;
                Return.Visible = false;
            }
            Label2.Text = "";

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
                Label Id = e.Item.FindControl("Id") as Label;
                Label Name = e.Item.FindControl("Name") as Label;
                Label Date = e.Item.FindControl("Date") as Label;
                Label Naiyou = e.Item.FindControl("Naiyou") as Label;
                if (dr.HentouNo == 0)
                {
                    No.Text = dr.No.ToString();
                }
                else
                {
                    No.Text = "";
                }

                Id.Text = dr.Id;

                Name.Text = dr.Name;

                Date.Text = "<font size=1px>" + dr.Date.ToString() + "</font>";

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
           
            dr.Id = SessionManager.User.ID; //変更
            dr.Name = Label1.Text;
            dr.Naiyou = TextBox1.Text;
            dr.HentouNo = 0;

            DATASET.DataSet.T_ChatRow dl = Class2.MaxNoRow(Global.GetConnection());
            int sl = dl.No;
            dr.No = sl + 1;
            dt.AddT_ChatRow(dr);
            Class2.InsertList(dt, Global.GetConnection());
            TextBox1.Text = "";
            Create();

        }
        protected void Update_Click(object sender, EventArgs e)
        {
            Create();
        }

        protected void ChatArea_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Label Cno = (Label)e.Item.Cells[0].FindControl("No");
            Label Cname = (Label)e.Item.Cells[1].FindControl("Id"); //変更
            Label Cnaiyou = (Label)e.Item.Cells[3].FindControl("Naiyou");
            string Cid = Cno.Text;
            string CnameNow = Cname.Text.Trim();
            string id = SessionManager.User.M_User.id.Trim();
            string cnaiyou = Cnaiyou.Text;
            switch (((LinkButton)e.CommandSource).CommandName)
            {
                case "Delete":
                    if (CnameNow == id)
                    {
                        if (Cid == "")
                        {
                            Class.Chat.DeleteHenshin(cnaiyou,id);
                        }
                        else
                        {
                            Class.Chat.DeleteChat(Cid);
                        }
                        
                        //Class.Chat.UpdateChat(Global.GetConnection());
                    }
                    else
                    {
                        Label2.Text = "他の人のコメントは削除できません！";
                    }
                break;
                case "Reply":
                    btnHenshin.Visible = true;
                    txtHenshin.Visible = true;
                    lblHenshin.Visible = true;
                    TextBox1.Visible = false;
                    txtHozon.Text = Cid;
                    lbl.Text = cnaiyou;
                    Send.Visible = false;
                    Return.Visible = true;
                    break;
            }
            Create();
        }

        protected void btnHenshin_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_ChatDataTable dt = new DATASET.DataSet.T_ChatDataTable();
            DATASET.DataSet.T_ChatRow dr = dt.NewT_ChatRow();
            if (txtHozon.Text=="")
            {
                DATASET.DataSet.T_ChatRow dr1 = Class.Chat.GetMaxHentouRow(Global.GetConnection(), lbl.Text);
                if (dr1.HentouNo == 0)
                {
                    dr.HentouNo = 1;

                }
                else
                {
                    dr.HentouNo = dr1.HentouNo + 1;
                }
                dr.No = dr1.No;
            }
            else
            {
                DATASET.DataSet.T_ChatRow dr2 = Class.Chat.GetMaxHentouNoRow(Global.GetConnection(), txtHozon.Text);
                if (dr2.HentouNo == 0)
                {
                    dr.HentouNo = 1;

                }
                else
                {
                    dr.HentouNo = dr2.HentouNo + 1;
                }
                dr.No = Int32.Parse(txtHozon.Text);
            }
            
            
            dr.Date = DateTime.Now;
            dr.Id = SessionManager.User.ID; //変更
            dr.Name = Label1.Text;
            dr.Naiyou = txtHenshin.Text;

            dt.AddT_ChatRow(dr);
            Class2.InsertList(dt, Global.GetConnection());
            Create();
        }

        protected void Return_Click(object sender, EventArgs e)
        {
            Response.Redirect("Chat.aspx");
        }
    }
}