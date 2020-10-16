
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;



namespace HRM.Core.Helper
{
    public class CookieStore
    {
        public static void SetCookie(string key, string value, TimeSpan expires)
        {
            //string securedValue = HttpSecureCookie.Protect(value);
            string securedValue = new EncryptedQueryString().Encrypt(value);
            HttpCookie encodedCookie = new HttpCookie(key, securedValue);

            //HttpCookie encodedCookie = HttpSecureCookie.Encode(new HttpCookie(key, value));

            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies[key];
                cookieOld.Expires = DateTime.Now.Add(expires);
                cookieOld.Value = encodedCookie.Value;
                HttpContext.Current.Response.Cookies.Add(cookieOld);
            }
            else
            {
                encodedCookie.Expires = DateTime.Now.Add(expires);
                HttpContext.Current.Response.Cookies.Add(encodedCookie);
            }
        }
        
        public static string GetCookie(string key)
        {
            string value = string.Empty;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie != null)
            {
                //value = HttpSecureCookie.Unprotect(cookie.Value);
                value = new EncryptedQueryString().Decrypt(cookie.Value);

                // For security purpose, we need to encrypt the value.
                //HttpCookie decodedCookie = HttpSecureCookie.Decode(cookie);
                //value = decodedCookie.Value;
            }
            return value;
        }
    }
}