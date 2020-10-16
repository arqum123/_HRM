using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.BranchShift;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class BranchShiftService : IBranchShiftService 
	{
		private IBranchShiftRepository _iBranchShiftRepository;
        
		public BranchShiftService(IBranchShiftRepository iBranchShiftRepository)
		{
			this._iBranchShiftRepository = iBranchShiftRepository;
		}
        
        public Dictionary<string, string> GetBranchShiftBasicSearchColumns()
        {
            
            return this._iBranchShiftRepository.GetBranchShiftBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetBranchShiftAdvanceSearchColumns()
        {
            
            return this._iBranchShiftRepository.GetBranchShiftAdvanceSearchColumns();
           
        }
        

		public virtual List<BranchShift> GetBranchShiftByBranchId(System.Int32? BranchId)
		{
			return _iBranchShiftRepository.GetBranchShiftByBranchId(BranchId);
		}

		public virtual List<BranchShift> GetBranchShiftByShiftId(System.Int32? ShiftId)
		{
			return _iBranchShiftRepository.GetBranchShiftByShiftId(ShiftId);
		}

		public BranchShift GetBranchShift(System.Int32 Id)
		{
			return _iBranchShiftRepository.GetBranchShift(Id);
		}

		public BranchShift UpdateBranchShift(BranchShift entity)
		{
			return _iBranchShiftRepository.UpdateBranchShift(entity);
		}

		public BranchShift UpdateBranchShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iBranchShiftRepository.UpdateBranchShiftByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteBranchShift(System.Int32 Id)
		{
			return _iBranchShiftRepository.DeleteBranchShift(Id);
		}

		public List<BranchShift> GetAllBranchShift()
		{
			return _iBranchShiftRepository.GetAllBranchShift();
		}

		public BranchShift InsertBranchShift(BranchShift entity)
		{
			 return _iBranchShiftRepository.InsertBranchShift(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				BranchShift branchshift = _iBranchShiftRepository.GetBranchShift(id);
                if(branchshift!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(branchshift);
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
            List<BranchShift> branchshiftlist = _iBranchShiftRepository.GetAllBranchShift();
            if (branchshiftlist != null && branchshiftlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(branchshiftlist);
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
                BranchShift branchshift = new BranchShift();
                PostOutput output = new PostOutput();
                branchshift.CopyFrom(Input);
                branchshift = _iBranchShiftRepository.InsertBranchShift(branchshift);
                output.CopyFrom(branchshift);
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
                BranchShift branchshiftinput = new BranchShift();
                BranchShift branchshiftoutput = new BranchShift();
                PutOutput output = new PutOutput();
                branchshiftinput.CopyFrom(Input);
                BranchShift branchshift = _iBranchShiftRepository.GetBranchShift(branchshiftinput.Id);
                if (branchshift!=null)
                {
                    branchshiftoutput = _iBranchShiftRepository.UpdateBranchShift(branchshiftinput);
                    if(branchshiftoutput!=null)
                    {
                        output.CopyFrom(branchshiftoutput);
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
				 bool IsDeleted = _iBranchShiftRepository.DeleteBranchShift(id);
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
