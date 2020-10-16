using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.PayrollVariable;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class PayrollVariableService : IPayrollVariableService 
	{
		private IPayrollVariableRepository _iPayrollVariableRepository;
        
		public PayrollVariableService(IPayrollVariableRepository iPayrollVariableRepository)
		{
			this._iPayrollVariableRepository = iPayrollVariableRepository;
		}
        
        public Dictionary<string, string> GetPayrollVariableBasicSearchColumns()
        {
            
            return this._iPayrollVariableRepository.GetPayrollVariableBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetPayrollVariableAdvanceSearchColumns()
        {
            
            return this._iPayrollVariableRepository.GetPayrollVariableAdvanceSearchColumns();
           
        }
        

		public PayrollVariable GetPayrollVariable(System.Int32 Id)
		{
			return _iPayrollVariableRepository.GetPayrollVariable(Id);
		}

		public PayrollVariable UpdatePayrollVariable(PayrollVariable entity)
		{
			return _iPayrollVariableRepository.UpdatePayrollVariable(entity);
		}

		public PayrollVariable UpdatePayrollVariableByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iPayrollVariableRepository.UpdatePayrollVariableByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeletePayrollVariable(System.Int32 Id)
		{
			return _iPayrollVariableRepository.DeletePayrollVariable(Id);
		}

		public List<PayrollVariable> GetAllPayrollVariable()
		{
			return _iPayrollVariableRepository.GetAllPayrollVariable();
		}

		public PayrollVariable InsertPayrollVariable(PayrollVariable entity)
		{
			 return _iPayrollVariableRepository.InsertPayrollVariable(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				PayrollVariable payrollvariable = _iPayrollVariableRepository.GetPayrollVariable(id);
                if(payrollvariable!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(payrollvariable);
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
            List<PayrollVariable> payrollvariablelist = _iPayrollVariableRepository.GetAllPayrollVariable();
            if (payrollvariablelist != null && payrollvariablelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(payrollvariablelist);
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
                PayrollVariable payrollvariable = new PayrollVariable();
                PostOutput output = new PostOutput();
                payrollvariable.CopyFrom(Input);
                payrollvariable = _iPayrollVariableRepository.InsertPayrollVariable(payrollvariable);
                output.CopyFrom(payrollvariable);
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
                PayrollVariable payrollvariableinput = new PayrollVariable();
                PayrollVariable payrollvariableoutput = new PayrollVariable();
                PutOutput output = new PutOutput();
                payrollvariableinput.CopyFrom(Input);
                PayrollVariable payrollvariable = _iPayrollVariableRepository.GetPayrollVariable(payrollvariableinput.Id);
                if (payrollvariable!=null)
                {
                    payrollvariableoutput = _iPayrollVariableRepository.UpdatePayrollVariable(payrollvariableinput);
                    if(payrollvariableoutput!=null)
                    {
                        output.CopyFrom(payrollvariableoutput);
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
				 bool IsDeleted = _iPayrollVariableRepository.DeletePayrollVariable(id);
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
