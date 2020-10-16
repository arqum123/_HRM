using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMGetPayrollEditSecond
    {
        public Int32? PayrollId { get; set; }
        public Int32? UserId { get; set; }
        public Int32? PayrollCycleId { get; set; }
        public Int32? DepartmentId { get; set; }
        public Int32? SalaryTypeId { get; set; }
        public System.String UserName { get; set; }
        public System.String UserFName { get; set; }
        public System.String UserMName { get; set; }
        public System.String UserLName { get; set; }
        public System.String DepartmentName { get; set; }
        public System.String Designation { get; set; }
        public System.String NICNo { get; set; }
        public System.String SalaryTypeName { get; set; }
        public System.Decimal? NetSalary { get; set; }
        public System.Decimal? Salary { get; set; }
        public System.Decimal? Addition { get; set; }
        public System.Decimal? Deduction { get; set; }
    }
}
