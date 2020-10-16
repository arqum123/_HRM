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

        System.Data.DataSet GetModifyPayrollByPayrollCycleId(System.Int32? PayrollCycleId, System.Int32? DepartmentId, System.String UserName, string SelectClause = null); //NewModifyPayroll
        System.Data.DataSet GetModifyPayslipEdit(System.Int32? PayrollId, System.Int32? DepartmentId, System.Int32? UserId); //NewModifyPayroll
        List<VMModifyPayrollEdit> GetModifyPayslipDetailEdit(System.Int32? PayrollId, System.Int32? PayrollVariableId, System.Decimal? Addition, System.Decimal? Deduction); //NewModifyPayroll
        System.Data.DataSet GetPayslipDetail(System.Int32? PayrollCycleId,  System.Int32? UserId); //NewPayslip

        System.Data.DataSet GetEmpPayslipDetail(System.Int32? PayrollCycleId, System.Int32? UserId); //NewPayslip

        System.Data.DataSet GetEmpPayrollDetail(System.Int32? PayrollCycleId,System.Int32? UserId); //NewPayslip
        System.Data.DataSet GetPendingTicketsDetail(); //NewModifyPayroll
        System.Data.DataSet GetTicketsByDateRangeDetail(DateTime StartDate,DateTime EndDate,int? UserId,int? DepartmentId); //NewModifyPayroll

    }


}
