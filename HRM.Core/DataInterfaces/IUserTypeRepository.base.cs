using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IUserTypeRepositoryBase
	{
        
        Dictionary<string, string> GetUserTypeBasicSearchColumns();
        List<SearchColumn> GetUserTypeSearchColumns();
        List<SearchColumn> GetUserTypeAdvanceSearchColumns();
        

		UserType GetUserType(System.Int32 Id,string SelectClause=null);
		UserType UpdateUserType(UserType entity);
		UserType UpdateUserTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUserType(System.Int32 Id);
		UserType DeleteUserType(UserType entity);
		List<UserType> GetPagedUserType(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<UserType> GetAllUserType(string SelectClause=null);
		UserType InsertUserType(UserType entity);
		List<UserType> GetUserTypeByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
