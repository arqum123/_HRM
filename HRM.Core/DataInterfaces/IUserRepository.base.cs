using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IUserRepositoryBase
	{
        
        Dictionary<string, string> GetUserBasicSearchColumns();
        List<SearchColumn> GetUserSearchColumns();
        List<SearchColumn> GetUserAdvanceSearchColumns();
        

		List<User> GetUserByUserTypeId(System.Int32? UserTypeId,string SelectClause=null);
		List<User> GetUserByGenderId(System.Int32? GenderId,string SelectClause=null);
		List<User> GetUserByReligionId(System.Int32? ReligionId,string SelectClause=null);
		List<User> GetUserByCountryId(System.Int32? CountryId,string SelectClause=null);
		List<User> GetUserByCityId(System.Int32? CityId,string SelectClause=null);
        List<User> GetUserByStateId(System.Int32? StateId, string SelectClause = null);
        List<User> GetUserBySalaryTypeId(System.Int32? SalaryTypeId, string SelectClause = null);
		User GetUser(System.Int32 Id,string SelectClause=null);
		User UpdateUser(User entity);
		User UpdateUserByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUser(System.Int32 Id);
		User DeleteUser(User entity);
		List<User> GetPagedUser(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<User> GetAllUser(string SelectClause=null);
		User InsertUser(User entity);
		List<User> GetUserByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
