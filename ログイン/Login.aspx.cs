﻿using System;
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


namespace WhereEver
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ImgBtn_Click(object sender, ImageClickEventArgs e)
        {
            string ID = TbxID.Text.Trim();
            string PW = TbxPW.Text.Trim();

            //GetM_UserRowでチェック、テーブルにあれば戻り値dt[0]=1、なければnull
            DataSet2.M_UserRow dr = Class1.GetM_UserRow(ID, PW, Global.GetConnection());

            if (dr == null)
            {
                LblError.Text = "ログインできませんでした<br/>ログインIDまたはパスワードをお確かめ下さい";
                return;
            }

            SessionManager.Login(dr);
            DataSet2.T_LoginListDataTable dd = Class2.GetLoginListDataTable(Global.GetConnection());
            DataSet2.T_LoginListRow dl = dd.NewT_LoginListRow();

            dl.name = dr.name;
            dl.Date = DateTime.Now;
            dd.AddT_LoginListRow(dl);
            Class2.InsertLoginList(dd, Global.GetConnection());
                
            this.Response.Redirect("LoginList.aspx");
        }
    }
}