using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using static System.Web.HttpUtility;

namespace WhereEver
{
    public partial class LoginList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DATASET.DataSet.T_LoginListDataTable dtLogin = Class2.GetLoginListDataTable(Global.GetConnection());

                DgTimeDetail.DataSource = dtLogin;
                DgTimeDetail.DataBind();

                Label_WhatNow.Text = "";

                TextBox_EditTop.Text = "";
                Button_EditTop.Visible = true;
                Panel_EditTop.Visible = false;

            }

            //宣言と初期化
            StringBuilder @sb = new StringBuilder();

            //WhatNowをロード
            Label_WhatNow.Text = "";
            DATASET.DataSet.T_ScheduleDataTable dts = Class.Schedule.GetT_ScheduleDataTable(Global.GetConnection(), DateTime.Now.ToString());
            if(dts != null)
            {
                for(int i = 0;  i < dts.Count; i++)
                {
                    //初期化
                    @sb = new StringBuilder();
                    if (!dts[i].IsNull(@"title"))
                    {
                        @sb.Append(dts[i].title);
                    }
                    if (!dts[i].IsNull(@"time"))
                    {
                        @sb.Append(@"　");
                        @sb.Append(dts[i].time);
                        @sb.Append(@"～");
                    }
                    if (!dts[i].IsNull(@"name"))
                    {
                        if(dts[i].name != "")
                        {
                            @sb.Append(@"　担当：");
                            @sb.Append(dts[i].name);
                        }
                    }

                    @sb.Append("<br />");
                    Label_WhatNow.Text += @sb.ToString();
                }
            }
            else
            {
                Label_WhatNow.Text = @"本日の予定はありません。<br />";
            }

            //初期化
            @sb = new StringBuilder();
            @sb.Append("<br /><div css= \"index1\">--お知らせ--</div><br />");
            //ここに更新内容をAppend
            DATASET.DataSet.T_TopPageRow dr = Class.Toppage.GetT_TopPage(Global.GetConnection());
            if(dr != null)
            {
                if (dr.TopPage != "")
                {
                    @sb.Append(dr.TopPage);
                    @sb.Replace("<li>", "<li type=\"circle\">");
                    @sb.Append("<br />");
                    @sb.Append("最終更新：");
                    @sb.Append(dr.DateTime);
                }
                else
                {
                    @sb.Append("なし");
                }
            }
            else
            {
                @sb.Append("警告：T_TopPageのデータベースがNULLです。");
            }
            @sb.Append("<br />");
            Label_WhatNow.Text += @sb.ToString();
        }

        protected void DgTimeDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                DATASET.DataSet.T_LoginListRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_LoginListRow;
                Label lblName = e.Item.FindControl("lblName") as Label;
                Label lblLoginTime = e.Item.FindControl("lblLoginTime") as Label;
                Label lblLogoutTime = e.Item.FindControl("lblLogoutTime") as Label;
                DATASET.DataSet.T_LoginListRow dl = Class.Login.UserLoginMAXTime(Global.GetConnection(), dr.name.ToString());
                DATASET.DataSet.T_LogoutListRow dt = Class.Login.UserLogoutMAXTime(Global.GetConnection(), dr.name.ToString());

                DATASET.DataSet.M_UserRow ml = Class2.Getname1(Global.GetConnection(), dr.name.ToString());
                lblName.Text = ml.name1.ToString();
                lblLoginTime.Text = dl.Date.ToString();
                if(dt == null)
                {
                    lblLogoutTime.Text = "";
                }
                else
                {
                    lblLogoutTime.Text = dt.Date.ToString();
                }
                
            }
        }

        protected void btnOut_Click(object sender, EventArgs e)
        {
            Class.Logout.InsertLogoutList(Global.GetConnection());
            this.Response.Redirect("Login.aspx", false);
        }

        protected void btnKanri_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("../管理ページ/Kanri.aspx", false);
        }

        protected void btnEditTop_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_TopPageRow dr = Class.Toppage.GetT_TopPage(Global.GetConnection());
            if (dr != null)
            {
                TextBox_EditTop.Text = dr.TopPage;
            }
            Button_EditTop.Visible = false;
            Panel_EditTop.Visible = true;
        }

        protected void btnReformTop_Click(object sender, EventArgs e)
        {
            //sql更新 ※全部Nullだと更新できません。DBのDateTimeには何か値を入れておいて下さい。

            StringBuilder sb = new StringBuilder();
            sb.Append(HtmlEncode(TextBox_EditTop.Text));
            //一部タグのみ許可
            sb.Replace("&lt;br /&gt;", "<br />");
            sb.Replace("&lt;p&gt;", "<p>");
            sb.Replace("&lt;/p&gt;", "</p>");
            sb.Replace("&lt;ol&gt;", "<ol>");
            sb.Replace("&lt;/ol&gt;", "</ol>");
            sb.Replace("&lt;li&gt;", "<li>");
            sb.Replace("&lt;/li&gt;", "</li>");
            sb.Replace("&lt;li&gt;", "<ul>");
            sb.Replace("&lt;/li&gt;", "</ul>");
            Class.Toppage.SetT_TopPageUpdate(Global.GetConnection(), sb.ToString());
            //リダイレクト
            this.Response.Redirect("LoginList.aspx", false);
        }

        protected void btnReformTopEnd_Click(object sender, EventArgs e)
        {
            Button_EditTop.Visible = true;
            Panel_EditTop.Visible = false;
            TextBox_EditTop.Text = "";
        }

        protected void btnReformTopDel_Click(object sender, EventArgs e)
        {
            TextBox_EditTop.Text = "";
        }

        protected void btnReformTopReload_Click(object sender, EventArgs e)
        {
            //再読み込み
            DATASET.DataSet.T_TopPageRow dr = Class.Toppage.GetT_TopPage(Global.GetConnection());
            if (dr != null)
            {
                TextBox_EditTop.Text = dr.TopPage;
            }
        }
    }
}