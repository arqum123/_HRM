using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IGenderRepositoryBase
	{
        
        Dictionary<string, string> GetGenderBasicSearchColumns();
        List<SearchColumn> GetGenderSearchColumns();
        List<SearchColumn> GetGenderAdvanceSearchColumns();
        

		Gender GetGender(System.Int32 Id,string SelectClause=null);
		Gender UpdateGender(Gender entity);
		Gender UpdateGenderByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteGender(System.Int32 Id);
		Gender DeleteGender(Gender entity);
		List<Gender> GetPagedGender(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<Gender> GetAllGender(string SelectClause=null);
		Gender InsertGender(Gender entity);
		List<Gender> GetGenderByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
