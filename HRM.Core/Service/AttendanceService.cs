using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Attendance;
using Validation;
using System.Linq;
using System.Data;
using HRM.Core.Model;

namespace HRM.Core.Service
{

    public class AttendanceService : IAttendanceService
    {
        private IAttendanceRepository _iAttendanceRepository;

        public AttendanceService(IAttendanceRepository iAttendanceRepository)
        {
            this._iAttendanceRepository = iAttendanceRepository;
        }

        public Dictionary<string, string> GetAttendanceBasicSearchColumns()
        {

            return this._iAttendanceRepository.GetAttendanceBasicSearchColumns();

        }

        public List<SearchColumn> GetAttendanceAdvanceSearchColumns()
        {

            return this._iAttendanceRepository.GetAttendanceAdvanceSearchColumns();

        }


        public virtual List<Attendance> GetAttendanceByUserId(System.Int32? UserId)
        {
            return _iAttendanceRepository.GetAttendanceByUserId(UserId);
        }

        public Attendance GetAttendance(System.Int32 Id)
        {
            return _iAttendanceRepository.GetAttendance(Id);
        }

        public Attendance UpdateAttendance(Attendance entity)
        {
            return _iAttendanceRepository.UpdateAttendance(entity);
        }

        public Attendance UpdateAttendanceByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
        {
            return _iAttendanceRepository.UpdateAttendanceByKeyValue(UpdateKeyValue, Id);
        }

        public bool DeleteAttendance(System.Int32 Id)
        {
            return _iAttendanceRepository.DeleteAttendance(Id);
        }

        public List<Attendance> GetAllAttendance()
        {
            return _iAttendanceRepository.GetAllAttendance();
        }

        public List<Attendance> GetAllAttendanceWithAttendanceDetail(string StartDate, string EndDate)
        {
            IAttendanceDetailService ObjAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
            IAttendanceStatusService ObjAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
            IUserService ObjUserService = IoC.Resolve<IUserService>("UserService");
            IUserDepartmentService objUserDepartmentService = IoC.Resolve<IUserDepartmentService>("UserDepartmentService");
            IDepartmentService objDepartmentService = IoC.Resolve<IDepartmentService>("DepartmentService");
            IUserShiftService objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
            IShiftService objShiftService = IoC.Resolve<IShiftService>("ShiftService");
            DateTime dtStart = DateTime.Now;
            List<Attendance> _attendanceList = new List<Attendance>();
            if (_iAttendanceRepository.GetAllAttendance() != null && _iAttendanceRepository.GetAllAttendance().Where(x => x.Date >= Convert.ToDateTime(StartDate) && x.Date <= Convert.ToDateTime(EndDate) && x.IsActive == true) != null)
            {
                _attendanceList = _iAttendanceRepository.GetAllAttendance().Where(x => x.Date >= Convert.ToDateTime(StartDate) && x.Date <= Convert.ToDateTime(EndDate) && x.IsActive == true).ToList();
                if (_attendanceList.Count > 0)
                {
                    foreach (var attendance in _attendanceList)
                    {
                        attendance.User = ObjUserService.GetUserWithDepartment((int)attendance.UserId);
                        if (attendance.User != null)
                        {
                            List<UserDepartment> UserDepartmentList = objUserDepartmentService.GetAllUserDepartment().Where(x => x.UserId == attendance.User.Id && attendance.Date >= x.EffectiveDate && attendance.Date <= (x.RetiredDate == null ? attendance.Date : x.RetiredDate)).ToList();
                            if (UserDepartmentList.Count > 0)
                            {
                                attendance.User.UserDepartment = UserDepartmentList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                                attendance.User.Department = objDepartmentService.GetAllDepartment().Where(x => x.Id == attendance.User.UserDepartment.DepartmentId).FirstOrDefault();
                            }

                            List<UserShift> UserShiftList = objUserShiftService.GetAllUserShift();
                            if (UserShiftList != null)
                            {
                                UserShiftList = UserShiftList.Where(x => x.UserId == attendance.User.Id && attendance.Date >= x.EffectiveDate && attendance.Date <= (x.RetiredDate == null ? attendance.Date : x.RetiredDate)).ToList();
                                if (UserShiftList.Count > 0)
                                {
                                    attendance.User.UserShift = UserShiftList.OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                                    attendance.User.Shift = objShiftService.GetAllShift().Where(x => x.Id == attendance.User.UserShift.ShiftId).FirstOrDefault();
                                }
                            }
                        }
                        List<AttendanceDetail> AttendanceDetailList = ObjAttendanceDetailService.GetAllAttendanceDetail().Where(x => x.AttendanceId == attendance.Id && x.AttendanceTypeId == (int)HRM.Core.Enum.AttendanceType.DailyAttendance && x.IsActive == true).ToList();
                        if (AttendanceDetailList.Count > 0)
                        {
                            attendance.AttendanceDetailTimeIn = AttendanceDetailList.OrderBy(x => x.StartDate).FirstOrDefault();
                            attendance.AttendanceDetailTimeOut = AttendanceDetailList.OrderByDescending(x => x.EndDate).FirstOrDefault();
                            if (attendance.AttendanceDetailTimeOut != null && attendance.AttendanceDetailTimeOut.EndDate != null)
                            {
                                TimeSpan diff = (TimeSpan)(attendance.AttendanceDetailTimeOut.EndDate - attendance.AttendanceDetailTimeIn.StartDate);
                                attendance.WorkingHours = diff.ToString(@"hh\:mm");
                            }
                            else
                                attendance.WorkingHours = "00:00";
                        }
                        List<AttendanceStatus> AttendanceStatusList = ObjAttendanceStatusService.GetAllAttendanceStatus().Where(x => x.AttendanceId == attendance.Id && x.IsActive == true).ToList();
                        if (AttendanceStatusList.Count() > 0)
                        {
                            attendance.AttendanceStatus = AttendanceStatusList.FirstOrDefault();
                        }
                    }
                }
            }
            DateTime dtEnd = DateTime.Now;
            double hours = dtEnd.Subtract(dtStart).TotalHours;
            return _attendanceList;
        }

