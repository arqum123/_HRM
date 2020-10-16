using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IUserContactRepositoryBase
	{
        
        Dictionary<string, string> GetUserContactBasicSearchColumns();
        List<SearchColumn> GetUserContactSearchColumns();
        List<SearchColumn> GetUserContactAdvanceSearchColumns();
        

		List<UserContact> GetUserContactByUserId(System.Int32? UserId,string SelectClause=null);
		List<UserContact> GetUserContactByContactTypeId(System.Int32? ContactTypeId,string SelectClause=null);
		UserContact GetUserContact(System.Int32 Id,string SelectClause=null);
		UserContact UpdateUserContact(UserContact entity);
		UserContact UpdateUserContactByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUserContact(System.Int32 Id);
		UserContact DeleteUserContact(UserContact entity);
		List<UserContact> GetPagedUserContact(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<UserContact> GetAllUserContact(string SelectClause=null);
		UserContact InsertUserContact(UserContact entity);
		List<UserContact> GetUserContactByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
