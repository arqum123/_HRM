using HRM.Core;
using HRM.Core.IService;
using HRM.Core.Model;
using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Core.Helper;

namespace HRM.WebAPI.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies["hrmus_id"] != null)
                CookieStore.SetCookie("hrmus_id", null, TimeSpan.FromDays(-1));
            if (Request.Cookies["hrmus_nm"] != null)
                CookieStore.SetCookie("hrmus_nm", null, TimeSpan.FromDays(-1));
            if (Request.Cookies["hrmus_im"] != null)
                CookieStore.SetCookie("hrmus_im", null, TimeSpan.FromDays(-1));
            if (Request.Cookies["hrmus_ut"] != null)
                CookieStore.SetCookie("hrmus_ut", null, TimeSpan.FromDays(-1));
            if (Request.Cookies["hrmus_fn"] != null)
                CookieStore.SetCookie("hrmus_fn", null, TimeSpan.FromDays(-1));
            if (Request.Cookies["hrmus_lg"] != null)
                CookieStore.SetCookie("hrmus_lg", null, TimeSpan.FromDays(-1));
            if (Request.Cookies["hrmus_dn"] != null)
                CookieStore.SetCookie("hrmus_dn", null, TimeSpan.FromDays(-1));

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model, string returnUrl, string key = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                    User _User = objUserService.GetUserWithDepartment(model.LoginID);

                    //if (_User != null && HashData.VerifyHash(model.Password, "MD5", _User.Password))
                    if (_User != null && model.Password == _User.Password)
                    {
                        AuthBase.UserId = _User.Id;
                        AuthBase.UserName = _User.FirstName;
                        AuthBase.UserImage = _User.ImagePath == "" ? "/assets/images/anonymoususer.jpg" : _User.ImagePath;
                        AuthBase.UserTypeId = (int)_User.UserTypeId;
                        AuthBase.UserFullName = _User.FirstName + " " + _User.MiddleName + " " + _User.LastName;
                        AuthBase.UserLoginID = _User.LoginId;

                        AuthBase.UserDepartmentName = _User.Department != null ? _User.Department.Name : "";

                        if (!model.RememberMe)
                        {
                            CookieStore.SetCookie("hrmus_id", _User.Id.ToString(), TimeSpan.FromDays(1));
                            CookieStore.SetCookie("hrmus_nm", _User.FirstName, TimeSpan.FromDays(1));
                            CookieStore.SetCookie("hrmus_im", _User.ImagePath, TimeSpan.FromDays(1));
                            CookieStore.SetCookie("hrmus_ut", _User.UserTypeId.ToString(), TimeSpan.FromDays(1));
                            CookieStore.SetCookie("hrmus_fn", _User.FirstName + " " + _User.MiddleName + " " + _User.LastName, TimeSpan.FromDays(1));
                            CookieStore.SetCookie("hrmus_lg", _User.LoginId, TimeSpan.FromDays(1));
                            CookieStore.SetCookie("hrmus_dn", AuthBase.UserDepartmentName, TimeSpan.FromDays(1));
                        }
                        else
                        {
                            CookieStore.SetCookie("hrmus_id", _User.Id.ToString(), TimeSpan.FromDays(30));
                            CookieStore.SetCookie("hrmus_nm", _User.FirstName, TimeSpan.FromDays(30));
                            CookieStore.SetCookie("hrmus_im", _User.ImagePath, TimeSpan.FromDays(30));
                            CookieStore.SetCookie("hrmus_ut", _User.UserTypeId.ToString(), TimeSpan.FromDays(30));
                            CookieStore.SetCookie("hrmus_fn", _User.FirstName + " " + _User.MiddleName + " " + _User.LastName, TimeSpan.FromDays(30));
                            CookieStore.SetCookie("hrmus_lg", _User.LoginId, TimeSpan.FromDays(30));
                            CookieStore.SetCookie("hrmus_dn", AuthBase.UserDepartmentName, TimeSpan.FromDays(30));
                        }

                        if (!string.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);
                        if (key == null)
                            return RedirectToAction("index", "home");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The Login ID or Password is incorrect");
            return View(model);
        }

    }
}
