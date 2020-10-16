using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;
using HRM.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.WebAPI.Controllers
{
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
        public ActionResult VariableList()
        {
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
        public ActionResult Payslip(string DepartmentID = null, string PayrollCycleID = null, string IsActive = null)
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
        public ActionResult Payslip(VMPayrollModel _model)
        {
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
                        if (detail.PayrollPolicy.PayrollVariable.Id == Convert.ToInt32(HRM.Core.Enum.PayrollVariable.OverTime))
                        {
                            _model.Payroll.OverTimeDayCount++;
                            _model.Payroll.OverTimeDayAmount += detail.Amount;
                        }
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
                return View(_model);
            }
            else
            {
                ModelState.AddModelError("", "No Records Found");
                return View();
            }
        }

        //New Generate Payroll
        private void GetPayrollCycle(int? PayrollCycleId)
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
            if(model.IsCheck == "IsUnit")
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
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
            IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");

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
            #region 2.Get PayrollPolicy->(Done)
            List<PayrollPolicy> PayrollPolicyList = objPayrollPolicyService.GetAllPayrollPolicy().Where(x => x.EffectiveDate <= payrollStartDate && (x.RetiredDate == null || x.RetiredDate >= payrollEndDate)).ToList();
            #endregion
            //List<AttendanceStatus> AttendanceStatusList;
            //start employee lopp
            foreach (User users in UserList)
            {
                if (users.UserTypeId != 1) //Start of UserTypeIdCondtion
                {
                    //Get All AttendanceStatus Between PayrollDateRange       
                    //1st Problem(Resolved) GetAttendanceStatusByDateRangeSharp
                    List<AttendanceStatus> AttendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByDateRangeSharp(payrollStartDate, payrollEndDate);

                    #region 3.1	Insert 0 value entry in payroll 
                    //Pick salary of user

                    //2nd Problem(Resolved)
                    decimal Salary = 0;
                    if (users.SalaryTypeId == 1 /*Monthly*/)
                    {
                        Salary = users.Salary.Value;
                    }
                    else if (users.SalaryTypeId == 2 /*weekly*/)
                    {
                        Salary = users.Salary.Value * 4;
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

                    #region 3.2	Loop Payroll Policy
                    foreach (PayrollPolicy policy in PayrollPolicyList)
                    {
                        var PayrollVariableList = objPayrollVariableService.GetPayrollVariable(policy.Id);
                        #region 3.2.1 variable type== NON Attendance


                        if (policy.IsAttendance == true)
                        {
                            #region 3.2.1.1	if AMOUNT - Insert in payroll detail
                            if (policy.IsUnit == true)
                            {

                              
                                //Insert in payrollDetail
                                PayrollDetail PayrollDetailEntry = null;
                                PayrollDetailEntry = new PayrollDetail()
                                {
                                    PayrollId = PayrollEntry.Id,
                                    PayrollPolicyId = policy.Id,
                                    Amount = policy.Value,
                                    IsActive = true,
                                    CreationDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                    UpdateBy = AuthBase.UserId,
                                    UserIp = Request.UserHostAddress,
                                    PayrollPolicyName = PayrollVariableList.Name
                                };
                                PayrollDetailEntry = objPayrollDetailService.InsertPayrollDetail(PayrollDetailEntry);
                            }
                            #endregion 3.2.1.2 if Percentage
                            else if (policy.IsPercentage == true)
                            {
                                //pending
                            }

                        }
                        #endregion 3.2.1 variable type== NON Attendance
                        #region 3.2.2 variable type == Attendance
                        else if (policy.IsAttendance == false)
                        {
                            #region 3.2.2.1	if AMOUNT
                            if (policy.IsUnit == true)
                            {
                                //pending

                            }
                            #endregion

                            #region 3.2.2.2	if PERCENTAGE

                            else if (policy.IsPercentage == true)
                            {
                                //pending
                            }
                            #endregion

                            #region if DAY, Check variable if Late, Count Is Late in Attendance Status / Occurance --> Get Integer


                            else if (policy.IsDay == true)
                            {
                                int Amount = 0, QuarterCount = 0, HalfCount = 0, FullDayCount = 0, EarlyCount = 0, LateCount = 0;

                                if (policy.PayrollVariableId == 1)/*QuarterDay*/
                                {
                                    
                                    int count = AttendanceStatusList.Where(x => x.IsQuarterDay == true).Count();
                                    
                                    if (count >= 3)
                                    {
                                        QuarterCount = 1;
                                        Amount = Convert.ToInt32(policy.Value / policy.Occurance);
                                    }
                                }
                                if (policy.PayrollVariableId == 2)/*HalfDay*/
                                {
                                    int count = AttendanceStatusList.Where(x => x.IsHalfDay == true).Count();
                                    if (count >= 3)
                                    {

                                        Amount = 0;
                                        Amount = Convert.ToInt32(policy.Value / policy.Occurance);
                                    }
                                }
                                if (policy.PayrollVariableId == 3)/*FullDay*/
                                {
                                    int count = AttendanceStatusList.Where(x => x.IsFullDay == true).Count();
                                    if (count >= 3)
                                    {
                                        Amount = 0;
                                      
                                        Amount = Convert.ToInt32(policy.Value / policy.Occurance);
                                    }
                                }
                                if (policy.PayrollVariableId == 45)/*Early*/
                                {
                                    int count = AttendanceStatusList.Where(x => x.IsEarly == true).Count();
                                    if (count >= 3)
                                    {
                                        Amount = 0;
                                   
                                        Amount = Convert.ToInt32(policy.Value / policy.Occurance);
                                    }
                                }

                                if (policy.PayrollVariableId == 46)/*Late*/
                                {


                                    int count = AttendanceStatusList.Where(x => x.IsLate == true).Count();
                                    if (count >= 3)
                                    {
                                        Amount = 0;
                                        Amount = Convert.ToInt32(policy.Value / policy.Occurance);
                                    }
                                }
                                //Insert in payrollDetail
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
                            #endregion
                        }

                        #endregion
                    }
                    //End loop PayrollPolicy
                    #endregion

                    #region 3.3 Payroll table update
                    List<PayrollDetail> userPayrollDetailList = objPayrollDetailService.GetPayrollDetailByPayrollId(PayrollEntry.Id);
                    //Amount Try


                    PayrollEntry.Addition = userPayrollDetailList.Where(x => x.Amount > 0).Sum(x => x.Amount);
                    PayrollEntry.Deduction = -(userPayrollDetailList.Where(x => x.Amount < 0).Sum(x => x.Amount));
                    PayrollEntry.NetSalary = PayrollEntry.Salary + PayrollEntry.Addition - PayrollEntry.Deduction;
                    PayrollEntry.IsActive = true;
                    PayrollEntry = objPayrollService.UpdatePayroll(PayrollEntry);


                    #endregion
                } 
            }
            #region 4.0 ShowDataOfTestPayroll
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
            #endregion 4.0 ShowDataOfTestPayroll
            //Start of UserTypeIdCondtion
            //End loop Employee
            GetPayrollCycle(model.PayrollCycleId); //forPayrollCycleDropDown
            return View(model);
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
        public ActionResult ModifyPayroll(string DepartmentId = null, string PayrollCycleID = null, string IsActive = null)
        {
            PayslipPrerequisiteData();
            VMModifyPayroll _model = new VMModifyPayroll();
            ViewPayrollPrerequisiteData();
            if (!string.IsNullOrEmpty(DepartmentId) && !string.IsNullOrEmpty(PayrollCycleID) && !string.IsNullOrEmpty(IsActive))
            {
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
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
            }
            return View(_model);

        }
        [HttpPost]
        [Authenticate]
        public ActionResult ModifyPayroll(VMModifyPayroll model)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            try
            {
                List<VMModifyPayrollEdit> VMModifyPayrollEditList = new List<VMModifyPayrollEdit>();
                model.VMModifyPayrollEditList = VMModifyPayrollEditList;

                model.VMModifyPayrollEditList = objPayrollService.GetModifyPayrollByPayrollCycleId(model.PayrollCycleId, model.DepartmentId, model.UserName);

                if (model.VMModifyPayrollEditList == null)
                {
                    ModelState.AddModelError("", "No Records Found");
                }
      
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            PayslipPrerequisiteData();
            ViewPayrollPrerequisiteData();
            return View(model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult ModifyPayrollDetail(Int32? PayrollId,Int32? UserId, Int32? DepartmentId)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            VMModifyPayroll model = new VMModifyPayroll();
            try
            {
                List<VMModifyPayrollEdit> VMModifyPayrollEditList = new List<VMModifyPayrollEdit>();
                model.VMModifyPayrollEditList = VMModifyPayrollEditList;
                model.VMModifyPayrollEditList = objPayrollService.GetModifyPayslipEdit(PayrollId, DepartmentId, UserId);


                if (model.VMModifyPayrollEditList == null || model.VMModifyPayrollEditList.Count == 0 )
                {
                    ModelState.AddModelError("", "No Records Found");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            PayslipPrerequisiteData();
            ViewPayrollPrerequisiteData();
            return View(model);
        }
       
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
            monthlist.Add(new SelectListItem() { Text = "January", Value = "1", Selected = DateTime.Now.Month == int.Parse("1") });
            monthlist.Add(new SelectListItem() { Text = "February", Value = "2", Selected = DateTime.Now.Month == int.Parse("2") });
            monthlist.Add(new SelectListItem() { Text = "March", Value = "3", Selected = DateTime.Now.Month == int.Parse("3") });
            monthlist.Add(new SelectListItem() { Text = "April", Value = "4", Selected = DateTime.Now.Month == int.Parse("4") });
            monthlist.Add(new SelectListItem() { Text = "May", Value = "5", Selected = DateTime.Now.Month == int.Parse("5") });
            monthlist.Add(new SelectListItem() { Text = "June", Value = "6", Selected = DateTime.Now.Month == int.Parse("6") });
            monthlist.Add(new SelectListItem() { Text = "July", Value = "7", Selected = DateTime.Now.Month == int.Parse("7") });
            monthlist.Add(new SelectListItem() { Text = "August", Value = "8", Selected = DateTime.Now.Month == int.Parse("8") });
            monthlist.Add(new SelectListItem() { Text = "September", Value = "9", Selected = DateTime.Now.Month == int.Parse("9") });
            monthlist.Add(new SelectListItem() { Text = "October", Value = "10", Selected = DateTime.Now.Month == int.Parse("10") });
            monthlist.Add(new SelectListItem() { Text = "November", Value = "11", Selected = DateTime.Now.Month == int.Parse("11") });
            monthlist.Add(new SelectListItem() { Text = "December", Value = "12", Selected = DateTime.Now.Month == int.Parse("12") });
            ViewBag.MonthList = new SelectList(monthlist, "Value", "Text");

            List<SelectListItem> year = new List<SelectListItem>();
            for (int i = 2016; i <= DateTime.Now.Year; i++)
            {
                year.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            ViewBag.Year = new SelectList(year, "Value", "Text");
        }

        private void ViewPayrollPrerequisiteData()
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            List<PayrollCycle> PayrollCycleList = objPayrollCycleService.GetAllPayrollCycle();
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
                if (PayrollCycleList == null || PayrollCycleList.Count <= 0)
                {
                    PayrollCycle pc = new PayrollCycle() { Id = 0, Month = DateTime.Now.Month, Year = DateTime.Now.Year, Name = DateTime.Now.ToString("MMM yyyy") + " Payroll", IsActive = true, CreationDate = DateTime.Now, UpdateBy = AuthBase.UserId, UserIp = Request.UserHostAddress };
                    objPayrollCycleService.InsertPayrollCycle(pc);
                }
                else
                {
                    List<PayrollCycle> tmpPayrollCycleList = null;
                    tmpPayrollCycleList = PayrollCycleList.Where(x => x.Month.Value == DateTime.Now.Month && x.Year.Value == DateTime.Now.Year && x.IsActive == true).ToList();
                    if (tmpPayrollCycleList == null || tmpPayrollCycleList.Count <= 0)
                    {
                        PayrollCycle pcLast = PayrollCycleList.OrderBy(x => x.Year).ThenBy(x => x.Month).Last();
                        DateTime dtStart = new DateTime(pcLast.Year.Value, pcLast.Month.Value, 1);
                        while (dtStart <= DateTime.Now)
                        {
                            PayrollCycle pc = new PayrollCycle() { Id = 0, Month = dtStart.Month, Year = dtStart.Year, Name = dtStart.ToString("MMM yyyy") + " Payroll", IsActive = true, CreationDate = DateTime.Now, UpdateBy = AuthBase.UserId, UserIp = Request.UserHostAddress, IsFinal = false };
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
    }
}
