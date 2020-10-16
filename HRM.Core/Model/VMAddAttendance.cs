using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMAddAttendance
    {

        [Required(ErrorMessage = "Please Select User")]        
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please input Addendance Date")]
        public DateTime AttendanceDate { get; set; }

        [Required(ErrorMessage = "Please input Time In")]
        [FieldNameAttribute("TimeIn", false, false, 5)]
        public string TimeIn { get; set; }

        [Required(ErrorMessage = "Please input Time Out")]
        [FieldNameAttribute("TimeOut", false, false, 5)]
        public string TimeOut { get; set; }

        //[Required(ErrorMessage = "Please input Time In")]
        //[FieldNameAttribute("TimeIn", false, false, 5)]
        public DateTime? DateTimeIn { get; set; }

        //[Required(ErrorMessage = "Please input Time Out")]
        //[FieldNameAttribute("TimeOut", false, false, 5)]
        public DateTime? DateTimeOut { get; set; }

        public string BreakType { get; set; }
    }
}
