using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IAttendanceVariableRepositoryBase
	{
        
        Dictionary<string, string> GetAttendanceVariableBasicSearchColumns();
        List<SearchColumn> GetAttendanceVariableSearchColumns();
        List<SearchColumn> GetAttendanceVariableAdvanceSearchColumns();
        

		AttendanceVariable GetAttendanceVariable(System.Int32 Id,string SelectClause=null);
		AttendanceVariable UpdateAttendanceVariable(AttendanceVariable entity);
		AttendanceVariable UpdateAttendanceVariableByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendanceVariable(System.Int32 Id);
		AttendanceVariable DeleteAttendanceVariable(AttendanceVariable entity);
		List<AttendanceVariable> GetPagedAttendanceVariable(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<AttendanceVariable> GetAllAttendanceVariable(string SelectClause=null);
		AttendanceVariable InsertAttendanceVariable(AttendanceVariable entity);
		List<AttendanceVariable> GetAttendanceVariableByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
