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
                ResetBS();

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");
                Session.Add("uuid", (string)"null");
            }

            SetMaxDate();

            if (DropDownList_PL_month_s.SelectedValue != "--")
            {
                DropDownList_PL_month_s.BackColor = System.Drawing.Color.Black;
            }
            if (DropDownList_PL_day_s.SelectedValue != "--")
            {
                DropDownList_PL_day_s.BackColor = System.Drawing.Color.Black;
            }
            if (DropDownList_PL_month_g.SelectedValue != "--")
            {
                DropDownList_PL_month_g.BackColor = System.Drawing.Color.Black;
            }
            if (DropDownList_PL_day_g.SelectedValue != "--")
            {
                DropDownList_PL_day_g.BackColor = System.Drawing.Color.Black;
            }

            if (DropDownList_BS_month.SelectedValue != "--")
            {
                DropDownList_BS_month.BackColor = System.Drawing.Color.Black;
            }
            if (DropDownList_BS_day.SelectedValue != "--")
            {
                DropDownList_BS_day.BackColor = System.Drawing.Color.Black;
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

            StringBuilder sb = new StringBuilder();
            sb.Append(DropDownList_PL_year_s.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_PL_month_s.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_PL_day_s.SelectedValue);
            DateTime dts = DateTime.Parse(sb.ToString());

            sb = new StringBuilder();
            sb.Append(DropDownList_PL_year_g.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_PL_month_g.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_PL_day_g.SelectedValue);
            DateTime dtg = DateTime.Parse(sb.ToString());

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
                TextBox_Uriage.Text = dr.uriagedaka.ToString();
                TextBox_UriageGenka.Text = dr.uriagegenka.ToString();
                TextBox_HanbaiKanrihi.Text = dr.hanbaikanrihi.ToString();
                TextBox_EigyouRieki.Text = dr.eigyourieki.ToString();
                TextBox_EigyougaiHiyou.Text = dr.eigyougaihiyou.ToString();
                TextBox_TokubetsuRieki.Text = dr.tokubetsurieki.ToString();
                TextBox_TokubetsuSonshitsu.Text = dr.tokubetsusonshitsu.ToString();
                TextBox_Houjinzei.Text = dr.houjinzeitou.ToString();

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

            // コマンド名が“BSRemove”の場合にのみ処理（独自の削除ボタン）
            if (e.CommandName == "BSRemove")
            {

                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");
                Session.Add("uuid", (string)"null");

                string uuid = GridView_BS.Rows[args].Cells[0].Text;
                ClassLibrary.MOMClass.DeleteT_BSRow(Global.GetConnection(), uuid);

                GridView_BS.DataBind();


                // コマンド名が“BSDownLoad”の場合にのみ処理（選択ボタン）
            }
            else if (e.CommandName == "BSDownLoad")
            {
                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                ResetBS();

                string uuid = GridView_BS.Rows[args].Cells[0].Text;
                DATASET.DataSet.T_BSRow dr = ClassLibrary.MOMClass.GetT_BSRow(Global.GetConnection(), uuid);


                if (Session["args"].ToString() != "null")
                {
                    //GridView1の色を変えた色をもとに戻す
                    int resetargs = int.Parse(Session["args"].ToString());
                    GridView_BS.Rows[resetargs].BackColor = System.Drawing.Color.Empty;
                }

                if (dr == null)
                {
                    //Fatal Error
                    return;
                }

                Session.Add("uuid", uuid);

                //編集テーブルに代入
                TextBox_BS1.Text = dr.BS1.ToString();
                TextBox_BS2.Text = dr.BS2.ToString();
                TextBox_BS3.Text = dr.BS3.ToString();
                TextBox_BS4.Text = dr.BS4.ToString();
                TextBox_BS5.Text = dr.BS5.ToString();
                TextBox_BS6.Text = dr.BS6.ToString();
                TextBox_BS7.Text = dr.BS7.ToString();
                TextBox_BS8.Text = dr.BS8.ToString();
                TextBox_BS9.Text = dr.BS9.ToString();
                TextBox_BS10.Text = dr.BS10.ToString();
                TextBox_BS11.Text = dr.BS11.ToString();
                TextBox_BS12.Text = dr.BS12.ToString();
                TextBox_BS13.Text = dr.BS13.ToString();
                TextBox_BS14.Text = dr.BS14.ToString();
                TextBox_BS15.Text = dr.BS15.ToString();
                TextBox_BS16.Text = dr.BS16.ToString();
                TextBox_BS17.Text = dr.BS17.ToString();
                TextBox_BS18.Text = dr.BS18.ToString();
                TextBox_BS19.Text = dr.BS19.ToString();
                TextBox_BS20.Text = dr.BS20.ToString();
                TextBox_BS21.Text = dr.BS21.ToString();
                TextBox_BS22.Text = dr.BS22.ToString();
                TextBox_BS23.Text = dr.BS23.ToString();
                TextBox_BS24.Text = dr.BS24.ToString();
                TextBox_BS25.Text = dr.BS25.ToString();
                TextBox_BS26.Text = dr.BS26.ToString();
                TextBox_BS27.Text = dr.BS27.ToString();
                TextBox_BS28.Text = dr.BS28.ToString();
                TextBox_BS29.Text = dr.BS29.ToString();
                TextBox_BS30.Text = dr.BS30.ToString();
                TextBox_BS31.Text = dr.BS31.ToString();
                TextBox_BS32.Text = dr.BS32.ToString();
                TextBox_BS33.Text = dr.BS33.ToString();
                TextBox_BS34.Text = dr.BS34.ToString();
                TextBox_BS35.Text = dr.BS35.ToString();
                TextBox_BS36.Text = dr.BS36.ToString();
                TextBox_BS37.Text = dr.BS37.ToString();
                TextBox_BS38.Text = dr.BS38.ToString();
                TextBox_BS39.Text = dr.BS39.ToString();
                TextBox_BS40.Text = dr.BS40.ToString();
                TextBox_BS41.Text = dr.BS41.ToString();
                TextBox_BS42.Text = dr.BS42.ToString();
                TextBox_BS43.Text = dr.BS43.ToString();
                TextBox_BS44.Text = dr.BS44.ToString();
                TextBox_BS45.Text = dr.BS45.ToString();
                TextBox_BS46.Text = dr.BS46.ToString();
                TextBox_BS47.Text = dr.BS47.ToString();
                TextBox_BS48.Text = dr.BS48.ToString();
                TextBox_BS49.Text = dr.BS49.ToString();
                TextBox_BS50.Text = dr.BS50.ToString();
                TextBox_BS51.Text = dr.BS51.ToString();              

                DropDownList_BS_year.SelectedValue = dr.Date.Year.ToString();
                DropDownList_BS_month.SelectedValue = dr.Date.Month.ToString();
                SetMaxDate();
                DropDownList_BS_day.SelectedValue = dr.Date.Day.ToString();

                //SUM
                Sum_BS();
                GridView_BS.DataBind();

                //新たに色を変更する行を記憶
                Session.Add("args", args);

                //行の色変更（選択行を強調表示）
                GridView_BS.Rows[args].BackColor = System.Drawing.Color.Red;

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
            //Response.Redirect("Money.aspx#BS_TOP", false);
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
            //宣言と初期化
            string str;

            str = HtmlEncode(TextBox_BS1.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value1);
            str = HtmlEncode(TextBox_BS2.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value2);
            str = HtmlEncode(TextBox_BS3.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value3);
            str = HtmlEncode(TextBox_BS4.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value4);
            str = HtmlEncode(TextBox_BS5.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value5);
            str = HtmlEncode(TextBox_BS6.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value6);
            str = HtmlEncode(TextBox_BS7.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value7);
            str = HtmlEncode(TextBox_BS8.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value8);
            str = HtmlEncode(TextBox_BS9.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value9);
            str = HtmlEncode(TextBox_BS10.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value10);
            str = HtmlEncode(TextBox_BS11.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value11);
            str = HtmlEncode(TextBox_BS12.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value12);
            str = HtmlEncode(TextBox_BS13.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value13);
            str = HtmlEncode(TextBox_BS14.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value14);
            str = HtmlEncode(TextBox_BS15.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value15);
            str = HtmlEncode(TextBox_BS16.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value16);
            str = HtmlEncode(TextBox_BS17.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value17);
            str = HtmlEncode(TextBox_BS18.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value18);
            str = HtmlEncode(TextBox_BS19.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value19);
            str = HtmlEncode(TextBox_BS20.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value20);
            str = HtmlEncode(TextBox_BS21.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value21);
            str = HtmlEncode(TextBox_BS22.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value22);
            str = HtmlEncode(TextBox_BS23.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value23);
            str = HtmlEncode(TextBox_BS24.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value24);
            str = HtmlEncode(TextBox_BS25.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value25);
            str = HtmlEncode(TextBox_BS26.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value26);
            str = HtmlEncode(TextBox_BS27.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value27);
            str = HtmlEncode(TextBox_BS28.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value28);
            str = HtmlEncode(TextBox_BS29.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value29);
            str = HtmlEncode(TextBox_BS30.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value30);
            str = HtmlEncode(TextBox_BS31.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value31);
            str = HtmlEncode(TextBox_BS32.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value32);
            str = HtmlEncode(TextBox_BS33.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value33);
            str = HtmlEncode(TextBox_BS34.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value34);
            str = HtmlEncode(TextBox_BS35.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value35);
            str = HtmlEncode(TextBox_BS36.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value36);
            str = HtmlEncode(TextBox_BS37.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value37);
            str = HtmlEncode(TextBox_BS38.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value38);
            str = HtmlEncode(TextBox_BS39.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value39);
            str = HtmlEncode(TextBox_BS40.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value40);
            str = HtmlEncode(TextBox_BS41.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value41);
            str = HtmlEncode(TextBox_BS42.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value42);
            str = HtmlEncode(TextBox_BS43.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value43);
            str = HtmlEncode(TextBox_BS44.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value44);
            str = HtmlEncode(TextBox_BS45.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value45);
            str = HtmlEncode(TextBox_BS46.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value46);
            str = HtmlEncode(TextBox_BS47.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value47);
            str = HtmlEncode(TextBox_BS48.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value48);
            str = HtmlEncode(TextBox_BS49.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value49);
            str = HtmlEncode(TextBox_BS50.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value50);
            str = HtmlEncode(TextBox_BS51.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value51);

            //--------------------------            
            //BS1-11　流動資産
            int ryuudoushisan = value1 + value2 + value3 + value4 + value5 + value6 + value7 + value8 + value9 + value10 + value11;
            Label_BS_RyuudouShisan.Text = string.Format("{0:C}", ryuudoushisan);
            //--------------------------            
            //BS12-18　有形固定資産
            int yuukeikoteishisan = value12 + value13 + value14 + value15 + value16 + value17 + value18;
            Label_BS_YuukeiKoteiShisan.Text = string.Format("{0:C}", yuukeikoteishisan);
            //BS19-21　無形固定資産
            int mukeikoteishisan = value19 + value20 + value21;
            Label_BS_MukeiKoteiShisan.Text = string.Format("{0:C}", mukeikoteishisan);
            //BS22-28　投資その他の資産
            int toushisonotanoshisan = value22 + value23 + value24 + value25 + value26 + value27 + value28;
            Label_BS_ToushiSonotanoShisan.Text = string.Format("{0:C}", toushisonotanoshisan);
            //BS12-28　固定資産(SUM)
            int koteishisan = yuukeikoteishisan + mukeikoteishisan + toushisonotanoshisan;
            Label_BS_KoteiShisan.Text = string.Format("{0:C}", koteishisan);
            //--------------------------            
            //BS1-29　資産合計
            int shisan_a = ryuudoushisan + koteishisan;
            Label_BS_ShisanGoukei.Text = string.Format("{0:C}", shisan_a);
            Label_BS_Shisan.Text = string.Format("{0:#,0}", shisan_a);
            //--------------------------
            //BS29-37　流動負債
            int ryuudoufusai = value29 + value30 + value31 + value32 + value33 + value34 + value35 + value36 + value37;
            Label_BS_RyuudouFusai.Text = string.Format("{0:C}", ryuudoufusai);
            //BS38-40　固定負債
            int koteifusai = value38 + value39 + value40;
            Label_BS_KoteiFusai.Text = string.Format("{0:C}", koteifusai);
            //--------------------------
            //BS29-40　負債合計
            int fusai_a = ryuudoufusai + koteifusai;
            Label_BS_FusaiGoukei.Text = string.Format("{0:C}", fusai_a);
            Label_BS_Fusai.Text = string.Format("{0:#,0}", fusai_a);
            //--------------------------
            //BS41-51　純資産
            int jyunshisan_a = value41 + value42 + value43 + value44 + value45 + value46 + value47 + value48 + value49 + value50 + value51;
            Label_BS_JyunshisanGoukei.Text = string.Format("{0:C}", jyunshisan_a);
            //--------------------------
            //負債純資産合計
            Label_BS_Fusai_JyunshisanGoukei.Text = string.Format("{0:C}", fusai_a + jyunshisan_a); ;
            //--------------------------

        }

        protected void ResetBS()
        {
            TextBox_BS1.Text = "0";
            TextBox_BS2.Text = "0";
            TextBox_BS3.Text = "0";
            TextBox_BS4.Text = "0";
            TextBox_BS5.Text = "0";
            TextBox_BS6.Text = "0";
            TextBox_BS7.Text = "0";
            TextBox_BS8.Text = "0";
            TextBox_BS9.Text = "0";
            TextBox_BS10.Text = "0";
            TextBox_BS11.Text = "0";
            TextBox_BS12.Text = "0";
            TextBox_BS13.Text = "0";
            TextBox_BS14.Text = "0";
            TextBox_BS15.Text = "0";
            TextBox_BS16.Text = "0";
            TextBox_BS17.Text = "0";
            TextBox_BS18.Text = "0";
            TextBox_BS19.Text = "0";
            TextBox_BS20.Text = "0";
            TextBox_BS21.Text = "0";
            TextBox_BS22.Text = "0";
            TextBox_BS23.Text = "0";
            TextBox_BS24.Text = "0";
            TextBox_BS25.Text = "0";
            TextBox_BS26.Text = "0";
            TextBox_BS27.Text = "0";
            TextBox_BS28.Text = "0";
            TextBox_BS29.Text = "0";
            TextBox_BS30.Text = "0";
            TextBox_BS31.Text = "0";
            TextBox_BS32.Text = "0";
            TextBox_BS33.Text = "0";
            TextBox_BS34.Text = "0";
            TextBox_BS35.Text = "0";
            TextBox_BS36.Text = "0";
            TextBox_BS37.Text = "0";
            TextBox_BS38.Text = "0";
            TextBox_BS39.Text = "0";
            TextBox_BS40.Text = "0";
            TextBox_BS41.Text = "0";
            TextBox_BS42.Text = "0";
            TextBox_BS43.Text = "0";
            TextBox_BS44.Text = "0";
            TextBox_BS45.Text = "0";
            TextBox_BS46.Text = "0";
            TextBox_BS47.Text = "0";
            TextBox_BS48.Text = "0";
            TextBox_BS49.Text = "0";
            TextBox_BS50.Text = "0";
            TextBox_BS51.Text = "0";

            Label_BS_Shisan.Text = string.Format("{0:C}", 0);
            Label_BS_RyuudouShisan.Text = string.Format("{0:C}", 0);
            Label_BS_KoteiShisan.Text = string.Format("{0:C}", 0);
            Label_BS_YuukeiKoteiShisan.Text = string.Format("{0:C}", 0);
            Label_BS_MukeiKoteiShisan.Text = string.Format("{0:C}", 0);
            Label_BS_ToushiSonotanoShisan.Text = string.Format("{0:C}", 0);
            Label_BS_ShisanGoukei.Text = string.Format("{0:C}", 0);
            Label_BS_Fusai.Text = string.Format("{0:C}", 0);
            Label_BS_RyuudouFusai.Text = string.Format("{0:C}", 0);
            Label_BS_KoteiFusai.Text = string.Format("{0:C}", 0);
            Label_BS_FusaiGoukei.Text = string.Format("{0:C}", 0);
            Label_BS_JyunshisanGoukei.Text = string.Format("{0:C}", 0);
            Label_BS_Fusai_JyunshisanGoukei.Text = string.Format("{0:C}", 0);

            DropDownList_BS_month.SelectedValue = "4";
            SetMaxDate();
            DropDownList_BS_day.SelectedValue = "1";
        }

        protected void Check_BS(bool b = false)
        {

            Sum_BS();

            if (DropDownList_BS_month.SelectedValue == "--")
            {
                DropDownList_BS_month.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (DropDownList_BS_day.SelectedValue == "--")
            {
                DropDownList_BS_day.BackColor = System.Drawing.Color.Red;
                return;
            }


            //宣言と初期化
            string str;

            str = HtmlEncode(TextBox_BS1.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value1);
            str = HtmlEncode(TextBox_BS2.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value2);
            str = HtmlEncode(TextBox_BS3.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value3);
            str = HtmlEncode(TextBox_BS4.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value4);
            str = HtmlEncode(TextBox_BS5.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value5);
            str = HtmlEncode(TextBox_BS6.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value6);
            str = HtmlEncode(TextBox_BS7.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value7);
            str = HtmlEncode(TextBox_BS8.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value8);
            str = HtmlEncode(TextBox_BS9.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value9);
            str = HtmlEncode(TextBox_BS10.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value10);
            str = HtmlEncode(TextBox_BS11.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value11);
            str = HtmlEncode(TextBox_BS12.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value12);
            str = HtmlEncode(TextBox_BS13.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value13);
            str = HtmlEncode(TextBox_BS14.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value14);
            str = HtmlEncode(TextBox_BS15.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value15);
            str = HtmlEncode(TextBox_BS16.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value16);
            str = HtmlEncode(TextBox_BS17.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value17);
            str = HtmlEncode(TextBox_BS18.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value18);
            str = HtmlEncode(TextBox_BS19.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value19);
            str = HtmlEncode(TextBox_BS20.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value20);
            str = HtmlEncode(TextBox_BS21.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value21);
            str = HtmlEncode(TextBox_BS22.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value22);
            str = HtmlEncode(TextBox_BS23.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value23);
            str = HtmlEncode(TextBox_BS24.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value24);
            str = HtmlEncode(TextBox_BS25.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value25);
            str = HtmlEncode(TextBox_BS26.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value26);
            str = HtmlEncode(TextBox_BS27.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value27);
            str = HtmlEncode(TextBox_BS28.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value28);
            str = HtmlEncode(TextBox_BS29.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value29);
            str = HtmlEncode(TextBox_BS30.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value30);
            str = HtmlEncode(TextBox_BS31.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value31);
            str = HtmlEncode(TextBox_BS32.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value32);
            str = HtmlEncode(TextBox_BS33.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value33);
            str = HtmlEncode(TextBox_BS34.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value34);
            str = HtmlEncode(TextBox_BS35.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value35);
            str = HtmlEncode(TextBox_BS36.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value36);
            str = HtmlEncode(TextBox_BS37.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value37);
            str = HtmlEncode(TextBox_BS38.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value38);
            str = HtmlEncode(TextBox_BS39.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value39);
            str = HtmlEncode(TextBox_BS40.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value40);
            str = HtmlEncode(TextBox_BS41.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value41);
            str = HtmlEncode(TextBox_BS42.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value42);
            str = HtmlEncode(TextBox_BS43.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value43);
            str = HtmlEncode(TextBox_BS44.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value44);
            str = HtmlEncode(TextBox_BS45.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value45);
            str = HtmlEncode(TextBox_BS46.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value46);
            str = HtmlEncode(TextBox_BS47.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value47);
            str = HtmlEncode(TextBox_BS48.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value48);
            str = HtmlEncode(TextBox_BS49.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value49);
            str = HtmlEncode(TextBox_BS50.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value50);
            str = HtmlEncode(TextBox_BS51.Text);
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value51);

            /*
            //--------------------------            
            //BS1-11　流動資産
            int ryuudoushisan = value1 + value2 + value3 + value4 + value5 + value6 + value7 + value8 + value9 + value10 + value11;
            //--------------------------            
            //BS12-18　有形固定資産
            int yuukeikoteishisan = value12 + value13 + value14 + value15 + value16 + value17 + value18;
            //BS19-21　無形固定資産
            int mukeikoteishisan = value19 + value20 + value21;
            //BS22-28　投資その他の資産
            int toushisonotanoshisan = value22 + value23 + value24 + value25 + value26 + value27 + value28;
            //BS12-28　固定資産(SUM)
            int koteishisan = yuukeikoteishisan + mukeikoteishisan + toushisonotanoshisan;
            //--------------------------            
            //BS1-29　資産合計
            int shisan_a = ryuudoushisan + koteishisan;
            //--------------------------
            //BS29-37　流動負債
            int ryuudoufusai = value29 + value30 + value31 + value32 + value33 + value34 + value35 + value36 + value37;
            //BS38-40　固定負債
            int koteifusai = value38 + value39 + value40;
            //--------------------------
            //BS29-40　負債合計
            int fusai_a = ryuudoufusai + koteifusai;
            //--------------------------
            //BS41-51　純資産
            int jyunshisan_a = value41 + value42 + value43 + value44 + value45 + value46 + value47 + value48 + value49 + value50 + value51;
            //--------------------------
            */

            StringBuilder sb = new StringBuilder();
            sb.Append(DropDownList_BS_year.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_BS_month.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_BS_day.SelectedValue);
            DateTime dt = DateTime.Parse(sb.ToString());


            string uuid = Session["uuid"].ToString();

            if (b)
            {
                DATASET.DataSet.T_BSRow dr = ClassLibrary.MOMClass.GetT_BSRow(Global.GetConnection(), uuid);
                if (dr != null)
                {
                    //Update
                    ClassLibrary.MOMClass.SetT_BSUpdate(Global.GetConnection(), uuid, value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16, value17, value18, value19, value20, value21, value22, value23, value24, value25, value26, value27, value28, value29, value30, value31, value32, value33, value34, value35, value36, value37, value38, value39, value40, value41, value42, value43, value44, value45, value46, value47, value48, value49, value50, value51, dt);
                }
                else
                {
                    b = false;
                }
            }

            if (!b)
            {
                //Insert
                ClassLibrary.MOMClass.SetT_BSInsert(Global.GetConnection(), value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, value16, value17, value18, value19, value20, value21, value22, value23, value24, value25, value26, value27, value28, value29, value30, value31, value32, value33, value34, value35, value36, value37, value38, value39, value40, value41, value42, value43, value44, value45, value46, value47, value48, value49, value50, value51, dt);
            }

            GridView_BS.DataBind();

        }



    }

}