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
using System.IO;
using System.Data;
using System.Text;

namespace HRM.WebAPI.Controllers
{
    public class AttendanceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Daily()
        {
            VMAttendanceModel model = new VMAttendanceModel();
            try
            {
                model.StartDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Daily(VMAttendanceModel model)
        {
            try
            {
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
                model.DepartmentList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);

                if (model.DepartmentID != null)
                {
                    model.DepartmentList = model.DepartmentList.Where(x => x.Id == model.DepartmentID).ToList();
                }

                if (model.DepartmentList != null)
                {
                    Department _allDepartments = new Department() { Id = -1, Name = "All Departments" };
                    _allDepartments.TotalUser = 0; _allDepartments.PresentUser = 0; _allDepartments.OffDayUser = 0; _allDepartments.LateUser = 0; _allDepartments.EarlyUser = 0; _allDepartments.OverTimeUser = 0;
                    _allDepartments.QuarterDayUser = 0; _allDepartments.HalfDayUser = 0; _allDepartments.FullDayUser = 0;

                    foreach (var department in model.DepartmentList)
                    {
                        department.TotalUser = 0; department.PresentUser = 0; department.OffDayUser = 0; department.LateUser = 0; department.EarlyUser = 0; department.OverTimeUser = 0;
                        department.QuarterDayUser = 0; department.HalfDayUser = 0; department.FullDayUser = 0;

                        List<User> UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId).Where(x => x.Department.Id == department.Id).ToList();
                        if (UserList.Count > 0)
                        {
                            //List<User> DepartmentUserList = UserList.Where(x => x.Department.Id == department.Id).ToList();
                            department.TotalUser = UserList.Count();
                            _allDepartments.TotalUser += UserList.Count();
                            foreach (var user in UserList)
                            {
                                List<Attendance> _attendanceList = objAttendanceService.GetAttendanceByUserId(user.Id);
                                Attendance _attendance = new Attendance();

                                if (_attendanceList != null && _attendanceList.Count > 0)
                                {
                                    _attendanceList = _attendanceList.Where(x => x.IsActive == true && x.Date == model.StartDate).ToList();
                                    _attendance = _attendanceList.FirstOrDefault();
                                    if (_attendance != null)
                                    {
                                        List<AttendanceStatus> _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                                        if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                                        {
                                            _attendanceStatusList = _attendanceStatusList.Where(x => x.IsActive == true).ToList();
                                            AttendanceStatus _attendanceStatus = new AttendanceStatus();
                                            if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                                            {
                                                _attendanceStatus = _attendanceStatusList.FirstOrDefault();
                                                if (_attendanceStatus.IsShiftOffDay == false && _attendanceStatus.IsHoliday == false && _attendanceStatus.IsLeaveDay == false)
                                                {
                                                    department.PresentUser++;
                                                    _allDepartments.PresentUser++;
                                                }
                                                if (_attendanceStatus.IsShiftOffDay == true)
                                                {
                                                    department.OffDayUser++;
                                                    _allDepartments.OffDayUser++;
                                                }
                                                if (_attendanceStatus.IsLate == true)
                                                {
                                                    department.LateUser++;
                                                    _allDepartments.LateUser++;
                                                }
                                                if (_attendanceStatus.IsEarly == true)
                                                {
                                                    department.EarlyUser++;
                                                    _allDepartments.EarlyUser++;
                                                }
                                                if (_attendanceStatus.OverTimeMinutes != null && _attendanceStatus.OverTimeMinutes > 0)
                                                {
                                                    department.OverTimeUser++;
                                                    _allDepartments.OverTimeUser++;
                                                }
                                                if (_attendanceStatus.IsQuarterDay == true)
                                                {
                                                    department.QuarterDayUser++;
                                                    _allDepartments.QuarterDayUser++;
                                                    ;
                                                }
                                                if (_attendanceStatus.IsHalfDay == true)
                                                {
                                                    department.HalfDayUser++;
                                                    _allDepartments.HalfDayUser++;
                                                }
                                                if (_attendanceStatus.IsFullDay == true)
                                                {
                                                    department.FullDayUser++;
                                                    _allDepartments.FullDayUser++;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    model.DepartmentList.Add(_allDepartments);
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(model);
        }

        private void GetDailyAttendanceUpdateUsers(int? UserId)
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
        }
        [HttpGet]
        [Authenticate]
        public ActionResult DailyAttendanceUpdate()
        {
            VMDailyAttendanceUpdate model = new VMDailyAttendanceUpdate();
            try
            {
                model.EndDate = model.StartDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetDailyAttendanceUpdateUsers(model.UserId); //TODO
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult DailyAttendanceUpdate(VMDailyAttendanceUpdate model)
        {
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                model.VMDailyAttendanceUpdateStatusList = new List<VMDailyAttendanceUpdateStatus>();
                model.VMDailyAttendanceUpdateTimeList = new List<VMDailyAttendanceUpdateTime>(); //ForDateTimeIn DateTimeOut Values in View

                if (model.StartDate != null && model.StartDate.Year > 2000)
                    model.VMDailyAttendanceUpdateStatusList = objAttendanceService.GetDailyAttendanceUpdateSummary(model.StartDate, model.EndDate, model.UserId);

                else
                    ModelState.AddModelError("", "No Records Found");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetDailyAttendanceUpdateUsers(model.UserId);
            return View(model);

        }

        [HttpPost]
        [Authenticate]
        public ActionResult DailyAttendanceUpdation(List<VMDailyAttendanceUpdateStatus> VMDailyAttendanceUpdateStatusList)
        {
            try
            {
                IAttendanceDetailService objAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
                IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
                IAttendancePolicyService objAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");
                IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                IHolidayService objHolidayService = IoC.Resolve<IHolidayService>("HolidayService");
                IShiftOffDayService objShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");

                foreach (VMDailyAttendanceUpdateStatus models in VMDailyAttendanceUpdateStatusList)
                {
                    foreach (VMDailyAttendanceUpdateTime model in models.VMDailyAttendanceUpdateTimeList)
                    {
                        if(model.IsUpdate == true)
                        { 
                        #region Update AttendanceDetail
                        AttendanceDetail attendanceDetailList = objAttendanceDetailService.GetAttendanceDetail(model.AttendanceDetailId);
                        var AttendanceId = attendanceDetailList.AttendanceId;

                        AttendanceDetail attendanceDetail = new AttendanceDetail()
                        {
                            StartDate = model.DateTimeIn,
                            EndDate = model.DateTimeOut,
                            Id = model.AttendanceDetailId,
                            AttendanceId = attendanceDetailList.AttendanceId,
                            AttendanceTypeId = attendanceDetailList.AttendanceTypeId,
                            IsActive = attendanceDetailList.IsActive,
                            CreationDate = attendanceDetailList.CreationDate,
                            UpdateDate = DateTime.Now,
                            UpdateBy = attendanceDetailList.UpdateBy,
                            UserIp = attendanceDetailList.UserIp,

                        };
                        attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(attendanceDetail);

                        #endregion

                        #region Update Attendance Table by min & max DateTime 

                        List<AttendanceDetail> attendanceDetails = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(attendanceDetailList.AttendanceId);
                        Attendance attendance = objAttendanceService.GetAttendance(attendanceDetailList.AttendanceId.Value);
                        attendance.DateTimeIn = attendanceDetails.Where(x => x.StartDate != null).Min(x => x.StartDate);
                        attendance.DateTimeOut = attendanceDetails.Where(x => x.EndDate != null).Max(x => x.EndDate);
                        attendance.UpdateDate = DateTime.Now;
                        attendance.UpdateBy = AuthBase.UserId;
                        attendance.UserIp = Request.UserHostAddress;

                        attendance = objAttendanceService.UpdateAttendance(attendance);
                        #endregion

                        #region Update AttendanceStatus Table by checking and calculating AttendanceTable


                        AttendanceStatus attendanceStatus = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(attendance.Id).FirstOrDefault();

                        List<UserShift> userShifts = objUserShiftService.GetUserShiftByUserId(attendance.UserId);
                        userShifts = userShifts.Where(x => x.EffectiveDate <= attendance.Date && (x.RetiredDate == null || x.RetiredDate >= attendance.Date)).ToList();
                        UserShift userShift = userShifts.FirstOrDefault();
                        
                        List<AttendancePolicy> attendancePolicies = objAttendancePolicyService.GetAttendancePolicyByShiftId(userShift.ShiftId);
                        attendancePolicies = attendancePolicies.Where(x => x.EffectiveDate <= attendance.Date && (x.RetiredDate == null || x.RetiredDate >= attendance.Date)).ToList();
                        
                        Shift shift = objShiftService.GetShift(userShift.ShiftId.Value);

                        //ShiftOffDays
                        List<ShiftOffDay> shiftOffDays = objShiftOffDayService.GetShiftOffDayByShiftId(userShift.ShiftId);
                        if (shiftOffDays != null)
                        {
                            shiftOffDays = shiftOffDays.Where(x => x.EffectiveDate <= attendance.Date && (x.RetiredDate == null || x.RetiredDate >= attendance.Date)).ToList();

                        }
                        //Holidays
                        List<Holiday> holidays = objHolidayService.GetAllHoliday();
                        if (holidays != null)
                        {
                            holidays = holidays.Where(x => x.Date == attendance.Date).ToList();


                        }
                        //TotalMinutes

                        attendanceStatus.TotalMinutes = Convert.ToInt32((Convert.ToDateTime(attendance.DateTimeOut) - Convert.ToDateTime(attendance.DateTimeIn)).TotalMinutes);


                        //LateMinutes
                        //if (attendanceStatus.LateMinutes == 0)
                        //{

                        double LateMinutes = (attendance.DateTimeIn.Value - Convert.ToDateTime(attendance.Date.Value.ToString("MM/dd/yyyy") + " " + shift.StartHour)).TotalMinutes;
                        if (LateMinutes > 0)
                            attendanceStatus.LateMinutes = Convert.ToInt32(LateMinutes);
                        else
                            attendanceStatus.LateMinutes = 0;
                        //EarlyMinutes &
                        //OverTimeMinutes
                        //}
                        double EarlyMinutes = (Convert.ToDateTime(attendance.Date.Value.ToString("MM/dd/yyyy") + " " + shift.EndHour) - attendance.DateTimeOut.Value).TotalMinutes;
                        if (EarlyMinutes >= 0)
                            attendanceStatus.EarlyMinutes = Convert.ToInt32(EarlyMinutes);
                        else
                        {
                            attendanceStatus.EarlyMinutes = 0;
                            attendanceStatus.OverTimeMinutes = Math.Abs(Convert.ToInt32(EarlyMinutes));
                        }



                        //Working Minutes
                        AttendanceDetail last = attendanceDetails.Last();
                        foreach (AttendanceDetail wm in attendanceDetails)
                        {

                            if (wm.Equals(last))
                            {
                                attendanceStatus.WorkingMinutes += Convert.ToInt32((wm.EndDate.Value - wm.StartDate.Value).TotalMinutes);
                            }

                        }
                        //List<Holiday> holidays = 
                        foreach (AttendancePolicy attendancePolicy in attendancePolicies)
                        {
                            switch ((Core.Enum.AttendanceVariable)attendancePolicy.AttendanceVariableId)
                            {
                                case Core.Enum.AttendanceVariable.FullDay:
                                    //condition full day
                                    if (attendanceStatus.LateMinutes / 60 >= attendancePolicy.Hours.Value || attendanceStatus.EarlyMinutes / 60 <= attendancePolicy.Hours.Value)
                                        attendanceStatus.IsFullDay = true;
                                    else
                                        attendanceStatus.IsFullDay = attendanceStatus.IsFullDay;
                                    break;

                                case Core.Enum.AttendanceVariable.HalfDay:
                                    //condition half day
                                    if (attendanceStatus.LateMinutes / 60 >= attendancePolicy.Hours.Value || attendanceStatus.EarlyMinutes / 60 >= attendancePolicy.Hours.Value)
                                        attendanceStatus.IsHalfDay = true;
                                    else
                                        attendanceStatus.IsHalfDay = attendanceStatus.IsHalfDay;
                                    break;

                                case Core.Enum.AttendanceVariable.QuarterDay:
                                    //condition quarter day
                                    if (attendanceStatus.LateMinutes / 60 >= attendancePolicy.Hours.Value || attendanceStatus.EarlyMinutes / 60 >= attendancePolicy.Hours.Value)
                                        attendanceStatus.IsQuarterDay = true;
                                    else
                                        attendanceStatus.IsQuarterDay = attendanceStatus.IsQuarterDay;
                                    break;

                                case Core.Enum.AttendanceVariable.Late:
                                    //condition IsLate day

                                    if ((decimal)attendanceStatus.LateMinutes / 60 >= (decimal)attendancePolicy.Hours.Value)
                                        attendanceStatus.IsLate = true;
                                    else
                                        attendanceStatus.IsLate = attendanceStatus.IsLate;
                                    break;

                                case Core.Enum.AttendanceVariable.Early:
                                    //condition IsEarly day
                                    if ((decimal)attendanceStatus.EarlyMinutes / 60 >= (decimal)attendancePolicy.Hours.Value)
                                        attendanceStatus.IsEarly = true;
                                    else
                                        attendanceStatus.IsEarly = attendanceStatus.IsEarly;
                                    break;

                            }
                        }

                        attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(attendanceStatus);
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            //return View(VMDailyAttendanceUpdateStatusList);
            return RedirectToAction("DailyAttendanceUpdate","Attendance");
        }
      


        //New MonthlyDetailAttendanceReport
        [HttpGet]
        [Authenticate]
        public ActionResult MonthlyDetailReport(String StartDate, String EndDate, int? UserId, int? DepartId)
        {
            VMAttendanceModel model = new VMAttendanceModel();
            try
            {

                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<VMAttendanceSummary> _attendanceList = new List<VMAttendanceSummary>();

                //if (Date != null && Date > 2000)
                _attendanceList = objAttendanceService.GetMonthlyDetailAttendanceReport(string.Format("{0:dd-MMM-yyyy}", StartDate), string.Format("{0:dd-MMM-yyyy}", EndDate), UserId, DepartId);

                if (_attendanceList != null && _attendanceList.Count > 0)
                {

                    model.AttendanceSummaryList = _attendanceList;
                }
                else
                {
                    ModelState.AddModelError("", "No Records Found");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            AttendancePrerequisiteData(model);
            return View(model);
        }




        // New PracticeMonthlyAttendanceReport
        private void GetDailyAttendanceUpdateUsersPractice(int? UserId,int? DepartmentId)
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
            if (DepartmentId != null && DepartmentId.Value>0)
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


        //New PracticeMonthlyAttendanceReport
        [HttpGet]
        [Authenticate]
        public ActionResult DailyAttendanceReport()
        {
            PracticeVMMonthlyReport model = new PracticeVMMonthlyReport();
            try
            {
                model.StartDate = DateTime.Now;
               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId); //TODO
            return View(model);
        }

        //New PracticeMonthlyAttendanceReport
        [HttpPost]
        [Authenticate]
        public ActionResult DailyAttendanceReport(PracticeVMMonthlyReport model)
        {
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<PracticeVMReport> PracticeVMReportList = new List<PracticeVMReport>();
                model.PracticeVMReportList = new List<PracticeVMReport>();
                model.EndDate = model.StartDate;



                if (model.StartDate != null && model.StartDate.Year > 2000)
                    model.PracticeVMReportList = objAttendanceService.GetMonthlyAttendanceReportPractice(string.Format("{0:dd-MMM-yyyy}", model.StartDate), string.Format("{0:dd-MMM-yyyy}", model.EndDate), model.UserId, model.DepartmentId);

                else
                    ModelState.AddModelError("", "No Records Found");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId); //TODO
            return View(model);
        }

        //New DailyDetailReport
        [HttpGet]
        [Authenticate]
        public ActionResult DailyDetailReport(String Date, int UserId, int DepartId)
        {
            string StartDate = Date;
            string EndDate = Date;
            PracticeVMMonthlyReport model = new PracticeVMMonthlyReport();
            try
            {

                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<PracticeVMReport> _attendanceList = new List<PracticeVMReport>();
                model.PracticeVMReportList = new List<PracticeVMReport>();

                //if (Date != null && Date > 2000)
                model.PracticeVMReportList = objAttendanceService.GetMonthlyDetailAttendanceReportPractice(string.Format("{0:dd-MMM-yyyy}", StartDate), string.Format("{0:dd-MMM-yyyy}", EndDate), UserId, DepartId);

                if (model.PracticeVMReportList != null && _attendanceList.Count > 0)
                {

                    //model.PracticeVMReportList = _attendanceList;
                }
                else
                {
                    ModelState.AddModelError("", "No Records Found");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }


            GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId); //TODO
            return View(model);
        }
        //New PracticeMonthlyAttendanceReport
        [HttpGet]
        [Authenticate]
        public ActionResult MonthlyAttendanceReport()
        {
            PracticeVMMonthlyReport model = new PracticeVMMonthlyReport();
            try
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId); //TODO
            return View(model);
        }

        //New PracticeMonthlyAttendanceReport
       [HttpPost]
        [Authenticate]
        public ActionResult MonthlyAttendanceReport(PracticeVMMonthlyReport model)
        {
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<PracticeVMReport> PracticeVMReportList = new List<PracticeVMReport>();
                model.PracticeVMReportList = new List<PracticeVMReport>();



                if (model.StartDate != null && model.StartDate.Year > 2000)
                    model.PracticeVMReportList = objAttendanceService.GetMonthlyAttendanceReportPractice(string.Format("{0:dd-MMM-yyyy}", model.StartDate), string.Format("{0:dd-MMM-yyyy}", model.EndDate), model.UserId, model.DepartmentId);

                else
                    ModelState.AddModelError("", "No Records Found");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId); //TODO
            return View(model);
        }

        // New PracticeMonthlyDetailReport
        [HttpGet]
        [Authenticate]
        public ActionResult PracticeMonthlyDetailReport(String StartDate, String EndDate, int? UserId, int? DepartId)
        {
        
            PracticeVMMonthlyReport model = new PracticeVMMonthlyReport();
            try
            {

                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<PracticeVMReport> _attendanceList = new List<PracticeVMReport>();
                model.PracticeVMReportList = new List<PracticeVMReport>();

                //if (Date != null && Date > 2000)
                model.PracticeVMReportList = objAttendanceService.GetMonthlyDetailAttendanceReportPractice(string.Format("{0:dd-MMM-yyyy}", StartDate), string.Format("{0:dd-MMM-yyyy}", EndDate), UserId, DepartId);

                if (model.PracticeVMReportList != null && _attendanceList.Count > 0)
                {

                    //model.PracticeVMReportList = _attendanceList;
                }
                else
                {
                    ModelState.AddModelError("", "No Records Found");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }


            GetDailyAttendanceUpdateUsersPractice(model.UserId, model.DepartmentId); //TODO
            return View(model);
        }



        [HttpGet]
        [Authenticate]
        public ActionResult AbsentReport()
        {
            VMAbsent model = new VMAbsent();
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

        [HttpPost]
        [Authenticate]
        public ActionResult AbsentReport(VMAbsent model)
        {
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                model.VMAbsentReportList = new List<VMAbsentReport>();

                //if (model.StartDate != null && model.StartDate.Year > 2000)
                    model.VMAbsentReportList = objAttendanceService.GetAbsentReport(model.StartDate, model.EndDate, model.UserId,model.DepartmentId);

                if (model.VMAbsentReportList == null)
                    ModelState.AddModelError("", "No Records Found");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            GetDailyAttendanceUpdateUsersPractice(model.UserId,model.DepartmentId);
            //DailyAttendanceUpdation(model);
            return View(model);

        }
        [HttpGet]
        [Authenticate]
        public ActionResult DailySummary()
        {
            VMAttendanceModel model = new VMAttendanceModel();
            try
            {
                model.StartDate = DateTime.Now;
                #region
                StringBuilder builder = new StringBuilder();
                builder.Append("/Reports/web_forms/daily_summary.aspx?");
                builder.Append("dpt_id=" + model.DepartmentID);
                builder.Append("&sd=" + model.StartDate);
                builder.Append("&br_id=" + model.BranchId);
                builder.Append("&sal_id=" + model.SalaryTypeId);
                builder.Append("&shf_id=" + model.ShiftId);
                ViewBag.DailySummmaryReportUrl = builder.ToString();
                #endregion

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(model);
        }


        [HttpPost]
        [Authenticate]
        public ActionResult DailySummary(VMAttendanceModel model)
        {
            try
            {
                #region Department Filter
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                List<Department> AllDepartmentList = objDepartmentService.GetAllDepartment();
                string deptName = null;
                if (model.DepartmentID.HasValue && model.DepartmentID.Value > 0)
                    deptName = AllDepartmentList.Where(x => x.Id == model.DepartmentID).FirstOrDefault().Name;
                #endregion Department Filter;

                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<VMAttendanceSummary> _attendanceList = new List<VMAttendanceSummary>();

                if (model.StartDate != null && model.StartDate.Year > 2000)
                    _attendanceList = objAttendanceService.GetDailyAttendanceSummary(string.Format("{0:dd-MMM-yyyy}", model.StartDate), string.Format("{0:dd-MMM-yyyy}", model.StartDate), model.BranchId, model.UserId, deptName, model.ShiftId, model.SalaryTypeId);

                if (_attendanceList != null && _attendanceList.Count > 0)
                {
                    _attendanceList.Add(new VMAttendanceSummary()
                    {
                        BranchId = model.BranchId.HasValue ? model.BranchId.Value : 0,
                        DepartmentId = model.DepartmentID.HasValue ? model.DepartmentID.Value : 0,
                        BranchName = "All",
                        DepartmentName = "All",
                        Early = _attendanceList.Sum(x => x.Early),
                        Late = _attendanceList.Sum(x => x.Late),
                        FullDay = _attendanceList.Sum(x => x.FullDay),
                        OffDay = _attendanceList.Sum(x => x.OffDay),
                        OverTime = _attendanceList.Sum(x => x.OverTime),
                        Present = _attendanceList.Sum(x => x.Present),
                        Total = _attendanceList.Sum(x => x.Total)
                    });
                    model.AttendanceSummaryList = _attendanceList;
                }
                else
                    ModelState.AddModelError("", "No Records Found");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            AttendancePrerequisiteData(model);

            #region
            StringBuilder builder = new StringBuilder();
            builder.Append("/Reports/web_forms/daily_summary.aspx?");
            builder.Append("dpt_id=" + model.DepartmentID);
            builder.Append("&sd=" + model.StartDate);
            builder.Append("&br_id=" + model.BranchId);
            builder.Append("&sal_id=" + model.SalaryTypeId);
            builder.Append("&shf_id=" + model.ShiftId);
            ViewBag.DailySummmaryReportUrl = builder.ToString();
            #endregion
            return View(model);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult DailyDetail_slow(string departmentId, string date)
        {
            VMAttendanceModel VMAttendanceModel = new VMAttendanceModel();
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<Attendance> _attendanceList = new List<Attendance>();
                if (date != null && date != "")
                    _attendanceList = objAttendanceService.GetAllAttendanceWithAttendanceDetail(string.Format("{0:dd-MMM-yyyy}", date), string.Format("{0:dd-MMM-yyyy}", date));

                if (departmentId != null && departmentId != "" && _attendanceList != null && _attendanceList.Where(x => x.User.UserDepartment.DepartmentId == (int.Parse(departmentId) == -1 ? x.User.UserDepartment.DepartmentId : int.Parse(departmentId))) != null && _attendanceList.Where(x => x.User.UserDepartment.DepartmentId == (int.Parse(departmentId) == -1 ? x.User.UserDepartment.DepartmentId : int.Parse(departmentId))).Any())
                    VMAttendanceModel.AttendanceList = _attendanceList.Where(x => x.User.UserDepartment.DepartmentId == (int.Parse(departmentId) == -1 ? x.User.UserDepartment.DepartmentId : int.Parse(departmentId))).OrderBy(x => x.Date).ToList();
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(VMAttendanceModel);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult DailyDetail(string departmentId, string date, string branchId = null)
        {
            VMAttendanceModel VMAttendanceModel = new VMAttendanceModel();
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<Attendance> _attendanceList = new List<Attendance>();


                if (date != null && date != "")
                    _attendanceList = objAttendanceService.GetAllAttendanceByDateUserWithDetail(string.Format("{0:dd-MMM-yyyy}", date), string.Format("{0:dd-MMM-yyyy}", date), null, branchId == null ? AuthBase.BranchId : Convert.ToInt32(branchId));

                if (departmentId != null && departmentId != "" && _attendanceList != null && _attendanceList.Where(x => x.User.Department.Id == (int.Parse(departmentId) == -1 ? x.User.Department.Id : int.Parse(departmentId))) != null && _attendanceList.Where(x => x.User.Department.Id == (int.Parse(departmentId) == -1 ? x.User.Department.Id : int.Parse(departmentId))).Any())
                    VMAttendanceModel.AttendanceList = _attendanceList.Where(x => x.User.Department.Id == (int.Parse(departmentId) == -1 ? x.User.Department.Id : int.Parse(departmentId))).OrderBy(x => x.Date).ToList();
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(VMAttendanceModel);
        }
        [HttpGet]
        [Authenticate]
        public ActionResult DailyDetail2(string departmentId, string date, Int32? branchId = null, string shiftId = null, string salaryTypeId = null)
        {
            VMAttendanceModel VMAttendanceModel = new VMAttendanceModel();
            try
            {
                if (departmentId == "0") departmentId = null;
                if (branchId.HasValue && branchId == 0) branchId = null;
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<Attendance> _attendanceList = new List<Attendance>();
                IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                IBranchService objBranchService = IoC.Resolve<IBranchService>("BranchService");
                ISalaryTypeService objSalaryTypeService = IoC.Resolve<ISalaryTypeService>("SalaryTypeService");
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                #region filterdata
                if (String.IsNullOrEmpty(shiftId))
                    ViewBag.Shift = "All";
                else
                {
                    Shift _Shift = objShiftService.GetShift(Convert.ToInt32(shiftId));
                    ViewBag.Shift = _Shift == null ? "N/A" : _Shift.Name;
                }

                if (String.IsNullOrEmpty(departmentId))
                    ViewBag.Department = "All";
                else
                {
                    Department _Department = objDepartmentService.GetDepartment(Convert.ToInt32(departmentId));
                    ViewBag.Department = _Department == null ? "N/A" : _Department.Name;
                }
                if (String.IsNullOrEmpty(salaryTypeId))
                    ViewBag.SalaryType = "All";
                else
                {
                    SalaryType _SalaryType = objSalaryTypeService.GetSalaryType(Convert.ToInt32(salaryTypeId));
                    ViewBag.SalaryType = _SalaryType == null ? "N/A" : _SalaryType.Name;
                }
                if (!branchId.HasValue)
                    ViewBag.Branch = "All";
                else
                {
                    Branch _Branch = objBranchService.GetBranch(Convert.ToInt32(branchId));
                    ViewBag.Branch = _Branch == null ? "N/A" : _Branch.Name;
                }
                ViewBag.Date = Convert.ToDateTime(date).ToString("MMM dd, yyyy");
                #endregion filterdata

                if (date != null && date != "")
                    _attendanceList = objAttendanceService.GetAllAttendanceByDateUserWithDetail(string.Format("{0:dd-MMM-yyyy}", date), string.Format("{0:dd-MMM-yyyy}", date), null, branchId);

                if (departmentId != null && departmentId != "" && _attendanceList != null && _attendanceList.Where(x => x.User.Department.Id == (int.Parse(departmentId) == -1 ? x.User.Department.Id : int.Parse(departmentId))) != null && _attendanceList.Where(x => x.User.Department.Id == (int.Parse(departmentId) == -1 ? x.User.Department.Id : int.Parse(departmentId))).Any())
                    _attendanceList = _attendanceList.Where(x => x.User.Department.Id == (int.Parse(departmentId) == -1 ? x.User.Department.Id : int.Parse(departmentId))).OrderBy(x => x.Date).ToList();
                if (salaryTypeId != null && salaryTypeId != "" && _attendanceList != null)
                    _attendanceList = _attendanceList.Where(x => x.User.SalaryTypeId == Convert.ToInt32(salaryTypeId)).ToList();
                if (shiftId != null && shiftId != "" && _attendanceList != null)
                    _attendanceList = _attendanceList.Where(x => x.User.Shift.Id == Convert.ToInt32(shiftId)).ToList();
                if (_attendanceList == null)
                    ModelState.AddModelError("", "No Records Found");
                else
                {
                    _attendanceList = _attendanceList.OrderBy(x => x.User.Id).ToList();
                    VMAttendanceModel.AttendanceList = _attendanceList;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(VMAttendanceModel);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Monthly()
        {
            VMAttendanceModel model = new VMAttendanceModel();
            try
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Monthly(VMAttendanceModel model)
        {
            try
            {
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
                model.DepartmentList = objDepartmentService.GetAllDepartment(AuthBase.BranchId);

                if (model.DepartmentID != null)
                {
                    model.DepartmentList = model.DepartmentList.Where(x => x.Id == model.DepartmentID).ToList();
                }

                model.UserList = new List<User>();
                if (model.DepartmentList != null)
                {
                    List<User> UserList = objUserService.GetAllUserWithDepartment(AuthBase.BranchId);
                    List<Attendance> _attendanceList = objAttendanceService.GetAllAttendance();
                    List<AttendanceStatus> _attendanceStatusList = objAttendanceStatusService.GetAllAttendanceStatus();
                    foreach (var department in model.DepartmentList)
                    {
                        List<User> DepartmentUserList = UserList.Where(x => x.Department.Id == department.Id).ToList();
                        if (DepartmentUserList.Count > 0)
                        {
                            model.UserList.AddRange(DepartmentUserList);
                            foreach (var user in DepartmentUserList)
                            {
                                if (_attendanceList != null && _attendanceList.Count > 0)
                                {
                                    _attendanceList = _attendanceList.Where(x => x.IsActive == true && x.Date >= model.StartDate && x.Date <= model.EndDate).ToList();
                                    user.OffDayCount = 0; user.LateCount = 0; user.QuarterDayCount = 0; user.HalfDayCount = 0; user.FullDayCount = 0; user.LateMinutes = 0;
                                    user.EarlyMinutes = 0; user.OverTimeMinutes = 0; user.WorkingMinutes = 0; user.PresentCount = 0; user.AbsentCount = 0;
                                    user.OverTimeCount = 0; user.EarlyCount = 0;
                                    List<Attendance> _userAttendanceList = _attendanceList.Where(x => x.UserId == user.Id).ToList();
                                    if (_userAttendanceList != null && _userAttendanceList.Count > 0)
                                    {
                                        foreach (var attendance in _userAttendanceList)
                                        {
                                            if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                                            {
                                                _attendanceStatusList = _attendanceStatusList.Where(x => x.IsActive == true).ToList();
                                                if (_attendanceStatusList.Where(x => x.AttendanceId == attendance.Id) != null)
                                                {
                                                    attendance.AttendanceStatus = _attendanceStatusList.Where(x => x.AttendanceId == attendance.Id).FirstOrDefault();
                                                    if (attendance.AttendanceStatus != null)
                                                    {
                                                        user.LateMinutes += (int)(attendance.AttendanceStatus.LateMinutes ?? 0);
                                                        user.EarlyMinutes += (int)(attendance.AttendanceStatus.EarlyMinutes ?? 0);
                                                        user.OverTimeMinutes += (int)(attendance.AttendanceStatus.OverTimeMinutes ?? 0);
                                                        user.WorkingMinutes += (int)(attendance.AttendanceStatus.WorkingMinutes ?? 0);

                                                        if (attendance.AttendanceStatus.IsShiftOffDay == true)
                                                            user.OffDayCount++;
                                                        else
                                                            user.PresentCount++;
                                                        if (attendance.AttendanceStatus.IsLate == true)
                                                            user.LateCount++;
                                                        if (attendance.AttendanceStatus.IsQuarterDay == true)
                                                            user.QuarterDayCount++;
                                                        if (attendance.AttendanceStatus.IsHalfDay == true)
                                                            user.HalfDayCount++;
                                                        if (attendance.AttendanceStatus.IsFullDay == true)
                                                            user.FullDayCount++;
                                                        if (attendance.AttendanceStatus.IsEarly == true)
                                                            user.EarlyCount++;
                                                        if (attendance.AttendanceStatus.OverTimeMinutes > 0)
                                                            user.OverTimeCount++;
                                                    }
                                                    else
                                                        user.AbsentCount++;
                                                }
                                                else
                                                    user.AbsentCount++;
                                            }
                                            else
                                                user.AbsentCount++;
                                        }
                                    }
                                }
                            }
                        }
                        //else
                        //{
                        //    model.DepartmentList.Remove(department);
                        //    ModelState.AddModelError("", "No Records Found");
                        //}
                    }
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(model);
        }




        [HttpGet]
        [Authenticate]
        public ActionResult MonthlySummary()
        {
            VMAttendanceModel model = new VMAttendanceModel();
            try
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;

                #region
                StringBuilder builder = new StringBuilder();
                builder.Append("/Reports/web_forms/monthly_att_report.aspx?");
                builder.Append("dpt_id=" + model.DepartmentID);
                builder.Append("&sd=" + model.StartDate);
                builder.Append("&ed=" + model.EndDate);
                builder.Append("&id=" + model.ID);
                builder.Append("&name=" + model.Name);
                builder.Append("&br_id=" + model.BranchId);
                builder.Append("&sal_id=" + model.SalaryTypeId);
                builder.Append("&shf_id=" + model.ShiftId);
                ViewBag.MonthlyAttendanceReportUrl = builder.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult MonthlySummary(VMAttendanceModel model)
        {
            try
            {
                #region Department Filter
                IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
                List<Department> AllDepartmentList = objDepartmentService.GetAllDepartment();
                string deptName = null;
                if (model.DepartmentID.HasValue && model.DepartmentID.Value > 0)
                    deptName = AllDepartmentList.Where(x => x.Id == model.DepartmentID).FirstOrDefault().Name;
                #endregion Department Filter

                IUserService objUserService = IoC.Resolve<IUserService>("UserService");
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");

                model.UserList = objAttendanceService.GetMonthlyAttendanceSummary(model.StartDate, model.EndDate, model.ID, model.Name, model.BranchId, deptName, model.ShiftId, model.SalaryTypeId);
                if (model.UserList == null || model.UserList.Count <= 0)
                    ModelState.AddModelError("", "No Records Found");

                #region Build Report URL
                StringBuilder builder = new StringBuilder();
                builder.Append("/Reports/web_forms/monthly_att_report.aspx?");
                builder.Append("dpt_id=" + model.DepartmentID);
                builder.Append("&sd=" + model.StartDate);
                builder.Append("&ed=" + model.EndDate);
                builder.Append("&id=" + model.ID);
                builder.Append("&name=" + model.Name);
                builder.Append("&br_id=" + model.BranchId);
                builder.Append("&sal_id=" + model.SalaryTypeId);
                builder.Append("&shf_id=" + model.ShiftId);
                ViewBag.MonthlyAttendanceReportUrl = builder.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(model);
        }


        [HttpGet]
        [Authenticate]
        public ActionResult MonthlyDetail_Slow(string userId, string startdate, string enddate)
        {
            VMAttendanceModel VMAttendanceModel = new VMAttendanceModel();
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<Attendance> _attendanceList = new List<Attendance>();
                if (startdate != null && enddate != null && startdate != "" && enddate != "")
                    _attendanceList = objAttendanceService.GetAllAttendanceWithAttendanceDetail(string.Format("{0:dd-MMM-yyyy}", startdate), string.Format("{0:dd-MMM-yyyy}", enddate));

                if (userId != null && userId != "" && _attendanceList != null && _attendanceList.Where(x => x.User.Id == int.Parse(userId)) != null && _attendanceList.Where(x => x.User.Id == int.Parse(userId)).Any())
                    VMAttendanceModel.AttendanceList = _attendanceList.Where(x => x.User.Id == int.Parse(userId)).OrderBy(x => x.Date).ToList();
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(VMAttendanceModel);
            return View(VMAttendanceModel);
        }


        [HttpGet]
        [Authenticate]
        public ActionResult MonthlyDetail(string userId, string startdate, string enddate)
        {
            VMAttendanceModel VMAttendanceModel = new VMAttendanceModel();
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<Attendance> _attendanceList = new List<Attendance>();
                Int32? UID;
                if (userId != null && userId != "")
                    UID = Convert.ToInt32(userId);
                else UID = null;
                if (startdate != null && enddate != null && startdate != "" && enddate != "")
                {
                    _attendanceList = objAttendanceService.GetAllAttendanceByDateUserWithDetail(startdate, enddate, UID, null);
                }

                if (_attendanceList.Count > 0)
                    VMAttendanceModel.AttendanceList = _attendanceList;
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(VMAttendanceModel);
            return View(VMAttendanceModel);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult MonthlyDetail2(string userId, string startdate, string enddate)
        {
            VMAttendanceModel VMAttendanceModel = new VMAttendanceModel();
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<Attendance> _attendanceList = new List<Attendance>();
                Int32? UID;
                if (userId != null && userId != "")
                    UID = Convert.ToInt32(userId);
                else UID = null;

                VMAttendanceModel.StartDate = Convert.ToDateTime(startdate);
                VMAttendanceModel.EndDate = Convert.ToDateTime(enddate);

                if (startdate != null && enddate != null && startdate != "" && enddate != "")
                {
                    _attendanceList = objAttendanceService.GetAllAttendanceByDateUserWithDetail(startdate, enddate, UID, null);
                }

                if (_attendanceList.Count > 0)
                    VMAttendanceModel.AttendanceList = _attendanceList;
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(VMAttendanceModel);
            return View(VMAttendanceModel);
        }


        [HttpGet]
        [Authenticate]
        public ActionResult EditMonthlyDetail(string userId, string startdate, string enddate)
        {
            List<Attendance> _attendanceList = new List<Attendance>();
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                IAttendanceDetailService objAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
                if (startdate != null && enddate != null && startdate != "" && enddate != "")
                    _attendanceList = objAttendanceService.GetAllAttendanceWithAttendanceDetail(string.Format("{0:dd-MMM-yyyy}", startdate), string.Format("{0:dd-MMM-yyyy}", enddate));

                if (userId != null && userId != "" && _attendanceList != null && _attendanceList.Where(x => x.User.Id == int.Parse(userId)) != null && _attendanceList.Where(x => x.User.Id == int.Parse(userId)).Any())
                {
                    _attendanceList = _attendanceList.Where(x => x.User.Id == int.Parse(userId)).OrderBy(x => x.Date).ToList();
                    foreach (Attendance _attendance in _attendanceList)
                    {
                        List<AttendanceDetail> _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                        if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                            _attendance.AttendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)HRM.Core.Enum.AttendanceType.DailyAttendance && x.IsActive == true).ToList();

                        if (_attendance.AttendanceDetailList == null || _attendance.AttendanceDetailList.Count <= 0)
                        {
                            _attendance.AttendanceDetailList = new List<AttendanceDetail>();
                            _attendance.AttendanceDetailList.Add(new AttendanceDetail());
                        }
                    }
                }
                else
                    ModelState.AddModelError("", "No Records Found");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "No Records Found");
            }
            ViewBag.UserID = userId;
            ViewBag.StartDate = startdate;
            ViewBag.EndDate = enddate;
            return View(_attendanceList);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult EditMonthlyDetail(List<Attendance> _attendanceList)
        {
            IAttendanceDetailService objAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
            foreach (Attendance _attendance in _attendanceList)
            {
                int TotalMinutes = 0, BreakMinutes = 0, WorkingMinutes = 0, ShiftMinutes = 0;
                foreach (AttendanceDetail _attendanceDetail in _attendance.AttendanceDetailList)
                {
                    bool executeAttendanceStatus = false;
                    if (_attendanceDetail.Id == 0 && _attendanceDetail.StartDate.HasValue && _attendanceDetail.EndDate.HasValue)
                    {
                        _attendanceDetail.UpdateBy = AuthBase.UserId;
                        _attendanceDetail.CreationDate = DateTime.Now;
                        _attendanceDetail.UserIp = Request.UserHostAddress;
                        _attendanceDetail.AttendanceId = _attendance.Id;
                        _attendanceDetail.IsActive = true;
                        _attendanceDetail.AttendanceTypeId = Convert.ToInt32(HRM.Core.Enum.AttendanceType.DailyAttendance);
                        objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);

                        executeAttendanceStatus = true;
                    }
                    else
                    {
                        AttendanceDetail _oldAttendanceDetail = objAttendanceDetailService.GetAttendanceDetail(_attendanceDetail.Id);
                        if (_oldAttendanceDetail != null && _attendanceDetail.StartDate != null && _attendanceDetail.EndDate != null)
                        {
                            if (_attendanceDetail.StartDate != _oldAttendanceDetail.StartDate || _attendanceDetail.EndDate != _oldAttendanceDetail.EndDate)
                            {
                                _attendanceDetail.UpdateBy = AuthBase.UserId;
                                _attendanceDetail.UpdateDate = DateTime.Now;
                                _attendanceDetail.UserIp = Request.UserHostAddress;
                                _attendanceDetail.IsActive = true;
                                _attendanceDetail.AttendanceId = _attendance.Id;
                                _attendanceDetail.AttendanceTypeId = Convert.ToInt32(HRM.Core.Enum.AttendanceType.DailyAttendance);
                                objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);

                                executeAttendanceStatus = true;
                            }
                        }
                    }
                    if (executeAttendanceStatus)
                    {



                        #region Attendance Status Timings Work
                        List<AttendanceDetail> AttendanceDetailList = new List<AttendanceDetail>();
                        List<AttendanceDetail> BreakAttendanceDetailList = new List<AttendanceDetail>();
                        AttendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                        if (AttendanceDetailList != null && AttendanceDetailList.Count > 0)
                        {
                            AttendanceDetailList = AttendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
                            if (AttendanceDetailList != null && AttendanceDetailList.Count > 0)
                            {
                                foreach (AttendanceDetail detail in AttendanceDetailList)
                                {
                                    TimeSpan diff = (TimeSpan)(detail.EndDate - detail.StartDate);
                                    TotalMinutes += Convert.ToInt32(diff.TotalMinutes);
                                }
                            }
                            BreakAttendanceDetailList = AttendanceDetailList.Where(x => x.AttendanceTypeId != (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
                            if (BreakAttendanceDetailList != null && BreakAttendanceDetailList.Count > 0)
                            {
                                foreach (AttendanceDetail detail in BreakAttendanceDetailList)
                                {
                                    TimeSpan diff = (TimeSpan)(detail.EndDate - detail.StartDate);
                                    BreakMinutes += Convert.ToInt32(diff.TotalMinutes);
                                }
                            }
                            WorkingMinutes = TotalMinutes - BreakMinutes;
                        }
                        DateTime attendanceNextDate = _attendance.Date.Value.AddDays(1);
                        #endregion

                        IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
                        List<AttendanceStatus> _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                        AttendanceStatus _attendanceStatus;
                        if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                        {
                            _attendanceStatus = _attendanceStatusList.FirstOrDefault();
                            _attendanceStatus.IsShiftOffDay = _attendanceStatus.IsShiftOffDay == null ? false : true;
                            _attendanceStatus.IsLeaveDay = _attendanceStatus.IsLeaveDay == null ? false : true;
                            _attendanceStatus.IsHoliday = _attendanceStatus.IsHoliday == null ? false : true;
                        }
                        else
                        {
                            _attendanceStatus = new AttendanceStatus() { AttendanceId = _attendance.Id, IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = "Service", LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                        }
                        _attendanceStatus.TotalMinutes = TotalMinutes;
                        _attendanceStatus.WorkingMinutes = WorkingMinutes;

                        IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
                        IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                        IAttendancePolicyService objAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");
                        List<UserShift> _userShiftList = objUserShiftService.GetUserShiftByUserId(_attendance.UserId);
                        UserShift _userShift;
                        Shift _shift;
                        AttendancePolicy _attendancePolicy;

                        if (_userShiftList != null && _userShiftList.Count > 0)
                        {
                            _userShiftList = _userShiftList.Where(x => _attendance.Date >= x.EffectiveDate && _attendance.Date <= (x.RetiredDate == null ? _attendance.Date : x.RetiredDate)).ToList();
                            if (_userShiftList != null && _userShiftList.Count > 0)
                            {
                                _userShift = _userShiftList.FirstOrDefault();
                                _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
                                if (!_shift.StartHour.Contains(":"))
                                    _shift.StartHour += ":00";
                                List<AttendancePolicy> _attendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(_userShift.ShiftId.Value);
                                if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                {
                                    _attendancePolicyList = _attendancePolicyList.Where(x => _attendance.Date >= x.EffectiveDate && _attendance.Date <= (x.RetiredDate == null ? _attendance.Date : x.RetiredDate)).ToList();
                                    if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                    {
                                        List<AttendancePolicy> _attendancePolicyListTemp = _attendancePolicyList;

                                        TimeSpan ShiftMinutesSpan;
                                        //same day shift off
                                        if (Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour) < Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour))
                                            ShiftMinutesSpan = Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour) - Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour);
                                        //next day shift off
                                        else
                                            ShiftMinutesSpan = Convert.ToDateTime(attendanceNextDate.ToString("MM/dd/yyyy") + " " + _shift.EndHour) - Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour);
                                        ShiftMinutes = Convert.ToInt32(ShiftMinutesSpan.TotalMinutes);

                                        //If off day/leave/holiday no policy should be check
                                        if ((bool)_attendanceStatus.IsShiftOffDay == false && (bool)_attendanceStatus.IsHoliday == false && (bool)_attendanceStatus.IsLeaveDay == false)
                                        {

                                            //Time in working
                                            //FULL Day
                                            _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                            if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                            {
                                                _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                if ((decimal)((DateTime)_attendanceDetail.StartDate - Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                                    _attendanceStatus.IsFullDay = true;
                                                else
                                                    _attendanceStatus.IsFullDay = false;
                                            }
                                            else
                                            {
                                                _attendanceStatus.IsFullDay = false;
                                            }

                                            //Half Day
                                            _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.HalfDay).ToList();
                                            if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value)
                                            {
                                                _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                if ((decimal)((DateTime)_attendanceDetail.StartDate - Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                                    _attendanceStatus.IsHalfDay = true;
                                                else
                                                    _attendanceStatus.IsHalfDay = false;
                                            }
                                            else
                                            {
                                                _attendanceStatus.IsHalfDay = false;
                                            }

                                            //Quater Day
                                            _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.QuarterDay).ToList();
                                            if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value && !_attendanceStatus.IsHalfDay.Value)
                                            {
                                                _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                if ((decimal)((DateTime)_attendanceDetail.StartDate - Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                                    _attendanceStatus.IsQuarterDay = true;
                                                else
                                                    _attendanceStatus.IsQuarterDay = false;
                                            }
                                            else
                                            {
                                                _attendanceStatus.IsQuarterDay = false;
                                            }

                                            //Time out working
                                            //If same day shift off
                                            if (Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour) < Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour))
                                            {
                                                //FULL Day
                                                if (!_attendanceStatus.IsFullDay.Value)
                                                {
                                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                                    {
                                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                        if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour) - (DateTime)_attendanceDetail.EndDate).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                        {
                                                            _attendanceStatus.IsFullDay = true;
                                                            _attendanceStatus.IsHalfDay = false;
                                                            _attendanceStatus.IsQuarterDay = false;
                                                        }
                                                        else
                                                            _attendanceStatus.IsFullDay = false;
                                                    }
                                                    else
                                                    {
                                                        _attendanceStatus.IsFullDay = false;
                                                    }
                                                }
                                                //Half Day
                                                if (!_attendanceStatus.IsHalfDay.Value)
                                                {
                                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.HalfDay).ToList();
                                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value)
                                                    {
                                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                        if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour) - (DateTime)_attendanceDetail.EndDate).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                        {
                                                            _attendanceStatus.IsHalfDay = true;
                                                            _attendanceStatus.IsFullDay = false;
                                                            _attendanceStatus.IsQuarterDay = false;
                                                        }
                                                        else
                                                            _attendanceStatus.IsHalfDay = false;
                                                    }
                                                    else
                                                    {
                                                        _attendanceStatus.IsHalfDay = false;
                                                    }
                                                }
                                                //Quater Day
                                                if (!_attendanceStatus.IsQuarterDay.Value)
                                                {
                                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.QuarterDay).ToList();
                                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value && !_attendanceStatus.IsHalfDay.Value)
                                                    {
                                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                        if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour) - (DateTime)_attendanceDetail.EndDate).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                        {
                                                            _attendanceStatus.IsQuarterDay = true;
                                                            _attendanceStatus.IsFullDay = false;
                                                            _attendanceStatus.IsHalfDay = false;
                                                        }
                                                        else
                                                            _attendanceStatus.IsQuarterDay = false;
                                                    }
                                                    else
                                                    {
                                                        _attendanceStatus.IsQuarterDay = false;
                                                    }
                                                }
                                            }
                                            //Next day shift off
                                            else
                                            {
                                                //FULL Day
                                                if (!_attendanceStatus.IsFullDay.Value)
                                                {
                                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                                    {
                                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                        if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString("MM/dd/yyyy") + " " + _shift.EndHour) - (DateTime)_attendanceDetail.EndDate).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                        {
                                                            _attendanceStatus.IsFullDay = true;
                                                            _attendanceStatus.IsHalfDay = false;
                                                            _attendanceStatus.IsQuarterDay = false;
                                                        }
                                                        else
                                                            _attendanceStatus.IsFullDay = false;
                                                    }
                                                    else
                                                    {
                                                        _attendanceStatus.IsFullDay = false;
                                                    }
                                                }
                                                //Half Day
                                                if (!_attendanceStatus.IsHalfDay.Value)
                                                {
                                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.HalfDay).ToList();
                                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value)
                                                    {
                                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                        if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString("MM/dd/yyyy") + " " + _shift.EndHour) - (DateTime)_attendanceDetail.EndDate).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                        {
                                                            _attendanceStatus.IsHalfDay = true;
                                                            _attendanceStatus.IsFullDay = false;
                                                            _attendanceStatus.IsQuarterDay = false;
                                                        }
                                                        else
                                                            _attendanceStatus.IsHalfDay = false;
                                                    }
                                                    else
                                                    {
                                                        _attendanceStatus.IsHalfDay = false;
                                                    }
                                                }
                                                //Quater Day
                                                if (!_attendanceStatus.IsQuarterDay.Value)
                                                {
                                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.QuarterDay).ToList();
                                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value && !_attendanceStatus.IsHalfDay.Value)
                                                    {
                                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                        if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString("MM/dd/yyyy") + " " + _shift.EndHour) - (DateTime)_attendanceDetail.EndDate).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                        {
                                                            _attendanceStatus.IsQuarterDay = true;
                                                            _attendanceStatus.IsFullDay = false;
                                                            _attendanceStatus.IsHalfDay = false;
                                                        }
                                                        else
                                                            _attendanceStatus.IsQuarterDay = false;
                                                    }
                                                    else
                                                    {
                                                        _attendanceStatus.IsQuarterDay = false;
                                                    }
                                                }
                                            }
                                        }
                                        //Overtime
                                        //if (!_attendanceStatus.IsOvertime.Value)
                                        _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.OverTime).ToList();
                                        if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                        {
                                            _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                            if (TotalMinutes >= _attendancePolicy.Hours.Value)
                                            {
                                                _attendanceStatus.OverTimeMinutes = TotalMinutes - ShiftMinutes;
                                            }
                                            else
                                            {
                                                _attendanceStatus.OverTimeMinutes = 0;
                                            }
                                        }
                                        else
                                        {
                                            _attendanceStatus.OverTimeMinutes = 0;
                                        }
                                    }
                                }
                                //If off day/leave/holiday late or early should not be counted
                                if ((bool)_attendanceStatus.IsShiftOffDay == false && (bool)_attendanceStatus.IsHoliday == false && (bool)_attendanceStatus.IsLeaveDay == false)
                                {
                                    //Late
                                    if (_attendanceDetail.StartDate > Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour))
                                    {
                                        TimeSpan TimeDiff = (DateTime)_attendanceDetail.StartDate - Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour);
                                        _attendanceStatus.LateMinutes = Convert.ToInt32(TimeDiff.TotalMinutes);
                                        _attendanceStatus.IsLate = true;
                                    }
                                    else
                                    {
                                        _attendanceStatus.LateMinutes = 0;
                                        _attendanceStatus.IsLate = false;
                                    }
                                    //Early
                                    //If same day shift off
                                    if (Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.StartHour) < Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour))
                                    {
                                        if (_attendanceDetail.EndDate < Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour))
                                        {
                                            TimeSpan TimeDiff = (DateTime)_attendanceDetail.EndDate - Convert.ToDateTime(_attendance.Date.Value.ToString("MM/dd/yyyy") + " " + _shift.EndHour);
                                            _attendanceStatus.EarlyMinutes = Convert.ToInt32(TimeDiff.TotalMinutes);
                                            _attendanceStatus.IsEarly = true;
                                        }
                                        else
                                        {
                                            _attendanceStatus.EarlyMinutes = 0;
                                            _attendanceStatus.IsEarly = false;
                                        }
                                    }
                                    //If next day shift off
                                    else
                                    {
                                        if (_attendanceDetail.EndDate < Convert.ToDateTime(attendanceNextDate.ToString("MM/dd/yyyy") + " " + _shift.EndHour))
                                        {
                                            TimeSpan TimeDiff = (DateTime)_attendanceDetail.EndDate - Convert.ToDateTime(attendanceNextDate.ToString("MM/dd/yyyy") + " " + _shift.EndHour);
                                            _attendanceStatus.EarlyMinutes = Convert.ToInt32(TimeDiff.TotalMinutes);
                                            _attendanceStatus.IsEarly = true;
                                        }
                                        else
                                        {
                                            _attendanceStatus.EarlyMinutes = 0;
                                            _attendanceStatus.IsEarly = false;
                                        }
                                    }
                                }
                            }
                        }
                        if (_attendanceStatus != null && _attendanceStatus.Id <= 0)
                            _attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                        else
                        {
                            _attendanceStatus.UpdateDate = DateTime.Now;
                            _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
                        }








                    }
                }
            }
            Response.Redirect("/Attendance/EditMonthlyDetail?userId=" + Request["UserID"] + "&startdate=" + Request["StartDate"] + "&enddate=" + Request["EndDate"]);
            return View();
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Absent()
        {
            VMAttendanceModel model = new VMAttendanceModel();
            try
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;
                #region Absentee Report URL
                StringBuilder builder = new StringBuilder();
                builder.Append("/Reports/web_forms/absentee_report.aspx?");
                builder.Append("&sd=" + model.StartDate);
                builder.Append("&ed=" + model.EndDate);
                ViewBag.AbsenteeReportUrl = builder.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult Absent(VMAttendanceModel model)
        {
            VMAttendanceModel VMAttendanceModel = new VMAttendanceModel();
            try
            {
                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                List<Attendance> _attendanceList = new List<Attendance>();
                if (model.StartDate != null && model.EndDate != null && model.StartDate.ToString() != "" && model.EndDate.ToString() != "")
                    _attendanceList = objAttendanceService.GetAllAttendanceWithAttendanceDetail(string.Format("{0:dd-MMM-yyyy}", model.StartDate), string.Format("{0:dd-MMM-yyyy}", model.EndDate));
                if (_attendanceList != null && _attendanceList.Count > 0)
                {
                    _attendanceList = _attendanceList.Where(x => x.AttendanceStatus == null).ToList();
                    if (_attendanceList.Count > 0)
                        VMAttendanceModel.AttendanceList = _attendanceList;
                    else
                        ModelState.AddModelError("", "No Records Found");
                }
                else
                    ModelState.AddModelError("", "No Records Found");

                #region Absentee Report URL
                StringBuilder builder = new StringBuilder();
                builder.Append("/Reports/web_forms/absentee_report.aspx?");
                builder.Append("&sd=" + model.StartDate);
                builder.Append("&ed=" + model.EndDate);
                ViewBag.AbsenteeReportUrl = builder.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            AttendancePrerequisiteData(model);
            return View(VMAttendanceModel);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult Variable(string Id)
        {
            AttendanceVariable model = new AttendanceVariable();
            try
            {
                if (Id != null && Id != "")
                {
                    IAttendanceVariableService objAttendanceVariableService = IoC.Resolve<IAttendanceVariableService>("AttendanceVariableService");
                    if (objAttendanceVariableService.GetAttendanceVariable(int.Parse(Id)) != null)
                        model = objAttendanceVariableService.GetAttendanceVariable(int.Parse(Id));
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
        public ActionResult Variable(AttendanceVariable model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id != 0)
                    {
                        #region Attendance Variable Updation
                        IAttendanceVariableService objAttendanceVariableService = IoC.Resolve<IAttendanceVariableService>("AttendanceVariableService");

                        model.UpdateDate = DateTime.Now;
                        model.UpdateBy = AuthBase.UserId;
                        model.UserIp = Request.UserHostAddress;

                        AttendanceVariable _attendanceVariable = objAttendanceVariableService.UpdateAttendanceVariable(model);
                        #endregion
                    }
                    else
                    {
                        #region Attendance Variable Entry
                        IAttendanceVariableService objAttendanceVariableService = IoC.Resolve<IAttendanceVariableService>("AttendanceVariableService");

                        model.IsActive = true;
                        model.CreationDate = DateTime.Now;
                        model.UpdateBy = AuthBase.UserId;
                        model.UserIp = Request.UserHostAddress;

                        AttendanceVariable _attendanceVariable = objAttendanceVariableService.InsertAttendanceVariable(model);
                        #endregion
                    }
                    return RedirectToAction("ShiftList", "Shift");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ModelState.AddModelError("", "Unable to process your request. Please contact Administration");
            return View();
        }


        private void AttendancePrerequisiteData(VMAttendanceModel model)
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            List<User> Users = objUserService.GetAllUser();
            if (model != null && model.UserId.HasValue)
                ViewBag.UserName = Users.Where(x => x.Id == model.UserId).FirstOrDefault().FirstName;
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
            if (model != null && model.DepartmentID.HasValue)
                ViewBag.DeptName = DistinctDepartmentList.Where(x => x.Id == model.DepartmentID).FirstOrDefault().Name;
            if (DistinctDepartmentList != null && DepartmentList != null && DepartmentList.Count > 0 && DistinctDepartmentList.Count > 0)
                ViewBag.Department = new SelectList(DistinctDepartmentList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Department", Value = "-1" });
                ViewBag.Department = new SelectList(blank, "Value", "Text", "");
            }

            IBranchService objBranchService = IoC.Resolve<IBranchService>("BranchService");
            List<Branch> BranchList = objBranchService.GetAllBranch();
            if (model != null && model.BranchId.HasValue)
                ViewBag.BranchName = BranchList.Where(x => x.Id == model.BranchId).FirstOrDefault().Name;
            if (BranchList != null && BranchList.Count > 0)
                ViewBag.Branch = new SelectList(BranchList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Branch", Value = "-1" });
                ViewBag.Branch = new SelectList(blank, "Value", "Text", "");
            }

            ISalaryTypeService objSalaryTypeService = IoC.Resolve<ISalaryTypeService>("SalaryTypeService");
            List<SalaryType> SalaryTypeList = objSalaryTypeService.GetAllSalaryType();
            if (SalaryTypeList != null && SalaryTypeList.Count > 0)
                SalaryTypeList = SalaryTypeList.Where(x => x.IsActive == true).ToList();
            if (model != null && model.SalaryTypeId.HasValue)
                ViewBag.SalaryTypeName = SalaryTypeList.Where(x => x.Id == model.SalaryTypeId).FirstOrDefault().Name;

            if (SalaryTypeList != null && SalaryTypeList.Count > 0)
                ViewBag.SalaryType = new SelectList(SalaryTypeList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "SalaryType", Value = "-1" });
                ViewBag.SalaryType = new SelectList(blank, "Value", "Text", "");
            }

            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            List<Shift> ShiftList = objShiftService.GetAllShift();
            if (model != null && model.ShiftId.HasValue)
                ViewBag.ShiftName = ShiftList.Where(x => x.Id == model.ShiftId).FirstOrDefault().Name;

            if (ShiftList != null && ShiftList.Count > 0)
                ViewBag.Shift = new SelectList(ShiftList, "Id", "Name");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "Shift", Value = "-1" });
                ViewBag.Shift = new SelectList(blank, "Value", "Text", "");
            }
        }

        [Authenticate]
        public ActionResult Browse()
        {
            return View();
        }


        [HttpPost]
        [Authenticate]
        public ActionResult Browse(string posted = null)
        {
            HttpPostedFileBase file = null;
            DateTime dtAttendance = DateTime.Now;
            List<VMBrowseAttendance> BAttendanceList = new List<VMBrowseAttendance>();
            List<VMBrowseAttendance> BTempAttendanceList = null;
            List<VMBrowseAttendance> BUniqueAttendanceList = new List<VMBrowseAttendance>();
            VMBrowseAttendance BAttendance = null;
            List<string> msg = new List<string>();
            ViewBag.Message = new string[] { "" };
            #region Validate
            if (Request.Files == null || Request.Files.Count <= 0 || Request.Files[0].ContentLength <= 0)
            {
                ViewBag.Message = new string[] { "Please browse file" };
                return View();
            }

            #endregion Validate
            #region File Upload
            file = Request.Files[0];
            if (!Directory.Exists(Server.MapPath("/Uploads")))
                Directory.CreateDirectory(Server.MapPath("/Uploads"));
            if (!Directory.Exists(Server.MapPath("/Uploads/AttendanceData")))
                Directory.CreateDirectory(Server.MapPath("/Uploads/AttendanceData"));
            if (!Directory.Exists(Server.MapPath("/Uploads/AttendanceData/" + dtAttendance.ToString("yyyyMM"))))
                Directory.CreateDirectory(Server.MapPath("/Uploads/AttendanceData/" + dtAttendance.ToString("yyyyMM")));
            string fileName = dtAttendance.ToString("ddhhmmss") + "-" + file.FileName;
            string fullFilePath = Server.MapPath("/Uploads/AttendanceData/" + dtAttendance.ToString("yyyyMM") + "/" + fileName);
            file.SaveAs(fullFilePath);
            #endregion File Upload
            #region Processing
            #region Init

            List<Attendance> _attendanceList = null;
            Attendance _attendance = null;
            List<AttendanceDetail> _attendanceDetailList = null;
            AttendanceDetail _attendanceDetail = null;
            List<AttendanceStatus> _attendanceStatusList = null;
            AttendanceStatus _attendanceStatus = null;
            List<VMAttendanceData> attendanceDataList = new List<VMAttendanceData>();

            List<UserShift> _userShiftList = null;
            List<AttendancePolicy> _attendancePolicyList = null;
            List<AttendancePolicy> _attendancePolicyListTemp = null;
            UserShift _userShift = null;
            Shift _shift = null;
            AttendancePolicy _attendancePolicy = null;

            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IAttendanceDetailService objAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
            IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            IAttendancePolicyService objAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");

            #endregion Init
            #region ReadData
            string[] lines = System.IO.File.ReadAllLines(fullFilePath);
            int i = 0;
            foreach (string line in lines)
            {
                if (i == 0) { i++; continue; }
                string[] parts = line.Split(',', '\t');
                if (parts.Length != 4)
                {
                    msg.Add("Invalid number of columns. Error on line " + (i + 1).ToString());
                    continue;
                }
                BAttendance = new VMBrowseAttendance();
                try
                {
                    BAttendance.UserId = Convert.ToInt32(parts[(int)Cols.EmpNo]);
                    BAttendance.AttendanceDate = Convert.ToDateTime(parts[(int)Cols.Date]);
                    BAttendance.TimeIn = String.IsNullOrWhiteSpace(parts[(int)Cols.TimeIn]) ? (DateTime?)null : Convert.ToDateTime(parts[(int)Cols.TimeIn]);
                    BAttendance.TimeOut = String.IsNullOrWhiteSpace(parts[(int)Cols.TimeOut]) ? (DateTime?)null : Convert.ToDateTime(parts[(int)Cols.TimeOut]);
                    if (BAttendance.TimeIn == null && BAttendance.TimeOut == null)
                        throw null;
                    BAttendanceList.Add(BAttendance);
                }
                catch
                {
                    msg.Add("Invalid data. Error on line " + (i + 1).ToString());
                    continue;
                }
            }
            #endregion ReadData
            #region FilterByUser
            List<UserDate> userDate = BAttendanceList.Select(x => new UserDate() { UserID = x.UserId, Date = x.AttendanceDate }).Distinct().ToList();
            foreach (UserDate _userDate in userDate)
            {
                #region Minimum TimeIn and Maximum Timeout
                BTempAttendanceList = BAttendanceList.Where(x => x.UserId == _userDate.UserID && x.AttendanceDate == _userDate.Date && x.TimeIn != null).ToList();
                if (BTempAttendanceList == null || BTempAttendanceList.Count <= 0)
                    _userDate.TimeIn = null;
                else
                    _userDate.TimeIn = BTempAttendanceList.OrderBy(x => x.TimeIn).Select(x => x.TimeIn).First();

                BTempAttendanceList = BAttendanceList.Where(x => x.UserId == _userDate.UserID && x.AttendanceDate == _userDate.Date && x.TimeOut != null).ToList();
                if (BTempAttendanceList == null || BTempAttendanceList.Count <= 0)
                    _userDate.TimeOut = null;
                else
                    _userDate.TimeOut = BTempAttendanceList.OrderBy(x => x.TimeOut).Select(x => x.TimeOut).Last();
                #endregion Minimum TimeIn and Maximum Timeout

                #region Attendance
                _attendanceList = objAttendanceService.GetAttendanceByUserIDAndDate(_userDate.UserID, _userDate.Date);
                //Insert Attendance
                if (_attendanceList == null || _attendanceList.Count <= 0)
                {
                    _attendance = new Attendance() { UserId = _userDate.UserID, Date = _userDate.Date, IsActive = true, CreationDate = DateTime.Now, UserIp = Request.UserHostAddress };
                    _attendance = objAttendanceService.InsertAttendance(_attendance);
                }
                //Attendance Already marked, Get Attendance
                else
                {
                    _attendance = _attendanceList.FirstOrDefault();
                }
                #endregion Attendance
                #region AttendanceDetail
                //Timein Entry
                if (_userDate.TimeIn != null)
                {
                    _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                    //Already Timein Entry
                    if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                    {
                        _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
                        if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                        {
                            _attendanceDetail = _attendanceDetailList.FirstOrDefault();
                            if (_attendanceDetailList.FirstOrDefault().StartDate == null)
                            {
                                _attendanceDetail.StartDate = _userDate.TimeIn;
                                _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                            }
                        }
                        else
                        {
                            //Insert Timein
                            _attendanceDetail = new AttendanceDetail()
                            {
                                AttendanceId = _attendance.Id,
                                AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                StartDate = _userDate.TimeIn,
                                IsActive = true,
                                UserIp = Request.UserHostAddress,
                                CreationDate = DateTime.Now
                            };
                            _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                        }
                    }
                    else
                    {



                        //Insert Timein
                        _attendanceDetail = new AttendanceDetail()
                        {
                            AttendanceId = _attendance.Id,
                            AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                            StartDate = _userDate.TimeIn,
                            IsActive = true,
                            UserIp = Request.UserHostAddress,
                            CreationDate = DateTime.Now
                        };
                        _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                    }
                }
                //Timeout Entry
                if (_userDate.TimeOut != null)
                {
                    _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                    //Already Timeout Entry
                    if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                    {
                        _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
                        if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                        {
                            if (_attendanceDetailList.FirstOrDefault().EndDate == null)
                            {
                                _attendanceDetail = _attendanceDetailList.FirstOrDefault();
                                _attendanceDetail.EndDate = _userDate.TimeOut;
                                _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                            }
                        }
                    }
                }

                #endregion AttendanceDetail
                #region AttendanceStatus
                if (_attendanceDetail != null && _attendanceDetail.Id > 0)
                {
                    _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                    if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                    {
                        _attendanceStatus = _attendanceStatusList.FirstOrDefault();
                    }
                    else
                    {
                        _attendanceStatus = new AttendanceStatus()
                        {
                            AttendanceId = _attendance.Id,
                            IsShiftOffDay = false,
                            IsLeaveDay = false,
                            IsHoliday = false,
                            IsFullDay = false,
                            IsQuarterDay = false,
                            IsHalfDay = false,
                            IsLate = false,
                            IsActive = true,
                            CreationDate = DateTime.Now,
                            UserIp = Request.UserHostAddress
                        };
                    }
                    _userShiftList = objUserShiftService.GetUserShiftByUserId(_attendance.UserId);
                    if (_userShiftList != null && _userShiftList.Count > 0)
                    {
                        _userShiftList = _userShiftList.Where(x => _userDate.Date >= x.EffectiveDate && _userDate.Date <= (x.RetiredDate == null ? _userDate.Date : x.RetiredDate)).ToList();
                        if (_userShiftList != null && _userShiftList.Count > 0)
                        {
                            _userShift = _userShiftList.FirstOrDefault();
                            _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
                            if (!_shift.StartHour.Contains(":"))
                                _shift.StartHour += ":00";
                            _attendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(_userShift.ShiftId.Value);
                            if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                            {
                                _attendancePolicyList = _attendancePolicyList.Where(x => _userDate.Date >= x.EffectiveDate && _userDate.Date <= (x.RetiredDate == null ? _userDate.Date : x.RetiredDate)).ToList();
                                if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                {
                                    //FULL Day
                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                    {
                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                        if ((decimal)(_userDate.TimeIn.Value - Convert.ToDateTime(_userDate.Date.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                            _attendanceStatus.IsFullDay = true;
                                        else
                                            _attendanceStatus.IsFullDay = false;
                                    }
                                    else
                                    {
                                        _attendanceStatus.IsFullDay = false;
                                    }

                                    //Half Day
                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.HalfDay).ToList();
                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value)
                                    {
                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                        if ((decimal)(_userDate.TimeIn.Value - Convert.ToDateTime(_userDate.Date.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                            _attendanceStatus.IsHalfDay = true;
                                        else
                                            _attendanceStatus.IsHalfDay = false;
                                    }
                                    else
                                    {
                                        _attendanceStatus.IsHalfDay = false;
                                    }

                                    //Quater Day
                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.QuarterDay).ToList();
                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value && !_attendanceStatus.IsHalfDay.Value)
                                    {
                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                        if ((decimal)(_userDate.TimeIn.Value - Convert.ToDateTime(_userDate.Date.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                            _attendanceStatus.IsQuarterDay = true;
                                        else
                                            _attendanceStatus.IsQuarterDay = false;
                                    }
                                    else
                                    {
                                        _attendanceStatus.IsQuarterDay = false;
                                    }
                                }
                                else
                                {
                                    //Late
                                    if (_userDate.TimeIn.Value > Convert.ToDateTime(_userDate.Date.ToString("MM/dd/yyyy") + " " + _shift.StartHour))
                                        _attendanceStatus.IsLate = true;
                                    else
                                        _attendanceStatus.IsLate = false;
                                }
                            }
                            //Late
                            if (_userDate.TimeIn.Value > Convert.ToDateTime(_userDate.Date.ToString("MM/dd/yyyy") + " " + _shift.StartHour))
                                _attendanceStatus.IsLate = true;
                            else
                                _attendanceStatus.IsLate = false;
                        }
                    }
                    if (_attendanceStatus != null && _attendanceStatus.Id <= 0)
                        _attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                    else
                    {
                        _attendanceStatus.UpdateDate = DateTime.Now;
                        _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
                    }
                }
                #endregion AttendanceStatus

                _attendance = null;
                _attendanceList = null;
                _attendanceDetailList = null;
                _attendanceDetail = null;
                _attendanceList = null;
                _attendanceStatus = null;
                _attendanceStatusList = null;
                _shift = null;
                _userShift = null;
                _userShiftList = null;
            }
            #endregion FilterByUser
            #endregion Processing
            if (msg.Count <= 0)
                ViewBag.Message = new string[] { "File processed successfully" };
            else
                ViewBag.Message = msg.ToArray();
            return View();
        }

        [HttpPost]
        public JsonResult MarkLeave(string id)
        {
            try
            {
                IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");

                if (id.Contains(','))
                {
                    string[] AttendanceIDList = id.Split(',');
                    foreach (var _attendanceID in AttendanceIDList)
                    {
                        AttendanceStatus _attendanceStatus = _attendanceStatus = new AttendanceStatus() { AttendanceId = Convert.ToInt32(_attendanceID), IsShiftOffDay = false, IsLeaveDay = true, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = Request.UserHostAddress, LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                        objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                    }
                }
                else
                {
                    AttendanceStatus _attendanceStatus = _attendanceStatus = new AttendanceStatus() { AttendanceId = Convert.ToInt32(id), IsShiftOffDay = false, IsLeaveDay = true, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = Request.UserHostAddress, LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                    objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                }
                return base.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return base.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MarkHoliday(string id)
        {
            try
            {
                IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");

                if (id.Contains(','))
                {
                    string[] AttendanceIDList = id.Split(',');
                    foreach (var _attendanceID in AttendanceIDList)
                    {
                        AttendanceStatus _attendanceStatus = _attendanceStatus = new AttendanceStatus() { AttendanceId = Convert.ToInt32(_attendanceID), IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = true, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = Request.UserHostAddress, LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                        objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                    }
                }
                else
                {
                    AttendanceStatus _attendanceStatus = _attendanceStatus = new AttendanceStatus() { AttendanceId = Convert.ToInt32(id), IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = true, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = Request.UserHostAddress, LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                    objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                }
                return base.Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return base.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authenticate]
        public ActionResult NewAddAttendance()
        {
            VMAddAttendance model = new VMAddAttendance();
            try
            {
                #region :: Load User List
                LoadAddUserInitialData();
                #endregion :: Load User List

                model.AttendanceDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }



        [HttpPost]
        [Authenticate]
        public ActionResult NewAddAttendance(VMAddAttendance model)
        {
            #region Step1 - Check Attendance Already Exists
            IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
            IAttendanceDetailService objAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
            IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
            IAttendancePolicyService objAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");

            IHolidayService objHolidayService = IoC.Resolve<IHolidayService>("HolidayService");
            IShiftOffDayService objShiftOffDayService = IoC.Resolve<IShiftOffDayService>("ShiftOffDayService");
            ILeaveService leaveService = IoC.Resolve<ILeaveService>("LeaveService");


            List<Attendance> attendanceList = objAttendanceService.GetAttendanceByUserIDAndDate(model.UserId, model.AttendanceDate);
            Attendance attendance = null;
            #region  nulll DateTimeIn & DateTimeOut


            if (model.DateTimeIn == null && model.DateTimeOut == null)
            {
                int count = 0;
                List<UserShift> userShifts = objUserShiftService.GetUserShiftByUserId(model.UserId);
                userShifts = userShifts.Where(x => x.EffectiveDate <= model.AttendanceDate && (x.RetiredDate == null || x.RetiredDate >= model.AttendanceDate)).ToList();
                UserShift userShift = userShifts.FirstOrDefault();

                #endregion Step2
                #region Step3 - Get and Process Policy



                List<AttendancePolicy> attendancePolicies = objAttendancePolicyService.GetAttendancePolicyByShiftId(userShift.ShiftId);
                attendancePolicies = attendancePolicies.Where(x => x.EffectiveDate <= model.AttendanceDate && (x.RetiredDate == null || x.RetiredDate >= model.AttendanceDate)).ToList();

                Shift shift = objShiftService.GetShift(userShift.ShiftId.Value);

                //ShiftOffDays
                List<ShiftOffDay> shiftOffDays = objShiftOffDayService.GetShiftOffDayByShiftId(userShift.ShiftId);
                if (shiftOffDays != null)
                {
                    shiftOffDays = shiftOffDays.Where(x => x.EffectiveDate <= model.AttendanceDate && (x.RetiredDate == null || x.RetiredDate >= model.AttendanceDate)).ToList();
                    foreach (ShiftOffDay shiftOffDay in shiftOffDays)
                    {
                        var attendanceDate = Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy"));

                        int dayNumberOfWeek = (int)attendanceDate.DayOfWeek;

                        //condition
                        if (shiftOffDay.OffDayOfWeek == dayNumberOfWeek)
                        {
                            attendance = new Attendance()
                            {
                                UserId = model.UserId,
                                Date = model.AttendanceDate,
                                DateTimeIn = model.DateTimeIn,
                                DateTimeOut = model.DateTimeOut,
                                CreationDate = DateTime.Now,
                                IsActive = true,
                                UpdateBy = AuthBase.UserId
                            };
                            attendance = objAttendanceService.InsertAttendance(attendance);
                            count++;
                        }
                    }
                }
                //Holidays
                List<Holiday> holidays = objHolidayService.GetAllHoliday();
                if (holidays != null)
                {
                    holidays = holidays.Where(x => x.Date == model.AttendanceDate).ToList();

                    foreach (Holiday holiday in holidays)
                    {
                        if (holiday.Date == model.AttendanceDate)
                        {

                            count++;

                        }
                    }
                }

                if (count == 0)
                {
                    attendance = new Attendance()
                    {
                        UserId = model.UserId,
                        Date = model.AttendanceDate,
                        DateTimeIn = model.DateTimeIn,
                        DateTimeOut = model.DateTimeOut,
                        CreationDate = DateTime.Now,
                        IsActive = true,
                        UpdateBy = AuthBase.UserId
                    };
                    attendance = objAttendanceService.InsertAttendance(attendance);
                }
            }
            #endregion
            else
            {
                if (attendanceList == null || attendanceList.Count <= 0)
                {
                    attendance = new Attendance()
                    {
                        UserId = model.UserId,
                        Date = model.AttendanceDate,
                        DateTimeIn = model.DateTimeIn,
                        DateTimeOut = model.DateTimeOut,
                        CreationDate = DateTime.Now,
                        IsActive = true,
                        UpdateBy = AuthBase.UserId
                    };
                    attendance = objAttendanceService.InsertAttendance(attendance);
                }
                else
                {
                    attendance = attendanceList.FirstOrDefault();
                }
                AttendanceDetail attendanceDetail = new AttendanceDetail() { AttendanceId = attendance.Id, StartDate = model.DateTimeIn, EndDate = model.DateTimeOut, CreationDate = DateTime.Now, UpdateBy = AuthBase.UserId, IsActive = true };
                attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(attendanceDetail);

                List<AttendanceDetail> attendanceDetails = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(attendance.Id);
                var DateTimeIn = attendance.DateTimeIn = attendanceDetails.Where(x => x.StartDate != null).Min(x => x.StartDate);
                var DateTimeOut = attendance.DateTimeOut = attendanceDetails.Where(x => x.EndDate != null).Max(x => x.EndDate);
                attendance = objAttendanceService.UpdateAttendance(attendance);

                #endregion Step1

                #region Step2 - Get Shift and Init AttendanceStatus
                List<AttendanceStatus> attendanceStatuses = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(attendance.Id);
                AttendanceStatus attendanceStatus = null;
                if (attendanceStatuses == null || attendanceStatuses.Count <= 0)
                {
                    attendanceStatus = new AttendanceStatus()
                    {
                        AttendanceId = attendance.Id,
                        IsShiftOffDay = false,
                        IsHoliday = false,
                        IsLeaveDay = false,
                        IsQuarterDay = false,
                        IsHalfDay = false,
                        IsFullDay = false,
                        IsLate = false,
                        IsEarly = false,
                        LateMinutes = 0,
                        EarlyMinutes = 0,
                        WorkingMinutes = 0,
                        TotalMinutes = 0,
                        OverTimeMinutes = 0,
                        CreationDate = DateTime.Now,
                        IsActive = true,
                        UpdateBy = AuthBase.UserId
                    };
                    attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(attendanceStatus);
                }
                else
                {
                    attendanceStatus = attendanceStatuses.FirstOrDefault();
                }
                List<UserShift> userShifts = objUserShiftService.GetUserShiftByUserId(attendance.UserId);
                userShifts = userShifts.Where(x => x.EffectiveDate <= attendance.Date && (x.RetiredDate == null || x.RetiredDate >= attendance.Date)).ToList();
                UserShift userShift = userShifts.FirstOrDefault();

                #endregion Step2
                #region Step3 - Get and Process Policy



                List<AttendancePolicy> attendancePolicies = objAttendancePolicyService.GetAttendancePolicyByShiftId(userShift.ShiftId);
                attendancePolicies = attendancePolicies.Where(x => x.EffectiveDate <= attendance.Date && (x.RetiredDate == null || x.RetiredDate >= attendance.Date)).ToList();

                Shift shift = objShiftService.GetShift(userShift.ShiftId.Value);

                //ShiftOffDays
                List<ShiftOffDay> shiftOffDays = objShiftOffDayService.GetShiftOffDayByShiftId(userShift.ShiftId);
                if (shiftOffDays != null)
                {
                    shiftOffDays = shiftOffDays.Where(x => x.EffectiveDate <= attendance.Date && (x.RetiredDate == null || x.RetiredDate >= attendance.Date)).ToList();
                    foreach (ShiftOffDay shiftOffDay in shiftOffDays)
                    {
                        var attendanceDate = Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy"));

                        int dayNumberOfWeek = (int)attendanceDate.DayOfWeek;

                        //condition
                        if (shiftOffDay.OffDayOfWeek == dayNumberOfWeek)
                        {
                            attendanceStatus.IsShiftOffDay = true;
                            break;
                        }
                    }
                }
                //Holidays
                List<Holiday> holidays = objHolidayService.GetAllHoliday();
                if (holidays != null)
                {
                    holidays = holidays.Where(x => x.Date == attendance.Date).ToList();

                    foreach (Holiday holiday in holidays)
                    {
                        if (holiday.Date == attendance.Date)
                        {
                            attendanceStatus.IsHoliday = true;
                            break;
                        }
                    }
                }
                //TotalMinutes

                attendanceStatus.TotalMinutes = Convert.ToInt32((Convert.ToDateTime(DateTimeOut) - Convert.ToDateTime(DateTimeIn)).TotalMinutes);


                //LateMinutes
                if (attendanceStatus.LateMinutes == 0)
                {
                    double LateMinutes = (model.DateTimeIn.Value - Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy") + " " + shift.StartHour)).TotalMinutes;
                    if (LateMinutes > 0)
                        attendanceStatus.LateMinutes = Convert.ToInt32(LateMinutes);
                    else
                        attendanceStatus.LateMinutes = 0;
                    //EarlyMinutes &
                    //OverTimeMinutes
                }
                double EarlyMinutes = (Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy") + " " + shift.EndHour) - model.DateTimeOut.Value).TotalMinutes;
                if (EarlyMinutes >= 0)
                    attendanceStatus.EarlyMinutes = Convert.ToInt32(EarlyMinutes);
                else
                {
                    attendanceStatus.EarlyMinutes = 0;
                    attendanceStatus.OverTimeMinutes = Math.Abs(Convert.ToInt32(EarlyMinutes));
                }



                //Working Minutes
                AttendanceDetail last = attendanceDetails.Last();
                foreach (AttendanceDetail wm in attendanceDetails)
                {

                    if (wm.Equals(last))
                    {
                        attendanceStatus.WorkingMinutes += Convert.ToInt32((wm.EndDate.Value - wm.StartDate.Value).TotalMinutes);
                    }

                }
                //List<Holiday> holidays = 
                foreach (AttendancePolicy attendancePolicy in attendancePolicies)
                {
                    switch ((Core.Enum.AttendanceVariable)attendancePolicy.AttendanceVariableId)
                    {
                        case Core.Enum.AttendanceVariable.FullDay:
                            //condition full day
                            if (attendanceStatus.LateMinutes / 60 >= attendancePolicy.Hours.Value || attendanceStatus.EarlyMinutes / 60 <= attendancePolicy.Hours.Value)
                                attendanceStatus.IsFullDay = true;
                            else
                                attendanceStatus.IsFullDay = false;
                            break;

                        case Core.Enum.AttendanceVariable.HalfDay:
                            //condition half day
                            if (attendanceStatus.LateMinutes / 60 >= attendancePolicy.Hours.Value || attendanceStatus.EarlyMinutes / 60 >= attendancePolicy.Hours.Value)
                                attendanceStatus.IsHalfDay = true;
                            else
                                attendanceStatus.IsHalfDay = false;
                            break;

                        case Core.Enum.AttendanceVariable.QuarterDay:
                            //condition quarter day
                            if (attendanceStatus.LateMinutes / 60 >= attendancePolicy.Hours.Value || attendanceStatus.EarlyMinutes / 60 >= attendancePolicy.Hours.Value)
                                attendanceStatus.IsQuarterDay = true;
                            else
                                attendanceStatus.IsQuarterDay = false;
                            break;

                        case Core.Enum.AttendanceVariable.Late:
                            //condition IsLate day

                            if ((decimal)attendanceStatus.LateMinutes / 60 >= (decimal)attendancePolicy.Hours.Value)
                                attendanceStatus.IsLate = true;
                            else
                                attendanceStatus.IsLate = false;
                            break;

                        case Core.Enum.AttendanceVariable.Early:
                            //condition IsEarly day
                            if ((decimal)attendanceStatus.EarlyMinutes / 60 >= (decimal)attendancePolicy.Hours.Value)
                                attendanceStatus.IsEarly = true;
                            else
                                attendanceStatus.IsEarly = false;
                            break;

                    }
                }
                #endregion Step3
                #region Step4 - Update Attendance Status
                attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(attendanceStatus);
                #endregion Step4
            }
            return View(model);
        }

        [HttpGet]
        [Authenticate]
        public ActionResult AddAttendance()
        {
            VMAddAttendance model = new VMAddAttendance();
            try
            {
                #region :: Load User List
                LoadAddUserInitialData();
                #endregion :: Load User List

                model.AttendanceDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        [Authenticate]
        public ActionResult AddAttendance(VMAddAttendance model)
        {
            try
            {

                #region Init
                List<Attendance> _attendanceList = null;
                Attendance _attendance = null;
                List<AttendanceDetail> _attendanceDetailList = null;
                AttendanceDetail _attendanceDetail = null;
                List<AttendanceStatus> _attendanceStatusList = null;
                AttendanceStatus _attendanceStatus = null;
                List<VMAttendanceData> attendanceDataList = new List<VMAttendanceData>();

                List<UserShift> _userShiftList = null;
                List<AttendancePolicy> _attendancePolicyList = null;
                List<AttendancePolicy> _attendancePolicyListTemp = null;
                UserShift _userShift = null;
                Shift _shift = null;
                AttendancePolicy _attendancePolicy = null;

                IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
                IAttendanceDetailService objAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
                IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
                IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
                IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                IAttendancePolicyService objAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");

                model.DateTimeIn = Convert.ToDateTime(model.AttendanceDate.ToShortDateString() + " " + model.TimeIn);
                model.DateTimeOut = Convert.ToDateTime(model.AttendanceDate.ToShortDateString() + " " + model.TimeOut);
                var TimeIn = model.TimeIn;


                #endregion Init

                #region Attendance
                _attendanceList = objAttendanceService.GetAttendanceByUserIDAndDate(model.UserId, model.AttendanceDate);
                bool a = true;
                //Insert Attendance
                if (_attendanceList == null || _attendanceList.Count <= 0)
                {
                    if (a)
                    {
                        _attendance = new Attendance() { UserId = model.UserId, Date = model.AttendanceDate, IsActive = true, CreationDate = DateTime.Now, UserIp = Request.UserHostAddress };
                        _attendance = objAttendanceService.InsertAttendance(_attendance);
                        ViewBag.Message = "Addedd successfully";
                        ViewBag.MsgClass = "alert-success";
                    }
                }
                //Attendance Already marked, Get Attendance
                else
                {
                    _attendance = _attendanceList.FirstOrDefault();
                    ViewBag.Message = "Updated successfully";
                    ViewBag.MsgClass = "alert-success";
                }
                #endregion Attendance

                #region AttendanceDetail
                //Timein Entry
                if (model.TimeIn != null)
                {
                    _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                    //Already Timein Entry
                    if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                    {
                        _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
                        if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                        {
                            _attendanceDetail = _attendanceDetailList.FirstOrDefault();


                            #region :: Commented to add Update Functionality
                            /*
                            if (_attendanceDetailList.FirstOrDefault().StartDate == null)
                            {
                                _attendanceDetail.StartDate = model.DateTimeIn;
                                _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                            }*/
                            #endregion :: Commented to add Update Functionality
                            _attendanceDetail.StartDate = model.DateTimeIn;
                            _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                        }
                        else
                        {
                            //      Insert Timein
                            _attendanceDetail = new AttendanceDetail()
                            {
                                AttendanceId = _attendance.Id,
                                AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                StartDate = model.DateTimeIn,
                                IsActive = true,
                                UserIp = Request.UserHostAddress,
                                CreationDate = DateTime.Now
                            };
                            _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                        }
                    }
                    else
                    {
                        //Insert Timein
                        _attendanceDetail = new AttendanceDetail()
                        {
                            AttendanceId = _attendance.Id,
                            AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                            StartDate = model.DateTimeIn,
                            IsActive = true,
                            UserIp = Request.UserHostAddress,
                            CreationDate = DateTime.Now
                        };

                        _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                    }
                }
                // Timeout Entry
                if (model.TimeOut != null)
                {
                    _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                    //Already Timeout Entry
                    if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                    {
                        _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
                        if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                        {
                            #region Commented to add Update Functionality
                            /*
                            if (_attendanceDetailList.FirstOrDefault().EndDate == null)
                            {
                                _attendanceDetail = _attendanceDetailList.FirstOrDefault();
                                _attendanceDetail.EndDate = model.DateTimeOut;
                                _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                            }*/
                            _attendanceDetail = _attendanceDetailList.FirstOrDefault();
                            _attendanceDetail.EndDate = model.DateTimeOut;
                            _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                            #endregion Commented to add Update Functionality
                        }
                    }
                }
                #endregion AttendanceDetail

                #region AttendanceStatus
                if (_attendanceDetail != null && _attendanceDetail.Id > 0)
                {
                    _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                    if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                    {
                        _attendanceStatus = _attendanceStatusList.FirstOrDefault();
                    }
                    else
                    {
                        _attendanceStatus = new AttendanceStatus()
                        {
                            AttendanceId = _attendance.Id,
                            IsShiftOffDay = false,
                            IsLeaveDay = false,
                            IsHoliday = false,
                            IsFullDay = false,
                            IsQuarterDay = false,
                            IsHalfDay = false,
                            IsLate = false,
                            IsActive = true,
                            CreationDate = DateTime.Now,
                            UserIp = Request.UserHostAddress,
                            BreakType = model.BreakType
                        };
                    }
                    _userShiftList = objUserShiftService.GetUserShiftByUserId(_attendance.UserId);
                    if (_userShiftList != null && _userShiftList.Count > 0)
                    {
                        _userShiftList = _userShiftList.Where(x => model.AttendanceDate >= x.EffectiveDate && model.AttendanceDate <= (x.RetiredDate == null ? model.AttendanceDate : x.RetiredDate)).ToList();
                        if (_userShiftList != null && _userShiftList.Count > 0)
                        {
                            _userShift = _userShiftList.FirstOrDefault();
                            _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
                            if (!_shift.StartHour.Contains(":"))
                                _shift.StartHour += ":00";
                            _attendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(_userShift.ShiftId.Value);
                            if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                            {
                                _attendancePolicyList = _attendancePolicyList.Where(x => model.AttendanceDate >= x.EffectiveDate && model.AttendanceDate <= (x.RetiredDate == null ? model.AttendanceDate : x.RetiredDate)).ToList();
                                if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                {
                                    // FULL Day
                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                    {
                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                        if ((decimal)(model.DateTimeIn.Value - Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                            _attendanceStatus.IsFullDay = true;
                                        else
                                            _attendanceStatus.IsFullDay = false;
                                    }
                                    else
                                    {
                                        _attendanceStatus.IsFullDay = false;
                                    }

                                    // Half Day
                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.HalfDay).ToList();
                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value)
                                    {
                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                        if ((decimal)(model.DateTimeIn.Value - Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                            _attendanceStatus.IsHalfDay = true;
                                        else
                                            _attendanceStatus.IsHalfDay = false;
                                    }
                                    else
                                    {
                                        _attendanceStatus.IsHalfDay = false;
                                    }

                                    // Quater Day
                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.QuarterDay).ToList();
                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value && !_attendanceStatus.IsHalfDay.Value)
                                    {
                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                        if ((decimal)(model.DateTimeIn.Value - Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                            _attendanceStatus.IsQuarterDay = true;
                                        else
                                            _attendanceStatus.IsQuarterDay = false;
                                    }
                                    else
                                    {
                                        _attendanceStatus.IsQuarterDay = false;
                                    }
                                }
                                else
                                {
                                    //Late
                                    if (model.DateTimeIn > Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy") + " " + _shift.StartHour))
                                        _attendanceStatus.IsLate = true;
                                    else
                                        _attendanceStatus.IsLate = false;
                                }
                            }
                            //Late
                            if (model.DateTimeIn > Convert.ToDateTime(model.AttendanceDate.ToString("MM/dd/yyyy") + " " + _shift.StartHour))
                                _attendanceStatus.IsLate = true;
                            else
                                _attendanceStatus.IsLate = false;
                        }
                    }
                    if (_attendanceStatus != null && _attendanceStatus.Id <= 0)
                        _attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                    else
                    {
                        _attendanceStatus.UpdateDate = DateTime.Now;
                        _attendanceStatus.BreakType = model.BreakType;
                        _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
                    }
                }
                #endregion AttendanceStatus

                #region :: Load User List
                LoadAddUserInitialData();
                #endregion :: Load User List

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "";
                ViewBag.MsgClass = "hide";
                ModelState.AddModelError("Error", ex.Message);

            }
            return View(model);
        }

        private void LoadAddUserInitialData()
        {
            IUserService objUserService = IoC.Resolve<IUserService>("UserService");
            List<User> UserList = objUserService.GetAllUser();

            if (UserList != null && UserList.Count > 0)
                ViewBag.UserList = new SelectList(UserList, "Id", "FirstName");
            else
            {
                List<SelectListItem> blank = new List<SelectListItem>();
                blank.Add(new SelectListItem() { Text = "User", Value = "-1" });
                ViewBag.UserList = new SelectList(blank, "Value", "Text", "");
            }

            List<SelectListItem> BreakTypeList = new List<SelectListItem>();
            BreakTypeList.Add(new SelectListItem { Text = "Meeting", Value = "M" });
            BreakTypeList.Add(new SelectListItem { Text = "Field", Value = "F" });

            ViewBag.BreakTypeList = new SelectList(BreakTypeList, "Value", "Text", "");
        }





        //[HttpPost]
        //[Authenticate]
        //public ActionResult Browse(string posted = null)
        //{
        //    #region Validate
        //    DateTime dtAttendance = new DateTime();
        //    if (String.IsNullOrWhiteSpace(Request.Form["AttendanceDate"]))
        //    {
        //        ViewBag.Message = "Please select date";
        //        return View();
        //    }
        //    else if (!DateTime.TryParse(Request.Form["AttendanceDate"], out dtAttendance))
        //    {
        //        ViewBag.Message = "Please select correct date";
        //        return View();
        //    }
        //    else if (Request.Files == null || Request.Files.Count <= 0 || Request.Files[0].ContentLength <= 0)
        //    {
        //        ViewBag.Message = "Please browse file";
        //        return View();
        //    }
        //    #endregion Validate
        //    #region File Upload
        //    HttpPostedFileBase file = Request.Files[0];
        //    if (!Directory.Exists(Server.MapPath("/Uploads")))
        //        Directory.CreateDirectory(Server.MapPath("/Uploads"));
        //    if (!Directory.Exists(Server.MapPath("/Uploads/AttendanceData")))
        //        Directory.CreateDirectory(Server.MapPath("/Uploads/AttendanceData"));
        //    if (!Directory.Exists(Server.MapPath("/Uploads/AttendanceData/" + dtAttendance.ToString("yyyyMMM"))))
        //        Directory.CreateDirectory(Server.MapPath("/Uploads/AttendanceData/" + dtAttendance.ToString("yyyyMMM")));
        //    string fileName = dtAttendance.ToString("ddhhmmss") + "-" + file.FileName;
        //    string fullFilePath = Server.MapPath("/Uploads/AttendanceData/" + dtAttendance.ToString("yyyyMMM") + "/" + fileName);
        //    file.SaveAs(fullFilePath);
        //    #endregion File Upload
        //    #region Processing
        //    #region Init

        //    List<Attendance> _attendanceList = null;
        //    Attendance _attendance = null;
        //    List<AttendanceDetail> _attendanceDetailList = null;
        //    AttendanceDetail _attendanceDetail = null;
        //    List<AttendanceStatus> _attendanceStatusList = null;
        //    AttendanceStatus _attendanceStatus = null;
        //    VMAttendanceData _attendanceData = null;
        //    VMAttendanceData _attendanceTimeInData = null;
        //    VMAttendanceData _attendanceTimeOutData = null;
        //    List<VMAttendanceData> attendanceDataList = new List<VMAttendanceData>();
        //    List<VMAttendanceData> attendanceDataListTemp = null;

        //    List<UserShift> _userShiftList = null;
        //    List<AttendancePolicy> _attendancePolicyList = null;
        //    List<AttendancePolicy> _attendancePolicyListTemp = null;
        //    UserShift _userShift = null;
        //    Shift _shift = null;
        //    AttendancePolicy _attendancePolicy = null;

        //    IAttendanceService objAttendanceService = IoC.Resolve<IAttendanceService>("AttendanceService");
        //    IAttendanceDetailService objAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
        //    IAttendanceStatusService objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
        //    IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
        //    IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
        //    IAttendancePolicyService objAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");

        //    #endregion Init
        //    #region ReadData
        //    string[] lines = System.IO.File.ReadAllLines(fullFilePath);
        //    int i = 0;
        //    foreach (string line in lines)
        //    {
        //        if (i == 0) { i++; continue; }
        //        string[] parts = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
        //        _attendanceData = new VMAttendanceData()
        //        {
        //            UserId = Convert.ToInt32(parts[(int)Cols.EnNo]),
        //            BreakType = parts[(int)Cols.In_Out],
        //            TimeStamp = Convert.ToDateTime(parts[(int)Cols.DateTime])
        //        };
        //        attendanceDataList.Add(_attendanceData);
        //    }
        //    #endregion ReadData
        //    #region FilterByUser
        //    attendanceDataList = attendanceDataList.OrderBy(x => x.UserId).ThenBy(x => x.TimeStamp).ToList();
        //    List<int> userIds = attendanceDataList.Select(x => x.UserId).Distinct().ToList();
        //    foreach (int userId in userIds)
        //    {
        //        #region Attendance
        //        _attendanceList = objAttendanceService.GetAttendanceByUserIDAndDate(userId, dtAttendance);
        //        //Insert Attendance
        //        if (_attendanceList == null || _attendanceList.Count <= 0)
        //        {
        //            _attendance = new Attendance() { UserId = userId, Date = dtAttendance, IsActive = true, CreationDate = DateTime.Now, UserIp = Request.UserHostAddress };
        //            _attendance = objAttendanceService.InsertAttendance(_attendance);
        //        }
        //        //Attendance Already marked, Get Attendance
        //        else
        //        {
        //            _attendance = _attendanceList.FirstOrDefault();
        //        }
        //        #endregion Attendance
        //        #region AttendanceDetail
        //        //TIMEIN
        //        attendanceDataListTemp = attendanceDataList.Where(x => x.TimeStamp.Date == dtAttendance && x.UserId == userId && x.BreakType == "DutyOn").ToList();
        //        if (attendanceDataListTemp != null && attendanceDataListTemp.Count > 0)
        //            _attendanceTimeInData = attendanceDataListTemp.FirstOrDefault();
        //        //Timein Entry
        //        if (_attendanceTimeInData != null)
        //        {
        //            _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
        //            //Already Timein Entry
        //            if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
        //            {
        //                _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
        //                if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
        //                {
        //                    _attendanceDetailList = _attendanceDetailList.Where(x => x.StartDate == _attendanceTimeInData.TimeStamp).ToList();
        //                    if (_attendanceDetailList == null || _attendanceDetailList.Count <= 0)
        //                    {
        //                        //Insert Timein
        //                        _attendanceDetail = new AttendanceDetail()
        //                        {
        //                            AttendanceId = _attendance.Id,
        //                            AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
        //                            StartDate = _attendanceTimeInData.TimeStamp,
        //                            IsActive = true,
        //                            UserIp = Request.UserHostAddress,
        //                            CreationDate = DateTime.Now
        //                        };
        //                        _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
        //                    }
        //                    else
        //                    {
        //                        //Do nothing, timin already marked
        //                    }
        //                }
        //                else
        //                {
        //                    //Insert Timein
        //                    _attendanceDetail = new AttendanceDetail()
        //                    {
        //                        AttendanceId = _attendance.Id,
        //                        AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
        //                        StartDate = _attendanceTimeInData.TimeStamp,
        //                        IsActive = true,
        //                        UserIp = Request.UserHostAddress,
        //                        CreationDate = DateTime.Now
        //                    };
        //                    _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
        //                }
        //            }
        //            else
        //            {
        //                //Insert Timein
        //                _attendanceDetail = new AttendanceDetail()
        //                {
        //                    AttendanceId = _attendance.Id,
        //                    AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
        //                    StartDate = _attendanceTimeInData.TimeStamp,
        //                    IsActive = true,
        //                    UserIp = Request.UserHostAddress,
        //                    CreationDate = DateTime.Now
        //                };
        //                _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
        //            }
        //        }
        //        //TIMEOUT
        //        attendanceDataListTemp = attendanceDataList.Where(x => x.UserId == userId && x.BreakType == "DutyOff").ToList();
        //        if (attendanceDataListTemp != null && attendanceDataListTemp.Count > 0)
        //            _attendanceTimeOutData = attendanceDataListTemp.Last();
        //        //Timeout Entry
        //        if (_attendanceTimeOutData != null)
        //        {
        //            _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
        //            if (_attendanceDetailList != null && _attendanceDetailList.Count > 0 && _attendanceDetailList.Count > 0)
        //            {
        //                _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance
        //                    && x.EndDate == null).ToList();
        //                if (_attendanceDetailList != null && _attendanceDetailList.Count > 0 && _attendanceDetailList.Count > 0)
        //                {
        //                    _attendanceDetail = _attendanceDetailList.FirstOrDefault();
        //                    _attendanceDetail.EndDate = _attendanceTimeOutData.TimeStamp;
        //                    _attendanceDetail.UpdateDate = DateTime.Now;
        //                    _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
        //                }
        //            }
        //        }
        //        #endregion AttendanceDetail
        //        #region AttendanceStatus
        //        if (_attendanceDetail != null && _attendanceDetail.Id > 0 && _attendanceTimeInData != null)
        //        {
        //            _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
        //            if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
        //            {
        //                _attendanceStatus = _attendanceStatusList.FirstOrDefault();
        //            }
        //            else
        //            {
        //                _attendanceStatus = new AttendanceStatus() { AttendanceId = _attendance.Id, IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsActive = true, CreationDate = DateTime.Now, UserIp = Request.UserHostAddress };
        //            }
        //            _userShiftList = objUserShiftService.GetUserShiftByUserId(_attendance.UserId);
        //            if (_userShiftList != null && _userShiftList.Count > 0)
        //            {
        //                _userShiftList = _userShiftList.Where(x => x.RetiredDate == null).ToList();
        //                if (_userShiftList != null && _userShiftList.Count > 0)
        //                {
        //                    _userShift = _userShiftList.FirstOrDefault();
        //                    _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
        //                    _attendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(_userShift.ShiftId.Value);
        //                    if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
        //                    {
        //                        //FULL Day
        //                        _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
        //                        if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
        //                        {
        //                            _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
        //                            if ((_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendanceTimeInData.TimeStamp.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalHours >= _attendancePolicy.Hours.Value)
        //                                _attendanceStatus.IsFullDay = true;
        //                            else
        //                                _attendanceStatus.IsFullDay = false;
        //                        }
        //                        else
        //                        {
        //                            _attendanceStatus.IsFullDay = false;
        //                        }

        //                        //Half Day
        //                        _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.HalfDay).ToList();
        //                        if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsFullDay.Value)
        //                        {
        //                            _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
        //                            if ((_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendanceTimeInData.TimeStamp.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalHours >= _attendancePolicy.Hours.Value)
        //                                _attendanceStatus.IsHalfDay = true;
        //                            else
        //                                _attendanceStatus.IsHalfDay = false;
        //                        }
        //                        else
        //                        {
        //                            _attendanceStatus.IsHalfDay = false;
        //                        }

        //                        //Quater Day
        //                        _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.QuarterDay).ToList();
        //                        if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0 && !_attendanceStatus.IsHalfDay.Value)
        //                        {
        //                            _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
        //                            if ((_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendanceTimeInData.TimeStamp.ToString("MM/dd/yyyy") + " " + _shift.StartHour)).TotalHours >= _attendancePolicy.Hours.Value)
        //                                _attendanceStatus.IsQuarterDay = true;
        //                            else
        //                                _attendanceStatus.IsQuarterDay = false;
        //                        }
        //                        else
        //                        {
        //                            _attendanceStatus.IsQuarterDay = false;
        //                        }
        //                    }
        //                    //Late
        //                    if (_attendanceTimeInData.TimeStamp > Convert.ToDateTime(_attendanceTimeInData.TimeStamp.ToString("MM/dd/yyyy") + " " + _shift.StartHour))
        //                        _attendanceStatus.IsLate = true;
        //                    else
        //                        _attendanceStatus.IsLate = false;
        //                }
        //                else
        //                {
        //                    //Late
        //                    if (_attendanceTimeInData.TimeStamp > Convert.ToDateTime(_attendanceTimeInData.TimeStamp.ToString("MM/dd/yyyy") + " " + _shift.StartHour))
        //                        _attendanceStatus.IsLate = true;
        //                    else
        //                        _attendanceStatus.IsLate = false;
        //                }
        //            }
        //            else
        //            {
        //                //Late
        //                if (_attendanceTimeInData.TimeStamp > Convert.ToDateTime(_attendanceTimeInData.TimeStamp.ToString("MM/dd/yyyy") + " " + _shift.StartHour))
        //                    _attendanceStatus.IsLate = true;
        //                else
        //                    _attendanceStatus.IsLate = false;
        //            }
        //            if (_attendanceStatus != null && _attendanceStatus.Id <= 0)
        //                _attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
        //            else
        //            {
        //                _attendanceStatus.UpdateDate = DateTime.Now;
        //                _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
        //            }
        //        }
        //        #endregion AttendanceStatus

        //        _attendance = null;
        //        _attendanceList = null;
        //        _attendanceDetailList = null;
        //        _attendanceDetail = null;
        //        _attendanceList = null;
        //        _attendanceStatus = null;
        //        _attendanceStatusList = null;
        //        _attendanceTimeInData = null;
        //        _attendanceTimeOutData = null;
        //        _shift = null;
        //        _userShift = null;
        //        _userShiftList = null;
        //    }
        //    #endregion FilterByUser
        //    #endregion Processing

        //    ViewBag.Message = "File processed successfully";
        //    return View();
        //}
    }
    public enum Cols
    {
        EmpNo = 0,
        Date = 1,
        TimeIn = 2,
        TimeOut = 3
    }
    public class UserDate
    {
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
    }
}

