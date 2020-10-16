using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class PracticeVMMonthlyReport
    {

        public DateTime StartDate { get; set; } //Input


        public DateTime EndDate { get; set; } //Input
      
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PageNumber { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }


        public List<PracticeVMReport> PracticeVMReportList { get; set; }

        public List<PracticeVMDetailReport> PracticeVMDetailReportList { get; set; }
    }
}