        public List<Attendance> GetAllAttendanceByDateUserWithDetail(string StartDate, string EndDate, Int32? UID)
        {
            List<Attendance> _attendanceList = new List<Attendance>();
            DataSet dsAttendance = _iAttendanceRepository.GetAttendanceByDateAndUser(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UID);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                Attendance objAttendance = null;
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    objAttendance = new Attendance();
                    objAttendance.User = new User();
                    objAttendance.User.Department = new Department();
                    objAttendance.User.Shift = new Shift();

                    objAttendance.User.Id = IntNull(dr["UserId"]);
                    objAttendance.User.FirstName = StringNull(dr["FirstName"]);
                    objAttendance.User.UserTypeId = IntNull(dr["UserTypeID"]);
                    objAttendance.User.Designation = StringNull(dr["Designation"]);
                    objAttendance.User.AccountNumber = StringNull(dr["AccountNumber"]);

                    objAttendance.User.Department.Id = IntNull(dr["DepartmentId"]);
                    objAttendance.User.Department.Name = StringNull(dr["DepartmentName"]);

                    objAttendance.User.Shift.Id = IntNull(dr["ShiftId"]);
                    objAttendance.User.Shift.Name = StringNull(dr["ShiftName"]);

                    objAttendance.AttendanceDetailTimeIn = new AttendanceDetail();
                    objAttendance.AttendanceDetailTimeIn.Id = IntNull(dr["XADTIAttendanceDetailId"]);
                    objAttendance.AttendanceDetailTimeIn.AttendanceId = IntNull(dr["XADTIAttendanceId"]);
                    objAttendance.AttendanceDetailTimeIn.StartDate = DateNull(dr["XADTIStartDate"]);
                    objAttendance.AttendanceDetailTimeIn.EndDate = DateNull(dr["XADTIEndDate"]);

                    objAttendance.AttendanceDetailTimeOut = new AttendanceDetail();
                    objAttendance.AttendanceDetailTimeOut.Id = IntNull(dr["XADTOAttendanceDetailId"]);
                    objAttendance.AttendanceDetailTimeOut.AttendanceId = IntNull(dr["XADTOAttendanceId"]);
                    objAttendance.AttendanceDetailTimeOut.StartDate = DateNull(dr["XADTOStartDate"]);
                    objAttendance.AttendanceDetailTimeOut.EndDate = DateNull(dr["XADTOEndDate"]);
                    if (objAttendance.AttendanceDetailTimeOut != null && objAttendance.AttendanceDetailTimeOut.EndDate != null)
                    {
                        TimeSpan diff = (TimeSpan)(objAttendance.AttendanceDetailTimeOut.EndDate - objAttendance.AttendanceDetailTimeIn.StartDate);
                        objAttendance.WorkingHours = diff.ToString(@"hh\:mm");
                    }
                    else
                        objAttendance.WorkingHours = "00:00";

                    objAttendance.AttendanceStatus = new AttendanceStatus();
                    objAttendance.AttendanceStatus.Id = IntNull(dr["AttendanceStatusId"]);
                    objAttendance.AttendanceStatus.IsShiftOffDay = BooleanNull(dr["IsShiftOffDay"]);
                    objAttendance.AttendanceStatus.IsHoliday = BooleanNull(dr["IsHoliday"]);
                    objAttendance.AttendanceStatus.IsLeaveDay = BooleanNull(dr["IsLeaveDay"]);
                    objAttendance.AttendanceStatus.IsQuarterDay = BooleanNull(dr["IsQuarterDay"]);
                    objAttendance.AttendanceStatus.IsHalfDay = BooleanNull(dr["IsHalfDay"]);
                    objAttendance.AttendanceStatus.IsFullDay = BooleanNull(dr["IsFullDay"]);
                    objAttendance.AttendanceStatus.IsLate = BooleanNull(dr["IsLate"]);
                    objAttendance.AttendanceStatus.IsEarly = BooleanNull(dr["IsEarly"]);
                    objAttendance.AttendanceStatus.LateMinutes = IntNull(dr["LateMinutes"]);
                    objAttendance.AttendanceStatus.EarlyMinutes = IntNull(dr["EarlyMinutes"]);
                    objAttendance.AttendanceStatus.WorkingMinutes = IntNull(dr["WorkingMinutes"]);
                    objAttendance.AttendanceStatus.TotalMinutes = IntNull(dr["TotalMinutes"]);
                    objAttendance.AttendanceStatus.OverTimeMinutes = IntNull(dr["OverTimeMinutes"]);
                    objAttendance.AttendanceStatus.Reason = StringNull(dr["Reason"]);
                    objAttendance.AttendanceStatus.IsApproved = BooleanNull(dr["IsApproved"]);
                    objAttendance.AttendanceStatus.Remarks = StringNull(dr["Remarks"]);
                    _attendanceList.Add(objAttendance);
                }
            }
            return _attendanceList;
        }
        public List<Attendance>     GetAllAttendanceByDateUserWithDetail(string StartDate, string EndDate, Int32? UID, Int32? BranchId)
        {
            List<Attendance> _attendanceList = new List<Attendance>();
            DataSet dsAttendance = _iAttendanceRepository.GetAttendanceByDateAndUser(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UID, BranchId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                Attendance objAttendance = null;
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    objAttendance = new Attendance();
                    objAttendance.Date = Convert.ToDateTime(dr["Date"]);

                    objAttendance.User = new User();
                    objAttendance.User.Department = new Department();
                    objAttendance.User.Shift = new Shift();


                    objAttendance.User.SalaryType = new SalaryType();
                    objAttendance.User.Branch = new Branch();

                    objAttendance.User.Id = IntNull(dr["UserId"]);
                    objAttendance.User.FirstName = StringNull(dr["FirstName"]);
                    objAttendance.User.UserTypeId = IntNull(dr["UserTypeID"]);
                    objAttendance.User.Designation = StringNull(dr["Designation"]);
                    objAttendance.User.AccountNumber = StringNull(dr["AccountNumber"]);
                    objAttendance.User.ImagePath = StringNull(dr["ImagePath"]);
                    
                    objAttendance.User.SalaryType.Id = IntNull(dr["SalaryTypeId"]);
                    objAttendance.User.SalaryType.Name = StringNull(dr["SalaryTypeName"]);

                    objAttendance.User.Department.Id = IntNull(dr["DepartmentId"]);
                    objAttendance.User.Department.Name = StringNull(dr["DepartmentName"]);

                    objAttendance.User.Shift.Id = IntNull(dr["ShiftId"]);
                    objAttendance.User.Shift.Name = StringNull(dr["ShiftName"]);

                    objAttendance.User.Branch.Id = IntNull(dr["BranchId"]);
                    objAttendance.User.Branch.Name = StringNull(dr["BranchName"]);

                    objAttendance.AttendanceDetailTimeIn = new AttendanceDetail();
                    objAttendance.AttendanceDetailTimeIn.Id = IntNull(dr["XADTIAttendanceDetailId"]);
                    objAttendance.AttendanceDetailTimeIn.AttendanceId = IntNull(dr["XADTIAttendanceId"]);
                    objAttendance.AttendanceDetailTimeIn.StartDate = DateNull(dr["XADTIStartDate"]);
                    objAttendance.AttendanceDetailTimeIn.EndDate = DateNull(dr["XADTIEndDate"]);

                    objAttendance.AttendanceDetailTimeOut = new AttendanceDetail();
                    objAttendance.AttendanceDetailTimeOut.Id = IntNull(dr["XADTOAttendanceDetailId"]);
                    objAttendance.AttendanceDetailTimeOut.AttendanceId = IntNull(dr["XADTOAttendanceId"]);
                    objAttendance.AttendanceDetailTimeOut.StartDate = DateNull(dr["XADTOStartDate"]);
                    objAttendance.AttendanceDetailTimeOut.EndDate = DateNull(dr["XADTOEndDate"]);
                    if (objAttendance.AttendanceDetailTimeOut != null && objAttendance.AttendanceDetailTimeOut.EndDate != null)
                    {
                        TimeSpan diff = (TimeSpan)(objAttendance.AttendanceDetailTimeOut.EndDate - objAttendance.AttendanceDetailTimeIn.StartDate);
                        objAttendance.WorkingHours = diff.ToString(@"hh\:mm");
                    }
                    else
                        objAttendance.WorkingHours = "00:00";

                    objAttendance.AttendanceStatus = new AttendanceStatus();
                    objAttendance.AttendanceStatus.Id = IntNull(dr["AttendanceStatusId"]);
                    objAttendance.AttendanceStatus.IsShiftOffDay = BooleanNull(dr["IsShiftOffDay"]);
                    objAttendance.AttendanceStatus.IsHoliday = BooleanNull(dr["IsHoliday"]);
                    objAttendance.AttendanceStatus.IsLeaveDay = BooleanNull(dr["IsLeaveDay"]);
                    objAttendance.AttendanceStatus.IsQuarterDay = BooleanNull(dr["IsQuarterDay"]);
                    objAttendance.AttendanceStatus.IsHalfDay = BooleanNull(dr["IsHalfDay"]);
                    objAttendance.AttendanceStatus.IsFullDay = BooleanNull(dr["IsFullDay"]);
                    objAttendance.AttendanceStatus.IsLate = BooleanNull(dr["IsLate"]);
                    objAttendance.AttendanceStatus.IsEarly = BooleanNull(dr["IsEarly"]);
                    objAttendance.AttendanceStatus.LateMinutes = IntNull(dr["LateMinutes"]);
                    objAttendance.AttendanceStatus.EarlyMinutes = IntNull(dr["EarlyMinutes"]);
                    objAttendance.AttendanceStatus.WorkingMinutes = IntNull(dr["WorkingMinutes"]);
                    objAttendance.AttendanceStatus.TotalMinutes = IntNull(dr["TotalMinutes"]);
                    objAttendance.AttendanceStatus.OverTimeMinutes = IntNull(dr["OverTimeMinutes"]);
                    objAttendance.AttendanceStatus.Reason = StringNull(dr["Reason"]);
                    objAttendance.AttendanceStatus.IsApproved = BooleanNull(dr["IsApproved"]);
                    objAttendance.AttendanceStatus.Remarks = StringNull(dr["Remarks"]);
                    objAttendance.AttendanceStatus.BreakType = StringNull(dr["BreakType"]);
                    _attendanceList.Add(objAttendance);
                }
            }
            return _attendanceList;
        }

        public List<Attendance> GetDailyAttendanceUpdateByDateUserWithDetail(string StartDate, string EndDate, Int32? UID, Int32? BranchId)
        {
            List<Attendance> _attendanceList = new List<Attendance>();
            DataSet dsAttendance = _iAttendanceRepository.GetAttendanceByDateAndUser(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UID, BranchId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                Attendance objAttendance = null;
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    objAttendance = new Attendance();
                    objAttendance.Date = Convert.ToDateTime(dr["Date"]);

                    objAttendance.User = new User();
                    objAttendance.User.Department = new Department();
                    objAttendance.User.Shift = new Shift();
                    objAttendance.User.SalaryType = new SalaryType();
                    objAttendance.User.Branch = new Branch();

                    objAttendance.User.Id = IntNull(dr["UserId"]);
                    objAttendance.User.FirstName = StringNull(dr["FirstName"]);
                    objAttendance.User.UserTypeId = IntNull(dr["UserTypeID"]);
                    objAttendance.User.Designation = StringNull(dr["Designation"]);
                    objAttendance.User.AccountNumber = StringNull(dr["AccountNumber"]);
                    objAttendance.User.ImagePath = StringNull(dr["ImagePath"]);

                    objAttendance.User.SalaryType.Id = IntNull(dr["SalaryTypeId"]);
                    objAttendance.User.SalaryType.Name = StringNull(dr["SalaryTypeName"]);

                    objAttendance.User.Department.Id = IntNull(dr["DepartmentId"]);
                    objAttendance.User.Department.Name = StringNull(dr["DepartmentName"]);

                    objAttendance.User.Shift.Id = IntNull(dr["ShiftId"]);
                    objAttendance.User.Shift.Name = StringNull(dr["ShiftName"]);

                    objAttendance.User.Branch.Id = IntNull(dr["BranchId"]);
                    objAttendance.User.Branch.Name = StringNull(dr["BranchName"]);

                    objAttendance.AttendanceDetailTimeIn = new AttendanceDetail();
                    objAttendance.AttendanceDetailTimeIn.Id = IntNull(dr["XADTIAttendanceDetailId"]);
                    objAttendance.AttendanceDetailTimeIn.AttendanceId = IntNull(dr["XADTIAttendanceId"]);
                    objAttendance.AttendanceDetailTimeIn.StartDate = DateNull(dr["XADTIStartDate"]);
                    objAttendance.AttendanceDetailTimeIn.EndDate = DateNull(dr["XADTIEndDate"]);

                    objAttendance.AttendanceDetailTimeOut = new AttendanceDetail();
                    objAttendance.AttendanceDetailTimeOut.Id = IntNull(dr["XADTOAttendanceDetailId"]);
                    objAttendance.AttendanceDetailTimeOut.AttendanceId = IntNull(dr["XADTOAttendanceId"]);
                    objAttendance.AttendanceDetailTimeOut.StartDate = DateNull(dr["XADTOStartDate"]);
                    objAttendance.AttendanceDetailTimeOut.EndDate = DateNull(dr["XADTOEndDate"]);
                    if (objAttendance.AttendanceDetailTimeOut != null && objAttendance.AttendanceDetailTimeOut.EndDate != null)
                    {
                        TimeSpan diff = (TimeSpan)(objAttendance.AttendanceDetailTimeOut.EndDate - objAttendance.AttendanceDetailTimeIn.StartDate);
                        objAttendance.WorkingHours = diff.ToString(@"hh\:mm");
                    }
                    else
                        objAttendance.WorkingHours = "00:00";

                    objAttendance.AttendanceStatus = new AttendanceStatus();
                    objAttendance.AttendanceStatus.Id = IntNull(dr["AttendanceStatusId"]);
                    objAttendance.AttendanceStatus.IsShiftOffDay = BooleanNull(dr["IsShiftOffDay"]);
                    objAttendance.AttendanceStatus.IsHoliday = BooleanNull(dr["IsHoliday"]);
                    objAttendance.AttendanceStatus.IsLeaveDay = BooleanNull(dr["IsLeaveDay"]);
                    objAttendance.AttendanceStatus.IsQuarterDay = BooleanNull(dr["IsQuarterDay"]);
                    objAttendance.AttendanceStatus.IsHalfDay = BooleanNull(dr["IsHalfDay"]);
                    objAttendance.AttendanceStatus.IsFullDay = BooleanNull(dr["IsFullDay"]);
                    objAttendance.AttendanceStatus.IsLate = BooleanNull(dr["IsLate"]);
                    objAttendance.AttendanceStatus.IsEarly = BooleanNull(dr["IsEarly"]);
                    objAttendance.AttendanceStatus.LateMinutes = IntNull(dr["LateMinutes"]);
                    objAttendance.AttendanceStatus.EarlyMinutes = IntNull(dr["EarlyMinutes"]);
                    objAttendance.AttendanceStatus.WorkingMinutes = IntNull(dr["WorkingMinutes"]);
                    objAttendance.AttendanceStatus.TotalMinutes = IntNull(dr["TotalMinutes"]);
                    objAttendance.AttendanceStatus.OverTimeMinutes = IntNull(dr["OverTimeMinutes"]);
                    objAttendance.AttendanceStatus.Reason = StringNull(dr["Reason"]);
                    objAttendance.AttendanceStatus.IsApproved = BooleanNull(dr["IsApproved"]);
                    objAttendance.AttendanceStatus.Remarks = StringNull(dr["Remarks"]);
                    objAttendance.AttendanceStatus.BreakType = StringNull(dr["BreakType"]);
                    _attendanceList.Add(objAttendance);
                }
            }
            return _attendanceList;
        }
        public List<VMPayslipAbsentInformation> GetAbsentReportEmployee(DateTime? StartDate, DateTime? EndDate, Int32? UserId, Int32? DepartmentId)
        {
            List<VMPayslipAbsentInformation> payslipAbsentSummary = new List<VMPayslipAbsentInformation>();
            DataSet dsAttendance = _iAttendanceRepository.GetAbsentReportByDateUserAndDepartment(StartDate, EndDate, UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    VMPayslipAbsentInformation objAbsent = new VMPayslipAbsentInformation();
                    objAbsent.AttendanceId = IntNull(dr["AttendanceId"]);
                    objAbsent.UserId = IntNull(dr["UserId"]);
                    if (DateNull(dr["AttendanceDate"]) != null)
                    {
                        objAbsent.Date = DateNull(dr["AttendanceDate"]).Value;
                    }
                    else
                    {
                        objAbsent.Date = DateTime.MinValue;
                    }
                    if (DateNull(dr["LeaveAttendanceDate"]) != null)
                    {
                        objAbsent.LeaveAttendanceDate = DateNull(dr["LeaveAttendanceDate"]).Value;
                    }
                    else
                    {
                        objAbsent.LeaveAttendanceDate = DateTime.MinValue;
                    }
                    payslipAbsentSummary.Add(objAbsent);
                }
            }
            return payslipAbsentSummary;
        }
        public List<VMAbsentReport> GetAbsentReport(DateTime? StartDate, DateTime? EndDate, Int32? UserId, Int32? DepartmentId)
        {
            List<VMAbsentReport> attendanceSummary = new List<VMAbsentReport>();
            DataSet dsAttendance = _iAttendanceRepository.GetAbsentReportByDateUserAndDepartment(StartDate, EndDate, UserId,DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    VMAbsentReport objAttendance = new VMAbsentReport();
                    objAttendance.AttendanceId = Convert.ToInt32(dr["AttendanceId"]);
                    objAttendance.UserId = Convert.ToInt32(dr["UserId"]);
                    objAttendance.DepartmentId = Convert.ToInt32(dr["DepartmentId"]);
                    objAttendance.AttendanceDate = Convert.ToDateTime(dr["AttendanceDate"]);
                    objAttendance.EmployeeFName = dr["FirstName"].ToString();
                    objAttendance.EmployeeMName = dr["MiddleName"].ToString();
                    objAttendance.EmployeeLName = dr["LastName"].ToString();
                    objAttendance.DepartmentName = dr["DepartmentName"].ToString();
                  
                    attendanceSummary.Add(objAttendance);
                }
            }
            return attendanceSummary;
        }
        public List<VMDailyAttendanceUpdateStatus> GetDailyAttendanceUpdateSummary(DateTime StartDate, DateTime EndDate, Int32? UserId)
        {
            List<VMDailyAttendanceUpdateStatus> attendanceSummary = new List<VMDailyAttendanceUpdateStatus>();
            DataSet dsAttendance = _iAttendanceRepository.GetAttendanceByDateAndUser(StartDate,EndDate,UserId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                //dsAttendance.Tables[0] // Attendance and AttendanceStatus
                //dsAttendance.Tables[1] // AttendanceDetail  
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    VMDailyAttendanceUpdateStatus objAttendance = new VMDailyAttendanceUpdateStatus()
                    {
                        AttendanceId = Convert.ToInt32(dr["AttendanceId"]),
                        AttendanceDate = Convert.ToDateTime(dr["Date"]),
                        EmployeeFName = dr["FirstName"].ToString(),
                        EmployeeMName = dr["MiddleName"].ToString(),
                        EmployeeLName = dr["LastName"].ToString(),
                        Early =Convert.ToBoolean(dr["IsEarly"]),
                        Late= Convert.ToBoolean(dr["IsLate"]),
                        EarlyMinutes= Convert.ToInt32(dr["EarlyMinutes"]),
                        LateMinutes= Convert.ToInt32(dr["LateMinutes"]),
                        VMDailyAttendanceUpdateTimeList=new List<VMDailyAttendanceUpdateTime>()
                    };
                    foreach (DataRow drDetail in dsAttendance.Tables[0].Select("AttendanceId=" + objAttendance.AttendanceId.ToString()))
                    {
                        VMDailyAttendanceUpdateTime objAttendanceDetail = new VMDailyAttendanceUpdateTime();
                        objAttendanceDetail.AttendanceDetailId = IntNull(drDetail["AttendanceDetailId"]);
                        if (DateNull(drDetail["StartDate"]) != null)
                        { 
                            objAttendanceDetail.DateTimeIn = DateNull(drDetail["StartDate"]).Value;
                        }
                        else
                        { 
                            objAttendanceDetail.DateTimeIn = DateTime.MinValue;
                        }
                        if (DateNull(drDetail["EndDate"])!=null)
                        { 
                             objAttendanceDetail.DateTimeOut = DateNull(drDetail["EndDate"]).Value;
                        }
                        else { 
                            objAttendanceDetail.DateTimeOut = DateTime.MinValue;
                        }
                        objAttendanceDetail.IsUpdate = false;
                        objAttendance.VMDailyAttendanceUpdateTimeList.Add(objAttendanceDetail);
                    }
                    attendanceSummary.Add(objAttendance);
                }
            }
            return attendanceSummary;
        }
        //New GetDailyAttendanceReport
        public List<VMAttendanceSummary> GetDailyAttendanceReport(string StartDate, Int32? UserId, Int32? DepartmentId)
        {
            List<VMAttendanceSummary> attendanceSummary = new List<VMAttendanceSummary>();
            DataSet dsAttendance = _iAttendanceRepository.GetAttendanceByDateUserAndDepartment(Convert.ToDateTime(StartDate), UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                VMAttendanceSummary objAttendance = null;
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    objAttendance = new VMAttendanceSummary();

                    objAttendance.AttendanceId = IntNull(dr["AttendanceId"]);
                    objAttendance.AttendanceStatudId = IntNull(dr["AttendanceStatudId"]);
                    objAttendance.UserId = IntNull(dr["UserId"]);
                    objAttendance.DepartId = IntNull(dr["DepartId"]);

                    objAttendance.UserName = Convert.ToString(dr["UserName"]);
                    objAttendance.MiddleName = Convert.ToString(dr["MiddleName"]);
                    objAttendance.LastName = Convert.ToString(dr["LastName"]);
                    objAttendance.DepartmentName = Convert.ToString(dr["DepartmentName"]);
                    objAttendance.EarlyMinutes = IntNull(dr["EarlyMinutes"]);
                    objAttendance.OverTimeMinutes = IntNull(dr["OverTimeMinutes"]);
                    objAttendance.LateMinutes = IntNull(dr["LateMinutes"]);
                    objAttendance.WorkingMinutes = IntNull(dr["WorkingMinutes"]);
                    objAttendance.TotalMinutes = IntNull(dr["TotalMinutes"]);

                    objAttendance.Date = Convert.ToDateTime(dr["Date"]); //AttendanceDate
                    objAttendance.ATStartDate = Convert.ToDateTime(dr["ATStartDate"]); //AT->Attendance Table
                    objAttendance.ATEndDate = Convert.ToDateTime(dr["ATEndDate"]);
                    //objAttendance.ADStartDate = Convert.ToDateTime(dr["ADStartDate"]); //AD->AttendanceDetail Table
                    //objAttendance.ADEndDate = Convert.ToDateTime(dr["ADEndDate"]);

                    objAttendance.IsShiftOffDay = Convert.ToBoolean(dr["IsShiftOffDay"]);
                    objAttendance.IsHalfDay = Convert.ToBoolean(dr["IsHalfDay"]);
                    objAttendance.IsQuarterDay = Convert.ToBoolean(dr["IsQuarterDay"]);
                    objAttendance.IsFullDay = Convert.ToBoolean(dr["IsFullDay"]);
                    objAttendance.IsEarly = Convert.ToBoolean(dr["IsEarly"]);
                    objAttendance.IsLate = Convert.ToBoolean(dr["IsLate"]);

                    //objAttendance.Designation = Convert.ToString(dr["Designation"]);
                    //objAttendance.NICNo = Convert.ToString(dr["NICNo"]);
                    attendanceSummary.Add(objAttendance);
                }
            }
            return attendanceSummary;
        }

        //New GetDailyDetailAttendanceReport
        public List<VMAttendanceSummary> GetDailyDetailAttendanceReport(string StartDate, Int32? UserId, Int32? DepartmentId)
        {
            List<VMAttendanceSummary> attendanceSummary = new List<VMAttendanceSummary>();
            DataSet dsAttendance = _iAttendanceRepository.GetAttendanceDetailByDateUserAndDepartment(Convert.ToDateTime(StartDate), UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                VMAttendanceSummary objAttendance = null;
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    objAttendance = new VMAttendanceSummary();

                    objAttendance.AttendanceId = IntNull(dr["AttendanceId"]);
                    objAttendance.AttendanceStatudId = IntNull(dr["AttendanceStatudId"]);
                    objAttendance.UserId = IntNull(dr["UserId"]);
                    objAttendance.DepartId = IntNull(dr["DepartId"]);

                    objAttendance.UserName = Convert.ToString(dr["UserName"]);
                    objAttendance.MiddleName = Convert.ToString(dr["MiddleName"]);
                    objAttendance.LastName = Convert.ToString(dr["LastName"]);
                    objAttendance.DepartmentName = Convert.ToString(dr["DepartmentName"]);
                    objAttendance.EarlyMinutes = IntNull(dr["EarlyMinutes"]);
                    objAttendance.OverTimeMinutes = IntNull(dr["OverTimeMinutes"]);
                    objAttendance.LateMinutes = IntNull(dr["LateMinutes"]);
                    objAttendance.WorkingMinutes = IntNull(dr["WorkingMinutes"]);
                    objAttendance.TotalMinutes = IntNull(dr["TotalMinutes"]);

                    objAttendance.Date = Convert.ToDateTime(dr["Date"]); //AttendanceDate
                    objAttendance.ATStartDate = Convert.ToDateTime(dr["ATStartDate"]); //AT->Attendance Table
                    objAttendance.ATEndDate = Convert.ToDateTime(dr["ATEndDate"]);

                    objAttendance.ADStartDate = Convert.ToDateTime(dr["ADStartDate"]); //AD->AttendanceDetail Table
                    objAttendance.ADEndDate = Convert.ToDateTime(dr["ADEndDate"]); //AD->AttendanceDetail Table

                    objAttendance.IsShiftOffDay = Convert.ToBoolean(dr["IsShiftOffDay"]);
                    objAttendance.IsHalfDay = Convert.ToBoolean(dr["IsHalfDay"]);
                    objAttendance.IsQuarterDay = Convert.ToBoolean(dr["IsQuarterDay"]);
                    objAttendance.IsFullDay = Convert.ToBoolean(dr["IsFullDay"]);
                    objAttendance.IsEarly = Convert.ToBoolean(dr["IsEarly"]);
                    objAttendance.IsLate = Convert.ToBoolean(dr["IsLate"]);

                    //objAttendance.Designation = Convert.ToString(dr["Designation"]);
                    //objAttendance.NICNo = Convert.ToString(dr["NICNo"]);
                    attendanceSummary.Add(objAttendance);
                }
            }
            return attendanceSummary;
        }



        
        //NewForMonthly
        //New GetMonthlyAttendanceReport
        public List<VMAttendanceSummary> GetMonthlyAttendanceReport(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId)
        {
         if(EndDate == "01-Jan-0001")
            {
                EndDate = StartDate;
            }
            List<VMAttendanceSummary> attendanceSummary = new List<VMAttendanceSummary>();
            DataSet dsAttendance = _iAttendanceRepository.GetMonthlyAttendanceByDateUserAndDepartment(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
           
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    VMAttendanceSummary objAttendance = new VMAttendanceSummary();

                    objAttendance.AttendanceId = IntNull(dr["AttendanceId"]);
                    objAttendance.AttendanceStatudId = IntNull(dr["AttendanceStatudId"]);
                    objAttendance.UserId = IntNull(dr["UserId"]);
                    objAttendance.DepartId = IntNull(dr["DepartId"]);

                    objAttendance.UserName = Convert.ToString(dr["UserName"]);
                    objAttendance.MiddleName = Convert.ToString(dr["MiddleName"]);
                    objAttendance.LastName = Convert.ToString(dr["LastName"]);
                    objAttendance.DepartmentName = Convert.ToString(dr["DepartmentName"]);
                    objAttendance.EarlyMinutes = IntNull(dr["EarlyMinutes"]);
                    objAttendance.OverTimeMinutes = IntNull(dr["OverTimeMinutes"]);
                    objAttendance.LateMinutes = IntNull(dr["LateMinutes"]);
                    objAttendance.WorkingMinutes = IntNull(dr["WorkingMinutes"]);
                    objAttendance.TotalMinutes = IntNull(dr["TotalMinutes"]);

                    objAttendance.Date = Convert.ToDateTime(dr["Date"]); //AttendanceDate
                    objAttendance.ATStartDate = Convert.ToDateTime(dr["ATStartDate"]); //AT->Attendance Table
                    objAttendance.ATEndDate = Convert.ToDateTime(dr["ATEndDate"]);
                    //objAttendance.ADStartDate = Convert.ToDateTime(dr["ADStartDate"]); //AD->AttendanceDetail Table
                    //objAttendance.ADEndDate = Convert.ToDateTime(dr["ADEndDate"]);

                    objAttendance.IsShiftOffDay = Convert.ToBoolean(dr["IsShiftOffDay"]);
                    objAttendance.IsHalfDay = Convert.ToBoolean(dr["IsHalfDay"]);
                    objAttendance.IsQuarterDay = Convert.ToBoolean(dr["IsQuarterDay"]);
                    objAttendance.IsFullDay = Convert.ToBoolean(dr["IsFullDay"]);
                    objAttendance.IsEarly = Convert.ToBoolean(dr["IsEarly"]);
                    objAttendance.IsLate = Convert.ToBoolean(dr["IsLate"]);

                    //objAttendance.Designation = Convert.ToString(dr["Designation"]);
                    //objAttendance.NICNo = Convert.ToString(dr["NICNo"]);
                    attendanceSummary.Add(objAttendance);
                }
            }
            return attendanceSummary;
        }

        //New GetMonthlyDetailAttendanceReport
        public List<VMAttendanceSummary> GetMonthlyDetailAttendanceReport(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId)
        {
            List<VMAttendanceSummary> attendanceSummary = new List<VMAttendanceSummary>();
            DataSet dsAttendance = _iAttendanceRepository.GetMonthlyAttendanceDetailByDateUserAndDepartment(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                //dsAttendance.Tables[0] // Attendance and AttendanceStatus
                //dsAttendance.Tables[1] // AttendanceDetail  

                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    VMAttendanceSummary objAttendance = new VMAttendanceSummary();

                    objAttendance.AttendanceId = IntNull(dr["AttendanceId"]);
                    objAttendance.AttendanceStatudId = IntNull(dr["AttendanceStatudId"]);
                    objAttendance.UserId = IntNull(dr["UserId"]);
                    objAttendance.DepartId = IntNull(dr["DepartId"]);

                    objAttendance.UserName = Convert.ToString(dr["UserName"]);
                    objAttendance.MiddleName = Convert.ToString(dr["MiddleName"]);
                    objAttendance.LastName = Convert.ToString(dr["LastName"]);
                    objAttendance.DepartmentName = Convert.ToString(dr["DepartmentName"]);
                    objAttendance.EarlyMinutes = IntNull(dr["EarlyMinutes"]);
                    objAttendance.OverTimeMinutes = IntNull(dr["OverTimeMinutes"]);
                    objAttendance.LateMinutes = IntNull(dr["LateMinutes"]);
                    objAttendance.WorkingMinutes = IntNull(dr["WorkingMinutes"]);
                    objAttendance.TotalMinutes = IntNull(dr["TotalMinutes"]);

                    objAttendance.Date = Convert.ToDateTime(dr["Date"]); //AttendanceDate
                    objAttendance.ATStartDate = Convert.ToDateTime(dr["ATStartDate"]); //AT->Attendance Table
                    objAttendance.ATEndDate = Convert.ToDateTime(dr["ATEndDate"]);

                    objAttendance.IsShiftOffDay = Convert.ToBoolean(dr["IsShiftOffDay"]);
                    objAttendance.IsHalfDay = Convert.ToBoolean(dr["IsHalfDay"]);
                    objAttendance.IsQuarterDay = Convert.ToBoolean(dr["IsQuarterDay"]);
                    objAttendance.IsFullDay = Convert.ToBoolean(dr["IsFullDay"]);
                    objAttendance.IsEarly = Convert.ToBoolean(dr["IsEarly"]);
                    objAttendance.IsLate = Convert.ToBoolean(dr["IsLate"]);

                  
                    attendanceSummary.Add(objAttendance);
                }
            };
            return attendanceSummary;
        }
        //New GetEmpDailyAttendance
        public List<EmpDailyDetailAttendance> GetEmpDailyAttendance(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId)
        {
            List<EmpDailyDetailAttendance> attendanceSummary = new List<EmpDailyDetailAttendance>();
            DataSet dsAttendance = _iAttendanceRepository.GetMonthlyAttendanceDetailByDateUserAndDepartment(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                //dsAttendance.Tables[0] // Attendance and AttendanceStatus
                //dsAttendance.Tables[1] // AttendanceDetail  
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    EmpDailyDetailAttendance objAttendance = new EmpDailyDetailAttendance()
                    {
                        AttendanceId = Convert.ToInt32(dr["AttendanceId"]),
                        EmpId = Convert.ToInt32(dr["UserId"]),
                        AttendanceDate = DateNull(dr["Date"]).Value,
                        EmpName = dr["FirstName"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        IsEarly = Convert.ToBoolean(dr["IsEarly"]),
                        IsLate = Convert.ToBoolean(dr["IsLate"]),
                        IsShiftOffDay = Convert.ToBoolean(dr["IsShiftOffDay"]),
                        IsFullDay = Convert.ToBoolean(dr["IsFullDay"]),
                        IsHalfDay = Convert.ToBoolean(dr["IsHalfDay"]),
                        IsQuarterDay = Convert.ToBoolean(dr["IsQuarterDay"]),
                        EarlyMin = Convert.ToInt32(dr["EarlyMinutes"]),
                        LateMin = Convert.ToInt32(dr["LateMinutes"]),
                        WorkingMin = Convert.ToInt32(dr["WorkingMinutes"]),
                        TotalMin = Convert.ToInt32(dr["TotalMinutes"]),
                        AtStartDate = DateNull(dr["AtStartDate"]).Value,
                        ATEndDate = DateNull(dr["ATEndDate"]).Value,
                        EmpDailyDetailAttendanceDurationList = new List<EmpDailyDetailAttendanceDuration>()
                    };
                    foreach (DataRow drDetail in dsAttendance.Tables[0].Select("AttendanceId=" + objAttendance.AttendanceId.ToString()))
                    {
                        EmpDailyDetailAttendanceDuration objAttendanceDetail = new EmpDailyDetailAttendanceDuration()
                        {
                            AttendanceDetailId = IntNull(drDetail["AttendanceDetailId"]),
                            ADStartDate = DateNull(drDetail["ADStartDate"]).Value,
                            ADEndDate = DateNull(drDetail["ADEndDate"]).Value
                        };
                        objAttendance.EmpDailyDetailAttendanceDurationList.Add(objAttendanceDetail);
                    }
                    attendanceSummary.Add(objAttendance);
                }
            };
            return attendanceSummary;
        }
        //New GetEmpMonthlyAttendance
        public List<EmpMonthlyDetailAttendance> GetEmpMonthlyAttendance(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId)
        {
            List<EmpMonthlyDetailAttendance> attendanceSummary = new List<EmpMonthlyDetailAttendance>();
            DataSet dsAttendance = _iAttendanceRepository.GetMonthlyAttendanceDetailByDateUserAndDepartment(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                //dsAttendance.Tables[0] // Attendance and AttendanceStatus
                //dsAttendance.Tables[1] // AttendanceDetail  
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    EmpMonthlyDetailAttendance objAttendance = new EmpMonthlyDetailAttendance();
                    
                        objAttendance.AttendanceId = IntNull(dr["AttendanceId"]);
                        objAttendance.EmpId = IntNull(dr["UserId"]);
                        objAttendance.AttendanceDate = DateNull(dr["Date"]).Value;
                        objAttendance.EmpName = dr["FirstName"].ToString();
                        objAttendance.DepartmentName = dr["DepartmentName"].ToString();
                        objAttendance.IsEarly = BooleanNull(dr["IsEarly"]);
                        objAttendance.IsLate = BooleanNull(dr["IsLate"]);
                        objAttendance.IsShiftOffDay = BooleanNull(dr["IsShiftOffDay"]);
                        objAttendance.IsFullDay = BooleanNull(dr["IsFullDay"]);
                        objAttendance.IsHalfDay = BooleanNull(dr["IsHalfDay"]);
                        objAttendance.IsQuarterDay = BooleanNull(dr["IsQuarterDay"]);
                        objAttendance.EarlyMin = IntNull(dr["EarlyMinutes"]);
                        objAttendance.LateMin = IntNull(dr["LateMinutes"]);
                        objAttendance.WorkingMin = IntNull(dr["WorkingMinutes"]);
                        objAttendance.TotalMin = IntNull(dr["TotalMinutes"]);
                    if (DateNull(dr["AtStartDate"]) != null)
                    {
                        objAttendance.AtStartDate = DateNull(dr["AtStartDate"]).Value;
                    }
                    else
                    {
                        objAttendance.AtStartDate = DateTime.MinValue;
                    }
                    if (DateNull(dr["ATEndDate"]) != null)
                    {
                        objAttendance.ATEndDate = DateNull(dr["ATEndDate"]).Value;
                    }
                    else
                    {
                        objAttendance.ATEndDate = DateTime.MinValue;
                    }

                    objAttendance.EmpMonthlyDetailAttendanceDurationList = new List<EmpMonthlyDetailAttendanceDuration>();
                
                foreach (DataRow drDetail in dsAttendance.Tables[0].Select("AttendanceId=" + objAttendance.AttendanceId.ToString()))
                    {
                        EmpMonthlyDetailAttendanceDuration objAttendanceDetail = new EmpMonthlyDetailAttendanceDuration();

                        objAttendanceDetail.AttendanceDetailId = IntNull(drDetail["AttendanceDetailId"]);
                        if (DateNull(drDetail["ADStartDate"]) != null)
                        {
                            objAttendanceDetail.ADStartDate = DateNull(drDetail["ADStartDate"]).Value;
                        }
                        else
                        {
                            objAttendanceDetail.ADStartDate = DateTime.MinValue;
                        }
                        if (DateNull(drDetail["ADEndDate"]) != null)
                        {
                            objAttendanceDetail.ADEndDate = DateNull(drDetail["ADEndDate"]).Value;
                        }
                        else
                        {
                            objAttendanceDetail.ADEndDate = DateTime.MinValue;
                        }
                      
                        objAttendance.EmpMonthlyDetailAttendanceDurationList.Add(objAttendanceDetail);
                    }
                    attendanceSummary.Add(objAttendance);
                }
            };
            return attendanceSummary;
        }
        //NewForMonthly
        //New GetMonthlyAttendanceReportPractice
        public List<PracticeVMReport> GetMonthlyAttendanceReportPractice(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId)
        {
            if (EndDate == "01-Jan-0001")
            {
                EndDate = StartDate;
            }
            List<PracticeVMReport> attendanceSummary = new List<PracticeVMReport>();
            DataSet dsAttendance = _iAttendanceRepository.GetMonthlyAttendanceByDateUserAndDepartment(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {

                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    PracticeVMReport objAttendance = new PracticeVMReport();
                    objAttendance.AttendanceId = IntNull(dr["AttendanceId"]);
                    objAttendance.AttendanceStatudId = IntNull(dr["AttendanceStatudId"]);
                    objAttendance.UserId = IntNull(dr["UserId"]);
                    objAttendance.DepartmentId = IntNull(dr["DepartmentId"]);
                    objAttendance.FirstName = Convert.ToString(dr["FirstName"]);
                    objAttendance.DepartmentName = Convert.ToString(dr["DepartmentName"]);
                    objAttendance.EarlyMinutes = IntNull(dr["EarlyMinutes"]);
                    objAttendance.OverTimeMinutes = IntNull(dr["OverTimeMinutes"]);
                    objAttendance.LateMinutes = IntNull(dr["LateMinutes"]);
                    objAttendance.WorkingMinutes = IntNull(dr["WorkingMinutes"]);
                    objAttendance.TotalMinutes = IntNull(dr["TotalMinutes"]);
                    objAttendance.IsShiftOffDay = BooleanNull(dr["IsShiftOffDay"]);
                    objAttendance.IsHalfDay = BooleanNull(dr["IsHalfDay"]);
                    objAttendance.IsQuarterDay = BooleanNull(dr["IsQuarterDay"]);
                    objAttendance.IsFullDay = BooleanNull(dr["IsFullDay"]);
                    objAttendance.IsEarly = BooleanNull(dr["IsEarly"]);
                    objAttendance.IsLate = BooleanNull(dr["IsLate"]);
                    objAttendance.Date = DateNull(dr["Date"]).Value; //AttendanceDate
                  /*  objAttendance.ATStartDate = DateNull(dr["ATStartDate"]).Value;*/ //AT->Attendance Table
                    if (DateNull(dr["ATStartDate"]).HasValue)
                    {
                        objAttendance.ATStartDate = DateNull(dr["ATStartDate"]).Value;
                    } //AT->Attendance Table }

                    else
                    {
                        objAttendance.ATStartDate = DateTime.MinValue;
                    }
                    if (DateNull(dr["ATEndDate"]).HasValue)
                    {
                        objAttendance.ATEndDate = DateNull(dr["ATEndDate"]).Value;
                    } //AT->Attendance Table }

                    else
                    {
                        objAttendance.ATEndDate = DateTime.MinValue;
                    }

                        objAttendance.Designation = Convert.ToString(dr["Designation"]);
                    objAttendance.NICNo = Convert.ToString(dr["NICNo"]);
                    attendanceSummary.Add(objAttendance);
                }
            }
            return attendanceSummary;
        }

        //New GetMonthlyDetailAttendanceReportPractice
        public List<PracticeVMReport> GetMonthlyDetailAttendanceReportPractice(string StartDate, string EndDate, Int32? UserId, Int32? DepartmentId)
        {
            List<PracticeVMReport> attendanceSummary = new List<PracticeVMReport>();
            DataSet dsAttendance = _iAttendanceRepository.GetMonthlyAttendanceDetailByDateUserAndDepartment(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UserId, DepartmentId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                //dsAttendance.Tables[0] // Attendance and AttendanceStatus
                //dsAttendance.Tables[1] // AttendanceDetail  
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    PracticeVMReport objAttendance = new PracticeVMReport()
                    {
                        AttendanceId = Convert.ToInt32(dr["AttendanceId"]),
                        Date = Convert.ToDateTime(dr["Date"]),
                        FirstName = dr["FirstName"].ToString(),
                        IsEarly = Convert.ToBoolean(dr["IsEarly"]),
                        IsFullDay = Convert.ToBoolean(dr["IsFullDay"]),
                        IsHalfDay = Convert.ToBoolean(dr["IsHalfDay"]),
                        IsQuarterDay = Convert.ToBoolean(dr["IsQuarterDay"]),
                        IsLate = Convert.ToBoolean(dr["IsLate"]),
                        EarlyMinutes = Convert.ToInt32(dr["EarlyMinutes"]),
                        LateMinutes = Convert.ToInt32(dr["LateMinutes"]),
                        WorkingMinutes = Convert.ToInt32(dr["WorkingMinutes"]),
                        TotalMinutes = Convert.ToInt32(dr["TotalMinutes"]),
                        PracticeVMDetailReportList = new List<PracticeVMDetailReport>()
                    };
                    foreach (DataRow drDetail in dsAttendance.Tables[0].Select("AttendanceId=" + objAttendance.AttendanceId.ToString()))
                    {
                        PracticeVMDetailReport objAttendanceDetail = new PracticeVMDetailReport();
                        objAttendanceDetail.AttendanceDetailId = IntNull(drDetail["AttendanceDetailId"]);
                        objAttendanceDetail.ADStartDate = DateNull(drDetail["ADStartDate"]).Value;
                        if (DateNull(drDetail["ADEndDate"]).HasValue)
                            objAttendanceDetail.ADEndDate = DateNull(drDetail["ADEndDate"]).Value;
                        else
                            objAttendanceDetail.ADEndDate = DateTime.MinValue;
                        objAttendance.PracticeVMDetailReportList.Add(objAttendanceDetail);
                    }
                    attendanceSummary.Add(objAttendance);
                }
            };
            return attendanceSummary;
        }
        public List<VMAttendanceSummary> GetDailyAttendanceSummary(string StartDate, string EndDate, Int32? UserId,Int32? BranchId, string DepartmentName, Int32? ShiftID, Int32? SalaryTypeId)
        {
            List<VMAttendanceSummary> attendanceSummary = new List<VMAttendanceSummary>();
            DataSet dsAttendance = _iAttendanceRepository.GetDailyAttendanceByDateTypeShiftBranch(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate),BranchId,DepartmentName,ShiftID,SalaryTypeId);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                VMAttendanceSummary objAttendance = null;
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    objAttendance = new VMAttendanceSummary();
                    objAttendance.DepartmentId = IntNull(dr["DepartmentId"]);
                    objAttendance.DepartmentName = StringNull(dr["DepartmentName"]);
                    objAttendance.BranchId = IntNull(dr["BranchID"]);
                    objAttendance.BranchName = StringNull(dr["BranchName"]);

                    objAttendance.Total=IntNull(dr["Total"]);
                    objAttendance.OffDay = IntNull(dr["OffDay"]);
                    objAttendance.Present = IntNull(dr["Present"]) - objAttendance.OffDay;
                    objAttendance.Late = IntNull(dr["Late"]);
                    objAttendance.Early = IntNull(dr["Early"]);
                    objAttendance.OverTime = IntNull(dr["OverTime"]);
                    objAttendance.FullDay = IntNull(dr["FullDay"]);
                    attendanceSummary.Add(objAttendance);
                }
            }
            return attendanceSummary;
        }

        //public List<User> GetMonthlyAttendanceSummaryNew(DateTime dtStart, Int32? UserID, Int32? DepartmentId, string UserName,  string DepartmentName)
        //{
        //    List<User> monthlySummary = new List<User>();
        //    DataSet dsUser = _iAttendanceRepository.GetMonthlyAttendanceSummaryNew(dtStart, UserID, DepartmentId, UserName, DepartmentName);
        //    if (dsUser != null && dsUser.Tables.Count > 0 && dsUser.Tables[0] != null)
        //    {
        //        User objUser = null;
        //        foreach (DataRow dr in dsUser.Tables[0].Rows)
        //        {
        //            objUser = new User();
        //            objUser.Id = IntNull(dr["UserId"]);
        //            objUser.FirstName = StringNull(dr["UserName"]);
        //            objUser.ImagePath = StringNull(dr["ImagePath"]); ;
        //            objUser.Designation = StringNull(dr["Designation"]);
        //            objUser.Department = new Department() { Id = IntNull(dr["DepartmentId"]), Name = StringNull(dr["DepartmentName"]) };
        //            objUser.SalaryType = new SalaryType() { Id = IntNull(dr["SalaryTypeId"]), Name = StringNull(dr["SalaryType"]) };
        //            objUser.Branch = new Branch() { Id = IntNull(dr["BranchID"]), Name = StringNull(dr["BranchName"]) };
        //            objUser.Shift = new Shift() { Id = IntNull(dr["ShiftId"]), Name = StringNull(dr["ShiftName"]) };

        //            objUser.TotalCount = IntNull(dr["TOTAL"]);
        //            objUser.OffDayCount = IntNull(dr["OffDay"]);
        //            objUser.PresentCount = IntNull(dr["Present"]);
        //            objUser.AbsentCount = objUser.TotalCount - objUser.PresentCount;
        //            objUser.PresentCount = objUser.PresentCount - objUser.OffDayCount;
        //            objUser.LateCount = IntNull(dr["Late"]);
        //            objUser.EarlyCount = IntNull(dr["Early"]);
        //            objUser.OverTimeCount = IntNull(dr["OverTime"]);
        //            objUser.FullDayCount = IntNull(dr["FullDay"]);

        //            monthlySummary.Add(objUser);
        //        }
        //    }
        //    return monthlySummary;
        //}

        public List<AttendanceAndAttendanceStatusViewList> GetAttendanceAndAttendanceStatusByUserIdAndDateRangeList(DateTime StartDate, DateTime EndDate, Int32? UID)
        {
           List<AttendanceAndAttendanceStatusViewList> attendanceSummary = new List<AttendanceAndAttendanceStatusViewList>();
            DataSet dsAttendance = _iAttendanceRepository.GetAttendanceAndAttendanceStatusByUserIdAndDateRange(Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate), UID);
            if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
            {
                //dsAttendance.Tables[0] // Attendance and AttendanceStatus
                //dsAttendance.Tables[1] // AttendanceDetail  
                foreach (DataRow dr in dsAttendance.Tables[0].Rows)
                {
                    AttendanceAndAttendanceStatusViewList objAttendance = new AttendanceAndAttendanceStatusViewList()
                    {
                        AttendanceID = Convert.ToInt32(dr["AttendanceId"]),
                        UserID = Convert.ToInt32(dr["UserId"]),
                        AttendanceDate = Convert.ToDateTime(dr["AttendanceDate"]),
                        IsShiftOffDay = BooleanNull(dr["IsShiftOffDay"]),
                        IsEarly = BooleanNull(dr["IsEarly"]),
                        IsQuarterDay = BooleanNull(dr["IsQuarterDay"]),
                        IsLate = BooleanNull(dr["IsLate"]),
                        IsFullDay = BooleanNull(dr["IsFullDay"]),
                        IsHalfDay = BooleanNull(dr["IsHalfDay"]),
                        IsLeaveDay = BooleanNull(dr["IsLeaveDay"]),
                        IsHoliday = BooleanNull(dr["IsHoliday"]),
                    };
                    attendanceSummary.Add(objAttendance);
                }
            };
            return attendanceSummary;
        }
    public List<User> GetMonthlyAttendanceSummary(DateTime dtStart, DateTime dtEnd, Int32? UserID, string UserName, Int32? BranchID, string DepartmentName, Int32? ShiftID, Int32? SalaryTypeId)
        {
            List<User> monthlySummary = new List<User>();
            DataSet dsUser = _iAttendanceRepository.GetMonthlyAttendanceSummary(dtStart,dtEnd, UserID,UserName,BranchID, DepartmentName, ShiftID, SalaryTypeId);
            if (dsUser != null && dsUser.Tables.Count > 0 && dsUser.Tables[0] != null)
            {
                User objUser = null;
                foreach (DataRow dr in dsUser.Tables[0].Rows)
                {
                    objUser = new User();
                    objUser.Id = IntNull(dr["UserId"]);
                    objUser.FirstName=StringNull(dr["UserName"]);
                    objUser.ImagePath=StringNull(dr["ImagePath"]);;
                    objUser.Designation=StringNull(dr["Designation"]);
                    objUser.Department = new Department() { Id = IntNull(dr["DepartmentId"]), Name = StringNull(dr["DepartmentName"]) };
                    objUser.SalaryType = new SalaryType() { Id = IntNull(dr["SalaryTypeId"]), Name = StringNull(dr["SalaryType"]) };
                    objUser.Branch = new Branch() { Id = IntNull(dr["BranchID"]), Name = StringNull(dr["BranchName"]) };
                    objUser.Shift = new Shift() { Id = IntNull(dr["ShiftId"]), Name = StringNull(dr["ShiftName"]) };

                    objUser.TotalCount = IntNull(dr["TOTAL"]);
                    objUser.OffDayCount = IntNull(dr["OffDay"]);
                    objUser.PresentCount = IntNull(dr["Present"]) ;
                    objUser.AbsentCount = objUser.TotalCount - objUser.PresentCount;
                    objUser.PresentCount = objUser.PresentCount-objUser.OffDayCount;
                    objUser.LateCount = IntNull(dr["Late"]);
                    objUser.EarlyCount = IntNull(dr["Early"]);
                    objUser.OverTimeCount = IntNull(dr["OverTime"]);
                    objUser.FullDayCount = IntNull(dr["FullDay"]);
                    
                    monthlySummary.Add(objUser);
                }
            }
            return monthlySummary;
        }

        private string StringNull(object val) { if (val == null || val == DBNull.Value) return ""; else return val.ToString(); }
        private DateTime? DateNull(object val) { if (val == null || val == DBNull.Value) return null; else return Convert.ToDateTime(val); }
        private Double DoubleNull(object val) { if (val == null || val == DBNull.Value) return 0.0; else return Convert.ToDouble(val); }
        private Int32 IntNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToInt32(val); }
        private Boolean BooleanNull(object val) { if (val == null || val == DBNull.Value) return false; else return Convert.ToBoolean(val); }
        public Attendance InsertAttendance(Attendance entity)
        {
            return _iAttendanceRepository.InsertAttendance(entity);
        }

        public List<Attendance> GetAttendanceByDate(DateTime dtAttendance, string SelectClause = null)
        {
            return _iAttendanceRepository.GetAttendanceByDate(dtAttendance, SelectClause);
        }
        public List<Attendance> GetAttendanceByDateRange(DateTime dtStart, DateTime dtEnd, string SelectClause = null)
        {
            return _iAttendanceRepository.GetAttendanceByDateRange(dtStart, dtEnd, SelectClause);
        }
        public List<Attendance> GetAttendanceByUserIdAndDateRange(DateTime dtStart, DateTime dtEnd, Int32? UserID,string SelectClause = null)
        {
            return _iAttendanceRepository.GetAttendanceByUserIdAndDateRange(dtStart, dtEnd, UserID,SelectClause);
        }

        //NewAttendanceStatus
        //public List<Attendance> GetAttendanceStatusByDateRange(DateTime dtStart, DateTime dtEnd, string SelectClause = null)
        //{
        //    return _iAttendanceRepository.GetAttendanceStatusByDateRange(dtStart, dtEnd, SelectClause);
        //}
        public List<Attendance> GetAttendanceByUserIDAndDate(int userId, DateTime dtAttendance, string SelectClause = null)
        {
            return _iAttendanceRepository.GetAttendanceByUserIDAndDate(userId, dtAttendance, SelectClause);
        }

        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                Attendance attendance = _iAttendanceRepository.GetAttendance(id);
                if (attendance != null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(attendance);
                    tranfer.Data = output;

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }
        public DataTransfer<List<GetOutput>> GetAll()
        {
            DataTransfer<List<GetOutput>> tranfer = new DataTransfer<List<GetOutput>>();
            List<Attendance> attendancelist = _iAttendanceRepository.GetAllAttendance();
            if (attendancelist != null && attendancelist.Count > 0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(attendancelist);
                tranfer.Data = outputlist;

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: No record found.";
            }
            return tranfer;
        }
        public DataTransfer<PostOutput> Insert(PostInput Input)
        {
            DataTransfer<PostOutput> transer = new DataTransfer<PostOutput>();
            IList<string> errors = Validator.Validate(Input);
            if (errors.Count == 0)
            {
                Attendance attendance = new Attendance();
                PostOutput output = new PostOutput();
                attendance.CopyFrom(Input);
                attendance = _iAttendanceRepository.InsertAttendance(attendance);
                output.CopyFrom(attendance);
                transer.IsSuccess = true;
                transer.Data = output;
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

        public DataTransfer<PutOutput> Update(PutInput Input)
        {
            DataTransfer<PutOutput> transer = new DataTransfer<PutOutput>();
            IList<string> errors = Validator.Validate(Input);
            if (errors.Count == 0)
            {
                Attendance attendanceinput = new Attendance();
                Attendance attendanceoutput = new Attendance();
                PutOutput output = new PutOutput();
                attendanceinput.CopyFrom(Input);
                Attendance attendance = _iAttendanceRepository.GetAttendance(attendanceinput.Id);
                if (attendance != null)
                {
                    attendanceoutput = _iAttendanceRepository.UpdateAttendance(attendanceinput);
                    if (attendanceoutput != null)
                    {
                        output.CopyFrom(attendanceoutput);
                        transer.IsSuccess = true;
                        transer.Data = output;
                    }
                    else
                    {
                        transer.IsSuccess = false;
                        transer.Errors = new string[1];
                        transer.Errors[0] = "Error: Could not update.";
                    }
                }
                else
                {
                    transer.IsSuccess = false;
                    transer.Errors = new string[1];
                    transer.Errors[0] = "Error: Record not found.";
                }
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }



































































































































































































        public DataTransfer<string> Delete(string _id)
        {
            DataTransfer<string> tranfer = new DataTransfer<string>();
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                bool IsDeleted = _iAttendanceRepository.DeleteAttendance(id);
                if (IsDeleted)
                {
                    tranfer.IsSuccess = true;
                    tranfer.Data = IsDeleted.ToString().ToLower();

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }

        public List<Attendance> GetAttendanceByDate(DateTime dtAttendance, int BranchID, string SelectClause = null)
        {
            return _iAttendanceRepository.GetAttendanceByDate(dtAttendance, BranchID, SelectClause);
        }
        public List<Attendance> GetAttendanceByDateRange(DateTime dtStart, DateTime dtEnd, int BranchID, string SelectClause = null)
        {
            return _iAttendanceRepository.GetAttendanceByDateRange(dtStart, dtEnd, BranchID, SelectClause);
        }

        //New AttendanceStatus
        //public List<Attendance> GetAttendanceStatusByDateRange(DateTime dtStart, DateTime dtEnd,string SelectClause = null)
        //{
        //    return _iAttendanceRepository.GetAttendanceStatusByDateRange(dtStart, dtEnd, SelectClause);
        //}

        public void InsertPreAttendance()
        {
             _iAttendanceRepository.InsertPreAttendance();
        }
        public System.Data.DataSet GetAttendanceByDateAndUser(DateTime dtStart, DateTime dtEnd, Int32? UserID)
        {
            return _iAttendanceRepository.GetAttendanceByDateAndUser(dtStart, dtEnd, UserID);
        }
        public System.Data.DataSet GetAttendanceByDateAndUser(DateTime dtStart, DateTime dtEnd, Int32? UserID,Int32? BranchId)
        {
            return _iAttendanceRepository.GetAttendanceByDateAndUser(dtStart, dtEnd, UserID, BranchId);
        }
        public System.Data.DataSet GetAttendanceByDateAndUserUpdate(DateTime dtStart, DateTime dtEnd, Int32? UserID)
        {
            return _iAttendanceRepository.GetAttendanceByDateAndUser(dtStart, dtEnd, UserID);
        }

        //New for GetAbsentReportByDateUserAndDepartment
        public System.Data.DataSet GetAbsentReportByDateUserAndDepartment(DateTime? dtStart, DateTime? dtEnd, Int32? UserID,Int32?DepartmentId)
        {
            return _iAttendanceRepository.GetAbsentReportByDateUserAndDepartment(dtStart, dtEnd, UserID,DepartmentId);
        }
    }


}
