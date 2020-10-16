
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class AttendanceStatus : AttendanceStatusBase 
	{
        public string Status
        {
            get
            {
                if (IsShiftOffDay.HasValue && IsShiftOffDay.Value)
                    return "OFF";
                else if (IsLeaveDay.HasValue && IsLeaveDay.Value)
                    return "LEV";
                else return "P";
            }
        }
	
		
	}
}
