﻿using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.PayrollPolicy;
using Validation;
using System.Linq;
using HRM.Core.Model;
using System.Data;

namespace HRM.Core.Service
{
		
	public class PayrollPolicyService : IPayrollPolicyService 
	{
		private IPayrollPolicyRepository _iPayrollPolicyRepository;
        
		public PayrollPolicyService(IPayrollPolicyRepository iPayrollPolicyRepository)
		{
			this._iPayrollPolicyRepository = iPayrollPolicyRepository;
		}
        
        public Dictionary<string, string> GetPayrollPolicyBasicSearchColumns()
        {
            
            return this._iPayrollPolicyRepository.GetPayrollPolicyBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetPayrollPolicyAdvanceSearchColumns()
        {
            
            return this._iPayrollPolicyRepository.GetPayrollPolicyAdvanceSearchColumns();
           
        }
        public DataSet GetPayrollPolicyInformationSP(System.Int32? UserId,  System.Boolean? IsEarly, System.Boolean? IsLate, System.String StartDate, System.String EndDate)
        {

            return this._iPayrollPolicyRepository.GetPayrollPolicyInformationSP(UserId,  IsEarly, IsLate,StartDate,EndDate);

        }

        public virtual List<PayrollPolicy> GetPayrollPolicyByPayrollVariableId(System.Int32? PayrollVariableId)
		{
			return _iPayrollPolicyRepository.GetPayrollPolicyByPayrollVariableId(PayrollVariableId);
		}


        public PayrollPolicy GetPayrollPolicy(System.Int32 Id)
		{
			return _iPayrollPolicyRepository.GetPayrollPolicy(Id);
		}

		public PayrollPolicy UpdatePayrollPolicy(PayrollPolicy entity)
		{
			return _iPayrollPolicyRepository.UpdatePayrollPolicy(entity);
		}

		public PayrollPolicy UpdatePayrollPolicyByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iPayrollPolicyRepository.UpdatePayrollPolicyByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeletePayrollPolicy(System.Int32 Id)
		{
			return _iPayrollPolicyRepository.DeletePayrollPolicy(Id);
		}

		public List<PayrollPolicy> GetAllPayrollPolicy()
		{
			return _iPayrollPolicyRepository.GetAllPayrollPolicy();
		}

		public PayrollPolicy InsertPayrollPolicy(PayrollPolicy entity)
		{
			 return _iPayrollPolicyRepository.InsertPayrollPolicy(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				PayrollPolicy payrollpolicy = _iPayrollPolicyRepository.GetPayrollPolicy(id);
                if(payrollpolicy!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(payrollpolicy);
                    tranfer.Data=output ;

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
            List<PayrollPolicy> payrollpolicylist = _iPayrollPolicyRepository.GetAllPayrollPolicy();
            if (payrollpolicylist != null && payrollpolicylist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(payrollpolicylist);
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
            if(errors.Count==0)
            {
                PayrollPolicy payrollpolicy = new PayrollPolicy();
                PostOutput output = new PostOutput();
                payrollpolicy.CopyFrom(Input);
                payrollpolicy = _iPayrollPolicyRepository.InsertPayrollPolicy(payrollpolicy);
                output.CopyFrom(payrollpolicy);
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
                PayrollPolicy payrollpolicyinput = new PayrollPolicy();
                PayrollPolicy payrollpolicyoutput = new PayrollPolicy();
                PutOutput output = new PutOutput();
                payrollpolicyinput.CopyFrom(Input);
                PayrollPolicy payrollpolicy = _iPayrollPolicyRepository.GetPayrollPolicy(payrollpolicyinput.Id);
                if (payrollpolicy!=null)
                {
                    payrollpolicyoutput = _iPayrollPolicyRepository.UpdatePayrollPolicy(payrollpolicyinput);
                    if(payrollpolicyoutput!=null)
                    {
                        output.CopyFrom(payrollpolicyoutput);
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
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				 bool IsDeleted = _iPayrollPolicyRepository.DeletePayrollPolicy(id);
                if(IsDeleted)
                {
                    tranfer.IsSuccess = true;
                    tranfer.Data=IsDeleted.ToString().ToLower() ;

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
        private string StringNull(object val) { if (val == null || val == DBNull.Value) return ""; else return val.ToString(); }
        private DateTime? DateNull(object val) { if (val == null || val == DBNull.Value) return null; else return Convert.ToDateTime(val); }
        private Double DoubleNull(object val) { if (val == null || val == DBNull.Value) return 0.0; else return Convert.ToDouble(val); }

        private Decimal DecimalNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToDecimal(val); }
        private Int32 IntNull(object val) { if (val == null || val == DBNull.Value) return 0; else return Convert.ToInt32(val); }
        private Boolean BooleanNull(object val) { if (val == null || val == DBNull.Value) return false; else return Convert.ToBoolean(val); }
        public List<VMPayslipVariableInformation> GetPayrollPolicyInformation(System.Int32? UserId,  System.Boolean? IsEarly, System.Boolean? IsLate, String StartDate, String EndDate)
        {
            List<VMPayslipVariableInformation> variableSummary = new List<VMPayslipVariableInformation>();
            DataSet dsPayrollPolicy = _iPayrollPolicyRepository.GetPayrollPolicyInformationSP( UserId, IsEarly, IsLate,StartDate,EndDate);
            if (dsPayrollPolicy != null && dsPayrollPolicy.Tables.Count > 0 && dsPayrollPolicy.Tables[0] != null)
            {
                foreach (DataRow dr in dsPayrollPolicy.Tables[0].Rows)
                {
                    VMPayslipVariableInformation objPayrollPolicyInformation = new VMPayslipVariableInformation()
                    {
                        AttendanceID = IntNull(dr["AttendanceID"]),
                        Date = DateNull(dr["Date"]),
                        TimeIn = DateNull(dr["TimeIn"]),
                        TimeOutt = DateNull(dr["TimeOutt"]),
                        ShiftTimeIn = DateNull(dr["ShiftTimeIn"]),
                        ShiftTimeOut = DateNull(dr["ShiftTimeOut"]),
                        IsEarly = BooleanNull(dr["IsEarly"]),
                        IsLate = BooleanNull(dr["IsLate"]),
                        IsOffDay = BooleanNull(dr["IsOffDay"]),
                        ShiftName = dr["ShiftName"].ToString(),
                        TicketAttendanceID = IntNull(dr["TicketAttendanceID"]),
                    };
                    variableSummary.Add(objPayrollPolicyInformation);
                }
            }
            return variableSummary;
        }
    }
	
	
}
