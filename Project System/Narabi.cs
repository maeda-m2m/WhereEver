using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Project_System
{
    public class Narabi
    {
        internal static void uebig(SqlConnection sqlConnection, string name, int Pid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select PBigid from T_PdbKanri where PBigname = @name and Pid like @Pid";
            da.SelectCommand.Parameters.AddWithValue("@name", name);
            da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);

            da.SelectCommand.CommandText =
                "select Distinct PBigid,PBigname from T_PdbKanri where (PBigid = @PBigid or PBigid = @PBigid -1) and Pid = @Pid1";
            da.SelectCommand.Parameters.AddWithValue("@PBigid", dt[0].PBigid);
            da.SelectCommand.Parameters.AddWithValue("@Pid1", Pid);
            DATASET.DataSet.T_PdbKanriDataTable dt1 = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt1);
            for (int i = 0; i< dt1.Count; i++)
            {
                if (dt1[i].PBigname != name)
                {
                    dt1[i].PBigid++;
                }
                else
                {
                    dt1[i].PBigid--;
                }
            }
            uebigUpdate(dt1,Pid);
        }
        internal static void uebigUpdate(DATASET.DataSet.T_PdbKanriDataTable dt, int Pid)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "update T_PdbKanri set " +
                    "PBigid = @PBigid " +
                    "where Pid = @Pid and PBigname = @PBigname";

                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                SqlDataAdapter da1 = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@PBigname", dt[0].PBigname);
                da.SelectCommand.Parameters.AddWithValue("@PBigid", dt[0].PBigid);
                da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();

                sql = "update T_PdbKanri set " +
                    "PBigid = @PBigid " +
                    "where Pid = @Pid and PBigname = @PBigname";


                da1.SelectCommand.Parameters.AddWithValue("@PBigname", dt[1].PBigname);
                da1.SelectCommand.Parameters.AddWithValue("@PBigid", dt[1].PBigid);
                da1.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
                connection.Open();
                int cnt2 = da1.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}