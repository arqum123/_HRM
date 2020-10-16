using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IContactTypeRepositoryBase
	{
        
        Dictionary<string, string> GetContactTypeBasicSearchColumns();
        List<SearchColumn> GetContactTypeSearchColumns();
        List<SearchColumn> GetContactTypeAdvanceSearchColumns();
        

		ContactType GetContactType(System.Int32 Id,string SelectClause=null);
		ContactType UpdateContactType(ContactType entity);
		ContactType UpdateContactTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteContactType(System.Int32 Id);
		ContactType DeleteContactType(ContactType entity);
		List<ContactType> GetPagedContactType(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<ContactType> GetAllContactType(string SelectClause=null);
		ContactType InsertContactType(ContactType entity);
		List<ContactType> GetContactTypeByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
