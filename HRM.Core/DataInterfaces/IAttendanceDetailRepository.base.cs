using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IAttendanceDetailRepositoryBase
	{
        
        Dictionary<string, string> GetAttendanceDetailBasicSearchColumns();
        List<SearchColumn> GetAttendanceDetailSearchColumns();
        List<SearchColumn> GetAttendanceDetailAdvanceSearchColumns();
        

		List<AttendanceDetail> GetAttendanceDetailByAttendanceId(System.Int32? AttendanceId,string SelectClause=null);
		List<AttendanceDetail> GetAttendanceDetailByAttendanceTypeId(System.Int32? AttendanceTypeId,string SelectClause=null);
		AttendanceDetail GetAttendanceDetail(System.Int32 Id,string SelectClause=null);
		AttendanceDetail UpdateAttendanceDetail(AttendanceDetail entity);
		AttendanceDetail UpdateAttendanceDetailByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendanceDetail(System.Int32 Id);
		AttendanceDetail DeleteAttendanceDetail(AttendanceDetail entity);
		List<AttendanceDetail> GetPagedAttendanceDetail(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<AttendanceDetail> GetAllAttendanceDetail(string SelectClause=null);
		AttendanceDetail InsertAttendanceDetail(AttendanceDetail entity);
		List<AttendanceDetail> GetAttendanceDetailByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
