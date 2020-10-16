using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Attendance;
using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.IService
{
    public interface ICustomPayrollDetailService
    {
        Dictionary<string, string> GetCustomPayrollDetailBasicSearchColumns();

        List<SearchColumn> GetCustomPayrollDetailAdvanceSearchColumns();
        //New
        List<CustomPayrollDetail> GetCustomPayrollDetailByPayrollId(System.Int32? PayrollId);
        List<CustomPayrollDetail> GetCustomPayrollDetailByPayrollPolicyId(System.Int32? PayrollPolicyId);
        CustomPayrollDetail GetCustomPayrollDetail(System.Int32 Id);
        DataTransfer<List<GetOutput>> GetAll();
        CustomPayrollDetail UpdateCustomPayrollDetail(CustomPayrollDetail entity);
        CustomPayrollDetail UpdateCustomPayrollDetailByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
        bool DeleteCustomPayrollDetail(System.Int32 Id);
        List<CustomPayrollDetail> GetAllCustomPayrollDetail();
        CustomPayrollDetail InsertCustomPayrollDetail(CustomPayrollDetail entity);

        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
    }

}
