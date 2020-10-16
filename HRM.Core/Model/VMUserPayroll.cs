using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public class VMUserPayroll
    {
        public string PayrollVariable { get; set; }
        public string OtherVariable { get; set; }
        public decimal? Amount { get; set; }
        public System.Boolean? IsEdit { get; set; }
        public System.Boolean? IsDelete { get; set; }
        public List<VMUserPayrollEdit> VMUserPayrollEditList { get; set; }
        public List<VMUserPayrollDetailEdit> VMUserPayrollDetailEditList { get; set; }
    }
}
