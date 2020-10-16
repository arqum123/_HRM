using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HRM.Core.Entities;
using HRM.Core.Enum;
using HRM.Core.IService;
using HRM.Core.Model;

namespace HRM.Core.Helper
{
    public class AttendanceFromDevice
    {
        string errorFilePath = "";
        string dateFormat = "dd-MMM-yyyy";
        public void ProcessAttendance(string logFilePath, string errorFilePath, List<String> lines)
        {
            #region Init
            this.errorFilePath = errorFilePath;
            DateTime dtAttendance = new DateTime();
            List<Attendance> _attendanceList = null;
            Attendance _attendance = null;
            List<AttendanceDetail> _attendanceDetailList = null;
            AttendanceDetail _attendanceDetail = null;
            List<AttendanceStatus> _attendanceStatusList = null;
            AttendanceStatus _attendanceStatus = null;
            VMAttendanceData _attendanceData = null;
            VMAttendanceData _attendanceTimeInData = null;
            VMAttendanceData _attendanceTimeOutData = null;
            List<VMAttendanceData> attendanceDataList = new List<VMAttendanceData>();
            List<VMAttendanceData> attendanceDataListTemp = null;

            List<UserShift> _userShiftList = null;
            List<AttendancePolicy> _attendancePolicyList = null;
            List<AttendancePolicy> _attendancePolicyListTemp = null;
            UserShift _userShift = null;
            Shift _shift = null;
            AttendancePolicy _attendancePolicy = null;

            IAttendanceService objAttendanceService = null;
            IAttendanceDetailService objAttendanceDetailService = null;
            IAttendanceStatusService objAttendanceStatusService = null;
            IUserShiftService objUserShiftService = null;
            IShiftService objShiftService = null;
            IAttendancePolicyService objAttendancePolicyService = null;

            try
            {
                objAttendanceService = IoC.Resolve<HRM.Core.IService.IAttendanceService>("AttendanceService");
                objAttendanceDetailService = IoC.Resolve<IAttendanceDetailService>("AttendanceDetailService");
                objAttendanceStatusService = IoC.Resolve<IAttendanceStatusService>("AttendanceStatusService");
                objUserShiftService = IoC.Resolve<IUserShiftService>("UserShiftService");
                objShiftService = IoC.Resolve<IShiftService>("ShiftService");
                objAttendancePolicyService = IoC.Resolve<IAttendancePolicyService>("AttendancePolicyService");
            }
            catch (Exception ex)
            {
                WriteError("Error on Process Init. Error Message: " + ex.Message);
                return;
            }
            #endregion Init
            #region ReadData
            foreach (string line in lines)
            {
                try
                {
                    string[] parts = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    _attendanceData = new VMAttendanceData()
                    {
                        UserId = Convert.ToInt32(parts[(int)Cols.EnNo]),
                        BreakType = parts[(int)Cols.In_Out],
                        TimeStamp = Convert.ToDateTime(parts[(int)Cols.DateTime])
                    };
                    attendanceDataList.Add(_attendanceData);
                }
                catch (Exception ex)
                {
                    WriteError("Error in Process ReadData. Error Message: " + ex.Message + " - Data: " + (line != null ? line : ""));
                }
            }
            #endregion ReadData
            #region Attendance Data List Processing
            attendanceDataList = attendanceDataList.Where(x => x.TimeStamp > DateTime.Now.AddMonths(-12)).OrderBy(x => x.UserId).ThenBy(x => x.TimeStamp).ToList();
            //dtAttendance = attendanceDataList.Where(x => x.TimeStamp > DateTime.Now.AddMonths(-1)).OrderBy(x => x.TimeStamp).Select(x => x.TimeStamp.Date).FirstOrDefault();
            #endregion

            //IConfigurationService objConfigurationService = IoC.Resolve<IConfigurationService>("ConfigurationService");
            //List<Configuration> ConfigurationList = objConfigurationService.GetAllConfiguration();
            SystemConfiguration objSystemConfiguration = new SystemConfiguration();
            bool AlternateTimeout = objSystemConfiguration.GetConfigurationBoolValue(HRM.Core.Enum.Constant.AlternateAttendance);

            if (AlternateTimeout)
            {
                #region Alternate Time In/Out
                foreach (VMAttendanceData _vmAttendance in attendanceDataList)
                {
                    try
                    {

                        if (_vmAttendance.BreakType.Contains("Duty") || _vmAttendance.BreakType == "0" || _vmAttendance.BreakType == "1")
                        {
                            //Console.WriteLine(String.Format("{0} {1}",_vmAttendance.UserId.ToString(),_vmAttendance.TimeStamp.ToString()));
                            #region Attendance
                            //_attendanceList = objAttendanceService.GetAttendanceByUserIDAndDate(userId, dtAttendance);
                            _attendanceList = objAttendanceService.GetAttendanceByUserId(_vmAttendance.UserId);

                            #region Date Selection
                            _userShiftList = objUserShiftService.GetUserShiftByUserId(_vmAttendance.UserId);
                            if (_userShiftList != null && _userShiftList.Count > 0)
                            {
                                _userShiftList = _userShiftList.Where(x => _vmAttendance.TimeStamp.Date >= x.EffectiveDate.Value && _vmAttendance.TimeStamp.Date <= (!x.RetiredDate.HasValue ? _vmAttendance.TimeStamp.Date : x.RetiredDate.Value)).ToList();
                                if (_userShiftList != null && _userShiftList.Count > 0)
                                {
                                    _userShift = _userShiftList.FirstOrDefault();
                                    _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
                                    if (!_shift.StartHour.Contains(":"))
                                        _shift.StartHour += ":00";
                                    if (Convert.ToDateTime(_vmAttendance.TimeStamp.ToString(dateFormat) + " " + _shift.StartHour).Subtract(_vmAttendance.TimeStamp).Hours <= 2)
                                        dtAttendance = _vmAttendance.TimeStamp.Date;
                                    else
                                        dtAttendance = _vmAttendance.TimeStamp.AddDays(-1).Date;
                                }
                                else
                                    dtAttendance = _vmAttendance.TimeStamp.Date;//***ADD
                            }
                            else
                                dtAttendance = _vmAttendance.TimeStamp.Date;//***ADD
                            #endregion Date Selection

                            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\ServiceLog.txt", DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "- 7: " + dtAttendance.ToString() + Environment.NewLine);

                            //Insert Attendance
                            if (_attendanceList == null || _attendanceList.Count <= 0)
                            {
                                WriteError("No Attendance not marked for userId: " + _vmAttendance.UserId.ToString() + " Inserting Attendance");
                                //dtAttendance = _vmAttendance.TimeStamp.Date;//***REMOVE
                                _attendance = new Attendance() { UserId = _vmAttendance.UserId, Date = dtAttendance, IsActive = true, CreationDate = DateTime.Now, UserIp = "Service" };
                                _attendance = objAttendanceService.InsertAttendance(_attendance);
                            }
                            //Attendance Already marked, Get Attendance
                            else
                            {
                                //_attendance = _attendanceList.OrderBy(x => x.Date).Last();//***REMOVE
                                //dtAttendance = _attendance.Date.Value;//***REMOVE
                                _attendance = _attendanceList.Where(x => x.Date.Value == dtAttendance).FirstOrDefault(); //***ADD
                            }
                            #endregion
                            //Console.WriteLine("Attendance");
                            #region AttendanceDetail
                            //Attendance Detail List
                            //if(_attendance.UserId==18)
                            //{
                            //}
                            _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                            //Check previous entry is time in or out
                            if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                            {
                                _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
                                if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                                {
                                    if (_attendanceDetailList.Where(x => x.StartDate != null && x.EndDate == null).Any())
                                    {
                                        //Update Timeout
                                        _attendanceDetail = _attendanceDetailList.Where(x => x.StartDate != null && x.EndDate == null).OrderByDescending(x => x.StartDate).FirstOrDefault();
                                        _attendanceDetail.EndDate = _vmAttendance.TimeStamp;
                                        _attendanceDetail.UpdateDate = DateTime.Now;
                                        _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                                        _attendanceTimeOutData = _vmAttendance;
                                    }
                                    else
                                    {
                                        //Hours check
                                        if (_attendanceList != null && _attendanceList.Count >= 2)
                                        {
                                            Attendance _attendanceYesterday = _attendanceList.OrderByDescending(x => x.Date).Take(2).Last();
                                            _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendanceYesterday.Id);
                                            bool isSameStartEnd = _attendanceDetailList.Where(x => x.StartDate == x.EndDate).Any();
                                            DateTime dtEndDate = _attendanceDetailList.OrderByDescending(x => x.EndDate).FirstOrDefault().EndDate.Value;
                                            DateTime dtStartDate = _attendanceDetailList.OrderBy(x => x.StartDate).FirstOrDefault().StartDate.Value;
                                            if (isSameStartEnd && dtEndDate < _vmAttendance.TimeStamp && _vmAttendance.TimeStamp.Subtract(dtStartDate).TotalHours <= 15)
                                            {
                                                //Update Timeout
                                                _attendanceDetail = _attendanceDetailList.Where(x => x.StartDate == x.EndDate).FirstOrDefault();
                                                _attendance = objAttendanceService.GetAttendance(_attendanceDetail.AttendanceId.Value);
                                                _attendanceDetail.EndDate = _vmAttendance.TimeStamp;
                                                _attendanceDetail.UpdateDate = DateTime.Now;
                                                _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                                                _attendanceTimeOutData = _vmAttendance;
                                            }
                                        }
                                        else
                                        {
                                            //Insert Timein
                                            _attendanceDetail = new AttendanceDetail()
                                            {
                                                AttendanceId = _attendance.Id,
                                                AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                                StartDate = _vmAttendance.TimeStamp,
                                                IsActive = true,
                                                UserIp = "Service",
                                                CreationDate = DateTime.Now
                                            };
                                            _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                                            _attendanceTimeInData = _vmAttendance;
                                        }
                                    }
                                }
                                else
                                {
                                    //Hours check
                                    if (_attendanceList != null && _attendanceList.Count >= 2)
                                    {
                                        Attendance _attendanceYesterday = _attendanceList.OrderByDescending(x => x.Date).Take(2).Last();
                                        _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendanceYesterday.Id);
                                        bool isSameStartEnd = _attendanceDetailList.Where(x => x.StartDate == x.EndDate).Any();
                                        DateTime dtEndDate = _attendanceDetailList.OrderByDescending(x => x.EndDate).FirstOrDefault().EndDate.Value;
                                        DateTime dtStartDate = _attendanceDetailList.OrderBy(x => x.StartDate).FirstOrDefault().StartDate.Value;
                                        if (isSameStartEnd && dtEndDate < _vmAttendance.TimeStamp && _vmAttendance.TimeStamp.Subtract(dtStartDate).TotalHours <= 15)
                                        {
                                                //Update Timeout
                                                _attendanceDetail = _attendanceDetailList.Where(x => x.StartDate == x.EndDate).FirstOrDefault();
                                                _attendance = objAttendanceService.GetAttendance(_attendanceDetail.AttendanceId.Value);
                                                _attendanceDetail.EndDate = _vmAttendance.TimeStamp;
                                                _attendanceDetail.UpdateDate = DateTime.Now;
                                                _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                                                _attendanceTimeOutData = _vmAttendance;
                                        }
                                    }
                                    else
                                    {
                                        //Insert Timein
                                        _attendanceDetail = new AttendanceDetail()
                                        {
                                            AttendanceId = _attendance.Id,
                                            AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                            StartDate = _vmAttendance.TimeStamp,
                                            IsActive = true,
                                            UserIp = "Service",
                                            CreationDate = DateTime.Now
                                        };
                                        _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                                        _attendanceTimeInData = _vmAttendance;
                                    }
                                }
                            }
                            else
                            {
                                //Hours check
                                if (_attendanceList != null && _attendanceList.Count >= 2)
                                {
                                    Attendance _attendanceYesterday = _attendanceList.OrderByDescending(x => x.Date).Take(2).Last();
                                    _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendanceYesterday.Id);
                                    if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                                    {
                                        bool isSameStartEnd = _attendanceDetailList.Where(x => x.StartDate == x.EndDate).Any();
                                        DateTime dtEndDate = _attendanceDetailList.OrderByDescending(x => x.EndDate).FirstOrDefault().EndDate.Value;
                                        DateTime dtStartDate = _attendanceDetailList.OrderBy(x => x.StartDate).FirstOrDefault().StartDate.Value;
                                        if (isSameStartEnd && dtEndDate < _vmAttendance.TimeStamp && _vmAttendance.TimeStamp.Subtract(dtStartDate).TotalHours <= 15)
                                        {
                                            //Update Timeout
                                            _attendanceDetail = _attendanceDetailList.Where(x => x.StartDate == x.EndDate).FirstOrDefault();
                                            _attendance = objAttendanceService.GetAttendance(_attendanceDetail.AttendanceId.Value);
                                            _attendanceDetail.EndDate = _vmAttendance.TimeStamp;
                                            _attendanceDetail.UpdateDate = DateTime.Now;
                                            _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                                            _attendanceTimeOutData = _vmAttendance;
                                        }
                                        else
                                        {
                                            //Insert Timein
                                            _attendanceDetail = new AttendanceDetail()
                                            {
                                                AttendanceId = _attendance.Id,
                                                AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                                StartDate = _vmAttendance.TimeStamp,
                                                IsActive = true,
                                                UserIp = "Service",
                                                CreationDate = DateTime.Now
                                            };
                                            _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                                            _attendanceTimeInData = _vmAttendance;
                                        }
                                    }
                                    else
                                    {
                                        //Insert Timein
                                        _attendanceDetail = new AttendanceDetail()
                                        {
                                            AttendanceId = _attendance.Id,
                                            AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                            StartDate = _vmAttendance.TimeStamp,
                                            IsActive = true,
                                            UserIp = "Service",
                                            CreationDate = DateTime.Now
                                        };
                                        _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                                        _attendanceTimeInData = _vmAttendance;
                                    }
                                }
                                else
                                {
                                    //Insert Timein
                                    _attendanceDetail = new AttendanceDetail()
                                    {
                                        AttendanceId = _attendance.Id,
                                        AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                        StartDate = _vmAttendance.TimeStamp,
                                        IsActive = true,
                                        UserIp = "Service",
                                        CreationDate = DateTime.Now
                                    };
                                    _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                                    _attendanceTimeInData = _vmAttendance;
                                }
                            }
                            #endregion
                            //Console.WriteLine("AttendanceDetail");
                            #region AttendanceStatus Time In
                            if (_attendanceDetail != null && _attendanceDetail.Id > 0 && _attendanceTimeInData != null)
                            {
                                _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                                if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                                {
                                    _attendanceStatus = _attendanceStatusList.FirstOrDefault();
                                    //Console.WriteLine("Old Attendance Status");
                                }
                                else
                                {
                                    //Console.WriteLine("New AttendanceStatus");
                                    _attendanceStatus = new AttendanceStatus() { AttendanceId = _attendance.Id, IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = "Service", LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                                }
                                _userShiftList = objUserShiftService.GetUserShiftByUserId(_attendance.UserId);
                                if (_userShiftList != null && _userShiftList.Count > 0)
                                {
                                    //Console.WriteLine("_userShiftList.Count > 0");
                                    _userShiftList = _userShiftList.Where(x => _attendance.Date.Value >= x.EffectiveDate.Value && _attendance.Date.Value <= (!x.RetiredDate.HasValue ? _attendance.Date.Value : x.RetiredDate.Value)).ToList();
                                    if (_userShiftList != null && _userShiftList.Count > 0)
                                    {
                                        _userShift = _userShiftList.FirstOrDefault();
                                        _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
                                        if (!_shift.StartHour.Contains(":"))
                                            _shift.StartHour += ":00";
                                        _attendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(_userShift.ShiftId.Value);
                                        if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                        {
                                            _attendancePolicyList = _attendancePolicyList.Where(x => _attendance.Date.Value >= x.EffectiveDate.Value && _attendance.Date.Value <= (!x.RetiredDate.HasValue ? _attendance.Date.Value : x.RetiredDate.Value)).ToList();
                                            if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                            {
                                                //FULL Day
                                                _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                                if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                                {
                                                    //Console.WriteLine("_attendancePolicyListTemp.Count > 0");
                                                    //Console.WriteLine(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour);
                                                    _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                    if ((decimal)(_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
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
                                                    if ((decimal)(_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
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
                                                    if ((decimal)(_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                                        _attendanceStatus.IsQuarterDay = true;
                                                    else
                                                        _attendanceStatus.IsQuarterDay = false;
                                                }
                                                else
                                                {
                                                    _attendanceStatus.IsQuarterDay = false;
                                                }
                                            }
                                        }
                                        //Late(
                                        WriteError("Late");
                                        WriteError("Late: " + _attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour);
                                        //Console.WriteLine("Late");
                                        //Console.WriteLine(_attendanceTimeInData.TimeStamp.ToString());
                                        //Console.WriteLine(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour);
                                        //Console.Read();
                                        if (_attendanceTimeInData.TimeStamp > Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour))
                                        {
                                            TimeSpan TimeDiff = _attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour);
                                            _attendanceStatus.LateMinutes = Convert.ToInt32(TimeDiff.TotalMinutes);
                                            _attendanceStatus.IsLate = true;
                                        }
                                        else
                                        {
                                            _attendanceStatus.LateMinutes = 0;
                                            _attendanceStatus.IsLate = false;
                                        }
                                    }
                                    //else
                                    //{
                                    //    //Late
                                    //    if (_attendanceTimeInData.TimeStamp > Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour))
                                    //        _attendanceStatus.IsLate = true;
                                    //    else
                                    //        _attendanceStatus.IsLate = false;
                                    //}
                                }
                                //else
                                //{
                                //    //Late
                                //        _attendanceStatus.IsLate = false;
                                //}
                                if (_attendanceStatus != null && _attendanceStatus.Id <= 0)
                                    _attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                                else
                                {
                                    _attendanceStatus.UpdateDate = DateTime.Now;
                                    _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
                                }
                            }
                            #endregion AttendanceStatus Time In
                            #region AttendanceStatus Time Out
                            else if (_attendanceDetail != null && _attendanceDetail.Id > 0 && _attendanceTimeOutData != null)
                            {
                                int TotalMinutes = 0, BreakMinutes = 0, WorkingMinutes = 0, ShiftMinutes = 0;
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
                                _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                                if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                                {
                                    _attendanceStatus = _attendanceStatusList.FirstOrDefault();
                                }
                                else
                                {
                                    _attendanceStatus = new AttendanceStatus() { AttendanceId = _attendance.Id, IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = "Service", LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                                }
                                _attendanceStatus.TotalMinutes = TotalMinutes;
                                _attendanceStatus.WorkingMinutes = WorkingMinutes;
                                _userShiftList = objUserShiftService.GetUserShiftByUserId(_attendance.UserId);
                                if (_userShiftList != null && _userShiftList.Count > 0)
                                {
                                    _userShiftList = _userShiftList.Where(x => _attendance.Date.Value >= x.EffectiveDate.Value && _attendance.Date.Value <= (!x.RetiredDate.HasValue ? _attendance.Date.Value : x.RetiredDate.Value)).ToList();
                                    if (_userShiftList != null && _userShiftList.Count > 0)
                                    {
                                        _userShift = _userShiftList.FirstOrDefault();
                                        _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
                                        if (!_shift.EndHour.Contains(":"))
                                            _shift.EndHour += ":00";
                                        _attendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(_userShift.ShiftId.Value);
                                        if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                        {
                                            _attendancePolicyList = _attendancePolicyList.Where(x => _attendance.Date.Value >= x.EffectiveDate.Value && _attendance.Date.Value <= (!x.RetiredDate.HasValue ? _attendance.Date.Value : x.RetiredDate.Value)).ToList();
                                            if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                            {
                                                //If same day shift off
                                                if (Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour) < Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.EndHour))
                                                {
                                                    TimeSpan ShiftMinutesSpan = Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.EndHour) - Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour);
                                                    ShiftMinutes = Convert.ToInt32(ShiftMinutesSpan.TotalMinutes);
                                                    //FULL Day
                                                    if (!_attendanceStatus.IsFullDay.Value)
                                                    {
                                                        _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                                        if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                                        {
                                                            _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                            if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                                || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                                _attendanceStatus.IsFullDay = true;
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
                                                            if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                                || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                                _attendanceStatus.IsHalfDay = true;
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
                                                            if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                                || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                                _attendanceStatus.IsQuarterDay = true;
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
                                                    TimeSpan ShiftMinutesSpan = Convert.ToDateTime(attendanceNextDate.ToString(dateFormat) + " " + _shift.EndHour) - Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour);
                                                    ShiftMinutes = Convert.ToInt32(ShiftMinutesSpan.TotalMinutes);
                                                    //FULL Day
                                                    if (!_attendanceStatus.IsFullDay.Value)
                                                    {
                                                        _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                                        if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                                        {
                                                            _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                            if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString(dateFormat) + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                                || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                                _attendanceStatus.IsFullDay = true;
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
                                                            if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString(dateFormat) + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                                || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                                _attendanceStatus.IsHalfDay = true;
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
                                                            if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString(dateFormat) + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                                || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                                _attendanceStatus.IsQuarterDay = true;
                                                            else
                                                                _attendanceStatus.IsQuarterDay = false;
                                                        }
                                                        else
                                                        {
                                                            _attendanceStatus.IsQuarterDay = false;
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
                                                        //_attendanceStatus.IsFullDay = true;
                                                    }
                                                    else
                                                    {
                                                        _attendanceStatus.OverTimeMinutes = 0;
                                                        //_attendanceStatus.IsFullDay = false;
                                                    }
                                                }
                                                else
                                                {
                                                    _attendanceStatus.OverTimeMinutes = 0;
                                                    //_attendanceStatus.IsFullDay = false;
                                                }
                                            }
                                        }
                                        //Early
                                        WriteError("Early");
                                        WriteError("Early: " + _attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour);
                                        //If same day shift off
                                        //if (Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour) < Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour))
                                        if (Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.StartHour) < Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.EndHour))
                                        {
                                            if (_attendanceTimeOutData.TimeStamp < Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.EndHour))
                                            {
                                                TimeSpan TimeDiff = _attendanceTimeOutData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString(dateFormat) + " " + _shift.EndHour);
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
                                            if (_attendanceTimeOutData.TimeStamp < Convert.ToDateTime(attendanceNextDate.ToString(dateFormat) + " " + _shift.EndHour))
                                            {
                                                TimeSpan TimeDiff = _attendanceTimeOutData.TimeStamp - Convert.ToDateTime(attendanceNextDate.ToString(dateFormat) + " " + _shift.EndHour);
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
                                    //else
                                    //{
                                    //    //Early
                                    //    if (_attendanceTimeOutData.TimeStamp < Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour))
                                    //        _attendanceStatus.IsEarly = true;
                                    //    else
                                    //        _attendanceStatus.IsEarly = false;
                                    //}
                                }
                                //else
                                //{
                                //    //Early
                                //    _attendanceStatus.IsEarly = false;
                                //}
                                if (_attendanceStatus != null && _attendanceStatus.Id <= 0)
                                    _attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                                else
                                {
                                    _attendanceStatus.UpdateDate = DateTime.Now;
                                    _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
                                }
                            }
                            #endregion AttendanceStatus Time Out
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteError("Error in Process FilterByUser. Error Message: " + ex.Message + " - Data: " + _vmAttendance.UserId.ToString());
                    }

                    _attendance = null;
                    _attendanceList = null;
                    _attendanceDetailList = null;
                    _attendanceDetail = null;
                    _attendanceList = null;
                    _attendanceStatus = null;
                    _attendanceStatusList = null;
                    _attendanceTimeInData = null;
                    _attendanceTimeOutData = null;
                    _shift = null;
                    _userShift = null;
                    _userShiftList = null;
                }
                #endregion Alternate Time In/Out
            }
            else
            {
                #region Normal TimeIn/Out
                List<int> userIds = attendanceDataList.Select(x => x.UserId).Distinct().ToList();
                foreach (int userId in userIds)
                {
                    try
                    {
                        #region Attendance
                        //_attendanceList = objAttendanceService.GetAttendanceByUserIDAndDate(userId, dtAttendance);
                        _attendanceList = objAttendanceService.GetAttendanceByUserId(userId);
                        //Insert Attendance
                        if (_attendanceList == null || _attendanceList.Count <= 0)
                        {
                            WriteError("No Attendance not marked for userId: " + userId.ToString() + " Inserting Attendance");
                            dtAttendance = attendanceDataList.Where(x => x.UserId == userId).OrderBy(x => x.TimeStamp).Select(x => x.TimeStamp.Date).FirstOrDefault();
                            _attendance = new Attendance() { UserId = userId, Date = dtAttendance, IsActive = true, CreationDate = DateTime.Now, UserIp = "Service" };
                            _attendance = objAttendanceService.InsertAttendance(_attendance);
                        }
                        //Attendance Already marked, Get Attendance
                        else
                        {
                            _attendance = _attendanceList.OrderBy(x => x.Date).Last();
                            dtAttendance = _attendance.Date.Value;
                        }
                        #endregion Attendance
                        #region AttendanceDetail
                        //TIMEIN
                        attendanceDataListTemp = attendanceDataList.Where(x => x.TimeStamp.Date == dtAttendance && x.UserId == userId && (x.BreakType.Contains("DutyOn") || x.BreakType == "0")).ToList();
                        if (attendanceDataListTemp != null && attendanceDataListTemp.Count > 0)
                            _attendanceTimeInData = attendanceDataListTemp.FirstOrDefault();
                        //Timein Entry
                        if (_attendanceTimeInData != null)
                        {
                            _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                            //Already Timein Entry
                            if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                            {
                                _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance).ToList();
                                if (_attendanceDetailList != null && _attendanceDetailList.Count > 0)
                                {
                                    _attendanceDetailList = _attendanceDetailList.Where(x => x.StartDate.Value.Date == _attendanceTimeInData.TimeStamp.Date).ToList();
                                    if (_attendanceDetailList == null || _attendanceDetailList.Count <= 0)
                                    {
                                        //Insert Timein
                                        _attendanceDetail = new AttendanceDetail()
                                        {
                                            AttendanceId = _attendance.Id,
                                            AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                            StartDate = _attendanceTimeInData.TimeStamp,
                                            IsActive = true,
                                            UserIp = "Service",
                                            CreationDate = DateTime.Now
                                        };
                                        _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                                    }
                                    else
                                    {
                                        //Do nothing, timin already marked
                                    }
                                }
                                else
                                {
                                    //Insert Timein
                                    _attendanceDetail = new AttendanceDetail()
                                    {
                                        AttendanceId = _attendance.Id,
                                        AttendanceTypeId = (int)Core.Enum.AttendanceType.DailyAttendance,
                                        StartDate = _attendanceTimeInData.TimeStamp,
                                        IsActive = true,
                                        UserIp = "Service",
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
                                    StartDate = _attendanceTimeInData.TimeStamp,
                                    IsActive = true,
                                    UserIp = "Service",
                                    CreationDate = DateTime.Now
                                };
                                _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                            }
                        }
                        //TIMEOUT
                        attendanceDataListTemp = attendanceDataList.Where(x => x.UserId == userId && (x.BreakType.Contains("DutyOff") || x.BreakType == "1")).ToList();
                        if (attendanceDataListTemp != null && attendanceDataListTemp.Count > 0)
                            _attendanceTimeOutData = attendanceDataListTemp.Last();
                        //Timeout Entry
                        if (_attendanceTimeOutData != null)
                        {
                            _attendanceDetailList = objAttendanceDetailService.GetAttendanceDetailByAttendanceId(_attendance.Id);
                            if (_attendanceDetailList != null && _attendanceDetailList.Count > 0 && _attendanceDetailList.Count > 0)
                            {
                                _attendanceDetailList = _attendanceDetailList.Where(x => x.AttendanceTypeId == (int)Core.Enum.AttendanceType.DailyAttendance
                                    && x.EndDate == null).ToList();
                                if (_attendanceDetailList != null && _attendanceDetailList.Count > 0 && _attendanceDetailList.Count > 0)
                                {
                                    _attendanceDetail = _attendanceDetailList.FirstOrDefault();
                                    _attendanceDetail.EndDate = _attendanceTimeOutData.TimeStamp;
                                    _attendanceDetail.UpdateDate = DateTime.Now;
                                    _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                                }
                            }
                        }
                        #endregion AttendanceDetail
                        #region AttendanceStatus Time In
                        if (_attendanceDetail != null && _attendanceDetail.Id > 0 && _attendanceTimeInData != null)
                        {
                            _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                            if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                            {
                                _attendanceStatus = _attendanceStatusList.FirstOrDefault();
                            }
                            else
                            {
                                _attendanceStatus = new AttendanceStatus() { AttendanceId = _attendance.Id, IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = "Service", LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                            }
                            _userShiftList = objUserShiftService.GetUserShiftByUserId(_attendance.UserId);
                            if (_userShiftList != null && _userShiftList.Count > 0)
                            {
                                _userShiftList = _userShiftList.Where(x => _attendance.Date.Value >= x.EffectiveDate.Value && _attendance.Date.Value <= (!x.RetiredDate.HasValue ? _attendance.Date.Value : x.RetiredDate.Value)).ToList();
                                if (_userShiftList != null && _userShiftList.Count > 0)
                                {
                                    _userShift = _userShiftList.FirstOrDefault();
                                    _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
                                    if (!_shift.StartHour.Contains(":"))
                                        _shift.StartHour += ":00";
                                    _attendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(_userShift.ShiftId.Value);
                                    if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                    {
                                        _attendancePolicyList = _attendancePolicyList.Where(x => _attendance.Date.Value >= x.EffectiveDate.Value && _attendance.Date.Value <= (!x.RetiredDate.HasValue ? _attendance.Date.Value : x.RetiredDate.Value)).ToList();
                                        if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                        {
                                            //FULL Day
                                            _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                            if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                            {
                                                _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                if ((decimal)(_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
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
                                                if ((decimal)(_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
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
                                                if ((decimal)(_attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour)).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                                    _attendanceStatus.IsQuarterDay = true;
                                                else
                                                    _attendanceStatus.IsQuarterDay = false;
                                            }
                                            else
                                            {
                                                _attendanceStatus.IsQuarterDay = false;
                                            }
                                        }
                                    }
                                    //Late
                                    if (_attendanceTimeInData.TimeStamp > Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour))
                                    {
                                        TimeSpan TimeDiff = _attendanceTimeInData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour);
                                        _attendanceStatus.LateMinutes = Convert.ToInt32(TimeDiff.TotalMinutes);
                                        _attendanceStatus.IsLate = true;
                                    }
                                    else
                                    {
                                        _attendanceStatus.LateMinutes = 0;
                                        _attendanceStatus.IsLate = false;
                                    }
                                }
                                //else
                                //{
                                //    //Late
                                //    if (_attendanceTimeInData.TimeStamp > Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour))
                                //        _attendanceStatus.IsLate = true;
                                //    else
                                //        _attendanceStatus.IsLate = false;
                                //}
                            }
                            //else
                            //{
                            //    //Late
                            //        _attendanceStatus.IsLate = false;
                            //}
                            if (_attendanceStatus != null && _attendanceStatus.Id <= 0)
                                _attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                            else
                            {
                                _attendanceStatus.UpdateDate = DateTime.Now;
                                _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
                            }
                        }
                        #endregion AttendanceStatus Time In
                        #region AttendanceStatus Time Out
                        else if (_attendanceDetail != null && _attendanceDetail.Id > 0 && _attendanceTimeOutData != null)
                        {
                            int TotalMinutes = 0, BreakMinutes = 0, WorkingMinutes = 0, ShiftMinutes = 0;
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
                            _attendanceStatusList = objAttendanceStatusService.GetAttendanceStatusByAttendanceId(_attendance.Id);
                            if (_attendanceStatusList != null && _attendanceStatusList.Count > 0)
                            {
                                _attendanceStatus = _attendanceStatusList.FirstOrDefault();
                            }
                            else
                            {
                                _attendanceStatus = new AttendanceStatus() { AttendanceId = _attendance.Id, IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsEarly = false, IsActive = true, CreationDate = DateTime.Now, UserIp = "Service", LateMinutes = 0, EarlyMinutes = 0, TotalMinutes = 0, WorkingMinutes = 0, OverTimeMinutes = 0 };
                            }
                            _attendanceStatus.TotalMinutes = TotalMinutes;
                            _attendanceStatus.WorkingMinutes = WorkingMinutes;
                            _userShiftList = objUserShiftService.GetUserShiftByUserId(_attendance.UserId);
                            if (_userShiftList != null && _userShiftList.Count > 0)
                            {
                                _userShiftList = _userShiftList.Where(x => _attendance.Date >= x.EffectiveDate && _attendance.Date <= (x.RetiredDate == null ? _attendance.Date : x.RetiredDate)).ToList();
                                if (_userShiftList != null && _userShiftList.Count > 0)
                                {
                                    _userShift = _userShiftList.FirstOrDefault();
                                    _shift = objShiftService.GetShift(_userShift.ShiftId.Value);
                                    if (!_shift.EndHour.Contains(":"))
                                        _shift.EndHour += ":00";
                                    _attendancePolicyList = objAttendancePolicyService.GetAttendancePolicyByShiftId(_userShift.ShiftId.Value);
                                    if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                    {
                                        _attendancePolicyList = _attendancePolicyList.Where(x => _attendance.Date >= x.EffectiveDate && _attendance.Date <= (x.RetiredDate == null ? _attendance.Date : x.RetiredDate)).ToList();
                                        if (_attendancePolicyList != null && _attendancePolicyList.Count > 0)
                                        {
                                            //If same day shift off
                                            if (Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour) < Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour))
                                            {
                                                TimeSpan ShiftMinutesSpan = Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour) - Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour);
                                                ShiftMinutes = Convert.ToInt32(ShiftMinutesSpan.TotalMinutes);
                                                //FULL Day
                                                if (!_attendanceStatus.IsFullDay.Value)
                                                {
                                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                                    {
                                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                        if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                            _attendanceStatus.IsFullDay = true;
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
                                                        if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                            _attendanceStatus.IsHalfDay = true;
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
                                                        if ((decimal)(Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                            _attendanceStatus.IsQuarterDay = true;
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
                                                TimeSpan ShiftMinutesSpan = Convert.ToDateTime(attendanceNextDate.ToString("dd/MM/yyyy") + " " + _shift.EndHour) - Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour);
                                                ShiftMinutes = Convert.ToInt32(ShiftMinutesSpan.TotalMinutes);
                                                //FULL Day
                                                if (!_attendanceStatus.IsFullDay.Value)
                                                {
                                                    _attendancePolicyListTemp = _attendancePolicyList.Where(x => x.AttendanceVariableId == (int)Core.Enum.AttendanceVariable.FullDay).ToList();
                                                    if (_attendancePolicyListTemp != null && _attendancePolicyListTemp.Count > 0)
                                                    {
                                                        _attendancePolicy = _attendancePolicyListTemp.FirstOrDefault();
                                                        if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString("dd/MM/yyyy") + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || (decimal)ShiftMinutes - TotalMinutes >= (decimal)_attendancePolicy.Hours.Value)
                                                            _attendanceStatus.IsFullDay = true;
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
                                                        if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString("dd/MM/yyyy") + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                            _attendanceStatus.IsHalfDay = true;
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
                                                        if ((decimal)(Convert.ToDateTime(attendanceNextDate.ToString("dd/MM/yyyy") + " " + _shift.EndHour) - _attendanceTimeOutData.TimeStamp).TotalMinutes >= (decimal)_attendancePolicy.Hours.Value
                                                            || ShiftMinutes - TotalMinutes >= _attendancePolicy.Hours.Value)
                                                            _attendanceStatus.IsQuarterDay = true;
                                                        else
                                                            _attendanceStatus.IsQuarterDay = false;
                                                    }
                                                    else
                                                    {
                                                        _attendanceStatus.IsQuarterDay = false;
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
                                                    _attendanceStatus.IsFullDay = true;
                                                }
                                                else
                                                {
                                                    _attendanceStatus.OverTimeMinutes = 0;
                                                    _attendanceStatus.IsFullDay = false;
                                                }
                                            }
                                            else
                                            {
                                                _attendanceStatus.OverTimeMinutes = 0;
                                                _attendanceStatus.IsFullDay = false;
                                            }
                                        }
                                    }
                                    //Early
                                    //If same day shift off
                                    if (Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.StartHour) < Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour))
                                    {
                                        if (_attendanceTimeOutData.TimeStamp < Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour))
                                        {
                                            TimeSpan TimeDiff = _attendanceTimeOutData.TimeStamp - Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour);
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
                                        if (_attendanceTimeOutData.TimeStamp < Convert.ToDateTime(attendanceNextDate.ToString("dd/MM/yyyy") + " " + _shift.EndHour))
                                        {
                                            TimeSpan TimeDiff = _attendanceTimeOutData.TimeStamp - Convert.ToDateTime(attendanceNextDate.ToString("dd/MM/yyyy") + " " + _shift.EndHour);
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
                                //else
                                //{
                                //    //Early
                                //    if (_attendanceTimeOutData.TimeStamp < Convert.ToDateTime(_attendance.Date.Value.ToString("dd/MM/yyyy") + " " + _shift.EndHour))
                                //        _attendanceStatus.IsEarly = true;
                                //    else
                                //        _attendanceStatus.IsEarly = false;
                                //}
                            }
                            //else
                            //{
                            //    //Early
                            //    _attendanceStatus.IsEarly = false;
                            //}
                            if (_attendanceStatus != null && _attendanceStatus.Id <= 0)
                                _attendanceStatus = objAttendanceStatusService.InsertAttendanceStatus(_attendanceStatus);
                            else
                            {
                                _attendanceStatus.UpdateDate = DateTime.Now;
                                _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
                            }
                        }
                        #endregion AttendanceStatus Time Out
                    }
                    catch (Exception ex)
                    {
                        WriteError("Error in Process FilterByUser. Error Message: " + ex.Message + " - Data: " + userId.ToString());
                    }

                    _attendance = null;
                    _attendanceList = null;
                    _attendanceDetailList = null;
                    _attendanceDetail = null;
                    _attendanceList = null;
                    _attendanceStatus = null;
                    _attendanceStatusList = null;
                    _attendanceTimeInData = null;
                    _attendanceTimeOutData = null;
                    _shift = null;
                    _userShift = null;
                    _userShiftList = null;
                }
                #endregion Normal Time In/Out
            }
        }
        private void WriteError(string msg)
        {
            File.AppendAllText(errorFilePath, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "_CORE_" + msg + Environment.NewLine);
        }
        public bool ServiceStatus()
        {
            bool serviceStatus;
            IConfigurationService objConfigurationService = IoC.Resolve<HRM.Core.IService.IConfigurationService>("ConfigurationService"); ;
            List<Configuration> _configurationList = objConfigurationService.GetAllConfiguration();
            if (_configurationList != null && _configurationList.Count > 0)
            {
                _configurationList = _configurationList.Where(x => x.Name == Constant.ServiceStatus).ToList();
                if (_configurationList != null && _configurationList.Count > 0)
                {
                    serviceStatus = Convert.ToBoolean(Convert.ToInt32(_configurationList.FirstOrDefault().Value));
                }
                else
                    serviceStatus = false;
            }
            else
                serviceStatus = false;
            return serviceStatus;
        }
        public void UpdateServiceStatus(bool status)
        {
            IConfigurationService objConfigurationService = IoC.Resolve<HRM.Core.IService.IConfigurationService>("ConfigurationService"); ;
            List<Configuration> _configurationList = objConfigurationService.GetAllConfiguration();
            Configuration _configuration = null;
            if (_configurationList != null && _configurationList.Count > 0)
            {
                _configurationList = _configurationList.Where(x => x.Name == Constant.ServiceStatus).ToList();
                if (_configurationList != null && _configurationList.Count > 0)
                {
                    _configuration = _configurationList.FirstOrDefault();
                }

                if (_configuration != null)
                {
                    _configuration.BoolValue = status;
                    objConfigurationService.UpdateConfiguration(_configuration);
                }
            }
        }
        public void InsertPreAttendance()
        {

            IAttendanceService objAttendanceService = IoC.Resolve<HRM.Core.IService.IAttendanceService>("AttendanceService");
            objAttendanceService.InsertPreAttendance();
        }
    }

    public enum Cols
    {
        No = 0,
        TMNo = 1,
        EnNo = 2,
        Name = 3,
        GMNo = 4,
        Mode = 5,
        In_Out = 6,
        Antipass = 7,
        ProxyWork = 8,
        DateTime = 9
    }
}
