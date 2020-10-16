using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public class VMEmpLeave : LeaveBase
    {
        public List<Leave> LeaveHistory { get; set; }
        public List<Leave> PreviousLeaveHistory { get; set; }
        public List<Leave> CurrentLeaveHistory { get; set; }
        public List<Leave> UpcomingLeaveHistory { get; set; }
        public Leave NewLeave { get; set; }

    }
}
