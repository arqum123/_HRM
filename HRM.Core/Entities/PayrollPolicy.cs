
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;


namespace HRM.Core.Entities
{
    [DataContract]
    public partial class PayrollPolicy : PayrollPolicyBase
    {
        public string PayrollVariableName { get; set; }
        public string PayrollVariableFactor { get; set; }
        public decimal PayrollPolicyValue { get; set; }

        public PayrollVariable PayrollVariable { get; set; }


    }
}
