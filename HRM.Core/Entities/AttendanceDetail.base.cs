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
	public abstract partial class AttendanceDetailBase:EntityBase, IEquatable<AttendanceDetailBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("AttendanceID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? AttendanceId{ get; set; }

		[FieldNameAttribute("AttendanceTypeID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? AttendanceTypeId{ get; set; }

		[FieldNameAttribute("StartDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? StartDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string StartDateStr
		{
			 get {if(StartDate.HasValue) return StartDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { StartDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("EndDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? EndDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string EndDateStr
		{
			 get {if(EndDate.HasValue) return EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { EndDate = date.ToUniversalTime();  }  } 
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

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<AttendanceDetailBase> Members

        public virtual bool Equals(AttendanceDetailBase other)
        {
			if(this.Id==other.Id  && this.AttendanceId==other.AttendanceId  && this.AttendanceTypeId==other.AttendanceTypeId  && this.StartDate==other.StartDate  && this.EndDate==other.EndDate  && this.IsActive==other.IsActive  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(AttendanceDetail other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.AttendanceId=other.AttendanceId;
				this.AttendanceTypeId=other.AttendanceTypeId;
				this.StartDate=other.StartDate;
				this.EndDate=other.EndDate;
				this.IsActive=other.IsActive;
				this.CreationDate=other.CreationDate;
				this.UpdateDate=other.UpdateDate;
				this.UpdateBy=other.UpdateBy;
				this.UserIp=other.UserIp;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
