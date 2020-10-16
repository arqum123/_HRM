using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.AttendanceStatus
{
    [DataContract]
	public class PutOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? AttendanceId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsShiftOffDay{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsHoliday{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsLeaveDay{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsQuarterDay{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsHalfDay{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsFullDay{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Reason{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsApproved{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Remarks{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? ActionBy{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? ActionDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string ActionDateStr
		{
			 get {if(ActionDate.HasValue) return ActionDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { ActionDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsActive{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

		[IgnoreDataMember]
		public System.DateTime? UpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string UpdateDateStr
		{
			 get {if(UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UpdateBy{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String UserIp{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsLate{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsEarly{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? LateMinutes{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? EarlyMinutes{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? WorkingMinutes{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? TotalMinutes{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? OverTimeMinutes{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String BreakType{ get; set; }

	}	
}
