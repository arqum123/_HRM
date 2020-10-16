using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Country;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class CountryService : ICountryService 
	{
		private ICountryRepository _iCountryRepository;
        
		public CountryService(ICountryRepository iCountryRepository)
		{
			this._iCountryRepository = iCountryRepository;
		}
        
        public Dictionary<string, string> GetCountryBasicSearchColumns()
        {
            
            return this._iCountryRepository.GetCountryBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetCountryAdvanceSearchColumns()
        {
            
            return this._iCountryRepository.GetCountryAdvanceSearchColumns();
           
        }
        

		public Country GetCountry(System.Int32 Id)
		{
			return _iCountryRepository.GetCountry(Id);
		}

		public Country UpdateCountry(Country entity)
		{
			return _iCountryRepository.UpdateCountry(entity);
		}

		public Country UpdateCountryByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iCountryRepository.UpdateCountryByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteCountry(System.Int32 Id)
		{
			return _iCountryRepository.DeleteCountry(Id);
		}

		public List<Country> GetAllCountry()
		{
			return _iCountryRepository.GetAllCountry();
		}

		public Country InsertCountry(Country entity)
		{
			 return _iCountryRepository.InsertCountry(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Country country = _iCountryRepository.GetCountry(id);
                if(country!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(country);
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
            List<Country> countrylist = _iCountryRepository.GetAllCountry();
            if (countrylist != null && countrylist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(countrylist);
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
                Country country = new Country();
                PostOutput output = new PostOutput();
                country.CopyFrom(Input);
                country = _iCountryRepository.InsertCountry(country);
                output.CopyFrom(country);
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
                Country countryinput = new Country();
                Country countryoutput = new Country();
                PutOutput output = new PutOutput();
                countryinput.CopyFrom(Input);
                Country country = _iCountryRepository.GetCountry(countryinput.Id);
                if (country!=null)
                {
                    countryoutput = _iCountryRepository.UpdateCountry(countryinput);
                    if(countryoutput!=null)
                    {
                        output.CopyFrom(countryoutput);
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
				 bool IsDeleted = _iCountryRepository.DeleteCountry(id);
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
