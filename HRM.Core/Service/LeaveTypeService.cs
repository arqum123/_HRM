using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.LeaveType;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class LeaveTypeService : ILeaveTypeService 
	{
		private ILeaveTypeRepository _iLeaveTypeRepository;
        
		public LeaveTypeService(ILeaveTypeRepository iLeaveTypeRepository)
		{
			this._iLeaveTypeRepository = iLeaveTypeRepository;
		}
        
        public Dictionary<string, string> GetLeaveTypeBasicSearchColumns()
        {
            
            return this._iLeaveTypeRepository.GetLeaveTypeBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetLeaveTypeAdvanceSearchColumns()
        {
            
            return this._iLeaveTypeRepository.GetLeaveTypeAdvanceSearchColumns();
           
        }
        

		public LeaveType GetLeaveType(System.Int32 Id)
		{
			return _iLeaveTypeRepository.GetLeaveType(Id);
		}

		public LeaveType UpdateLeaveType(LeaveType entity)
		{
			return _iLeaveTypeRepository.UpdateLeaveType(entity);
		}

		public LeaveType UpdateLeaveTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iLeaveTypeRepository.UpdateLeaveTypeByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteLeaveType(System.Int32 Id)
		{
			return _iLeaveTypeRepository.DeleteLeaveType(Id);
		}

		public List<LeaveType> GetAllLeaveType()
		{
			return _iLeaveTypeRepository.GetAllLeaveType();
		}

		public LeaveType InsertLeaveType(LeaveType entity)
		{
			 return _iLeaveTypeRepository.InsertLeaveType(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				LeaveType leavetype = _iLeaveTypeRepository.GetLeaveType(id);
                if(leavetype!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(leavetype);
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
            List<LeaveType> leavetypelist = _iLeaveTypeRepository.GetAllLeaveType();
            if (leavetypelist != null && leavetypelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(leavetypelist);
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
                LeaveType leavetype = new LeaveType();
                PostOutput output = new PostOutput();
                leavetype.CopyFrom(Input);
                leavetype = _iLeaveTypeRepository.InsertLeaveType(leavetype);
                output.CopyFrom(leavetype);
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
                LeaveType leavetypeinput = new LeaveType();
                LeaveType leavetypeoutput = new LeaveType();
                PutOutput output = new PutOutput();
                leavetypeinput.CopyFrom(Input);
                LeaveType leavetype = _iLeaveTypeRepository.GetLeaveType(leavetypeinput.Id);
                if (leavetype!=null)
                {
                    leavetypeoutput = _iLeaveTypeRepository.UpdateLeaveType(leavetypeinput);
                    if(leavetypeoutput!=null)
                    {
                        output.CopyFrom(leavetypeoutput);
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
				 bool IsDeleted = _iLeaveTypeRepository.DeleteLeaveType(id);
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
