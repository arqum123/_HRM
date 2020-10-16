using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMAttendanceSummary
    {
       
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public string UserName { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public int Total { get; set; }
        public int Present { get; set; }
        public int Absent { get { return Total - Present-OffDay ; } }
        public int OffDay { get; set; }
        public int Late { get; set; }
        public int Early { get; set; }
        public int OverTime { get; set; }
        public int FullDay { get; set; }


        //NewFor DailyAttendanceReport

        public int AttendanceId { get; set; }
        public int AttendanceStatudId { get; set; }
        public int UserId { get; set; }
        public int DepartId { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public DateTime ATStartDate { get; set; } //Attendance Table TimeIn
        public DateTime ATEndDate { get; set; } //Attendance Table TimeOut

        public DateTime ADStartDate { get; set; } //AttendanceDetail Table TimeIn
        public DateTime ADEndDate { get; set; } //AttendanceDetail Table TimeOut
        public int EarlyMinutes { get; set; }
        public int LateMinutes { get; set; }
        public int OverTimeMinutes { get; set; }

        public int WorkingMinutes { get; set; }
        public int TotalMinutes { get; set; }

        public bool IsShiftOffDay { get; set; }
        public bool IsHoliday { get; set; }

        public bool IsQuarterDay { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsFullDay { get; set; }
        public bool IsEarly { get; set; }
        public bool IsLate { get; set; }
        
        public String Designation { get; set; }
        public String NICNo { get; set; }
        public List<VMAttendanceReportDetailTime> VMAttendanceReportDetailTimeList { get; set; }
    }
}
