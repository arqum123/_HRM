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
	public abstract partial class PayrollPolicyBase:EntityBase, IEquatable<PayrollPolicyBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("PayrollVariableID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? PayrollVariableId{ get; set; }

		[FieldNameAttribute("IsUnit",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsUnit{ get; set; }

		[FieldNameAttribute("IsPercentage",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsPercentage{ get; set; }


        [FieldNameAttribute("IsDay", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsDay { get; set; }

        [FieldNameAttribute("Value", true, false, 5)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Decimal? Value{ get; set; }

		[FieldNameAttribute("Description",true,false,1000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Description{ get; set; }

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

        //New
        [FieldNameAttribute("IsAttendance", true, false, 1)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Boolean? IsAttendance { get; set; }

        [FieldNameAttribute("SalaryType", true, false, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32? SalaryType { get; set; }

        [FieldNameAttribute("Occurance", true, false, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32? Occurance { get; set; }

        public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<PayrollPolicyBase> Members

        public virtual bool Equals(PayrollPolicyBase other)
        {
			if(this.Id==other.Id  && this.PayrollVariableId==other.PayrollVariableId  && this.IsUnit==other.IsUnit  && this.IsPercentage==other.IsPercentage  && this.Value==other.Value  && this.Description==other.Description  && this.EffectiveDate==other.EffectiveDate  && this.RetiredDate==other.RetiredDate  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(PayrollPolicy other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.PayrollVariableId=other.PayrollVariableId;
				this.IsUnit=other.IsUnit;
				this.IsPercentage=other.IsPercentage;
				this.Value=other.Value;
				this.Description=other.Description;
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
