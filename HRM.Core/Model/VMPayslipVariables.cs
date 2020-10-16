using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMPayslipVariables
    {
        public Int32? PayrollCycleId { get; set; }
        public Int32? UserId { get; set; }
        public Int32? DepartmentId { get; set; }
        public string PayrollId { get; set; }
        public string IsLate { get; set; }
        public string IsEarly { get; set; }
        public List<VMPayslipVariableInformation> VMPayslipVariableInformationList { get; set; }
        public List<VMPayslipAbsentInformation> VMPayslipAbsentInformationList { get; set; }
    }
}
