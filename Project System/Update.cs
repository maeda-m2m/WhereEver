using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Project_System
{
    public class Update
    {
        internal static void UpdateProject(DATASET.DataSet.T_PdbRow dt)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "update T_Pdb set " +
                    "Pname = @Pname , Pcustomer = @Pcustomer , Presponsible = @Presponsible ,Pcategory = @Pcategory ,Pstarttime = @Pstarttime ,Povertime = @Povertime " +
                    "where Pid = @id ";

                SqlDataAdapter da = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@Pname", dt.Pname);
                da.SelectCommand.Parameters.AddWithValue("@Pcustomer", dt.Pcustomer);
                da.SelectCommand.Parameters.AddWithValue("@Presponsible", dt.Presponsible);
                da.SelectCommand.Parameters.AddWithValue("@Pcategory", dt.Pcategory);
                da.SelectCommand.Parameters.AddWithValue("@Pstarttime", dt.Pstarttime);
                da.SelectCommand.Parameters.AddWithValue("@Povertime", dt.Povertime);
                da.SelectCommand.Parameters.AddWithValue("@id", dt.Pid);

                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        internal static void UpdateMiddle(DATASET.DataSet.T_PdbKanriRow dt)
        {
            //string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            //using (SqlConnection connection = new SqlConnection(cstr))
            //{
            //    string sql = "update T_PdbKanri set " +
            //        "Pname = @Pname , Pcustomer = @Pcustomer , Presponsible = @Presponsible ,Pcategory = @Pcategory ,Pstarttime = @Pstarttime ,Povertime = @Povertime " +
            //        "where Pid = @id ";

            //    SqlDataAdapter da = new SqlDataAdapter(sql, connection);

            //    da.SelectCommand.Parameters.AddWithValue("@Pname", dt.Pname);
            //    da.SelectCommand.Parameters.AddWithValue("@Pcustomer", dt.Pcustomer);
            //    da.SelectCommand.Parameters.AddWithValue("@Presponsible", dt.Presponsible);
            //    da.SelectCommand.Parameters.AddWithValue("@Pcategory", dt.Pcategory);
            //    da.SelectCommand.Parameters.AddWithValue("@Pstarttime", dt.Pstarttime);
            //    da.SelectCommand.Parameters.AddWithValue("@Povertime", dt.Povertime);
            //    da.SelectCommand.Parameters.AddWithValue("@id", dt.Pid);

            //}
        }
    }
}