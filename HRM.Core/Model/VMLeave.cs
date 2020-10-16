using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMLeave
    {
        public List<Leave> LeaveHistory { get; set; }
        public Leave NewLeave { get; set; }
    }
}
