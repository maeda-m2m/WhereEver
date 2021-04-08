using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver.管理ページ
{
    public partial class Kanri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblResult.Text = SessionManager.User.M_User.id;
        }


        //GridViewのRowCommand属性に参照可能なメソッドを入力すると↓が自動生成されます。
        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int args = Int32.Parse((String)e.CommandArgument);

            switch (e.CommandName)
            {
                // ［更新］ボタンがクリックされた場合
                case "MyGrid_Update":
                    GridView1.Rows[args].BackColor = System.Drawing.Color.Azure;
                    break;

                // ［ハイライト］ボタンがクリックされた場合
                case "Highlight":
                    GridView1.Rows[args].BackColor = System.Drawing.Color.Azure;
                    break;

                // ［解除］ボタンがクリックされた場合
                case "Clear":
                    GridView1.Rows[args].BackColor = System.Drawing.Color.Empty;
                    break;
            }
        }

    }
}