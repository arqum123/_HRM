using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.PayrollDetail;

namespace HRM.Core.IService
{
		
	public interface IPayrollDetailService
	{
        Dictionary<string, string> GetPayrollDetailBasicSearchColumns();
        
        List<SearchColumn> GetPayrollDetailAdvanceSearchColumns();
        //New
        List<PayrollDetail> GetPayrollDetailAmountByPayrollId(System.Int32? PayrollId);
        List<PayrollDetail> GetPayrollDetailByPayrollId(System.Int32? PayrollId);
		List<PayrollDetail> GetPayrollDetailByPayrollPolicyId(System.Int32? PayrollPolicyId);
		PayrollDetail GetPayrollDetail(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		PayrollDetail UpdatePayrollDetail(PayrollDetail entity);
		PayrollDetail UpdatePayrollDetailByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayrollDetail(System.Int32 Id);
		List<PayrollDetail> GetAllPayrollDetail();
		PayrollDetail InsertPayrollDetail(PayrollDetail entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
