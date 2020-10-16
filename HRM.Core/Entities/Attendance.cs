
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;
using System.Collections.Generic;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class Attendance : AttendanceBase 
	{
        public User User { get; set; }
        public AttendanceDetail AttendanceDetailTimeIn { get; set; }
        public AttendanceDetail AttendanceDetailTimeOut { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
        public List<AttendanceDetail> AttendanceDetailList { get; set; }
        public string WorkingHours { get; set; }
	}
}
