using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.State;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class StateService : IStateService 
	{
		private IStateRepository _iStateRepository;
        
		public StateService(IStateRepository iStateRepository)
		{
			this._iStateRepository = iStateRepository;
		}
        
        public Dictionary<string, string> GetStateBasicSearchColumns()
        {
            
            return this._iStateRepository.GetStateBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetStateAdvanceSearchColumns()
        {
            
            return this._iStateRepository.GetStateAdvanceSearchColumns();
           
        }
        

		public virtual List<State> GetStateByCountryId(System.Int32? CountryId)
		{
			return _iStateRepository.GetStateByCountryId(CountryId);
		}

		public State GetState(System.Int32 Id)
		{
			return _iStateRepository.GetState(Id);
		}

		public State UpdateState(State entity)
		{
			return _iStateRepository.UpdateState(entity);
		}

		public State UpdateStateByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iStateRepository.UpdateStateByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteState(System.Int32 Id)
		{
			return _iStateRepository.DeleteState(Id);
		}

		public List<State> GetAllState()
		{
			return _iStateRepository.GetAllState();
		}

		public State InsertState(State entity)
		{
			 return _iStateRepository.InsertState(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				State state = _iStateRepository.GetState(id);
                if(state!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(state);
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
            List<State> statelist = _iStateRepository.GetAllState();
            if (statelist != null && statelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(statelist);
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
                State state = new State();
                PostOutput output = new PostOutput();
                state.CopyFrom(Input);
                state = _iStateRepository.InsertState(state);
                output.CopyFrom(state);
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
                State stateinput = new State();
                State stateoutput = new State();
                PutOutput output = new PutOutput();
                stateinput.CopyFrom(Input);
                State state = _iStateRepository.GetState(stateinput.Id);
                if (state!=null)
                {
                    stateoutput = _iStateRepository.UpdateState(stateinput);
                    if(stateoutput!=null)
                    {
                        output.CopyFrom(stateoutput);
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
				 bool IsDeleted = _iStateRepository.DeleteState(id);
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
