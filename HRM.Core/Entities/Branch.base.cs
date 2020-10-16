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
	public abstract partial class BranchBase:EntityBase, IEquatable<BranchBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("Name",true,false,500)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Name{ get; set; }

		[FieldNameAttribute("AddressLine",true,false,1000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String AddressLine{ get; set; }

		[FieldNameAttribute("CityID",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? CityId{ get; set; }

		[FieldNameAttribute("StateID",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? StateId{ get; set; }

		[FieldNameAttribute("CountryID",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? CountryId{ get; set; }

		[FieldNameAttribute("ZipCode",true,false,20)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String ZipCode{ get; set; }

		[FieldNameAttribute("PhoneNumber",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String PhoneNumber{ get; set; }

		[FieldNameAttribute("GUID",true,false,500)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Guid{ get; set; }

		[FieldNameAttribute("CreatedDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? CreatedDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string CreatedDateStr
		{
			 get {if(CreatedDate.HasValue) return CreatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreatedDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdationDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? UpdationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string UpdationDateStr
		{
			 get {if(UpdationDate.HasValue) return UpdationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdationDate = date.ToUniversalTime();  }  } 
		}

		[FieldNameAttribute("UpdatedBy",true,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UpdatedBy{ get; set; }

		[FieldNameAttribute("UserIP",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String UserIp{ get; set; }

		[FieldNameAttribute("IsActive",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsActive{ get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<BranchBase> Members

        public virtual bool Equals(BranchBase other)
        {
			if(this.Id==other.Id  && this.Name==other.Name  && this.AddressLine==other.AddressLine  && this.CityId==other.CityId  && this.StateId==other.StateId  && this.CountryId==other.CountryId  && this.ZipCode==other.ZipCode  && this.PhoneNumber==other.PhoneNumber  && this.Guid==other.Guid  && this.UpdationDate==other.UpdationDate  && this.UpdatedBy==other.UpdatedBy  && this.UserIp==other.UserIp  && this.IsActive==other.IsActive )
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(Branch other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.Name=other.Name;
				this.AddressLine=other.AddressLine;
				this.CityId=other.CityId;
				this.StateId=other.StateId;
				this.CountryId=other.CountryId;
				this.ZipCode=other.ZipCode;
				this.PhoneNumber=other.PhoneNumber;
				this.Guid=other.Guid;
				this.CreatedDate=other.CreatedDate;
				this.UpdationDate=other.UpdationDate;
				this.UpdatedBy=other.UpdatedBy;
				this.UserIp=other.UserIp;
				this.IsActive=other.IsActive;
			}
			
		
		}

        #endregion
		
		
		
	}
	
	
}
