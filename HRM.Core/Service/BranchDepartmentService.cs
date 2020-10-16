using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.BranchDepartment;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class BranchDepartmentService : IBranchDepartmentService 
	{
		private IBranchDepartmentRepository _iBranchDepartmentRepository;
        
		public BranchDepartmentService(IBranchDepartmentRepository iBranchDepartmentRepository)
		{
			this._iBranchDepartmentRepository = iBranchDepartmentRepository;
		}
        
        public Dictionary<string, string> GetBranchDepartmentBasicSearchColumns()
        {
            
            return this._iBranchDepartmentRepository.GetBranchDepartmentBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetBranchDepartmentAdvanceSearchColumns()
        {
            
            return this._iBranchDepartmentRepository.GetBranchDepartmentAdvanceSearchColumns();
           
        }
        

		public virtual List<BranchDepartment> GetBranchDepartmentByBranchId(System.Int32? BranchId)
		{
			return _iBranchDepartmentRepository.GetBranchDepartmentByBranchId(BranchId);
		}

		public virtual List<BranchDepartment> GetBranchDepartmentByDepartmentId(System.Int32? DepartmentId)
		{
			return _iBranchDepartmentRepository.GetBranchDepartmentByDepartmentId(DepartmentId);
		}

		public BranchDepartment GetBranchDepartment(System.Int32 Id)
		{
			return _iBranchDepartmentRepository.GetBranchDepartment(Id);
		}

		public BranchDepartment UpdateBranchDepartment(BranchDepartment entity)
		{
			return _iBranchDepartmentRepository.UpdateBranchDepartment(entity);
		}

		public BranchDepartment UpdateBranchDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iBranchDepartmentRepository.UpdateBranchDepartmentByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteBranchDepartment(System.Int32 Id)
		{
			return _iBranchDepartmentRepository.DeleteBranchDepartment(Id);
		}

		public List<BranchDepartment> GetAllBranchDepartment()
		{
			return _iBranchDepartmentRepository.GetAllBranchDepartment();
		}

		public BranchDepartment InsertBranchDepartment(BranchDepartment entity)
		{
			 return _iBranchDepartmentRepository.InsertBranchDepartment(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				BranchDepartment branchdepartment = _iBranchDepartmentRepository.GetBranchDepartment(id);
                if(branchdepartment!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(branchdepartment);
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
            List<BranchDepartment> branchdepartmentlist = _iBranchDepartmentRepository.GetAllBranchDepartment();
            if (branchdepartmentlist != null && branchdepartmentlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(branchdepartmentlist);
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
                BranchDepartment branchdepartment = new BranchDepartment();
                PostOutput output = new PostOutput();
                branchdepartment.CopyFrom(Input);
                branchdepartment = _iBranchDepartmentRepository.InsertBranchDepartment(branchdepartment);
                output.CopyFrom(branchdepartment);
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
                BranchDepartment branchdepartmentinput = new BranchDepartment();
                BranchDepartment branchdepartmentoutput = new BranchDepartment();
                PutOutput output = new PutOutput();
                branchdepartmentinput.CopyFrom(Input);
                BranchDepartment branchdepartment = _iBranchDepartmentRepository.GetBranchDepartment(branchdepartmentinput.Id);
                if (branchdepartment!=null)
                {
                    branchdepartmentoutput = _iBranchDepartmentRepository.UpdateBranchDepartment(branchdepartmentinput);
                    if(branchdepartmentoutput!=null)
                    {
                        output.CopyFrom(branchdepartmentoutput);
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
				 bool IsDeleted = _iBranchDepartmentRepository.DeleteBranchDepartment(id);
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
