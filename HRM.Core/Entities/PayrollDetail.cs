
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class PayrollDetail : PayrollDetailBase 
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
