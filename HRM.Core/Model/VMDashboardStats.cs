using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMDashboardStats
    {
        public string dtAttendanceString { get; set; }
        public int PresentCount { get; set; }
        public decimal PresentPercentage { get; set; }
        public int OnTimeCount { get; set; }
        public decimal OnTimePercentage { get; set; }
        public int LateCount { get; set; }
        public decimal LatePercentage { get; set; }
    }
}
