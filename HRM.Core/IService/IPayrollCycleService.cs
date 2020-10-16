using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.PayrollCycle;

namespace HRM.Core.IService
{
		
	public interface IPayrollCycleService
	{
        Dictionary<string, string> GetPayrollCycleBasicSearchColumns();
        
        List<SearchColumn> GetPayrollCycleAdvanceSearchColumns();

		PayrollCycle GetPayrollCycle(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		PayrollCycle UpdatePayrollCycle(PayrollCycle entity);
		PayrollCycle UpdatePayrollCycleByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayrollCycle(System.Int32 Id);
		List<PayrollCycle> GetAllPayrollCycle();
		PayrollCycle InsertPayrollCycle(PayrollCycle entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
