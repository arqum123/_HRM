using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface ISalaryTypeRepositoryBase
	{
        
        Dictionary<string, string> GetSalaryTypeBasicSearchColumns();
        List<SearchColumn> GetSalaryTypeSearchColumns();
        List<SearchColumn> GetSalaryTypeAdvanceSearchColumns();
        

		SalaryType GetSalaryType(System.Int32 Id,string SelectClause=null);
		SalaryType UpdateSalaryType(SalaryType entity);
		SalaryType UpdateSalaryTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteSalaryType(System.Int32 Id);
		SalaryType DeleteSalaryType(SalaryType entity);
		List<SalaryType> GetPagedSalaryType(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<SalaryType> GetAllSalaryType(string SelectClause=null);
		SalaryType InsertSalaryType(SalaryType entity);
		List<SalaryType> GetSalaryTypeByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
