using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IDepartmentRepositoryBase
	{
        
        Dictionary<string, string> GetDepartmentBasicSearchColumns();
        List<SearchColumn> GetDepartmentSearchColumns();
        List<SearchColumn> GetDepartmentAdvanceSearchColumns();
        

		Department GetDepartment(System.Int32 Id,string SelectClause=null);
		Department UpdateDepartment(Department entity);
		Department UpdateDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteDepartment(System.Int32 Id);
		Department DeleteDepartment(Department entity);
		List<Department> GetPagedDepartment(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Department> GetAllDepartment(string SelectClause=null);
		Department InsertDepartment(Department entity);
		List<Department> GetDepartmentByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
