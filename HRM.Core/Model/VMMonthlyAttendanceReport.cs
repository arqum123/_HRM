using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMMonthlyAttendanceReport
    {
        public int Id { get; set; }        
        public string FirstName { get; set; }
        public string Designation { get; set; }
        public string DepartmentName { get; set; }
        public string SalaryTypeName { get; set; }
        public string BranchName { get; set; }
        public string ShiftName { get; set; }
        public int TotalCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int OffDayCount { get; set; }
        public int LateCount { get; set; }
        public int EarlyCount { get; set; }
        public int OverTimeCount { get; set; }
        public int FullDayCount { get; set; }
        public string PicturePath { get; set; }
        public byte[] Picture { get; set; }
    }

}
