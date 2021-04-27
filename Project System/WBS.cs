using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Project_System
{
    public class WBS
    {
        internal static DATASET.DataSet.T_PdbKanriRow GetPMiddleTimeRow(SqlConnection sqlConnection)
        {
            SqlDataAdapter da = new SqlDataAdapter("", sqlConnection);
            da.SelectCommand.CommandText =
                "select MIN(PMiddlestart) as PMiddlestart , MAX(PMiddleover) as PMiddleover from T_PdbKanri";
            DATASET.DataSet.T_PdbKanriDataTable dt = new DATASET.DataSet.T_PdbKanriDataTable();
            da.Fill(dt);
            return dt[0];
        }
    }
}