using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMApprovedAllLeaves
    {
       

        public int UserID { get; set; }
        public int UserDepartmentID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string EmpName { get; set; }
        public int LeaveID { get; set; }

        public int LeaveTypeID { get; set; }
        public string Reason { get; set; }
        public bool IsApproved { get; set; }
        public bool IsReject { get; set; }
        public string AdminReason { get; set; }
        public string LeaveTypeName { get; set; }
        public DateTime Date { get; set; }
    }
}
