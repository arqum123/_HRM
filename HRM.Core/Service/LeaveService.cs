using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Leave;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class LeaveService : ILeaveService 
	{
		private ILeaveRepository _iLeaveRepository;
        
		public LeaveService(ILeaveRepository iLeaveRepository)
		{
			this._iLeaveRepository = iLeaveRepository;
		}
        
        public Dictionary<string, string> GetLeaveBasicSearchColumns()
        {
            
            return this._iLeaveRepository.GetLeaveBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetLeaveAdvanceSearchColumns()
        {
            
            return this._iLeaveRepository.GetLeaveAdvanceSearchColumns();
           
        }
        

		public virtual List<Leave> GetLeaveByUserId(System.Int32? UserId)
		{
			return _iLeaveRepository.GetLeaveByUserId(UserId);
		}

		public virtual List<Leave> GetLeaveByLeaveTypeId(System.Int32? LeaveTypeId)
		{
			return _iLeaveRepository.GetLeaveByLeaveTypeId(LeaveTypeId);
		}

		public Leave GetLeave(System.Int32 Id)
		{
			return _iLeaveRepository.GetLeave(Id);
		}

		public Leave UpdateLeave(Leave entity)
		{
			return _iLeaveRepository.UpdateLeave(entity);
		}

		public Leave UpdateLeaveByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iLeaveRepository.UpdateLeaveByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteLeave(System.Int32 Id)
		{
			return _iLeaveRepository.DeleteLeave(Id);
		}

		public List<Leave> GetAllLeave()
		{
			return _iLeaveRepository.GetAllLeave();
		}

		public Leave InsertLeave(Leave entity)
		{
			 return _iLeaveRepository.InsertLeave(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Leave leave = _iLeaveRepository.GetLeave(id);
                if(leave!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(leave);
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
            List<Leave> leavelist = _iLeaveRepository.GetAllLeave();
            if (leavelist != null && leavelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(leavelist);
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
                Leave leave = new Leave();
                PostOutput output = new PostOutput();
                leave.CopyFrom(Input);
                leave = _iLeaveRepository.InsertLeave(leave);
                output.CopyFrom(leave);
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
                Leave leaveinput = new Leave();
                Leave leaveoutput = new Leave();
                PutOutput output = new PutOutput();
                leaveinput.CopyFrom(Input);
                Leave leave = _iLeaveRepository.GetLeave(leaveinput.Id);
                if (leave!=null)
                {
                    leaveoutput = _iLeaveRepository.UpdateLeave(leaveinput);
                    if(leaveoutput!=null)
                    {
                        output.CopyFrom(leaveoutput);
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
				 bool IsDeleted = _iLeaveRepository.DeleteLeave(id);
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
