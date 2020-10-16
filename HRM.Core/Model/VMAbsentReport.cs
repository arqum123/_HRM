using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMAbsentReport
    {
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime AttendanceDate { get; set; }

        public string DepartmentName { get; set; }
        public string EmployeeFName { get; set; }
        public string EmployeeMName { get; set; }
        public string EmployeeLName { get; set; }

    }
}
