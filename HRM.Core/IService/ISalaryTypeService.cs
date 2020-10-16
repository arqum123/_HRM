using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.SalaryType;

namespace HRM.Core.IService
{
		
	public interface ISalaryTypeService
	{
        Dictionary<string, string> GetSalaryTypeBasicSearchColumns();
        
        List<SearchColumn> GetSalaryTypeAdvanceSearchColumns();

		SalaryType GetSalaryType(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		SalaryType UpdateSalaryType(SalaryType entity);
		SalaryType UpdateSalaryTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeleteSalaryType(System.Int32 Id);
		List<SalaryType> GetAllSalaryType();
		SalaryType InsertSalaryType(SalaryType entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
