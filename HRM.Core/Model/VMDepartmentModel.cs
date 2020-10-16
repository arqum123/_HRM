using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Model
{
    public class VMDepartmentModel
    {
        public List<Department> DepartmentList { get; set; }

        public List<UserDepartment> UserDepartmentList { get; set; }
    }
}
