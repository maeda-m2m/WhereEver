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


        /* GridView1はデータバインドしているためUpdateMethod="MyGrid_Update" は追加できません。*/
        void MyGrid_Update(object sender, DataGridCommandEventArgs e)
        {
            TextBox tb1 = (TextBox)e.Item.Cells[1].Controls[0];
            TextBox tb2 = (TextBox)e.Item.Cells[2].Controls[0];
            TextBox tb3 = (TextBox)e.Item.Cells[1].Controls[0];
            TextBox tb4 = (TextBox)e.Item.Cells[2].Controls[0];

            // tb1.Textとtb2.Textの値によりデータソースを更新
          
           // GridView1.EditItemIndex = -1;
        //BindMyGrid();
        }


        //例
        /*

        void MyGrid_Edit(object sender, DataGridCommandEventArgs e) {
         MyGrid.EditItemIndex = e.Item.ItemIndex;
        BindMyGrid();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //string test = ((TextBox)(GridView1.Rows[e.NewEditIndex].FindControl("TextBox1"))).Text;
        }

        void MyGrid_Cancel(object sender, DataGridCommandEventArgs e) {
         MyGrid.EditItemIndex = -1;
        BindMyGrid();
        }

        void MyGrid_Update(object sender, DataGridCommandEventArgs e)
        {
            TextBox tb1 = (TextBox)e.Item.Cells[1].Controls[0];
            TextBox tb2 = (TextBox)e.Item.Cells[2].Controls[0];

            // tb1.Textとtb2.Textの値によりデータソースを更新

            MyGrid.EditItemIndex = -1;
            BindMyGrid();
        }
        */

    }
}