using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IBranchRepositoryBase
	{
        
        Dictionary<string, string> GetBranchBasicSearchColumns();
        List<SearchColumn> GetBranchSearchColumns();
        List<SearchColumn> GetBranchAdvanceSearchColumns();
        

		Branch GetBranch(System.Int32 Id,string SelectClause=null);
		Branch UpdateBranch(Branch entity);
		Branch UpdateBranchByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteBranch(System.Int32 Id);
		Branch DeleteBranch(Branch entity);
		List<Branch> GetPagedBranch(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Branch> GetAllBranch(string SelectClause=null);
		Branch InsertBranch(Branch entity);
		List<Branch> GetBranchByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
