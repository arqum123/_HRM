using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.UserShift;

namespace HRM.Core.IService
{
		
	public interface IUserShiftService
	{
        Dictionary<string, string> GetUserShiftBasicSearchColumns();
        
        List<SearchColumn> GetUserShiftAdvanceSearchColumns();

		List<UserShift> GetUserShiftByUserId(System.Int32? UserId);
		List<UserShift> GetUserShiftByShiftId(System.Int32? ShiftId);
		UserShift GetUserShift(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		UserShift UpdateUserShift(UserShift entity);
		UserShift UpdateUserShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteUserShift(System.Int32 Id);
		List<UserShift> GetAllUserShift();
		UserShift InsertUserShift(UserShift entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
