using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public class VMPayslipVariableInformation
    {
        public int? AttendanceID { get; set; }
        public int? UserID { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOutt { get; set; }
        public DateTime? ShiftTimeIn { get; set; }
        public DateTime? ShiftTimeOut { get; set; }
        public bool? IsEarly { get; set; }
        public bool? IsLate { get; set; }
        public bool? IsOffDay { get; set; }
        public string ShiftName { get; set; }
        public string Reason { get; set; }
        public int? TicketAttendanceID { get; set; }
    }
}
