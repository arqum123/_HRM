using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMAttendanceModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public System.Int32? DepartmentID { get; set; }
        public System.Int32? UserId { get; set; }

        public int? BranchId { get; set; }
        public int? SalaryTypeId { get; set; }
        public int? ShiftId { get; set; }

        public List<Leave> LeaveList { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<User> UserList { get; set; }
        public List<Attendance> AttendanceList { get; set; }
        public PayrollCycle PayrollCycle { get; set; }

        public PayrollPolicy PayrollPolicy { get; set; }

        public PayrollVariable PayrollVariable { get; set; }
        public List<VMAttendanceSummary> AttendanceSummaryList { get; set; }

        public List<VMAttendanceStatusSummary> AttendanceStatusSummaryList { get; set; }


        public string Name { get; set; }
       
        public int? ID { get; set; }
        //New
        public int? PayrollCycleId { get; set; }


    }
}
