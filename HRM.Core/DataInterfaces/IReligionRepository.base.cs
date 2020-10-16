using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IReligionRepositoryBase
	{
        
        Dictionary<string, string> GetReligionBasicSearchColumns();
        List<SearchColumn> GetReligionSearchColumns();
        List<SearchColumn> GetReligionAdvanceSearchColumns();
        

		Religion GetReligion(System.Int32 Id,string SelectClause=null);
		Religion UpdateReligion(Religion entity);
		Religion UpdateReligionByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteReligion(System.Int32 Id);
		Religion DeleteReligion(Religion entity);
		List<Religion> GetPagedReligion(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Religion> GetAllReligion(string SelectClause=null);
		Religion InsertReligion(Religion entity);
		List<Religion> GetReligionByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
