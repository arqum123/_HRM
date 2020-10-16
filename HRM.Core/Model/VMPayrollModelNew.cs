using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMPayrollModelNew
    {
        public List<PayrollDetail> PayrollPolicyList { get; set; }
        public List<Payroll> PayrollList { get; set; }
    }
}
