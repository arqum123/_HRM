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
    [Authorize(Roles = "Admin")]
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
            //userdept = objuserdept.GetUserDepartmentByUserId(UserId).Last();
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
        [HttpGet]
        [Authenticate]
        public ActionResult ApprovedLeaves(string UserName, int? DepartmentName, DateTime? StartDate, DateTime? EndDate, string SortOrder, string SortBy, int? PageNumber)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            if (PageNumber == 0 || PageNumber == null)
                PageNumber = 1;
            if (SortOrder == null && SortBy == null && StartDate == null && EndDate == null)
            {
                VMApprovedLeaves model = new VMApprovedLeaves();
                try
                {
                    model.StartDate = DateTime.Now;
                    model.EndDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                GetDepartmentsAndUsers(model.UserID, model.DepartmentID); //TODO
                return View(model);
            }

            else
            {
                VMApprovedLeaves model = new VMApprovedLeaves();
                if (StartDate != null || StartDate != DateTime.MinValue)
                    model.StartDate = Convert.ToDateTime(StartDate);
                if (EndDate != null || EndDate != DateTime.MinValue)
                    model.EndDate = Convert.ToDateTime(EndDate);
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                ILeaveService objLeaveService = IoC.Resolve<ILeaveService>("LeaveService");
                model.VMApprovedAllLeavesList = objLeaveService.GetApprovedLeavesByDateRange(model.UserID, model.DepartmentID, model.StartDate, model.EndDate).ToList();
                if (model.UserName != null)
                    model.VMApprovedAllLeavesList = model.VMApprovedAllLeavesList.Where(x => x.EmpName.ToLower().Contains(model.UserName.ToLower())).ToList();
                if (model.VMApprovedAllLeavesList == null || model.VMApprovedAllLeavesList.Count == 0)
                    ModelState.AddModelError("", "No Records Found");
                else
                {
                    #region Pagination
                    double PageCount = model.VMApprovedAllLeavesList.Count;
                    ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                    model.VMApprovedAllLeavesList = model.VMApprovedAllLeavesList.Skip((Convert.ToInt32(PageNumber - 1)) * 5).Take(5).ToList();
                    #endregion Pagination
                    #region Sorting
                    if (SortBy == "Date")
                    {
                        if (SortOrder == "Asc")
                        {
                            model.VMApprovedAllLeavesList = model.VMApprovedAllLeavesList.OrderBy(x => x.Date).ToList();

                        }
                        else if (SortOrder == "Desc")
                        {
                            model.VMApprovedAllLeavesList = model.VMApprovedAllLeavesList.OrderByDescending(x => x.Date).ToList();

                        }
                    }
                    #endregion Sorting
                }
                GetDepartmentsAndUsers(model.UserID, model.DepartmentID); //TODO
                return View(model);
            }
        }
        [HttpPost]
        [Authenticate]
        public ActionResult ApprovedLeaves(VMApprovedLeaves model, string SortOrder, string SortBy, int PageNumber = 1)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            ILeaveService objLeaveService = IoC.Resolve<ILeaveService>("LeaveService");
            model.VMApprovedAllLeavesList = objLeaveService.GetApprovedLeavesByDateRange(model.UserID, model.DepartmentID, model.StartDate, model.EndDate).ToList();
            if (model.UserName != null)
                model.VMApprovedAllLeavesList = model.VMApprovedAllLeavesList.Where(x => x.EmpName.ToLower().Contains(model.UserName.ToLower())).ToList();
            if (model.VMApprovedAllLeavesList == null || model.VMApprovedAllLeavesList.Count == 0)
                ModelState.AddModelError("", "No Records Found");
            else
            {
                #region Pagination
                double PageCount = model.VMApprovedAllLeavesList.Count;
                ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                model.VMApprovedAllLeavesList = model.VMApprovedAllLeavesList.Skip((Convert.ToInt32(PageNumber - 1)) * 5).Take(5).ToList();
                #endregion Pagination
                #region Sorting
                if (SortBy == "Date")
                {
                    if (SortOrder == "Asc")
                    {
                        model.VMApprovedAllLeavesList = model.VMApprovedAllLeavesList.OrderBy(x => x.Date).ToList();

                    }
                    else if (SortOrder == "Desc")
                    {
                        model.VMApprovedAllLeavesList = model.VMApprovedAllLeavesList.OrderByDescending(x => x.Date).ToList();

                    }
                }
                #endregion Sorting
            }
            GetDepartmentsAndUsers(model.UserID, model.DepartmentID); //TODO
            return View(model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult ManageLeaves(string UserName, int? DepartmentName, DateTime? StartDate, DateTime? EndDate, string SortOrder, string SortBy, int? PageNumber)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            if (PageNumber == 0 || PageNumber == null)
                PageNumber = 1;
            if (SortOrder == null && SortBy == null && StartDate == null && EndDate == null)
            {
                VMViewPendingLeaves model = new VMViewPendingLeaves();
                try
                {
                    model.StartDate = DateTime.Now;
                    model.EndDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                GetDepartmentsAndUsers(model.UserID, model.DepartmentID); //TODO
                return View(model);
            }
            else
            {
                VMViewPendingLeaves model = new VMViewPendingLeaves();
                if (StartDate != null || StartDate != DateTime.MinValue)
                    model.StartDate = Convert.ToDateTime(StartDate);
                if (EndDate != null || EndDate != DateTime.MinValue)
                    model.EndDate = Convert.ToDateTime(EndDate);
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                ILeaveService objLeaveService = IoC.Resolve<ILeaveService>("LeaveService");
                model.VMPendingViewAllLeavesList = objLeaveService.GetAllPendingLeavesByDateRange(model.UserID, model.DepartmentID, model.StartDate, model.EndDate).Where(x => x.IsReject == false).ToList();
                if (model.UserName != null)
                    model.VMPendingViewAllLeavesList = model.VMPendingViewAllLeavesList.Where(x => x.EmpName.ToLower().Contains(model.UserName.ToLower())).ToList();
                if (model.VMPendingViewAllLeavesList == null || model.VMPendingViewAllLeavesList.Count == 0)
                    ModelState.AddModelError("", "No Records Found");
                else
                {
                    #region Pagination
                    double PageCount = model.VMPendingViewAllLeavesList.Count;
                    ViewBag.TotalPages = Math.Ceiling(PageCount / 5);
                    model.VMPendingViewAllLeavesList = model.VMPendingViewAllLeavesList.Skip((Convert.ToInt32(PageNumber - 1)) * 5).Take(5).ToList();
                    #endregion Pagination
                    #region Sorting
                    if (SortBy == "Date")
                    {
                        if (SortOrder == "Asc")
                        {
                            model.VMPendingViewAllLeavesList = model.VMPendingViewAllLeavesList.OrderBy(x => x.Date).ToList();

                        }
                        else if (SortOrder == "Desc")
                        {
                            model.VMPendingViewAllLeavesList = model.VMPendingViewAllLeavesList.OrderByDescending(x => x.Date).ToList();

                        }
                    }
                    #endregion Sorting
                }
                GetDepartmentsAndUsers(model.UserID, model.DepartmentID); //TODO
                return View(model);
            }
        }
        [HttpPost]
        [Authenticate]
        public ActionResult ManageLeaves(VMViewPendingLeaves model, string SortOrder, string SortBy, int PageNumber = 1)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            ILeaveService objLeaveService = IoC.Resolve<ILeaveService>("LeaveService");
            model.VMPendingViewAllLeavesList = objLeaveService.GetAllPendingLeavesByDateRange(model.UserID, model.DepartmentID, model.StartDate, model.EndDate).Where(x=>x.IsReject == false).ToList();
            if (model.UserName != null)
                model.VMPendingViewAllLeavesList = model.VMPendingViewAllLeavesList.Where(x => x.EmpName.ToLower().Contains(model.UserName.ToLower())).ToList();
            if(model.VMPendingViewAllLeavesList ==null || model.VMPendingViewAllLeavesList.Count ==0)
                ModelState.AddModelError("", "No Records Found");
            else
            {
                #region Pagination
                double PageCount = model.VMPendingViewAllLeavesList.Count;
                ViewBag.TotalPages = Math.Ceiling(PageCount / 5);
                model.VMPendingViewAllLeavesList = model.VMPendingViewAllLeavesList.Skip((Convert.ToInt32(PageNumber - 1)) * 5).Take(5).ToList();
                #endregion Pagination
                #region Sorting
                if (SortBy == "Date")
                {
                    if (SortOrder == "Asc")
                    {
                        model.VMPendingViewAllLeavesList = model.VMPendingViewAllLeavesList.OrderBy(x => x.Date).ToList();

                    }
                    else if (SortOrder == "Desc")
                    {
                        model.VMPendingViewAllLeavesList = model.VMPendingViewAllLeavesList.OrderByDescending(x => x.Date).ToList();

                    }
                }
                #endregion Sorting
            }
            GetDepartmentsAndUsers(model.UserID, model.DepartmentID); //TODO
            return View(model);
        }
        [HttpPost]
        [Authenticate]
        public JsonResult ManageLeavesUpdation(List<VMPendingViewAllLeaves> VMPendingViewAllLeavesList)
        {
            try
            {
                ILeaveService objLeaveService = IoC.Resolve<ILeaveService>("LeaveService");
                foreach (VMPendingViewAllLeaves model in VMPendingViewAllLeavesList)
                {
                    if (model.IsApproved == true)
                    {
                        #region Update Leave
                        Leave LeaveList = objLeaveService.GetLeave(model.LeaveID);
                        Leave LeaveDetail = new Leave()
                        {
                            Id = model.LeaveID,
                            UserId = model.UserID,
                            Date = model.Date,
                            Reason = LeaveList.Reason,
                            LeaveTypeId = LeaveList.LeaveTypeId,
                            IsActive = LeaveList.IsActive,
                            UpdateDate = DateTime.Now,
                            UserIp = Request.UserHostAddress,
                            CreationDate = LeaveList.CreationDate,
                            AdminReason = model.AdminReason,
                            IsApproved = model.IsApproved,
                            IsReject = model.IsReject,
                            UpdatedBy = AuthBase.UserId
                        };
                        LeaveDetail = objLeaveService.UpdateLeave(LeaveDetail);
                        #endregion
                    }
                    else if (model.IsReject == true)
                    {
                        #region Update Leave
                        Leave LeaveList = objLeaveService.GetLeave(model.LeaveID);
                        var LeaveID = LeaveList.Id;
                        Leave LeaveDetail = new Leave()
                        {
                            Id = model.LeaveID,
                            UserId = model.UserID,
                            Date = model.Date,
                            Reason = LeaveList.Reason,
                            LeaveTypeId = LeaveList.LeaveTypeId,
                            IsActive = LeaveList.IsActive,
                            UpdateDate = DateTime.Now,
                            UserIp = Request.UserHostAddress,
                            CreationDate = LeaveList.CreationDate,
                            AdminReason = model.AdminReason,
                            IsApproved = model.IsApproved,
                            IsReject = model.IsReject,
                            UpdatedBy = AuthBase.UserId
                        };
                        LeaveDetail = objLeaveService.UpdateLeave(LeaveDetail);
                        #endregion
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Json(new { Success = false, Status = "Failure", ModelError = true });
            }
                return Json(new { Success = false, Status = "Success" }); 
        }
      private void GetDepartmentsAndUsers(int? UserId, int? DepartmentId)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            List<User> Users = objUserService.GetAllUser();
            if (UserId.HasValue && UserId.Value > 0)
                ViewBag.UserName = Users.Where(x => x.Id == UserId.Value).FirstOrDefault().FirstName;
            if (Users != null && Users.Count > 0)
                ViewBag.User = new SelectList(Users, "ID", "FirstName");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "User", Value = "-1" });
                ViewBag.User = new SelectList(blank, "Value", "Text", "");
            }
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            List<Department> DepartmentList = objDepartmentService.GetAllDepartment();
            List<Department> DistinctDepartmentList = new List<Department>();
            foreach (string dep in DepartmentList.GroupBy(l => l.Name).Select(x => x.Key))
            {
                DistinctDepartmentList.Add(new Department() { Name = dep, Id = DepartmentList.Where(x => x.Name == dep).FirstOrDefault().Id });
            }
            //DepartmentList = DepartmentList.GroupBy(l => l.Name).SelectMany(cl => cl.Select(csLine => new Department { Name = csLine.Name, Id = cl.Max(c => c.Id), })).ToList<Department>();
            if (DepartmentId != null && DepartmentId.Value > 0)
                ViewBag.DeptName = DistinctDepartmentList.Where(x => x.Id == DepartmentId).FirstOrDefault().Name;
            if (DistinctDepartmentList != null && DepartmentList != null && DepartmentList.Count > 0 && DistinctDepartmentList.Count > 0)
                ViewBag.Department = new SelectList(DistinctDepartmentList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Department", Value = "-1" });
                ViewBag.Department = new SelectList(blank, "Value", "Text", "");
            }
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
