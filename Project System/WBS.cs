using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Project_System
{
    public class WBS
    {
        internal static DATASET.DataSet.T_PdbKanriRow GetPMiddleTimeRow(SqlConnection sqlConnection,int spId)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select MIN(PMiddlestart) as PMiddlestart , MAX(PMiddleover) as PMiddleover from T_PdbKanri where Pid = @spId";
            da.SelectCommand.Parameters.AddWithValue("@spId", spId);
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);
            return dt[0];
        }

        internal static int GetT_PdbKanriDataTable(SqlConnection sqlConnection, int spId)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select count(*) as count from T_PdbKanri where Pid = @spId";
            da.SelectCommand.Parameters.AddWithValue("@spId", spId);
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);
            return (int)dt[0][10];
        }
    }
}