using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMModifyPayrollEdit
    {
        public int? PayrollCycleId { get; set; }
        public int? PayrollId { get; set; }
        public int? PayrollDetailId { get; set; }
        public int? PayrollVariableId { get; set; }
        public int? UserId { get; set; }
        public int? SalaryTypeId { get; set; }
        public int? DepartmentId { get; set; }
        public string UserName { get; set; }
        public string UserFName { get; set; }
        public string UserMName { get; set; }
        public string UserLName { get; set; }

        public string DepartmentName { get; set; }
        public string PayrollDetailName { get; set; }
        public string PayrollVariableName { get; set; }
        public string Designation { get; set; }
        public string NICNo { get; set; }
        public string SalaryTypeName { get; set; }
        public string PayrollCycleName { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Addition { get; set; }
        public decimal? Deduction { get; set; }
        public decimal? ExtraAddition {get;set;}
        public decimal? ExtraDeduction { get; set; }
        public decimal? NetSalary { get; set; }
    }
}
