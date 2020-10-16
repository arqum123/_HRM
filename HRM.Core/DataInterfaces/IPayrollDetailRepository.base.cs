using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IPayrollDetailRepositoryBase
	{
        
        Dictionary<string, string> GetPayrollDetailBasicSearchColumns();
        List<SearchColumn> GetPayrollDetailSearchColumns();
        List<SearchColumn> GetPayrollDetailAdvanceSearchColumns();
		List<PayrollDetail> GetPayrollDetailByPayrollPolicyId(System.Int32? PayrollPolicyId,string SelectClause=null);
		PayrollDetail GetPayrollDetail(System.Int32 Id,string SelectClause=null);
		PayrollDetail UpdatePayrollDetail(PayrollDetail entity);
		PayrollDetail UpdatePayrollDetailByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayrollDetail(System.Int32 Id);
		PayrollDetail DeletePayrollDetail(PayrollDetail entity);
		List<PayrollDetail> GetPagedPayrollDetail(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<PayrollDetail> GetAllPayrollDetail(string SelectClause=null);
		PayrollDetail InsertPayrollDetail(PayrollDetail entity);
		List<PayrollDetail> GetPayrollDetailByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
