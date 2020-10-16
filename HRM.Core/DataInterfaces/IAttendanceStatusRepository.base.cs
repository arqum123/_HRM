using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IAttendanceStatusRepositoryBase
	{
        
        Dictionary<string, string> GetAttendanceStatusBasicSearchColumns();
        List<SearchColumn> GetAttendanceStatusSearchColumns();
        List<SearchColumn> GetAttendanceStatusAdvanceSearchColumns();

        ////New
        List<AttendanceStatus> GetAttendanceStatusByDateRangeSharp(DateTime StartDate, DateTime EndDate);

        List<AttendanceStatus> GetAttendanceStatusByAttendanceId(System.Int32? AttendanceId,string SelectClause=null);
		AttendanceStatus GetAttendanceStatus(System.Int32 Id,string SelectClause=null);
		AttendanceStatus UpdateAttendanceStatus(AttendanceStatus entity);
		AttendanceStatus UpdateAttendanceStatusByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendanceStatus(System.Int32 Id);
		AttendanceStatus DeleteAttendanceStatus(AttendanceStatus entity);
		List<AttendanceStatus> GetPagedAttendanceStatus(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<AttendanceStatus> GetAllAttendanceStatus(string SelectClause=null);

        List<AttendanceStatus> GetAllAttendanceStatusStartAndEndDate(string StartDate, string EndDate, string SelectClause = null);

      
        AttendanceStatus InsertAttendanceStatus(AttendanceStatus entity);
		List<AttendanceStatus> GetAttendanceStatusByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
