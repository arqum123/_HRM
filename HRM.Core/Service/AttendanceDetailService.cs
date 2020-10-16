using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendanceDetail;
using Validation;
using System.Linq;

namespace HRM.Core.Service
{
		
	public class AttendanceDetailService : IAttendanceDetailService 
	{
		private IAttendanceDetailRepository _iAttendanceDetailRepository;
        
		public AttendanceDetailService(IAttendanceDetailRepository iAttendanceDetailRepository)
		{
			this._iAttendanceDetailRepository = iAttendanceDetailRepository;
		}
        
        public Dictionary<string, string> GetAttendanceDetailBasicSearchColumns()
        {
            
            return this._iAttendanceDetailRepository.GetAttendanceDetailBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetAttendanceDetailAdvanceSearchColumns()
        {
            
            return this._iAttendanceDetailRepository.GetAttendanceDetailAdvanceSearchColumns();
           
        }
        

		public virtual List<AttendanceDetail> GetAttendanceDetailByAttendanceId(System.Int32? AttendanceId)
		{
			return _iAttendanceDetailRepository.GetAttendanceDetailByAttendanceId(AttendanceId);
		}

		public virtual List<AttendanceDetail> GetAttendanceDetailByAttendanceTypeId(System.Int32? AttendanceTypeId)
		{
			return _iAttendanceDetailRepository.GetAttendanceDetailByAttendanceTypeId(AttendanceTypeId);
		}

		public AttendanceDetail GetAttendanceDetail(System.Int32 Id)
		{
			return _iAttendanceDetailRepository.GetAttendanceDetail(Id);
		}

		public AttendanceDetail UpdateAttendanceDetail(AttendanceDetail entity)
		{
			return _iAttendanceDetailRepository.UpdateAttendanceDetail(entity);
		}

		public AttendanceDetail UpdateAttendanceDetailByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iAttendanceDetailRepository.UpdateAttendanceDetailByKeyValue(UpdateKeyValue,Id);
		}

		public bool DeleteAttendanceDetail(System.Int32 Id)
		{
			return _iAttendanceDetailRepository.DeleteAttendanceDetail(Id);
		}

		public List<AttendanceDetail> GetAllAttendanceDetail()
		{
			return _iAttendanceDetailRepository.GetAllAttendanceDetail();
		}

		public AttendanceDetail InsertAttendanceDetail(AttendanceDetail entity)
		{
			 return _iAttendanceDetailRepository.InsertAttendanceDetail(entity);
		}

        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				AttendanceDetail attendancedetail = _iAttendanceDetailRepository.GetAttendanceDetail(id);
                if(attendancedetail!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(attendancedetail);
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
            List<AttendanceDetail> attendancedetaillist = _iAttendanceDetailRepository.GetAllAttendanceDetail();
            if (attendancedetaillist != null && attendancedetaillist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(attendancedetaillist);
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
                AttendanceDetail attendancedetail = new AttendanceDetail();
                PostOutput output = new PostOutput();
                attendancedetail.CopyFrom(Input);
                attendancedetail = _iAttendanceDetailRepository.InsertAttendanceDetail(attendancedetail);
                output.CopyFrom(attendancedetail);
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
                AttendanceDetail attendancedetailinput = new AttendanceDetail();
                AttendanceDetail attendancedetailoutput = new AttendanceDetail();
                PutOutput output = new PutOutput();
                attendancedetailinput.CopyFrom(Input);
                AttendanceDetail attendancedetail = _iAttendanceDetailRepository.GetAttendanceDetail(attendancedetailinput.Id);
                if (attendancedetail!=null)
                {
                    attendancedetailoutput = _iAttendanceDetailRepository.UpdateAttendanceDetail(attendancedetailinput);
                    if(attendancedetailoutput!=null)
                    {
                        output.CopyFrom(attendancedetailoutput);
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
				 bool IsDeleted = _iAttendanceDetailRepository.DeleteAttendanceDetail(id);
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
