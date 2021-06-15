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
                dt1[i].PTorokusya= SessionManager.User.M_User.id.Trim();
                dt1[i].PTorokutime= DateTime.Now;
            }
            bigUpdate(dt1,Pid);
        }
        internal static void bigUpdate(DATASET.DataSet.T_PdbKanriDataTable dt, int Pid)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "update T_PdbKanri set " +
                    "PBigid = @PBigid ," +
                    "PTorokutime = @PTorokutime ," +
                    "PTorokusya = @PTorokusya " +
                    "where Pid = @Pid and PBigname = @PBigname";

                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                SqlDataAdapter da1 = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@PTorokutime", dt[0].PTorokutime);
                da.SelectCommand.Parameters.AddWithValue("@PTorokusya", dt[0].PTorokusya);
                da.SelectCommand.Parameters.AddWithValue("@PBigname", dt[0].PBigname);
                da.SelectCommand.Parameters.AddWithValue("@PBigid", dt[0].PBigid);
                da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();

                sql = "update T_PdbKanri set " +
                    "PBigid = @PBigid ," +
                    "PTorokutime = @PTorokutime ," +
                    "PTorokusya = @PTorokusya " +
                    "where Pid = @Pid and PBigname = @PBigname";

                da1.SelectCommand.Parameters.AddWithValue("@PTorokutime", dt[1].PTorokutime);
                da1.SelectCommand.Parameters.AddWithValue("@PTorokusya", dt[1].PTorokusya);
                da1.SelectCommand.Parameters.AddWithValue("@PBigname", dt[1].PBigname);
                da1.SelectCommand.Parameters.AddWithValue("@PBigid", dt[1].PBigid);
                da1.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
                connection.Open();
                int cnt2 = da1.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        internal static void sitabig(SqlConnection sqlConnection, string name, int Pid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select PBigid from T_PdbKanri where PBigname = @name and Pid like @Pid";
            da.SelectCommand.Parameters.AddWithValue("@name", name);
            da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);

            da.SelectCommand.CommandText =
                "select Distinct PBigid,PBigname from T_PdbKanri where (PBigid = @PBigid or PBigid = @PBigid +1) and Pid = @Pid1";
            da.SelectCommand.Parameters.AddWithValue("@PBigid", dt[0].PBigid);
            da.SelectCommand.Parameters.AddWithValue("@Pid1", Pid);
            DATASET.DataSet.T_PdbKanriDataTable dt1 = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt1);
            for (int i = 0; i < dt1.Count; i++)
            {
                if (dt1[i].PBigname != name)
                {
                    dt1[i].PBigid--;
                }
                else
                {
                    dt1[i].PBigid++;
                }
                dt1[i].PTorokusya = SessionManager.User.M_User.id.Trim();
                dt1[i].PTorokutime = DateTime.Now;
            }
            bigUpdate(dt1, Pid);
        }

        internal static void uemiddle(SqlConnection sqlConnection, string Middleid,string bigname, int Pid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            da.SelectCommand.CommandText =
                "select PMiddleid,PMiddlename from T_PdbKanri where (PMiddleid = @Middleid or PMiddleid = @Middleid -1) and Pbigname = @bigname and Pid = @Pid";
            da.SelectCommand.Parameters.AddWithValue("@Middleid", Middleid);
            da.SelectCommand.Parameters.AddWithValue("@bigname", bigname);
            da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
            DATASET.DataSet.T_PdbKanriDataTable dt1 = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt1);
            for (int i = 0; i < dt1.Count; i++)
            {
                if (dt1[i].PMiddleid.ToString() != Middleid)
                {
                    dt1[i].PMiddleid++;
                }
                else
                {
                    dt1[i].PMiddleid--;
                }
                dt1[i].PTorokusya = SessionManager.User.M_User.id.Trim();
                dt1[i].PTorokutime = DateTime.Now;
            }
            middleUpdate(dt1, Pid);
        }
        internal static void sitamiddle(SqlConnection sqlConnection, string Middleid, string bigname, int Pid)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);

            da.SelectCommand.CommandText =
                "select PMiddleid,PMiddlename from T_PdbKanri where (PMiddleid = @Middleid or PMiddleid = @Middleid +1) and Pbigname = @bigname and Pid = @Pid";
            da.SelectCommand.Parameters.AddWithValue("@Middleid", Middleid);
            da.SelectCommand.Parameters.AddWithValue("@bigname", bigname);
            da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
            DATASET.DataSet.T_PdbKanriDataTable dt1 = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt1);
            for (int i = 0; i < dt1.Count; i++)
            {
                if (dt1[i].PMiddleid.ToString() != Middleid)
                {
                    dt1[i].PMiddleid--;
                }
                else
                {
                    dt1[i].PMiddleid++;
                }
                dt1[i].PTorokusya = SessionManager.User.M_User.id.Trim();
                dt1[i].PTorokutime = DateTime.Now;
            }
            middleUpdate(dt1, Pid);
        }

        internal static void middleUpdate(DATASET.DataSet.T_PdbKanriDataTable dt, int Pid)
        {
            string cstr = System.Configuration.ConfigurationManager.ConnectionStrings["WhereverConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(cstr))
            {
                string sql = "update T_PdbKanri set " +
                    "PMiddleid = @PMiddleid ," +
                    "PTorokutime = @PTorokutime ," +
                    "PTorokusya = @PTorokusya " +
                    "where Pid = @Pid and PMiddlename = @PMiddlename";

                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                SqlDataAdapter da1 = new SqlDataAdapter(sql, connection);

                da.SelectCommand.Parameters.AddWithValue("@PTorokutime", dt[0].PTorokutime);
                da.SelectCommand.Parameters.AddWithValue("@PTorokusya", dt[0].PTorokusya);
                da.SelectCommand.Parameters.AddWithValue("@PMiddlename", dt[0].PMiddlename);
                da.SelectCommand.Parameters.AddWithValue("@PMiddleid", dt[0].PMiddleid);
                da.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
                connection.Open();
                int cnt = da.SelectCommand.ExecuteNonQuery();
                connection.Close();

                sql = "update T_PdbKanri set " +
                    "PMiddleid = @PMiddleid ," +
                    "PTorokutime = @PTorokutime ," +
                    "PTorokusya = @PTorokusya " +
                    "where Pid = @Pid and PMiddlename = @PMiddlename";

                da1.SelectCommand.Parameters.AddWithValue("@PTorokutime", dt[1].PTorokutime);
                da1.SelectCommand.Parameters.AddWithValue("@PTorokusya", dt[1].PTorokusya);
                da1.SelectCommand.Parameters.AddWithValue("@PMiddlename", dt[1].PMiddlename);
                da1.SelectCommand.Parameters.AddWithValue("@PMiddleid", dt[1].PMiddleid);
                da1.SelectCommand.Parameters.AddWithValue("@Pid", Pid);
                connection.Open();
                int cnt2 = da1.SelectCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}