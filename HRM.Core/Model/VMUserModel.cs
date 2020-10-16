using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using HRM.Core.Entities;

namespace HRM.Core.Model
{
    public class VMUserModel
    {
        public User User { get; set; }
        public UserDepartment UserDepartment { get; set; }
        public UserContact UserContactEmail { get; set; }
        public UserContact UserContactMobile { get; set; }
        public UserContact UserContactAlternateMobile { get; set; }

        public System.Int32? DepartmentID { get; set; }

        public List<User> UserList { get; set; }

        public System.Int32? ShiftId { get; set; }

        public string UserName { get; set; }
    }
}
