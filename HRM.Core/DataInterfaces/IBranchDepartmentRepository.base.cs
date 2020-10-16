using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IBranchDepartmentRepositoryBase
	{
        
        Dictionary<string, string> GetBranchDepartmentBasicSearchColumns();
        List<SearchColumn> GetBranchDepartmentSearchColumns();
        List<SearchColumn> GetBranchDepartmentAdvanceSearchColumns();
        

		List<BranchDepartment> GetBranchDepartmentByBranchId(System.Int32? BranchId,string SelectClause=null);
		List<BranchDepartment> GetBranchDepartmentByDepartmentId(System.Int32? DepartmentId,string SelectClause=null);
		BranchDepartment GetBranchDepartment(System.Int32 Id,string SelectClause=null);
		BranchDepartment UpdateBranchDepartment(BranchDepartment entity);
		BranchDepartment UpdateBranchDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteBranchDepartment(System.Int32 Id);
		BranchDepartment DeleteBranchDepartment(BranchDepartment entity);
		List<BranchDepartment> GetPagedBranchDepartment(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<BranchDepartment> GetAllBranchDepartment(string SelectClause=null);
		BranchDepartment InsertBranchDepartment(BranchDepartment entity);
		List<BranchDepartment> GetBranchDepartmentByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
