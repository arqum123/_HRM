using System;
using System.Web;

namespace HRM.Core.Helper
{
    public class AuthBase
    {
        #region User
        public static int UserId
        {
            get
            {
                if (HttpContext.Current.Session["userId"] != null && HttpContext.Current.Session["userId"].ToString().Length > 0)
                    return Convert.ToInt32(HttpContext.Current.Session["userId"]);
                else if (HttpContext.Current.Request.Cookies["hrmus_id"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["hrmus_id"].Value))
                {
                    HttpContext.Current.Session["userId"] = CookieStore.GetCookie("hrmus_id");
                    return Convert.ToInt32(HttpContext.Current.Session["userId"]);
                }
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["userId"] = value;
            }
        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["userName"] != null && HttpContext.Current.Session["userName"].ToString().Length > 0)
                    return HttpContext.Current.Session["userName"].ToString();
                else if (HttpContext.Current.Request.Cookies["hrmus_nm"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["hrmus_nm"].Value))
                {
                    HttpContext.Current.Session["userName"] = CookieStore.GetCookie("hrmus_nm");
                    return HttpContext.Current.Session["userName"].ToString();
                }
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["userName"] = value;
            }
        }

        public static string UserImage
        {
            get
            {
                if (HttpContext.Current.Session["userImage"] != null && HttpContext.Current.Session["userImage"].ToString().Length > 0)
                    return HttpContext.Current.Session["userImage"].ToString();
                else if (HttpContext.Current.Request.Cookies["hrmus_im"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["hrmus_im"].Value))
                {
                    HttpContext.Current.Session["userImage"] = CookieStore.GetCookie("hrmus_im");
                    return HttpContext.Current.Session["userImage"].ToString();
                }
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["userImage"] = value;
            }
        }

        public static string UserFullName
        {
            get
            {
                if (HttpContext.Current.Session["UserFullName"] != null && HttpContext.Current.Session["UserFullName"].ToString().Length > 0)
                    return HttpContext.Current.Session["UserFullName"].ToString();
                else if (HttpContext.Current.Request.Cookies["hrmus_fn"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["hrmus_fn"].Value))
                {
                    HttpContext.Current.Session["UserFullName"] = CookieStore.GetCookie("hrmus_fn");
                    return HttpContext.Current.Session["UserFullName"].ToString();
                }
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["UserFullName"] = value;
            }
        }

        public static string UserLoginID
        {
            get
            {
                if (HttpContext.Current.Session["loginID"] != null && HttpContext.Current.Session["loginID"].ToString().Length > 0)
                    return HttpContext.Current.Session["loginID"].ToString();
                else if (HttpContext.Current.Request.Cookies["hrmus_lg"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["hrmus_lg"].Value))
                {
                    HttpContext.Current.Session["loginID"] = CookieStore.GetCookie("hrmus_lg");
                    return HttpContext.Current.Session["loginID"].ToString();
                }
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["loginID"] = value;
            }
        }

        public static int UserTypeId
        {
            get
            {
                if (HttpContext.Current.Session["userTypeId"] != null && HttpContext.Current.Session["userTypeId"].ToString().Length > 0)
                    return Convert.ToInt32(HttpContext.Current.Session["userTypeId"]);
                else if (HttpContext.Current.Request.Cookies["hrmus_ut"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["hrmus_ut"].Value))
                {
                    HttpContext.Current.Session["userTypeId"] = CookieStore.GetCookie("hrmus_ut");
                    return Convert.ToInt32(HttpContext.Current.Session["userTypeId"]);
                }
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["userTypeId"] = value;
            }
        }

        public static string UserDepartmentName
        {
            get
            {
                if (HttpContext.Current.Session["userDepartmentName"] != null && HttpContext.Current.Session["userDepartmentName"].ToString().Length > 0)
                    return HttpContext.Current.Session["userDepartmentName"].ToString();
                else if (HttpContext.Current.Request.Cookies["hrmus_dn"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["hrmus_dn"].Value))
                {
                    HttpContext.Current.Session["userDepartmentName"] = CookieStore.GetCookie("hrmus_dn");
                    return HttpContext.Current.Session["userDepartmentName"].ToString();
                }
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["userDepartmentName"] = value;
            }
        }

        public static int BranchId
        {
            get
            {
                if (HttpContext.Current.Session["branchId"] != null && HttpContext.Current.Session["branchId"].ToString().Length > 0)
                    return Convert.ToInt32(HttpContext.Current.Session["branchId"]);
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["branchId"] = value;
            }
        }

       
        #endregion User
    }
}