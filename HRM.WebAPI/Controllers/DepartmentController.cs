using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;
using HRM.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        [HttpGet]
        [Authenticate]
        public ActionResult Index(string Id)
        {
            Department model = new Department();
            try
            {
                if (Id != null && Id != "")
                {
                    IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                    model = objDepartmentService.GetDepartment(int.Parse(Id));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Index(Department model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id != 0)
                    {
                        #region Department Updation
                        IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                        
                        model.UpdateDate = DateTime.Now;
                        model.UpdateBy = AuthBase.UserId;
                        model.UserIp = Request.UserHostAddress;

                        Department _department = objDepartmentService.UpdateDepartment(model);

                        #endregion
                    }
                    else
                    {
                        #region Department Entry
                        IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

                        model.IsActive = true;
                        model.CreationDate = DateTime.Now;
                        model.UpdateBy = AuthBase.UserId;
                        model.UserIp = Request.UserHostAddress;

                        Department _department = objDepartmentService.InsertDepartment(model);

                        IBranchDepartmentService iBranchDepartmentService = IoC.Resolve<IBranchDepartmentService>("BranchDepartmentService");
                        BranchDepartment _branchDept = new BranchDepartment() 
                        {
                            BranchId = AuthBase.BranchId,
                            DepartmentId = _department.Id,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                            UserIp = Request.UserHostAddress,
                            UpdatedBy = AuthBase.UserId
                        };
                        iBranchDepartmentService.InsertBranchDepartment(_branchDept);

                        #endregion
                    }
                    return RedirectToAction("DepartmentList", "Department");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ModelState.AddModelError("", "Unable to process your request. Please contact Administration");
            return View();
        }

        [HttpGet]
        [Authenticate]
        public ActionResult DepartmentList(int? PageNumber,int? Id)
        {

            if (PageNumber == null)
                PageNumber = 1;
            VMDepartmentModel VMDepartmentModel = new VMDepartmentModel();
            VMDepartmentModel model = new VMDepartmentModel();
            try
            {
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                VMDepartmentModel.DepartmentList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);
                if (VMDepartmentModel.DepartmentList == null || VMDepartmentModel.DepartmentList.Where(x => x.IsActive == true).ToList().Count == 0)
                    ModelState.AddModelError("", "No Records Found");
                else
                {
                    IUserDepartmentService ObjUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
                    List<UserDepartment> UserDepartmentList = ObjUserDepartmentService.GetUserDepartmentByUserId(Convert.ToInt32(Id));
                    if (UserDepartmentList != null && UserDepartmentList.Count > 0)
                    {
                        model.UserDepartmentList = UserDepartmentList;
                        foreach (var department in model.UserDepartmentList)
                        {
                            department.Department = objDepartmentService.GetDepartment((int)department.DepartmentId);
                        }
                        UserDepartmentList = ApplyDepartmentListPagination(Convert.ToInt32(PageNumber), UserDepartmentList);
                        ViewBag.PageNumber = PageNumber;
                        ViewBag.DepartmentID = Id;
                    }
                    if (UserDepartmentList == null || UserDepartmentList.Count ==0 && VMDepartmentModel.DepartmentList != null)
                    {
                        double PageCount = VMDepartmentModel.DepartmentList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        VMDepartmentModel.DepartmentList = VMDepartmentModel.DepartmentList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                        ViewBag.PageNumber = PageNumber;
                        ViewBag.DepartmentID = Id;
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(VMDepartmentModel);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult DepartmentHistory(string Id, int? PageNumber)
        {
            if (PageNumber == null)
                PageNumber = 1;
            VMDepartmentModel model = new VMDepartmentModel();
            try
            {
                if (Id != null && Id != "")
                {
                    IUserDepartmentService ObjUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
                    IDepartmentService ObjDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                    List<UserDepartment> UserDepartmentList = ObjUserDepartmentService.GetUserDepartmentByUserId(int.Parse(Id));
                    if (UserDepartmentList != null && UserDepartmentList.Count > 0)
                    {
                        model.UserDepartmentList = UserDepartmentList;
                        foreach (var department in model.UserDepartmentList)
                        {
                            department.Department = ObjDepartmentService.GetDepartment((int)department.DepartmentId);
                        }
                        double PageCount = UserDepartmentList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        UserDepartmentList = UserDepartmentList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                        ViewBag.PageNumber = PageNumber;
                        ViewBag.DepartmentID = Id;
                    }
                    else
                        ModelState.AddModelError("", "No Records Found");
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        public List<UserDepartment> ApplyDepartmentListPagination(int PageNumber, List<UserDepartment> UserDepartmentList)
        {

            double PageCount = UserDepartmentList.Count;
            ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
            UserDepartmentList = UserDepartmentList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
            return UserDepartmentList;
        }
    }
}
