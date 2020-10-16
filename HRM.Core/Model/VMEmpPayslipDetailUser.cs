using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMEmpPayslipDetailUser
    {
        public int PayrollCycleId { get; set; }
        public bool PayrollCycleIsActive { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string DName { get; set; }
        public string Designation { get; set; }
        public string PayrollCycleName { get; set; }

        public List<VMEmpPayslipDetailPayroll> VMEmpPayslipDetailPayrollList { get; set; }
        public List<VMEmpPayslipDetailVariables> VMEmpPayslipDetailVariablesList { get; set; }
    }
}
