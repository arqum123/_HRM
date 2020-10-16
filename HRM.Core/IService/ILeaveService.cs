using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Leave;

namespace HRM.Core.IService
{
		
	public interface ILeaveService
	{
        Dictionary<string, string> GetLeaveBasicSearchColumns();
        
        List<SearchColumn> GetLeaveAdvanceSearchColumns();

		List<Leave> GetLeaveByUserId(System.Int32? UserId);
		List<Leave> GetLeaveByLeaveTypeId(System.Int32? LeaveTypeId);
		Leave GetLeave(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Leave UpdateLeave(Leave entity);
		Leave UpdateLeaveByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteLeave(System.Int32 Id);
		List<Leave> GetAllLeave();
		Leave InsertLeave(Leave entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
