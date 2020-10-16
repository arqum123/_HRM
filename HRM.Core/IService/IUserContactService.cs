using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.UserContact;

namespace HRM.Core.IService
{
		
	public interface IUserContactService
	{
        Dictionary<string, string> GetUserContactBasicSearchColumns();
        
        List<SearchColumn> GetUserContactAdvanceSearchColumns();

		List<UserContact> GetUserContactByUserId(System.Int32? UserId);
		List<UserContact> GetUserContactByContactTypeId(System.Int32? ContactTypeId);
		UserContact GetUserContact(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		UserContact UpdateUserContact(UserContact entity);
		UserContact UpdateUserContactByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUserContact(System.Int32 Id);
		List<UserContact> GetAllUserContact();
		UserContact InsertUserContact(UserContact entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
