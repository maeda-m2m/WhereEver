using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Class
{
    public class Logout
    {
        public static void GetMAXLoginTimeName()
        {
            string cstr = ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql1 = "SELECT name FROM T_LoginList WHERE date = ( SELECT MAX(date) FROM T_LoginList)";
                SqlDataAdapter da1 = new SqlDataAdapter(sql1, connection);
                DATASET.DataSet.T_LogoutListDataTable dt = new DATASET.DataSet.T_LogoutListDataTable();
                da1.Fill(dt);
                string Name = dt.Rows[0][1].ToString();
                dt.Rows[0][2] = DateTime.Now;
            }
        }
        public static void InsertLogoutList(DATASET.DataSet.T_LoginListDataTable dt, SqlConnection sql)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sql);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_LogoutList";
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();

            sql.Open();
            SqlTransaction sqltra = sql.BeginTransaction();

            da.SelectCommand.Transaction = da.InsertCommand.Transaction = sqltra;

            da.Update(dt);
            sqltra.Commit();
            sql.Close();

        }
    }
}