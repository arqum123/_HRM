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

        List<VMModifyPayrollEdit> GetModifyPayslipDetailEdit(System.Int32? PayrollId, System.Int32? PayrollVariableId, System.Decimal? Addition, System.Decimal? Deduction); //NewModifyPayroll
        System.Data.DataSet GetModifyPayslipEdit(System.Int32? PayrollId, System.Int32? DepartmentId, System.Int32? UserId); //NewModifyPayroll
        System.Data.DataSet GetPayslipDetail(System.Int32? PayrollCycleID, System.Int32? UserId); //NewPayslip
        System.Data.DataSet GetEmpPayslipDetail(System.Int32? PayrollCycleID, System.Int32? UserId); //NewPayslip
        System.Data.DataSet GetEmpPayrollDetail(System.Int32? PayrollCycleId,System.Int32? UserId); //NewPayslip
        List<HRM.Core.Model.VMPayslipDetailUser> GetPayslip(System.Int32? PayrollCycleId, System.Int32? UserId);
        List<HRM.Core.Model.VMEmpPayslipDetailUser> GetEmpPayslip(System.Int32? PayrollCycleId, System.Int32? UserId);
        List<HRM.Core.Model.VMEmpPayrollService> GetEmpPayrollByPayrollCycleId(System.Int32? PayrollCycleId,System.Int32? UserId);

        List<HRM.Core.Model.VMUserPayrollEdit> GetModifyPayrollSummary(System.Int32? DepartmentId, System.Int32? UserId, System.Int32? PayrollId);

        System.Data.DataSet GetModifyPayrollByPayrollCycleId(System.Int32? PayrollCycleId, System.Int32? DepartmentId, System.String UserName); //NewModifyPayroll
        List<HRM.Core.Model.VMGetPayrollEditSecond> GetModifyPayrollSummaryByPayrollCycleId(System.Int32? DepartmentId, System.String UserName, System.Int32? PayrollCycleId);

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
        List<HRM.Core.Model.VMTicket> GetPendingTickets();
        List<HRM.Core.Model.VMTicketHistory> GetTicketsByDateRange(DateTime StartDate,DateTime EndDate,int?UserID,int? DepartmentID);
        System.Data.DataSet GetTicketsByDateRangeDetail(DateTime StartDate, DateTime EndDate, int? UserID, int? DepartmentID);
        System.Data.DataSet GetPendingTicketsDetail(); 
    }
	
	
}
