using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMModifyPayrollEdit
    {
       

        public Int32? UserId { get; set; } //User
        public Int32? PayrollId { get; set; } ////User
        //public Int32? PayrollDetailId { get; set; } //New
        public string UserName { get; set; } //User
        public string UserFName { get; set; } //User
        public string UserMName { get; set; } //User
        public string UserLName { get; set; } //User

        public string DepartmentName { get; set; } //Department       //New
        public Decimal? Addition { get; set; } //Payroll
        public Decimal? Deduction { get; set; } //Payroll

        public string Designation { get; set; } //User
        public Int32? PayrollCycleId { get; set; } //PayrollCycle
        public string PayrollCycleName { get; set; } //PayrollCycle
        public Int32? DepartmentId { get; set; } //Department
        public Decimal? Salary { get; set; } //User
        public Int32? SalaryTypeId { get; set; } //SalaryType
        public Decimal? NetSalary { get; set; } //Payroll
        public List<VMModifyPayrollEditVariable> VMModifyPayrollEditVariableList { get; set; }
    }

}
