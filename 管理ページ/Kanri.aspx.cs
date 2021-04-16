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
        //GridViewが読み込まれたときに実行されます。
        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //好きなコードを入れて下さい。


        }

        protected void grid_RowUpdatedCommand(object sender, GridViewUpdatedEventArgs e)
        {

            GridView2.DataBind();

            //列
            int args = 0;

            //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
            // クリックされた[args]行の左から2番目の列[0-nで数える]のセルにある「テキスト」を取得
            //【重要】ReadOnly属性がついていないと読み込みできない。

            //idをロード（必要なら）
            //string isbn_name = GridView2.Rows[args].Cells[0].Text.Trim();

            //名前をロード
            string isbn_name1 = GridView2.Rows[args].Cells[2].Text.Trim();

            //passwordをロード
            string isbn_pw = GridView2.Rows[args].Cells[3].Text.Trim();

            if (isbn_name1 != null || isbn_pw != null) {
                //SessionManagerに代入
                SessionManager.User.M_User.name1 = isbn_name1;
                SessionManager.User.M_User.pw = isbn_pw;
            }
            return;
        }
    }
}