using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMUserPayrollEdit
    {
        public System.Int32? PayrollId { get; set; }
        public System.Int32? UserId { get; set; }
        public System.Int32? DepartmentId { get; set; }
        public System.Int32? SalaryTypeId { get; set; }
        public System.Int32? PayrollCycleId { get; set; }
        public System.String UserFName { get; set; }
        public System.String UserMName { get; set; }
        public System.String UserLName { get; set; }
        public System.String DepartmentName { get; set; }
        public System.String SalaryTypeName { get; set; }
        public System.String Designation { get; set; }
        public System.String NICNo { get; set; }
        public System.String PayrollCycleName { get; set; }
        public System.String PayrollCycleMonth { get; set; }
        public System.String PayrollCycleYear { get; set; }
        public System.Decimal? Salary { get; set; }
        public System.Decimal? Addition { get; set; }
        public System.Decimal? Deduction { get; set; }
        public System.Decimal? NetSalary { get; set; }
        public List<VMUserPayrollDetailEdit> VMUserPayrollDetailEditList { get; set; }

    }
}

