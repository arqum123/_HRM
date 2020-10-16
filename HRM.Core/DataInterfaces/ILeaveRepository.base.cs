using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface ILeaveRepositoryBase
	{
        
        Dictionary<string, string> GetLeaveBasicSearchColumns();
        List<SearchColumn> GetLeaveSearchColumns();
        List<SearchColumn> GetLeaveAdvanceSearchColumns();
        

		List<Leave> GetLeaveByUserId(System.Int32? UserId,string SelectClause=null);
		List<Leave> GetLeaveByLeaveTypeId(System.Int32? LeaveTypeId,string SelectClause=null);
		Leave GetLeave(System.Int32 Id,string SelectClause=null);
		Leave UpdateLeave(Leave entity);
		Leave UpdateLeaveByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteLeave(System.Int32 Id);
		Leave DeleteLeave(Leave entity);
		List<Leave> GetPagedLeave(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Leave> GetAllLeave(string SelectClause=null);
		Leave InsertLeave(Leave entity);
		List<Leave> GetLeaveByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
