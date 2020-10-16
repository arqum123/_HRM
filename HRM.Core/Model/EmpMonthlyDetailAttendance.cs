using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class EmpMonthlyDetailAttendance
    {
        public int AttendanceId { get; set; }
        public int AttendanceStatudId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string DepartmentName { get; set; }
        public double LateMin { get; set; }
        public double EarlyMin { get; set; }
        public double WorkingMin { get; set; }
        public double TotalMin { get; set; }
        public Boolean IsShiftOffDay { get; set; }
        public Boolean IsFullDay { get; set; }
        public Boolean IsHalfDay { get; set; }
        public Boolean IsQuarterDay { get; set; }
        public Boolean IsLate { get; set; }
        public Boolean IsEarly { get; set; }
        public DateTime AtStartDate { get; set; }
        public DateTime ATEndDate { get; set; }
        public List<EmpMonthlyDetailAttendanceDuration> EmpMonthlyDetailAttendanceDurationList { get; set; }
    }
}
