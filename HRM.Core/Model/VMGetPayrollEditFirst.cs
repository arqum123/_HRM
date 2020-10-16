using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMGetPayrollEditFirst
    {
        public System.Int32? PayrollCycleId { get; set; }
        public System.Int32? UserId { get; set; }
        public System.Int32? DepartmentId { get; set; }
        public System.String UserName { get; set; }
        public List<VMGetPayrollEditSecond> VMGetPayrollEditSecondList { get; set; }
        public List<Payroll> PayrollList { get; set; }
        public List<VMModifyPayrollEdit> VMModifyPayrollEditList { get; set; }
        public List<VMModifyPayrollEditVariable> VMModifyPayrollEditVariableList { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
