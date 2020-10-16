using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMEMPPayslipDetail
    {
        public List<VMEmpPayslipDetailUser> VMEmpPayslipDetailUserList { get; set; }
        public List<VMEmpPayslipDetailPayroll> VMEmpPayslipDetailPayrollList { get; set; }
        public List<VMEmpPayslipDetailVariables> VMEmpPayslipDetailVariablesList { get; set; }
    }
}
