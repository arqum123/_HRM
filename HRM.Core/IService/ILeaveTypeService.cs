using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.LeaveType;

namespace HRM.Core.IService
{
		
	public interface ILeaveTypeService
	{
        Dictionary<string, string> GetLeaveTypeBasicSearchColumns();
        
        List<SearchColumn> GetLeaveTypeAdvanceSearchColumns();

		LeaveType GetLeaveType(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		LeaveType UpdateLeaveType(LeaveType entity);
		LeaveType UpdateLeaveTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteLeaveType(System.Int32 Id);
		List<LeaveType> GetAllLeaveType();
		LeaveType InsertLeaveType(LeaveType entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
