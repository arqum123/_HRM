
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class Department : DepartmentBase 
	{
        [Required(ErrorMessage = "Please Input Department Name")]
        [FieldNameAttribute("Name", true, false, 200)]
        [DataMember(EmitDefaultValue = false, IsRequired = true)]
        public override System.String Name { get; set; }

        public int TotalUser { get; set; }
        public int PresentUser { get; set; }
        public int OffDayUser { get; set; }
        public int LateUser { get; set; }
        public int EarlyUser { get; set; }
        public int OverTimeUser { get; set; }
        public int QuarterDayUser { get; set; }
        public int HalfDayUser { get; set; }
        public int FullDayUser { get; set; }

        public decimal TotalSalary { get; set; }
        public decimal TotalAddition { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetSalary { get; set; }
	}
}
