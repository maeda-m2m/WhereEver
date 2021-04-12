using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhereEver
{
    public class SessionManager
    {
        private const string SESSION_USER_ID = "SESSION_USER_ID";
        private const string SESSION_USER_PW = "SESSION_USER_PW";
        private const string SESSION_USER_KUBUN = "SESSION_USER_KUBUN";

        public const string SESSION_USER = "SESSION_LOGIN_USER";

        internal static void Logout()
        {
            HttpContext.Current.Session.Abandon();
            System.Web.Security.FormsAuthentication.SignOut();
        }

        internal static void Login(DATASET.DataSet.M_UserRow dr)
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(dr.id.ToString(), false);
            System.Web.Security.FormsAuthentication.SetAuthCookie(dr.pw.ToString(), false);
            System.Web.HttpContext.Current.Session[SESSION_USER_KUBUN] = dr.name1;

            System.Web.HttpContext.Current.Session[SESSION_USER_ID] = dr.id;
            System.Web.HttpContext.Current.Session[SESSION_USER_PW] = dr.pw;

            LoginUser u = LoginUser.New(dr.id.ToString(), dr.pw);
            System.Web.HttpContext.Current.Session[SESSION_USER] = u;
        }

        public class LoginUser
        {
            private DATASET.DataSet.M_UserRow _drUser = null;

            internal static LoginUser New(string ID, string PW)
            {
                LoginUser u = new LoginUser
                {
                    _drUser = Class.Login.GetM_UserRow(ID, PW,Global.GetConnection())
                };
                if (null == u._drUser) return null;

                //u.TwoLetterISOLanguageName = v;

                return u;
            }

            public DATASET.DataSet.M_UserRow M_User
            {
                get { return this._drUser; }
                set { this._drUser = value; }
            }

            public string ID
            {
                get
                {
                    return _drUser.id.ToString();
                }
            }


        }

        public static LoginUser User
        {
            get
            {
                try
                {
                    object obj = System.Web.HttpContext.Current.Session[SESSION_USER];
                    if (null == obj) throw new Exception("SessionOut");
                    return obj as LoginUser;
                }
                catch
                {
                    System.Web.HttpContext.Current.Response.Redirect(Global.LoginPageURL, true);
                    return null;
                }
            }
        }

        private const string SESSION_PROJECT_NAME = "SESSION_PROJECT_NAME";
        private const string SESSION_PROJECT_CUSTOMER = "SESSION_PROJECT_CUSTOMER";
        private const string SESSION_PROJECT_RESPONSIBLE = "SESSION_PROJECT_RESPONSIBLE";
        private const string SESSION_PROJECT_CATEGORY = "SESSION_PROJECT_CATEGORY";
        private const string SESSION_PROJECT_STARTTIME = "SESSION_PROJECT_STARTTIME";
        private const string SESSION_PROJECT_OVERTIME = "SESSION_PROJECT_OVERTIME";

        internal static void Project(DATASET.DataSet.T_PdbRow dr)
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(dr.Pname.ToString(), false);
            System.Web.Security.FormsAuthentication.SetAuthCookie(dr.Pcustomer.ToString(), false);
            System.Web.Security.FormsAuthentication.SetAuthCookie(dr.Presponsible.ToString(), false);
            System.Web.Security.FormsAuthentication.SetAuthCookie(dr.Pcategory.ToString(), false);
            System.Web.Security.FormsAuthentication.SetAuthCookie(dr.Pstarttime.ToString(), false);
            System.Web.Security.FormsAuthentication.SetAuthCookie(dr.Povertime.ToString(), false);

            System.Web.HttpContext.Current.Session[SESSION_PROJECT_NAME] = dr.Pname;
            System.Web.HttpContext.Current.Session[SESSION_PROJECT_CUSTOMER] = dr.Pcustomer;
            System.Web.HttpContext.Current.Session[SESSION_PROJECT_RESPONSIBLE] = dr.Presponsible;
            System.Web.HttpContext.Current.Session[SESSION_PROJECT_CATEGORY] = dr.Pcategory;
            System.Web.HttpContext.Current.Session[SESSION_PROJECT_STARTTIME] = dr.Pstarttime;
            System.Web.HttpContext.Current.Session[SESSION_PROJECT_OVERTIME] = dr.Povertime;

        }
    }
}

