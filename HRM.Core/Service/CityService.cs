using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.City;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class CityService : ICityService 
	{
		private ICityRepository _iCityRepository;
        
		public CityService(ICityRepository iCityRepository)
		{
			this._iCityRepository = iCityRepository;
		}
        
        public Dictionary<string, string> GetCityBasicSearchColumns()
        {
            
            return this._iCityRepository.GetCityBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetCityAdvanceSearchColumns()
        {
            
            return this._iCityRepository.GetCityAdvanceSearchColumns();
           
        }
        

		public virtual List<City> GetCityByStateId(System.Int32? StateId)
		{
			return _iCityRepository.GetCityByStateId(StateId);
		}

		public City GetCity(System.Int32 Id)
		{
			return _iCityRepository.GetCity(Id);
		}

		public City UpdateCity(City entity)
		{
			return _iCityRepository.UpdateCity(entity);
		}

		public City UpdateCityByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iCityRepository.UpdateCityByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteCity(System.Int32 Id)
		{
			return _iCityRepository.DeleteCity(Id);
		}

		public List<City> GetAllCity()
		{
			return _iCityRepository.GetAllCity();
		}

		public City InsertCity(City entity)
		{
			 return _iCityRepository.InsertCity(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				City city = _iCityRepository.GetCity(id);
                if(city!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(city);
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
            List<City> citylist = _iCityRepository.GetAllCity();
            if (citylist != null && citylist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(citylist);
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
                City city = new City();
                PostOutput output = new PostOutput();
                city.CopyFrom(Input);
                city = _iCityRepository.InsertCity(city);
                output.CopyFrom(city);
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
                City cityinput = new City();
                City cityoutput = new City();
                PutOutput output = new PutOutput();
                cityinput.CopyFrom(Input);
                City city = _iCityRepository.GetCity(cityinput.Id);
                if (city!=null)
                {
                    cityoutput = _iCityRepository.UpdateCity(cityinput);
                    if(cityoutput!=null)
                    {
                        output.CopyFrom(cityoutput);
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
				 bool IsDeleted = _iCityRepository.DeleteCity(id);
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
