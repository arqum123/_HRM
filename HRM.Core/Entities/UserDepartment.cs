
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class UserDepartment : UserDepartmentBase 
	{
        [Required(ErrorMessage = "Please Select Department")]
        [FieldNameAttribute("DepartmentID", true, true, 4)]
        [DataMember(EmitDefaultValue = false, IsRequired = true)]
        public override System.Int32? DepartmentId { get; set; }

        public Department Department { get; set; }
	}
}
