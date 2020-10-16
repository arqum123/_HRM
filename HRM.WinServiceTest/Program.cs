using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HRM.Core;
using HRM.Core.Entities;
using HRM.Core.Helper;
using HRM.Core.IService;
using HRM.Core.Model;
using HRM.WinServiceLibrary;

namespace HRM.WinServiceTest
{
    class Program
    {
        static bool isStopped = true;
        static int stopCounter = 0;
        static string mainErrorFile = AppDomain.CurrentDomain.BaseDirectory + "\\ServiceLog.txt";
        static Auth objAuth = null;
        static SystemConfiguration objSystemConfiguration = null;
        static string fn = "";
        static string fp = AppDomain.CurrentDomain.BaseDirectory + "\\AD";
        static void Main(string[] args)
        {
            OnStart();
            Manual();
            Process();
            return;
            Console.WriteLine("User id: " + args[0]);
            Console.WriteLine("Date: " + args[1]);
            Console.WriteLine("Time IN: " + args[2]);
            Console.WriteLine("Time OUT: " + args[3]);
            Temp(Convert.ToInt32(args[0]),args[1],args[2],args[3]);
            return;

            if (fn.Length > 0)
                fn = args[0];
            else
            {
                fn = "";
                //return;
            }
            Console.WriteLine(DateTime.Now.ToString());
            string str = Console.ReadLine();
            if (str.ToLower().Trim() == "n")
                return;
            OnStart();
            Manual();
            //Process();
        }
        static void Manual()
        {
            AttendanceFromDevice objAttendanceFromDevice = new AttendanceFromDevice();
            List<String> lines = null;
                    
            //Console.WriteLine("Here I am 3"); Console.ReadLine();
            string[] dirs = Directory.GetDirectories(fp);
            //Console.WriteLine(fp);
            //Console.WriteLine(dirs.Length.ToString()); 
            //Console.ReadLine();
            string errorFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd");
            string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd");
            foreach (string dir in dirs)
            {
                Console.WriteLine("Processing..." + dir);
                //Console.ReadLine();
                string[] files = Directory.GetFiles(dir);
                List<DataFile> DataFileList = new List<DataFile>();
                foreach (string f in files)
                    DataFileList.Add(new DataFile() { path = f, crdate = File.GetLastWriteTime(f) });
                DataFileList = DataFileList.OrderBy(x => x.crdate).ToList();
                foreach (DataFile df in DataFileList)
                {
                    Console.WriteLine(df.path);
                    lines = File.ReadAllLines(df.path).ToList<string>();
                    if (lines != null && lines.Count > 0)
                    {
                        objAttendanceFromDevice.ProcessAttendance(logFilePath, errorFilePath, lines);
                    }
                }
            }
        }
        static void OnStart()
        {
            try
            {
                Console.WriteLine("Starting...");
                isStopped = false;
                objAuth = new Auth(mainErrorFile);
                objSystemConfiguration = new SystemConfiguration();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Error on Start - Error:" + ex.Message + Environment.NewLine);
            }
        }
        static void Process()
        {
            try
            {
                Console.WriteLine("Processing...");
                AttendanceFromDevice objAttendanceFromDevice = new AttendanceFromDevice();
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Configuration: isStopped:" + isStopped.ToString() + " stopCounter:" + stopCounter.ToString() + Environment.NewLine);
                
                AttendanceMarking objAttendanceMarking = null;
                Branch _branch = null;
                foreach (Device _device in objSystemConfiguration.GetAllActiveDevices())
                {
                    //if (_device.MachineId != 99)
                    //    continue;
                    System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "-" + "Device:" + _device.DeviceId.Value.ToString() + Environment.NewLine);
                    objAttendanceMarking = new AttendanceMarking(_device.DeviceModalId.Value, _device.MachineId.Value, _device.DeviceId.Value.ToString(), _device.IpAddress, _device.PortNumber.Value, _device.Password, _device.ComPort.Value, _device.Baudrate.Value.ToString(), _device.ConnectionTypeId.Value);
                    objAttendanceMarking.mainLogFilePath = mainErrorFile;
                    _branch = objSystemConfiguration.GetBranchInfo(_device.BranchId.Value);
                    //Console.WriteLine("Here I am 1"); Console.ReadLine();
                    List<String> lines=null;
                    if (fn == "")
                    {
                        //Console.WriteLine("Here I am 2"); Console.ReadLine();
                        //lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\" + fn).ToList<string>();
                        lines = objAttendanceMarking.GetAttendanceDataFromDevice();
                        objSystemConfiguration.UpdateDeviceStatus(_device.Id, objAttendanceMarking.connectionStatus, objAttendanceMarking.connectionStatusDesc);
                        if (lines != null && lines.Count > 0)
                        {
                            objAttendanceFromDevice.ProcessAttendance(objAttendanceMarking.logFilePath, objAttendanceMarking.errorFilePath, lines);
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Here I am 3"); Console.ReadLine();
                        string[] dirs = Directory.GetDirectories(fp);
                        //Console.WriteLine(fp);
                        //Console.WriteLine(dirs.Length.ToString()); 
                        //Console.ReadLine();
                        foreach (string dir in dirs)
                        {
                            Console.WriteLine("Processing..."+dir);
                            //Console.ReadLine();
                            string[] files = Directory.GetFiles(dir);
                            List<DataFile> DataFileList = new List<DataFile>();
                            foreach (string f in files)
                                DataFileList.Add(new DataFile() { path = f, crdate = File.GetLastWriteTime(f) });
                            DataFileList = DataFileList.OrderBy(x => x.crdate).ToList();
                            foreach (DataFile df in DataFileList)
                            {
                                Console.WriteLine(df.path);
                                lines = File.ReadAllLines(df.path).ToList<string>();
                                objSystemConfiguration.UpdateDeviceStatus(_device.Id, objAttendanceMarking.connectionStatus, objAttendanceMarking.connectionStatusDesc);
                                if (lines != null && lines.Count > 0)
                                {
                                    objAttendanceFromDevice.ProcessAttendance(objAttendanceMarking.logFilePath, objAttendanceMarking.errorFilePath, lines);
                                }
                            }
                        }
                    }
                    //List<String> lines = objAttendanceMarking.GetAttendanceDataFromDevice();
                    
                    objAttendanceMarking = null;
                    _branch = null;
                }
                objAttendanceFromDevice = null;
                //objSystemConfiguration = null;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(mainErrorFile, DateTime.Now.ToString("[MM/dd/yyyy hh:mm:ss]") + "- Error: " + ex.Message + Environment.NewLine);
            }
        }

