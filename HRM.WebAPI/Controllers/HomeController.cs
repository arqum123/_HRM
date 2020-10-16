using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.IService;
using HRM.Core.Model;
using HRM.Core.Helper;

namespace HRM.WebAPI.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class HomeController : Controller
    {
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authenticate]
        public JsonResult GetDashboardStats()
        {
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");

            List<Attendance> _attendanceList = objAttendanceService.GetAttendanceByDate(DateTime.Now, AuthBase.BranchId);
            //List<Attendance> _attendanceList = objAttendanceService.GetAttendanceByDate(Convert.ToDateTime("04/17/2016"));
            List<AttendanceStatus> _attendanceStatusList = null;
            AttendanceStatus _attendanceStatus = null;

            VMDashboardStats _VMDashboardStats = new VMDashboardStats() { LateCount = 0, LatePercentage = 0, OnTimeCount = 0, OnTimePercentage = 0, PresentCount = 0, PresentPercentage = 0 };
            int userCount = 0, lateCount = 0, absentCount = 0, onTimeCount = 0;
            if (_attendanceList != null)
            {
                foreach (Attendance _attendance in _attendanceList)
                {
                    _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                    if (_attendanceStatusList != null) // Present or Off Day
                    {
                        _attendanceStatus = _attendanceStatusList.FirstOrDefault();

                        // Off/Leave/Holiday
                        if ((_attendanceStatus.IsShiftOffDay.HasValue && _attendanceStatus.IsShiftOffDay.Value)
                            || (_attendanceStatus.IsHoliday.HasValue && _attendanceStatus.IsHoliday.Value)
                            || (_attendanceStatus.IsLeaveDay.HasValue && _attendanceStatus.IsLeaveDay.Value))
                        {
                            // Do Nothing
                        }
                        // Late Present 
                        else if (_attendanceStatus.IsLate.HasValue && _attendanceStatus.IsLate.Value)
                        {
                            userCount++;
                            lateCount++;
                        }
                        // On Time Present
                        else
                        {
                            userCount++;
                            onTimeCount++;
                        }
                    }
                    else // Absent
                    {
                        userCount++;
                        absentCount++;
                    }
                }
                _VMDashboardStats.PresentCount = onTimeCount + lateCount;
                _VMDashboardStats.PresentPercentage = ((onTimeCount + lateCount) * 100) / userCount;

                _VMDashboardStats.OnTimeCount = onTimeCount;
                if (_VMDashboardStats.PresentCount == 0)
                    _VMDashboardStats.OnTimePercentage = 0;
                else
                    _VMDashboardStats.OnTimePercentage = (onTimeCount * 100) / _VMDashboardStats.PresentCount;

                _VMDashboardStats.LateCount = lateCount;
                if (_VMDashboardStats.PresentCount == 0)
                    _VMDashboardStats.LatePercentage = 0;
                else
                    _VMDashboardStats.LatePercentage = (lateCount * 100) / _VMDashboardStats.PresentCount;
            }
            return Json(_VMDashboardStats);
        }

        [Authenticate]
        public JsonResult GetDashboardGraphData()
        {
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");

            List<Attendance> _attendanceList = objAttendanceService.GetAttendanceByDateRange(DateTime.Now.AddDays(-5).Date, DateTime.Now.AddDays(0).Date, AuthBase.BranchId);
            //List<Attendance> _attendanceList = objAttendanceService.GetAttendanceByDateRange(DateTime.Now.AddDays(-6).Date, DateTime.Now.AddDays(-1).Date);
            //List<Attendance> _attendanceList = objAttendanceService.GetAttendanceByDateRange(DateTime.Now.AddDays(-7).Date, DateTime.Now.AddDays(-2).Date);
            List<Attendance> _attendanceListTemp = null;
            List<AttendanceStatus> _attendanceStatusList = null;
            AttendanceStatus _attendanceStatus = null;
            List<VMDashboardStats> _VMDashboardStatsList = new List<VMDashboardStats>();
            VMDashboardStats _VMDashboardStats = null;
            List<DateTime> _dateList = new List<DateTime>();
            //VMDashboardStats _VMDashboardStats = new VMDashboardStats() { LateCount = 0, LatePercentage = 0, OnTimeCount = 0, OnTimePercentage = 0, PresentCount = 0, PresentPercentage = 0 };
            if (_attendanceList != null)
            {
                _attendanceList = _attendanceList.OrderBy(x => x.Date).ToList();
                _dateList = _attendanceList.Select(x => x.Date.Value).Distinct().ToList();
                foreach (DateTime _date in _dateList)
                {
                    int userCount = 0, lateCount = 0, absentCount = 0, onTimeCount = 0;
                    _attendanceListTemp = _attendanceList.Where(x => x.Date.Value == _date).ToList();
                    foreach (Attendance _attendance in _attendanceListTemp)
                    {
                        _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                        if (_attendanceStatusList != null) // Present or Off Day
                        {
                            _attendanceStatus = _attendanceStatusList.FirstOrDefault();

                            // Off/Leave/Holiday
                            if ((_attendanceStatus.IsShiftOffDay.HasValue && _attendanceStatus.IsShiftOffDay.Value)
                                || (_attendanceStatus.IsHoliday.HasValue && _attendanceStatus.IsHoliday.Value)
                                || (_attendanceStatus.IsLeaveDay.HasValue && _attendanceStatus.IsLeaveDay.Value))
                            {
                                // Do Nothing
                            }
                            // Late Present 
                            else if (_attendanceStatus.IsLate.HasValue && _attendanceStatus.IsLate.Value)
                            {
                                userCount++;
                                lateCount++;
                            }
                            // On Time Present
                            else
                            {
                                userCount++;
                                onTimeCount++;
                            }
                        }
                        else // Absent
                        {
                            userCount++;
                            absentCount++;
                        }
                    }
                    _VMDashboardStats = new VMDashboardStats() { dtAttendanceString = _date.ToString() };
                    _VMDashboardStats.PresentCount = onTimeCount + lateCount;
                    _VMDashboardStats.PresentPercentage = userCount >0 ? ((onTimeCount + lateCount) * 100) / userCount : 0;

                    _VMDashboardStats.OnTimeCount = onTimeCount;
                    if (_VMDashboardStats.PresentCount == 0)
                        _VMDashboardStats.OnTimePercentage = 0;
                    else
                        _VMDashboardStats.OnTimePercentage = _VMDashboardStats.PresentCount >0 ?(onTimeCount * 100) / _VMDashboardStats.PresentCount: 0;

                    _VMDashboardStats.LateCount = lateCount;
                    if (_VMDashboardStats.PresentCount == 0)
                        _VMDashboardStats.LatePercentage = 0;
                    else
                        _VMDashboardStats.LatePercentage = _VMDashboardStats.PresentCount > 0 ? (lateCount * 100) / _VMDashboardStats.PresentCount : 0;
                    _VMDashboardStatsList.Add(_VMDashboardStats);
                }

            }
            DateTime dtNow = DateTime.Now.Date;
            int count = 0;
            if (_VMDashboardStatsList != null && _VMDashboardStatsList.Count > 0)
            {
                dtNow = Convert.ToDateTime(_VMDashboardStatsList.Last().dtAttendanceString);
                count = _VMDashboardStatsList.Count;
            }
            for (int i = 1; i <= 6 - count; i++)
            {
                _VMDashboardStats = new VMDashboardStats() { dtAttendanceString = dtNow.AddDays(i * (-1)).ToString() };
                _VMDashboardStats.PresentCount = 0;
                _VMDashboardStats.PresentPercentage = 0;
                _VMDashboardStats.OnTimeCount = 0;
                _VMDashboardStats.OnTimePercentage = 0;
                _VMDashboardStats.LateCount = 0;
                _VMDashboardStats.LatePercentage = 0;
                _VMDashboardStatsList.Add(_VMDashboardStats);
            }
            return Json(_VMDashboardStatsList);
        }

        private bool ToBool(Boolean? obj)
        {
            if (obj == null)
                return false;
            else
                return obj.Value;
        }

        [HttpGet]
        [Authenticate]

        public ActionResult PartialBranch()
        {
            List<Branch> lstBranch = IoC.Resolve<IBranchService>("BranchService").GetAllBranch();
            if (lstBranch != null)
            {
                lstBranch = lstBranch.Where(x => x.IsActive == true).ToList();
                if (lstBranch != null && lstBranch.Count > 0)
                {
                    if (AuthBase.BranchId <= 0)
                    {
                        AuthBase.BranchId = lstBranch.OrderBy(x => x.Id).Take(1).FirstOrDefault().Id;
                        ViewBag.Branch = new SelectList(lstBranch, "ID", "Name");
                    }
                    else
                    {
                        ViewBag.Branch = new SelectList(lstBranch, "ID", "Name", AuthBase.BranchId);
                    }
                }
                else
                {
                    List<SelectListItem> blank = new List<SelectListItem>();
                    ViewBag.Branch = new SelectList(blank, "Value", "Text", "");
                }
            }
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                ViewBag.Branch = new SelectList(blank, "Value", "Text", "");
            }
            return PartialView();
        }

        [HttpPost]
        [Authenticate]
        public ActionResult PartialBranch(Branch _modelBranch)
        {
            string url = _modelBranch.currentURL;
            try
            {
                List<Branch> _branchList = IoC.Resolve<IBranchService>("BranchService").GetAllBranch();
                if (_branchList != null)
                {
                    _branchList = _branchList.Where(x => x.IsActive == true).ToList();
                    if (_branchList != null && _branchList.Count > 0)
                    {
                        if (_modelBranch.Id != 0)
                            AuthBase.BranchId = _modelBranch.Id;

                        if (AuthBase.BranchId <= 0)
                        {
                            ViewBag.Branch = new SelectList(_branchList, "Id", "Name");
                        }
                        else
                        {
                            ViewBag.Branch = new SelectList(_branchList, "Id", "Name", AuthBase.BranchId);
                            _modelBranch = _branchList.Where(x => x.Id == AuthBase.BranchId).FirstOrDefault();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            if (string.IsNullOrWhiteSpace(url))
                //return Redirect("/");
                return PartialView(_modelBranch);
            return Redirect(url);
        }
    }
}