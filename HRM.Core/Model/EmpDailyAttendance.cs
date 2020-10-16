using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class EmpDailyAttendance
    {
        public int? UserLoginId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime Date { get; set; }
        public UserDepartment UserDepartmentList { get; set; }
        public List<EmpDailyDetailAttendance> EmpDailyDetailAttendanceList { get; set; }
        public List<EmpDailyDetailAttendanceDuration> EmpDailyDetailAttendanceDurationList { get; set; }
    }
}
