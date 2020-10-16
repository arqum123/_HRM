using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;
using HRM.Core.Model;
using HRM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.WebAPI.Controllers
{
    public class LeaveController : Controller
    {
        //
        // GET: /Leave/

        [HttpGet]
        [Authenticate]
        public ActionResult ViewAllUsers()
        {
            VMUserModel VMUserModel = new VMUserModel();
            UserListPrerequisiteData();
            return View(VMUserModel);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult ViewAllUsers(VMUserModel model)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

            model.UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId);

            if (model.DepartmentID != null && model.DepartmentID != -1)
                model.UserList = model.UserList.Where(x => x.UserDepartment.DepartmentId == model.DepartmentID).ToList();
            if (model.UserName != null && model.UserName != "")
                model.UserList = model.UserList.Where(x => x.FirstName.ToLower().Contains(model.UserName.ToLower())).ToList();


            UserListPrerequisiteData();

            return View(model);

        }

        [HttpGet]
        [Authenticate]
        public ActionResult MarkLeave(int UserId = 0)
        {

            LeaveTypeRepository objleavetype = new LeaveTypeRepository();

            UserDepartment userdept = null;
            UserDepartmentRepository objuserdept = new UserDepartmentRepository();
            userdept = objuserdept.GetUserDepartmentByUserId(UserId).Last();
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

            IUserService objUserService = IoC.Resolve<IUserService>("UserService");

            ViewBag.dept = objDepartmentService.GetDepartment(userdept.DepartmentId.Value).Name;

            ViewBag.user = objUserService.GetUser(userdept.UserId.Value).FirstName;


            VMLeave model = new VMLeave();
            model.NewLeave = new Leave();
            LeaveRepository objleave = new LeaveRepository();

            model.LeaveHistory = objleave.GetLeaveByUserId(UserId);
            model.NewLeave.UserId = UserId;




            //List<UserDepartment> userdepts = objuserdept.GetUserDepartmentByUserId(UserId);
            //if (userdepts != null && userdepts.Count > 0)
            //    ViewBag.dept = new SelectList(userdepts, "Id", "Name");


            List<LeaveType> leavetypes = objleavetype.GetAllLeaveType();
            if (leavetypes != null && leavetypes.Count > 0)
                ViewBag.LeaveTypeList = new SelectList(leavetypes, "Id", "Name");

            SelectList sl = new SelectList(leavetypes, "Id", "Name");
            string ssss = sl.Where(x => x.Value == "1").FirstOrDefault().Text;
            return View(model);

        }

        [HttpPost]
        [Authenticate]
        public ActionResult MarkLeave(VMLeave model)
        {
            PayrollCycle _model = new PayrollCycle();
            LeaveRepository objleave = new LeaveRepository();
            IPayrollCycleService objPayrollCycle = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            _model = objPayrollCycle.GetAllPayrollCycle().OrderBy(x => x.Year).ThenBy(x => x.Month).Last();
          

            // _model.Month,_model.Year;

            DateTime dtLastDateOfLastPayroll = new DateTime(_model.Year.Value, _model.Month.Value, 1).AddMonths(1).AddDays(-1);
             model.LeaveHistory = objleave.GetLeaveByUserId(model.NewLeave.UserId);
             if (ModelState.IsValid)
             {
                 if (model.NewLeave.DateFrom.Value <= dtLastDateOfLastPayroll)
                 {
                     ViewBag.ErrorMessage = "Date From must be greater than payroll generated month";
                 }
                 else if (model.NewLeave.DateTo.Value < model.NewLeave.DateFrom.Value)
                 {
                     ViewBag.ErrorMessage = "Date To must be greater than date From";
                 }

                 else
                 {


                     for (DateTime dt = model.NewLeave.DateFrom.Value; dt <= model.NewLeave.DateTo.Value; dt = dt.AddDays(1))
                     {
                         model.NewLeave.Date = dt;
                         model.NewLeave.IsActive = true;
                         model.NewLeave.CreationDate = DateTime.Now;
                         model.NewLeave.UpdatedBy = AuthBase.UserId;
                         model.NewLeave.UserIp = Request.UserHostAddress;
                         if (model.LeaveHistory != null && model.LeaveHistory.Where(x => x.Date.Value == model.NewLeave.Date.Value).Any())
                         {
                             ViewBag.ErrorMessage = "Leave already exists on that date.";
                         }
                         else
                         {
                             objleave.InsertLeave(model.NewLeave);
                         }
                     }
                 }
             }



            LeaveTypeRepository objleavetype = new LeaveTypeRepository();
            List<LeaveType> leavetypes = objleavetype.GetAllLeaveType();
            ViewBag.LeaveTypeList = new SelectList(leavetypes, "Id", "Name");

            UserDepartment userdept = null;
            UserDepartmentRepository objuserdept = new UserDepartmentRepository();
            userdept = objuserdept.GetUserDepartmentByUserId(model.NewLeave.UserId).Last();
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

            IUserService objUserService = IoC.Resolve<IUserService>("UserService");

            ViewBag.dept = objDepartmentService.GetDepartment(userdept.DepartmentId.Value).Name;

            ViewBag.user = objUserService.GetUser(userdept.UserId.Value).FirstName;



            int userId = model.NewLeave.UserId.Value;
            model.NewLeave = new Leave();
            model.NewLeave.UserId = userId;

            Response.Redirect("/Leave/MarkLeave?UserID=" + userId.ToString());

            return View(model);
        }




        private void UserPrerequisiteData()
        {
            //IUserTypeService objUserTypeService = IoC.Resolve<IUserTypeService>("UserTypeService");
            //ViewBag.UserType = new SelectList(objUserTypeService.GetAllUserType(), "Id", "Name");

            //ICountryService objCountryService = IoC.Resolve<ICountryService>("CountryService");
            //ViewBag.Country = new SelectList(objCountryService.GetAllCountry(), "Id", "Name");

            //IReligionService objReligionService = IoC.Resolve<IReligionService>("ReligionService");
            //ViewBag.Religion = new SelectList(objReligionService.GetAllReligion(), "Id", "Name");

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

            //ISalaryTypeService objSalaryTypeService = IoC.Resolve<ISalaryTypeService>("SalaryTypeService");
            //ViewBag.SalaryType = new SelectList(objSalaryTypeService.GetAllSalaryType(), "Id", "Name");
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


    }


}
