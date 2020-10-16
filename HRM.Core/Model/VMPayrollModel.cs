using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMPayrollModel
    {
        public List<PayrollPolicy> PayrollPolicyList { get; set; }
        public List<Payroll> PayrollList { get; set; }
        public PayrollCycle PayrollCycle { get; set; }

        //New
        public List<PayrollDetail> PayrollDetailList { get; set; }

        public Payroll Payroll { get; set; }

        public System.Int32? DepartmentID { get; set; }
        public string UserName { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public int IsActive { get; set; }
    }
}
