using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IBranchShiftRepositoryBase
	{
        
        Dictionary<string, string> GetBranchShiftBasicSearchColumns();
        List<SearchColumn> GetBranchShiftSearchColumns();
        List<SearchColumn> GetBranchShiftAdvanceSearchColumns();
        

		List<BranchShift> GetBranchShiftByBranchId(System.Int32? BranchId,string SelectClause=null);
		List<BranchShift> GetBranchShiftByShiftId(System.Int32? ShiftId,string SelectClause=null);
		BranchShift GetBranchShift(System.Int32 Id,string SelectClause=null);
		BranchShift UpdateBranchShift(BranchShift entity);
		BranchShift UpdateBranchShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteBranchShift(System.Int32 Id);
		BranchShift DeleteBranchShift(BranchShift entity);
		List<BranchShift> GetPagedBranchShift(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<BranchShift> GetAllBranchShift(string SelectClause=null);
		BranchShift InsertBranchShift(BranchShift entity);
		List<BranchShift> GetBranchShiftByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
