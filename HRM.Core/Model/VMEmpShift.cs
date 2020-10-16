using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMEmpShift
    {

        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string BreakHour { get; set; }
        public int? ShiftOffDays { get; set; }
        public string ShiftOffDaysInWords { get; set; }
        public List<VMEmpShiftOffDays> VMEmpShiftOffDaysList { get; set; }
    }
}
