using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IPayrollVariableRepositoryBase
	{
        
        Dictionary<string, string> GetPayrollVariableBasicSearchColumns();
        List<SearchColumn> GetPayrollVariableSearchColumns();
        List<SearchColumn> GetPayrollVariableAdvanceSearchColumns();
        

		PayrollVariable GetPayrollVariable(System.Int32 Id,string SelectClause=null);
		PayrollVariable UpdatePayrollVariable(PayrollVariable entity);
		PayrollVariable UpdatePayrollVariableByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayrollVariable(System.Int32 Id);
		PayrollVariable DeletePayrollVariable(PayrollVariable entity);
		List<PayrollVariable> GetPagedPayrollVariable(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<PayrollVariable> GetAllPayrollVariable(string SelectClause=null);
		PayrollVariable InsertPayrollVariable(PayrollVariable entity);
		List<PayrollVariable> GetPayrollVariableByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
