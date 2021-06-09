using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using static System.Web.HttpUtility;

namespace WhereEver
{
    public partial class LoginList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetMaxYear();
                ResetMonthItem();
                SetTemp();

                DATASET.DataSet.T_LoginListDataTable dtLogin = Class2.GetLoginListDataTable(Global.GetConnection());

                DgTimeDetail.DataSource = dtLogin;
                DgTimeDetail.DataBind();

                Label_WhatNow.Text = "";
                Label_Weather.Text = "";
                Label_Weather2.Text = "";

                TextBox_EditTop.Text = "";
                Button_EditTop.Visible = true;
                Panel_EditTop.Visible = false;
            }

            //本日の日付を表示
            Label_Today.Text = DateTime.Now.ToString("yyyy年M月d日");

            SetMaxDate();
            Label_WeatherDate.Text = DateTime.Now.Date.ToString();
            Literal_js.Text = "";

            if (DropDownList_CF_month.SelectedValue != "--")
            {
                DropDownList_CF_month.BackColor = System.Drawing.Color.Empty;
            }
            if (DropDownList_CF_day.SelectedValue != "--")
            {
                DropDownList_CF_day.BackColor = System.Drawing.Color.Empty;
            }


            //宣言と初期化
            StringBuilder @sb = new StringBuilder();


            DATASET.DataSet.T_WeatherDataTable wdt = Class.Toppage.GetT_Weather(Global.GetConnection());
            if (wdt != null)
            {
                //初期化
                @sb = new StringBuilder();
                @sb.Append("<ul>");
                @sb.Append("<li>");
                @sb.Append("新宿区の10日間天気予報（出典：気象庁 tenki.jp");              
                if (!wdt[0].IsNull(@"Date"))
                {
                    @sb.Append(" ");
                    if (wdt[0].Date.Date < DateTime.Now.Date)
                    {
                        if (DateTime.Now.Date < wdt[wdt.Count-1].Date.Date)
                        {
                            @sb.Append(DateTime.Now.ToShortDateString());
                        }
                        else
                        {
                            //更新していない
                            @sb.Append(wdt[wdt.Count-1].Date.ToShortDateString());
                        }
                    }
                    else
                    {
                        //現在か未来のできごとについて語っている
                        @sb.Append(DateTime.Now.ToShortDateString());
                    }
                    @sb.Append("時点）");
                }
                else
                {
                    @sb.Append("）");
                }
                @sb.Append("</li>");
                for (int i = 0; i < wdt.Count; i++)
                {

                    if (wdt[i].Date.Date < DateTime.Now.Date)
                    {
                        continue;
                    }


                        @sb.Append("<li>");
                    if (!wdt[i].IsNull(@"Date"))
                    {
                        @sb.Append(wdt[i].Date.ToShortDateString());
                        @sb.Append("（");
                        @sb.Append(string.Format("{0:ddd}", wdt[i].Date));
                        @sb.Append("）");
                    }
                    if (!wdt[i].IsNull(@"Weather"))
                    {
                        @sb.Append(wdt[i].Weather);
                        @sb.Append("　");
                    }
                    if (!wdt[i].IsNull(@"MaxTemp"))
                    {
                        @sb.Append("<span class=\"hot\">");
                        @sb.Append(wdt[i].MaxTemp);
                        @sb.Append("℃</span>");
                    }
                    @sb.Append("/");
                    if (!wdt[i].IsNull(@"MinTemp"))
                    {
                        @sb.Append("<span class=\"cold\">");
                        @sb.Append(wdt[i].MinTemp);
                        @sb.Append("℃</span>");
                    }
                    @sb.Append(" 降水確率：");
                    if (!wdt[i].IsNull(@"Rain_p"))
                    {
                        @sb.Append("<span>");
                        @sb.Append(wdt[i].Rain_p);
                        @sb.Append("%</span>");
                    }
                    @sb.Append("</li>");
                }
                @sb.Append("</ul>");
                Label_Weather.Text = sb.ToString();
                Label_Weather2.Text = sb.ToString();
            }
            else
            {
                Label_Weather.Text = @"No Data";
                Label_Weather2.Text = @"No Data";
            }


            //WhatNowをロード
            Label_WhatNow.Text = "";
            DATASET.DataSet.T_ScheduleDataTable dts = Class.Schedule.GetT_ScheduleDataTable(Global.GetConnection(), DateTime.Now.ToString());
            if(dts != null)
            {
                for(int i = 0;  i < dts.Count; i++)
                {
                    //初期化
                    @sb = new StringBuilder();
                    if (!dts[i].IsNull(@"title"))
                    {
                        @sb.Append(dts[i].title);
                    }
                    if (!dts[i].IsNull(@"time"))
                    {
                        @sb.Append(@"　");
                        @sb.Append(dts[i].time);
                        @sb.Append(@"～");
                    }
                    if (!dts[i].IsNull(@"name"))
                    {
                        if(dts[i].name != "")
                        {
                            @sb.Append(@"　担当：");
                            @sb.Append(dts[i].name);
                        }
                    }

                    @sb.Append("<br />");
                    Label_WhatNow.Text += @sb.ToString();
                }
            }
            else
            {
                Label_WhatNow.Text = @"本日の予定はありません。<br />";
            }

            //初期化
            @sb = new StringBuilder();
            //ここに更新内容をAppend
            DATASET.DataSet.T_TopPageRow dr = Class.Toppage.GetT_TopPage(Global.GetConnection());
            if(dr != null)
            {
                if (dr.TopPage != "")
                {
                    @sb.Append(dr.TopPage);
                    //@sb.Replace("<li>", "<li type=\"circle\">");
                    @sb.Append("<br />");
                    @sb.Append("最終更新：");
                    @sb.Append(dr.DateTime);
                }
                else
                {
                    @sb.Append("なし");
                }
            }
            else
            {
                @sb.Append("警告：T_TopPageのデータベースがNULLです。");
            }
            @sb.Append("<br />");
            Label_Yotei.Text = @sb.ToString();
        }



        protected void SetTemp()
        {
            DropDownList_MaxTemp.Items.Clear();
            DropDownList_MinTemp.Items.Clear();

            const int maxtemp = 60;
            const int mintemp = -60;

            for(int i = maxtemp; i >= mintemp; i--)
            {
                DropDownList_MaxTemp.Items.Insert((i - maxtemp)*-1, i.ToString());
                DropDownList_MinTemp.Items.Insert((i - maxtemp)*-1, i.ToString());
            }
            DropDownList_MaxTemp.SelectedValue = "24";
            DropDownList_MinTemp.SelectedValue = "23";


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
            DropDownList_CF_year.Items.Clear();
        }

        /// <summary>
        /// DropDownList_monthを初期化します。
        /// </summary>
        protected void ResetMonthItem()
        {
            DropDownList_CF_month.Items.Clear();
            DropDownList_CF_month.Items.Insert(0, "--");

            for (int i = 1; i <= 12; i++)
            {
                DropDownList_CF_month.Items.Insert(i, i.ToString());
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
            DropDownList_CF_year.Items.Insert(i, item);
        }

        /// <summary>
        /// 特殊なDwopDownList_yearを任意（現在）の年にします。
        /// </summary>
        /// <param name="item"></param>
        protected void SetYearValue(string item)
        {
            DropDownList_CF_year.SelectedValue = item;
        }

        /// <summary>
        /// 日付入力を全初期化します。
        /// </summary>
        protected void SetResetDate()
        {
            //day
            //初期化
            DropDownList_CF_day.Items.Clear();
            DropDownList_CF_day.Items.Insert(0, "--");
            //-------------------------------------------------------------------------------

            //month
            //初期化
            DropDownList_CF_month.SelectedValue = "--";

            SetMaxYear();
        }

        protected void SetMaxDate()
        {
            //---------------------------------------------------------------------------------
            //選択可能な日付の最大値を設定
            //---------------------------------------------------------------------------------
            //宣言
            int i;
            int m;
            int y;
            int maxday;
            string memory1;
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

        }








        protected void DgTimeDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {

                DATASET.DataSet.T_LoginListRow dr = (e.Item.DataItem as DataRowView).Row as DATASET.DataSet.T_LoginListRow;
                Label lblName = e.Item.FindControl("lblName") as Label;
                Label lblLoginTime = e.Item.FindControl("lblLoginTime") as Label;
                Label lblLogoutTime = e.Item.FindControl("lblLogoutTime") as Label;
                DATASET.DataSet.T_LoginListRow dl = Class.Login.UserLoginMAXTime(Global.GetConnection(), dr.name.ToString());
                DATASET.DataSet.T_LogoutListRow dt = Class.Login.UserLogoutMAXTime(Global.GetConnection(), dr.name.ToString());

                DATASET.DataSet.M_UserRow ml = Class2.Getname1(Global.GetConnection(), dr.name.ToString());
                lblName.Text = ml.name1.ToString();
                lblLoginTime.Text = dl.Date.ToString();
                if(dt == null)
                {
                    lblLogoutTime.Text = "";
                }
                else
                {
                    lblLogoutTime.Text = dt.Date.ToString();
                }

                //----------------------------------------------------
                //ユーザーアイコン追加分
                //----------------------------------------------------
                Label lblIcon = e.Item.FindControl("lblIcon") as Label;
                string result;
                if (dr != null)
                {
                    if (!dr.IsNull("name")) //idがnullのため値が同じnameで代用（要最適化）
                    {
                        //ThumbnailDownLoad by DataBase
                        string id = dr.name.Trim();
                        result = ClassLibrary.FileShareClass.Get_Thumbnail_DownLoad_src(Page.Response, id, 20000);                       
                    }
                    else
                    {
                        result = @"No_Image";
                    }

                    if(result == "No_Image")
                    {
                        result = "<img width=\"150px\" height=\"150px\" src=\"m2m-logo.png\" alt=\"Replaced_for_No_Image\" />";
                    }
                        lblIcon.Text = @result;
                }
                //----------------------------------------------------
            }
        }

        protected void btnOut_Click(object sender, EventArgs e)
        {
            Class.Logout.InsertLogoutList(Global.GetConnection());
            this.Response.Redirect("Login.aspx", false);
        }

        protected void btnKanri_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("../管理ページ/Kanri.aspx", false);
        }



        protected void btnReformTop_Click(object sender, EventArgs e)
        {
            //sql更新 ※全部Nullだと更新できません。DBのDateTimeには何か値を入れておいて下さい。

            StringBuilder sb = new StringBuilder();
            sb.Append(HtmlEncode(TextBox_EditTop.Text));
            //一部タグのみ許可
            sb.Replace("&lt;br /&gt;", "<br />");
            sb.Replace("&lt;p&gt;", "<p>");
            sb.Replace("&lt;/p&gt;", "</p>");
            sb.Replace("&lt;ol&gt;", "<ol>");
            sb.Replace("&lt;/ol&gt;", "</ol>");
            sb.Replace("&lt;li&gt;", "<li>");
            sb.Replace("&lt;/li&gt;", "</li>");
            sb.Replace("&lt;ul&gt;", "<ul>");
            sb.Replace("&lt;/ul&gt;", "</ul>");
            Class.Toppage.SetT_TopPageUpdate(Global.GetConnection(), sb.ToString());
            //リダイレクト
            this.Response.Redirect("LoginList.aspx", false);
        }



        protected void btnReformTopDel_Click(object sender, EventArgs e)
        {
            TextBox_EditTop.Text = "";
        }

        protected void btnReformTopReload_Click(object sender, EventArgs e)
        {
            //再読み込み
            DATASET.DataSet.T_TopPageRow dr = Class.Toppage.GetT_TopPage(Global.GetConnection());
            if (dr != null)
            {
                TextBox_EditTop.Text = dr.TopPage;
            }
        }



        /// <summary>
        /// 天気予報を挿入/更新します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReformWeather_Click(object sender, EventArgs e)
        {
            //宣言
            DateTime date;
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            int maxtemp;
            int mintemp;
            int rain_p;

            if (DropDownList_Weather1.SelectedValue == "")
            {
                if (DropDownList_Weather2.SelectedValue == "")
                {
                    //不正な天気
                    Label_WeatherResult.Text = "※天気を選択して下さい。";
                    return;
                }
            }
            else
            {
                sb.Append(DropDownList_Weather1.SelectedValue);
            }

            if (DropDownList_WeatherN.SelectedValue != "" && DropDownList_Weather2.SelectedValue != "")
            {
                if (DropDownList_Weather1.SelectedValue == "")
                {
                    sb.Append(DropDownList_Weather2.SelectedValue);
                }
                else
                {
                    sb.Append(DropDownList_WeatherN.SelectedValue);
                    sb.Append(DropDownList_Weather2.SelectedValue);
                }
            }

            //気温　セルシウス（摂氏）　※日本で使うことが前提のためファーレンハイト（華氏）は使用しない
            maxtemp = int.Parse(DropDownList_MaxTemp.SelectedValue);
            mintemp = int.Parse(DropDownList_MinTemp.SelectedValue);

            if(maxtemp < mintemp)
            {
                //不正な気温
                Label_WeatherResult.Text = "※気温が不正です。";
                return;
            }

            if(DropDownList_CF_year.SelectedValue==""|| DropDownList_CF_month.SelectedValue==""|| DropDownList_CF_day.SelectedValue == "")
            {
                //不正な日付
                Label_WeatherResult.Text = "※日付を選択して下さい。";
                return;
            }

            sb2.Append(DropDownList_CF_year.SelectedValue);
            sb2.Append("/");
            sb2.Append(DropDownList_CF_month.SelectedValue);
            sb2.Append("/");
            sb2.Append(DropDownList_CF_day.SelectedValue);
            date = DateTime.Parse(sb2.ToString()).Date;

            rain_p = int.Parse(DropDownList_Rain_p.SelectedValue);

            if (Class.Toppage.GetT_WeatherSelect(Global.GetConnection(), date) == null)
            {
                Class.Toppage.SetT_WeatherInsert(Global.GetConnection(), date, sb.ToString(), maxtemp, mintemp, rain_p);
                Label_WeatherResult.Text = "新しい天気予報を保存しました。";
            }
            else
            {
                Class.Toppage.SetT_WeatherUpdate(Global.GetConnection(), date, sb.ToString(), maxtemp, mintemp, rain_p);
                Label_WeatherResult.Text = "既存の天気予報に上書き保存しました。";
            }





        }




        protected void btnEditTop_Click(object sender, EventArgs e)
        {
            DATASET.DataSet.T_TopPageRow dr = Class.Toppage.GetT_TopPage(Global.GetConnection());
            if (dr != null)
            {
                TextBox_EditTop.Text = dr.TopPage;
            }
            Button_EditTop.Visible = false;
            Panel_EditTop.Visible = true;

            Button_EditWeather.Visible = false;

            Panel_Main.Visible = false;
            Panel_Navigation.Visible = false;
        }



        protected void btnReformTopEnd_Click(object sender, EventArgs e)
        {
            Button_EditTop.Visible = true;
            Panel_EditTop.Visible = false;

            Button_EditWeather.Visible = true;

            Panel_Main.Visible = true;
            Panel_Navigation.Visible = true;
            TextBox_EditTop.Text = "";
        }

        /// <summary>
        /// 天気予報パネルを閉じます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReformWeatherEnd_Click(object sender, EventArgs e)
        {
            Button_EditTop.Visible = true;

            Button_EditWeather.Visible = true;
            Panel_EditWeather.Visible = false;

            Panel_Main.Visible = true;
            Panel_Navigation.Visible = true;
        }


        /// <summary>
        /// 天気予報パネルを開きます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditWeather_Click(object sender, EventArgs e)
        {
            Button_EditTop.Visible = false;

            Button_EditWeather.Visible = false;
            Panel_EditWeather.Visible = true;

            Panel_Main.Visible = false;
            Panel_Navigation.Visible = false;
        }

        protected void Postback_Weather(object sender, EventArgs e)
        {
            //↓ASPだと何故か機能しない
            //Literal_js.Text = "<script type=\"text / javascript\"> var element = document.getElementById(\"weather\"); var rect = element.getBoundingClientRect(); var position = rect.top; function scroll(){ scrollTo(0, position); } scroll();</script>";

            //↓こう書くとセッション変数に入れない限りSelectedValueもリセットされてしまう。メモリ管理の方法としてダメなやり方。MVCには流用できない。
            //this.Response.Redirect("#editweather", false);

            Label_WeatherResult.Text = "";

        }
    }
}