using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.DataInterfaces
{
    public interface ICustomPayrollDetailRepositoryBase
    {

        Dictionary<string, string> GetCustomPayrollDetailBasicSearchColumns();
        List<SearchColumn> GetCustomPayrollDetailSearchColumns();
        List<SearchColumn> GetCustomPayrollDetailAdvanceSearchColumns();
        List<CustomPayrollDetail> GetCustomPayrollDetailByPayrollPolicyId(System.Int32? PayrollPolicyId, string SelectClause = null);
        CustomPayrollDetail GetCustomPayrollDetail(System.Int32 Id, string SelectClause = null);
        CustomPayrollDetail UpdateCustomPayrollDetail(CustomPayrollDetail entity);
        CustomPayrollDetail UpdateCustomPayrollDetailByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
        bool DeleteCustomPayrollDetail(System.Int32 Id);
        CustomPayrollDetail DeleteCustomPayrollDetail(CustomPayrollDetail entity);
        List<CustomPayrollDetail> GetPagedCustomPayrollDetail(string orderByClause, int pageSize, int startIndex, out int count, List<SearchColumn> searchColumns, string SelectClause = null);
        List<CustomPayrollDetail> GetAllCustomPayrollDetail(string SelectClause = null);
        CustomPayrollDetail InsertCustomPayrollDetail(CustomPayrollDetail entity);
        List<CustomPayrollDetail> GetCustomPayrollDetailByKeyValue(string Key, string Value, Operands operand, string SelectClause = null);
    }

}


