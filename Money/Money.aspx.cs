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
            if (SessionManager.User.M_User.id == null || SessionManager.User.M_User.id.Trim() == "")
            {
                //不正ログイン防止
                this.Response.Redirect("../ログイン/Login.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                SetMaxYear();
                ResetMonthItem();
                ResetPL();
                ResetBS();
                ResetCF();
                Reset_Rental();             

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");
                Session.Add("uuid", (string)"null");
            }

            //消去すると１段ずれることがあるため、選択行かどうか１段ずつ検索し直す。
            for (int i = 0; i < GridView_Rental.Rows.Count; i++)
            {
                if (GridView_Rental.Rows[i].Cells[0].Text == TextBox_order_uuid.Text)
                {
                    Session.Add("args", i);
                    Session.Add("uuid", GridView_Rental.Rows[i].Cells[0].Text);
                    //GridView_Resetの選択行を赤色にする
                    //int resetargs = int.Parse(Session["args"].ToString());
                    GridView_Rental.Rows[i].BackColor = System.Drawing.Color.Red;
                    break;
                }
            }

            SetMaxDate();

            if (DropDownList_PL_month_s.SelectedValue != "--")
            {
                DropDownList_PL_month_s.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_PL_day_s.SelectedValue != "--")
            {
                DropDownList_PL_day_s.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_PL_month_g.SelectedValue != "--")
            {
                DropDownList_PL_month_g.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_PL_day_g.SelectedValue != "--")
            {
                DropDownList_PL_day_g.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_BS_month.SelectedValue != "--")
            {
                DropDownList_BS_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_BS_day.SelectedValue != "--")
            {
                DropDownList_BS_day.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_CF_month.SelectedValue != "--")
            {
                DropDownList_CF_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_CF_day.SelectedValue != "--")
            {
                DropDownList_CF_day.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_order_month.SelectedValue != "--")
            {
                DropDownList_order_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_order_day.SelectedValue != "--")
            {
                DropDownList_order_day.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_youki_month.SelectedValue != "--")
            {
                DropDownList_youki_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_youki_day.SelectedValue != "--")
            {
                DropDownList_youki_day.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_seiki_month.SelectedValue != "--")
            {
                DropDownList_seiki_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_seiki_day.SelectedValue != "--")
            {
                DropDownList_seiki_day.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_shipping_month.SelectedValue != "--")
            {
                DropDownList_shipping_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_shipping_day.SelectedValue != "--")
            {
                DropDownList_shipping_day.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_receive_month.SelectedValue != "--")
            {
                DropDownList_receive_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_receive_day.SelectedValue != "--")
            {
                DropDownList_receive_day.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_send_d_month.SelectedValue != "--")
            {
                DropDownList_send_d_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_send_d_day.SelectedValue != "--")
            {
                DropDownList_send_d_day.BackColor = System.Drawing.Color.Empty;
            }

            if (DropDownList_send_month.SelectedValue != "--")
            {
                DropDownList_send_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_send_day.SelectedValue != "--")
            {
                DropDownList_send_day.BackColor = System.Drawing.Color.Empty;
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
            DropDownList_CF_year.Items.Clear();
            DropDownList_order_year.Items.Clear();
            DropDownList_youki_year.Items.Clear();
            DropDownList_seiki_year.Items.Clear();
            DropDownList_shipping_year.Items.Clear();
            DropDownList_receive_year.Items.Clear();
            DropDownList_send_d_year.Items.Clear();
            DropDownList_send_year.Items.Clear();
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

            DropDownList_CF_month.Items.Clear();
            DropDownList_CF_month.Items.Insert(0, "--");

            DropDownList_order_month.Items.Clear();
            DropDownList_youki_month.Items.Clear();
            DropDownList_seiki_month.Items.Clear();
            DropDownList_shipping_month.Items.Clear();
            DropDownList_receive_month.Items.Clear();
            DropDownList_send_d_month.Items.Clear();
            DropDownList_send_month.Items.Clear();

            DropDownList_order_month.Items.Insert(0, "--");
            DropDownList_youki_month.Items.Insert(0, "--");
            DropDownList_seiki_month.Items.Insert(0, "--");
            DropDownList_shipping_month.Items.Insert(0, "--");
            DropDownList_receive_month.Items.Insert(0, "--");
            DropDownList_send_d_month.Items.Insert(0, "--");
            DropDownList_send_month.Items.Insert(0, "--");

            for (int i=1; i <= 12; i++) {
                DropDownList_PL_month_s.Items.Insert(i, i.ToString());
                DropDownList_PL_month_g.Items.Insert(i, i.ToString());
                DropDownList_BS_month.Items.Insert(i, i.ToString());
                DropDownList_CF_month.Items.Insert(i, i.ToString());
                DropDownList_order_month.Items.Insert(i, i.ToString());
                DropDownList_youki_month.Items.Insert(i, i.ToString());
                DropDownList_seiki_month.Items.Insert(i, i.ToString());
                DropDownList_shipping_month.Items.Insert(i, i.ToString());
                DropDownList_receive_month.Items.Insert(i, i.ToString());
                DropDownList_send_d_month.Items.Insert(i, i.ToString());
                DropDownList_send_month.Items.Insert(i, i.ToString());
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
            DropDownList_CF_year.Items.Insert(i, item);
            DropDownList_order_year.Items.Insert(i, item);
            DropDownList_youki_year.Items.Insert(i, item);
            DropDownList_seiki_year.Items.Insert(i, item);
            DropDownList_shipping_year.Items.Insert(i, item);
            DropDownList_receive_year.Items.Insert(i, item);
            DropDownList_send_d_year.Items.Insert(i, item);
            DropDownList_send_year.Items.Insert(i, item);
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
            DropDownList_CF_year.SelectedValue = item;
            DropDownList_order_year.SelectedValue = item;
            DropDownList_youki_year.SelectedValue = item;
            DropDownList_seiki_year.SelectedValue = item;
            DropDownList_shipping_year.SelectedValue = item;
            DropDownList_receive_year.SelectedValue = item;
            DropDownList_send_d_year.SelectedValue = item;
            DropDownList_send_year.SelectedValue = item;
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
            DropDownList_CF_day.Items.Clear();
            DropDownList_CF_day.Items.Insert(0, "--");

            DropDownList_order_day.Items.Clear();
            DropDownList_youki_day.Items.Clear();
            DropDownList_seiki_day.Items.Clear();
            DropDownList_shipping_day.Items.Clear();
            DropDownList_receive_day.Items.Clear();
            DropDownList_send_d_day.Items.Clear();
            DropDownList_send_day.Items.Clear();

            DropDownList_order_day.Items.Insert(0, "--");
            DropDownList_youki_day.Items.Insert(0, "--");
            DropDownList_seiki_day.Items.Insert(0, "--");
            DropDownList_shipping_day.Items.Insert(0, "--");
            DropDownList_receive_day.Items.Insert(0, "--");
            DropDownList_send_d_day.Items.Insert(0, "--");
            DropDownList_send_day.Items.Insert(0, "--");
            //-------------------------------------------------------------------------------

            //month
            //初期化
            DropDownList_PL_month_s.SelectedValue = "--";
            DropDownList_PL_month_g.SelectedValue = "--";
            DropDownList_BS_month.SelectedValue = "--";
            DropDownList_CF_month.SelectedValue = "--";
            DropDownList_order_month.SelectedValue = "--";
            DropDownList_youki_month.SelectedValue = "--";
            DropDownList_seiki_month.SelectedValue = "--";
            DropDownList_shipping_month.SelectedValue = "--";
            DropDownList_receive_month.SelectedValue = "--";
            DropDownList_send_d_month.SelectedValue = "--";
            DropDownList_send_month.SelectedValue = "--";

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


            //memory
            memory1 = DropDownList_CF_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_CF_day.Items.Clear();
            DropDownList_CF_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_CF_year.SelectedValue);
            if (DropDownList_CF_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_CF_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_CF_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_CF_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_CF_day.SelectedValue = memory1;
            //---------------------------------------------------------------------------------
            //memory
            memory1 = DropDownList_order_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_order_day.Items.Clear();
            DropDownList_order_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_order_year.SelectedValue);
            if (DropDownList_order_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_order_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_order_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_order_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_order_day.SelectedValue = memory1;
            //---------------------------------------------------------------------------------
            //memory
            memory1 = DropDownList_youki_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_youki_day.Items.Clear();
            DropDownList_youki_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_youki_year.SelectedValue);
            if (DropDownList_youki_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_youki_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_youki_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_youki_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_youki_day.SelectedValue = memory1;
            //---------------------------------------------------------------------------------
            //memory
            memory1 = DropDownList_seiki_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_seiki_day.Items.Clear();
            DropDownList_seiki_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_seiki_year.SelectedValue);
            if (DropDownList_seiki_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_seiki_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_seiki_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_seiki_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_seiki_day.SelectedValue = memory1;
            //---------------------------------------------------------------------------------
            //memory
            memory1 = DropDownList_shipping_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_shipping_day.Items.Clear();
            DropDownList_shipping_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_shipping_year.SelectedValue);
            if (DropDownList_shipping_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_shipping_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_shipping_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_shipping_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_shipping_day.SelectedValue = memory1;
            //---------------------------------------------------------------------------------
            //memory
            memory1 = DropDownList_receive_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_receive_day.Items.Clear();
            DropDownList_receive_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_receive_year.SelectedValue);
            if (DropDownList_receive_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_receive_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_receive_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_receive_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_receive_day.SelectedValue = memory1;
            //---------------------------------------------------------------------------------
            //memory
            memory1 = DropDownList_send_d_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_send_d_day.Items.Clear();
            DropDownList_send_d_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_send_d_year.SelectedValue);
            if (DropDownList_send_d_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_send_d_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_send_d_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_send_d_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_send_d_day.SelectedValue = memory1;
            //---------------------------------------------------------------------------------
            //memory
            memory1 = DropDownList_send_day.SelectedValue;

            if (memory1 == "--" || memory1 == "")
            {
                memory1 = "0";
            }

            //初期化
            DropDownList_send_day.Items.Clear();
            DropDownList_send_day.Items.Insert(0, "--");

            y = int.Parse(DropDownList_send_year.SelectedValue);
            if (DropDownList_send_month.SelectedValue != "--")
            {

                m = int.Parse(DropDownList_send_month.SelectedValue);
                maxday = GetDateMax(y, m);
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_send_day.Items.Insert(i, i.ToString());
                }

            }
            else
            {
                maxday = 31;
                memory1 = Math.Min(int.Parse(memory1), maxday).ToString();
                for (i = 1; i <= maxday; i++)
                {
                    DropDownList_send_day.Items.Insert(i, i.ToString());
                }
            }

            //load
            if (memory1 == "0")
            {
                memory1 = "--";
            }

            DropDownList_send_day.SelectedValue = memory1;
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
            float ri_r;
            string str;

            str = TextBox_Uriage.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;
            int uriagedaka = value;

            str = TextBox_UriageGenka.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum -= value;

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
            pl_sum -= value;

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
            pl_sum -= value;

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

            str = TextBox_TokubetsuSonshitsu.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum -= value;

            Label_Zeibikimae.Text = string.Format("{0:C}", pl_sum);

            str = TextBox_Houjinzei.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum -= value;

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


        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
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

            // コマンド名が“CFRemove”の場合にのみ処理（独自の削除ボタン）
            if (e.CommandName == "CFRemove")
            {

                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                //セッション変数argsを初期化
                Session.Add("args", (string)"null");
                Session.Add("uuid", (string)"null");

                string uuid = GridView_CF.Rows[args].Cells[0].Text;
                ClassLibrary.MOMClass.DeleteT_CFRow(Global.GetConnection(), uuid);

                GridView_CF.DataBind();


                // コマンド名が“CFDownLoad”の場合にのみ処理（選択ボタン）
            }
            else if (e.CommandName == "CFDownLoad")
            {
                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                ResetCF();

                string uuid = GridView_CF.Rows[args].Cells[0].Text;
                DATASET.DataSet.T_CFRow dr = ClassLibrary.MOMClass.GetT_CFRow(Global.GetConnection(), uuid);


                if (Session["args"].ToString() != "null")
                {
                    //GridView1の色を変えた色をもとに戻す
                    int resetargs = int.Parse(Session["args"].ToString());
                    GridView_CF.Rows[resetargs].BackColor = System.Drawing.Color.Empty;
                }

                if (dr == null)
                {
                    //Fatal Error
                    return;
                }

                Session.Add("uuid", uuid);

                //編集テーブルに代入
                TextBox_CF1.Text = dr.CF1.ToString();
                TextBox_CF2.Text = dr.CF2.ToString();
                TextBox_CF3.Text = dr.CF3.ToString();
                TextBox_CF4.Text = dr.CF4.ToString();
                TextBox_CF5.Text = dr.CF5.ToString();
                TextBox_CF6.Text = dr.CF6.ToString();
                TextBox_CF7.Text = dr.CF7.ToString();
                TextBox_CF8.Text = dr.CF8.ToString();
                TextBox_CF9.Text = dr.CF9.ToString();
                TextBox_CF10.Text = dr.CF10.ToString();
                TextBox_CF11.Text = dr.CF11.ToString();
                TextBox_CF12.Text = dr.CF12.ToString();
                TextBox_CF13.Text = dr.CF13.ToString();
                TextBox_CF14.Text = dr.ACL5.ToString(); //(6)現金および現金同等物期末残高
                TextBox_CF15.Text = dr.CF14.ToString(); //売上高


                DropDownList_CF_year.SelectedValue = dr.Date.Year.ToString();
                DropDownList_CF_month.SelectedValue = dr.Date.Month.ToString();
                SetMaxDate();
                DropDownList_CF_day.SelectedValue = dr.Date.Day.ToString();

                //SUM
                Sum_CF();
                GridView_CF.DataBind();

                //新たに色を変更する行を記憶
                Session.Add("args", args);

                //行の色変更（選択行を強調表示）
                GridView_CF.Rows[args].BackColor = System.Drawing.Color.Red;

            }

            // コマンド名が“RNRemove”の場合にのみ処理（独自の削除ボタン）
            if (e.CommandName == "RNRemove")
            {

                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                string uuid = GridView_Rental.Rows[args].Cells[0].Text;
                ClassLibrary.RentalClass.SetT_RentalDelete(Global.GetConnection(), uuid);

                if(TextBox_order_uuid.Text == uuid)
                {
                    //セッション変数argsを初期化
                    Session.Add("args", (string)"null");
                    Session.Add("uuid", (string)"null");
                    Reset_Rental();
                }
               
                GridView_Rental.DataBind();


                //消去すると１段ずれることがあるため、選択行かどうか１段ずつ検索し直す。
                for (int i = 0; i < GridView_Rental.Rows.Count; i++)
                {
                    if (GridView_Rental.Rows[i].Cells[0].Text == TextBox_order_uuid.Text)
                    {
                        Session.Add("args", i);
                        Session.Add("uuid", GridView_Rental.Rows[i].Cells[0].Text);
                        //GridView_Resetの選択行を赤色にする
                        //int resetargs = int.Parse(Session["args"].ToString());
                        GridView_Rental.Rows[i].BackColor = System.Drawing.Color.Red;
                        break;
                    }
                }




                // コマンド名が“RNDownLoad”の場合にのみ処理（選択ボタン）
            }
            else if (e.CommandName == "RNDownLoad")
            {
                //コマンドの引数を取得
                int args = Int32.Parse(e.CommandArgument.ToString());

                //ロードのためにテーブルには用いるデータをバインドし、Visible=trueにしている必要がある。falseでも配列int[]は数える。
                //【重要】ReadOnly属性がついていないと読み込みできない。

                Reset_Rental();

                string uuid = GridView_Rental.Rows[args].Cells[0].Text;
                DATASET.DataSet.T_RentalRow dr = ClassLibrary.RentalClass.GetT_Rental(Global.GetConnection(), uuid);

                if (Session["args"].ToString() != "null")
                {
                    //GridView1の色を変えた色をもとに戻す
                    int resetargs = int.Parse(Session["args"].ToString());
                    GridView_Rental.Rows[resetargs].BackColor = System.Drawing.Color.Empty;
                }

                if (dr == null)
                {
                    //Fatal Error
                    return;
                }

                Session.Add("uuid", uuid);

                //編集テーブルに代入
                SetMaxYear();

                if (!dr.IsNull("uuid"))
                {
                    TextBox_order_uuid.Text = dr.uuid.ToString().Trim();
                }
                if (!dr.IsNull("order_name"))
                {
                    TextBox_order_name.Text = dr.order_name.ToString().Trim();
                }
                if (!dr.IsNull("order_rest"))
                {
                    TextBox_order_rest.Text = dr.order_rest.ToString().Trim();
                }
                if (!dr.IsNull("send_rest"))
                {
                    TextBox_send_rest.Text = dr.send_rest.ToString().Trim();
                }
                if (!dr.IsNull("rental_name"))
                {
                    TextBox_rental_name.Text = dr.rental_name.ToString().Trim();
                }
                if (!dr.IsNull("rental_type"))
                {
                    TextBox_rental_type.Text = dr.rental_type.ToString().Trim();
                }
                if (!dr.IsNull("rental_tanka"))
                {
                    TextBox_rental_tanka.Text = dr.rental_tanka.ToString().Trim();
                }
                if (!dr.IsNull("rental_amount"))
                {
                    TextBox_rental_amount.Text = dr.rental_amount.ToString().Trim();
                }
                if (!dr.IsNull("rental_total_amount"))
                {
                    TextBox_rental_total_amount.Text = dr.rental_total_amount.ToString().Trim();
                }

                if (!dr.IsNull("order_date"))
                {
                    DropDownList_order_year.SelectedValue = dr.order_date.Year.ToString();
                    DropDownList_order_month.SelectedValue = dr.order_date.Month.ToString();
                    SetMaxDate();
                    DropDownList_order_day.SelectedValue = dr.order_date.Day.ToString();
                }
                else
                {
                    DropDownList_order_year.SelectedValue = DateTime.Now.Year.ToString();
                    DropDownList_order_month.SelectedValue = "--";
                    SetMaxDate();
                    DropDownList_order_day.SelectedValue = "--";
                }

                if (!dr.IsNull("order_youki"))
                {
                    if (dr.order_youki.Year != 0001)
                    {
                        DropDownList_youki_year.SelectedValue = dr.order_youki.Year.ToString();
                        DropDownList_youki_month.SelectedValue = dr.order_youki.Month.ToString();
                        SetMaxDate();
                        DropDownList_youki_day.SelectedValue = dr.order_youki.Day.ToString();
                    }
                    else
                    {
                        DropDownList_youki_year.SelectedValue = DateTime.Now.Year.ToString();
                        DropDownList_youki_month.SelectedValue = "--";
                        SetMaxDate();
                        DropDownList_youki_day.SelectedValue = "--";
                    }
                }
                else
                {
                    DropDownList_youki_year.SelectedValue = DateTime.Now.Year.ToString();
                    DropDownList_youki_month.SelectedValue = "--";
                    SetMaxDate();
                    DropDownList_youki_day.SelectedValue = "--";
                }

                if (!dr.IsNull("order_seiki"))
                {
                    if (dr.order_seiki.Year != 0001)
                    {
                        DropDownList_seiki_year.SelectedValue = dr.order_seiki.Year.ToString();
                        DropDownList_seiki_month.SelectedValue = dr.order_seiki.Month.ToString();
                        SetMaxDate();
                        DropDownList_seiki_day.SelectedValue = dr.order_seiki.Day.ToString();
                    }
                    else
                    {
                        DropDownList_seiki_year.SelectedValue = DateTime.Now.Year.ToString();
                        DropDownList_seiki_month.SelectedValue = "--";
                        SetMaxDate();
                        DropDownList_seiki_day.SelectedValue = "--";
                    }
                }
                else
                {
                    DropDownList_seiki_year.SelectedValue = DateTime.Now.Year.ToString();
                    DropDownList_seiki_month.SelectedValue = "--";
                    SetMaxDate();
                    DropDownList_seiki_day.SelectedValue = "--";
                }

                if (!dr.IsNull("order_shipping_date"))
                {
                    if (dr.order_shipping_date.Year != 0001)
                    {
                        DropDownList_shipping_year.SelectedValue = dr.order_shipping_date.Year.ToString();
                        DropDownList_shipping_month.SelectedValue = dr.order_shipping_date.Month.ToString();
                        SetMaxDate();
                        DropDownList_shipping_day.SelectedValue = dr.order_shipping_date.Day.ToString();
                    }
                    else
                    {
                        DropDownList_shipping_year.SelectedValue = DateTime.Now.Year.ToString();
                        DropDownList_shipping_month.SelectedValue = "--";
                        SetMaxDate();
                        DropDownList_shipping_day.SelectedValue = "--";
                    }
                }
                else
                {
                    DropDownList_shipping_year.SelectedValue = DateTime.Now.Year.ToString();
                    DropDownList_shipping_month.SelectedValue = "--";
                    SetMaxDate();
                    DropDownList_shipping_day.SelectedValue = "--";
                }

                if (!dr.IsNull("receive_date"))
                {
                    if (dr.receive_date.Year != 0001)
                    {
                        DropDownList_receive_year.SelectedValue = dr.receive_date.Year.ToString();
                        DropDownList_receive_month.SelectedValue = dr.receive_date.Month.ToString();
                        SetMaxDate();
                        DropDownList_receive_day.SelectedValue = dr.receive_date.Day.ToString();
                    }
                    else
                    {
                        DropDownList_receive_year.SelectedValue = DateTime.Now.Year.ToString();
                        DropDownList_receive_month.SelectedValue = "--";
                        SetMaxDate();
                        DropDownList_receive_day.SelectedValue = "--";
                    }
                }
                else
                {
                    DropDownList_receive_year.SelectedValue = DateTime.Now.Year.ToString();
                    DropDownList_receive_month.SelectedValue = "--";
                    SetMaxDate();
                    DropDownList_receive_day.SelectedValue = "--";
                }

                if (!dr.IsNull("send_deadline"))
                {
                    if (dr.send_deadline.Year != 0001)
                    {
                        DropDownList_send_d_year.SelectedValue = dr.send_deadline.Year.ToString();
                        DropDownList_send_d_month.SelectedValue = dr.send_deadline.Month.ToString();
                        SetMaxDate();
                        DropDownList_send_d_day.SelectedValue = dr.send_deadline.Day.ToString();
                    }
                    else
                    {
                        DropDownList_send_d_year.SelectedValue = DateTime.Now.Year.ToString();
                        DropDownList_send_d_month.SelectedValue = "--";
                        SetMaxDate();
                        DropDownList_send_d_day.SelectedValue = "--";
                    }
                }
                else
                {
                    DropDownList_send_d_year.SelectedValue = DateTime.Now.Year.ToString();
                    DropDownList_send_d_month.SelectedValue = "--";
                    SetMaxDate();
                    DropDownList_send_d_day.SelectedValue = "--";
                }

                if (!dr.IsNull("send_date"))
                {
                    if (dr.send_date.Year != 0001)
                    {
                        DropDownList_send_year.SelectedValue = dr.send_date.Year.ToString();
                        DropDownList_send_month.SelectedValue = dr.send_date.Month.ToString();
                        SetMaxDate();
                        DropDownList_send_day.SelectedValue = dr.send_date.Day.ToString();
                    }
                    else
                    {
                        DropDownList_send_year.SelectedValue = DateTime.Now.Year.ToString();
                        DropDownList_send_month.SelectedValue = "--";
                        SetMaxDate();
                        DropDownList_send_day.SelectedValue = "--";
                    }
                }
                else
                {
                    DropDownList_send_year.SelectedValue = DateTime.Now.Year.ToString();
                    DropDownList_send_month.SelectedValue = "--";
                    SetMaxDate();
                    DropDownList_send_day.SelectedValue = "--";
                }

                GridView_Rental.DataBind();

                //新たに色を変更する行を記憶
                Session.Add("args", args);

                //行の色変更（選択行を強調表示）
                GridView_Rental.Rows[args].BackColor = System.Drawing.Color.Red;

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


        protected void Push_Check_CF(object sender, EventArgs e)
        {
            Check_CF(false);
        }

        protected void Push_CheckAS_CF(object sender, EventArgs e)
        {
            Check_CF(true);
        }

        protected void Change_CF(object sender, EventArgs e)
        {
            Sum_CF();
        }


        protected void Sum_CF()
        {
            //宣言と初期化
            float ri_r;
            string str;

            //(1)営業活動によるキャッシュフロー 
            str = TextBox_CF1.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value1);
            str = TextBox_CF2.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value2);
            str = TextBox_CF3.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value3);
            str = TextBox_CF4.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value4);
            str = TextBox_CF5.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value5);
            str = TextBox_CF6.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value6);
            int cf1 = value1 + value2 - value3 - value4 + value5 - value6;
            Label_CF1.Text = string.Format("{0:C}", cf1);

            //(2)投資活動によるキャッシュフロー
            str = TextBox_CF7.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value7);
            str = TextBox_CF8.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value8);
            str = TextBox_CF9.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value9);
            str = TextBox_CF10.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value10);
            int cf2 = - value7 + value8 - value9 + value10;
            Label_CF2.Text = string.Format("{0:C}", cf2);

            //(3)財務活動によるキャッシュ・フロー
            str = TextBox_CF11.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value11);
            str = TextBox_CF12.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value12);
            int cf3 = value11 - value12;
            Label_CF3.Text = string.Format("{0:C}", cf3);

            //(5)現金及び現金同等物等期首残高
            str = TextBox_CF13.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value13);

            //(6)現金および現金同等物期末残高
            str = TextBox_CF14.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value14);

            //売上高
            str = TextBox_CF15.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value15);

            int cf4 = cf1 + cf2 + cf3;
            Label_CF4.Text = string.Format("{0:C}", cf4);

            //int cf5 = value14; //削除



            //キャッシュ・フローマージン＝「営業活動によるキャッシュ・フロー」÷「売上高」
            if ((float)value15 != 0)
            {
                ri_r = (float)cf1 / (float)value15;
            }
            else
            {
                ri_r = 0f;
            }
            Label_CF6.Text = string.Format("{0:0.0%}", (double)ri_r);
        }


        protected void Check_CF(bool b = false)
        {
            Sum_CF();

            if (DropDownList_CF_month.SelectedValue == "--")
            {
                DropDownList_CF_month.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (DropDownList_CF_day.SelectedValue == "--")
            {
                DropDownList_CF_day.BackColor = System.Drawing.Color.Red;
                return;
            }


            //宣言と初期化
            string str;

            //(1)営業活動によるキャッシュフロー 
            str = TextBox_CF1.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value1);
            str = TextBox_CF2.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value2);
            str = TextBox_CF3.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value3);
            str = TextBox_CF4.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value4);
            str = TextBox_CF5.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value5);
            str = TextBox_CF6.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value6);

            //(2)投資活動によるキャッシュフロー
            str = TextBox_CF7.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value7);
            str = TextBox_CF8.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value8);
            str = TextBox_CF9.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value9);
            str = TextBox_CF10.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value10);

            //(3)財務活動によるキャッシュ・フロー
            str = TextBox_CF11.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value11);
            str = TextBox_CF12.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value12);

            //(5)現金及び現金同等物等期首残高
            str = TextBox_CF13.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value13);

            //(6)現金および現金同等物期末残高
            str = TextBox_CF14.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value14);

            //売上高
            str = TextBox_CF15.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out int value15);

            StringBuilder sb = new StringBuilder();
            sb.Append(DropDownList_CF_year.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_CF_month.SelectedValue);
            sb.Append("/");
            sb.Append(DropDownList_CF_day.SelectedValue);
            DateTime dt = DateTime.Parse(sb.ToString());

            string uuid = Session["uuid"].ToString();

            if (b)
            {
                DATASET.DataSet.T_CFRow dr = ClassLibrary.MOMClass.GetT_CFRow(Global.GetConnection(), uuid);
                if (dr != null)
                {
                    ClassLibrary.MOMClass.SetT_CFUpdate(Global.GetConnection(), uuid, value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, dt);
                }
                else
                {
                    b = false;
                }
            }

            if (!b)
            {
                ClassLibrary.MOMClass.SetT_CFInsert(Global.GetConnection(), value1, value2, value3, value4, value5, value6, value7, value8, value9, value10, value11, value12, value13, value14, value15, dt);
            }

            GridView_CF.DataBind();
        }



        protected void ResetCF()
        {
            TextBox_CF1.Text = "0";
            TextBox_CF2.Text = "0";
            TextBox_CF3.Text = "0";
            TextBox_CF4.Text = "0";
            TextBox_CF5.Text = "0";
            TextBox_CF6.Text = "0";
            TextBox_CF7.Text = "0";
            TextBox_CF8.Text = "0";
            TextBox_CF9.Text = "0";
            TextBox_CF10.Text = "0";
            TextBox_CF11.Text = "0";
            TextBox_CF12.Text = "0";
            TextBox_CF13.Text = "0";
            TextBox_CF14.Text = "0";
            TextBox_CF15.Text = "0";

            Label_CF1.Text = string.Format("{0:C}", 0);
            Label_CF2.Text = string.Format("{0:C}", 0);
            Label_CF3.Text = string.Format("{0:C}", 0);
            Label_CF4.Text = string.Format("{0:C}", 0);
            Label_CF6.Text = string.Format("{0:0.0%}", 0);

            DropDownList_CF_month.SelectedValue = "4";
            SetMaxDate();
            DropDownList_CF_day.SelectedValue = "1";
        }

        //-----------------------------------------------------------------------------------

        protected void Push_Rental(object sender, EventArgs e)
        {
            if (Panel_Rental.Visible)
            {
                Panel_Rental.Visible = false;
            }
            else
            {
                Panel_Rental.Visible = true;
            }
        }

        protected void Push_Rental_Correct(object sender, EventArgs e)
        {

            DateTime? order_date;
            DateTime? youki_date;
            DateTime? seiki_date;
            DateTime? shipping_date;
            DateTime? receive_date;
            DateTime? send_d_date;
            DateTime? send_date;

            StringBuilder sb = new StringBuilder();
            sb.Append(HtmlEncode(DropDownList_order_year.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_order_month.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_order_day.Text));
            if (sb.ToString().Contains("--")){
                order_date = DateTime.MinValue;
            }
            else
            {
                order_date = DateTime.Parse(sb.ToString());
            }

            sb = new StringBuilder();
            sb.Append(HtmlEncode(DropDownList_youki_year.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_youki_month.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_youki_day.Text));
            if (sb.ToString().Contains("--")){
                youki_date = DateTime.MinValue;
            }
            else
            {
                youki_date = DateTime.Parse(sb.ToString());
            }

            sb = new StringBuilder();
            sb.Append(HtmlEncode(DropDownList_seiki_year.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_seiki_month.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_seiki_day.Text));
            if (sb.ToString().Contains("--")){
                seiki_date = DateTime.MinValue;
            }
            else
            {
                seiki_date = DateTime.Parse(sb.ToString());
            }

            sb = new StringBuilder();
            sb.Append(HtmlEncode(DropDownList_shipping_year.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_shipping_month.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_shipping_day.Text));
            if (sb.ToString().Contains("--")){
                shipping_date = DateTime.MinValue;
            }
            else
            {
                shipping_date = DateTime.Parse(sb.ToString());
            }

            sb = new StringBuilder();
            sb.Append(HtmlEncode(DropDownList_receive_year.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_receive_month.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_receive_day.Text));
            if (sb.ToString().Contains("--")){
                receive_date = DateTime.MinValue;
            }
            else
            {
                receive_date = DateTime.Parse(sb.ToString());
            }

            sb = new StringBuilder();
            sb.Append(HtmlEncode(DropDownList_send_d_year.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_send_d_month.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_send_d_day.Text));
            if (sb.ToString().Contains("--")){
                send_d_date = DateTime.MinValue;
            }
            else
            {
                send_d_date = DateTime.Parse(sb.ToString());
            }

            sb = new StringBuilder();
            sb.Append(HtmlEncode(DropDownList_send_year.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_send_month.Text));
            sb.Append("/");
            sb.Append(HtmlEncode(DropDownList_send_day.Text));
            if (sb.ToString().Contains("--")){
                send_date = DateTime.MinValue;
            }
            else
            {
                send_date = DateTime.Parse(sb.ToString());
            }


            //id
            string id = HtmlEncode(SessionManager.User.M_User.id).Trim();
            //uuid
            string uuid;
            if (TextBox_order_uuid.Text == "")
            {
                uuid = Guid.NewGuid().ToString().Trim();
            }
            else
            {
                uuid = HtmlEncode(TextBox_order_uuid.Text).Trim();
            }
            //発注者名
            string order_name = HtmlEncode(TextBox_order_name.Text);
            //リース商品名、リース商品タイプ
            string rental_name = HtmlEncode(TextBox_rental_name.Text);
            string rental_type = HtmlEncode(TextBox_rental_type.Text);
            //発注残、返却残
            int.TryParse(HtmlEncode(TextBox_order_rest.Text), System.Globalization.NumberStyles.Currency, null, out int order_rest);
            int.TryParse(HtmlEncode(TextBox_send_rest.Text), System.Globalization.NumberStyles.Currency, null, out int send_rest);
            //単価、個数
            string rental_tanka = HtmlEncode(TextBox_rental_tanka.Text);
            string rental_amount = HtmlEncode(TextBox_rental_amount.Text);
            int.TryParse(rental_tanka, System.Globalization.NumberStyles.Currency, null, out int rental_tanka_value);
            int.TryParse(rental_amount, System.Globalization.NumberStyles.Currency, null, out int rental_amount_value);
            //合計費用
            TextBox_rental_total_amount.Text = String.Format("{0:C}", rental_tanka_value * rental_amount_value);
            int rental_total_amount = rental_tanka_value * rental_amount_value;

            if (ClassLibrary.RentalClass.GetT_Rental(Global.GetConnection(), uuid) == null)
            {
                //新規登録
                ClassLibrary.RentalClass.SetT_RentalInsert(Global.GetConnection(), uuid, order_date, order_name, youki_date, seiki_date, order_rest, shipping_date, receive_date, send_d_date, send_rest, send_date, rental_name, rental_type, rental_tanka_value, rental_amount_value, rental_total_amount);
            }
            else
            {
                //更新登録
                ClassLibrary.RentalClass.SetT_RentalUpdate(Global.GetConnection(), uuid, order_date, order_name, youki_date, seiki_date, order_rest, shipping_date, receive_date, send_d_date, send_rest, send_date, rental_name, rental_type, rental_tanka_value, rental_amount_value, rental_total_amount);
            }

            GridView_Rental.DataBind();

            /*
            if (Session["args"].ToString() != "null")
            {
                int resetargs = int.Parse(Session["args"].ToString());
                if (GridView_Rental.Rows[resetargs].Cells[0].Text == uuid)
                {
                    //編集中なら該当uuidを赤色にする
                    GridView_Rental.Rows[resetargs].BackColor = System.Drawing.Color.Red;
                }
            }
            */

            for (int i = 0; i < GridView_Rental.Rows.Count; i++)
            {
                if (GridView_Rental.Rows[i].Cells[0].Text == TextBox_order_uuid.Text)
                {
                    Session.Add("args", i);
                    Session.Add("uuid", GridView_Rental.Rows[i].Cells[0].Text);
                    //GridView_Resetの選択行を赤色にする
                    //int resetargs = int.Parse(Session["args"].ToString());
                    GridView_Rental.Rows[i].BackColor = System.Drawing.Color.Red;
                    break;
                }
            }


        }

        protected void Change_Rental(object sender, EventArgs e)
        {
            //単価、個数
            string rental_tanka = HtmlEncode(TextBox_rental_tanka.Text);
            string rental_amount = HtmlEncode(TextBox_rental_amount.Text);
            int.TryParse(rental_tanka, System.Globalization.NumberStyles.Currency, null, out int rental_tanka_value);
            int.TryParse(rental_amount, System.Globalization.NumberStyles.Currency, null, out int rental_amount_value);
            //合計費用
            TextBox_rental_total_amount.Text = String.Format("{0:C}", rental_tanka_value * rental_amount_value);
            int rental_total_amount = rental_tanka_value * rental_amount_value;
            //int rental_total_amount = rental_tanka_value * rental_amount_value;
        }

        protected void Reset_Rental()
        {
            TextBox_order_uuid.Text = "";
            TextBox_order_name.Text = HtmlEncode(SessionManager.User.M_User.name1).Trim();
            DropDownList_order_year.SelectedValue = DateTime.Now.Year.ToString();
            DropDownList_order_month.SelectedValue = DateTime.Now.Month.ToString();
            DropDownList_order_day.SelectedValue = DateTime.Now.Day.ToString();
            TextBox_rental_tanka.Text = "0";
            TextBox_rental_amount.Text = "0";
            TextBox_rental_total_amount.Text = String.Format("{0:C}", 0);
        }

        protected void Push_Reset_Rental(object sender, EventArgs e)
        {
            TextBox_order_uuid.Text = "";
            if (Session["args"].ToString() != "null")
            {
                //GridView1の色を変えた色をもとに戻す
                int resetargs = int.Parse(Session["args"].ToString());
                GridView_Rental.Rows[resetargs].BackColor = System.Drawing.Color.Empty;
            }
            //init
            Session.Add("uuid", "null");
            Session.Add("args", "null");
        }


    }

}