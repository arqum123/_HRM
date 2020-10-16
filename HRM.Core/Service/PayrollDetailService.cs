using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.PayrollDetail;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class PayrollDetailService : IPayrollDetailService 
	{
		private IPayrollDetailRepository _iPayrollDetailRepository;
        
		public PayrollDetailService(IPayrollDetailRepository iPayrollDetailRepository)
		{
			this._iPayrollDetailRepository = iPayrollDetailRepository;
		}
        
        public Dictionary<string, string> GetPayrollDetailBasicSearchColumns()
        {
            
            return this._iPayrollDetailRepository.GetPayrollDetailBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetPayrollDetailAdvanceSearchColumns()
        {
            
            return this._iPayrollDetailRepository.GetPayrollDetailAdvanceSearchColumns();
           
        }

        //New
        public virtual List<PayrollDetail> GetPayrollDetailAmountByPayrollId(System.Int32? PayrollId)
        {
            return _iPayrollDetailRepository.GetPayrollDetailAmountByPayrollId(PayrollId);
        }


        public virtual List<PayrollDetail> GetPayrollDetailByPayrollId(System.Int32? PayrollPolicyId)
        {
            return _iPayrollDetailRepository.GetPayrollDetailByPayrollId(PayrollPolicyId);
        }

        public virtual List<PayrollDetail> GetPayrollDetailByPayrollPolicyId(System.Int32? PayrollPolicyId)
        {
            return _iPayrollDetailRepository.GetPayrollDetailByPayrollPolicyId(PayrollPolicyId);
        }

        public PayrollDetail GetPayrollDetail(System.Int32 Id)
		{
			return _iPayrollDetailRepository.GetPayrollDetail(Id);
		}

		public PayrollDetail UpdatePayrollDetail(PayrollDetail entity)
		{
			return _iPayrollDetailRepository.UpdatePayrollDetail(entity);
		}

		public PayrollDetail UpdatePayrollDetailByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iPayrollDetailRepository.UpdatePayrollDetailByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeletePayrollDetail(System.Int32 Id)
		{
			return _iPayrollDetailRepository.DeletePayrollDetail(Id);
		}

		public List<PayrollDetail> GetAllPayrollDetail()
		{
			return _iPayrollDetailRepository.GetAllPayrollDetail();
		}

		public PayrollDetail InsertPayrollDetail(PayrollDetail entity)
		{
			 return _iPayrollDetailRepository.InsertPayrollDetail(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				PayrollDetail payrolldetail = _iPayrollDetailRepository.GetPayrollDetail(id);
                if(payrolldetail!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(payrolldetail);
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
            List<PayrollDetail> payrolldetaillist = _iPayrollDetailRepository.GetAllPayrollDetail();
            if (payrolldetaillist != null && payrolldetaillist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(payrolldetaillist);
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
                PayrollDetail payrolldetail = new PayrollDetail();
                PostOutput output = new PostOutput();
                payrolldetail.CopyFrom(Input);
                payrolldetail = _iPayrollDetailRepository.InsertPayrollDetail(payrolldetail);
                output.CopyFrom(payrolldetail);
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
                PayrollDetail payrolldetailinput = new PayrollDetail();
                PayrollDetail payrolldetailoutput = new PayrollDetail();
                PutOutput output = new PutOutput();
                payrolldetailinput.CopyFrom(Input);
                PayrollDetail payrolldetail = _iPayrollDetailRepository.GetPayrollDetail(payrolldetailinput.Id);
                if (payrolldetail!=null)
                {
                    payrolldetailoutput = _iPayrollDetailRepository.UpdatePayrollDetail(payrolldetailinput);
                    if(payrolldetailoutput!=null)
                    {
                        output.CopyFrom(payrolldetailoutput);
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
				 bool IsDeleted = _iPayrollDetailRepository.DeletePayrollDetail(id);
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
	}
	
	
}
