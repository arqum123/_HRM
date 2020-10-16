using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMDailyAttendanceUpdate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? UserId { get; set; }
        public List<User> UserList { get; set; }
        public List<VMDailyAttendanceUpdateStatus> VMDailyAttendanceUpdateStatusList { get; set; }
        public List<VMDailyAttendanceUpdateTime> VMDailyAttendanceUpdateTimeList { get; set; }
    }
}
