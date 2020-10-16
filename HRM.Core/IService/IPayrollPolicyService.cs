using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.PayrollPolicy;
using HRM.Core.Model;

namespace HRM.Core.IService
{
		
	public interface IPayrollPolicyService
	{
        Dictionary<string, string> GetPayrollPolicyBasicSearchColumns();
        
        List<SearchColumn> GetPayrollPolicyAdvanceSearchColumns();

		List<PayrollPolicy> GetPayrollPolicyByPayrollVariableId(System.Int32? PayrollVariableId);
		PayrollPolicy GetPayrollPolicy(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		PayrollPolicy UpdatePayrollPolicy(PayrollPolicy entity);
		PayrollPolicy UpdatePayrollPolicyByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayrollPolicy(System.Int32 Id);
		List<PayrollPolicy> GetAllPayrollPolicy();
		PayrollPolicy InsertPayrollPolicy(PayrollPolicy entity);
        List<VMPayslipVariableInformation> GetPayrollPolicyInformation(System.Int32? UserId,  System.Boolean? IsEarly, System.Boolean? IsLate, System.String StartDate, System.String EndDate); //Declare
        System.Data.DataSet GetPayrollPolicyInformationSP(System.Int32? UserId,System.Boolean? IsEarly, System.Boolean? IsLate, System.String StartDate, System.String EndDate);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
	}
	
	
}
