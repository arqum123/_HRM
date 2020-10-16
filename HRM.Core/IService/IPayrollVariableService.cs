using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.PayrollVariable;

namespace HRM.Core.IService
{
		
	public interface IPayrollVariableService
	{
        Dictionary<string, string> GetPayrollVariableBasicSearchColumns();
        
        List<SearchColumn> GetPayrollVariableAdvanceSearchColumns();

		PayrollVariable GetPayrollVariable(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		PayrollVariable UpdatePayrollVariable(PayrollVariable entity);
		PayrollVariable UpdatePayrollVariableByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayrollVariable(System.Int32 Id);
		List<PayrollVariable> GetAllPayrollVariable();
		PayrollVariable InsertPayrollVariable(PayrollVariable entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
