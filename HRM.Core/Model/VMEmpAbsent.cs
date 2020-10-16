using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMEmpAbsent
    {
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserName { get; set; }
        public List<VMAbsentReport> VMAbsentReportList { get; set; }
    }
}
