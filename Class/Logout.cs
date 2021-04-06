﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WhereEver.Class
{
    public class Logout
    {
        public static void InsertLogoutList(SqlConnection sql)
        {
            string GetUserName = SessionManager.User.M_User.name;
            DateTime dtime = DateTime.Now;
            DATASET.DataSet.T_LogoutListDataTable dt = new DATASET.DataSet.T_LogoutListDataTable();

            dt[0][0] = GetUserName;
            dt[0][1] = dtime;

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