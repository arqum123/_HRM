using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace HRM.Core.Model
{
   public class VMUserPayrollDetailEdit
    {
        public System.Int32? PayrollDetailId { get; set; }
        public System.Int32? PayrollVariableId { get; set; }
        public System.String PayrollPolicyName { get; set; }
        public System.String PayrollVariableName { get; set; }
        public System.String PayrollVariableIsActive { get; set; }
        public System.Decimal? Amount { get; set; }
        public System.Int32? IsUpdate { get; set; }
        public System.Int32? IsDelete { get; set; }
        //public List<VMUserCustomPayrollDetailEdit> VMUserCustomPayrollDetailEditList { get; set; }
    }

}

