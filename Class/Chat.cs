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

                da.SelectCommand.Parameters.AddWithValue("@i", id);
            }
        }
    }
}