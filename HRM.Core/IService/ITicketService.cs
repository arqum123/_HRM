using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Ticket;
using HRM.Core.Model;


namespace HRM.Core.IService
{
    public interface ITicketService
    {
        Dictionary<string, string> GetTicketBasicSearchColumns();
        List<SearchColumn> GetTicketAdvanceSearchColumns();
        List<Ticket> GetTicketByUserId(System.Int32? UserId);
        List<Ticket> GetTicketByPayrollId(System.Int32? PayrollId);
        Ticket GetTicket(System.Int32 Id);
        DataTransfer<List<GetOutput>> GetAll();
        Ticket UpdateTicket(Ticket entity);
        Ticket UpdateTicketByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id);
        bool DeleteTicket(System.Int32 Id);
        Ticket InsertTicket(Ticket entity);
        DataTransfer<GetOutput> Get(string id);
        DataTransfer<PostOutput> Insert(PostInput Input);
        DataTransfer<PutOutput> Update(PutInput Input);
        DataTransfer<string> Delete(string id);
        List<HRM.Core.Model.VMTicket> GetPendingTickets();
        System.Data.DataSet GetPendingTicketsDetail(); //NewModifyPayroll
        List<Ticket> GetTicketByUserIdAndDateRange(System.Int32? UserId,System.DateTime StartDate,System.DateTime EndDate);
        System.Data.DataSet GetTicketByUserIdAndDateRangeSP(System.Int32? UserId, System.DateTime StartDate, System.DateTime EndDate);
    }
}
