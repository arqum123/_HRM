
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class AttendanceVariable : AttendanceVariableBase 
	{
        public bool IsSelected { get; set; }
        public decimal Hours { get; set; }

        public System.Decimal Minutes { get; set; }
        public string Reason { get; set; }
	}
}
