using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.PayrollCycle;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class PayrollCycleService : IPayrollCycleService 
	{
		private IPayrollCycleRepository _iPayrollCycleRepository;
        
		public PayrollCycleService(IPayrollCycleRepository iPayrollCycleRepository)
		{
			this._iPayrollCycleRepository = iPayrollCycleRepository;
		}
        
        public Dictionary<string, string> GetPayrollCycleBasicSearchColumns()
        {
            
            return this._iPayrollCycleRepository.GetPayrollCycleBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetPayrollCycleAdvanceSearchColumns()
        {
            
            return this._iPayrollCycleRepository.GetPayrollCycleAdvanceSearchColumns();
           
        }
        

		public PayrollCycle GetPayrollCycle(System.Int32 Id)
		{
			return _iPayrollCycleRepository.GetPayrollCycle(Id);
		}

		public PayrollCycle UpdatePayrollCycle(PayrollCycle entity)
		{
			return _iPayrollCycleRepository.UpdatePayrollCycle(entity);
		}

		public PayrollCycle UpdatePayrollCycleByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iPayrollCycleRepository.UpdatePayrollCycleByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeletePayrollCycle(System.Int32 Id)
		{
			return _iPayrollCycleRepository.DeletePayrollCycle(Id);
		}

		public List<PayrollCycle> GetAllPayrollCycle()
		{
			return _iPayrollCycleRepository.GetAllPayrollCycle();
		}

		public PayrollCycle InsertPayrollCycle(PayrollCycle entity)
		{
			 return _iPayrollCycleRepository.InsertPayrollCycle(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				PayrollCycle payrollcycle = _iPayrollCycleRepository.GetPayrollCycle(id);
                if(payrollcycle!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(payrollcycle);
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
            List<PayrollCycle> payrollcyclelist = _iPayrollCycleRepository.GetAllPayrollCycle();
            if (payrollcyclelist != null && payrollcyclelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(payrollcyclelist);
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
                PayrollCycle payrollcycle = new PayrollCycle();
                PostOutput output = new PostOutput();
                payrollcycle.CopyFrom(Input);
                payrollcycle = _iPayrollCycleRepository.InsertPayrollCycle(payrollcycle);
                output.CopyFrom(payrollcycle);
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
                PayrollCycle payrollcycleinput = new PayrollCycle();
                PayrollCycle payrollcycleoutput = new PayrollCycle();
                PutOutput output = new PutOutput();
                payrollcycleinput.CopyFrom(Input);
                PayrollCycle payrollcycle = _iPayrollCycleRepository.GetPayrollCycle(payrollcycleinput.Id);
                if (payrollcycle!=null)
                {
                    payrollcycleoutput = _iPayrollCycleRepository.UpdatePayrollCycle(payrollcycleinput);
                    if(payrollcycleoutput!=null)
                    {
                        output.CopyFrom(payrollcycleoutput);
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
				 bool IsDeleted = _iPayrollCycleRepository.DeletePayrollCycle(id);
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
