using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IAttendancePolicyRepositoryBase
	{
        
        Dictionary<string, string> GetAttendancePolicyBasicSearchColumns();
        List<SearchColumn> GetAttendancePolicySearchColumns();
        List<SearchColumn> GetAttendancePolicyAdvanceSearchColumns();
        

		List<AttendancePolicy> GetAttendancePolicyByShiftId(System.Int32? ShiftId,string SelectClause=null);
		List<AttendancePolicy> GetAttendancePolicyByAttendanceVariableId(System.Int32? AttendanceVariableId,string SelectClause=null);
		AttendancePolicy GetAttendancePolicy(System.Int32 Id,string SelectClause=null);
		AttendancePolicy UpdateAttendancePolicy(AttendancePolicy entity);
		AttendancePolicy UpdateAttendancePolicyByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendancePolicy(System.Int32 Id);
		AttendancePolicy DeleteAttendancePolicy(AttendancePolicy entity);
		List<AttendancePolicy> GetPagedAttendancePolicy(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<AttendancePolicy> GetAllAttendancePolicy(string SelectClause=null);
		AttendancePolicy InsertAttendancePolicy(AttendancePolicy entity);
		List<AttendancePolicy> GetAttendancePolicyByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
