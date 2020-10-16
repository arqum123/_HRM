using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HRM.Core.Entities
{
    [DataContract]
    public partial class CustomPayrollDetail : CustomPayrollDetailBase
    {
        public PayrollPolicy PayrollPolicy { get; set; }

        public int PayrollVariableId { get; set; }

        public int PayrollDetailId { get; set; }

        public bool IsAddition { get; set; }
        public bool IsDeduction { get; set; }

        public bool IsUpdate { get; set; }

        public string PayrollPolicyName { get; set; }

    }
}
