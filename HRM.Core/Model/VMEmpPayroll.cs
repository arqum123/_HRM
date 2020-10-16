using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMEmpPayroll
    {
        public int? PayrollCycleId { get; set; }
        public List<VMEmpPayrollService> VMEmpPayrollServiceList { get; set; }
    }
}
