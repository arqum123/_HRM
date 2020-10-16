using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IAttendanceRepositoryBase
	{
        
        Dictionary<string, string> GetAttendanceBasicSearchColumns();
        List<SearchColumn> GetAttendanceSearchColumns();
        List<SearchColumn> GetAttendanceAdvanceSearchColumns();
        

		List<Attendance> GetAttendanceByUserId(System.Int32? UserId,string SelectClause=null);
		Attendance GetAttendance(System.Int32 Id,string SelectClause=null);
		Attendance UpdateAttendance(Attendance entity);
		Attendance UpdateAttendanceByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendance(System.Int32 Id);
		Attendance DeleteAttendance(Attendance entity);
		List<Attendance> GetPagedAttendance(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Attendance> GetAllAttendance(string SelectClause=null);
		Attendance InsertAttendance(Attendance entity);
		List<Attendance> GetAttendanceByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
