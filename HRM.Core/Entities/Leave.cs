
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.Generic;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class Leave : LeaveBase 
	{
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Boolean? IsApproved { get; set; }


    }
}
