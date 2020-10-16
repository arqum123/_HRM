using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Entities
{
    public class VMADDPayrollModel
    {
        //New Payroll Model
        public Boolean IsDeduction { get; set; }
        public Boolean IsAddition { get; set; }
        public Boolean IsActive { get; set; }
        public int? SalaryTypeId { get; set; } //for dropdown
        public int? Id { get; set; }
        public int? PayrollVariableId { get; set; }
        public string PayrollVariableName { get; set; }

        public string Description { get; set; }
        public string IsCheck { get; set; }
        public Boolean? IsUnit { get; set; }
        public Boolean? IsPercentage { get; set; }
        public Boolean? IsDay { get; set; }
        public int? Value { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? RetiredDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? UpdateBy { get; set; }
        public int? UserIp { get; set; }
        public Boolean? IsAttendace { get; set; }
        public int? SalaryType { get; set; }
        public int? Occurance { get; set; }
    }
}
