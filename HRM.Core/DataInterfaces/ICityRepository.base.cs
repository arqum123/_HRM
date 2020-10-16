using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface ICityRepositoryBase
	{
        
        Dictionary<string, string> GetCityBasicSearchColumns();
        List<SearchColumn> GetCitySearchColumns();
        List<SearchColumn> GetCityAdvanceSearchColumns();
        

		List<City> GetCityByStateId(System.Int32? StateId,string SelectClause=null);
		City GetCity(System.Int32 Id,string SelectClause=null);
		City UpdateCity(City entity);
		City UpdateCityByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteCity(System.Int32 Id);
		City DeleteCity(City entity);
		List<City> GetPagedCity(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<City> GetAllCity(string SelectClause=null);
		City InsertCity(City entity);
		List<City> GetCityByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
