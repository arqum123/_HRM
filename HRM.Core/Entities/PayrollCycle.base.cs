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
	public abstract partial class PayrollCycleBase:EntityBase, IEquatable<PayrollCycleBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("Name",true,false,100)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Name{ get; set; }

		[FieldNameAttribute("Month",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? Month{ get; set; }

		[FieldNameAttribute("Year",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? Year{ get; set; }

		[FieldNameAttribute("IsActive",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsActive{ get; set; }

        //New
        [FieldNameAttribute("IsFinal", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsFinal { get; set; }

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
		
		#region IEquatable<PayrollCycleBase> Members

        public virtual bool Equals(PayrollCycleBase other)
        {
			if(this.Id==other.Id  && this.Name==other.Name  && this.Month==other.Month  && this.Year==other.Year  && this.IsActive==other.IsActive  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp && this.IsFinal == other.IsFinal)
            {
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(PayrollCycle other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.Name=other.Name;
				this.Month=other.Month;
				this.Year=other.Year;
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
