using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public  class AttendanceAndAttendanceStatusViewList
    {
        public int AttendanceID { get; set; }
        public int UserID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsShiftOffDay { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsLeaveDay { get; set; }
        public bool IsFullDay { get; set; }
        public bool IsQuarterDay { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsLate { get; set; }
        public bool IsEarly { get; set; }

        //public List<AttendanceAndAttendanceStatusViewStatus> AttendanceAndAttendanceStatusViewStatusList { get; set; }
    }
}
