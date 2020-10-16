using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMAttendanceData
    {
        public int UserId { get; set; }
        public string BreakType { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
