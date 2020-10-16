
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class Shift : ShiftBase 
	{
        [Required]
        [FieldNameAttribute("Name", true, false, 200)]
        [DataMember(EmitDefaultValue = false, IsRequired = true)]
        public override System.String Name { get; set; }

        //[Required]
        //[FieldNameAttribute("StartHour", true, false, 20)]
        //[DataMember(EmitDefaultValue = false, IsRequired = true)]
        //public override System.String StartHour { get; set; }

        //[Required]
        //[FieldNameAttribute("EndHour", true, false, 20)]
        //[DataMember(EmitDefaultValue = false, IsRequired = true)]
        //public override System.String EndHour { get; set; }

        [Required(ErrorMessage = "Start Hour is required")]
        public string StartHourOnly { get; set; }

        [Required(ErrorMessage = "Start Minute is required")]
        public string StartMinuteOnly { get; set; }

        [Required(ErrorMessage = "End Hour is required")]
        public string EndHourOnly { get; set; }

        [Required(ErrorMessage = "End Minute is required")]
        public string EndMinuteOnly { get; set; }

        [Required(ErrorMessage = "Break Hour is required")]
        public string BreakHourOnly { get; set; }

        [Required(ErrorMessage = "Break Minute is required")]
        public string BreakMinuteOnly { get; set; }

        public List<ShiftOffDay> ShiftOffDayList { get; set; }
        public List<AttendancePolicy> AttendancePolicyList { get; set; }
        public string OffDays { get; set; }
        public string Policies { get; set; }
	}
}
