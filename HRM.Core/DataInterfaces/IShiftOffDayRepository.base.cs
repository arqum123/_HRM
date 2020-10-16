using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IShiftOffDayRepositoryBase
	{
        
        Dictionary<string, string> GetShiftOffDayBasicSearchColumns();
        List<SearchColumn> GetShiftOffDaySearchColumns();
        List<SearchColumn> GetShiftOffDayAdvanceSearchColumns();
        

		List<ShiftOffDay> GetShiftOffDayByShiftId(System.Int32? ShiftId,string SelectClause=null);
		ShiftOffDay GetShiftOffDay(System.Int32 Id,string SelectClause=null);
		ShiftOffDay UpdateShiftOffDay(ShiftOffDay entity);
		ShiftOffDay UpdateShiftOffDayByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteShiftOffDay(System.Int32 Id);
		ShiftOffDay DeleteShiftOffDay(ShiftOffDay entity);
		List<ShiftOffDay> GetPagedShiftOffDay(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<ShiftOffDay> GetAllShiftOffDay(string SelectClause=null);
		ShiftOffDay InsertShiftOffDay(ShiftOffDay entity);
		List<ShiftOffDay> GetShiftOffDayByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
