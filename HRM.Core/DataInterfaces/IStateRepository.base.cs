using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IStateRepositoryBase
	{
        
        Dictionary<string, string> GetStateBasicSearchColumns();
        List<SearchColumn> GetStateSearchColumns();
        List<SearchColumn> GetStateAdvanceSearchColumns();
        

		List<State> GetStateByCountryId(System.Int32? CountryId,string SelectClause=null);
		State GetState(System.Int32 Id,string SelectClause=null);
		State UpdateState(State entity);
		State UpdateStateByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteState(System.Int32 Id);
		State DeleteState(State entity);
		List<State> GetPagedState(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<State> GetAllState(string SelectClause=null);
		State InsertState(State entity);
		List<State> GetStateByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
