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
            da.SelectCommand.CommandText = "select * from T_Chat Order by No asc";
            DATASET.DataSet.T_ChatDataTable dt = new DataSet.T_ChatDataTable();
            da.Fill(dt);
            return (dt);
        }
    }
}