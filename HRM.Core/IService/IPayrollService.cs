using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Payroll;
using HRM.Core.Model;

namespace HRM.Core.IService
{
		
	public interface IPayrollService
	{
        Dictionary<string, string> GetPayrollBasicSearchColumns();
        
        List<SearchColumn> GetPayrollAdvanceSearchColumns();

		List<Payroll> GetPayrollByPayrollCycleId(System.Int32? PayrollCycleId);

        List<VMModifyPayrollEdit> GetModifyPayrollByPayrollCycleId(System.Int32? PayrollCycleId,System.Int32? DepartmentId,System.String UserName); //NewModfyPayroll
        List<VMModifyPayrollEdit> GetModifyPayslipEdit(System.Int32? PayrollId, System.Int32? DepartmentId, System.Int32? UserId); //NewModifyPayroll

        Payroll GetPayroll(System.Int32 Id);
		DataTransfer<List<GetOutput>> GetAll();
		Payroll UpdatePayroll(Payroll entity);
		Payroll UpdatePayrollByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
		bool DeletePayroll(System.Int32 Id);
		List<Payroll> GetAllPayroll();
		Payroll InsertPayroll(Payroll entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
        List<Payroll> GetAllPayrollWithUser(int BranchID);
        void RunPayroll(int PayrollCycleID, int IsFinal);
        List<Payroll> GetAllPayrollWithUserWithTestPayroll(int BranchID);
        List<Payroll> GetAllPayrollWithUser(int BranchID, int IsActive);
	}
	
	
}
