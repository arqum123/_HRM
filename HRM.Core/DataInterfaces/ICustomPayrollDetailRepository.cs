using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.DataInterfaces
{
    public interface ICustomPayrollDetailRepository : ICustomPayrollDetailRepositoryBase
    {

        List<CustomPayrollDetail> GetCustomPayrollDetailByPayrollId(System.Int32? PayrollId, string SelectClause = null);

        List<CustomPayrollDetail> GetCustomPayrollDetailAmountByPayrollId(System.Int32? PayrollId, string SelectClause = null);
    }
}
