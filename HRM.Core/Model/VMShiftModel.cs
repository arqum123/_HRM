using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMShiftModel
    {
        public Shift Shift { get; set; }
        public List<ShiftOffDay> ShiftOffDayList { get; set; }
        
        public List<DaysOfWeek> DaysOfWeekList { get; set; }

        public List<Shift> ShiftList { get; set; }

        public List<AttendanceVariable> AttendanceVariableList { get; set; }

        public int ShiftId { get; set; }

        public List<UserShift> UserShiftList { get; set; }
    }

    public class DaysOfWeek
    {
        public bool IsSelected { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
