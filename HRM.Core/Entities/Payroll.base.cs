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
	public abstract partial class PayrollBase:EntityBase, IEquatable<PayrollBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("PayrollCycleID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? PayrollCycleId{ get; set; }

		[FieldNameAttribute("UserID",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UserId{ get; set; }

		[FieldNameAttribute("Salary",true,false,5)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Decimal? Salary{ get; set; }

		[FieldNameAttribute("Addition",true,false,5)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Decimal? Addition{ get; set; }

		[FieldNameAttribute("Deduction",true,false,5)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Decimal? Deduction{ get; set; }

		[FieldNameAttribute("NetSalary",true,false,5)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Decimal? NetSalary{ get; set; }

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
		
		#region IEquatable<PayrollBase> Members

        public virtual bool Equals(PayrollBase other)
        {
			if(this.Id==other.Id  && this.PayrollCycleId==other.PayrollCycleId  && this.UserId==other.UserId  && this.Salary==other.Salary  && this.Addition==other.Addition  && this.Deduction==other.Deduction  && this.NetSalary==other.NetSalary  && this.IsActive==other.IsActive  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(Payroll other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.PayrollCycleId=other.PayrollCycleId;
				this.UserId=other.UserId;
				this.Salary=other.Salary;
				this.Addition=other.Addition;
				this.Deduction=other.Deduction;
				this.NetSalary=other.NetSalary;
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
