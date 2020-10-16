using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.Model;

namespace HRM.Core.DataInterfaces
{
		
	public interface IPayrollRepository: IPayrollRepositoryBase
	{
        void RunPayroll(int PayrollCycleID, int IsFinal);
        List<VMModifyPayrollEdit> GetModifyPayrollByPayrollCycleId(System.Int32? PayrollCycleId, System.Int32? DepartmentId, string UserName, string SelectClause = null); //NewModifyPayroll
        List<VMModifyPayrollEdit> GetModifyPayslipEdit(System.Int32? PayrollId, System.Int32? DepartmentId, System.Int32? UserId); //NewModifyPayroll
    }


}
