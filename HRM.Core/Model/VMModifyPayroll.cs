using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public class VMModifyPayroll
    {
        public System.Int32? DepartmentId { get; set; }
        public System.Int32? UserId { get; set; }
        public System.Int32? PayrollId { get; set; }
        public string UserName { get; set; }
        public System.Int32? PayrollCycleId { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public List<Payroll> PayrollList { get; set; }
        public PayrollCycle PayrollCycle { get; set; }
        public PayrollVariable PayrollVariable { get; set; }
        public List<VMModifyPayrollEdit> VMModifyPayrollEditList { get; set; }
    }
}
