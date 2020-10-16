using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMPayslipDetailVariables
    {
        public int PayrollId { get; set; }
        public int PayrollDetailId { get; set; }
        public int PayrollPolicyId { get; set; }
        public int PayrollVariableId { get; set; }
        public string PayrollVariableName { get; set; }
        public string PayrollPolicyName { get; set; }
        public double Amount { get; set; }
    }
}
