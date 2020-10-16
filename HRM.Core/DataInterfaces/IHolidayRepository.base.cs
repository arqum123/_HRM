using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IHolidayRepositoryBase
	{
        
        Dictionary<string, string> GetHolidayBasicSearchColumns();
        List<SearchColumn> GetHolidaySearchColumns();
        List<SearchColumn> GetHolidayAdvanceSearchColumns();
        

		Holiday GetHoliday(System.Int32 Id,string SelectClause=null);
		Holiday UpdateHoliday(Holiday entity);
		Holiday UpdateHolidayByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteHoliday(System.Int32 Id);
		Holiday DeleteHoliday(Holiday entity);
		List<Holiday> GetPagedHoliday(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Holiday> GetAllHoliday(string SelectClause=null);
		Holiday InsertHoliday(Holiday entity);
		List<Holiday> GetHolidayByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
