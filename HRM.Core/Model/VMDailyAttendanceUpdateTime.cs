using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMDailyAttendanceUpdateTime
    {
        public int AttendanceDetailId { get; set; }
        public DateTime DateTimeIn { get; set; }
        public DateTime DateTimeOut { get; set; }
        public bool IsUpdate { get; set; }
    }
}
