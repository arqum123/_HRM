using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;
using HRM.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.WebAPI.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [Authenticate]
        public ActionResult Index(string Id)
        {
            VMUserModel model = new VMUserModel();
            UserPrerequisiteData();
            if (Id != null && Id != "")
            {
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
                IUserContactService objUserContactService = IoC.Resolve<IUserContactService>("UserContactService");
                model.User = objUserService.GetUser(int.Parse(Id));
                model.UserDepartment = objUserDepartmentService.GetUserDepartmentByUserId(int.Parse(Id)).Where(x => x.RetiredDate == null).FirstOrDefault();

                List<UserContact> _userContactList = objUserContactService.GetUserContactByUserId(int.Parse(Id));
                if (_userContactList != null && _userContactList.Count > 0)
                {
                    if (_userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.EmailAddress) != null)
                        model.UserContactEmail = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.EmailAddress).FirstOrDefault();

                    if (_userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.MobileNumber) != null)
                        model.UserContactMobile = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.MobileNumber).FirstOrDefault();

                    if (_userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.AlternateMobileNumber) != null)
                        model.UserContactAlternateMobile = _userContactList.Where(x => x.IsActive == true && x.ContactTypeId == (int)HRM.Core.Enum.ContactType.AlternateMobileNumber).FirstOrDefault();
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Index(VMUserModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.User.Id != 0)
                    {
                        #region User Updation
                        IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                        if (file != null && file.ContentLength > 0)
                        {
                            string DirectoryName = "/Uploads/ProfileImages";
                            Directory.CreateDirectory(Server.MapPath("~" + DirectoryName));
                            var fileName = model.User.Id.ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                            var path = Path.Combine(Server.MapPath("~" + DirectoryName), fileName);
                            file.SaveAs(path);

                            model.User.ImagePath = Path.Combine(DirectoryName, fileName);
                        }

                        model.User.UpdateDate = DateTime.Now;
                        model.User.UpdateBy = AuthBase.UserId;
                        model.User.UserIp = Request.UserHostAddress;

                        User _user = objUserService.UpdateUser(model.User);

                        #endregion

                        #region User Contact Update
                        IUserContactService objUserContactService = IoC.Resolve<IUserContactService>("UserContactService");

                        UserContact _userContactEmail = objUserContactService.GetUserContact(model.UserContactEmail.Id);
                        UserContact _userContactMobile = objUserContactService.GetUserContact(model.UserContactMobile.Id);
                        UserContact _userContactAlternateMobile = objUserContactService.GetUserContact(model.UserContactAlternateMobile.Id);

                        _userContactEmail.Detail = model.UserContactEmail.Detail;
                        _userContactEmail.UpdateDate = DateTime.Now;
                        _userContactEmail.UpdateBy = AuthBase.UserId;
                        _userContactEmail.UserIp = Request.UserHostAddress;
                        objUserContactService.UpdateUserContact(_userContactEmail);

                        _userContactMobile.Detail = model.UserContactMobile.Detail;
                        _userContactMobile.UpdateDate = DateTime.Now;
                        _userContactMobile.UpdateBy = AuthBase.UserId;
                        _userContactMobile.UserIp = Request.UserHostAddress;
                        objUserContactService.UpdateUserContact(_userContactMobile);

                        _userContactAlternateMobile.Detail = model.UserContactAlternateMobile.Detail;
                        _userContactAlternateMobile.UpdateDate = DateTime.Now;
                        _userContactAlternateMobile.UpdateBy = AuthBase.UserId;
                        _userContactAlternateMobile.UserIp = Request.UserHostAddress;
                        objUserContactService.UpdateUserContact(_userContactAlternateMobile);

                        #endregion

                        #region User Dept Update and Entry
                        IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
                        UserDepartment _userDepartment = objUserDepartmentService.GetUserDepartment(model.UserDepartment.Id);
                        if (_userDepartment.DepartmentId != model.UserDepartment.DepartmentId)
                        {
                            _userDepartment.RetiredDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(-1)) + " 23:59:59");
                            _userDepartment.UpdateDate = DateTime.Now;
                            _userDepartment.UpdateBy = AuthBase.UserId;
                            _userDepartment.UserIp = Request.UserHostAddress;

                            objUserDepartmentService.UpdateUserDepartment(_userDepartment);

                            model.UserDepartment.Id = 0;
                            model.UserDepartment.UserId = _user.Id;
                            model.UserDepartment.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                            model.UserDepartment.CreationDate = DateTime.Now;
                            model.UserDepartment.UpdateBy = AuthBase.UserId;
                            model.UserDepartment.UserIp = Request.UserHostAddress;

                            objUserDepartmentService.InsertUserDepartment(model.UserDepartment);
                        }

                        #endregion
                    }
                    else
                    {
                        #region User Entry
                        IUserService objUserService = IoC.Resolve<IUserService>("UserService");

                        model.User.IsActive = true;
                        model.User.CreationDate = DateTime.Now;
                        model.User.UpdateBy = AuthBase.UserId;
                        model.User.UserIp = Request.UserHostAddress;

                        User _user = objUserService.InsertUser(model.User);

                        if (file != null && file.ContentLength > 0)
                        {
                            string DirectoryName = "/Uploads/ProfileImages";
                            Directory.CreateDirectory(Server.MapPath("~" + DirectoryName));
                            var fileName = _user.Id.ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                            var path = Path.Combine(Server.MapPath("~" + DirectoryName), fileName);
                            file.SaveAs(path);

                            Dictionary<string, string> dictionary = new Dictionary<string, string>();
                            dictionary.Add("ImagePath", Path.Combine(DirectoryName, fileName));
                            _user = objUserService.UpdateUserByKeyValue(dictionary, _user.Id);
                        }

                        #endregion

                        #region User Contact Entry
                        IUserContactService objUserContactService = IoC.Resolve<IUserContactService>("UserContactService");

                        model.UserContactEmail.UserId = _user.Id;
                        model.UserContactEmail.ContactTypeId = (int)HRM.Core.Enum.ContactType.EmailAddress;
                        model.UserContactEmail.IsActive = true;
                        model.UserContactEmail.CreationDate = DateTime.Now;
                        model.UserContactEmail.UpdateBy = AuthBase.UserId;
                        model.UserContactEmail.UserIp = Request.UserHostAddress;

                        model.UserContactMobile.UserId = _user.Id;
                        model.UserContactMobile.ContactTypeId = (int)HRM.Core.Enum.ContactType.MobileNumber;
                        model.UserContactMobile.IsActive = true;
                        model.UserContactMobile.CreationDate = DateTime.Now;
                        model.UserContactMobile.UpdateBy = AuthBase.UserId;
                        model.UserContactMobile.UserIp = Request.UserHostAddress;

                        model.UserContactAlternateMobile.UserId = _user.Id;
                        model.UserContactAlternateMobile.ContactTypeId = (int)HRM.Core.Enum.ContactType.AlternateMobileNumber;
                        model.UserContactAlternateMobile.IsActive = true;
                        model.UserContactAlternateMobile.CreationDate = DateTime.Now;
                        model.UserContactAlternateMobile.UpdateBy = AuthBase.UserId;
                        model.UserContactAlternateMobile.UserIp = Request.UserHostAddress;

                        objUserContactService.InsertUserContact(model.UserContactEmail);
                        objUserContactService.InsertUserContact(model.UserContactMobile);
                        objUserContactService.InsertUserContact(model.UserContactAlternateMobile);

                        #endregion

                        #region User Dept Entry
                        IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");

                        model.UserDepartment.UserId = _user.Id;
                        model.UserDepartment.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                        model.UserDepartment.CreationDate = DateTime.Now;
                        model.UserDepartment.UpdateBy = AuthBase.UserId;
                        model.UserDepartment.UserIp = Request.UserHostAddress;

                        objUserDepartmentService.InsertUserDepartment(model.UserDepartment);
                        #endregion
                    }
                    return RedirectToAction("UserList", "User");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ModelState.AddModelError("", "Unable to process your request. Please contact Administration");

            UserPrerequisiteData();
            return View();
        }

        [HttpGet]
        [Authenticate]
        public ActionResult UserList()
        {
            VMUserModel VMUserModel = new VMUserModel();
            UserListPrerequisiteData();
            return View(VMUserModel);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult UserList(VMUserModel model)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

            VMUserModel VMUserModel = new VMUserModel();
            VMUserModel.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId);

            VMUserModel VMUserModelDepartment = new VMUserModel();
            VMUserModelDepartment.UserList = VMUserModel.UserList;
            if (VMUserModelDepartment.UserList != null && model.DepartmentID != null && VMUserModel.UserList.Where(x => x.UserDepartment.DepartmentId == model.DepartmentID) != null)
                VMUserModelDepartment.UserList = VMUserModel.UserList.Where(x => x.UserDepartment.DepartmentId == model.DepartmentID).ToList();
            if (VMUserModelDepartment.UserList != null && model.UserName != null && model.UserName != "")
                VMUserModelDepartment.UserList = VMUserModelDepartment.UserList.Where(x => x.FirstName.ToLower().Contains(model.UserName.ToLower())).ToList();

            UserListPrerequisiteData();
            if (VMUserModelDepartment.UserList != null && VMUserModelDepartment.UserList.Count > 0)
                return View(VMUserModelDepartment);
            else
            {
                ModelState.AddModelError("", "No Records Found");
                return View(VMUserModelDepartment);
            }
        }

        [HttpPost]
        [Authenticate]
        public JsonResult SearchUserList(string ShiftId, string DepartmentId, string UserName, int recCount)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

            VMUserModel VMUserModel = new VMUserModel();
            VMUserModel.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId);

            VMUserModel VMUserModelDepartment = new VMUserModel();
            VMUserModelDepartment.UserList = VMUserModel.UserList;
            if (ShiftId != "" && VMUserModelDepartment.UserList.Where(x => x.Shift != null && x.Shift.Id == int.Parse(ShiftId)) != null)
                VMUserModelDepartment.UserList = VMUserModelDepartment.UserList.Where(x => x.Shift != null && x.Shift.Id == int.Parse(ShiftId)).ToList();
            if (DepartmentId != "" && VMUserModelDepartment.UserList.Where(x => x.UserDepartment.DepartmentId == int.Parse(DepartmentId)) != null)
                VMUserModelDepartment.UserList = VMUserModelDepartment.UserList.Where(x => x.UserDepartment.DepartmentId == int.Parse(DepartmentId)).ToList();
            VMUserModelDepartment.UserList = VMUserModelDepartment.UserList.Where(x => x.FirstName.ToLower().Contains(UserName.ToLower())).Take(recCount).ToList();

            return Json(VMUserModelDepartment.UserList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            try
            {
                Dictionary<string, string> dictUser = new Dictionary<string, string>();
                dictUser.Add("IsActive", "false");
                dictUser.Add("UpdateDate", DateTime.Now.ToString());
                dictUser.Add("UpdateBy", AuthBase.UserId.ToString());
                dictUser.Add("UserIP", Request.UserHostAddress);

                IUserService ObjUserService = IoC.Resolve<IUserService>("UserService");
                ObjUserService.UpdateUserByKeyValue(dictUser, id);
                return base.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return base.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [Authenticate]
        public ActionResult PrintUserList(int? departmentId, string userName="", int branchId=0)
        {
            if (branchId != 0)
                ViewBag.BranchName = new SystemConfiguration().GetBranchInfo(branchId).Name;
            else
                ViewBag.BranchName = "All Branches";
            VMUserModel model = new VMUserModel() { DepartmentID = departmentId, UserName = userName };
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

            VMUserModel VMUserModel = new VMUserModel();
            VMUserModel.UserList = objUserService.GetAllUserWithDepartment(branchId);

            VMUserModel VMUserModelDepartment = new VMUserModel();
            VMUserModelDepartment.UserList = VMUserModel.UserList;
            if (VMUserModelDepartment.UserList != null && model.DepartmentID != null && VMUserModel.UserList.Where(x => x.UserDepartment.DepartmentId == model.DepartmentID) != null)
                VMUserModelDepartment.UserList = VMUserModel.UserList.Where(x => x.UserDepartment.DepartmentId == model.DepartmentID).ToList();
            if (VMUserModelDepartment.UserList != null && model.UserName != null && model.UserName != "")
                VMUserModelDepartment.UserList = VMUserModelDepartment.UserList.Where(x => x.FirstName.ToLower().Contains(model.UserName.ToLower())).ToList();

            UserListPrerequisiteData();
            if (VMUserModelDepartment.UserList != null && VMUserModelDepartment.UserList.Count > 0)
                return View(VMUserModelDepartment);
            else
            {
                ModelState.AddModelError("", "No Records Found");
                return View(VMUserModelDepartment);
            }
        }

        private void UserPrerequisiteData()
        {
            IUserTypeService objUserTypeService = IoC.Resolve<IUserTypeService>("UserTypeService");
            ViewBag.UserType = new SelectList(objUserTypeService.GetAllUserType(), "Id", "Name");

            ICountryService objCountryService = IoC.Resolve<ICountryService>("CountryService");
            ViewBag.Country = new SelectList(objCountryService.GetAllCountry(), "Id", "Name");

            IReligionService objReligionService = IoC.Resolve<IReligionService>("ReligionService");
            ViewBag.Religion = new SelectList(objReligionService.GetAllReligion(), "Id", "Name");

            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            List<Department> _deptList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);
            if (_deptList != null && _deptList.Count > 0)
                ViewBag.Department = new SelectList(_deptList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Department", Value = "-1" });
                ViewBag.Department = new SelectList(blank, "Value", "Text", "");
            }

            ISalaryTypeService objSalaryTypeService = IoC.Resolve<ISalaryTypeService>("SalaryTypeService");
            ViewBag.SalaryType = new SelectList(objSalaryTypeService.GetAllSalaryType(), "Id", "Name");
        }

        private void UserListPrerequisiteData()
        {
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            List<Department> _deptList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);
            if (_deptList != null && _deptList.Count > 0)
                ViewBag.Department = new SelectList(_deptList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Department", Value = "-1" });
                ViewBag.Department = new SelectList(blank, "Value", "Text", "");
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult doesLoginIDExist(string LoginID)
        //{
        //    IUserService ObjUserService = IoC.Resolve<IUserService>("IUserService");
        //    var user = ObjUserService.GetUser(LoginID);
        //    return Json(user == null);
        //}
    }
}
