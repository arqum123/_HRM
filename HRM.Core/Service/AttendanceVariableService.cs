using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendanceVariable;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class AttendanceVariableService : IAttendanceVariableService 
	{
		private IAttendanceVariableRepository _iAttendanceVariableRepository;
        
		public AttendanceVariableService(IAttendanceVariableRepository iAttendanceVariableRepository)
		{
			this._iAttendanceVariableRepository = iAttendanceVariableRepository;
		}
        
        public Dictionary<string, string> GetAttendanceVariableBasicSearchColumns()
        {
            
            return this._iAttendanceVariableRepository.GetAttendanceVariableBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetAttendanceVariableAdvanceSearchColumns()
        {
            
            return this._iAttendanceVariableRepository.GetAttendanceVariableAdvanceSearchColumns();
           
        }
        

		public AttendanceVariable GetAttendanceVariable(System.Int32 Id)
		{
			return _iAttendanceVariableRepository.GetAttendanceVariable(Id);
		}

		public AttendanceVariable UpdateAttendanceVariable(AttendanceVariable entity)
		{
			return _iAttendanceVariableRepository.UpdateAttendanceVariable(entity);
		}

		public AttendanceVariable UpdateAttendanceVariableByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iAttendanceVariableRepository.UpdateAttendanceVariableByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteAttendanceVariable(System.Int32 Id)
		{
			return _iAttendanceVariableRepository.DeleteAttendanceVariable(Id);
		}

		public List<AttendanceVariable> GetAllAttendanceVariable()
		{
			return _iAttendanceVariableRepository.GetAllAttendanceVariable();
		}

		public AttendanceVariable InsertAttendanceVariable(AttendanceVariable entity)
		{
			 return _iAttendanceVariableRepository.InsertAttendanceVariable(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				AttendanceVariable attendancevariable = _iAttendanceVariableRepository.GetAttendanceVariable(id);
                if(attendancevariable!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(attendancevariable);
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
            List<AttendanceVariable> attendancevariablelist = _iAttendanceVariableRepository.GetAllAttendanceVariable();
            if (attendancevariablelist != null && attendancevariablelist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(attendancevariablelist);
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
                AttendanceVariable attendancevariable = new AttendanceVariable();
                PostOutput output = new PostOutput();
                attendancevariable.CopyFrom(Input);
                attendancevariable = _iAttendanceVariableRepository.InsertAttendanceVariable(attendancevariable);
                output.CopyFrom(attendancevariable);
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
                AttendanceVariable attendancevariableinput = new AttendanceVariable();
                AttendanceVariable attendancevariableoutput = new AttendanceVariable();
                PutOutput output = new PutOutput();
                attendancevariableinput.CopyFrom(Input);
                AttendanceVariable attendancevariable = _iAttendanceVariableRepository.GetAttendanceVariable(attendancevariableinput.Id);
                if (attendancevariable!=null)
                {
                    attendancevariableoutput = _iAttendanceVariableRepository.UpdateAttendanceVariable(attendancevariableinput);
                    if(attendancevariableoutput!=null)
                    {
                        output.CopyFrom(attendancevariableoutput);
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
				 bool IsDeleted = _iAttendanceVariableRepository.DeleteAttendanceVariable(id);
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
