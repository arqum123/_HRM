using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMModifyPayrollEditVariable
    {
        public Int32? PayrollVariableId { get; set; } //PayrollVariable
        public Int32? PayrollPolicyId { get; set; } //PayrollPolicy
        public Int32? PayrollDetailId { get; set; } //PayrollDetail
        public Decimal? Amount { get; set; }

        public string PayrollVariableName { get; set; } //PayrollVariable


        public string PayrollPolicyName { get; set; } //PayrollPolicy
        public string OtherPayrollVariableName { get; set; } //New
        public Boolean? PayrollVariableIsActive { get; set; } //PayrollVariable
    }
}
