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


namespace WhereEver
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string ID = TbxID.Text.Trim();
            string PW = TbxPW.Text.Trim();

            //GetM_UserRowでチェック、テーブルにあれば戻り値dt[0]=、なければnull
            DATASET.DataSet.M_UserRow dr = Class.Login.GetM_UserRow(ID, PW, Global.GetConnection());

            if (dr == null)
            {
                lblError.Text = "ログインできませんでした<br/>ログインIDまたはパスワードをお確かめ下さい";
                return;
            }

            SessionManager.Login(dr);
            DATASET.DataSet.T_LoginListDataTable dd = Class2.GetLoginListDataTable(Global.GetConnection());
            DATASET.DataSet.T_LoginListRow dl = dd.NewT_LoginListRow();

            dl.name = dr.name;
            dl.Date = DateTime.Now;
            dd.AddT_LoginListRow(dl);
            Class.Login.InsertLoginList(dd, Global.GetConnection());

            Response.Redirect("LoginList.aspx", false);
        }
    }
}