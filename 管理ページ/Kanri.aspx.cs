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

            //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。

            /*
            //コマンドの引数を取得
            int args = Int32.Parse(e.CommandArgument.ToString());

            //参照してみると最初からトリム不要の状態になっていることが多い
            //String isbn_key = (String)GridView1.DataKeys[args].Value;
            string isbn_name1 = GridView1.Rows[args].Cells[2].Text.Trim();
            string isbn_pw = GridView1.Rows[args].Cells[3].Text.Trim();

            //trimした値をもどす（意味なし）
            GridView1.Rows[args].Cells[2].Text = isbn_name1;
            GridView1.Rows[args].Cells[3].Text = isbn_pw;

            //SessionManagerに代入
            SessionManager.User.M_User.id = isbn_name1;
            SessionManager.User.M_User.pw = isbn_pw;
            */

            //なにもしない
            return;
        }

    }
}