using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Ticket;
using Validation;
using System.Linq;
using HRM.Core.Model;
using System.Data;


namespace HRM.Core.Service
{
   public class TicketService : ITicketService
    {
        private ITicketRepository _iTicketRepository;

        public TicketService(ITicketRepository iTicketRepository)
        {
            this._iTicketRepository = iTicketRepository;
        }

        public Dictionary<string, string> GetTicketBasicSearchColumns()
        {

            return this._iTicketRepository.GetTicketBasicSearchColumns();

        }

        public List<SearchColumn> GetTicketAdvanceSearchColumns()
        {

            return this._iTicketRepository.GetTicketAdvanceSearchColumns();

        }

        public virtual List<Ticket> GetTicketByUserId(System.Int32? UserId)
        {
            return _iTicketRepository.GetTicketByUserId(UserId);
        }

        public virtual List<Ticket> GetTicketByPayrollId(System.Int32? PayrollId)
        {
            return _iTicketRepository.GetTicketByPayrollId(PayrollId);
        }

        public Ticket GetTicket(System.Int32 Id)
        {
            return _iTicketRepository.GetTicket(Id);
        }

        public Ticket UpdateTicket(Ticket entity)
        {
            return _iTicketRepository.UpdateTicket(entity);
        }

        public Ticket UpdateTicketByKeyValue(Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
        {
            return _iTicketRepository.UpdateTicketByKeyValue(UpdateKeyValue, Id);
        }

        public bool DeleteTicket(System.Int32 Id)
        {
            return _iTicketRepository.DeleteTicket(Id);
        }
        public List<Ticket> GetAllTicket()
        {
            return _iTicketRepository.GetAllTicket();
        }
        public virtual System.Data.DataSet GetTicketByUserIdAndDateRangeSP(System.Int32? UserId, System.DateTime StartDate, System.DateTime EndDate) //NewEmpPayroll
        {
            return _iTicketRepository.GetTicketByUserIdAndDateRangeSP(UserId,StartDate, EndDate);
        }
        public Ticket InsertTicket(Ticket entity)
        {
            return _iTicketRepository.InsertTicket(entity);
        }

        public virtual System.Data.DataSet GetPendingTicketsDetail() //NewModfyPayroll
        {
            return _iTicketRepository.GetPendingTicketsDetail();
        }
        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                Ticket ticket = _iTicketRepository.GetTicket(id);
                if (ticket != null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(ticket);
                    tranfer.Data = output;

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }
        public DataTransfer<List<GetOutput>> GetAll()
        {
            DataTransfer<List<GetOutput>> tranfer = new DataTransfer<List<GetOutput>>();
            List<Ticket> ticketlist = _iTicketRepository.GetAllTicket();
            if (ticketlist != null && ticketlist.Count > 0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(ticketlist);
                tranfer.Data = outputlist;

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: No record found.";
            }
            return tranfer;
        }
        public DataTransfer<PostOutput> Insert(PostInput Input)
        {
            DataTransfer<PostOutput> transer = new DataTransfer<PostOutput>();
            IList<string> errors = Validator.Validate(Input);
            if (errors.Count == 0)
            {
                Ticket ticket = new Ticket();
                PostOutput output = new PostOutput();
                ticket.CopyFrom(Input);
                ticket = _iTicketRepository.InsertTicket(ticket);
                output.CopyFrom(ticket);
                transer.IsSuccess = true;
                transer.Data = output;
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

        public DataTransfer<PutOutput> Update(PutInput Input)
        {
            DataTransfer<PutOutput> transer = new DataTransfer<PutOutput>();
            IList<string> errors = Validator.Validate(Input);
            if (errors.Count == 0)
            {
                Ticket ticketinput = new Ticket();
                Ticket ticketoutput = new Ticket();
                PutOutput output = new PutOutput();
                ticketinput.CopyFrom(Input);
                Ticket ticket = _iTicketRepository.GetTicket(ticketinput.ID);
                if (ticket != null)
                {
                    ticketoutput = _iTicketRepository.UpdateTicket(ticketinput);
                    if (ticketoutput != null)
                    {
                        output.CopyFrom(ticketoutput);
                        transer.IsSuccess = true;
                        transer.Data = output;
                    }
                    else
                    {
                        transer.IsSuccess = false;
                        transer.Errors = new string[1];
                        transer.Errors[0] = "Error: Could not update.";
                    }
                }
                else
                {
                    transer.IsSuccess = false;
                    transer.Errors = new string[1];
                    transer.Errors[0] = "Error: Record not found.";
                }
            }
            else
            {
                transer.IsSuccess = false;
                transer.Errors = errors.ToArray<string>();
            }
            return transer;
        }

        public DataTransfer<string> Delete(string _id)
        {
            DataTransfer<string> tranfer = new DataTransfer<string>();
            System.Int32 id = 0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id, out id))
            {
                bool IsDeleted = _iTicketRepository.DeleteTicket(id);
                if (IsDeleted)
                {
                    tranfer.IsSuccess = true;
                    tranfer.Data = IsDeleted.ToString().ToLower();

                }
                else
                {
                    tranfer.IsSuccess = false;
                    tranfer.Errors = new string[1];
                    tranfer.Errors[0] = "Error: No record found.";
                }

            }
            else
            {
                tranfer.IsSuccess = false;
                tranfer.Errors = new string[1];
                tranfer.Errors[0] = "Error: Invalid request.";
            }
            return tranfer;
        }
        public List<VMTicket> GetPendingTickets()
        {
            List<VMTicket> ticketSummary = new List<VMTicket>();
            DataSet dsTicket = _iTicketRepository.GetPendingTicketsDetail();
            if (dsTicket != null && dsTicket.Tables.Count > 0 && dsTicket.Tables[0] != null)
            {
                //dsPayroll.Tables[0] // Payroll , PayrollCycle,User and Department
                //dsPayroll.Tables[1] // PayrollDetail ,PayrollVariable,SalaryType  
                foreach (DataRow dr in dsTicket.Tables[0].Rows)
                {
                    VMTicket objTicketSummary = new VMTicket()
                    {
                        TicketID = IntNull(dr["TicketID"]),
                        AttendanceID = IntNull(dr["AttendanceID"]),
                        TimeIn = Convert.ToDateTime(dr["TimeIn"]),
                        TimeOut = Convert.ToDateTime(dr["TimeOut"]),
                        AttendanceDate = Convert.ToDateTime(dr["TimeOut"]),
                        ShiftTimeIn = Convert.ToDateTime(dr["ShiftTimeIn"]),
                        ShiftTimeOut = Convert.ToDateTime(dr["ShiftTimeOut"]),
                        OffDay = BooleanNull(dr["OffDay"]),
                        Reason = dr["Reason"].ToString(),
                        IsApproved = BooleanNull(dr["IsApproved"]),
                        IsReject = BooleanNull(dr["IsReject"]),
                        Comments = dr["Comments"].ToString(),
                        EmpName = dr["EmpName"].ToString(),
                        DepartmentName = dr["DepartmentName"].ToString(),
                        ShiftName = dr["ShiftName"].ToString(),
                    };
                    ticketSummary.Add(objTicketSummary);
                }
            }
            return ticketSummary;
        }
        public List<Ticket> GetTicketByUserIdAndDateRange(System.Int32? UserId,System.DateTime StartDate, System.DateTime EndDate)
        {
            List<Ticket> ticketSummary = new List<Ticket>();
            DataSet dsTicket = _iTicketRepository.GetTicketByUserIdAndDateRangeSP(UserId,StartDate,EndDate);
            if (dsTicket != null && dsTicket.Tables.Count > 0 && dsTicket.Tables[0] != null)
            {
                foreach (DataRow dr in dsTicket.Tables[0].Rows)
                {
                    Ticket objTicketSummary = new Ticket()
                    {
                        ID = IntNull(dr["TicketID"]),
                        UserID = IntNull(dr["UserID"]),
                        Reason = dr["Reason"].ToString(),
                        IsApproved = BooleanNull(dr["IsApproved"]),
                        IsReject = BooleanNull(dr["IsReject"]),
                        Comments = dr["Comments"].ToString(),
                        CreationDate = DateNull(dr["CreationDate"]), 
                        UpdateDate = DateNull(dr["UpdateDate"]), 
                        UpdateBy = IntNull(dr["UpdateBy"]), 
                        AttendanceID = IntNull(dr["AttendanceID"]), 
                        UserIp = dr["UserIp"].ToString()
                    };
                    ticketSummary.Add(objTicketSummary);
                }
            }
            return ticketSummary;
        }
        
        private string StringNull(object val) { if (val == null || val == DBNull.Value) return ""; else return val.ToString(); }
        private DateTime? DateNull(object val) { if (val == null || val == DBNull.Value) return null; else return Convert.ToDateTime(val); }
        private Double DoubleNull(object val) { if (val == null || val == DBNull.Value) return 0.0; else return Convert.ToDouble(val); }

        private Decimal DecimalNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToDecimal(val); }
        private Int32 IntNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToInt32(val); }
        private Boolean BooleanNull(object val) { if (val == null || val == DBNull.Value) return false; else return Convert.ToBoolean(val); }

    }
}
