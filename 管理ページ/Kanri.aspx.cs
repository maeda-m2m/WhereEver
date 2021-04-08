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



        /*
                //DataSourseの設定です。OnUpdate属性にメソッドを設定します。アップデート完了後に実行されます。
                private void OnDSUpdatedHandler(Object source, SqlDataSourceStatusEventArgs e)
                {
                    if (e.AffectedRows > 0)
                    {
                        // Perform any additional processing, 
                        // such as setting a status label after the operation.
                        lblResult2.Text = Request.LogonUserIdentity.Name +
                            " changed user information successfully!";
                    }
                    else
                    {
                        lblResult2.Text = "No data updated!";
                    }
                }
        */

        //GridViewのRowCommand属性に参照可能なメソッドを入力すると↓が自動生成されます。
        //GridViewが読み込まれたときに実行されます。
        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //好きなコードを入れて下さい。以下は例です。
            /*
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
            */

            //なにもしない
            return;
        }

    }
}