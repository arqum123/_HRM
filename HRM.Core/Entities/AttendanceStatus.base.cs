using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;

namespace HRM.Core.Entities
{
    [DataContract]
	public abstract partial class AttendanceStatusBase:EntityBase, IEquatable<AttendanceStatusBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("AttendanceID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? AttendanceId{ get; set; }

		[FieldNameAttribute("IsShiftOffDay",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsShiftOffDay{ get; set; }

		[FieldNameAttribute("IsHoliday",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsHoliday{ get; set; }

		[FieldNameAttribute("IsLeaveDay",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsLeaveDay{ get; set; }

		[FieldNameAttribute("IsQuarterDay",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsQuarterDau { get; set; }

		[FieldNameAttribute("IsHalfDay",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsHalfDay{ get; set; }

        [FieldNameAttribute("IsQuarterDay", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsQuarterDay { get; set; }

        [FieldNameAttribute("IsFullDay",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsFullDay{ get; set; }

		[FieldNameAttribute("Reason",true,false,2000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Reason{ get; set; }

		[FieldNameAttribute("IsApproved",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsApproved{ get; set; }

		[FieldNameAttribute("Remarks",true,false,2000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Remarks{ get; set; }

		[FieldNameAttribute("ActionBy",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? ActionBy{ get; set; }

		[FieldNameAttribute("ActionDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? ActionDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string ActionDateStr
		{
			 get {if(ActionDate.HasValue) return ActionDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { ActionDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("IsActive",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsActive{ get; set; }

		[FieldNameAttribute("CreationDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdateDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? UpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string UpdateDateStr
		{
			 get {if(UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdateBy",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UpdateBy{ get; set; }

		[FieldNameAttribute("UserIP",true,false,20)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String UserIp{ get; set; }

		[FieldNameAttribute("IsLate",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsLate{ get; set; }

		[FieldNameAttribute("IsEarly",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsEarly{ get; set; }

		[FieldNameAttribute("LateMinutes",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? LateMinutes{ get; set; }

		[FieldNameAttribute("EarlyMinutes",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? EarlyMinutes{ get; set; }

		[FieldNameAttribute("WorkingMinutes",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? WorkingMinutes{ get; set; }

		[FieldNameAttribute("TotalMinutes",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? TotalMinutes{ get; set; }

		[FieldNameAttribute("OverTimeMinutes",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? OverTimeMinutes{ get; set; }

		[FieldNameAttribute("BreakType",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String BreakType{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<AttendanceStatusBase> Members

        public virtual bool Equals(AttendanceStatusBase other)
        {
			if(this.Id==other.Id  && this.AttendanceId==other.AttendanceId  && this.IsShiftOffDay==other.IsShiftOffDay  && this.IsHoliday==other.IsHoliday  && this.IsLeaveDay==other.IsLeaveDay  && this.IsQuarterDay==other.IsQuarterDay  && this.IsHalfDay==other.IsHalfDay  && this.IsFullDay==other.IsFullDay  && this.Reason==other.Reason  && this.IsApproved==other.IsApproved  && this.Remarks==other.Remarks  && this.ActionBy==other.ActionBy  && this.ActionDate==other.ActionDate  && this.IsActive==other.IsActive  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp  && this.IsLate==other.IsLate  && this.IsEarly==other.IsEarly  && this.LateMinutes==other.LateMinutes  && this.EarlyMinutes==other.EarlyMinutes  && this.WorkingMinutes==other.WorkingMinutes  && this.TotalMinutes==other.TotalMinutes  && this.OverTimeMinutes==other.OverTimeMinutes  && this.BreakType==other.BreakType )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(AttendanceStatus other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.AttendanceId=other.AttendanceId;
				this.IsShiftOffDay=other.IsShiftOffDay;
				this.IsHoliday=other.IsHoliday;
				this.IsLeaveDay=other.IsLeaveDay;
				this.IsQuarterDay=other.IsQuarterDay;
				this.IsHalfDay=other.IsHalfDay;
				this.IsFullDay=other.IsFullDay;
				this.Reason=other.Reason;
				this.IsApproved=other.IsApproved;
				this.Remarks=other.Remarks;
				this.ActionBy=other.ActionBy;
				this.ActionDate=other.ActionDate;
				this.IsActive=other.IsActive;
				this.CreationDate=other.CreationDate;
				this.UpdateDate=other.UpdateDate;
				this.UpdateBy=other.UpdateBy;
				this.UserIp=other.UserIp;
				this.IsLate=other.IsLate;
				this.IsEarly=other.IsEarly;
				this.LateMinutes=other.LateMinutes;
				this.EarlyMinutes=other.EarlyMinutes;
				this.WorkingMinutes=other.WorkingMinutes;
				this.TotalMinutes=other.TotalMinutes;
				this.OverTimeMinutes=other.OverTimeMinutes;
				this.BreakType=other.BreakType;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
