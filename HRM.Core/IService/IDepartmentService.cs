using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Department;

namespace HRM.Core.IService
{
		
	public interface IDepartmentService
	{
        Dictionary<string, string> GetDepartmentBasicSearchColumns();
        
        List<SearchColumn> GetDepartmentAdvanceSearchColumns();

		Department GetDepartment(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Department UpdateDepartment(Department entity);
		Department UpdateDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteDepartment(System.Int32 Id);
		List<Department> GetAllDepartment();
		Department InsertDepartment(Department entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);

        List<Department> GetAllDepartment(int BranchID);
	}
	
	
}