        static void Temp(int uid,string dt,string tin, string tout)
        {
            Console.WriteLine("1");
            VMAddAttendance m = new VMAddAttendance() {UserId=uid, AttendanceDate=Convert.ToDateTime(dt), TimeIn=tin, TimeOut=tout};
            AddAttendance(m);
        }
        public static string AddAttendance(VMAddAttendance model)
        {
            try
            {

                Console.WriteLine("2");

                #region Init
                List<Attendance> _attendanceList = null;
                Attendance _attendance = null;
                List<AttendanceDetail> _attendanceDetailList = null;
                AttendanceDetail _attendanceDetail = null;
                List<AttendanceStatus> _attendanceStatusList = null;
                AttendanceStatus _attendanceStatus = null;
                //List<VMAttendanceData> attendanceDataList = new List<VMAttendanceData>();

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

                #endregion Init
                Console.WriteLine("3");

                #region Attendance
                _attendanceList = objAttendanceService.GetAttendanceByUserIDAndDate(model.UserId, model.AttendanceDate);
                //Insert Attendance
                if (_attendanceList == null || _attendanceList.Count <= 0)
                {
                    _attendance = new Attendance() { UserId = model.UserId, Date = model.AttendanceDate, IsActive = true, CreationDate = DateTime.Now, UserIp = "Manual" };
                    _attendance = objAttendanceService.InsertAttendance(_attendance);
                    //ViewBag.Message = "Addedd successfully";
                    //ViewBag.MsgClass = "alert-success";
                }
                //Attendance Already marked, Get Attendance
                else
                {
                    _attendance = _attendanceList.FirstOrDefault();
                    //ViewBag.Message = "Updated successfully";
                    //ViewBag.MsgClass = "alert-success";
                }
                #endregion Attendance
                Console.WriteLine("4");

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
                            if (_attendanceDetailList.FirstOrDefault().StartDate == null)
                            {
                                _attendanceDetail.StartDate = model.DateTimeIn;
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
                                StartDate = model.DateTimeIn,
                                IsActive = true,
                                UserIp = "Manual",
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
                            UserIp = "Manual",
                            CreationDate = DateTime.Now
                        };
                        _attendanceDetail = objAttendanceDetailService.InsertAttendanceDetail(_attendanceDetail);
                    }
                }
                //Timeout Entry
                if (model.TimeOut != null)
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
                                _attendanceDetail.EndDate = model.DateTimeOut;
                                _attendanceDetail = objAttendanceDetailService.UpdateAttendanceDetail(_attendanceDetail);
                            }
                        }
                    }
                }
                #endregion AttendanceDetail
                Console.WriteLine("5");

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
                        _attendanceStatus = new AttendanceStatus() { AttendanceId = _attendance.Id, IsShiftOffDay = false, IsLeaveDay = false, IsHoliday = false, IsFullDay = false, IsQuarterDay = false, IsHalfDay = false, IsLate = false, IsActive = true, CreationDate = DateTime.Now, UserIp = "Manual" };
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
                                    //FULL Day
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

                                    //Half Day
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

                                    //Quater Day
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
                        _attendanceStatus = objAttendanceStatusService.UpdateAttendanceStatus(_attendanceStatus);
                    }
                }
                #endregion AttendanceStatus
                Console.WriteLine("6");

                return "DONE";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
            return "DONE";
        }

    }
    public class DataFile
    {
        public string path { get; set; }
        public DateTime crdate { get; set; }
    }
}
