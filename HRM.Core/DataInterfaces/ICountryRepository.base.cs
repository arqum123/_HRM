using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface ICountryRepositoryBase
	{
        
        Dictionary<string, string> GetCountryBasicSearchColumns();
        List<SearchColumn> GetCountrySearchColumns();
        List<SearchColumn> GetCountryAdvanceSearchColumns();
        

		Country GetCountry(System.Int32 Id,string SelectClause=null);
		Country UpdateCountry(Country entity);
		Country UpdateCountryByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteCountry(System.Int32 Id);
		Country DeleteCountry(Country entity);
		List<Country> GetPagedCountry(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Country> GetAllCountry(string SelectClause=null);
		Country InsertCountry(Country entity);
		List<Country> GetCountryByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
