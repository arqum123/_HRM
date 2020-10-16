using HRM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.DataInterfaces
{
    public interface ITicketRepositoryBase
    {
        Dictionary<string, string> GetTicketBasicSearchColumns();
        List<SearchColumn> GetTicketSearchColumns();
        List<SearchColumn> GetTicketAdvanceSearchColumns();


        List<Ticket> GetTicketByUserId(System.Int32? UserId, string SelectClause = null);
        List<Ticket> GetTicketByPayrollId(System.Int32? PayrollId, string SelectClause = null);
        Ticket GetTicket(System.Int32 Id, string SelectClause = null);
        Ticket UpdateTicket(Ticket entity);
        Ticket UpdateTicketByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
        bool DeleteTicket(System.Int32 Id);
        Ticket DeleteTicket(Ticket entity);
        List<Ticket> GetPagedTicket(string orderByClause, int pageSize, int startIndex, out int count, List<SearchColumn> searchColumns, string SelectClause = null);
        List<Ticket> GetAllTicket(string SelectClause = null);
        Ticket InsertTicket(Ticket entity);
        List<Ticket> GetTicketByKeyValue(string Key, string Value, Operands operand, string SelectClause = null);
    }
}
