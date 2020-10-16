
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class Branch : BranchBase 
	{
        [Required]
        [FieldNameAttribute("Name", true, false, 500)]
        [DataMember(EmitDefaultValue = false)]
        public override System.String Name { get; set; }

        public string currentURL { get; set; }
	}
}
