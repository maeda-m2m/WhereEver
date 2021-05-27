using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using static System.Web.HttpUtility;

namespace WhereEver.Money
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetMaxYear();
                ResetMonthItem();
                ResetPL();

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");
                Session.Add("uuid", (string)"null");
            }

            SetMaxDate();

            if (DropDownList_PL_month_s.SelectedValue != "--")
            {
                DropDownList_PL_month_s.BackColor = System.Drawing.Color.White;
            }
            if (DropDownList_PL_day_s.SelectedValue != "--")
            {
                DropDownList_PL_day_s.BackColor = System.Drawing.Color.White;
            }
            if (DropDownList_PL_month_g.SelectedValue != "--")
            {
                DropDownList_PL_month_g.BackColor = System.Drawing.Color.White;
            }
            if (DropDownList_PL_day_g.SelectedValue != "--")
            {
                DropDownList_PL_day_g.BackColor = System.Drawing.Color.White;
            }

            if (DropDownList_BS_month.SelectedValue != "--")
            {
                DropDownList_BS_month.BackColor = System.Drawing.Color.White;
            }
            if (DropDownList_BS_day.SelectedValue != "--")
            {
                DropDownList_BS_day.BackColor = System.Drawing.Color.White;
            }


        }


        protected void SetMaxYear()
        {
            //---------------------------------------------------------------------------------------------
            //---------------------------------------------------------------------------------------------

            //年を初期化
            DeleteYearItem();

            string item;

            //翌々年を取得
            item = DateTime.Today.Year.ToString();
            item = (int.Parse(item) + 2).ToString();

            //---------------------------------------
            //翌年を取得
            item = DateTime.Today.Year.ToString();
            item = (int.Parse(item) + 1).ToString();
            SetYearItem(0, item);

            //今年を取得
            item = DateTime.Today.Year.ToString();
            SetYearItem(0, item);

            //去年を取得
            item = DateTime.Today.Year.ToString();
            item = (int.Parse(item) - 1).ToString();
            SetYearItem(0, item);

            //本年を日付選択肢に適用
            SetYearValue(DateTime.Today.Year.ToString());

            //---------------------------------------------------------------------------------------------
        }

        protected int GetDateMax(int y, int m)
        {
            int maxday = 31;

            //2 4 6 9 11月は30日まで
            if (m == 2 || m == 4 || m == 6 || m == 9 || m == 11)
            {
                maxday = 30;

                if (m == 2)
                {
                    maxday = 28;
                    //今年が閏年であるか確かめる
                    if (DateTime.IsLeapYear(y))
                    {
                        //閏年
                        maxday = 29;
                    }

                }

            }
            else
            {
                maxday = 31;
            }

            return maxday;

        }

        /// <summary>
        /// DropDownList_yearを初期化します。
        /// </summary>
        protected void DeleteYearItem()
        {
            DropDownList_PL_year_s.Items.Clear();
            DropDownList_PL_year_g.Items.Clear();
            DropDownList_BS_year.Items.Clear();
        }

        /// <summary>
        /// DropDownList_monthを初期化します。
        /// </summary>
        protected void ResetMonthItem()
        {
            DropDownList_PL_month_s.Items.Clear();
            DropDownList_PL_month_s.Items.Insert(0, "--");
            DropDownList_PL_month_g.Items.Clear();
            DropDownList_PL_month_g.Items.Insert(0, "--");

            DropDownList_BS_month.Items.Clear();
            DropDownList_BS_month.Items.Insert(0, "--");

            for (int i=1; i <= 12; i++) {
                DropDownList_PL_month_s.Items.Insert(i, i.ToString());
                DropDownList_PL_month_g.Items.Insert(i, i.ToString());
                DropDownList_BS_month.Items.Insert(i, i.ToString());
            }

        }

        /// <summary>
        /// DropDownList_yearに年の項目を入力します。
        /// DropDownList.Items.Insert(int p1, string p2);
        /// </summary>
        /// <param name="i">index</param>
        /// <param name="item">item</param>
        protected void SetYearItem(int i, string item)
        {
            DropDownList_PL_year_s.Items.Insert(i, item);
            DropDownList_PL_year_g.Items.Insert(i, item);
            DropDownList_BS_year.Items.Insert(i, item);
        }

        /// <summary>
        /// 特殊なDwopDownList_yearを任意（現在）の年にします。
        /// </summary>
        /// <param name="i"></param>
        /// <param name="item"></param>
        protected void SetYearValue(string item)
        {
            DropDownList_PL_year_s.SelectedValue = (int.Parse(item) - 1).ToString();
            DropDownList_PL_year_g.SelectedValue = item;
            DropDownList_BS_year.SelectedValue = item;
        }

        /// <summary>
        /// 日付入力を全初期化します。
        /// </summary>
        protected void SetResetDate()
        {
            //day
            //初期化
            DropDownList_PL_day_s.Items.Clear();
            DropDownList_PL_day_s.Items.Insert(0, "--");
            DropDownList_PL_day_g.Items.Clear();
            DropDownList_PL_day_g.Items.Insert(0, "--");
            DropDownList_BS_day.Items.Clear();
            DropDownList_BS_day.Items.Insert(0, "--");
            //-------------------------------------------------------------------------------

            //month
            //初期化
            DropDownList_PL_month_s.SelectedValue = "--";
            DropDownList_PL_month_g.SelectedValue = "--";
            DropDownList_BS_month.SelectedValue = "--";

            SetMaxYear();
        }

        protected void SetMaxDate()
        {
            //---------------------------------------------------------------------------------
            //選択可能な日付の最大値を設定
            //---------------------------------------------------------------------------------

            //初期化
            int i = 0;
            int m = 0;
            int maxday = 0;
            string memory1 = "--";
            string memory2 = "--";

            //memory
            memory1 = DropDownList_PL_day_s.SelectedValue;
            memory2 = DropDownList_PL_day_g.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }
            if (memory2 == "--" || memory2 == "")
            {
                memory2 = "0";
            }


            //初期化
            DropDownList_PL_day_s.Items.Clear();
            DropDownList_PL_day_s.Items.Insert(0, "--");
            DropDownList_PL_day_g.Items.Clear();
            DropDownList_PL_day_g.Items.Insert(0, "--");
            int y;

            y = int.Parse(DropDownList_PL_year_s.SelectedValue);
            if (DropDownList_PL_month_s.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_PL_month_s.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_PL_day_s.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_PL_day_s.Items.Insert(i, i.ToString());
                }
            }

            y = int.Parse(DropDownList_PL_year_g.SelectedValue);
            if (DropDownList_PL_month_g.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_PL_month_g.SelectedValue);
                maxday = GetDateMax(y, m);
                memory2 = Math.Min(int.Parse(memory2), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_PL_day_g.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory2 = Math.Min(int.Parse(memory2), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_PL_day_g.Items.Insert(i, i.ToString());
                }
            }
            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }
            if (memory2 == "0")
            {
                memory2 = "--";
            }

            DropDownList_PL_day_s.SelectedValue = memory1;
            DropDownList_PL_day_g.SelectedValue = memory2;



            //---------------------------------------------------------------------------------


            //memory
            memory1 = DropDownList_BS_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_BS_day.Items.Clear();
            DropDownList_BS_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_BS_year.SelectedValue);
            if (DropDownList_BS_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_BS_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_BS_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_BS_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_BS_day.SelectedValue = memory1;
            //---------------------------------------------------------------------------------


        }


        //------------------------------------------------------------------------------------------------------------------------------------------
        //P/L
        //------------------------------------------------------------------------------------------------------------------------------------------


        protected void Push_PL_test(object sender, EventArgs e)
        {
            if (Panel_PL.Visible)
            {
                Panel_PL.Visible = false;
            }
            else
            {
                Panel_PL.Visible = true;
            }
        }

        protected void Push_CheckAS_PL(object sender, EventArgs e)
        {
            Check_PL(true);
        }

        protected void Push_Check_PL(object sender, EventArgs e)
        {
            Check_PL(false);
        }

        /// <summary>
        /// P/Lを保存します。
        /// </summary>
        /// <param name="b">trueなら上書き保存（データがあれば）</param>
        protected void Check_PL(bool b = false)
        {
            Sum_PL();

            if (DropDownList_PL_month_s.SelectedValue == "--")
            {
                DropDownList_PL_month_s.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (DropDownList_PL_day_s.SelectedValue == "--")
            {
                DropDownList_PL_day_s.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (DropDownList_PL_month_g.SelectedValue == "--")
            {
                DropDownList_PL_month_g.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (DropDownList_PL_day_g.SelectedValue == "--")
            {
                DropDownList_PL_day_g.BackColor = System.Drawing.Color.Red;
                return;
            }


            //宣言と初期化
            string str;

            str = HtmlEncode(TextBox_Uriage.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value1);

            str = HtmlEncode(TextBox_UriageGenka.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value2);

            str = HtmlEncode(TextBox_HanbaiKanrihi.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value3);

            str = HtmlEncode(TextBox_EigyouRieki.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value4);

            str = HtmlEncode(TextBox_EigyougaiHiyou.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value5);

            str = HtmlEncode(TextBox_TokubetsuRieki.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value6);

            str = HtmlEncode(TextBox_TokubetsuSonshitsu.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value7);

            str = HtmlEncode(TextBox_Houjinzei.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value8);

            DateTime dts = new DateTime();
            DateTime dtg = new DateTime();

            StringBuilder sb = new StringBuilder();
            sb.Append(DropDownList_PL_year_s.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_PL_month_s.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_PL_day_s.SelectedValue);
            dts = DateTime.Parse(sb.ToString());


            sb = new StringBuilder();
            sb.Append(DropDownList_PL_year_g.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_PL_month_g.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_PL_day_g.SelectedValue);
            dtg = DateTime.Parse(sb.ToString());

            string uuid = Session["uuid"].ToString();

            if (b)
            {
                DATASET.DataSet.T_PLRow dr = ClassLibrary.MOMClass.GetT_PLRow(Global.GetConnection(), uuid);
                if (dr != null)
                {
                    ClassLibrary.MOMClass.SetT_PLUpdate(Global.GetConnection(), uuid, value1, value2, value3, value4, value5, value6, value7, value8, dts, dtg);
                }
                else
                {
                    b = false;
                }
            }
            
            if(!b)
            {
                ClassLibrary.MOMClass.SetT_PLInsert(Global.GetConnection(), value1, value2, value3, value4, value5, value6, value7, value8, dts, dtg);
            }

            GridView_PL.DataBind();
        }


        protected void Change_PL(object sender, EventArgs e)
        {
            Sum_PL();
        }

        protected void Sum_PL()
        {
            //宣言と初期化
            int value;
            int pl_sum = 0;
            double ri_r;
            string str;

            str = TextBox_Uriage.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;
            int uriagedaka = value;

            str = TextBox_UriageGenka.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            Label_UriageSourieki.Text = string.Format("{0:C}", pl_sum);

            //売上総利益率（％）＝売上総利益÷売上高×100%
            if ((float)uriagedaka != 0)
            {
                ri_r = (float)pl_sum / (float)uriagedaka;
            }
            else
            {
                ri_r = 0f;
            }
            Label_ArariR.Text = string.Format("{0:0.0%}", (double)ri_r);

            str = TextBox_HanbaiKanrihi.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            str = TextBox_EigyouRieki.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            //売上高営業利益率（％）＝営業利益÷売上高×100%
            if ((float)uriagedaka != 0)
            {
                ri_r = (float)value / (float)uriagedaka;
            }
            else
            {
                ri_r = 0f;
            }
            Label_EigyouR.Text = string.Format("{0:0.0%}", (double)ri_r);

            str = TextBox_EigyougaiHiyou.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            Label_KeijyouRieki.Text = string.Format("{0:C}", pl_sum);
            //売上高経常利益率（％）＝経常利益÷売上高×100%
            if ((float)uriagedaka != 0)
            {
                ri_r = (float)pl_sum / (float)uriagedaka;
            }
            else
            {
                ri_r = 0f;
            }
            Label_KeijyouR.Text = string.Format("{0:0.0%}", (double)ri_r);

            str = TextBox_TokubetsuRieki.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            Label_Zeibikimae.Text = string.Format("{0:C}", pl_sum);

            str = TextBox_TokubetsuSonshitsu.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            str = TextBox_Houjinzei.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            Label_Jyunrieki.Text = string.Format("{0:C}", pl_sum);
        }

        protected void ResetPL()
        {
            TextBox_Uriage.Text = "0";
            TextBox_UriageGenka.Text = "0";
            TextBox_HanbaiKanrihi.Text = "0";
            TextBox_EigyouRieki.Text = "0";
            TextBox_EigyougaiHiyou.Text = "0";
            TextBox_TokubetsuRieki.Text = "0";
            TextBox_TokubetsuSonshitsu.Text = "0";
            TextBox_Houjinzei.Text = "0";
            
            Label_UriageSourieki.Text = string.Format("{0:C}", 0);
            Label_KeijyouRieki.Text = string.Format("{0:C}", 0);
            Label_Zeibikimae.Text = string.Format("{0:C}", 0);
            Label_Jyunrieki.Text = string.Format("{0:C}", 0);

            Label_ArariR.Text = string.Format("{0:0.0%}", (double)0);
            Label_EigyouR.Text = string.Format("{0:0.0%}", (double)0);
            Label_KeijyouR.Text = string.Format("{0:0.0%}", (double)0);

            DropDownList_PL_month_s.SelectedValue = "4";
            DropDownList_PL_month_g.SelectedValue = "3";
            SetMaxDate();
            DropDownList_PL_day_s.SelectedValue = "1";
            DropDownList_PL_day_g.SelectedValue = "31";
        }


        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //好きなコードを入れて下さい。

            // コマンド名が“Remove”の場合にのみ処理（独自の削除ボタン）
            if (e.CommandName == "Remove")
            {

                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");
                Session.Add("uuid", (string)"null");

                string uuid = GridView_PL.Rows[args].Cells[0].Text;
                ClassLibrary.MOMClass.DeleteT_PLRow(Global.GetConnection(), uuid);

                GridView_PL.DataBind();


                // コマンド名が“DownLoad”の場合にのみ処理（選択ボタン）
            }
            else if (e.CommandName == "DownLoad")
            {
                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                ResetPL();

                string uuid = GridView_PL.Rows[args].Cells[0].Text;
                DATASET.DataSet.T_PLRow dr = ClassLibrary.MOMClass.GetT_PLRow(Global.GetConnection(), uuid);


                if (Session["args"].ToString() != "null")
                {
                    //GridView1の色を変えた色をもとに戻す
                    int resetargs = int.Parse(Session["args"].ToString());
                    GridView_PL.Rows[resetargs].BackColor = System.Drawing.Color.Empty;
                }

                if (dr == null)
                {
                    //Fatal Error
                    return;
                }

                Session.Add("uuid", uuid);

                //編集テーブルに代入
                TextBox_Uriage.Text = dr.uriagedaka.ToString().Replace(".0000","");
                TextBox_UriageGenka.Text = dr.uriagegenka.ToString().Replace(".0000", "");
                TextBox_HanbaiKanrihi.Text = dr.hanbaikanrihi.ToString().Replace(".0000", "");
                TextBox_EigyouRieki.Text = dr.eigyourieki.ToString().Replace(".0000", "");
                TextBox_EigyougaiHiyou.Text = dr.eigyougaihiyou.ToString().Replace(".0000", "");
                TextBox_TokubetsuRieki.Text = dr.tokubetsurieki.ToString().Replace(".0000", "");
                TextBox_TokubetsuSonshitsu.Text = dr.tokubetsusonshitsu.ToString().Replace(".0000", "");
                TextBox_Houjinzei.Text = dr.houjinzeitou.ToString().Replace(".0000", "");

                DropDownList_PL_year_s.SelectedValue = dr.Date_S.Year.ToString();
                DropDownList_PL_month_s.SelectedValue = dr.Date_S.Month.ToString();
                SetMaxDate();
                DropDownList_PL_day_s.SelectedValue = dr.Date_S.Day.ToString();

                DropDownList_PL_year_g.SelectedValue = dr.Date_G.Year.ToString();
                DropDownList_PL_month_g.SelectedValue = dr.Date_G.Month.ToString();
                SetMaxDate();
                DropDownList_PL_day_g.SelectedValue = dr.Date_G.Day.ToString();


                //SUM
                Sum_PL();
                GridView_PL.DataBind();

                //新たに色を変更する行を記憶
                Session.Add("args", args);

                //行の色変更（選択行を強調表示）
                GridView_PL.Rows[args].BackColor = System.Drawing.Color.Red;

            }
            //---------------------
            return; //grid_RowCommand
            //---------------------
        }





        //------------------------------------------------------------------------------------------------------------------------------------------
        //B/S
        //------------------------------------------------------------------------------------------------------------------------------------------


        protected void Push_BS_test(object sender, EventArgs e)
        {
            if (Panel_BS.Visible)
            {
                Panel_BS.Visible = false;
            }
            else
            {
                Panel_BS.Visible = true;
            }

        }



        //------------------------------------------------------------------------------------------------------------------------------------------
        //C/F
        //------------------------------------------------------------------------------------------------------------------------------------------



        protected void Push_CF_test(object sender, EventArgs e)
        {
            if (Panel_CF.Visible)
            {
                Panel_CF.Visible = false;
            }
            else
            {
                Panel_CF.Visible = true;
            }

        }

        protected void Change_BS(object sender, EventArgs e)
        {
            Sum_BS();
        }


        protected void Push_CheckAS_BS(object sender, EventArgs e)
        {
            Check_BS(true);
        }


        protected void Push_Check_BS(object sender, EventArgs e)
        {
            Check_BS(false);
        }

        protected void Sum_BS()
        {

        }

        protected void Check_BS(bool b = false)
        {

        }



    }

}