using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.BranchShift;

namespace HRM.Core.IService
{
		
	public interface IBranchShiftService
	{
        Dictionary<string, string> GetBranchShiftBasicSearchColumns();
        
        List<SearchColumn> GetBranchShiftAdvanceSearchColumns();

		List<BranchShift> GetBranchShiftByBranchId(System.Int32? BranchId);
		List<BranchShift> GetBranchShiftByShiftId(System.Int32? ShiftId);
		BranchShift GetBranchShift(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		BranchShift UpdateBranchShift(BranchShift entity);
		BranchShift UpdateBranchShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteBranchShift(System.Int32 Id);
		List<BranchShift> GetAllBranchShift();
		BranchShift InsertBranchShift(BranchShift entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
