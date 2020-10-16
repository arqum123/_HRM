using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;
using HRM.Core.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
namespace HRM.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PayrollController : Controller
    {
        //
        // GET: /Payroll/

        [HttpGet]
        [Authenticate]
        public ActionResult Index()
        {
            InsertPayrolCycle();
            ViewPayrollPrerequisiteData();
            VMAttendanceModel model = new VMAttendanceModel();
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Index(VMAttendanceModel model)
        {
            try
            {
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                List<Payroll> PayrollList = objPayrollService.GetAllPayrollWithUser(AuthBase.BranchId);

                if (PayrollList != null && PayrollList.Count > 0)
                {
                    int CurrentPayrollCycleID = model.PayrollCycle.Id;
                    PayrollList = PayrollList.Where(x => x.PayrollCycleId == CurrentPayrollCycleID).ToList();
                    if (PayrollList != null && PayrollList.Count > 0)
                    {
                        model.DepartmentList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);
                        if (model.DepartmentList != null)
                        {
                            foreach (var department in model.DepartmentList)
                            {
                                department.TotalUser = 0; department.TotalSalary = 0; department.TotalAddition = 0; department.TotalDeduction = 0; department.NetSalary = 0;
                                List<User> UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId).Where(x => x.Department.Id == department.Id).ToList();
                                if (UserList.Count > 0)
                                {
                                    department.TotalUser = UserList.Count();
                                    department.TotalSalary = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Salary).Value;
                                    department.TotalAddition = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Addition).Value;
                                    department.TotalDeduction = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Deduction).Value;
                                    department.NetSalary = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.NetSalary).Value;
                                }
                            }
                        }
                        else
                            ModelState.AddModelError("", "No Records Found");
                    }
                    else
                        ModelState.AddModelError("", "No Records Found");
                }
                else
                {
                    model.PayrollCycle = objPayrollCycleService.GetAllPayrollCycle().OrderBy(x => x.Id).FirstOrDefault();
                    ModelState.AddModelError("", "No Records Found");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewPayrollPrerequisiteData();
            return View(model);
        }

        private void GetSalaryType(int? SalaryTypeId)
        {
            ISalaryTypeService objSalaryTypeService = IoC.Resolve<ISalaryTypeService>("SalaryTypeService");
            List<SalaryType> salaries = objSalaryTypeService.GetAllSalaryType();
            if (SalaryTypeId.HasValue && SalaryTypeId.Value > 0)
                ViewBag.UserName = salaries.Where(x => x.Id == SalaryTypeId.Value).FirstOrDefault().Name;
            if (salaries != null && salaries.Count > 0)
                ViewBag.SalaryType = new SelectList(salaries, "ID", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "SalaryType", Value = "-1" });
                ViewBag.SalaryType = new SelectList(blank, "Value", "Text", "");
            }
        }
        [HttpGet]
        [Authenticate]
        public ActionResult Variable(string Id)
        {
            InsertPayrolCycle();
            VariablePrerequisiteData();
            PayrollPolicy _model = new PayrollPolicy();
            try
            {
                IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
                if (Id != null && Id != "")
                {
                    PayrollPolicy _policy = objPayrollPolicyService.GetPayrollPolicy(int.Parse(Id));
                    if (_policy != null)
                        _model = _policy;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(_model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Variable(PayrollPolicy _model)
        {
            bool AlreadyExist = false;
            if (ModelState.IsValid)
            {
                try
                {
                    IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
                    if (_model.Id != 0)
                    {
                        PayrollPolicy _policy = objPayrollPolicyService.GetPayrollPolicy(_model.Id);
                        #region Payroll Variable Update and Entry
                        if (_model.Value != _policy.Value || _model.IsUnit != _policy.IsUnit)
                        {
                            Dictionary<string, string> dicPayrollPolicy = new Dictionary<string, string>();
                            dicPayrollPolicy.Add("RetiredDate", string.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(-1)) + " 23:59:59");
                            dicPayrollPolicy.Add("UpdateDate", DateTime.Now.ToString());
                            dicPayrollPolicy.Add("UpdateBy", AuthBase.UserId.ToString());
                            dicPayrollPolicy.Add("UserIp", Request.UserHostAddress);
                            objPayrollPolicyService.UpdatePayrollPolicyByKeyValue(dicPayrollPolicy, _model.Id);

                            _model.Id = 0;
                            _model.PayrollVariableId = _policy.PayrollVariableId;
                            _model.IsPercentage = true;
                            if ((bool)_model.IsUnit)
                                _model.IsPercentage = false;
                            _model.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                            _model.CreationDate = DateTime.Now;
                            _model.UpdateBy = AuthBase.UserId;
                            _model.UserIp = Request.UserHostAddress;
                            objPayrollPolicyService.InsertPayrollPolicy(_model);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Payroll Variable Entry
                        List<PayrollPolicy> PayrollPolicyList = objPayrollPolicyService.GetAllPayrollPolicy();
                        if (PayrollPolicyList != null && PayrollPolicyList.Count > 0)
                        {
                            if (PayrollPolicyList.Where(x => x.PayrollVariableId == _model.PayrollVariableId && x.RetiredDate == null).Any())
                                AlreadyExist = true;
                            else
                            {
                                _model.IsPercentage = true;
                                if ((bool)_model.IsUnit)
                                    _model.IsPercentage = false;
                                _model.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                                _model.CreationDate = DateTime.Now;
                                _model.UpdateBy = AuthBase.UserId;
                                _model.UserIp = Request.UserHostAddress;
                                objPayrollPolicyService.InsertPayrollPolicy(_model);
                            }
                        }
                        else
                        {
                            _model.IsPercentage = true;
                            if ((bool)_model.IsUnit)
                                _model.IsPercentage = false;
                            _model.EffectiveDate = Convert.ToDateTime(string.Format("{0:dd-MMM-yyyy}", DateTime.Now));
                            _model.CreationDate = DateTime.Now;
                            _model.UpdateBy = AuthBase.UserId;
                            _model.UserIp = Request.UserHostAddress;
                            objPayrollPolicyService.InsertPayrollPolicy(_model);
                        }
                        #endregion
                    }

                    //VariablePrerequisiteData();
                    //return View();
                    if (!AlreadyExist)
                        return RedirectToAction("VariableList", "Payroll");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            VariablePrerequisiteData();
            if (AlreadyExist)
                ModelState.AddModelError("", "This Variable is already marked in system. To edit this variable please go to View All Payroll Variables screen.");
            else
                ModelState.AddModelError("", "Unable to process your request. Please contact Administration");
            return View();
        }

        [HttpGet]
        [Authenticate]
        public ActionResult VariableList(int? PageNumber)
        {
            if (PageNumber == null || PageNumber == 0)
                PageNumber = 1;
            ViewBag.PageNumber = PageNumber;
            InsertPayrolCycle();
            VMPayrollModel _model = new VMPayrollModel();
            try
            {
                IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
                IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
                List<PayrollPolicy> PayrollPolicyList = objPayrollPolicyService.GetAllPayrollPolicy();
                if (PayrollPolicyList != null && PayrollPolicyList.Count > 0)
                {
                    PayrollPolicyList = PayrollPolicyList.Where(x => x.RetiredDate == null).ToList();
                    if (PayrollPolicyList != null && PayrollPolicyList.Count > 0)
                    {
                        _model.PayrollPolicyList = PayrollPolicyList;
                        #region Pagination
                        double PageCount = _model.PayrollPolicyList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        _model.PayrollPolicyList = _model.PayrollPolicyList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                        #endregion Pagination
                        foreach (var policy in _model.PayrollPolicyList)
                        {
                            PayrollVariable _variable = objPayrollVariableService.GetPayrollVariable((int)policy.PayrollVariableId);
                            policy.PayrollVariableName = Enum.GetName(typeof(HRM.Core.Enum.PayrollVariable), policy.PayrollVariableId);
                            policy.PayrollVariableFactor = _variable.IsDeduction == true ? "Deduction" : "Addition";
                            policy.PayrollPolicyValue = (decimal)policy.Value;
                        }
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
            return View(_model);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Payslip(int? PageNumber,int? Month,int? Year,string DepartmentID = null, string PayrollCycleID = null, string IsActive = null)
        {
          
            PayslipPrerequisiteData();
            if(PageNumber != null)
            {
                VMPayrollModel model = new VMPayrollModel();
           if(Month != null && Year != null)
                {
                    model.Month = (int)Month;
                    model.Year = (int)Year;
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                List<Payroll> PayrollList = new List<Payroll>();
                model.PayrollList = PayrollList;
                model.IsActive = 1;
                PayrollList = objPayrollService.GetAllPayrollWithUser(AuthBase.BranchId);
                if (PayrollList != null && PayrollList.Count > 0)
                {
                    model.PayrollList = PayrollList.Where(x => x.PayrollCycle.Month == model.Month && x.PayrollCycle.Year <= model.Year).ToList();
                    if (model.DepartmentID != null && model.PayrollList != null && model.PayrollList.Count > 0)
                        model.PayrollList = model.PayrollList.Where(x => x.User.Department.Id == model.DepartmentID).ToList();

                    if (model.UserName != null && model.UserName != "" && model.PayrollList != null && model.PayrollList.Count > 0)
                        model.PayrollList = model.PayrollList.Where(x => x.User.FirstName.ToLower().Contains(model.UserName.ToLower())).ToList();

                    if (!(model.PayrollList != null && model.PayrollList.Count > 0))
                        ModelState.AddModelError("", "No Records Found");

                    double PageCount = model.PayrollList.Count;
                    ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                    model.PayrollList = model.PayrollList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                    ViewBag.PageNumber = PageNumber;

                }
                else
                    ModelState.AddModelError("", "No Records Found");
                PayslipPrerequisiteData();
                return View(model);
                }
            }
            VMPayrollModel _model = new VMPayrollModel();
            if (!string.IsNullOrEmpty(DepartmentID) && !string.IsNullOrEmpty(PayrollCycleID) && !string.IsNullOrEmpty(IsActive))
            {
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                List<Payroll> PayrollList = new List<Payroll>();
                _model.PayrollList = PayrollList;
                PayrollList = objPayrollService.GetAllPayrollWithUser(AuthBase.BranchId, Convert.ToInt32(IsActive));
                if (PayrollList != null && PayrollList.Count > 0)
                {
                    IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
                    PayrollCycle _payrollCycle = objPayrollCycleService.GetPayrollCycle(Convert.ToInt32(PayrollCycleID));
                    _model.Month = (int)_payrollCycle.Month;
                    _model.Year = (int)_payrollCycle.Year;
                    _model.IsActive = Convert.ToInt32(IsActive);
                    _model.DepartmentID = Convert.ToInt32(DepartmentID);

                    _model.PayrollList = PayrollList.Where(x => x.PayrollCycle.Month == _model.Month && x.PayrollCycle.Year <= _model.Year).ToList();
                    if (_model.DepartmentID != null && _model.PayrollList != null && _model.PayrollList.Count > 0)
                        _model.PayrollList = _model.PayrollList.Where(x => x.User.Department.Id == _model.DepartmentID).ToList();

                    if (_model.UserName != null && _model.UserName != "" && _model.PayrollList != null && _model.PayrollList.Count > 0)
                        _model.PayrollList = _model.PayrollList.Where(x => x.User.FirstName.ToLower().Contains(_model.UserName.ToLower())).ToList();

                    if (!(_model.PayrollList != null && _model.PayrollList.Count > 0))
                        ModelState.AddModelError("", "No Records Found");
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            return View(_model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Payslip(VMPayrollModel _model,int? PageNumber)
        {
            if (PageNumber == null)
                PageNumber = 1;
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            List<Payroll> PayrollList = new List<Payroll>();
            _model.PayrollList = PayrollList;
            _model.IsActive = 1;
            PayrollList = objPayrollService.GetAllPayrollWithUser(AuthBase.BranchId);
            if (PayrollList != null && PayrollList.Count > 0)
            {
                _model.PayrollList = PayrollList.Where(x => x.PayrollCycle.Month == _model.Month && x.PayrollCycle.Year <= _model.Year).ToList();
                if (_model.DepartmentID != null && _model.PayrollList != null && _model.PayrollList.Count > 0)
                    _model.PayrollList = _model.PayrollList.Where(x => x.User.Department.Id == _model.DepartmentID).ToList();

                if (_model.UserName != null && _model.UserName != "" && _model.PayrollList != null && _model.PayrollList.Count > 0)
                    _model.PayrollList = _model.PayrollList.Where(x => x.User.FirstName.ToLower().Contains(_model.UserName.ToLower())).ToList();

                if (!(_model.PayrollList != null && _model.PayrollList.Count > 0))
                    ModelState.AddModelError("", "No Records Found");

                double PageCount = _model.PayrollList.Count;
                ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                _model.PayrollList = _model.PayrollList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                ViewBag.PageNumber = PageNumber;
            }
            else
                ModelState.AddModelError("", "No Records Found");
            PayslipPrerequisiteData();
            return View(_model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult PayslipDetail(string Id, string Month, string Year, string IsActive)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
            IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            VMPayrollModel _model = new VMPayrollModel();
            _model.Payroll = objPayrollService.GetAllPayrollWithUser(AuthBase.BranchId, Convert.ToInt32(IsActive)).Where(x => x.UserId == int.Parse(Id) && x.PayrollCycle.Month == int.Parse(Month) && x.PayrollCycle.Year == int.Parse(Year)).FirstOrDefault();
            _model.IsActive = Convert.ToInt32(IsActive);
            if (_model.Payroll != null)
            {
                _model.Payroll.QuarterDayCount = 0; _model.Payroll.QuarterDayAmount = 0;
                _model.Payroll.HalfDayCount = 0; _model.Payroll.HalfDayAmount = 0;
                _model.Payroll.FullDayCount = 0; _model.Payroll.FullDayAmount = 0;
                _model.Payroll.AbsentDayCount = 0; _model.Payroll.AbsentDayAmount = 0;
                _model.Payroll.OverTimeDayCount = 0; _model.Payroll.OverTimeDayAmount = 0;
                //New
                _model.Payroll.EarlyDayCount = 0; _model.Payroll.EarlyDayCount = 0;
                _model.Payroll.LateDayCount = 0; _model.Payroll.LateDayCount = 0;

                _model.Payroll.PayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId((int)_model.Payroll.Id);
                if (_model.Payroll.PayrollDetailList != null && _model.Payroll.PayrollDetailList.Count > 0)
                {
                    foreach (var detail in _model.Payroll.PayrollDetailList)
                    {
                        if (detail.PayrollPolicyId != null)
                        {
                            detail.PayrollPolicy = objPayrollPolicyService.GetPayrollPolicy((int)detail.PayrollPolicyId);
                            detail.PayrollPolicy.PayrollVariable = objPayrollVariableService.GetPayrollVariable((int)detail.PayrollPolicy.PayrollVariableId);

                            if (detail.PayrollPolicy.PayrollVariable.Id == Convert.ToInt32(HRM.Core.Enum.PayrollVariable.QuarterDay))
                            {
                                _model.Payroll.QuarterDayCount++;
                                _model.Payroll.QuarterDayAmount += detail.Amount;
                            }
                            if (detail.PayrollPolicy.PayrollVariable.Id == Convert.ToInt32(HRM.Core.Enum.PayrollVariable.HalfDay))
                            {
                                _model.Payroll.HalfDayCount++;
                                _model.Payroll.HalfDayAmount += detail.Amount;
                            }
                            if (detail.PayrollPolicy.PayrollVariable.Id == Convert.ToInt32(HRM.Core.Enum.PayrollVariable.FullDay))
                            {
                                _model.Payroll.FullDayCount++;
                                _model.Payroll.FullDayAmount += detail.Amount;
                            }
                            if (detail.PayrollPolicy.PayrollVariable.Id == Convert.ToInt32(HRM.Core.Enum.PayrollVariable.Absent))
                            {
                                _model.Payroll.AbsentDayCount++;
                                _model.Payroll.AbsentDayAmount += detail.Amount;
                            }
                            //if (detail.PayrollPolicy.PayrollVariable.Id == Convert.ToInt32(HRM.Core.Enum.PayrollVariable.OverTime))
                            //{
                            //    _model.Payroll.OverTimeDayCount++;
                            //    _model.Payroll.OverTimeDayAmount += detail.Amount;
                            //}
                            //New
                            if (detail.PayrollPolicy.PayrollVariable.Id == Convert.ToInt32(HRM.Core.Enum.PayrollVariable.Early))
                            {
                                _model.Payroll.EarlyDayCount++;
                                _model.Payroll.EarlyDayAmount += detail.Amount;
                            }
                            if (detail.PayrollPolicy.PayrollVariable.Id == Convert.ToInt32(HRM.Core.Enum.PayrollVariable.Late))
                            {
                                _model.Payroll.LateDayCount++;
                                _model.Payroll.LateDayAmount += detail.Amount;
                            }
                        }
                    }
                }
                return View(_model);
            }
            else
            {
                ModelState.AddModelError("", "No Records Found");
                return View();
            }
        }
        [HttpGet]
        [Authenticate]
        public ActionResult Ticket()
        {
            VMTicket model = new VMTicket();
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            model.VMTicketList = objPayrollService.GetPendingTickets();
            return View(model);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult Ticket(VMTicket model)
        {
            ITicketService objTicketService = IoC.Resolve<ITicketService>("TicketService");
            try { 
            Ticket ticketList = objTicketService.GetTicket(Convert.ToInt32(model.TicketID));
            #region Update Ticket
            Ticket vmTicket = new Ticket()
            {
                ID = Convert.ToInt32(model.TicketID),
                UserID = ticketList.UserID,
                Reason = ticketList.Reason,
                IsApproved = model.IsApproved,
                IsReject = model.IsReject,
                Comments = model.Comments,
                CreationDate = ticketList.CreationDate,
                UpdateDate = DateTime.Now,
                UpdateBy = AuthBase.UserId,
                UserIp = Request.UserHostAddress,
                AttendanceID = ticketList.AttendanceID,
            };
            vmTicket = objTicketService.UpdateTicket(vmTicket);
            #endregion Update Ticket
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json("vmTicket", JsonRequestBehavior.AllowGet);
        }
        private void GetUsersAndDepartments(int? UserId, int? DepartmentId)
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
        [HttpGet]
        [Authenticate]
        public ActionResult TicketHistory(DateTime? StartDate,DateTime?EndDate,string UserName,int?UserId,int?DepartmentId,String SortOrder, String SortBy, int? PageNumber)
        {
            VMTicketFilter model = new VMTicketFilter();
            if(StartDate!=null && EndDate!=null)
            {
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                model.VMTicketHistoryList = objPayrollService.GetTicketsByDateRange(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UserId, DepartmentId);
                if (model.UserName != null)
                    model.VMTicketHistoryList = model.VMTicketHistoryList.Where(x => x.EmpName == model.UserName).ToList();
                if (model.VMTicketHistoryList != null && model.VMTicketHistoryList.Count !=0)
                {
                    if (PageNumber == null)
                        PageNumber = 1;
                    //pagination
                    model.VMTicketHistoryList = ApplyTicketHistoryPagination(Convert.ToInt32(PageNumber), model.VMTicketHistoryList);
                    ViewBag.SortOrder = SortOrder;
                    ViewBag.PageNumber = PageNumber;
                    ViewBag.EndDate = EndDate;
                    ViewBag.StartDate = StartDate;
                    ViewBag.UserName = UserName;
                    ViewBag.DepartmentId = DepartmentId;
                    //Sorting
                    model.VMTicketHistoryList= ApplyTicketHistorySorting(SortOrder, SortBy, model.VMTicketHistoryList);
                   
                }
            }
            try
            {
                model.StartDate = DateTime.Now.AddMonths(-1).AddDays(-30);
                model.EndDate = DateTime.Now.AddMonths(-1);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            if(UserId != null && DepartmentId != null)
                GetUsersAndDepartments(UserId, DepartmentId); //TODO
            else
            GetUsersAndDepartments(model.UserId, model.DepartmentId); //TODO
            return View(model);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult TicketHistory(VMTicketFilter model,String SortOrder,String SortBy,int? PageNumber)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            model.VMTicketHistoryList = objPayrollService.GetTicketsByDateRange(model.StartDate,model.EndDate,model.UserId,model.DepartmentId);
            if (model.UserName != null)
                model.VMTicketHistoryList = model.VMTicketHistoryList.Where(x => x.EmpName == model.UserName).ToList();
            if (model.VMTicketHistoryList == null)
                ModelState.AddModelError("", "No Records Found");
            else
            {
                if (PageNumber == null)
                    PageNumber = 1;
                //Sorting
                ApplyTicketHistorySorting(SortOrder, SortBy, model.VMTicketHistoryList);
                //pagination
                ApplyTicketHistoryPagination(Convert.ToInt32(PageNumber), model.VMTicketHistoryList);
                ViewBag.SortOrder = SortOrder;
                ViewBag.PageNumber = PageNumber;
                ViewBag.EndDate = model.EndDate;
                ViewBag.StartDate = model.StartDate;
                ViewBag.UserName = model.UserName;
                ViewBag.DepartmentId = model.DepartmentId;
            }
            GetUsersAndDepartments(model.UserId, model.DepartmentId); //TODO
            return View(model);
        }
        public List<VMTicketHistory> ApplyTicketHistorySorting(string SortOrder, string SortBy, List<VMTicketHistory> VMTicketHistoryList)
        {
            if (SortBy == "Date")
            {
                if (SortOrder == "Asc")
                {
                    VMTicketHistoryList = VMTicketHistoryList.OrderBy(x => x.AttendanceDate).ToList();

                }
                else if (SortOrder == "Desc")
                {
                    VMTicketHistoryList = VMTicketHistoryList.OrderByDescending(x => x.AttendanceDate).ToList();

                }
                else
                {
                    VMTicketHistoryList = VMTicketHistoryList.OrderBy(x => x.AttendanceDate).ToList();
                    SortOrder = "Asc";
                }
            }
            return VMTicketHistoryList;
        }
        public List<VMTicketHistory> ApplyTicketHistoryPagination(int PageNumber, List<VMTicketHistory> VMTicketHistoryList)
        {

            double PageCount = VMTicketHistoryList.Count;
            ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
            VMTicketHistoryList = VMTicketHistoryList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
            return VMTicketHistoryList;
        }
        [HttpGet]
        [Authenticate]
        public ActionResult NewPayslipDetail(int UserId, int PayrollCycleId)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            VMPayslipDetail model = new VMPayslipDetail();
            try
            {
                model.VMPayslipDetailUserList = new List<VMPayslipDetailUser>();
                if (PayrollCycleId != 0 && UserId != 0)
                    model.VMPayslipDetailUserList = objPayrollService.GetPayslip(PayrollCycleId, UserId);
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }
        //New Generate Payroll
        private void GetPayrollCycle(int? PayrollCycleId)
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            List<PayrollCycle> payrollCycles = objPayrollCycleService.GetAllPayrollCycle().OrderByDescending(x=>x.Id).ToList();
            if (PayrollCycleId.HasValue && PayrollCycleId.Value > 0)
                ViewBag.UserName = payrollCycles.Where(x => x.Id == PayrollCycleId.Value).FirstOrDefault().Name;
            if (payrollCycles != null && payrollCycles.Count > 0)
                ViewBag.PayrollCycle = new SelectList(payrollCycles, "ID", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "payrollCycles", Value = "-1" });
                ViewBag.PayrollCycle = new SelectList(blank, "Value", "Text", "");
            }
        }
        //New Add Payroll Variable
        [HttpGet]
        [Authenticate]
        public ActionResult AddPayrollVariable()
        {
            VMADDPayrollModel model = new VMADDPayrollModel();
            GetSalaryType(model.SalaryTypeId);
            return View(model);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult AddPayrollVariable(VMADDPayrollModel model) //NewVMADDPayrollModel
        {
            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
            //work on conditions below
            if (model.IsCheck == "IsUnit")
            {
                model.IsUnit = true;
                model.IsPercentage = false;
                model.IsDay = false;
            }
            if (model.IsCheck == "IsPercentage")
            {
                model.IsPercentage = true;
                model.IsUnit = false;
                model.IsDay = false;
            }
            if (model.IsCheck == "IsDay")
            {
                model.IsDay = true;
                model.IsUnit = false;
                model.IsPercentage = false;
            }
            // if(model.IsUnit == null)
            //{
            //    model.IsUnit = false;
            //}
            //if (model.IsPercentage == null)
            //{
            //    model.IsPercentage = false;
            //}
            //if (model.IsDay == null)
            //{
            //    model.IsDay = false;
            //}
            //work on conditions above
            if (model.Value > 0)
            {
                model.IsAddition = true;
                model.IsDeduction = false;
            }
            else if (model.Value < 0)
            {
                model.IsDeduction = true;
                model.IsAddition = false;
            }
            #region PayrollVariable Entry
            PayrollVariable variable = null;
            variable = new PayrollVariable()
            {
                Name = model.PayrollVariableName,
                IsDeduction = model.IsDeduction,
                IsAddition = model.IsAddition,
                IsActive = true,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateBy = AuthBase.UserId,
                UserIp = Request.UserHostAddress
            };
            variable = objPayrollVariableService.InsertPayrollVariable(variable);
            #endregion

            #region PayrollPolicy Entry
            PayrollPolicy policy = null;
            policy = new PayrollPolicy()
            {
                PayrollVariableId = variable.Id,
                IsUnit = model.IsUnit,
                IsPercentage = model.IsPercentage,
                IsDay = model.IsDay,
                Value = model.Value,
                Description = model.Description,
                EffectiveDate = DateTime.Now,
                RetiredDate = null,
                CreationDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdateBy = AuthBase.UserId,
                UserIp = Request.UserHostAddress,
                IsAttendance = model.IsAttendace,
                SalaryType = model.SalaryTypeId,
                Occurance = model.Occurance
            };
            policy = objPayrollPolicyService.InsertPayrollPolicy(policy);
            #endregion

            GetSalaryType(model.SalaryTypeId);
            return View(model);
        }
        private void GetPayrollCycleAndUser(int? PayrollCycleId, int? UserId)
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            List<PayrollCycle> payrollCycles = objPayrollCycleService.GetAllPayrollCycle();
            if (PayrollCycleId.HasValue && PayrollCycleId.Value > 0)
                ViewBag.UserName = payrollCycles.Where(x => x.Id == PayrollCycleId.Value).FirstOrDefault().Name;
            if (payrollCycles != null && payrollCycles.Count > 0)
                ViewBag.PayrollCycle = new SelectList(payrollCycles, "ID", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "payrollCycles", Value = "-1" });
                ViewBag.PayrollCycle = new SelectList(blank, "Value", "Text", "");
            }
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
        }
        ////New Generate Payroll for Particular Users who has no attendance (get)
        [HttpGet]
        [Authenticate]
        public ActionResult GeneratePayrollForUser()
        {
            VMGeneratePayrollForUser model = new VMGeneratePayrollForUser();
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            IUserService userService = IoC.Resolve<IUserService>("UserService");
            GetPayrollCycleAndUser(model.PayrollCycleId,model.UserId);
            return View(model);
        }
        ////New Generate Payroll for Particular Users who has no attendance (post)
       [HttpPost]
       [Authenticate]
       public ActionResult GeneratePayrollForUser(VMGeneratePayrollForUser model)
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            try {
                if (model.UserId > 0 && model.PayrollCycleId > 0)
                {
                    int Amount = 0;
                    model.PayrollList = objPayrollService.GetAllPayroll().Where(x => x.UserId == model.UserId && x.PayrollCycleId == model.PayrollCycleId).ToList();
                    PayrollCycle payrollCycle = objPayrollCycleService.GetPayrollCycle(model.PayrollCycleId);
                    int StartDay = 1;
                    int EndDay = 28;
                    int Month = (int)payrollCycle.Month;
                    int Year = (int)payrollCycle.Year;
                    var StartDate = new DateTime(Year, Month, StartDay);
                    var EndDate = new DateTime(Year, Month, EndDay);
                    var formattedStartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
                    var formattedEndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                    model.UserList = objUserService.GetAllUser().Where(x => x.Id == model.UserId).ToList();
                    User users = model.UserList.FirstOrDefault();
                    List<Attendance> attendanceList = objAttendanceService.GetAttendanceByUserIdAndDateRange(Convert.ToDateTime(formattedStartDate), Convert.ToDateTime(formattedEndDate), model.UserId);
                    List<PayrollPolicy> PayrollPolicyList = objPayrollPolicyService.GetAllPayrollPolicy().Where(x => x.EffectiveDate <= Convert.ToDateTime(formattedStartDate) && (x.RetiredDate == null || x.RetiredDate >= Convert.ToDateTime(formattedEndDate))).ToList();
                    if (attendanceList == null || attendanceList.Count == 0)
                    {
                        if (model.PayrollList.Count == 0)
                        {
                            Payroll payrollEntry = new Payroll()
                            {
                                PayrollCycleId = model.PayrollCycleId,
                                UserId = model.UserId,
                                Salary = users.Salary,
                                Addition = 0,
                                Deduction = 0,
                                NetSalary = 0,
                                IsActive = true,
                                CreationDate = DateTime.Now,
                                UpdateDate = null,
                                UpdateBy = null,
                                UserIp = Request.UserHostAddress
                            };
                            payrollEntry = objPayrollService.InsertPayroll(payrollEntry);
                            foreach (PayrollPolicy policy in PayrollPolicyList)
                            {
                                var PayrollVariableList = objPayrollVariableService.GetPayrollVariable(policy.Id);
                                if (policy.PayrollVariableId == 5)/*Early Day*/
                                {
                                    Amount = 0;

                                }
                                if (policy.PayrollVariableId == 6)/*Late Day*/
                                {
                                    Amount = 0;
                                }
                                if (policy.PayrollVariableId == 7)/*Absent*/
                                {
                                    Amount = 0;
                                }
                                PayrollDetail PayrollDetailEntry = new PayrollDetail()
                                {
                                    PayrollId = payrollEntry.Id,
                                    PayrollPolicyId = policy.Id,
                                    Amount = Amount,
                                    IsActive = true,
                                    CreationDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = AuthBase.UserId,
                                    UserIp = Request.UserHostAddress,
                                    PayrollPolicyName = PayrollVariableList.Name
                                };
                                PayrollDetailEntry = objPayrollDetailService.InsertPayrollDetail(PayrollDetailEntry);
                            }
                            #region 3.3 Payroll table update
                            List<PayrollDetail> userPayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId(payrollEntry.Id);
                            payrollEntry.Addition = userPayrollDetailList.Where(x => x.Amount > 0).Sum(x => x.Amount);
                            payrollEntry.Deduction = -(userPayrollDetailList.Where(x => x.Amount < 0).Sum(x => x.Amount));
                            payrollEntry.NetSalary = payrollEntry.Salary + payrollEntry.Addition - payrollEntry.Deduction;
                            payrollEntry.IsActive = true;
                            payrollEntry = objPayrollService.UpdatePayroll(payrollEntry);
                            #endregion 3.3 Payroll table update
                        }
                    }
                    else
                        ModelState.AddModelError("", "Payroll already made of an employee");
                }
                else
                {
                    ModelState.AddModelError("", "Please select Employee and Payroll both");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetPayrollCycleAndUser(model.PayrollCycleId, model.UserId);
            return View(model);
        }
        //New Generater Payroll
        [HttpGet]
        [Authenticate]
        public ActionResult GeneratePayroll()
        {
            InsertPayrolCycle();
            VMAttendanceModel model = new VMAttendanceModel();

            GetPayrollCycle(model.PayrollCycleId); //forPayrollCycleDropDown

            return View(model);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult GeneratePayroll(VMAttendanceModel model)
        {
            try { 
            if(model.PayrollCycleId == null)
            {
                ModelState.AddModelError("", "Please Select Payroll");
            }
            else { 
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            IShiftOffDayService objShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
            IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
            ILeaveService objLeaveService = IoC.Resolve<ILeaveService>("LeaveService");
            ITicketService objTicketService = IoC.Resolve<ITicketService>("TicketService");
            #region 1- Get all Employees not resigned/terminated within Payroll Cycle Date
                    //get PayrollCycleId
                    int payrollCycleId = Convert.ToInt32(model.PayrollCycleId);
            model.PayrollCycle = objPayrollCycleService.GetPayrollCycle(payrollCycleId);
            //Get PayrollCycleDate
            DateTime payrollStartDate = Convert.ToDateTime(model.PayrollCycle.Year + "-" + model.PayrollCycle.Month + "-1");
            DateTime payrollEndDate = payrollStartDate.AddMonths(1).AddDays(-1);
            //Get All Users within payroll Cycle Date
            List<User> UserList = objUserService.GetAllUser().Where(x => x.IsActive == true).ToList();
            #endregion
            //Get PayrollPolicy
            #region 2.Get PayrollPolicy
            List<PayrollPolicy> PayrollPolicyList = objPayrollPolicyService.GetAllPayrollPolicy().Where(x => x.EffectiveDate <= payrollStartDate && (x.RetiredDate == null || x.RetiredDate >= payrollEndDate)).ToList();
            #endregion
                        foreach (User users in UserList)
                        {
                            if (users.UserTypeId != 1) //Start of UserTypeIdCondtion
                            {
                            #region Check User Payroll already exist, if exist then delete his/her Payroll
                            List<Payroll> PayrollLists = objPayrollService.GetAllPayroll().Where(x => x.UserId == users.Id && x.PayrollCycleId == model.PayrollCycleId).ToList();
                                if (PayrollLists.Count > 0)
                                {
                                    foreach (Payroll p in PayrollLists)
                                    {
                                        objPayrollService.DeletePayroll(Convert.ToInt32(p.Id));
                                        List<PayrollDetail> pdList = objPayrollDetailService.GetPayrollDetailByPayrollId(p.Id).ToList();
                                        if (pdList.Count > 0)
                                        {
                                            foreach (PayrollDetail pd in pdList)
                                            {
                                                objPayrollDetailService.DeletePayrollDetail(pd.Id);
                                            }
                                        }
                                    }
                                }
                            #endregion Check User Payroll already exist, if exist then delete his/her Payroll
                            List<Payroll> PayrollListss = objPayrollService.GetAllPayroll().Where(x => x.UserId == users.Id && x.PayrollCycleId == model.PayrollCycleId).ToList();
                            if (PayrollListss.Count == 0)
                            {
                                //If AttendanceList is not Null
                                List<Attendance> attendanceList = objAttendanceService.GetAttendanceByUserIdAndDateRange(payrollStartDate, payrollEndDate, users.Id);
                                int TicketCount = 0;
                                List<Ticket> tickets = objTicketService.GetTicketByUserIdAndDateRange(users.Id,payrollStartDate,payrollEndDate).Where(x => x.IsApproved == true).ToList();
                                if (attendanceList != null)
                                {
                                    List<AttendanceAndAttendanceStatusViewList> attendances = objAttendanceService.GetAttendanceAndAttendanceStatusByUserIdAndDateRangeList(payrollStartDate, payrollEndDate, users.Id);
                                    List<AttendanceStatus> AttendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByDateRangeSharp(payrollStartDate, payrollEndDate);
                                    #region 3.1	Insert 0 value entry in payroll 
                                    decimal Salary = 0;

                                    if (users.SalaryTypeId == 1 /*Monthly*/)
                                    {
                                        Salary = users.Salary.Value;
                                    }
                                    else if (users.SalaryTypeId == 2 /*weekly*/)
                                    {
                                        Salary = Convert.ToDecimal(Convert.ToDouble(users.Salary.Value) * 4.29);
                                    }
                                    else if (users.SalaryTypeId == 3 /*days*/)
                                    {
                                        Salary = users.Salary.Value * 30;
                                    }
                                    Payroll PayrollEntry = null;
                                    PayrollEntry = new Payroll()
                                    {
                                        PayrollCycleId = model.PayrollCycleId,
                                        UserId = users.Id,
                                        Salary = Salary,
                                        Addition = null,
                                        Deduction = null,
                                        NetSalary = null,
                                        IsActive = false,
                                        CreationDate = DateTime.Now,
                                        UpdateDate = DateTime.Now,
                                        UpdateBy = AuthBase.UserId,
                                        UserIp = Request.UserHostAddress
                                    };
                                    PayrollEntry = objPayrollService.InsertPayroll(PayrollEntry);
                                    #endregion
                                    #region Finding joining Date of user and minus salary from Payroll
                                    DateTime UserJoiningDateDay;
                                    DateTime PayrollStartDateDay;
                                    int PayrollStartDateDays = 0;
                                    int UserJoiningDateDays = 0;
                                    decimal minusByJoiningDate = 0;
                                    DateTime AtDateYearr = Convert.ToDateTime(payrollStartDate);
                                    int AtDateYearss = (int)AtDateYearr.Year;
                                    DateTime AtDateMonthh = Convert.ToDateTime(payrollStartDate);
                                    int AtDateMonthss = (int)AtDateMonthh.Month;
                                    int daysInMonths = DateTime.DaysInMonth(AtDateYearss, AtDateMonthss);
                                    if (payrollStartDate < users.JoiningDate)
                                    {
                                        UserJoiningDateDay = Convert.ToDateTime(users.JoiningDate);
                                        UserJoiningDateDays = (int)UserJoiningDateDay.Day;
                                        PayrollStartDateDay = Convert.ToDateTime(payrollStartDate);
                                        PayrollStartDateDays = (int)PayrollStartDateDay.Day;
                                        if (UserJoiningDateDays > PayrollStartDateDays)
                                        {
                                            minusByJoiningDate = UserJoiningDateDays - PayrollStartDateDays;
                                            minusByJoiningDate = Math.Floor(Convert.ToInt32(users.Salary.Value / daysInMonths) * minusByJoiningDate);
                                        }
                                    }
                                    #endregion Finding joining Date of user and minus salary from Payroll
                                    #region 3.2	Loop Payroll Policy and PayrollDetail Entry   
                                    decimal Amount = 0;
                                    int ApprovedLeavesCount = 0;
                                    int ApprovedLeaveEarlyCount = 0;
                                    int ApprovedLeaveLateCount = 0;
                                    #region Check User Leave 
                                    List<Leave> LeaveLists = objLeaveService.GetLeaveByUserIdAndDateRange(users.Id, payrollStartDate, payrollEndDate).Where(x=> x.IsApproved == true).ToList();
                                    //List<Leave> LeaveLists = objLeaveService.GetLeaveByUserId(users.Id).Where(x => x.Date >= payrollStartDate && x.Date <= payrollEndDate && x.IsApproved == true && x.IsReject == false).ToList();
                                    if (LeaveLists.Count> 0)
                                    {
                                        ApprovedLeavesCount = LeaveLists.Count;
                                        foreach (Leave lv in LeaveLists)
                                        {
                                            List<Attendance> FirstATIdList = objAttendanceService.GetAttendanceByUserIDAndDate(users.Id, Convert.ToDateTime(lv.Date)).ToList();
                                            Attendance FirstATId = FirstATIdList.FirstOrDefault();
                                            if (FirstATId != null && FirstATId.DateTimeIn != null)
                                            {
                                                List<AttendanceStatus> atsList = null;
                                                atsList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(Convert.ToInt32(FirstATId.Id)).ToList();
                                                if (atsList != null)
                                                {
                                                    AttendanceStatus ats = atsList.FirstOrDefault();
                                                    if (ats != null && ats.IsEarly == true)
                                                    {
                                                        ApprovedLeaveEarlyCount++;
                                                    }
                                                    else if (ats != null && ats.IsLate == true)
                                                    {
                                                        ApprovedLeaveLateCount++;
                                                    }
                                                }
                                            }

                                        }

                                    }
                                    #endregion Check User Leave
                                    foreach (PayrollPolicy policy in PayrollPolicyList)
                                    {
                                        var PayrollVariableList = objPayrollVariableService.GetPayrollVariable(policy.Id);
                                        #region IsAttendance=true
                                        if (policy.IsAttendance == true)
                                        {
                                            #region 3.2.1.1	if AMOUNT - Insert in payroll detail
                                            #region IsUnit=true
                                            if (policy.IsUnit == true)
                                            {
                                                //int AmountCount =0;
                                                if (policy.PayrollVariableId == 1)/*Full Day*/
                                                {
                                                    int count = AttendanceStatusList.Where(x => x.IsFullDay == true && x.IsShiftOffDay == false).Count();
                                                    if (count >= policy.Occurance)
                                                    {
                                                        Amount = 0;
                                                        decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                                        decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                                        Amount = Convert.ToInt32(AmountUnit);
                                                    }
                                                }
                                                if (policy.PayrollVariableId == 3)/*Half Day*/
                                                {
                                                    int count = AttendanceStatusList.Where(x => x.IsHalfDay == true && x.IsShiftOffDay == false).Count();
                                                    if (count >= policy.Occurance)
                                                    {
                                                        Amount = 0;
                                                        decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                                        decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                                        Amount = Convert.ToInt32(AmountUnit);
                                                    }
                                                }
                                                if (policy.PayrollVariableId == 4)/*Quarter Day*/
                                                {
                                                    int count = AttendanceStatusList.Where(x => x.IsQuarterDay == true && x.IsShiftOffDay == false).Count();
                                                    if (count >= policy.Occurance)
                                                    {
                                                        Amount = 0;
                                                        decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                                        decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                                        Amount = Convert.ToInt32(AmountUnit);
                                                    }
                                                }
                                                if (policy.PayrollVariableId == 5)/*Early*/
                                                {
                                                    int count = AttendanceStatusList.Where(x => x.IsEarly == true && x.IsShiftOffDay == false).Count();
                                                    if (count >= policy.Occurance)
                                                    {
                                                        Amount = 0;
                                                        decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                                        decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                                        Amount = Convert.ToInt32(AmountUnit);
                                                    }
                                                }

                                                if (policy.PayrollVariableId == 6)/*Late*/
                                                {
                                                    int count = AttendanceStatusList.Where(x => x.IsLate == true && x.IsShiftOffDay == false).Count();
                                                    if (count >= policy.Occurance)
                                                    {
                                                        Amount = 0;
                                                        decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                                        decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                                        Amount = Convert.ToInt32(AmountUnit);
                                                    }
                                                }
                                                if (policy.PayrollVariableId == 7)/*Absent*/
                                                {
                                                    int AbsentDaysCount = 0;
                                                    //Get All Attendance of PAyrollMonth
                                                    //List<Attendance> attendanceList = objAttendanceService.GetAttendanceByDateRange(payrollStartDate, payrollEndDate);
                                                    Amount = 0;
                                                    //Start of finding effective date of user  and minus in Salary
                                                    DateTime payrollStartDatee;
                                                    DateTime UserShiftEffectiveDate;
                                                    DateTime userEffectiveDate;
                                                    int payrollStartDateDay = 0;
                                                    int userEffectiveDateDay = 0;
                                                    List<Shift> shifts = objShiftService.GetAllShift();
                                                    List<UserShift> UserShifts = objUserShiftService.GetUserShiftByUserId(users.Id);
                                                    if (UserShifts != null && UserShifts.Count > 0)
                                                    {
                                                        UserShifts = UserShifts.Where(x => x.UserId == users.Id && x.EffectiveDate >= payrollStartDate).ToList();
                                                        UserShift usershift = UserShifts.FirstOrDefault();
                                                        if (usershift != null)
                                                        {
                                                            if (usershift.EffectiveDate > payrollStartDate)
                                                            {
                                                                payrollStartDatee = Convert.ToDateTime(payrollStartDate);
                                                                payrollStartDateDay = (int)payrollStartDatee.Day;
                                                                UserShiftEffectiveDate = Convert.ToDateTime(usershift.EffectiveDate);
                                                                userEffectiveDate = Convert.ToDateTime(UserShiftEffectiveDate);
                                                                userEffectiveDateDay = (int)userEffectiveDate.Day;
                                                                DateTime date = Convert.ToDateTime(usershift.EffectiveDate - payrollStartDate);
                                                            }
                                                        }
                                                    }
                                                    int minusDays = 0;
                                                    decimal EffectiveSalary = 0;
                                                    foreach (Attendance at in attendanceList)
                                                    {
                                                        DateTime AtDateYear = Convert.ToDateTime(at.Date);
                                                        int AtDateYears = (int)AtDateYear.Year;
                                                        DateTime AtDateMonth = Convert.ToDateTime(at.Date);
                                                        int AtDateMonths = (int)AtDateMonth.Month;
                                                        int daysInMonth = DateTime.DaysInMonth(AtDateYears, AtDateMonths);
                                                        if (userEffectiveDateDay > payrollStartDateDay)
                                                        {
                                                            minusDays = userEffectiveDateDay - payrollStartDateDay;
                                                            EffectiveSalary = Math.Floor(users.Salary.Value / daysInMonth * minusDays);
                                                        }

                                                        //End of finding effective date of user and minus in Salary
                                                        List<Shift> ShiftList = objShiftService.GetAllShift();
                                                        List<UserShift> UserShiftList = objUserShiftService.GetUserShiftByUserId(users.Id);
                                                        if (UserShiftList != null && UserShiftList.Count > 0)
                                                        {
                                                            UserShiftList = UserShiftList.Where(x => x.UserId == users.Id && x.EffectiveDate <= at.Date && (x.RetiredDate == null || x.RetiredDate >= at.Date)).ToList();
                                                            if (UserShiftList != null && UserShiftList.Count > 0)
                                                            {
                                                                UserShift userShift = UserShiftList.FirstOrDefault();
                                                                List<ShiftOffDay> ShiftOffDayList = objShiftOffDayService.GetShiftOffDayByShiftId(userShift.ShiftId);
                                                                int countAbsent = AttendanceStatusList.Where(x => x.AttendanceId == at.Id).Count();
                                                                if (countAbsent == 0)
                                                                {
                                                                    int OffDay = 0;
                                                                    if (PayrollEntry.UserId == at.UserId)
                                                                    {
                                                                        foreach (ShiftOffDay OffDays in ShiftOffDayList)
                                                                        {
                                                                            var attendanceDate = Convert.ToDateTime(at.Date);
                                                                            int dayNumberOfWeek = (int)attendanceDate.DayOfWeek;
                                                                            if (OffDays.OffDayOfWeek == dayNumberOfWeek)
                                                                            {
                                                                                OffDay++;
                                                                            }
                                                                        }
                                                                        if (OffDay == 0)
                                                                        {
                                                                            AbsentDaysCount++;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            Amount = ((Convert.ToInt32(Salary) / daysInMonth) * AbsentDaysCount * Convert.ToInt32(policy.Value) - Convert.ToInt32(EffectiveSalary));
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion IsUnit=true
                                            #region 3.2.1.2 ISPercentage=true
                                            else if (policy.IsPercentage == true)
                                            {
                                                //pending
                                            }
                                            #endregion 3.2.1.2 ISPercentage=true
                                            #region 3.2.1.2 if ISDay=true
                                            else if (policy.IsDay == true)
                                            {
                                                Amount = 0;
                                                
                                                if (policy.PayrollVariableId == 5)/*Early Day*/
                                                {
                                                    TicketCount = 0;
                                                    int count = attendances.Where(x => x.IsEarly == true && x.IsShiftOffDay == false).Count();
                                                    if(tickets.Count>0)
                                                    {
                                                        foreach(Ticket t in tickets)
                                                        {
                                                            foreach(AttendanceAndAttendanceStatusViewList attendance in attendances.Where(x => x.IsEarly == true && x.IsShiftOffDay == false))
                                                            {
                                                                if (t.AttendanceID == attendance.AttendanceID)
                                                                    TicketCount++;
                                                            }
                                                           
                                                        }
                                                    }
                                                    count = count - ApprovedLeaveEarlyCount - TicketCount;
                                                    if (count >= policy.Occurance)
                                                    {
                                                        decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                                        int Value = Math.Abs(Convert.ToInt32(policy.Value));
                                                        Amount = Math.Floor(Salary / daysInMonths * count2 * Convert.ToInt32(policy.Value));
                                                    }
                                                }
                                                if (policy.PayrollVariableId == 6)/*Late Day*/
                                                {
                                                    TicketCount = 0;
                                                    int count = attendances.Where(x => x.IsLate == true && x.IsShiftOffDay == false).Count();
                                                    if (tickets.Count > 0)
                                                    {
                                                        foreach (Ticket t in tickets)
                                                        {
                                                            foreach (AttendanceAndAttendanceStatusViewList attendance in attendances.Where(x => x.IsLate == true && x.IsShiftOffDay == false))
                                                            {
                                                                if (t.AttendanceID == attendance.AttendanceID)
                                                                    TicketCount++;
                                                            }

                                                        }
                                                    }
                                                    count = count - ApprovedLeaveLateCount - TicketCount;
                                                    if (count >= policy.Occurance)
                                                    {
                                                        decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                                        int Value = Math.Abs(Convert.ToInt32(policy.Value));
                                                        Amount = Math.Floor(Salary / daysInMonths * count2 * Convert.ToInt32(policy.Value));
                                                    }
                                                }
                                                if (policy.PayrollVariableId == 7)/*Absent*/
                                                {
                                                    int AbsentDaysCount = 0;
                                                    Amount = 0;
                                                    Leave leave = null;
                                                    foreach (Attendance at in attendanceList)
                                                    {
                                                        if (LeaveLists != null) //Exception
                                                        {
                                                             leave = LeaveLists.Where(x => x.Date == at.Date && x.IsApproved == true).FirstOrDefault();
                                                        }
                                                         if(leave == null)
                                                        { 
                                                        List<Shift> ShiftList = objShiftService.GetAllShift(); //GetAllShifts
                                                        List<UserShift> UserShiftList = objUserShiftService.GetUserShiftByUserId(users.Id); //GetUserShiftByID
                                                        if (UserShiftList != null && UserShiftList.Count > 0)
                                                        {
                                                            UserShiftList = UserShiftList.Where(x => x.UserId == users.Id && x.EffectiveDate <= at.Date && (x.RetiredDate == null || x.RetiredDate >= at.Date)).ToList(); //GetUserShiftByEffectiveDate
                                                            if (UserShiftList != null && UserShiftList.Count > 0)
                                                            {
                                                                UserShift userShift = UserShiftList.FirstOrDefault();
                                                                Amount = Math.Floor((Salary / daysInMonths) * AbsentDaysCount * Convert.ToInt32(policy.Value));
                                                                List<ShiftOffDay> ShiftOffDayList = objShiftOffDayService.GetShiftOffDayByShiftId(userShift.ShiftId); //GetShiftOffDayList of User
                                                                int countAbsent = AttendanceStatusList.Where(x => x.AttendanceId == at.Id).Count(); //CountAbsent of user in Payroll
                                                                if (countAbsent == 0)
                                                                {
                                                                    int OffDay = 0;
                                                                    if (PayrollEntry.UserId == at.UserId) //Chcek PayrollUserId and AttendanceUserId is same or not
                                                                    {
                                                                        foreach (ShiftOffDay OffDays in ShiftOffDayList)
                                                                        {
                                                                            var attendanceDate = Convert.ToDateTime(at.Date);
                                                                            int dayNumberOfWeek = (int)attendanceDate.DayOfWeek; //GetAttendanceDateDay in number
                                                                            if (OffDays.OffDayOfWeek == dayNumberOfWeek) //Compare with ShiftOffDay with AttendanceDateDay
                                                                            {
                                                                                OffDay++;
                                                                            }
                                                                        }

                                                                        if (OffDay == 0)
                                                                        {
                                                                            AbsentDaysCount++;
                                                                        }
                                                                    }
                                                                }
                                                              }
                                                            }        
                                                        }
                                                    }
                                                    Amount = Math.Floor((Convert.ToInt32(Salary) / daysInMonths) * AbsentDaysCount  * Convert.ToInt32(policy.Value) - minusByJoiningDate);
                                                }
                                            }
                                            #endregion 3.2.1.2 if ISDay=true
                                            #endregion
                                            PayrollDetail PayrollDetailEntry = new PayrollDetail()
                                            {
                                                PayrollId = PayrollEntry.Id,
                                                PayrollPolicyId = policy.Id,
                                                Amount = Amount,
                                                IsActive = true,
                                                CreationDate = DateTime.Now,
                                                UpdateDate = DateTime.Now,
                                                UpdateBy = AuthBase.UserId,
                                                UserIp = Request.UserHostAddress,
                                                PayrollPolicyName = PayrollVariableList.Name
                                            };
                                            PayrollDetailEntry = objPayrollDetailService.InsertPayrollDetail(PayrollDetailEntry);

                                            //GetAttendanceByID
                                            //CheckIn AttendanceStatus , AttendanceID exist or not
                                            //If Exist Apply normal function
                                            //If not Apply Absent Variable in payroll Detail
                                        }
                                        #endregion IsAttendance=true
                                        //Checking in AttendnaceStatus table whether AttendanceId is exist or not
                                        //if not then mark absent variable to particular PayrollEntry
                                        //#region 3.2.1 IsAttendance== false
                                        //else if (policy.IsAttendance == false)
                                        //{
                                        //    #region 3.2.2.1	if AMOUNT
                                        //    if (policy.IsUnit == true)
                                        //    {
                                        //        if (policy.IsUnit == true)
                                        //        {
                                        //            //int AmountCount =0;
                                        //            if (policy.PayrollVariableId == 9)/*Transport*/
                                        //            {
                                        //                Amount = 0;
                                        //                Amount = (decimal)policy.Value;
                                        //            }
                                        //        }
                                        //    }
                                        //    #endregion
                                        //    //#region 3.2.2.2	if PERCENTAGE
                                        //    //else if (policy.IsPercentage == true)
                                        //    //{
                                        //    //    //pending
                                        //    //}
                                        //    //#endregion
                                        //    //#region if DAY, Check variable if Late, Count Is Late in Attendance Status / Occurance --> Get Integer
                                        //    //else if (policy.IsDay == true)
                                        //    //{
                                        //    //    Amount = 0;
                                        //    //    if (policy.PayrollVariableId == 1)/*Full Day*/
                                        //    //    {
                                        //    //        int count = AttendanceStatusList.Where(x => x.IsFullDay == true && x.IsShiftOffDay == false).Count();
                                        //    //        if (count >= policy.Occurance)
                                        //    //        {
                                        //    //            Amount = 0;
                                        //    //            decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                        //    //            decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                        //    //            Amount = Convert.ToInt32(AmountUnit);
                                        //    //        }
                                        //    //    }
                                        //    //    if (policy.PayrollVariableId == 3)/*Half Day*/
                                        //    //    {
                                        //    //        int count = AttendanceStatusList.Where(x => x.IsHalfDay == true && x.IsShiftOffDay == false).Count();
                                        //    //        if (count >= policy.Occurance)
                                        //    //        {
                                        //    //            Amount = 0;
                                        //    //            decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                        //    //            decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                        //    //            Amount = Convert.ToInt32(AmountUnit);
                                        //    //        }
                                        //    //    }
                                        //    //    if (policy.PayrollVariableId == 4)/*Quarter Day*/
                                        //    //    {
                                        //    //        int count = AttendanceStatusList.Where(x => x.IsQuarterDay == true && x.IsShiftOffDay == false).Count();
                                        //    //        if (count >= policy.Occurance)
                                        //    //        {
                                        //    //            Amount = 0;
                                        //    //            decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                        //    //            decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                        //    //            Amount = Convert.ToInt32(AmountUnit);
                                        //    //        }
                                        //    //    }
                                        //    //    if (policy.PayrollVariableId == 5)/*Early*/
                                        //    //    {
                                        //    //        int count = AttendanceStatusList.Where(x => x.IsEarly == true && x.IsShiftOffDay == false).Count();
                                        //    //        if (count >= policy.Occurance)
                                        //    //        {
                                        //    //            Amount = 0;
                                        //    //            decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                        //    //            decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                        //    //            Amount = Convert.ToInt32(AmountUnit);
                                        //    //        }
                                        //    //    }

                                        //    //    if (policy.PayrollVariableId == 6)/*Late*/
                                        //    //    {
                                        //    //        int count = AttendanceStatusList.Where(x => x.IsLate == true && x.IsShiftOffDay == false).Count();
                                        //    //        if (count >= policy.Occurance)
                                        //    //        {
                                        //    //            Amount = 0;
                                        //    //            decimal count2 = Math.Floor(count / Convert.ToDecimal(policy.Occurance));
                                        //    //            decimal AmountUnit = Math.Floor(Convert.ToDecimal(policy.Value * count2));
                                        //    //            Amount = Convert.ToInt32(AmountUnit);
                                        //    //        }
                                        //    //    }
                                        //    //    if (policy.PayrollVariableId == 7)/*Absent*/
                                        //    //    {
                                        //    //        int AbsentDaysCount = 0;
                                        //    //        //Get All Attendance of PAyrollMonth
                                        //    //        //List<Attendance> attendanceList = objAttendanceService.GetAttendanceByDateRange(payrollStartDate, payrollEndDate);
                                        //    //        Amount = 0;
                                        //    //        //Start of finding effective date of user  and minus in Salary
                                        //    //        DateTime payrollStartDatee;
                                        //    //        DateTime UserShiftEffectiveDate;
                                        //    //        DateTime userEffectiveDate;
                                        //    //        int payrollStartDateDay = 0;
                                        //    //        int userEffectiveDateDay = 0;
                                        //    //        List<Shift> shifts = objShiftService.GetAllShift();
                                        //    //        List<UserShift> UserShifts = objUserShiftService.GetUserShiftByUserId(users.Id);
                                        //    //        if (UserShifts != null && UserShifts.Count > 0)
                                        //    //        {
                                        //    //            UserShifts = UserShifts.Where(x => x.UserId == users.Id && x.EffectiveDate >= payrollStartDate).ToList();
                                        //    //            UserShift usershift = UserShifts.FirstOrDefault();
                                        //    //            if (usershift != null)
                                        //    //            {
                                        //    //                if (usershift.EffectiveDate > payrollStartDate)
                                        //    //                {
                                        //    //                    payrollStartDatee = Convert.ToDateTime(payrollStartDate);
                                        //    //                    payrollStartDateDay = (int)payrollStartDatee.Day;
                                        //    //                    UserShiftEffectiveDate = Convert.ToDateTime(usershift.EffectiveDate);
                                        //    //                    userEffectiveDate = Convert.ToDateTime(UserShiftEffectiveDate);
                                        //    //                    userEffectiveDateDay = (int)userEffectiveDate.Day;
                                        //    //                    //DateTime date = Convert.ToDateTime(usershift.EffectiveDate - payrollStartDate);
                                        //    //                }
                                        //    //            }
                                        //    //        }
                                        //    //        int minusDays = 0;
                                        //    //        decimal EffectiveSalary = 0;
                                        //    //        foreach (Attendance at in attendanceList)
                                        //    //        {
                                        //    //            DateTime AtDateYear = Convert.ToDateTime(at.Date);
                                        //    //            int AtDateYears = (int)AtDateYear.Year;
                                        //    //            DateTime AtDateMonth = Convert.ToDateTime(at.Date);
                                        //    //            int AtDateMonths = (int)AtDateMonth.Month;
                                        //    //            int daysInMonth = DateTime.DaysInMonth(AtDateYears, AtDateMonths);
                                        //    //            if (userEffectiveDateDay > payrollStartDateDay)
                                        //    //            {
                                        //    //                minusDays = userEffectiveDateDay - payrollStartDateDay;
                                        //    //                EffectiveSalary = users.Salary.Value / daysInMonth * minusDays;
                                        //    //            }

                                        //    //            //End of finding effective date of user and minus in Salary
                                        //    //            List<Shift> ShiftList = objShiftService.GetAllShift();
                                        //    //            List<UserShift> UserShiftList = objUserShiftService.GetUserShiftByUserId(users.Id);
                                        //    //            if (UserShiftList != null && UserShiftList.Count > 0)
                                        //    //            {
                                        //    //                UserShiftList = UserShiftList.Where(x => x.UserId == users.Id && x.EffectiveDate <= at.Date && (x.RetiredDate == null || x.RetiredDate >= at.Date)).ToList();
                                        //    //                if (UserShiftList != null && UserShiftList.Count > 0)
                                        //    //                {
                                        //    //                    UserShift userShift = UserShiftList.FirstOrDefault();
                                        //    //                    List<ShiftOffDay> ShiftOffDayList = objShiftOffDayService.GetShiftOffDayByShiftId(userShift.ShiftId);
                                        //    //                    int countAbsent = AttendanceStatusList.Where(x => x.AttendanceId == at.Id).Count();
                                        //    //                    if (countAbsent == 0)
                                        //    //                    {
                                        //    //                        int OffDay = 0;
                                        //    //                        if (PayrollEntry.UserId == at.UserId)
                                        //    //                        {
                                        //    //                            foreach (ShiftOffDay OffDays in ShiftOffDayList)
                                        //    //                            {
                                        //    //                                var attendanceDate = Convert.ToDateTime(at.Date);
                                        //    //                                int dayNumberOfWeek = (int)attendanceDate.DayOfWeek;
                                        //    //                                if (OffDays.OffDayOfWeek == dayNumberOfWeek)
                                        //    //                                {
                                        //    //                                    OffDay++;
                                        //    //                                }
                                        //    //                            }
                                        //    //                            if (OffDay == 0)
                                        //    //                            {
                                        //    //                                AbsentDaysCount++;
                                        //    //                            }
                                        //    //                        }
                                        //    //                    }
                                        //    //                }
                                        //    //                Amount = (Convert.ToInt32(Salary) / daysInMonth) * AbsentDaysCount * Convert.ToInt32(policy.Value) - Convert.ToInt32(EffectiveSalary);
                                        //    //            }
                                        //    //        }
                                        //    //    }
                                        //    //    //Insert in payrollDetail
                                        //    //    PayrollDetail PayrollDetailEntry = new PayrollDetail()
                                        //    //    {
                                        //    //        PayrollId = PayrollEntry.Id,
                                        //    //        PayrollPolicyId = policy.Id,
                                        //    //        Amount = Amount,
                                        //    //        IsActive = true,
                                        //    //        CreationDate = DateTime.Now,
                                        //    //        UpdateDate = DateTime.Now,
                                        //    //        UpdateBy = AuthBase.UserId,
                                        //    //        UserIp = Request.UserHostAddress,
                                        //    //        PayrollPolicyName = PayrollVariableList.Name
                                        //    //    };
                                        //    //    PayrollDetailEntry = objPayrollDetailService.InsertPayrollDetail(PayrollDetailEntry);
                                        //    //}
                                        //    //#endregion
                                        //}
                                        //#endregion 3.2.1 IsAttendance== false
                                        //PayrollDetail PayrollDetailEntry1 = new PayrollDetail()
                                        //{
                                        //    PayrollId = PayrollEntry.Id,
                                        //    PayrollPolicyId = policy.Id,
                                        //    Amount = Amount,
                                        //    IsActive = true,
                                        //    CreationDate = DateTime.Now,
                                        //    UpdateDate = DateTime.Now,
                                        //    UpdateBy = AuthBase.UserId,
                                        //    UserIp = Request.UserHostAddress,
                                        //    PayrollPolicyName = PayrollVariableList.Name
                                        //};
                                        //PayrollDetailEntry1 = objPayrollDetailService.InsertPayrollDetail(PayrollDetailEntry1);
                                        //End loop PayrollPolicy
                                        #endregion 3.2	Loop Payroll Policy and PayrollDetail Entry  
                                    #region 3.3 Payroll table update


                                    }
                                    List<PayrollDetail> userPayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId(PayrollEntry.Id);
                                    //Amount Try
                                    PayrollEntry.Addition = userPayrollDetailList.Where(x => x.Amount > 0).Sum(x => x.Amount);
                                    PayrollEntry.Deduction = -(userPayrollDetailList.Where(x => x.Amount < 0).Sum(x => x.Amount));
                                    PayrollEntry.NetSalary = PayrollEntry.Salary + PayrollEntry.Addition - PayrollEntry.Deduction;
                                    PayrollEntry.IsActive = true;
                                    PayrollEntry = objPayrollService.UpdatePayroll(PayrollEntry);
                                    #endregion 3.3 Payroll table update
                                }
                                #region Payroll when attendanceList is null
                                else if (attendanceList == null)
                                {
                                    #region 3.1	Insert 0 value entry in payroll 
                                    decimal Salary = 0;

                                    if (users.SalaryTypeId == 1 /*Monthly*/)
                                    {
                                        Salary = users.Salary.Value;
                                    }
                                    else if (users.SalaryTypeId == 2 /*weekly*/)
                                    {
                                        Salary = Convert.ToDecimal(Convert.ToDouble(users.Salary.Value) * 4.29);
                                    }
                                    else if (users.SalaryTypeId == 3 /*days*/)
                                    {
                                        Salary = users.Salary.Value * 30;
                                    }
                                    Payroll PayrollEntry = null;
                                    PayrollEntry = new Payroll()
                                    {
                                        PayrollCycleId = model.PayrollCycleId,
                                        UserId = users.Id,
                                        Salary = Salary,
                                        Addition = null,
                                        Deduction = null,
                                        NetSalary = null,
                                        IsActive = false,
                                        CreationDate = DateTime.Now,
                                        UpdateDate = DateTime.Now,
                                        UpdateBy = AuthBase.UserId,
                                        UserIp = Request.UserHostAddress
                                    };
                                    PayrollEntry = objPayrollService.InsertPayroll(PayrollEntry);
                                    #endregion 3.1	Insert 0 value entry in payrol
                                    #region Finding joining Date of user and minus salary from Payroll
                                    DateTime UserJoiningDateDay;
                                    DateTime PayrollStartDateDay;
                                    int PayrollStartDateDays = 0;
                                    int UserJoiningDateDays = 0;
                                    decimal minusByJoiningDate = 0;
                                    DateTime AtDateYearr = Convert.ToDateTime(payrollStartDate);
                                    int AtDateYearss = (int)AtDateYearr.Year;
                                    DateTime AtDateMonthh = Convert.ToDateTime(payrollStartDate);
                                    int AtDateMonthss = (int)AtDateMonthh.Month;
                                    int daysInMonths = DateTime.DaysInMonth(AtDateYearss, AtDateMonthss);
                                    if (payrollStartDate < users.JoiningDate)
                                    {
                                        UserJoiningDateDay = Convert.ToDateTime(users.JoiningDate);
                                        UserJoiningDateDays = (int)UserJoiningDateDay.Day;
                                        PayrollStartDateDay = Convert.ToDateTime(payrollStartDate);
                                        PayrollStartDateDays = (int)PayrollStartDateDay.Day;
                                        if (UserJoiningDateDays > PayrollStartDateDays)
                                        {
                                            minusByJoiningDate = UserJoiningDateDays - PayrollStartDateDays;
                                            minusByJoiningDate = Math.Floor(Convert.ToInt32(users.Salary.Value / daysInMonths) * minusByJoiningDate);
                                        }
                                    }
                                    #endregion Finding joining Date of user and minus salary from Payroll
                                    #region 3.2	Loop Payroll Policy and PayrollDetail Entry   
                                    decimal Amount = 0;
                                    foreach (PayrollPolicy policy in PayrollPolicyList)
                                    {

                                        var PayrollVariableList = objPayrollVariableService.GetPayrollVariable(policy.Id);
                                        #region IsAttendance=true
                                        if (policy.IsAttendance == true)
                                        {
                                            #region 3.2.1.2 if ISDay=true
                                            if (policy.IsDay == true)
                                            {
                                                Amount = 0;
                                                if (policy.PayrollVariableId == 7) /*Absent*/
                                                {
                                                    Amount = Salary * (Convert.ToInt32(policy.Value) - minusByJoiningDate);
                                                }
                                            }
                                            #endregion 3.2.1.2 if ISDay=true
                                            #endregion
                                            PayrollDetail PayrollDetailEntry = new PayrollDetail()
                                            {
                                                PayrollId = PayrollEntry.Id,
                                                PayrollPolicyId = policy.Id,
                                                Amount = Amount,
                                                IsActive = true,
                                                CreationDate = DateTime.Now,
                                                UpdateDate = DateTime.Now,
                                                UpdateBy = AuthBase.UserId,
                                                UserIp = Request.UserHostAddress,
                                                PayrollPolicyName = PayrollVariableList.Name
                                            };
                                            PayrollDetailEntry = objPayrollDetailService.InsertPayrollDetail(PayrollDetailEntry);

                                        }
                                        #endregion IsAttendance=true
                                    }
                                    #region 3.3 Payroll table update
                                    List<PayrollDetail> userPayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId(PayrollEntry.Id);
                                    //Amount Try
                                    PayrollEntry.Addition = userPayrollDetailList.Where(x => x.Amount > 0).Sum(x => x.Amount);
                                    PayrollEntry.Deduction = -(userPayrollDetailList.Where(x => x.Amount < 0).Sum(x => x.Amount));
                                    PayrollEntry.NetSalary = PayrollEntry.Salary + PayrollEntry.Addition - PayrollEntry.Deduction;
                                    PayrollEntry.IsActive = true;
                                    PayrollEntry = objPayrollService.UpdatePayroll(PayrollEntry);
                                    #endregion 3.3 Payroll table update
                                }
                                #endregion Payroll when attendanceList is null 
                            }
                        }

                        else
                        {
                            ModelState.AddModelError("", "Payroll already generated");
                        }
                    }
                    #region 4.0 ShowDataOfTestPayroll In VIEW
                    try
                    {
                        IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                        List<Payroll> PayrollList = objPayrollService.GetAllPayrollWithUserWithTestPayroll(AuthBase.BranchId);
                        if (PayrollList != null && PayrollList.Count > 0)
                        {
                            int LastPayrollCycleID = 0, CurrentPayrollCycleID = 0;
                            if (PayrollList.Exists(x => x.IsActive == true))
                            {
                                LastPayrollCycleID = PayrollList.Where(p => p.IsActive == true).Max(p => p.PayrollCycleId).Value;
                                model.PayrollCycle = objPayrollCycleService.GetAllPayrollCycle().Where(x => x.Id >= LastPayrollCycleID).OrderBy(x => x.Id).Take(2).LastOrDefault();
                            }
                            else
                            {
                                LastPayrollCycleID = PayrollList.Max(p => p.PayrollCycleId).Value;
                                model.PayrollCycle = objPayrollCycleService.GetPayrollCycle(LastPayrollCycleID);
                            }
                            CurrentPayrollCycleID = model.PayrollCycle.Id;

                            PayrollList = PayrollList.Where(x => x.PayrollCycleId == CurrentPayrollCycleID).ToList();
                            if (PayrollList != null && PayrollList.Count > 0)
                            {
                                model.DepartmentList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);
                                if (model.DepartmentList != null)
                                {
                                    foreach (var department in model.DepartmentList)
                                    {
                                        department.TotalUser = 0; department.TotalSalary = 0; department.TotalAddition = 0; department.TotalDeduction = 0; department.NetSalary = 0;
                                        UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId).Where(x => x.Department.Id == department.Id).ToList();
                                        if (UserList.Count > 0)
                                        {
                                            department.TotalUser = UserList.Count();
                                            department.TotalSalary = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Salary).Value;
                                            department.TotalAddition = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Addition).Value;
                                            department.TotalDeduction = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Deduction).Value;
                                            department.NetSalary = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.NetSalary).Value;
                                        }
                                    }
                                    model.DepartmentList = model.DepartmentList.Where(x => x.TotalUser > 0).ToList();
                                }
                                else
                                    ModelState.AddModelError("", "No Records Found");
                            }
                            else
                                ModelState.AddModelError("", "No Records Found");
                        }
                        else
                        {
                            model.PayrollCycle = objPayrollCycleService.GetAllPayrollCycle().OrderBy(x => x.Id).FirstOrDefault();
                            ModelState.AddModelError("", "No Records Found");
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            #endregion 4.0 ShowDataOfTestPayroll
                  catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            //Start of UserTypeIdCondtion
            //End loop Employee
            GetPayrollCycle(model.PayrollCycleId); //forPayrollCycleDropDown
            return View(model);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult RunFinalPayroll(VMFinalPayroll model)
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            List<User> UserList = objUserService.GetAllUser().Where(x => x.IsActive == true).ToList();
            if (model.PayrollCycleId>0 && model.IsFinal == true)
            {
                    List<Payroll> PayrollLists = objPayrollService.GetAllPayroll().Where(x =>x.PayrollCycleId == model.PayrollCycleId).ToList();
                    if (PayrollLists.Count > 0)
                    { 
                        PayrollCycle payrollCycle = objPayrollCycleService.GetPayrollCycle(model.PayrollCycleId);
                        {
                            payrollCycle.IsFinal = true;
                        }
                        payrollCycle = objPayrollCycleService.UpdatePayrollCycle(payrollCycle);
                    }
                    else
                        ModelState.AddModelError("", "Please make the test payroll first");
            }
            else
                ModelState.AddModelError("", "Please select Payroll");

            return RedirectToAction("Index","Payroll");
        }
        [HttpGet]
        [Authenticate]
        public ActionResult PayrollEdit(string DepartmentID = null, string PayrollCycleID = null, string IsActive = null)
        {
            PayslipPrerequisiteData();
            VMPayrollModel _model = new VMPayrollModel();
            if (!string.IsNullOrEmpty(DepartmentID) && !string.IsNullOrEmpty(PayrollCycleID) && !string.IsNullOrEmpty(IsActive))
            {
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                List<Payroll> PayrollList = new List<Payroll>();
                _model.PayrollList = PayrollList;
                PayrollList = objPayrollService.GetAllPayrollWithUser(AuthBase.BranchId, Convert.ToInt32(IsActive));
                if (PayrollList != null && PayrollList.Count > 0)
                {
                    IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
                    PayrollCycle _payrollCycle = objPayrollCycleService.GetPayrollCycle(Convert.ToInt32(PayrollCycleID));
                    _model.Month = (int)_payrollCycle.Month;
                    _model.Year = (int)_payrollCycle.Year;
                    _model.IsActive = Convert.ToInt32(IsActive);
                    _model.DepartmentID = Convert.ToInt32(DepartmentID);

                    _model.PayrollList = PayrollList.Where(x => x.PayrollCycle.Month == _model.Month && x.PayrollCycle.Year <= _model.Year).ToList();
                    if (_model.DepartmentID != null && _model.PayrollList != null && _model.PayrollList.Count > 0)
                        _model.PayrollList = _model.PayrollList.Where(x => x.User.Department.Id == _model.DepartmentID).ToList();

                    if (_model.UserName != null && _model.UserName != "" && _model.PayrollList != null && _model.PayrollList.Count > 0)
                        _model.PayrollList = _model.PayrollList.Where(x => x.User.FirstName.ToLower().Contains(_model.UserName.ToLower())).ToList();

                    if (!(_model.PayrollList != null && _model.PayrollList.Count > 0))
                        ModelState.AddModelError("", "No Records Found");
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            return View(_model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult PayrollEdit(VMPayrollModel _model)
        {

            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
            List<Payroll> PayrollList = new List<Payroll>();
            _model.PayrollList = PayrollList;
            //New
            List<PayrollDetail> PayrollDetailList = new List<PayrollDetail>();
            _model.PayrollList = PayrollList;
            _model.IsActive = 1;
            //New
            _model.PayrollDetailList = PayrollDetailList;
            PayrollList = objPayrollService.GetAllPayrollWithUser(AuthBase.BranchId);
            if (PayrollList != null && PayrollList.Count > 0)
            {
                _model.PayrollList = PayrollList.Where(x => x.PayrollCycle.Month == _model.Month && x.PayrollCycle.Year <= _model.Year).ToList();
                _model.PayrollDetailList = objPayrollDetailService.GetAllPayrollDetail();
                foreach (Payroll payrollList in _model.PayrollList)
                {
                    /* _model.PayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId(payrollList.Id);*/ //Get PayrollDetailList by PayrollId


                    if (_model.DepartmentID != null && _model.PayrollList != null && _model.PayrollList.Count > 0)
                        _model.PayrollList = _model.PayrollList.Where(x => x.User.Department.Id == _model.DepartmentID).ToList();

                    if (_model.UserName != null && _model.UserName != "" && _model.PayrollList != null && _model.PayrollList.Count > 0)
                        _model.PayrollList = _model.PayrollList.Where(x => x.User.FirstName.ToLower().Contains(_model.UserName.ToLower())).ToList();

                    if (!(_model.PayrollList != null && _model.PayrollList.Count > 0))
                        ModelState.AddModelError("", "No Records Found");

                    //_model.PayrollList = _model.PayrollList.Add(_model.PayrollDetailList);


                }   
            }
            else
                ModelState.AddModelError("", "No Records Found");
            PayslipPrerequisiteData();
            return View(_model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult ModifyPayroll(string SortOrder, string SortBy, int? PageNumber,string DepartmentId = null, string UserName = null, string PayrollCycleID = null, string IsActive = null)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            PayslipPrerequisiteData();
            VMGetPayrollEditFirst _model = new VMGetPayrollEditFirst();
            VMGetPayrollEditFirst model = new VMGetPayrollEditFirst();
            ModifyViewPayrollPrerequisiteData();
            if (!string.IsNullOrEmpty(DepartmentId) && !string.IsNullOrEmpty(PayrollCycleID) && !string.IsNullOrEmpty(IsActive))
            {
                List<Payroll> PayrollList = new List<Payroll>();
                _model.PayrollList = PayrollList;
                PayrollList = objPayrollService.GetAllPayrollWithUser(AuthBase.BranchId, Convert.ToInt32(IsActive));
                if (PayrollList != null && PayrollList.Count > 0)
                {
                    _model.PayrollList = PayrollList.Where(x => x.PayrollCycle.Month == _model.Month && x.PayrollCycle.Year <= _model.Year).ToList();
                    if (_model.DepartmentId != null && _model.PayrollList != null && _model.PayrollList.Count > 0)
                        _model.PayrollList = _model.PayrollList.Where(x => x.User.Department.Id == _model.DepartmentId).ToList();

                    if (_model.UserName != null && _model.UserName != "" && _model.PayrollList != null && _model.PayrollList.Count > 0)
                        _model.PayrollList = _model.PayrollList.Where(x => x.User.FirstName.ToLower().Contains(_model.UserName.ToLower())).ToList();

                    if (!(_model.PayrollList != null && _model.PayrollList.Count > 0))
                        ModelState.AddModelError("", "No Records Found");
                }
                else
                    ModelState.AddModelError("", "No Records Found");

             return View(_model);
            }
            if (PayrollCycleID!=null || PayrollCycleID == "")
            {
               
                if (PageNumber == null)
                    PageNumber = 1;
                if (PayrollCycleID != null)
                    model.PayrollCycleId = Convert.ToInt32(PayrollCycleID);
                if (DepartmentId != null)
                    model.DepartmentId = Convert.ToInt32(DepartmentId);
                if (UserName != null)
                    model.UserName = UserName;
                model.VMGetPayrollEditSecondList = objPayrollService.GetModifyPayrollSummaryByPayrollCycleId(model.DepartmentId, model.UserName, model.PayrollCycleId);
                if (model.UserName != null)
                    model.VMGetPayrollEditSecondList = model.VMGetPayrollEditSecondList.Where(x => x.UserName.ToLower().Contains(model.UserName.ToLower())).ToList();
                if (model.VMGetPayrollEditSecondList.Count == 0)
                {
                    ModelState.AddModelError("", "No Records Found");
                }
                else
                {
                    #region Pagination
                    ApplyModifyPayrollPagination(Convert.ToInt32(PageNumber), model.VMGetPayrollEditSecondList);
                    #endregion Pagination
                    #region Sorting
                    ApplyModifyPayrollSorting(SortOrder, SortBy, model.VMGetPayrollEditSecondList);
                    #endregion Sorting
                    ViewBag.SortOrder = SortOrder;
                    ViewBag.PageNumber = PageNumber;

                }
               
            }
            if(model!=null)
                return View(model);
            else
            return View(_model);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult ModifyPayroll(VMGetPayrollEditFirst model, string SortOrder, string SortBy, int PageNumber=1)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            try
            {
                List<VMGetPayrollEditSecond> VMGetPayrollEditSecondList = new List<VMGetPayrollEditSecond>();

                model.VMGetPayrollEditSecondList = objPayrollService.GetModifyPayrollSummaryByPayrollCycleId(model.DepartmentId, model.UserName, model.PayrollCycleId);
                if (model.UserName != null)
                    model.VMGetPayrollEditSecondList = model.VMGetPayrollEditSecondList.Where(x => x.UserName.ToLower().Contains(model.UserName.ToLower())).ToList();
                if (model.VMGetPayrollEditSecondList.Count == 0)
                {
                    ModelState.AddModelError("", "No Records Found");
                }
                else
                {
                    #region Pagination
                    model.VMGetPayrollEditSecondList = ApplyModifyPayrollPagination(PageNumber,model.VMGetPayrollEditSecondList);
                    #endregion Pagination
                    #region Sorting
                    model.VMGetPayrollEditSecondList = ApplyModifyPayrollSorting(SortOrder,SortBy,model.VMGetPayrollEditSecondList);
                    #endregion Sorting
                    ViewBag.SortOrder = SortOrder;
                    ViewBag.PageNumber = PageNumber;
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            PayslipPrerequisiteData();
            ModifyViewPayrollPrerequisiteData();
            return View(model);
        }
        public List<VMGetPayrollEditSecond> ApplyModifyPayrollSorting(string SortOrder, string SortBy,List<VMGetPayrollEditSecond> VMGetPayrollEditSecondList)
        {
            if (SortBy == "Employee")
            {
                if (SortOrder == "Asc")
                {
                    VMGetPayrollEditSecondList = VMGetPayrollEditSecondList.OrderBy(x => x.UserName).ToList();

                }
                else if (SortOrder == "Desc")
                {
                    VMGetPayrollEditSecondList = VMGetPayrollEditSecondList.OrderByDescending(x => x.UserName).ToList();

                }
                else
                {
                    VMGetPayrollEditSecondList = VMGetPayrollEditSecondList.OrderBy(x => x.UserName).ToList();
                    SortOrder = "Asc";
                }
            }
            return VMGetPayrollEditSecondList;
        }
        public List<VMGetPayrollEditSecond> ApplyModifyPayrollPagination (int PageNumber, List<VMGetPayrollEditSecond> VMGetPayrollEditSecondList)
        {
            
            double PageCount =VMGetPayrollEditSecondList.Count;
            ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
           VMGetPayrollEditSecondList = VMGetPayrollEditSecondList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
            return VMGetPayrollEditSecondList;
        }

        [HttpGet]
        [Authenticate]
        public ActionResult ModifyPayrollDetail(Int32? PayrollId, Int32? UserId, Int32? DepartmentId)
        {
            VMUserPayroll model = new VMUserPayroll();
            try
            {
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                model.VMUserPayrollEditList = new List<VMUserPayrollEdit>();
                if (PayrollId != null)
                    model.VMUserPayrollEditList = objPayrollService.GetModifyPayrollSummary(PayrollId, UserId, DepartmentId);

                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            PayslipPrerequisiteData();
            ModifyViewPayrollPrerequisiteData();
            return View(model);

        }
        [HttpPost]
        [Authenticate]
        public JsonResult ModifyPayrollDetail2(List<VMUserPayrollEdit> VMUserPayrollEditList)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            ICustomPayrollDetailService objCustomPayrollDetailService = IoC.Resolve<ICustomPayrollDetailService>("CustomPayrollDetailService");
            try
            {
                foreach (VMUserPayrollEdit models in VMUserPayrollEditList)
                {
                    foreach (VMUserPayrollDetailEdit detailEdit in models.VMUserPayrollDetailEditList)
                    {
                        if (detailEdit.PayrollDetailId > 0 && detailEdit.IsDelete == 0 && detailEdit.IsUpdate == 1)
                        {
                            #region CustomPayrollDetailAdd and delete from PAyrollDetail
                            PayrollDetail payrollDetailList = objPayrollDetailService.GetPayrollDetail(Convert.ToInt32(detailEdit.PayrollDetailId));
                            CustomPayrollDetail customPayrollDetailList = objCustomPayrollDetailService.GetCustomPayrollDetail(Convert.ToInt32(detailEdit.PayrollDetailId));
                            //delete from PAyrollDetail
                            if (payrollDetailList != null)
                                objPayrollDetailService.DeletePayrollDetail(payrollDetailList.Id);
                            //Add in customPayrollDetail
                            if (customPayrollDetailList == null)
                            {
                                CustomPayrollDetail CustompayrollDetailAdd = new CustomPayrollDetail()
                                {
                                    PayrollId = models.PayrollId,
                                    PayrollPolicyId = payrollDetailList.PayrollPolicyId,
                                    Amount = detailEdit.Amount,
                                    IsActive = payrollDetailList.IsActive,
                                    CreationDate = payrollDetailList.CreationDate,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = AuthBase.UserId,
                                    UserIp = Request.UserHostAddress,
                                    PayrollPolicyName = detailEdit.PayrollPolicyName,
                                };
                                CustompayrollDetailAdd = objCustomPayrollDetailService.InsertCustomPayrollDetail(CustompayrollDetailAdd);
                            }
                            else if (customPayrollDetailList != null)
                            {
                                CustomPayrollDetail CustompayrollDetailUpdate = new CustomPayrollDetail()
                                {
                                    Id = customPayrollDetailList.Id,
                                    PayrollId = models.PayrollId,
                                    PayrollPolicyId = customPayrollDetailList.PayrollPolicyId,
                                    Amount = detailEdit.Amount,
                                    IsActive = customPayrollDetailList.IsActive,
                                    CreationDate = customPayrollDetailList.CreationDate,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = AuthBase.UserId,
                                    UserIp = Request.UserHostAddress,
                                    PayrollPolicyName = customPayrollDetailList.PayrollPolicyName,
                                };
                                CustompayrollDetailUpdate = objCustomPayrollDetailService.UpdateCustomPayrollDetail(CustompayrollDetailUpdate);
                            }
                            #endregion  CustomPayrollDetailAdd and delete from PAyrollDetail
                        }
                        else if (detailEdit.PayrollDetailId == 0 && detailEdit.IsDelete == 0 && detailEdit.IsUpdate == 0)
                        {
                            #region CustomPayrollDetailAdd
                            CustomPayrollDetail CustompayrollDetailAdd = new CustomPayrollDetail()
                            {
                                PayrollId = models.PayrollId,
                                PayrollPolicyId = null, //Issue
                                Amount = detailEdit.Amount,
                                IsActive = true,
                                CreationDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                UpdateBy = AuthBase.UserId,
                                UserIp = Request.UserHostAddress,
                                PayrollPolicyName = detailEdit.PayrollPolicyName,
                            };
                            CustompayrollDetailAdd = objCustomPayrollDetailService.InsertCustomPayrollDetail(CustompayrollDetailAdd);
                            #endregion PayrollDetailAdd
                        }
                        else if (detailEdit.IsDelete == 1 && detailEdit.PayrollDetailId > 0)
                        {
                            PayrollDetail payrollDetailList = objPayrollDetailService.GetPayrollDetail(Convert.ToInt32(detailEdit.PayrollDetailId));
                            CustomPayrollDetail customPayrollDetailList = objCustomPayrollDetailService.GetCustomPayrollDetail(Convert.ToInt32(detailEdit.PayrollDetailId));
                            #region PayrollDetailDelete
                            if (payrollDetailList != null)
                            {
                                int PayrollDetailId = (int)detailEdit.PayrollDetailId;
                                objPayrollDetailService.DeletePayrollDetail(PayrollDetailId);
                            }
                            #endregion PayrollDetailDelete

                            #region CustomPayrollDetailDelete
                            else if (customPayrollDetailList != null)
                            {
                                int CustomPayrollDetailId = (int)detailEdit.PayrollDetailId;
                                objCustomPayrollDetailService.DeleteCustomPayrollDetail(CustomPayrollDetailId);
                            }
                            #endregion CustomPayrollDetailDelete
                        }

                    }

                    #region PayrollUpdate
                    Payroll payrollList = objPayrollService.GetPayroll(Convert.ToInt32(models.PayrollId));
                    if (payrollList != null)
                    {
                        List<PayrollDetail> userPayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId(payrollList.Id);
                        List<CustomPayrollDetail> userCustomPayrollDetailList = objCustomPayrollDetailService.GetCustomPayrollDetailByPayrollId(payrollList.Id);
                        decimal Deduction = 0;
                        decimal Addition = 0;
                        if (userPayrollDetailList == null || userPayrollDetailList.Count == 0)
                        {

                        }
                        else if (userPayrollDetailList.Count > 0 && userPayrollDetailList != null)
                        {
                            payrollList.Addition = userPayrollDetailList.Where(x => x.Amount > 0).Sum(x => x.Amount) + Addition;
                            payrollList.Deduction = -(userPayrollDetailList.Where(x => x.Amount < 0).Sum(x => x.Amount) - Deduction);
                            payrollList.NetSalary = payrollList.Salary + payrollList.Addition - (payrollList.Deduction);
                            payrollList.IsActive = true;
                            payrollList = objPayrollService.UpdatePayroll(payrollList);
                        }
                        if (userCustomPayrollDetailList.Count == 0 && userCustomPayrollDetailList == null)
                        {

                        }
                        else if (userCustomPayrollDetailList.Count > 0 && userCustomPayrollDetailList != null)
                        {
                            payrollList.Addition = userCustomPayrollDetailList.Where(x => x.Amount > 0).Sum(x => x.Amount);
                            payrollList.Deduction = -(userCustomPayrollDetailList.Where(x => x.Amount < 0).Sum(x => x.Amount));
                            payrollList.NetSalary = payrollList.Salary + payrollList.Addition - payrollList.Deduction;
                            payrollList.IsActive = true;
                            payrollList = objPayrollService.UpdatePayroll(payrollList);
                            Deduction = Math.Floor(Convert.ToDecimal(payrollList.Deduction));
                            Addition = Math.Floor(Convert.ToDecimal(payrollList.Addition));
                        }
                    }
                    #endregion PayrollUpdate

                }
            }
            catch (Exception ex)
            {
                return Json("Failure", JsonRequestBehavior.AllowGet);
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //[Authenticate]
        //public JsonResult ModifyPayrollDetail2(List<VMUserPayrollEdit> VMUserPayrollEditList)
        //{
        //    IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
        //    IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
        //    IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
        //        foreach (VMUserPayrollEdit models in VMUserPayrollEditList)
        //        {
        //            foreach (VMUserPayrollDetailEdit detailEdit in models.VMUserPayrollDetailEditList)
        //            {
        //                if (detailEdit.PayrollDetailId > 0 && detailEdit.IsDelete == 0)
        //                {
        //                    #region PayrollDetailUpdate
        //                    PayrollDetail payrollDetailList = objPayrollDetailService.GetPayrollDetail(Convert.ToInt32(detailEdit.PayrollDetailId));
        //                    PayrollDetail payrollDetailUpdate = new PayrollDetail()
        //                    {
        //                        Id = Convert.ToInt32(detailEdit.PayrollDetailId),
        //                        PayrollId = models.PayrollId,
        //                        PayrollPolicyId = payrollDetailList.PayrollPolicyId,
        //                        Amount = detailEdit.Amount,
        //                        IsActive = payrollDetailList.IsActive,
        //                        CreationDate = payrollDetailList.CreationDate,
        //                        UpdateDate = DateTime.Now,
        //                        UpdateBy = AuthBase.UserId,
        //                        UserIp = Request.UserHostAddress,
        //                        PayrollPolicyName = detailEdit.PayrollPolicyName,
        //                    };
        //                    payrollDetailUpdate = objPayrollDetailService.UpdatePayrollDetail(payrollDetailUpdate);
        //                    #endregion PayrollDetailUpdate
        //                }
        //                else if (detailEdit.PayrollDetailId == 0 && detailEdit.IsDelete == 0)
        //                {
        //                    #region PayrollDetailAdd
        //                    PayrollDetail payrollDetailAdd = new PayrollDetail()
        //                    {
        //                        PayrollId = models.PayrollId,
        //                        PayrollPolicyId = null, //Issue
        //                        Amount = detailEdit.Amount,
        //                        IsActive = true,
        //                        CreationDate = DateTime.Now,
        //                        UpdateDate = DateTime.Now,
        //                        UpdateBy = AuthBase.UserId,
        //                        UserIp = Request.UserHostAddress,
        //                        PayrollPolicyName = detailEdit.PayrollPolicyName,
        //                    };
        //                    payrollDetailAdd = objPayrollDetailService.InsertPayrollDetail(payrollDetailAdd);
        //                    #endregion PayrollDetailAdd
        //                }
        //                else if (detailEdit.IsDelete == 1 && detailEdit.PayrollDetailId > 0)
        //                {
        //                #region PayrollDetailDelete
        //                int PayrollDetailId = (int)detailEdit.PayrollDetailId;
        //                objPayrollDetailService.DeletePayrollDetail((int)detailEdit.PayrollDetailId);
        //                #endregion PayrollDetailDelete
        //            }
        //            }

        //            #region PayrollUpdate
        //            Payroll payrollList = objPayrollService.GetPayroll(Convert.ToInt32(models.PayrollId));

        //            List<PayrollDetail> userPayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId(payrollList.Id);
        //            payrollList.Addition = userPayrollDetailList.Where(x => x.Amount > 0).Sum(x => x.Amount);
        //            payrollList.Deduction = -(userPayrollDetailList.Where(x => x.Amount < 0).Sum(x => x.Amount));
        //            payrollList.NetSalary = payrollList.Salary + payrollList.Addition - payrollList.Deduction;
        //            payrollList.IsActive = true;
        //            payrollList = objPayrollService.UpdatePayroll(payrollList);
        //            #endregion PayrollUpdate
        //        }
        //    return Json("Success", JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        [Authenticate]
        //List<Payroll> payrollList
        public ActionResult PayrollEdition(List<Payroll> PayrollList)
        {
            try
            {
                IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");

                foreach (Payroll models in PayrollList) 
                {
                    foreach (PayrollDetail model in models.PayrollDetailList)
                    {
                        if (model.IsUpdate == true)
                        {
                            #region PayrollDetail Updation
                            PayrollDetail payrollDetailList = objPayrollDetailService.GetPayrollDetail(model.Id);
                            int PayrollId = Convert.ToInt32(payrollDetailList.PayrollId);

                            PayrollDetail payrollDetail = new PayrollDetail()
                            {
                                Id = payrollDetailList.Id,
                                PayrollId = payrollDetailList.PayrollId,
                                PayrollPolicyId = payrollDetailList.PayrollPolicyId,
                                Amount = model.Amount,
                                IsActive = payrollDetailList.IsActive,
                                CreationDate = payrollDetailList.CreationDate,
                                UpdateDate = DateTime.Now,
                                UpdateBy = payrollDetailList.UpdateBy,
                                UserIp = payrollDetailList.UserIp,
                                PayrollPolicyName = payrollDetailList.PayrollPolicyName,
                            };
                            payrollDetail = objPayrollDetailService.UpdatePayrollDetail(payrollDetail);
                            #endregion PayrollDetail Updation

                            #region PayrollUpdation

                            List<Payroll> payrollLists = objPayrollService.GetAllPayroll().Where(x => x.Id == PayrollId).ToList();
                            List<PayrollDetail> userPayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId(PayrollId);
                            Payroll PayrollEntry = new Payroll();
                            foreach (Payroll payrollModel in payrollLists)
                            {
                                PayrollEntry.PayrollCycleId = payrollModel.PayrollCycleId;
                                PayrollEntry.UserId = payrollModel.UserId;
                                PayrollEntry.Salary = payrollModel.Salary;
                                PayrollEntry.Addition = userPayrollDetailList.Where(x => x.Amount > 0).Sum(x => x.Amount);
                                PayrollEntry.Deduction = -(userPayrollDetailList.Where(x => x.Amount < 0).Sum(x => x.Amount));
                                PayrollEntry.NetSalary = PayrollEntry.Salary + PayrollEntry.Addition - PayrollEntry.Deduction;
                                PayrollEntry.IsActive = true;
                                PayrollEntry.CreationDate = payrollModel.CreationDate;
                                PayrollEntry.UpdateDate = DateTime.Now;
                                PayrollEntry.UpdateBy = AuthBase.UserId;
                                PayrollEntry.UserIp = payrollModel.UserIp;

                            }
                            PayrollEntry = objPayrollService.UpdatePayroll(PayrollEntry);

                            #endregion PayrollUpdation
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction("PayrollEdit", "Payroll");
        }
        [HttpGet]
        [Authenticate]
        public ActionResult Generate()
        {
            InsertPayrolCycle();
            VMAttendanceModel model = new VMAttendanceModel();
            try
            {
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                List<Payroll> PayrollList = objPayrollService.GetAllPayrollWithUserWithTestPayroll(AuthBase.BranchId);

                if (PayrollList != null && PayrollList.Count > 0)
                {
                    int LastPayrollCycleID = 0, CurrentPayrollCycleID = 0;
                    if (PayrollList.Exists(x => x.IsActive == true))
                    {
                        LastPayrollCycleID = PayrollList.Where(p => p.IsActive == true).Max(p => p.PayrollCycleId).Value;
                        model.PayrollCycle = objPayrollCycleService.GetAllPayrollCycle().Where(x => x.Id >= LastPayrollCycleID).OrderBy(x => x.Id).Take(2).LastOrDefault();
                    }
                    else
                    {
                        LastPayrollCycleID = PayrollList.Max(p => p.PayrollCycleId).Value;
                        model.PayrollCycle = objPayrollCycleService.GetPayrollCycle(LastPayrollCycleID);
                    }
                    CurrentPayrollCycleID = model.PayrollCycle.Id;

                    PayrollList = PayrollList.Where(x => x.PayrollCycleId == CurrentPayrollCycleID).ToList();
                    if (PayrollList != null && PayrollList.Count > 0)
                    {
                        model.DepartmentList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);
                        if (model.DepartmentList != null)
                        {
                            foreach (var department in model.DepartmentList)
                            {
                                department.TotalUser = 0; department.TotalSalary = 0; department.TotalAddition = 0; department.TotalDeduction = 0; department.NetSalary = 0;
                                List<User> UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId).Where(x => x.Department.Id == department.Id).ToList();
                                if (UserList.Count > 0)
                                {
                                    department.TotalUser = UserList.Count();
                                    department.TotalSalary = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Salary).Value;
                                    department.TotalAddition = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Addition).Value;
                                    department.TotalDeduction = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.Deduction).Value;
                                    department.NetSalary = PayrollList.Where(x => x.User.Department.Id == department.Id).Sum(x => x.NetSalary).Value;
                                }
                            }
                            model.DepartmentList = model.DepartmentList.Where(x => x.TotalUser > 0).ToList();
                        }
                        else
                            ModelState.AddModelError("", "No Records Found");
                    }
                    else
                        ModelState.AddModelError("", "No Records Found");
                }
                else
                {
                    model.PayrollCycle = objPayrollCycleService.GetAllPayrollCycle().OrderBy(x => x.Id).FirstOrDefault();
                    ModelState.AddModelError("", "No Records Found");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult RunPayroll(int PayrollCycleID, int IsFinal)
        {
            try
            {
                IPayrollService ObjPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                ObjPayrollService.RunPayroll(PayrollCycleID, IsFinal);
                return base.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return base.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteVariable(int id)
        {
            try
            {
                Dictionary<string, string> dictPolicy = new Dictionary<string, string>();
                dictPolicy.Add("RetiredDate", string.Format("{0:dd-MMM-yyyy}", DateTime.Now.AddDays(-1)) + " 23:59:59");
                dictPolicy.Add("UpdateDate", DateTime.Now.ToString());
                dictPolicy.Add("UpdateBy", AuthBase.UserId.ToString());
                dictPolicy.Add("UserIP", Request.UserHostAddress);

                IPayrollPolicyService ObjPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
                ObjPayrollPolicyService.UpdatePayrollPolicyByKeyValue(dictPolicy, id);
                return base.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return base.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private void VariablePrerequisiteData()
        {
            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            List<PayrollVariable> PayrollVariableList = objPayrollVariableService.GetAllPayrollVariable();
            if (PayrollVariableList != null && PayrollVariableList.Count > 0 && PayrollVariableList.Where(x => x.IsActive == true) != null)
            {
                List<SelectListItem> _variableList = new List<SelectListItem>();
                foreach (PayrollVariable variable in PayrollVariableList.Where(x => x.IsActive == true && x.IsDeduction == false).ToList())
                {
                    _variableList.Add(new SelectListItem() { Text = variable.Name + " - Addition", Value = variable.Id.ToString() });
                }

                foreach (PayrollVariable variable in PayrollVariableList.Where(x => x.IsActive == true && x.IsDeduction == true).ToList())
                {
                    _variableList.Add(new SelectListItem() { Text = variable.Name + " - Deduction", Value = variable.Id.ToString() });
                }
                ViewBag.PayrollVariable = new SelectList(_variableList, "Value", "Text");
            }
        }

        private void PayslipPrerequisiteData()
        {
            int count = 0;
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            List<Department> DepartmentList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);
            if (DepartmentList != null && DepartmentList.Count > 0)
                ViewBag.Department = new SelectList(DepartmentList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Department", Value = "-1" });
                ViewBag.Department = new SelectList(blank, "Value", "Text", "");
            }

            List<SelectListItem> monthlist = new List<SelectListItem>();
            var MonthNum1 = DateTime.Now.Month;
            var MonthNum = MonthNum1-1;
            if (MonthNum == 0)
            { 
                MonthNum = 12;
                count++;
            }


            for (int i = MonthNum; i > 0; i--)
            {
                if (MonthNum == 1) { monthlist.Add(new SelectListItem() { Text = "January", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 2) { monthlist.Add(new SelectListItem() { Text = "Feburary", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 3) { monthlist.Add(new SelectListItem() { Text = "March", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 4) { monthlist.Add(new SelectListItem() { Text = "April", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 5) { monthlist.Add(new SelectListItem() { Text = "May", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 6) { monthlist.Add(new SelectListItem() { Text = "June", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 7) { monthlist.Add(new SelectListItem() { Text = "July", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 8) { monthlist.Add(new SelectListItem() { Text = "August", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 9) { monthlist.Add(new SelectListItem() { Text = "September", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 10) { monthlist.Add(new SelectListItem() { Text = "October", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 11) { monthlist.Add(new SelectListItem() { Text = "November", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }

                if (MonthNum == 12) { monthlist.Add(new SelectListItem() { Text = "December", Value = Convert.ToString(i), Selected = DateTime.Now.Month == int.Parse(Convert.ToString(i)) }); }


                MonthNum--;
            }
            //monthlist.Add(new SelectListItem() { Text = "January", Value = "1", Selected = DateTime.Now.Month == int.Parse("1") });
            //monthlist.Add(new SelectListItem() { Text = "February", Value = "2", Selected = DateTime.Now.Month == int.Parse("2") });
            //monthlist.Add(new SelectListItem() { Text = "March", Value = "3", Selected = DateTime.Now.Month == int.Parse("3") });
            //monthlist.Add(new SelectListItem() { Text = "April", Value = "4", Selected = DateTime.Now.Month == int.Parse("4") });
            //monthlist.Add(new SelectListItem() { Text = "May", Value = "5", Selected = DateTime.Now.Month == int.Parse("5") });
            //monthlist.Add(new SelectListItem() { Text = "June", Value = "6", Selected = DateTime.Now.Month == int.Parse("6") });
            //monthlist.Add(new SelectListItem() { Text = "July", Value = "7", Selected = DateTime.Now.Month == int.Parse("7") });
            //monthlist.Add(new SelectListItem() { Text = "August", Value = "8", Selected = DateTime.Now.Month == int.Parse("8") });
            //monthlist.Add(new SelectListItem() { Text = "September", Value = "9", Selected = DateTime.Now.Month == int.Parse("9") });
            //monthlist.Add(new SelectListItem() { Text = "October", Value = "10", Selected = DateTime.Now.Month == int.Parse("10") });
            //monthlist.Add(new SelectListItem() { Text = "November", Value = "11", Selected = DateTime.Now.Month == int.Parse("11") });
            //monthlist.Add(new SelectListItem() { Text = "December", Value = "12", Selected = DateTime.Now.Month == int.Parse("12") });

            ViewBag.MonthList = new SelectList(monthlist, "Value", "Text");

            List<SelectListItem> year = new List<SelectListItem>();
            if(count ==0)
            { 
                for (int i = DateTime.Now.Year; i >=2016 ; i--)
                {
                    year.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
            }
            else
            {
                for (int i = (DateTime.Now.Year)-1; i >= 2016; i--)
                {
                    year.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
                }
            }
            ViewBag.Year = new SelectList(year, "Value", "Text");
        }
        private void ModifyViewPayrollPrerequisiteData()
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            List<PayrollCycle> PayrollCycleList = objPayrollCycleService.GetAllPayrollCycle().Where(x => x.IsFinal == false).OrderByDescending(x => x.Id).ToList();
            if (PayrollCycleList != null && PayrollCycleList.Count > 0)
                ViewBag.PayrollCycle = new SelectList(PayrollCycleList, "Id", "Name");

            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            List<PayrollVariable> PayrollVariableList = objPayrollVariableService.GetAllPayrollVariable();
            if (PayrollVariableList != null && PayrollVariableList.Count > 0)
                ViewBag.PayrollVariable = new SelectList(PayrollVariableList, "Id", "Name");
        }
        //private void ViewPayrollPrerequisiteData()
        //{
        //    IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
        //    //List<PayrollCycle> PayrollCycleList = objPayrollCycleService.GetAllPayrollCycle().Where(x=>x.IsFinal == false).OrderByDescending(x=>x.Id).ToList();
        //    List<PayrollCycle> PayrollCycleList = objPayrollCycleService.GetAllPayrollCycle().ToList();
        //    if (PayrollCycleList != null && PayrollCycleList.Count > 0)
        //    ViewBag.PayrollCycle = new SelectList(PayrollCycleList, "Id", "Name");

        //    IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
        //    List<PayrollVariable> PayrollVariableList = objPayrollVariableService.GetAllPayrollVariable();
        //    if (PayrollVariableList != null && PayrollVariableList.Count > 0)
        //        ViewBag.PayrollVariable = new SelectList(PayrollVariableList, "Id", "Name");
        //}
        private void ViewPayrollPrerequisiteData()
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            List<PayrollCycle> PayrollCycleList = objPayrollCycleService.GetAllPayrollCycle().Where(x => x.IsFinal == true && x.IsActive == true).OrderByDescending(x => x.Id).ToList();
            if (PayrollCycleList != null && PayrollCycleList.Count > 0)
                ViewBag.PayrollCycle = new SelectList(PayrollCycleList, "Id", "Name");

            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            List<PayrollVariable> PayrollVariableList = objPayrollVariableService.GetAllPayrollVariable();
            if (PayrollVariableList != null && PayrollVariableList.Count > 0)
                ViewBag.PayrollVariable = new SelectList(PayrollVariableList, "Id", "Name");
        }
     
        private void InsertPayrolCycle()
        {
            try
            {
                IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
                List<PayrollCycle> PayrollCycleList = objPayrollCycleService.GetAllPayrollCycle();
                var dateMonth = DateTime.Now.AddMonths(-1);
                var Month = DateTime.Now.Month ;
                var Year = 0;
                if (Month == 0)
                {
                    Month = 12;
                    Year = DateTime.Now.Year - 1;
                }
                else { Month = Month - 1; Year = DateTime.Now.Year; }
                if (PayrollCycleList == null || PayrollCycleList.Count <= 0)
                {
                  
                    PayrollCycle pc = new PayrollCycle() { Id = 0, Month = Month, Year = Year, Name = dateMonth.ToString("MMM yyyy") + " Payroll", IsActive = true, CreationDate = DateTime.Now, UpdateBy = AuthBase.UserId, UserIp = Request.UserHostAddress };
                    objPayrollCycleService.InsertPayrollCycle(pc);
                }
                else
                {
                    List<PayrollCycle> tmpPayrollCycleList = null;
                    tmpPayrollCycleList = PayrollCycleList.Where(x => x.Month.Value == Month && x.Year.Value == Year && x.IsActive == true).ToList();
                    if (tmpPayrollCycleList == null || tmpPayrollCycleList.Count <= 0)
                    {
                        PayrollCycle pcLast = PayrollCycleList.OrderBy(x => x.Year).ThenBy(x => x.Month).Last();
                        DateTime dtStart = new DateTime(pcLast.Year.Value, pcLast.Month.Value, 1);
                        DateTime previousMonth = new DateTime(Year, Month, 1);
                        while (dtStart < previousMonth)
                        {
                            PayrollCycle pc = new PayrollCycle() { Id = 0, Month = Month, Year = dtStart.Year, Name = dtStart.AddMonths(1).ToString("MMM yyyy") + " Payroll", IsActive = true, CreationDate = DateTime.Now, UpdateDate=DateTime.Now, UpdateBy = AuthBase.UserId, UserIp = Request.UserHostAddress, IsFinal = false };
                            objPayrollCycleService.InsertPayrollCycle(pc);
                            dtStart = dtStart.AddMonths(1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //private void InsertPayrolCycle()
        //{
        //    try
        //    {
        //        IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
        //        List<PayrollCycle> PayrollCycleList = objPayrollCycleService.GetAllPayrollCycle();
        //        if (PayrollCycleList == null || PayrollCycleList.Count <= 0)
        //        {
        //         
        //            PayrollCycle pc = new PayrollCycle() { Id = 0, Month = DateTime.Now.Month, Year = DateTime.Now.Year, Name = DateTime.Now.ToString("MMM yyyy") + " Payroll", IsActive = true, CreationDate = DateTime.Now, UpdateBy = AuthBase.UserId, UserIp = Request.UserHostAddress };
        //            objPayrollCycleService.InsertPayrollCycle(pc);
        //        }
        //        else
        //        {
        //            List<PayrollCycle> tmpPayrollCycleList = null;
        //            tmpPayrollCycleList = PayrollCycleList.Where(x => x.Month.Value == DateTime.Now.Month && x.Year.Value == DateTime.Now.Year && x.IsActive == true).ToList();
        //            if (tmpPayrollCycleList == null || tmpPayrollCycleList.Count <= 0)
        //            {
        //                PayrollCycle pcLast = PayrollCycleList.OrderBy(x => x.Year).ThenBy(x => x.Month).Last();
        //                DateTime dtStart = new DateTime(pcLast.Year.Value, pcLast.Month.Value, 1);
        //                while (dtStart <= DateTime.Now)
        //                {
        //                    PayrollCycle pc = new PayrollCycle() { Id = 0, Month = dtStart.Month, Year = dtStart.Year, Name = dtStart.ToString("MMM yyyy") + " Payroll", IsActive = true, CreationDate = DateTime.Now, UpdateBy = AuthBase.UserId, UserIp = Request.UserHostAddress, IsFinal = false };
        //                    objPayrollCycleService.InsertPayrollCycle(pc);
        //                    dtStart = dtStart.AddMonths(1);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }
}
