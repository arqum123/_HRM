﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
   public class VMViewPendingLeaves
    {
        public int? UserID { get; set; }
        public int? DepartmentID { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<VMPendingViewAllLeaves> VMPendingViewAllLeavesList { get; set; }
    }
}
