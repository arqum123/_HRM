﻿
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class UserShift : UserShiftBase 
	{
        public Shift Shift { get; set; }
	}
}
