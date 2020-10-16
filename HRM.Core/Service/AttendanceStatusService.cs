using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using HRM.Core.Entities;
using HRM.Core.DataInterfaces;
using HRM.Core.IService;
using HRM.Core.DataTransfer;
using HRM.Core.DataTransfer.AttendanceStatus;
using Validation;
using System.Linq;
using System.Data;
using HRM.Core.Model;

namespace HRM.Core.Service
{
		
	public class AttendanceStatusService : IAttendanceStatusService 
	{
		private IAttendanceStatusRepository _iAttendanceStatusRepository;

        public AttendanceStatusService(IAttendanceStatusRepository iAttendanceStatusRepository)
		{
			this._iAttendanceStatusRepository = iAttendanceStatusRepository;
		}
        
        public Dictionary<string, string> GetAttendanceStatusBasicSearchColumns()
        {
            
            return this._iAttendanceStatusRepository.GetAttendanceStatusBasicSearchColumns();
           
        }
        
        public List<SearchColumn> GetAttendanceStatusAdvanceSearchColumns()
        {
            
            return this._iAttendanceStatusRepository.GetAttendanceStatusAdvanceSearchColumns();
           
        }


        public virtual List<AttendanceStatus> GetAttendanceStatusByAttendanceId(System.Int32? AttendanceId)
		{
			return _iAttendanceStatusRepository.GetAttendanceStatusByAttendanceId(AttendanceId);
		}
        //NEw
        public virtual List<AttendanceStatus> GetAttendanceStatusByDateRangeSharp(DateTime dtStart, DateTime dtEnd)
        {
            return _iAttendanceStatusRepository.GetAttendanceStatusByDateRangeSharp(dtStart, dtEnd);
        }

        public AttendanceStatus GetAttendanceStatus(System.Int32 Id)
		{
			return _iAttendanceStatusRepository.GetAttendanceStatus(Id);
		}

		public AttendanceStatus UpdateAttendanceStatus(AttendanceStatus entity)
		{
			return _iAttendanceStatusRepository.UpdateAttendanceStatus(entity);
		}

		public AttendanceStatus UpdateAttendanceStatusByKeyValue (Dictionary<string, string> UpdateKeyValue, System.Int32 Id)
		{
			return _iAttendanceStatusRepository.UpdateAttendanceStatusByKeyValue(UpdateKeyValue,Id);

		}

		public bool DeleteAttendanceStatus(System.Int32 Id)
		{
			return _iAttendanceStatusRepository.DeleteAttendanceStatus(Id);
		}

		public List<AttendanceStatus> GetAllAttendanceStatus()
		{
			return _iAttendanceStatusRepository.GetAllAttendanceStatus();
		}

        public List<AttendanceStatus> GetAllAttendanceStatusStartAndEndDate(System.String StartDate,System.String EndDate)
        {
            return _iAttendanceStatusRepository.GetAllAttendanceStatusStartAndEndDate(StartDate,EndDate);
        }
      
        public AttendanceStatus InsertAttendanceStatus(AttendanceStatus entity)
		{
			 return _iAttendanceStatusRepository.InsertAttendanceStatus(entity);
		}


        public DataTransfer<GetOutput> Get(string _id)
        {
            DataTransfer<GetOutput> tranfer = new DataTransfer<GetOutput>();
            System.Int32 id=0;
            if (!string.IsNullOrEmpty(_id) && System.Int32.TryParse(_id,out id))
            {
				AttendanceStatus attendancestatus = _iAttendanceStatusRepository.GetAttendanceStatus(id);
                if(attendancestatus!=null)
                {
                    tranfer.IsSuccess = true;
                    GetOutput output = new GetOutput();
                    output.CopyFrom(attendancestatus);
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
            List<AttendanceStatus> attendancestatuslist = _iAttendanceStatusRepository.GetAllAttendanceStatus();
            if (attendancestatuslist != null && attendancestatuslist.Count>0)
            {
                tranfer.IsSuccess = true;
                List<GetOutput> outputlist = new List<GetOutput>();
                outputlist.CopyFrom(attendancestatuslist);
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
                AttendanceStatus attendancestatus = new AttendanceStatus();
                PostOutput output = new PostOutput();
                attendancestatus.CopyFrom(Input);
                attendancestatus = _iAttendanceStatusRepository.InsertAttendanceStatus(attendancestatus);
                output.CopyFrom(attendancestatus);
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
        //public List<VMAttendanceStatusSummary> GetAttendanceStatusByDate(DateTime StartDate, DateTime EndDate)
        //{
        //    List<VMAttendanceStatusSummary> attendanceSummary = new List<VMAttendanceStatusSummary>();
        //    DataSet dsAttendance = _iAttendanceStatusRepository.GetAttendanceStatusByDateRange(StartDate, EndDate);
        //    if (dsAttendance != null && dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0] != null)
        //    {
        //        //dsAttendance.Tables[0] // Attendance and AttendanceStatus
        //        //dsAttendance.Tables[1] // AttendanceDetail  
        //        foreach (DataRow dr in dsAttendance.Tables[0].Rows)
        //        {
        //            VMAttendanceStatusSummary objAttendance = new VMAttendanceStatusSummary()
        //            {
        //                AttendanceStatusId = Convert.ToInt32(dr["AttendanceStatusId"]),
        //                IsShiftOffDay = Convert.ToBoolean(dr["IsShiftOffDay"]),
        //                IsHoliday = Convert.ToBoolean(dr["IsHoliday"]),
        //                IsLeaveDay = Convert.ToBoolean(dr["IsLeaveDay"]),
        //                IsQuarterDay = Convert.ToBoolean(dr["IsQuarterDay"]),
        //                IsHalfDay = Convert.ToBoolean(dr["IsHalfDay"]),
        //                IsFullDay = Convert.ToBoolean(dr["IsFullDay"]),
        //                IsApproved = Convert.ToBoolean(dr["IsApproved"]),
        //                IsLate = Convert.ToBoolean(dr["IsLate"]),
        //                IsEarly = Convert.ToBoolean(dr["IsEarly"]),
        //                IsActive = Convert.ToBoolean(dr["IsActive"]),
        //            };
        //            attendanceSummary.Add(objAttendance);
        //        }
        //    }
        //    return attendanceSummary;
        //}
        public DataTransfer<PutOutput> Update(PutInput Input)
        {
            DataTransfer<PutOutput> transer = new DataTransfer<PutOutput>();
            IList<string> errors = Validator.Validate(Input);
            if (errors.Count == 0)
            {
                AttendanceStatus attendancestatusinput = new AttendanceStatus();
                AttendanceStatus attendancestatusoutput = new AttendanceStatus();
                PutOutput output = new PutOutput();
                attendancestatusinput.CopyFrom(Input);
                AttendanceStatus attendancestatus = _iAttendanceStatusRepository.GetAttendanceStatus(attendancestatusinput.Id);
                if (attendancestatus!=null)
                {
                    attendancestatusoutput = _iAttendanceStatusRepository.UpdateAttendanceStatus(attendancestatusinput);
                    if(attendancestatusoutput!=null)
                    {
                        output.CopyFrom(attendancestatusoutput);
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
				 bool IsDeleted = _iAttendanceStatusRepository.DeleteAttendanceStatus(id);
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
