using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMPayslip
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set;}
        public string UserName { get; set; }
        public int PayrollCycleMonth { get; set; }
        public int PayrollCycleYear { get; set;}
        public bool PayrollCycleIsActive { get; set; }
    

        public List<Payroll> PayrollList { get; set; }

        public Payroll Payroll { get; set; }
    }
}
