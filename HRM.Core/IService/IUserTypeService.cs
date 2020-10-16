using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.UserType;

namespace HRM.Core.IService
{
		
	public interface IUserTypeService
	{
        Dictionary<string, string> GetUserTypeBasicSearchColumns();
        
        List<SearchColumn> GetUserTypeAdvanceSearchColumns();

		UserType GetUserType(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		UserType UpdateUserType(UserType entity);
		UserType UpdateUserTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUserType(System.Int32 Id);
		List<UserType> GetAllUserType();
		UserType InsertUserType(UserType entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
