using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMBrowseAttendance
    {
        public int UserId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
    }
}
