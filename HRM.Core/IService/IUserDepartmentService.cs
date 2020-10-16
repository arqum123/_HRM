using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.UserDepartment;

namespace HRM.Core.IService
{
		
	public interface IUserDepartmentService
	{
        Dictionary<string, string> GetUserDepartmentBasicSearchColumns();
        
        List<SearchColumn> GetUserDepartmentAdvanceSearchColumns();

		List<UserDepartment> GetUserDepartmentByUserId(System.Int32? UserId);
		List<UserDepartment> GetUserDepartmentByDepartmentId(System.Int32? DepartmentId);
		UserDepartment GetUserDepartment(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		UserDepartment UpdateUserDepartment(UserDepartment entity);
		UserDepartment UpdateUserDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUserDepartment(System.Int32 Id);
		List<UserDepartment> GetAllUserDepartment();
		UserDepartment InsertUserDepartment(UserDepartment entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
