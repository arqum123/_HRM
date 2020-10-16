using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMEmpPayrollService
    {
        public int? PayrollCycleId { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public double? Salary { get; set; }
        public double? Addition { get; set; }
        public double? Deduction { get; set; }
        public double? NetSalary { get; set; }
    }
}
