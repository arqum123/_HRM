
using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;


namespace HRM.Core.Entities
{
    [DataContract]
	public partial class Device : DeviceBase 
	{
        public string BranchName { get; set; }
        public string DeviceModalName { get; set; }
	}
}
