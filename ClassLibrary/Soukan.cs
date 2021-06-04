﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WhereEver.ClassLibrary
{
    public class Soukan
    {



        //----------------------------------------------------------------------------------------------------

        /// <summary>
        /// 要素List<double> x, List<double> yより、相関係数rを求めます。
        /// Listのいずれかがnullの場合は0をreturnします。
        /// ピアソンとスピアマンはこちら
        /// https://kusuri-jouhou.com/statistics/soukan.html
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetSoukan(List<double> x, List<double> y)
        {

            if (x == null || y == null)
            {
                //中身なし
                return 0;
            }

            //x基準
            //int number = x.Count();


            /*
            Σの公式一覧
            1. [∑ k=1 n] a = na
            2. [∑ k=1 n] k = (1/2n)(n+1)
            3. [∑ k=1 n] k^2 = (1/6n)(n+1)(2n+1)
            4. [∑ k=1 n] k^3 = {(1/2n)(n+1)}2
            5. [∑ k=1 n] ar^(k−1) = a(1−rn)/1−r = a(rn–1)/r−1
             */

            //共分散(Sxy): [Σ i=1 n] (データx - データxの平均)(データy - データyの平均)
            double s_xy = GetS_xy(x, y);

            //xの標準偏差: √([Σ i=1 n] (データx - データxの平均)^2)
            //yの標準偏差: √([Σ i=1 n] (データy - データyの平均)^2)
            double h = GetHensa(x, y);

            //相関係数(r): 共分散 / xの標準偏差 * yの標準偏差
            double r = (double)s_xy / (double)h;

            return r;   //相関係数r
        }



        /// <summary>
        /// 要素List<double> x, List<double> yの共分散を求めます。
        /// Σ(データx - データxの平均)(データy - データyの平均)
        /// Listのいずれかがnullの場合は0をreturnします。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetS_xy(List<double> x, List<double> y)
        {

            if (x == null || y == null)
            {
                //中身なし
                return 0;
            }

            //x基準
            int n = x.Count();

            double sum = 0; //合計
            for (int k = 1; k <= n; k++)
            {
                sum += (x[k - 1] * k - x.Average()) * (y[k - 1] * k - y.Average()); //x基準　nullはないものとみなす
            }

            return sum;
        }

        /// <summary>
        /// 要素List<double> x, List<double> yの標準偏差を求めます。
        /// √(Σ(データx - データxの平均)^2 /(n - 1)) * √(Σ(データy - データyの平均)^2)
        /// Listのいずれかがnullの場合は0をreturnします。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetHensa(List<double> x, List<double> y)
        {

            if (x == null || y == null)
            {
                //中身なし
                return 0;
            }

            //x基準
            int n = x.Count();

            double sum = 0; //合計
            for (int k = 1; k <= n; k++)
            {
                sum += Math.Sqrt((x[k - 1] * k - x.Average()) * (x[k - 1] * k - x.Average())) * Math.Sqrt((y[k - 1] * k - y.Average()) * (y[k - 1] * k - y.Average())); //x基準とy基準　nullはないものとみなす
            }

            return sum;
        }


        /// <summary>
        /// ピアソンの相関係数rを求めます。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetParametric(List<double> x, List<double> y)
        {

            int count_x = x.Count;
            int count_y = y.Count;

            //Σxi   xを全て足したもの
            double sigma_xi = x.Sum();
            //Σyi   yを全て足したもの
            double sigma_yi = y.Sum();

            //宣言と初期化　偏差平方和
            double sigma_xi2 = 0;
            double sigma_yi2 = 0;
            double sigma_xiyi = 0;

            //Σxi^2 xの２乗を全て足したもの
            for(int i = 1; i < count_x; i++)
            {
                sigma_xi2 += (x[i - 1] * x[i - 1]);
            }
            //Σyi^2 yの２乗を全て足したもの
            for (int i = 1; i < count_x; i++)
            {
                sigma_yi2 += (y[i - 1] * y[i - 1]);
            }
            //Σxiyi xとyの積を全て足したもの
            for (int i = 1; i < count_x; i++)
            {
                sigma_xiyi += (x[i - 1] * y[i - 1]);
            }

            //Sxx, Sxy, Syyを求める。countは本来n(Number)のため、xとyの値数がnullで異なるとうまくいかない。

            double Sxx = sigma_xi2 - (sigma_xi * sigma_xi / count_x);
            double Syy = sigma_yi2 - (sigma_yi * sigma_yi / count_y);
            double Sxy = sigma_xiyi - (sigma_xi * sigma_yi / count_x); //x優先

            //統計量ρを求める（ρが全角のため、pで代用）
            double pearson_p = Sxy / (Math.Sqrt(Sxx * Syy));
            return pearson_p;
        }

        public static double GetPearson(double r, double n)
        {
            //正規分布が前提　FisherのF>=10
            // 自由度 = サンプル数 - 2;
            // ※サンプル数とは片方の群;

            // t分布に従う検定統計量 t = Pearsonの相関係数r * (サンプル数 - 2) / SQRT(1 - (r*r));
            double t = r * (n - 2) / Math.Sqrt(1 - (r * r));

            // tが得られる確率 = p値 TDIST(t);

            return t;
        }



        /// <summary>
        /// Spearmanのノンパラメトリック検定を行います。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double GetNonParametric(List<double> x, List<double> y)
        {
            //RANK
            //rxi
            x.Sort();
            //ryi
            y.Sort();
            //差
            double di = 0;
            double di2 = 0;
            int n = x.Count;


            for(int i=1; i < n; i++)
            {
                di += y[i - 1] - x[i - 1];
                di2 += di * di; //Σdi^2
            }

            //正規分布しないデータに用いる。
            double rs = 1 - (6 * di2) / ((n * n * n) - n);

            if (n <= 30)
            {
                //spearman検定表を使用
                //p >= a ×帰無仮説あり
                //p < a 〇帰無仮説棄却
            }
            if (n > 30)
            {
                //t=t=rs√((n-2)/1-rs^2)が自由度df=n-1のt分布→t分布表
            }
            return rs;
        }



            //----------------------------------------------------------------------------------------------------


            /// <summary>
            /// 申請コンフィグテーブルにインサートします。
            /// </summary>
            /// <param name="sqlConnection"></param>
            /// <param name="id"></param>
            /// <param name="tableName"></param>
            /// <param name="item_A"></param>
            /// <param name="item_B"></param>
            public static void SetT_Soukan_Main(SqlConnection sqlConnection, string id, string tableName, float item_A, float item_B)
        {
            sqlConnection.Open();

            //Create the Update Command.
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            //Sql Commandを作成します。
            SqlCommand command = sqlConnection.CreateCommand();

            //ファイルを書き込み可能なようにオープンしてSqlのデータをアップデートします。
            //Start a local transaction. usingブロックを抜けると自動でcloseされます。
            using (SqlTransaction transaction = sqlConnection.BeginTransaction())
            {

                //Must assign both transaction object and connection
                //to Command object for apending local transaction
                command.Connection = sqlConnection;
                command.Transaction = transaction;

                try
                {
                    //Add the paramaters for the Updatecommand.必ずダブルクオーテーションで@変数の宣言を囲んでください。command.CommandTextで使用するものは、必ずすべて宣言してください。
                    //-------------------------------------------------------------------------------------------------------------------
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar, 50, "id")).Value = id.Trim();
                    command.Parameters.Add(new SqlParameter("@uuid", System.Data.SqlDbType.NVarChar, 50, "uuid")).Value = Guid.NewGuid().ToString();
                    command.Parameters.Add(new SqlParameter("@a", System.Data.SqlDbType.Float, 8, "item_A")).Value = (float)item_A;
                    command.Parameters.Add(new SqlParameter("@b", System.Data.SqlDbType.Float, 8, "item_B")).Value = (float)item_B;
                    command.Parameters.Add(new SqlParameter("@table", System.Data.SqlDbType.NVarChar, 50, "TableName")).Value = tableName.Trim();
                    command.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.DateTime, 8, "DateTime")).Value = DateTime.Now;

                    //↓SqlCommand command = sqlConnection.CreateCommand();を実行した場合はこちらでSQL文を入力
                    command.CommandText = "INSERT INTO T_Soukan_Main(id, uuid, TableName, item_A, item_B, DateTime) VALUES(LTRIM(RTRIM(@id)), @uuid, @table, @a, @b, @date)";


                    //このメソッドでは、XmlCommandTypeプロパティおよびCommandTextプロパティを使用してSQL文またはコマンドを実行し、影響を受ける行数を戻します（必須）。 
                    //ここでエラーが出る場合は、宣言やSql文が不正な場合があります。
                    command.ExecuteNonQuery();


                    //Attempt to commit the transaction.
                    da.UpdateCommand = command;
                    transaction.Commit();

                    //Console.WriteLine("Update Completed");

                }
                catch
                {
                    //catch文
                    //Console.WriteLine("Update Failed");
                    transaction.Rollback();
                }

            } //sqlConnection.Close();

            // データベースの接続終了
            sqlConnection.Close();
            return;

        }




    }
}