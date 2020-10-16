using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;

namespace HRM.Core.DataInterfaces
{
		
	public interface IUserShiftRepositoryBase
	{
        
        Dictionary<string, string> GetUserShiftBasicSearchColumns();
        List<SearchColumn> GetUserShiftSearchColumns();
        List<SearchColumn> GetUserShiftAdvanceSearchColumns();
        

		List<UserShift> GetUserShiftByUserId(System.Int32? UserId,string SelectClause=null);
		List<UserShift> GetUserShiftByShiftId(System.Int32? ShiftId,string SelectClause=null);
		UserShift GetUserShift(System.Int32 Id,string SelectClause=null);
		UserShift UpdateUserShift(UserShift entity);
		UserShift UpdateUserShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUserShift(System.Int32 Id);
		UserShift DeleteUserShift(UserShift entity);
		List<UserShift> GetPagedUserShift(string orderByClause, int pageSize, int startIndex,out int count, List<SearchColumn> searchColumns,string SelectClause=null);
		List<UserShift> GetAllUserShift(string SelectClause=null);
		UserShift InsertUserShift(UserShift entity);
		List<UserShift> GetUserShiftByKeyValue(string Key,string Value,Operands operand,string SelectClause=null);	}
	
	
}
