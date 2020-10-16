using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMDailyAttendanceUpdateStatus
    {
        public int AttendanceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string EmployeeName {
            get
            {
                return EmployeeFName ?? "" + (" " + EmployeeMName) ?? "" + (" " + EmployeeLName) ?? "";
            }
        }
        public string EmployeeFName { get; set; }
        public string EmployeeMName { get; set; }
        public string EmployeeLName { get; set; }


        public bool Early { get; set; }
        public bool Late { get; set; }
        public int EarlyMinutes { get; set; }
        public int LateMinutes { get; set; }
        public List<VMDailyAttendanceUpdateTime> VMDailyAttendanceUpdateTimeList { get; set; }

    }
}
