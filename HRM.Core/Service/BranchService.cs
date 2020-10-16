using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Branch;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class BranchService : IBranchService 
	{
		private IBranchRepository _iBranchRepository;
        
		public BranchService(IBranchRepository iBranchRepository)
		{
			this._iBranchRepository = iBranchRepository;
		}
        
        public Dictionary<string, string> GetBranchBasicSearchColumns()
        {
            
            return this._iBranchRepository.GetBranchBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetBranchAdvanceSearchColumns()
        {
            
            return this._iBranchRepository.GetBranchAdvanceSearchColumns();
           
        }
        

		public Branch GetBranch(System.Int32 Id)
		{
			return _iBranchRepository.GetBranch(Id);
		}

		public Branch UpdateBranch(Branch entity)
		{
			return _iBranchRepository.UpdateBranch(entity);
		}

		public Branch UpdateBranchByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iBranchRepository.UpdateBranchByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteBranch(System.Int32 Id)
		{
			return _iBranchRepository.DeleteBranch(Id);
		}

		public List<Branch> GetAllBranch()
		{
			return _iBranchRepository.GetAllBranch();
		}

		public Branch InsertBranch(Branch entity)
		{
			 return _iBranchRepository.InsertBranch(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Branch branch = _iBranchRepository.GetBranch(id);
                if(branch!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(branch);
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
            List<Branch> branchlist = _iBranchRepository.GetAllBranch();
            if (branchlist != null && branchlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(branchlist);
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
                Branch branch = new Branch();
                PostOutput output = new PostOutput();
                branch.CopyFrom(Input);
                branch = _iBranchRepository.InsertBranch(branch);
                output.CopyFrom(branch);
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
                Branch branchinput = new Branch();
                Branch branchoutput = new Branch();
                PutOutput output = new PutOutput();
                branchinput.CopyFrom(Input);
                Branch branch = _iBranchRepository.GetBranch(branchinput.Id);
                if (branch!=null)
                {
                    branchoutput = _iBranchRepository.UpdateBranch(branchinput);
                    if(branchoutput!=null)
                    {
                        output.CopyFrom(branchoutput);
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
				 bool IsDeleted = _iBranchRepository.DeleteBranch(id);
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
