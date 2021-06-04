using System;
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
            //(x-x.Average)(y-y.Average)
            for (int k = 1; k <= n; k++)
            {
                sum += (x[k - 1] - x.Average()) * (y[k - 1] - y.Average()); //x基準　nullはないものとみなす
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
            //Math.Sqrt(x-x.average)^2*(y-y.Average)^2)
            for (int k = 1; k <= n; k++)
            {
                sum += Math.Sqrt((x[k - 1] - x.Average()) * (x[k - 1] - x.Average())) * Math.Sqrt((y[k - 1] - y.Average()) * (y[k - 1] - y.Average())); //x基準とy基準　nullはないものとみなす
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

            int n = x.Count;

            //Σxi   xを全て足したもの
            double sigma_xi = x.Sum();
            //Σyi   yを全て足したもの
            double sigma_yi = y.Sum();

            //宣言と初期化　偏差平方和
            double sigma_xi2 = 0;
            double sigma_yi2 = 0;
            double sigma_xiyi = 0;

            //Σxi^2 xの２乗を全て足したもの
            for(int i = 1; i <= n; i++)
            {
                sigma_xi2 += (x[i - 1] * x[i - 1]);
            }
            //Σyi^2 yの２乗を全て足したもの
            for (int i = 1; i <= n; i++)
            {
                sigma_yi2 += (y[i - 1] * y[i - 1]);
            }
            //Σxiyi xとyの積を全て足したもの
            for (int i = 1; i <= n; i++)
            {
                sigma_xiyi += (x[i - 1] * y[i - 1]);
            }

            //Sxx, Sxy, Syyを求める。countは本来n(Number)のため、xとyの値数がnullで異なるとうまくいかない。
            double Sxx = sigma_xi2 - ((sigma_xi * sigma_xi) / n);
            double Syy = sigma_yi2 - ((sigma_yi * sigma_yi) / n);
            double Sxy = sigma_xiyi - ((sigma_xi * sigma_yi) / n); //x優先
            //Pearsonの相関係数p_rを求める
            double pearson_r = Sxy / (Math.Sqrt(Sxx * Syy));
            return pearson_r;

            //Pearsonの相関係数p_r
            //double result = (((sigma_xi * sigma_yi) - (x.Average() * y.Average())));
            //double result2 = Math.Sqrt(sigma_xi2 - n * (x.Average() * x.Average()) * sigma_yi2 - n * (y.Average() * y.Average()));
            //result = result / result2;
            //return result;
        }

        /// <summary>
        /// 正規分布を対象とするPearsonのt検定を行います。
        /// </summary>
        /// <param name="r"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double GetPearson(double r, double n)
        {
            //正規分布が前提　FisherのF>=10
            // 自由度f = サンプル数 - 2;
            // ※サンプル数とは片方の群;

            // t分布に従う検定統計量 t = Pearsonの相関係数r * SQRT(サンプル数 - 2) / SQRT(1 - (r*r));
            double t = r * Math.Sqrt(n - 2) / Math.Sqrt(1 - (r * r));

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


            for(int i=1; i <= n; i++)
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

        /// <summary>
        /// Spearmanの検定の一種を行います。下記は標本数が20以上あると有効です。
        /// </summary>
        /// <param name="r"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double GetSpearman(double r, double n)
        {
            //正規分布は不要
            // 自由度f = サンプル数 - 2;
            // ※サンプル数とは片方の群;

            //帰無仮説が真（2変数に相関なし）であると仮定した場合、スチューデントのt分布(自由度n-2)に従う検定統計量 t = Spearmanの相関係数r / SQRT((1 - r*r) / (n - 2));
            double t = r / Math.Sqrt((1 - (r * r)) / (n - 2));

            // tが得られる確率 = p値 TDIST(t);

            return t;
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