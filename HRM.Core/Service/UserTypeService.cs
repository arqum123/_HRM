using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.UserType;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class UserTypeService : IUserTypeService 
	{
		private IUserTypeRepository _iUserTypeRepository;
        
		public UserTypeService(IUserTypeRepository iUserTypeRepository)
		{
			this._iUserTypeRepository = iUserTypeRepository;
		}
        
        public Dictionary<string, string> GetUserTypeBasicSearchColumns()
        {
            
            return this._iUserTypeRepository.GetUserTypeBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetUserTypeAdvanceSearchColumns()
        {
            
            return this._iUserTypeRepository.GetUserTypeAdvanceSearchColumns();
           
        }
        

		public UserType GetUserType(System.Int32 Id)
		{
			return _iUserTypeRepository.GetUserType(Id);
		}

		public UserType UpdateUserType(UserType entity)
		{
			return _iUserTypeRepository.UpdateUserType(entity);
		}

		public UserType UpdateUserTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iUserTypeRepository.UpdateUserTypeByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteUserType(System.Int32 Id)
		{
			return _iUserTypeRepository.DeleteUserType(Id);
		}

		public List<UserType> GetAllUserType()
		{
			return _iUserTypeRepository.GetAllUserType();
		}

		public UserType InsertUserType(UserType entity)
		{
			 return _iUserTypeRepository.InsertUserType(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				UserType usertype = _iUserTypeRepository.GetUserType(id);
                if(usertype!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(usertype);
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
            List<UserType> usertypelist = _iUserTypeRepository.GetAllUserType();
            if (usertypelist != null && usertypelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(usertypelist);
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
                UserType usertype = new UserType();
                PostOutput output = new PostOutput();
                usertype.CopyFrom(Input);
                usertype = _iUserTypeRepository.InsertUserType(usertype);
                output.CopyFrom(usertype);
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
                UserType usertypeinput = new UserType();
                UserType usertypeoutput = new UserType();
                PutOutput output = new PutOutput();
                usertypeinput.CopyFrom(Input);
                UserType usertype = _iUserTypeRepository.GetUserType(usertypeinput.Id);
                if (usertype!=null)
                {
                    usertypeoutput = _iUserTypeRepository.UpdateUserType(usertypeinput);
                    if(usertypeoutput!=null)
                    {
                        output.CopyFrom(usertypeoutput);
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
				 bool IsDeleted = _iUserTypeRepository.DeleteUserType(id);
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
