using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WhereEver.スケジュール
{
    public partial class test1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void TestGV_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    var ltr = new Literal();

            //    ltr.Text = $"総ページ数:{TestGV.PageCount}";

            //    e.Row.Cells[e.Row.Cells.Count - 1].Wrap = false;
            //    e.Row.Cells[e.Row.Cells.Count - 1].Controls.Add(ltr);


            //}
        }
    }
}