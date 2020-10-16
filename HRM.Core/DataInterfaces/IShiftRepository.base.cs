using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IShiftRepositoryBase
	{
        
        Dictionary<string, string> GetShiftBasicSearchColumns();
        List<SearchColumn> GetShiftSearchColumns();
        List<SearchColumn> GetShiftAdvanceSearchColumns();
        

		Shift GetShift(System.Int32 Id,string SelectClause=null);
		Shift UpdateShift(Shift entity);
		Shift UpdateShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteShift(System.Int32 Id);
		Shift DeleteShift(Shift entity);
		List<Shift> GetPagedShift(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Shift> GetAllShift(string SelectClause=null);
		Shift InsertShift(Shift entity);
		List<Shift> GetShiftByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
