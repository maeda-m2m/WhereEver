using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Project_System
{
    public class Delete
    {
        internal static void DeleteProject(string id)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "DELETE FROM T_Pdb WHERE Pid = @i";
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@i", id);

                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}