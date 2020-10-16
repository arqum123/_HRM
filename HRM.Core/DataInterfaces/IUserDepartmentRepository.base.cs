using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IUserDepartmentRepositoryBase
	{
        
        Dictionary<string, string> GetUserDepartmentBasicSearchColumns();
        List<SearchColumn> GetUserDepartmentSearchColumns();
        List<SearchColumn> GetUserDepartmentAdvanceSearchColumns();
        

		List<UserDepartment> GetUserDepartmentByUserId(System.Int32? UserId,string SelectClause=null);
		List<UserDepartment> GetUserDepartmentByDepartmentId(System.Int32? DepartmentId,string SelectClause=null);
		UserDepartment GetUserDepartment(System.Int32 Id,string SelectClause=null);
		UserDepartment UpdateUserDepartment(UserDepartment entity);
		UserDepartment UpdateUserDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUserDepartment(System.Int32 Id);
		UserDepartment DeleteUserDepartment(UserDepartment entity);
		List<UserDepartment> GetPagedUserDepartment(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<UserDepartment> GetAllUserDepartment(string SelectClause=null);
		UserDepartment InsertUserDepartment(UserDepartment entity);
		List<UserDepartment> GetUserDepartmentByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
