using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IPayrollPolicyRepositoryBase
	{
        
        Dictionary<string, string> GetPayrollPolicyBasicSearchColumns();
        List<SearchColumn> GetPayrollPolicySearchColumns();
        List<SearchColumn> GetPayrollPolicyAdvanceSearchColumns();
        

		List<PayrollPolicy> GetPayrollPolicyByPayrollVariableId(System.Int32? PayrollVariableId,string SelectClause=null);
		PayrollPolicy GetPayrollPolicy(System.Int32 Id,string SelectClause=null);
		PayrollPolicy UpdatePayrollPolicy(PayrollPolicy entity);
		PayrollPolicy UpdatePayrollPolicyByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayrollPolicy(System.Int32 Id);
		PayrollPolicy DeletePayrollPolicy(PayrollPolicy entity);
		List<PayrollPolicy> GetPagedPayrollPolicy(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<PayrollPolicy> GetAllPayrollPolicy(string SelectClause=null);
		PayrollPolicy InsertPayrollPolicy(PayrollPolicy entity);
		List<PayrollPolicy> GetPayrollPolicyByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
