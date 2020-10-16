using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IPayrollDetailRepository: IPayrollDetailRepositoryBase
	{

        List<PayrollDetail> GetPayrollDetailByPayrollId(System.Int32? PayrollId, string SelectClause = null);

        List<PayrollDetail> GetPayrollDetailAmountByPayrollId(System.Int32? PayrollId, string SelectClause = null);
    }


}
