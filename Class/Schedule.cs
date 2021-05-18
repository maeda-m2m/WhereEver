using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.Class
{
    public class Schedule
    {

        //------------------------------------------------------------------------------------------------------------
        //T_Shinsei_Config
        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// T_Shinsei_ConfigのDataTableを返します。
        /// 引数はDATASET.DataSet.T_Shinsei_ConfigDataTable型で参照して下さい。
        /// 中身がない場合や入力値が不正な場合はnullを返します。
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <param name="id"></param>
        /// <returns>DATASET.DataSet.T_Shinsei_ConfigDataTable</returns>
        public static DATASET.DataSet.T_ScheduleDataTable GetT_ScheduleDataTable(SqlConnection sqlConnection, string date)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //パラメータを取得（必要のないパラメータを設定するとコンパイルエラーする）
            da.SelectCommand.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.DateTime, 8, "date")).Value = date;

            da.SelectCommand.CommandText =
                "SELECT * FROM T_Schedule WHERE  CONVERT(date, [date]) = CONVERT(date, LTRIM(RTRIM(@date))) ORDER BY [date] ASC";

            //特定のDataTableをインスタンス化
            DATASET.DataSet.T_ScheduleDataTable dt = new DATASET.DataSet.T_ScheduleDataTable();



                //↓でコンパイルエラーが出るときはWeb.configに誤りがある場合があります。
                da.Fill(dt);

                if (dt.Count >= 1)
                {
                    //中身あり
                    return dt;  //dt[0]の中にflag_del_popなどが入っています。

                }
                else
                {
                    //中身なし
                    return null;
                }
            try
            {
            }
            catch
            {
                //不正な値が入力された場合はnullを返します。
                return null;
            }

        }





    }
}