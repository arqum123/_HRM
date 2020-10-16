using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IPayrollCycleRepositoryBase
	{
        
        Dictionary<string, string> GetPayrollCycleBasicSearchColumns();
        List<SearchColumn> GetPayrollCycleSearchColumns();
        List<SearchColumn> GetPayrollCycleAdvanceSearchColumns();
        

		PayrollCycle GetPayrollCycle(System.Int32 Id,string SelectClause=null);
		PayrollCycle UpdatePayrollCycle(PayrollCycle entity);
		PayrollCycle UpdatePayrollCycleByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayrollCycle(System.Int32 Id);

		PayrollCycle DeletePayrollCycle(PayrollCycle entity);
		List<PayrollCycle> GetPagedPayrollCycle(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<PayrollCycle> GetAllPayrollCycle(string SelectClause=null);
		PayrollCycle InsertPayrollCycle(PayrollCycle entity);
		List<PayrollCycle> GetPayrollCycleByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
