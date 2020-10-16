using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMFinalPayroll
    {
        public int PayrollCycleId { get; set; }
        public bool IsFinal { get; set; }
        public List<PayrollCycle> payrollCycleList { get; set; }
        public List<Payroll> payrollList { get; set; }

    }
}
