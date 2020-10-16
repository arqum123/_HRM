using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMPayslipDetailPayroll
    {
        public int PayrollCycleId { get; set; }
        public int PayrollId { get; set; }
        public double Amount { get; set; }
        public double Salary { get; set; }
        public double NetSalary { get; set; }
        public double TotalAddition { get; set; }
        public double TotalDeduction { get; set; }

        public List<VMPayslipDetailVariables> VMPayslipDetailVariablesList { get; set; }
    }
}
