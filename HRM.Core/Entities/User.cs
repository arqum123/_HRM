
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class User : UserBase 
	{
        [Required(ErrorMessage = "Please Input User Name")]
        [FieldNameAttribute("FirstName", true, false, 200)]
        [DataMember(EmitDefaultValue = false, IsRequired = true)]
        public override System.String FirstName { get; set; }

        public Branch Branch { get; set; }
        public UserDepartment UserDepartment { get; set; }
        public SalaryType SalaryType { get; set; }
        public Department Department { get; set; }

        public UserContact UserContactEmail { get; set; }
        public UserContact UserContactMobile { get; set; }
        public UserContact UserContactAlternateMobile { get; set; }

        public UserShift UserShift { get; set; }
        public Shift Shift { get; set; }
        public bool IsSelected { get; set; }

        public int OffDayCount { get; set; }
        public int LateCount { get; set; }
        public int QuarterDayCount { get; set; }
        public int HalfDayCount { get; set; }
        public int FullDayCount { get; set; }
        public int LateMinutes { get; set; }
        public int EarlyMinutes { get; set; }
        public int OverTimeMinutes { get; set; }
        public int WorkingMinutes { get; set; }
        public int EarlyCount { get; set; }
        public int TotalCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int OverTimeCount { get; set; }

        //[FieldNameAttribute("LoginID", true, false, 50)]
        //[DataMember(EmitDefaultValue = false)]
        //[Remote("doesLoginIDExist", "User", HttpMethod = "POST", ErrorMessage = "This Login ID is already assigned to another user")]
        //public override System.String LoginId { get; set; }
	}
}
