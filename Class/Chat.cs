using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using WhereEver.DATASET;

namespace WhereEver.Class
{
    public class Chat
    {
        internal static DATASET.DataSet.T_ChatDataTable GetChatDataTable(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText = "select * from T_Chat Order by No";
            DATASET.DataSet.T_ChatDataTable dt = new DataSet.T_ChatDataTable();
            da.Fill(dt);
            return (dt);
        }
        internal static void DeleteChat(string id)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "DELETE FROM T_Chat WHERE No = @i";
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@i", Int32.Parse(id));
                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        internal static void UpdateChat(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText = "select * from T_Chat order by Date";
            DATASET.DataSet.T_ChatDataTable dt = new DataSet.T_ChatDataTable();
            da.Fill(dt);

            da.UpdateCommand = (new SqlCommandBuilder(da)).GetUpdateCommand();
            SqlTransaction sql = null;
            sqlConnection.Open();
            sql = sqlConnection.BeginTransaction();
            da.SelectCommand.Transaction = da.UpdateCommand.Transaction = sql;
            for (int i = 0; i < dt.Count; i++)
            {
                dt[i].No = i + 1;
            }
            da.Update(dt);
            sql.Commit();

        }
        internal static DATASET.DataSet.T_ChatRow GetMaxHentouNoRow(SqlConnection sqlConnection, string No)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select MAX(HentouNo) as HentouNo from T_Chat where No like @No";
            da.SelectCommand.Parameters.AddWithValue("@No", No);
            DATASET.DataSet.T_ChatDataTable dt = new DATASET.DataSet.T_ChatDataTable();
            da.Fill(dt);
            return dt[0];
        }
    }
}