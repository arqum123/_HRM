using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMPayslipAbsentInformation
    {
        public int? AttendanceId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? LeaveAttendanceDate { get; set; }
    }
}
