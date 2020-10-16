using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.Model;

namespace HRM.Core.DataInterfaces
{
		
	public interface IPayrollRepositoryBase
	{
        
        Dictionary<string, string> GetPayrollBasicSearchColumns();
        List<SearchColumn> GetPayrollSearchColumns();
        List<SearchColumn> GetPayrollAdvanceSearchColumns();
        

		List<Payroll> GetPayrollByPayrollCycleId(System.Int32? PayrollCycleId,string SelectClause=null);
		Payroll GetPayroll(System.Int32 Id,string SelectClause=null);
		Payroll UpdatePayroll(Payroll entity);
		Payroll UpdatePayrollByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayroll(System.Int32 Id);
		Payroll DeletePayroll(Payroll entity);
		List<Payroll> GetPagedPayroll(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Payroll> GetAllPayroll(string SelectClause=null);
		Payroll InsertPayroll(Payroll entity);
        List<Payroll> GetPayrollByKeyValue(string Key, string Value, Operands operand, string SelectClause = null);

    }
	
}
