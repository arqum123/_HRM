using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public class VMTicketHistory
    {
        public int? TicketID { get; set; }
        public int? AttendanceID { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime ShiftTimeIn { get; set; }
        public DateTime ShiftTimeOut { get; set; }
        public DateTime AttendanceDate { get; set; }
        public Boolean? OffDay { get; set; }
        public Boolean? Holiday { get; set; }
        public string Reason { get; set; }
        public string ShiftName { get; set; }
        public string DepartmentName { get; set; }
        public string EmpName { get; set; }
        public string ManagerName { get; set; }
        public Boolean? IsApproved { get; set; }
        public Boolean? IsReject { get; set; }
        public string Comments { get; set; }
    }
}
