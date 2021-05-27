using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            }

            SetMaxDate();

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
        }

        /// <summary>
        /// DropDownList_monthを初期化します。
        /// </summary>
        protected void ResetMonthItem()
        {
            DropDownList_PL_month_s.Items.Clear();
            DropDownList_PL_month_g.Items.Clear();
            DropDownList_PL_month_s.Items.Insert(0, "--");
            DropDownList_PL_month_g.Items.Insert(0, "--");

            for (int i=1; i <= 12; i++) {
                DropDownList_PL_month_s.Items.Insert(i, i.ToString());
                DropDownList_PL_month_g.Items.Insert(i, i.ToString());
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
            //-------------------------------------------------------------------------------

            //month
            //初期化
            DropDownList_PL_month_s.SelectedValue = "--";
            DropDownList_PL_month_g.SelectedValue = "--";

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
        }



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

        protected void Push_Check_PL(object sender, EventArgs e)
        {
            Sum_PL();
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
            pl_sum -= value;

            Label_UriageSourieki.Text = string.Format("{0:C}", pl_sum);

            //売上総利益率（％）＝売上総利益÷売上高×100%
            ri_r = (float)pl_sum / (float)uriagedaka;
            Label_ArariR.Text = string.Format("{0:#.#%}", (double)ri_r);

            str = TextBox_HanbaiKanrihi.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum -= value;

            str = TextBox_EigyouRieki.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            //売上高営業利益率（％）＝営業利益÷売上高×100%
            ri_r = (float)value / (float)uriagedaka;
            Label_EigyouR.Text = string.Format("{0:#.#%}", (double)ri_r);

            str = TextBox_EigyougaiHiyou.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum -= value;

            Label_KeijyouRieki.Text = string.Format("{0:C}", pl_sum);
            //売上高経常利益率（％）＝経常利益÷売上高×100%
            ri_r = (float)pl_sum / (float)uriagedaka;
            Label_KeijyouR.Text = string.Format("{0:#.#%}", (double)ri_r);

            str = TextBox_TokubetsuRieki.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum += value;

            Label_Zeibikimae.Text = string.Format("{0:C}", pl_sum);

            str = TextBox_TokubetsuSonshitsu.Text;
            int.TryParse(str, System.Globalization.NumberStyles.Currency, null, out value);
            pl_sum -= value;

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

            Label_ArariR.Text = string.Format("{0:#.#%}", (double)0);
            Label_EigyouR.Text = string.Format("{0:#.#%}", (double)0);
            Label_KeijyouR.Text = string.Format("{0:#.#%}", (double)0);

            DropDownList_PL_month_s.SelectedValue = "4";
            DropDownList_PL_month_g.SelectedValue = "3";
            SetMaxDate();
            DropDownList_PL_day_s.SelectedValue = "1";
            DropDownList_PL_day_g.SelectedValue = "31";
        }

    }

}