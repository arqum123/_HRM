using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public class VMGeneratePayrollForUser
    {
        public int UserId { get; set; }
        public int PayrollCycleId { get; set; }
        public string UserName { get; set; }
        public string PayrollCycleName { get; set; }
        public List<PayrollCycle> PayrollCycleList { get; set; }
        public List<User> UserList { get; set; }
        public List<Payroll> PayrollList { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<Attendance> AttendanceList { get; set; }
        public PayrollCycle PayrollCycle { get; set; }

        public PayrollPolicy PayrollPolicy { get; set; }

        public PayrollVariable PayrollVariable { get; set; }
        public List<VMAttendanceSummary> AttendanceSummaryList { get; set; }

        public List<VMAttendanceStatusSummary> AttendanceStatusSummaryList { get; set; }

    }
}
