using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IAttendanceTypeRepositoryBase
	{
        
        Dictionary<string, string> GetAttendanceTypeBasicSearchColumns();
        List<SearchColumn> GetAttendanceTypeSearchColumns();
        List<SearchColumn> GetAttendanceTypeAdvanceSearchColumns();
        

		AttendanceType GetAttendanceType(System.Int32 Id,string SelectClause=null);
		AttendanceType UpdateAttendanceType(AttendanceType entity);
		AttendanceType UpdateAttendanceTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteAttendanceType(System.Int32 Id);
		AttendanceType DeleteAttendanceType(AttendanceType entity);
		List<AttendanceType> GetPagedAttendanceType(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<AttendanceType> GetAllAttendanceType(string SelectClause=null);
		AttendanceType InsertAttendanceType(AttendanceType entity);
		List<AttendanceType> GetAttendanceTypeByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
