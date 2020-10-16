using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.Configuration;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class ConfigurationService : IConfigurationService 
	{
		private IConfigurationRepository _iConfigurationRepository;
        
		public ConfigurationService(IConfigurationRepository iConfigurationRepository)
		{
			this._iConfigurationRepository = iConfigurationRepository;
		}
        
        public Dictionary<string, string> GetConfigurationBasicSearchColumns()
        {
            
            return this._iConfigurationRepository.GetConfigurationBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetConfigurationAdvanceSearchColumns()
        {
            
            return this._iConfigurationRepository.GetConfigurationAdvanceSearchColumns();
           
        }
        

		public Configuration GetConfiguration(System.Int32 Id)
		{
			return _iConfigurationRepository.GetConfiguration(Id);
		}

		public Configuration UpdateConfiguration(Configuration entity)
		{
			return _iConfigurationRepository.UpdateConfiguration(entity);
		}

		public Configuration UpdateConfigurationByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iConfigurationRepository.UpdateConfigurationByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteConfiguration(System.Int32 Id)
		{
			return _iConfigurationRepository.DeleteConfiguration(Id);
		}

		public List<Configuration> GetAllConfiguration()
		{
			return _iConfigurationRepository.GetAllConfiguration();
		}

		public Configuration InsertConfiguration(Configuration entity)
		{
			 return _iConfigurationRepository.InsertConfiguration(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				Configuration configuration = _iConfigurationRepository.GetConfiguration(id);
                if(configuration!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(configuration);
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
            List<Configuration> configurationlist = _iConfigurationRepository.GetAllConfiguration();
            if (configurationlist != null && configurationlist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(configurationlist);
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
                Configuration configuration = new Configuration();
                PostOutput output = new PostOutput();
                configuration.CopyFrom(Input);
                configuration = _iConfigurationRepository.InsertConfiguration(configuration);
                output.CopyFrom(configuration);
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
                Configuration configurationinput = new Configuration();
                Configuration configurationoutput = new Configuration();
                PutOutput output = new PutOutput();
                configurationinput.CopyFrom(Input);
                Configuration configuration = _iConfigurationRepository.GetConfiguration(configurationinput.Id);
                if (configuration!=null)
                {
                    configurationoutput = _iConfigurationRepository.UpdateConfiguration(configurationinput);
                    if(configurationoutput!=null)
                    {
                        output.CopyFrom(configurationoutput);
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
				 bool IsDeleted = _iConfigurationRepository.DeleteConfiguration(id);
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
