using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.UserShift;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class UserShiftService : IUserShiftService 
	{
		private IUserShiftRepository _iUserShiftRepository;
        
		public UserShiftService(IUserShiftRepository iUserShiftRepository)
		{
			this._iUserShiftRepository = iUserShiftRepository;
		}
        
        public Dictionary<string, string> GetUserShiftBasicSearchColumns()
        {
            
            return this._iUserShiftRepository.GetUserShiftBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetUserShiftAdvanceSearchColumns()
        {
            
            return this._iUserShiftRepository.GetUserShiftAdvanceSearchColumns();
           
        }
        

		public virtual List<UserShift> GetUserShiftByUserId(System.Int32? UserId)
		{
			return _iUserShiftRepository.GetUserShiftByUserId(UserId);
		}

		public virtual List<UserShift> GetUserShiftByShiftId(System.Int32? ShiftId)
		{
			return _iUserShiftRepository.GetUserShiftByShiftId(ShiftId);
		}

		public UserShift GetUserShift(System.Int32 Id)
		{
			return _iUserShiftRepository.GetUserShift(Id);
		}

		public UserShift UpdateUserShift(UserShift entity)
		{
			return _iUserShiftRepository.UpdateUserShift(entity);
		}

		public UserShift UpdateUserShiftByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iUserShiftRepository.UpdateUserShiftByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteUserShift(System.Int32 Id)
		{
			return _iUserShiftRepository.DeleteUserShift(Id);
		}

		public List<UserShift> GetAllUserShift()
		{
			return _iUserShiftRepository.GetAllUserShift();
		}

		public UserShift InsertUserShift(UserShift entity)
		{
			 return _iUserShiftRepository.InsertUserShift(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				UserShift usershift = _iUserShiftRepository.GetUserShift(id);
                if(usershift!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(usershift);
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
            List<UserShift> usershiftlist = _iUserShiftRepository.GetAllUserShift();
            if (usershiftlist != null && usershiftlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(usershiftlist);
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
                UserShift usershift = new UserShift();
                PostOutput output = new PostOutput();
                usershift.CopyFrom(Input);
                usershift = _iUserShiftRepository.InsertUserShift(usershift);
                output.CopyFrom(usershift);
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
                UserShift usershiftinput = new UserShift();
                UserShift usershiftoutput = new UserShift();
                PutOutput output = new PutOutput();
                usershiftinput.CopyFrom(Input);
                UserShift usershift = _iUserShiftRepository.GetUserShift(usershiftinput.Id);
                if (usershift!=null)
                {
                    usershiftoutput = _iUserShiftRepository.UpdateUserShift(usershiftinput);
                    if(usershiftoutput!=null)
                    {
                        output.CopyFrom(usershiftoutput);
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
				 bool IsDeleted = _iUserShiftRepository.DeleteUserShift(id);
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
