using HRM.Core;
using HRM.Core.Helper;
using HRM.Core.IService;
using HRM.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRM.Core.Entities;
using System.Web.Mvc;
using HRM.Repository;
using Newtonsoft.Json;

namespace HRM.WebAPI.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authenticate]
        public ActionResult DailyAttendance(DateTime? Date,  string SortOrder, string SortBy, int? PageNumber)
        {
            if (PageNumber == 0 || PageNumber == null)
                PageNumber = 1;
            if (SortOrder == null && SortBy == null && Date == null )
            {
                EmpDailyAttendance Model = new EmpDailyAttendance();
                try
                {
                    Model.Date = DateTime.Now;

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                return View(Model);
            }
            else
            {
                EmpDailyAttendance Model = new EmpDailyAttendance();
                Model.Date = Convert.ToDateTime(Date);
                ViewBag.SortOrder = SortOrder;
                ViewBag.PageNumber = PageNumber;
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
                string StartDate = Convert.ToString(Date);
                string EndDate = Convert.ToString(Date);
                int UserId = AuthBase.UserId;
                List<UserDepartment> userDepartments = objUserDepartmentService.GetUserDepartmentByUserId(UserId).Where(x => x.EffectiveDate >= Convert.ToDateTime(StartDate) && x.RetiredDate <= Convert.ToDateTime(EndDate) || x.RetiredDate == null).ToList();
                UserDepartment userDepartment = userDepartments.FirstOrDefault();
                var DepartmentId = userDepartment.DepartmentId;
                try
                {
                    if (Model.Date != null)
                        Model.EmpDailyDetailAttendanceList = objAttendanceService.GetEmpDailyAttendance(string.Format("{0:dd-MMM-yyyy}", StartDate), string.Format("{0:dd-MMM-yyyy}", EndDate), UserId, DepartmentId);
                    if (Model.EmpDailyDetailAttendanceList.Count == 0)
                        ModelState.AddModelError("", "No records found");
                    else
                    {
                        #region Searching
                        //if (SearchText != null)
                        //{
                        //    model.PracticeVMReportList = model.PracticeVMReportList.Where(x => x.FirstName.Contains(SearchText) || x.MiddleName.Contains(SearchText) || x.DepartmentName.Contains(SearchText)).ToList();
                        //}
                        #endregion Searching
                        #region Pagination
                        double PageCount = Model.EmpDailyDetailAttendanceList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        Model.EmpDailyDetailAttendanceList = Model.EmpDailyDetailAttendanceList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                        #endregion Pagination
                        #region Sorting
                        if (SortBy == "Date")
                        {
                            if (SortOrder == "Asc")
                            {
                                Model.EmpDailyDetailAttendanceList = Model.EmpDailyDetailAttendanceList.OrderBy(x => x.AttendanceDate).ToList();

                            }
                            else if (SortOrder == "Desc")
                            {
                                Model.EmpDailyDetailAttendanceList = Model.EmpDailyDetailAttendanceList.OrderByDescending(x => x.AttendanceDate).ToList();

                            }
                        }
                        #endregion Sorting
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                return View(Model);
            }
        }
        [HttpPost]
        [Authenticate]
        public ActionResult DailyAttendance(EmpDailyAttendance Model, string SortOrder, string SortBy, int PageNumber = 1)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            string StartDate = Convert.ToString(Model.Date);
            string EndDate = Convert.ToString(Model.Date);
            int UserId = AuthBase.UserId;
            List<UserDepartment> userDepartments = objUserDepartmentService.GetUserDepartmentByUserId(UserId).Where(x=>x.EffectiveDate >= Convert.ToDateTime(StartDate) && x.RetiredDate <= Convert.ToDateTime(EndDate) || x.RetiredDate == null).ToList();
            UserDepartment userDepartment = userDepartments.FirstOrDefault();
            var DepartmentId = userDepartment.DepartmentId;
            try
            {
                if(Model.Date != null) 
                Model.EmpDailyDetailAttendanceList = objAttendanceService.GetEmpDailyAttendance(string.Format("{0:dd-MMM-yyyy}", StartDate), string.Format("{0:dd-MMM-yyyy}", EndDate), UserId, DepartmentId);
                if (Model.EmpDailyDetailAttendanceList.Count == 0)
                    ModelState.AddModelError("", "No records found");
                else
                {
                    #region Searching
                    //if (SearchText != null)
                    //{
                    //    model.PracticeVMReportList = model.PracticeVMReportList.Where(x => x.FirstName.Contains(SearchText) || x.MiddleName.Contains(SearchText) || x.DepartmentName.Contains(SearchText)).ToList();
                    //}
                    #endregion Searching
                    #region Pagination
                    double PageCount = Model.EmpDailyDetailAttendanceList.Count;
                    ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                    Model.EmpDailyDetailAttendanceList = Model.EmpDailyDetailAttendanceList.Skip((PageNumber - 1) * 10).Take(10).ToList();
                    #endregion Pagination
                    #region Sorting
                    if (SortBy == "Date")
                    {
                        if (SortOrder == "Asc")
                        {
                            Model.EmpDailyDetailAttendanceList = Model.EmpDailyDetailAttendanceList.OrderBy(x => x.AttendanceDate).ToList();

                        }
                        else if (SortOrder == "Desc")
                        {
                            Model.EmpDailyDetailAttendanceList = Model.EmpDailyDetailAttendanceList.OrderByDescending(x => x.AttendanceDate).ToList();

                        }
                    }
                    #endregion Sorting
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
                return View(Model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult DailyDetailAttendace(String Date)
        {
            EmpDailyAttendance Model = new EmpDailyAttendance();
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            string StartDate = Date;
            string EndDate = Date;
            int UserId = AuthBase.UserId;
            List<UserDepartment> userDepartments = objUserDepartmentService.GetUserDepartmentByUserId(UserId).Where(x => x.EffectiveDate >= Convert.ToDateTime(StartDate) && x.RetiredDate <= Convert.ToDateTime(EndDate) || x.RetiredDate == null).ToList();
            UserDepartment userDepartment = userDepartments.FirstOrDefault();
            var DepartmentId = userDepartment.DepartmentId;
            try
            {
                if (Date != null)
                    Model.EmpDailyDetailAttendanceList = objAttendanceService.GetEmpDailyAttendance(string.Format("{0:dd-MMM-yyyy}", StartDate), string.Format("{0:dd-MMM-yyyy}", EndDate), UserId, DepartmentId);
                if (Model.EmpDailyDetailAttendanceList.Count == 0)
                    ModelState.AddModelError("", "No records found");
                else
                {
                    ModelState.AddModelError("", "No Records Found");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(Model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult MonthlyAttendance(DateTime? StartDate, DateTime? EndDate, string SortOrder, string SortBy, int? PageNumber)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            if (PageNumber == 0 || PageNumber == null)
                PageNumber = 1;
            if (SortOrder == null && SortBy == null && StartDate == null && EndDate == null)
            {
                EmpMonthlyAttendance Model = new EmpMonthlyAttendance();
                try
                {
                    Model.StartDate = DateTime.Now.AddDays(-30);
                    Model.EndDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                return View(Model);
            }
            else
            {
                EmpMonthlyAttendance Model = new EmpMonthlyAttendance();
                Model.StartDate = Convert.ToDateTime(StartDate);
                Model.EndDate = Convert.ToDateTime(EndDate);
                ViewBag.SortOrder = SortOrder;
                ViewBag.PageNumber = PageNumber;
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
                int UserId = AuthBase.UserId;
                string StartDate2 = Convert.ToString(Model.StartDate);
                string EndDate2 = Convert.ToString(Model.EndDate);
                List<UserDepartment> userDepartments = objUserDepartmentService.GetUserDepartmentByUserId(UserId).Where(x => x.EffectiveDate >= Convert.ToDateTime(StartDate2) && x.RetiredDate <= Convert.ToDateTime(EndDate2) || x.RetiredDate == null).ToList();
                UserDepartment userDepartment = userDepartments.FirstOrDefault();
                var DepartmentId = userDepartment.DepartmentId;
                try
                {
                    if (Model.StartDate != null)
                        Model.EmpMonthlyDetailAttendanceList = objAttendanceService.GetEmpMonthlyAttendance(string.Format("{0:dd-MMM-yyyy}", StartDate2), string.Format("{0:dd-MMM-yyyy}", EndDate2), UserId, DepartmentId);
                    if (Model.EmpMonthlyDetailAttendanceList.Count == 0)
                        ModelState.AddModelError("", "No records found");
                    else
                    {
                        #region Searching
                        //if (SearchText != null)
                        //{
                        //    model.PracticeVMReportList = model.PracticeVMReportList.Where(x => x.FirstName.Contains(SearchText) || x.MiddleName.Contains(SearchText) || x.DepartmentName.Contains(SearchText)).ToList();
                        //}
                        #endregion Searching
                        #region Pagination
                        double PageCount = Model.EmpMonthlyDetailAttendanceList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        Model.EmpMonthlyDetailAttendanceList = Model.EmpMonthlyDetailAttendanceList.Skip((Convert.ToInt32(PageNumber - 1) )* 10).Take(10).ToList();
                        #endregion Pagination
                        #region Sorting
                        if (SortBy == "Date")
                        {
                            if (SortOrder == "Asc")
                            {
                                Model.EmpMonthlyDetailAttendanceList = Model.EmpMonthlyDetailAttendanceList.OrderBy(x => x.AttendanceDate).ToList();

                            }
                            else if (SortOrder == "Desc")
                            {
                                Model.EmpMonthlyDetailAttendanceList = Model.EmpMonthlyDetailAttendanceList.OrderByDescending(x => x.AttendanceDate).ToList();

                            }
                        }
                        #endregion Sorting
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                //return Json(new { data = Model.EmpMonthlyDetailAttendanceList }, JsonRequestBehavior.AllowGet);
                return View(Model);
            }
        }
        [HttpPost]
        [Authenticate]
        public ActionResult MonthlyAttendance(EmpMonthlyAttendance Model, string SortOrder, string SortBy, int PageNumber = 1)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            int UserId = AuthBase.UserId;
            string StartDate = Convert.ToString(Model.StartDate);
            string EndDate = Convert.ToString(Model.EndDate);
            List<UserDepartment> userDepartments = objUserDepartmentService.GetUserDepartmentByUserId(UserId).Where(x => x.EffectiveDate >= Convert.ToDateTime(StartDate) && x.RetiredDate <= Convert.ToDateTime(EndDate) || x.RetiredDate == null).ToList();
            UserDepartment userDepartment = userDepartments.FirstOrDefault();
            var DepartmentId = userDepartment.DepartmentId;
            try
            {
                if (Model.StartDate != null)
                    Model.EmpMonthlyDetailAttendanceList = objAttendanceService.GetEmpMonthlyAttendance(string.Format("{0:dd-MMM-yyyy}", StartDate), string.Format("{0:dd-MMM-yyyy}", EndDate), UserId, DepartmentId);
                if (Model.EmpMonthlyDetailAttendanceList.Count == 0)
                    ModelState.AddModelError("", "No records found");
                else
                {
                    #region Searching
                    //if (SearchText != null)
                    //{
                    //    model.PracticeVMReportList = model.PracticeVMReportList.Where(x => x.FirstName.Contains(SearchText) || x.MiddleName.Contains(SearchText) || x.DepartmentName.Contains(SearchText)).ToList();
                    //}
                    #endregion Searching
                    #region Pagination
                    double PageCount = Model.EmpMonthlyDetailAttendanceList.Count;
                    ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                    Model.EmpMonthlyDetailAttendanceList = Model.EmpMonthlyDetailAttendanceList.Skip((PageNumber - 1) * 10).Take(10).ToList();
                    #endregion Pagination
                    #region Sorting
                    if (SortBy == "Date")
                    {
                        if (SortOrder == "Asc")
                        {
                            Model.EmpMonthlyDetailAttendanceList = Model.EmpMonthlyDetailAttendanceList.OrderBy(x => x.AttendanceDate).ToList();

                        }
                        else if (SortOrder == "Desc")
                        {
                            Model.EmpMonthlyDetailAttendanceList = Model.EmpMonthlyDetailAttendanceList.OrderByDescending(x => x.AttendanceDate).ToList();

                        }
                    }
                    #endregion Sorting
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            //return Json(new { data = Model.EmpMonthlyDetailAttendanceList }, JsonRequestBehavior.AllowGet);
            return View(Model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult MonthlyDetailAttendace(string StartDate, string EndDate)
        {
            EmpMonthlyAttendance Model = new EmpMonthlyAttendance();
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            int UserId = AuthBase.UserId;
            List<UserDepartment> userDepartments = objUserDepartmentService.GetUserDepartmentByUserId(UserId).Where(x => x.EffectiveDate >= Convert.ToDateTime(StartDate) && x.RetiredDate <= Convert.ToDateTime(EndDate) || x.RetiredDate == null).ToList();
            UserDepartment userDepartment = userDepartments.FirstOrDefault();
            var DepartmentId = userDepartment.DepartmentId;
            try
            {
                if (StartDate != null)
                    Model.EmpMonthlyDetailAttendanceList = objAttendanceService.GetEmpMonthlyAttendance(string.Format("{0:dd-MMM-yyyy}", StartDate), string.Format("{0:dd-MMM-yyyy}", EndDate), UserId, DepartmentId);
                if (Model.EmpMonthlyDetailAttendanceList.Count == 0)
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(Model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult AbsentReport(DateTime? StartDate, DateTime? EndDate, string SortOrder, string SortBy, int? PageNumber)
        {
            IUserDepartmentService userDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            if (PageNumber == 0 || PageNumber == null)
                PageNumber = 1;
            if (SortOrder == null && SortBy == null && StartDate == null && EndDate == null)
            {
                VMEmpAbsent model = new VMEmpAbsent();
                try
                {
                    model.EndDate = model.StartDate = DateTime.Now;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId); //TODO
                return View(model);
            }
            else
            {
                VMEmpAbsent model = new VMEmpAbsent();
                model.StartDate = Convert.ToDateTime(StartDate);
                model.EndDate = Convert.ToDateTime(EndDate);
                ViewBag.SortOrder = SortOrder;
                ViewBag.PageNumber = PageNumber;
                try
                {
                    IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                    model.VMAbsentReportList = new List<VMAbsentReport>();
                    if (model.StartDate != null)
                        model.VMAbsentReportList = objAttendanceService.GetAbsentReport(model.StartDate, model.EndDate, AuthBase.UserId, model.DepartmentId);
                    if (model.UserName != null)
                        model.VMAbsentReportList = model.VMAbsentReportList.Where(x => x.EmployeeFName.ToLower().Contains(model.UserName.ToLower())).ToList();

                    if (model.VMAbsentReportList.Count == 0)
                        ModelState.AddModelError("", "No Records Found");
                    else
                    {
                        #region Searching
                        //if (SearchText != null)
                        //{
                        //    model.PracticeVMReportList = model.PracticeVMReportList.Where(x => x.FirstName.Contains(SearchText) || x.MiddleName.Contains(SearchText) || x.DepartmentName.Contains(SearchText)).ToList();
                        //}
                        #endregion Searching
                        #region Pagination
                        double PageCount = model.VMAbsentReportList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        model.VMAbsentReportList = model.VMAbsentReportList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                        #endregion Pagination
                        #region Sorting
                        if (SortBy == "Date")
                        {
                            if (SortOrder == "Asc")
                            {
                                model.VMAbsentReportList = model.VMAbsentReportList.OrderBy(x => x.AttendanceDate).ToList();

                            }
                            else if (SortOrder == "Desc")
                            {
                                model.VMAbsentReportList = model.VMAbsentReportList.OrderByDescending(x => x.AttendanceDate).ToList();

                            }
                        }
                        #endregion Sorting
                    }


                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId);
                //DailyAttendanceUpdation(model);
                return View(model);
            }
        }
        [HttpPost]
        [Authenticate]
        public ActionResult AbsentReport(VMEmpAbsent model, string SortOrder, string SortBy, int PageNumber = 1)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                model.VMAbsentReportList = new List<VMAbsentReport>();

                if (model.StartDate != null)
                    model.VMAbsentReportList = objAttendanceService.GetAbsentReport(model.StartDate, model.EndDate, AuthBase.UserId, model.DepartmentId);
                if (model.UserName != null)
                    model.VMAbsentReportList = model.VMAbsentReportList.Where(x => x.EmployeeFName.ToLower().Contains(model.UserName.ToLower())).ToList();

                if (model.VMAbsentReportList.Count == 0)
                    ModelState.AddModelError("", "No Records Found");
                     else
                {
                        #region Searching
                        //if (SearchText != null)
                        //{
                        //    model.PracticeVMReportList = model.PracticeVMReportList.Where(x => x.FirstName.Contains(SearchText) || x.MiddleName.Contains(SearchText) || x.DepartmentName.Contains(SearchText)).ToList();
                        //}
                        #endregion Searching
                        #region Pagination
                        double PageCount = model.VMAbsentReportList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        model.VMAbsentReportList = model.VMAbsentReportList.Skip((PageNumber - 1) * 10).Take(10).ToList();
                        #endregion Pagination
                        #region Sorting
                        if (SortBy == "Date")
                        {
                            if (SortOrder == "Asc")
                            {
                                model.VMAbsentReportList = model.VMAbsentReportList.OrderBy(x => x.AttendanceDate).ToList();

                            }
                            else if (SortOrder == "Desc")
                            {
                                model.VMAbsentReportList = model.VMAbsentReportList.OrderByDescending(x => x.AttendanceDate).ToList();

                            }
                        }
                        #endregion Sorting
                    }
                

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId);
            //DailyAttendanceUpdation(model);
            return View(model);

        }
        [HttpGet]
        [Authenticate]
        public ActionResult HolidayList(int? PageNumber)
        {
            
            if (PageNumber == 0 || PageNumber == null)
                PageNumber = 1;
            else
                ViewBag.PageNumber = PageNumber;
            VMHolidayModel model = new VMHolidayModel();
            try
            {
                IHolidayService objHolidayService = IoC.Resolve<IHolidayService>("HolidayService");
                List<Holiday> HolidayList = objHolidayService.GetAllHoliday();
                if (HolidayList != null && HolidayList.Count > 0)
                {
                    HolidayList = HolidayList.Where(x => x.IsActive == true).ToList();
                    if (HolidayList.Count > 0)
                    {
                        model.HolidayList = HolidayList;
                    }
                    else
                        ModelState.AddModelError("", "No Records Found");
                    if(HolidayList.Count>0 || HolidayList != null)
                    { 
                    #region Pagination
                    double PageCount = model.HolidayList.Count;
                    ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                    model.HolidayList = model.HolidayList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                    #endregion Pagination
                    }
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
        [HttpGet]
        [Authenticate]
        public ActionResult TicketHistory(DateTime? StartDate, DateTime? EndDate, string SortOrder, string SortBy, int? PageNumber)
        {
            if (PageNumber == 0 || PageNumber == null)
                PageNumber = 1;
            if (SortOrder == null && SortBy == null && StartDate == null && EndDate == null)
            {
                VMEmpTicketFilter model = new VMEmpTicketFilter();
                try
                {
                    model.StartDate = DateTime.Now.AddMonths(-1).AddDays(-30);
                    model.EndDate = DateTime.Now.AddMonths(-1);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

                GetUsersAndDepartments(AuthBase.UserId, model.DepartmentId); //TODO
                return View(model);
            }
            else
            {
                ViewBag.SortOrder = SortOrder;
                ViewBag.PageNumber = PageNumber;
                VMEmpTicketFilter model = new VMEmpTicketFilter();
                model.StartDate = Convert.ToDateTime(StartDate);
                model.EndDate = Convert.ToDateTime(EndDate);
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                model.VMTicketHistoryList = objPayrollService.GetTicketsByDateRange(model.StartDate, model.EndDate, AuthBase.UserId, model.DepartmentId);
                if (model.VMTicketHistoryList == null)
                    ModelState.AddModelError("", "No Records Found");
                else
                {
                    #region Searching
                    //if (SearchText != null)
                    //{
                    //    model.PracticeVMReportList = model.PracticeVMReportList.Where(x => x.FirstName.Contains(SearchText) || x.MiddleName.Contains(SearchText) || x.DepartmentName.Contains(SearchText)).ToList();
                    //}
                    #endregion Searching
                    #region Pagination
                    double PageCount = model.VMTicketHistoryList.Count;
                    ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                    model.VMTicketHistoryList = model.VMTicketHistoryList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                    #endregion Pagination
                    #region Sorting
                    if (SortBy == "Date")
                    {
                        if (SortOrder == "Asc")
                        {
                            model.VMTicketHistoryList = model.VMTicketHistoryList.OrderBy(x => x.AttendanceDate).ToList();

                        }
                        else if (SortOrder == "Desc")
                        {
                            model.VMTicketHistoryList = model.VMTicketHistoryList.OrderByDescending(x => x.AttendanceDate).ToList();

                        }
                    }
                    #endregion Sorting
                }
                GetUsersAndDepartments(AuthBase.UserId, model.DepartmentId); //TODO
                return View(model);
            }
        }
        [HttpPost]
        [Authenticate]
        public ActionResult TicketHistory(VMEmpTicketFilter model, string SortOrder, string SortBy, int? PageNumber)
        {
            ViewBag.SortOrder = SortOrder;
            ViewBag.PageNumber = PageNumber;
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            model.VMTicketHistoryList = objPayrollService.GetTicketsByDateRange(model.StartDate, model.EndDate, AuthBase.UserId, model.DepartmentId);
            if (model.VMTicketHistoryList == null)
                ModelState.AddModelError("", "No Records Found");
            else
            {
                if (PageNumber == 0 || PageNumber == null)
                    PageNumber = 1;
                #region Searching
                //if (SearchText != null)
                //{
                //    model.PracticeVMReportList = model.PracticeVMReportList.Where(x => x.FirstName.Contains(SearchText) || x.MiddleName.Contains(SearchText) || x.DepartmentName.Contains(SearchText)).ToList();
                //}
                #endregion Searching
                #region Pagination
                double PageCount = model.VMTicketHistoryList.Count;
                ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                model.VMTicketHistoryList = model.VMTicketHistoryList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                #endregion Pagination
                #region Sorting
                if (SortBy == "Date")
                {
                    if (SortOrder == "Asc")
                    {
                        model.VMTicketHistoryList = model.VMTicketHistoryList.OrderBy(x => x.AttendanceDate).ToList();

                    }
                    else if (SortOrder == "Desc")
                    {
                        model.VMTicketHistoryList = model.VMTicketHistoryList.OrderByDescending(x => x.AttendanceDate).ToList();

                    }
                }
                #endregion Sorting
            }
            GetUsersAndDepartments(AuthBase.UserId, model.DepartmentId); //TODO
            return View(model);
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
        private void GetDailyAttendanceUpdateUsersPractice(int? UserId, int? DepartmentId)
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
        private void ViewPayrollPrerequisiteData()
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            List<PayrollCycle> PayrollCycleList = objPayrollCycleService.GetAllPayrollCycle().OrderByDescending(x=>x.Id).ToList();
            if (PayrollCycleList != null && PayrollCycleList.Count > 0)
                ViewBag.PayrollCycle = new SelectList(PayrollCycleList, "Id", "Name");

            IPayrollVariableService objPayrollVariableService = IoC.Resolve<IPayrollVariableService>("PayrollVariableService");
            List<PayrollVariable> PayrollVariableList = objPayrollVariableService.GetAllPayrollVariable();
            if (PayrollVariableList != null && PayrollVariableList.Count > 0)
                ViewBag.PayrollVariable = new SelectList(PayrollVariableList, "Id", "Name");
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Payroll(int? PageNumber)
        {
            if (PageNumber == 0 || PageNumber == null)
            {
                PageNumber = 1;
            
                ViewPayrollPrerequisiteData();
            VMEmpPayroll model = new VMEmpPayroll();
            return View(model);
            }
            else
            {
                VMEmpPayroll model = new VMEmpPayroll();
                ViewBag.PageNumber = PageNumber;
                IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
                try
                {
                    model.VMEmpPayrollServiceList = new List<VMEmpPayrollService>();
                    model.VMEmpPayrollServiceList = objPayrollService.GetEmpPayrollByPayrollCycleId(model.PayrollCycleId, AuthBase.UserId);
                    if (model.VMEmpPayrollServiceList.Count == 0)
                        ModelState.AddModelError("", "No Records Found");
                    else
                    {
                        #region Pagination
                        double PageCount = model.VMEmpPayrollServiceList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        model.VMEmpPayrollServiceList = model.VMEmpPayrollServiceList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                        #endregion Pagination
                    }
                }
                catch (Exception ex)

                {
                    ModelState.AddModelError("", ex.Message);
                }
                ViewPayrollPrerequisiteData();
                return View(model);
            }
        }
        [HttpPost]
        [Authenticate]
        public ActionResult Payroll(VMEmpPayroll model,Int32 PageNumber = 1)
        {
            ViewBag.PageNumber = PageNumber;
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            try
            {
                model.VMEmpPayrollServiceList = new List<VMEmpPayrollService>();
                model.VMEmpPayrollServiceList = objPayrollService.GetEmpPayrollByPayrollCycleId(model.PayrollCycleId,AuthBase.UserId);
                if (model.VMEmpPayrollServiceList.Count == 0)
                    ModelState.AddModelError("", "No Records Found");
                else
                {
                        #region Pagination
                        double PageCount = model.VMEmpPayrollServiceList.Count;
                        ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                        model.VMEmpPayrollServiceList = model.VMEmpPayrollServiceList.Skip((PageNumber - 1) * 10).Take(10).ToList();
                        #endregion Pagination
                    }
                }
            catch (Exception ex)

            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewPayrollPrerequisiteData();
            return View(model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult PayrollDetail(Int32? PayrollCycleId,Int32?UserId)
        {
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            VMEMPPayslipDetail model = new VMEMPPayslipDetail();
            try
            {
                model.VMEmpPayslipDetailUserList = new List<VMEmpPayslipDetailUser>();
                if (PayrollCycleId != null && UserId != null)
                    model.VMEmpPayslipDetailUserList = objPayrollService.GetEmpPayslip(PayrollCycleId, UserId);

                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)

            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult PayslipVariableInformation(VMPayslipVariables model)
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
            IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
            model.PayrollId = model.PayrollId.Replace(",", "");
            bool? Early = false;
            bool? Late = false;
            if (model.IsEarly == "Early" && model.IsLate == "Early")
            {
                Early = true;
                Late = null;
            }
            else if (model.IsEarly == "Late" && model.IsLate == "Late")
            {
                Late = true;
                Early = null;
            }
            try
            {
                Payroll payroll = objPayrollService.GetPayroll(Convert.ToInt32(model.PayrollId)); //PickByPayrollID
                PayrollCycle payrollCycle = objPayrollCycleService.GetPayrollCycle((int)payroll.PayrollCycleId); //PickByPayrollCycleID
                int PayrollCycleMonth = (int)payrollCycle.Month;
                int PayrollCycleYear = (int)payrollCycle.Year;
                int PayrollCycleMonthLastDay = DateTime.DaysInMonth(PayrollCycleYear, PayrollCycleMonth);
                int PayrollCycleMonthStartDay = 1;
                var StartDate1 = new DateTime(PayrollCycleYear, PayrollCycleMonth, PayrollCycleMonthStartDay);
                var EndDate1 = new DateTime(PayrollCycleYear, PayrollCycleMonth, PayrollCycleMonthLastDay);
                string StartDate = StartDate1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string EndDate = EndDate1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                model.VMPayslipVariableInformationList = objPayrollPolicyService.GetPayrollPolicyInformation(model.UserId, Early, Late, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            var json = JsonConvert.SerializeObject(model.VMPayslipVariableInformationList);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult PayslipAbsentInformation(VMPayslipVariables model)
        {
            IPayrollCycleService objPayrollCycleService = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            IPayrollService objPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            IPayrollDetailService objPayrollDetailService = IoC.Resolve<IPayrollDetailService>("PayrollDetailService");
            IPayrollPolicyService objPayrollPolicyService = IoC.Resolve<IPayrollPolicyService>("PayrollPolicyService");
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            model.PayrollId = model.PayrollId.Replace(",", "");
            try
            {
                Payroll payroll = objPayrollService.GetPayroll(Convert.ToInt32(model.PayrollId)); //PickByPayrollID
                PayrollCycle payrollCycle = objPayrollCycleService.GetPayrollCycle((int)payroll.PayrollCycleId); //PickByPayrollCycleID
                int PayrollCycleMonth = (int)payrollCycle.Month;
                int PayrollCycleYear = (int)payrollCycle.Year;
                int PayrollCycleMonthLastDay = DateTime.DaysInMonth(PayrollCycleYear, PayrollCycleMonth);
                int PayrollCycleMonthStartDay = 1;
                var StartDate1 = new DateTime(PayrollCycleYear, PayrollCycleMonth, PayrollCycleMonthStartDay);
                var EndDate1 = new DateTime(PayrollCycleYear, PayrollCycleMonth, PayrollCycleMonthLastDay);
                string StartDate = StartDate1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string EndDate = EndDate1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                model.VMPayslipAbsentInformationList = objAttendanceService.GetAbsentReportEmployee(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), model.UserId, model.DepartmentId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            var json = JsonConvert.SerializeObject(model.VMPayslipAbsentInformationList);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authenticate]
        public JsonResult SentTicket(VMPayslipVariableInformation model)
        {
            ITicketService objTicketService = IoC.Resolve<ITicketService>("TicketService");
            #region Ticket Entry
            Ticket ticket = null;
            ticket = new Ticket()
            {
                UserID = AuthBase.UserId,
                Reason = model.Reason,
                IsApproved = false,
                IsReject = false,
                Comments = null,
                CreationDate = DateTime.Now,
                UpdateDate = null,
                UpdateBy = AuthBase.UserId,
                UserIp = Request.UserHostAddress,
                AttendanceID = model.AttendanceID
            };
            ticket = objTicketService.InsertTicket(ticket);
            #endregion Ticket Entry
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        private void AssignShiftPrerequisiteData()
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

            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            List<Shift> ShiftList = objShiftService.GetAllShift(AuthBase.BranchId);
            if (ShiftList != null && ShiftList.Count > 0)
                ViewBag.Shift = new SelectList(ShiftList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Shift", Value = "-1" });
                ViewBag.Shift = new SelectList(blank, "Value", "Text", "");
            }
        }
        [HttpGet]
        [Authenticate]
        public ActionResult CurrentShift(int? UserId)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
            IShiftOffDayService objShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
            UserId = AuthBase.UserId;
            VMEmpShift model = new VMEmpShift();
            try
            {
                List<UserShift> UserShiftList = objUserShiftService.GetUserShiftByUserId(AuthBase.UserId).Where(x => x.EffectiveDate >= DateTime.Now && x.RetiredDate <= DateTime.Now || x.RetiredDate == null).ToList();
                UserShift userShift = UserShiftList.FirstOrDefault();
                List<ShiftOffDay> ShiftOffDayList = objShiftOffDayService.GetShiftOffDayByShiftId(userShift.ShiftId).ToList();


                Shift shift = objShiftService.GetShift(Convert.ToInt32(userShift.ShiftId));
                if (shift.IsActive == true)
                {
                    model.ShiftId = shift.Id;
                    model.ShiftName = shift.Name;
                    model.StartHour = shift.StartHour;
                    model.EndHour = shift.EndHour;
                    model.BreakHour = Convert.ToString(shift.BreakHour);
                }
                model.VMEmpShiftOffDaysList = new List<VMEmpShiftOffDays>();

                foreach (ShiftOffDay shiftOffDay in ShiftOffDayList)
                {
                if (shiftOffDay.OffDayOfWeek == 7)
                   shiftOffDay.OffDayOfWeek = 0;
                model.ShiftOffDaysInWords += Enum.GetName(typeof(System.DayOfWeek), shiftOffDay.OffDayOfWeek) + ", ";
                }
                if (shift.OffDays != null)
                  model.ShiftOffDaysInWords = shift.OffDays.Substring(0, model.ShiftOffDaysInWords.LastIndexOf(", "));
                }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult ShiftHistory(string Id, int? PageNumber)
        {
            if (PageNumber == null || PageNumber == 0)
                PageNumber = 1;
            ViewBag.PageNumber = PageNumber;
            VMShiftModel model = new VMShiftModel();
            try
            {
                Id = Convert.ToString(AuthBase.UserId);
                if (Id != null && Id != "")
                {
                    IUserShiftService ObjUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
                    IShiftService ObjShiftService = IoC.Resolve<IShiftService>("ShiftService");
                    model.UserShiftList = ObjUserShiftService.GetUserShiftByUserId(int.Parse(Id));
                    if (model.UserShiftList != null && model.UserShiftList.Count > 0)
                    {
                        model.UserShiftList = model.UserShiftList.OrderBy(x => x.EffectiveDate).ToList();
                        foreach (var shift in model.UserShiftList)
                        {
                            shift.Shift = ObjShiftService.GetShift((int)shift.ShiftId);
                        }
                    }
                    else
                        ModelState.AddModelError("", "No Records Found");

                    #region Pagination
                    double PageCount = model.UserShiftList.Count;
                    ViewBag.TotalPages = Math.Ceiling(PageCount / 10);
                    model.UserShiftList = model.UserShiftList.Skip((Convert.ToInt32(PageNumber - 1)) * 10).Take(10).ToList();
                    #endregion Pagination
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult Leave(DateTime? StartDate, DateTime? EndDate,string SortOrder, string SortBy, int? CurrentPageNumber, int? PreviousPageNumber, int? UpcomingPageNumber)
        {
            ViewBag.CurrentPageNumber = CurrentPageNumber;
            ViewBag.PreviousPageNumber = PreviousPageNumber;
            ViewBag.UpcomingPageNumber = UpcomingPageNumber;
            LeaveTypeRepository objleavetype = new LeaveTypeRepository();
            UserDepartment userdept = null;
            UserDepartmentRepository objuserdept = new UserDepartmentRepository();
            VMEmpLeave model = new VMEmpLeave();
            try { 
            userdept = objuserdept.GetUserDepartmentByUserId(AuthBase.UserId).Last();
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");

            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            
            model.NewLeave = new Leave();
            LeaveRepository objleave = new LeaveRepository();

            model.LeaveHistory = objleave.GetLeaveByUserId(AuthBase.UserId);
            
            model.NewLeave.UserId = AuthBase.UserId;

            List<LeaveType> leavetypes = objleavetype.GetAllLeaveType();
            if (leavetypes != null && leavetypes.Count > 0)
                ViewBag.LeaveTypeList = new SelectList(leavetypes, "Id", "Name");

            SelectList sl = new SelectList(leavetypes, "Id", "Name");
            string ssss = sl.Where(x => x.Value == "1").FirstOrDefault().Text;
           
            if(StartDate != null && EndDate !=null)
            {
                model.NewLeave.DateFrom = StartDate;
                model.NewLeave.DateTo = EndDate;
            }

            if(model.LeaveHistory !=null)
            { 
            model.PreviousLeaveHistory = model.LeaveHistory.Where(x => x.Date.Value.Month < DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year || x.Date.Value.Year < DateTime.Now.Year && x.Date.Value > DateTime.Now.AddMonths(-6)).ToList();
            model.UpcomingLeaveHistory = model.LeaveHistory.Where(x => x.Date.Value.Month > DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year ||  x.Date.Value.Year > DateTime.Now.Year).ToList(); 
            model.CurrentLeaveHistory = model.LeaveHistory.Where(x => x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).ToList();

            #region Pagination for PreviousLeaveHistory
            double PageCount1 = model.PreviousLeaveHistory.Count;
            ViewBag.PreviousTotalPages = Math.Ceiling(PageCount1 / 10);
            model.PreviousLeaveHistory = model.PreviousLeaveHistory.Skip((Convert.ToInt32(PreviousPageNumber - 1)) * 10).Take(10).ToList();
            #endregion  Pagination for PreviousLeaveHistory

            #region Pagination for UpcomingLeaveHistory
            double PageCount2 = model.UpcomingLeaveHistory.Count;
            ViewBag.UpcomingTotalPages = Math.Ceiling(PageCount2 / 10);
            model.UpcomingLeaveHistory = model.UpcomingLeaveHistory.Skip((Convert.ToInt32(UpcomingPageNumber - 1)) * 10).Take(10).ToList();
            #endregion  Pagination for UpcomingLeaveHistory

            #region Pagination for CurrentLeaveHistory
            double PageCount3 = model.CurrentLeaveHistory.Count;
            ViewBag.CurrentTotalPages = Math.Ceiling(PageCount3 / 10);
            model.CurrentLeaveHistory = model.CurrentLeaveHistory.Skip((Convert.ToInt32(CurrentPageNumber - 1)) * 10).Take(10).ToList();
            #endregion  Pagination for CurrentLeaveHistory
                }
            }
            catch(Exception ex)
            { 
            ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }
        [HttpPost]
        [Authenticate]
        public ActionResult Leave(VMEmpLeave model, int? CurrentPageNumber, int? PreviousPageNumber, int? UpcomingPageNumber)
        {
            ViewBag.CurrentPageNumber = CurrentPageNumber;
            ViewBag.PreviousPageNumber = PreviousPageNumber;
            ViewBag.UpcomingPageNumber = UpcomingPageNumber;
            PayrollCycle _model = new PayrollCycle();
            LeaveRepository objleave = new LeaveRepository();
            IPayrollCycleService objPayrollCycle = IoC.Resolve<IPayrollCycleService>("PayrollCycleService");
            IPayrollService obPayrollService = IoC.Resolve<IPayrollService>("PayrollService");
            _model = objPayrollCycle.GetAllPayrollCycle().OrderBy(x => x.Year).ThenBy(x => x.Month).Last();
            DateTime dtLastDateOfLastPayroll = new DateTime(_model.Year.Value, _model.Month.Value, 1).AddMonths(1).AddDays(-1);
            model.LeaveHistory = objleave.GetLeaveByUserId(AuthBase.UserId);
            if (model.NewLeave.DateTo.Value == null)
                model.NewLeave.DateTo = model.NewLeave.DateFrom.Value;
            if (ModelState.IsValid)
            {
                 if (model.NewLeave.DateTo.Value < model.NewLeave.DateFrom.Value)
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
                        model.NewLeave.UpdatedBy = null;
                        model.NewLeave.UserIp = Request.UserHostAddress;
                        model.NewLeave.IsApproved = false;
                        model.NewLeave.IsReject = false;
                        if (model.LeaveHistory != null && model.LeaveHistory.Where(x => x.Date.Value == model.NewLeave.Date.Value).Any())
                        {
                            ViewBag.ErrorMessage = "Leave already exists on that date";
                        }
                        else
                        {
                            objleave.InsertLeave(model.NewLeave);
                            ViewBag.SuccessMessage = "Your application is successfully sent";
                        }
                    }
                }
            }
            if (model.LeaveHistory != null)
            {
                model.PreviousLeaveHistory = model.LeaveHistory.Where(x => x.Date.Value.Month < DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year || x.Date.Value.Year < DateTime.Now.Year && x.Date.Value > DateTime.Now.AddMonths(-6)).ToList();
                model.UpcomingLeaveHistory = model.LeaveHistory.Where(x => x.Date.Value.Month > DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year || x.Date.Value.Year > DateTime.Now.Year).ToList();
                model.CurrentLeaveHistory = model.LeaveHistory.Where(x => x.Date.Value.Month == DateTime.Now.Month && x.Date.Value.Year == DateTime.Now.Year).ToList();

                #region Pagination for PreviousLeaveHistory
                double PageCount1 = model.PreviousLeaveHistory.Count;
                ViewBag.PreviousTotalPages = Math.Ceiling(PageCount1 / 10);
                model.PreviousLeaveHistory = model.PreviousLeaveHistory.Skip((Convert.ToInt32(PreviousPageNumber - 1)) * 10).Take(10).ToList();
                #endregion  Pagination for PreviousLeaveHistory

                #region Pagination for UpcomingLeaveHistory
                double PageCount2 = model.UpcomingLeaveHistory.Count;
                ViewBag.UpcomingTotalPages = Math.Ceiling(PageCount2 / 10);
                model.UpcomingLeaveHistory = model.UpcomingLeaveHistory.Skip((Convert.ToInt32(UpcomingPageNumber - 1)) * 10).Take(10).ToList();
                #endregion  Pagination for UpcomingLeaveHistory

                #region Pagination for CurrentLeaveHistory
                double PageCount3 = model.CurrentLeaveHistory.Count;
                ViewBag.CurrentTotalPages = Math.Ceiling(PageCount3 / 10);
                model.CurrentLeaveHistory = model.CurrentLeaveHistory.Skip((Convert.ToInt32(CurrentPageNumber - 1)) * 10).Take(10).ToList();
                #endregion  Pagination for CurrentLeaveHistory
            }
            LeaveTypeRepository objleavetype = new LeaveTypeRepository();
            List<LeaveType> leavetypes = objleavetype.GetAllLeaveType();
            ViewBag.LeaveTypeList = new SelectList(leavetypes, "Id", "Name");
            return View(model);
        }
    }
}