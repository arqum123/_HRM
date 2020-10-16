using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.UserContact;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class UserContactService : IUserContactService 
	{
		private IUserContactRepository _iUserContactRepository;
        
		public UserContactService(IUserContactRepository iUserContactRepository)
		{
			this._iUserContactRepository = iUserContactRepository;
		}
        
        public Dictionary<string, string> GetUserContactBasicSearchColumns()
        {
            
            return this._iUserContactRepository.GetUserContactBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetUserContactAdvanceSearchColumns()
        {
            
            return this._iUserContactRepository.GetUserContactAdvanceSearchColumns();
           
        }
        

		public virtual List<UserContact> GetUserContactByUserId(System.Int32? UserId)
		{
			return _iUserContactRepository.GetUserContactByUserId(UserId);
		}

		public virtual List<UserContact> GetUserContactByContactTypeId(System.Int32? ContactTypeId)
		{
			return _iUserContactRepository.GetUserContactByContactTypeId(ContactTypeId);
		}

		public UserContact GetUserContact(System.Int32 Id)
		{
			return _iUserContactRepository.GetUserContact(Id);
		}

		public UserContact UpdateUserContact(UserContact entity)
		{
			return _iUserContactRepository.UpdateUserContact(entity);
		}

		public UserContact UpdateUserContactByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iUserContactRepository.UpdateUserContactByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteUserContact(System.Int32 Id)
		{
			return _iUserContactRepository.DeleteUserContact(Id);
		}

		public List<UserContact> GetAllUserContact()
		{
			return _iUserContactRepository.GetAllUserContact();
		}

		public UserContact InsertUserContact(UserContact entity)
		{
			 return _iUserContactRepository.InsertUserContact(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				UserContact usercontact = _iUserContactRepository.GetUserContact(id);
                if(usercontact!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(usercontact);
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
            List<UserContact> usercontactlist = _iUserContactRepository.GetAllUserContact();
            if (usercontactlist != null && usercontactlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(usercontactlist);
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
                UserContact usercontact = new UserContact();
                PostOutput output = new PostOutput();
                usercontact.CopyFrom(Input);
                usercontact = _iUserContactRepository.InsertUserContact(usercontact);
                output.CopyFrom(usercontact);
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
                UserContact usercontactinput = new UserContact();
                UserContact usercontactoutput = new UserContact();
                PutOutput output = new PutOutput();
                usercontactinput.CopyFrom(Input);
                UserContact usercontact = _iUserContactRepository.GetUserContact(usercontactinput.Id);
                if (usercontact!=null)
                {
                    usercontactoutput = _iUserContactRepository.UpdateUserContact(usercontactinput);
                    if(usercontactoutput!=null)
                    {
                        output.CopyFrom(usercontactoutput);
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
				 bool IsDeleted = _iUserContactRepository.DeleteUserContact(id);
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
