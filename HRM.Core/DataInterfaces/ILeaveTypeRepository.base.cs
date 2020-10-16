using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface ILeaveTypeRepositoryBase
	{
        
        Dictionary<string, string> GetLeaveTypeBasicSearchColumns();
        List<SearchColumn> GetLeaveTypeSearchColumns();
        List<SearchColumn> GetLeaveTypeAdvanceSearchColumns();
        

		LeaveType GetLeaveType(System.Int32 Id,string SelectClause=null);
		LeaveType UpdateLeaveType(LeaveType entity);
		LeaveType UpdateLeaveTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteLeaveType(System.Int32 Id);
		LeaveType DeleteLeaveType(LeaveType entity);
		List<LeaveType> GetPagedLeaveType(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<LeaveType> GetAllLeaveType(string SelectClause=null);
		LeaveType InsertLeaveType(LeaveType entity);
		List<LeaveType> GetLeaveTypeByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
