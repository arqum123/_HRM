using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMEmpTicketFilter
    {
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<VMTicketHistory> VMTicketHistoryList { get; set; }
    }
}
