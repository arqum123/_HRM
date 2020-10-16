using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    class VMAttendanceReport
    {
      
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public bool IsOffDay { get; set; }
        public bool IsLate { get; set; }
        public bool IsEarly { get; set; }
        public bool IsFullDay { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsQuarterDay { get; set; }
        public int OverTimeMinutes { get; set; }
        public int EarlyMinutes { get; set; }
        public int LateMinutes { get; set; }
    }
}
