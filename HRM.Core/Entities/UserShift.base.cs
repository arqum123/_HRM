﻿using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;

namespace HRM.Core.Entities
{
    [DataContract]
	public abstract partial class UserShiftBase:EntityBase, IEquatable<UserShiftBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("UserID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UserId{ get; set; }

		[FieldNameAttribute("ShiftID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? ShiftId{ get; set; }

		[FieldNameAttribute("EffectiveDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? EffectiveDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string EffectiveDateStr
		{
			 get {if(EffectiveDate.HasValue) return EffectiveDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { EffectiveDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("RetiredDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? RetiredDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string RetiredDateStr
		{
			 get {if(RetiredDate.HasValue) return RetiredDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { RetiredDate = date.ToUniversalTime();  }  } 
		}

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
		
		#region IEquatable<UserShiftBase> Members

        public virtual bool Equals(UserShiftBase other)
        {
			if(this.Id==other.Id  && this.UserId==other.UserId  && this.ShiftId==other.ShiftId  && this.EffectiveDate==other.EffectiveDate  && this.RetiredDate==other.RetiredDate  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(UserShift other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.UserId=other.UserId;
				this.ShiftId=other.ShiftId;
				this.EffectiveDate=other.EffectiveDate;
				this.RetiredDate=other.RetiredDate;
				this.CreationDate=other.CreationDate;
				this.UpdateDate=other.UpdateDate;
				this.UpdateBy=other.UpdateBy;
				this.UserIp=other.UserIp;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
