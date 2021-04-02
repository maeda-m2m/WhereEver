using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WhereEver
{
    public class Global : HttpApplication
    {
        public static string LoginPageURL
        {
            get
            {
                return System.Web.HttpContext.Current.Request.ApplicationPath + "/Login.aspx";
            }
        }
        void Application_Start(object sender, EventArgs e)
        {
            // アプリケーションのスタートアップで実行するコードです
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        internal static SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);
        }
    }
}