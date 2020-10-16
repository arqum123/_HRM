using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.SalaryType;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class SalaryTypeService : ISalaryTypeService 
	{
		private ISalaryTypeRepository _iSalaryTypeRepository;
        
		public SalaryTypeService(ISalaryTypeRepository iSalaryTypeRepository)
		{
			this._iSalaryTypeRepository = iSalaryTypeRepository;
		}
        
        public Dictionary<string, string> GetSalaryTypeBasicSearchColumns()
        {
            
            return this._iSalaryTypeRepository.GetSalaryTypeBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetSalaryTypeAdvanceSearchColumns()
        {
            
            return this._iSalaryTypeRepository.GetSalaryTypeAdvanceSearchColumns();
           
        }
        

		public SalaryType GetSalaryType(System.Int32 Id)
		{
			return _iSalaryTypeRepository.GetSalaryType(Id);
		}

		public SalaryType UpdateSalaryType(SalaryType entity)
		{
			return _iSalaryTypeRepository.UpdateSalaryType(entity);
		}

		public SalaryType UpdateSalaryTypeByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iSalaryTypeRepository.UpdateSalaryTypeByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteSalaryType(System.Int32 Id)
		{
			return _iSalaryTypeRepository.DeleteSalaryType(Id);
		}

		public List<SalaryType> GetAllSalaryType()
		{
			return _iSalaryTypeRepository.GetAllSalaryType();
		}

		public SalaryType InsertSalaryType(SalaryType entity)
		{
			 return _iSalaryTypeRepository.InsertSalaryType(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				SalaryType salarytype = _iSalaryTypeRepository.GetSalaryType(id);
                if(salarytype!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(salarytype);
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
            List<SalaryType> salarytypelist = _iSalaryTypeRepository.GetAllSalaryType();
            if (salarytypelist != null && salarytypelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(salarytypelist);
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
                SalaryType salarytype = new SalaryType();
                PostOutput output = new PostOutput();
                salarytype.CopyFrom(Input);
                salarytype = _iSalaryTypeRepository.InsertSalaryType(salarytype);
                output.CopyFrom(salarytype);
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
                SalaryType salarytypeinput = new SalaryType();
                SalaryType salarytypeoutput = new SalaryType();
                PutOutput output = new PutOutput();
                salarytypeinput.CopyFrom(Input);
                SalaryType salarytype = _iSalaryTypeRepository.GetSalaryType(salarytypeinput.Id);
                if (salarytype!=null)
                {
                    salarytypeoutput = _iSalaryTypeRepository.UpdateSalaryType(salarytypeinput);
                    if(salarytypeoutput!=null)
                    {
                        output.CopyFrom(salarytypeoutput);
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
				 bool IsDeleted = _iSalaryTypeRepository.DeleteSalaryType(id);
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
