using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.UserDepartment;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class UserDepartmentService : IUserDepartmentService 
	{
		private IUserDepartmentRepository _iUserDepartmentRepository;
        
		public UserDepartmentService(IUserDepartmentRepository iUserDepartmentRepository)
		{
			this._iUserDepartmentRepository = iUserDepartmentRepository;
		}
        
        public Dictionary<string, string> GetUserDepartmentBasicSearchColumns()
        {
            
            return this._iUserDepartmentRepository.GetUserDepartmentBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetUserDepartmentAdvanceSearchColumns()
        {
            
            return this._iUserDepartmentRepository.GetUserDepartmentAdvanceSearchColumns();
           
        }
        

		public virtual List<UserDepartment> GetUserDepartmentByUserId(System.Int32? UserId)
		{
			return _iUserDepartmentRepository.GetUserDepartmentByUserId(UserId);
		}

		public virtual List<UserDepartment> GetUserDepartmentByDepartmentId(System.Int32? DepartmentId)
		{
			return _iUserDepartmentRepository.GetUserDepartmentByDepartmentId(DepartmentId);
		}

		public UserDepartment GetUserDepartment(System.Int32 Id)
		{
			return _iUserDepartmentRepository.GetUserDepartment(Id);
		}

		public UserDepartment UpdateUserDepartment(UserDepartment entity)
		{
			return _iUserDepartmentRepository.UpdateUserDepartment(entity);
		}

		public UserDepartment UpdateUserDepartmentByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iUserDepartmentRepository.UpdateUserDepartmentByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteUserDepartment(System.Int32 Id)
		{
			return _iUserDepartmentRepository.DeleteUserDepartment(Id);
		}

		public List<UserDepartment> GetAllUserDepartment()
		{
			return _iUserDepartmentRepository.GetAllUserDepartment();
		}

		public UserDepartment InsertUserDepartment(UserDepartment entity)
		{
			 return _iUserDepartmentRepository.InsertUserDepartment(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				UserDepartment userdepartment = _iUserDepartmentRepository.GetUserDepartment(id);
                if(userdepartment!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(userdepartment);
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
            List<UserDepartment> userdepartmentlist = _iUserDepartmentRepository.GetAllUserDepartment();
            if (userdepartmentlist != null && userdepartmentlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(userdepartmentlist);
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
                UserDepartment userdepartment = new UserDepartment();
                PostOutput output = new PostOutput();
                userdepartment.CopyFrom(Input);
                userdepartment = _iUserDepartmentRepository.InsertUserDepartment(userdepartment);
                output.CopyFrom(userdepartment);
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
                UserDepartment userdepartmentinput = new UserDepartment();
                UserDepartment userdepartmentoutput = new UserDepartment();
                PutOutput output = new PutOutput();
                userdepartmentinput.CopyFrom(Input);
                UserDepartment userdepartment = _iUserDepartmentRepository.GetUserDepartment(userdepartmentinput.Id);
                if (userdepartment!=null)
                {
                    userdepartmentoutput = _iUserDepartmentRepository.UpdateUserDepartment(userdepartmentinput);
                    if(userdepartmentoutput!=null)
                    {
                        output.CopyFrom(userdepartmentoutput);
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
				 bool IsDeleted = _iUserDepartmentRepository.DeleteUserDepartment(id);
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
