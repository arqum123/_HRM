using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.BranchDepartment;

namespace HRM.Core.IService
{
		
	public interface IBranchDepartmentService
	{
        Dictionary<string, string> GetBranchDepartmentBasicSearchColumns();
        
        List<SearchColumn> GetBranchDepartmentAdvanceSearchColumns();

		List<BranchDepartment> GetBranchDepartmentByBranchId(System.Int32? BranchId);
		List<BranchDepartment> GetBranchDepartmentByDepartmentId(System.Int32? DepartmentId);
		BranchDepartment GetBranchDepartment(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		BranchDepartment UpdateBranchDepartment(BranchDepartment entity);
		BranchDepartment UpdateBranchDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteBranchDepartment(System.Int32 Id);
		List<BranchDepartment> GetAllBranchDepartment();
		BranchDepartment InsertBranchDepartment(BranchDepartment entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
