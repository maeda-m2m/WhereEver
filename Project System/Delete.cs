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
        internal static void DeleteBig(string Bigname,int spId)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "DELETE FROM T_PdbKanri WHERE PBigname = @i and Pid = @spId";
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@i", Bigname);
                da.SelectCommand.Parameters.AddWithValue("@spId", spId);
                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        internal static void DeleteMiddle(string Bigname, string Middleid,int Pid)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "DELETE FROM T_PdbKanri WHERE PBigname = @i and PMiddleid = @m and Pid = @Pid";
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@i", Bigname);
                da.SelectCommand.Parameters.AddWithValue("@m", Middleid);
                da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}