using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Project_System
{
    public class Insert
    {
        internal static DATASET.DataSet.T_PdbRow GetMaxPidRow(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select MAX(Pid) as Pid from T_Pdb";
            DATASET.DataSet.T_PdbDataTable dt = new DATASET.DataSet.T_PdbDataTable();
            da.Fill(dt);
            return dt[0];
        }

        internal static void InsertProject(DATASET.DataSet.T_PdbDataTable dt, SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "SELECT * FROM T_Pdb";
            da.InsertCommand = (new SqlCommandBuilder(da)).GetInsertCommand();

            SqlTransaction sql;

            try
            {
                sqlConnection.Open();
                sql = sqlConnection.BeginTransaction();

                da.SelectCommand.Transaction = da.InsertCommand.Transaction = sql;

                da.Update(dt);

                sql.Commit();
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        internal static DATASET.DataSet.M_UserDataTable GetM_User(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select * from M_User";
            DATASET.DataSet.M_UserDataTable dt = new DATASET.DataSet.M_UserDataTable();
            da.Fill(dt);
            return dt;
        }
    }
}