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
	public abstract partial class AttendanceBase:EntityBase, IEquatable<AttendanceBase>
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


        [FieldNameAttribute("DateTimeIn", true, false, 8)]
        [IgnoreDataMember]
        public virtual System.DateTime? DateTimeIn { get; set; }

        [FieldNameAttribute("DateTimeOut", true, false, 8)]
        [IgnoreDataMember]
        public virtual System.DateTime? DateTimeOut { get; set; }


        public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<AttendanceBase> Members

        public virtual bool Equals(AttendanceBase other)
        {
			if(this.Id==other.Id  && this.UserId==other.UserId  && this.Date==other.Date  && this.IsActive==other.IsActive  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp && this.DateTimeIn == other.DateTimeIn && this.DateTimeOut == other.DateTimeOut)
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(Attendance other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.UserId=other.UserId;
				this.Date=other.Date;
				this.IsActive=other.IsActive;
				this.CreationDate=other.CreationDate;
				this.UpdateDate=other.UpdateDate;
				this.UpdateBy=other.UpdateBy;
				this.UserIp=other.UserIp;
                this.DateTimeIn = other.DateTimeIn;
                this.DateTimeOut = other.DateTimeOut;
            }
			
		
		}

        #endregion
		
		
		
	}
	
	
}
