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
	public abstract partial class LeaveBase:EntityBase, IEquatable<LeaveBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("UserID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UserId{ get; set; }

		[FieldNameAttribute("Date",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? Date{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string DateStr
		{
			 get {if(Date.HasValue) return Date.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { Date = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("Reason",true,false,500)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Reason{ get; set; }

		[FieldNameAttribute("LeaveTypeID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? LeaveTypeId{ get; set; }

		[FieldNameAttribute("IsActive",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsActive{ get; set; }

		[FieldNameAttribute("UpdateDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? UpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string UpdateDateStr
		{
			 get {if(UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdatedBy",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UpdatedBy{ get; set; }

		[FieldNameAttribute("UserIP",true,false,20)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String UserIp{ get; set; }

		[FieldNameAttribute("CreationDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}
        //[FieldNameAttribute("LeaveDateEnd", true, false, 8)]
        //[IgnoreDataMember]
        //public virtual System.DateTime? LeaveDateEnd { get; set; }
        //[DataMember(EmitDefaultValue = false)]
        //public virtual string LeaveDateEndStr
        //{
        //    get { if (LeaveDateEnd.HasValue) return LeaveDateEnd.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty; }
        //    set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { LeaveDateEnd = date.ToUniversalTime(); } }
        //}
        [FieldNameAttribute("IsApproved", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsApproved { get; set; }
        [FieldNameAttribute("IsReject", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsReject { get; set; }
        [FieldNameAttribute("AdminReason", true, false, 500)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.String AdminReason { get; set; }
        public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<LeaveBase> Members

        public virtual bool Equals(LeaveBase other)
        {
			if(this.Id==other.Id  && this.UserId==other.UserId  && this.Date==other.Date  && this.Reason==other.Reason  && this.LeaveTypeId==other.LeaveTypeId  && this.IsActive==other.IsActive  && this.UpdateDate==other.UpdateDate  && this.UpdatedBy==other.UpdatedBy  && this.UserIp==other.UserIp  && this.CreationDate==other.CreationDate && this.AdminReason == other.AdminReason && this.IsApproved == other.IsApproved && this.IsReject == other.IsReject)
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(Leave other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.UserId=other.UserId;
				this.Date=other.Date;
				this.Reason=other.Reason;
				this.LeaveTypeId=other.LeaveTypeId;
				this.IsActive=other.IsActive;
				this.UpdateDate=other.UpdateDate;
				this.UpdatedBy=other.UpdatedBy;
				this.UserIp=other.UserIp;
				this.CreationDate=other.CreationDate;
                this.IsApproved = other.IsApproved;
                this.IsReject = other.IsReject;
                this.AdminReason = other.AdminReason;
            }
			
		
		}

        #endregion
		
		
		
	}
	
	
}
