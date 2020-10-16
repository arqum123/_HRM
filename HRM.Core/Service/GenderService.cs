using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Gender;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class GenderService : IGenderService 
	{
		private IGenderRepository _iGenderRepository;
        
		public GenderService(IGenderRepository iGenderRepository)
		{
			this._iGenderRepository = iGenderRepository;
		}
        
        public Dictionary<string, string> GetGenderBasicSearchColumns()
        {
            
            return this._iGenderRepository.GetGenderBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetGenderAdvanceSearchColumns()
        {
            
            return this._iGenderRepository.GetGenderAdvanceSearchColumns();
           
        }
        

		public Gender GetGender(System.Int32 Id)
		{
			return _iGenderRepository.GetGender(Id);
		}

		public Gender UpdateGender(Gender entity)
		{
			return _iGenderRepository.UpdateGender(entity);
		}

		public Gender UpdateGenderByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iGenderRepository.UpdateGenderByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteGender(System.Int32 Id)
		{
			return _iGenderRepository.DeleteGender(Id);
		}

		public List<Gender> GetAllGender()
		{
			return _iGenderRepository.GetAllGender();
		}

		public Gender InsertGender(Gender entity)
		{
			 return _iGenderRepository.InsertGender(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Gender gender = _iGenderRepository.GetGender(id);
                if(gender!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(gender);
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
            List<Gender> genderlist = _iGenderRepository.GetAllGender();
            if (genderlist != null && genderlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(genderlist);
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
                Gender gender = new Gender();
                PostOutput output = new PostOutput();
                gender.CopyFrom(Input);
                gender = _iGenderRepository.InsertGender(gender);
                output.CopyFrom(gender);
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
                Gender genderinput = new Gender();
                Gender genderoutput = new Gender();
                PutOutput output = new PutOutput();
                genderinput.CopyFrom(Input);
                Gender gender = _iGenderRepository.GetGender(genderinput.Id);
                if (gender!=null)
                {
                    genderoutput = _iGenderRepository.UpdateGender(genderinput);
                    if(genderoutput!=null)
                    {
                        output.CopyFrom(genderoutput);
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
				 bool IsDeleted = _iGenderRepository.DeleteGender(id);
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
