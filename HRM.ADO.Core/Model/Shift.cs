using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace HRM.ADO.Core.Model
{
    public class Shift
    {
        [Required]
        [Display(Name = "Shift Name")]
        public string Name { get; set; }

        [Display(Name = "Shift Description")]
        public string Description { get; set; }
        
        public string UserIP { get; set; }
        public decimal BreakHour { get; internal set; }
    }
}
