using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public class VMPayslipDetail
    {
        public List<VMPayslipDetailUser> VMPayslipDetailUserList { get; set; }
        public List<VMPayslipDetailPayroll> VMPayslipDetailPayrollList { get; set; }
        public List<VMPayslipDetailVariables> VMPayslipDetailVariablesList { get; set; }
    }
}
