using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class EmpMonthlyAttendance
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public UserDepartment UserDepartmentList { get; set; }
        public List<EmpMonthlyDetailAttendance> EmpMonthlyDetailAttendanceList { get; set; }
        public List<EmpMonthlyDetailAttendanceDuration> EmpMonthlyDetailAttendanceDurationList { get; set; }
    }
}
