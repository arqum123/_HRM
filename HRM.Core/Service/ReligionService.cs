using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Religion;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class ReligionService : IReligionService 
	{
		private IReligionRepository _iReligionRepository;
        
		public ReligionService(IReligionRepository iReligionRepository)
		{
			this._iReligionRepository = iReligionRepository;
		}
        
        public Dictionary<string, string> GetReligionBasicSearchColumns()
        {
            
            return this._iReligionRepository.GetReligionBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetReligionAdvanceSearchColumns()
        {
            
            return this._iReligionRepository.GetReligionAdvanceSearchColumns();
           
        }
        

		public Religion GetReligion(System.Int32 Id)
		{
			return _iReligionRepository.GetReligion(Id);
		}

		public Religion UpdateReligion(Religion entity)
		{
			return _iReligionRepository.UpdateReligion(entity);
		}

		public Religion UpdateReligionByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iReligionRepository.UpdateReligionByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteReligion(System.Int32 Id)
		{
			return _iReligionRepository.DeleteReligion(Id);
		}

		public List<Religion> GetAllReligion()
		{
			return _iReligionRepository.GetAllReligion();
		}

		public Religion InsertReligion(Religion entity)
		{
			 return _iReligionRepository.InsertReligion(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Religion religion = _iReligionRepository.GetReligion(id);
                if(religion!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(religion);
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
            List<Religion> religionlist = _iReligionRepository.GetAllReligion();
            if (religionlist != null && religionlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(religionlist);
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
                Religion religion = new Religion();
                PostOutput output = new PostOutput();
                religion.CopyFrom(Input);
                religion = _iReligionRepository.InsertReligion(religion);
                output.CopyFrom(religion);
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
                Religion religioninput = new Religion();
                Religion religionoutput = new Religion();
                PutOutput output = new PutOutput();
                religioninput.CopyFrom(Input);
                Religion religion = _iReligionRepository.GetReligion(religioninput.Id);
                if (religion!=null)
                {
                    religionoutput = _iReligionRepository.UpdateReligion(religioninput);
                    if(religionoutput!=null)
                    {
                        output.CopyFrom(religionoutput);
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
				 bool IsDeleted = _iReligionRepository.DeleteReligion(id);
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
