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
	public abstract partial class UserBase:EntityBase, IEquatable<UserBase>
	{
			
		[PrimaryKey]
		[FieldNameAttribute("ID",false,false,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32 Id{ get; set; }

		[FieldNameAttribute("FirstName",true,false,200)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String FirstName{ get; set; }

		[FieldNameAttribute("MiddleName",true,false,200)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String MiddleName{ get; set; }

		[FieldNameAttribute("LastName",true,false,200)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String LastName{ get; set; }

		[FieldNameAttribute("UserTypeID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? UserTypeId{ get; set; }

		[FieldNameAttribute("GenderID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? GenderId{ get; set; }

		[FieldNameAttribute("DateOfBirth",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? DateOfBirth{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public virtual string DateOfBirthStr
		{
			 get {if(DateOfBirth.HasValue) return DateOfBirth.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { DateOfBirth = date.ToUniversalTime();  }  } 
		}

 
        [FieldNameAttribute("NICNo",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String NicNo{ get; set; }

		[FieldNameAttribute("ReligionID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? ReligionId{ get; set; }

		[FieldNameAttribute("Address1",true,false,2000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Address1{ get; set; }

		[FieldNameAttribute("Address2",true,false,2000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Address2{ get; set; }

		[FieldNameAttribute("ZipCode",true,false,20)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String ZipCode{ get; set; }

		[FieldNameAttribute("CountryID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? CountryId{ get; set; }

		[FieldNameAttribute("CityID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? CityId{ get; set; }

		[FieldNameAttribute("StateID",true,true,4)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Int32? StateId{ get; set; }

		[FieldNameAttribute("AcadmicQualification",true,false,1000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String AcadmicQualification{ get; set; }

		[FieldNameAttribute("Designation",true,false,200)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Designation{ get; set; }

		[FieldNameAttribute("Salary",true,false,5)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Decimal? Salary{ get; set; }

		[FieldNameAttribute("LoginID",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String LoginId{ get; set; }

		[FieldNameAttribute("Password",true,false,50)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String Password{ get; set; }

		[FieldNameAttribute("ImagePath",true,false,1000)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.String ImagePath{ get; set; }

		[FieldNameAttribute("IsActive",true,false,1)]
		[DataMember (EmitDefaultValue=false)]
		public virtual System.Boolean? IsActive{ get; set; }

		[FieldNameAttribute("CreationDate",true,false,8)]
		[IgnoreDataMember]
		public virtual System.DateTime? CreationDate{ get; set;}
        [DataMember(EmitDefaultValue = false)]
        public virtual string CreationDateStr
        {
            get { if (CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty; }
            set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime(); } }
        }

        [FieldNameAttribute("JoiningDate", true, false, 8)]
        [IgnoreDataMember]
        public virtual System.DateTime? JoiningDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public virtual string JoiningDateStr
        {
            get { if (JoiningDate.HasValue) return JoiningDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty; }
            set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { JoiningDate = date.ToUniversalTime(); } }
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
        public virtual System.String UserIp { get; set; }

        [FieldNameAttribute("SalaryTypeID", true, true, 4)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.Int32? SalaryTypeId { get; set; }

        [FieldNameAttribute("FatherName", true, false, 200)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.String FatherName { get; set; }

        [FieldNameAttribute("AccountNumber", true, false, 50)]
        [DataMember(EmitDefaultValue = false)]
        public virtual System.String AccountNumber { get; set; }

		
		public virtual bool IsTransient()
        {

            return EntityHelper.IsTransient(this);
        }
		
		#region IEquatable<UserBase> Members

        public virtual bool Equals(UserBase other)
        {
			if(this.Id==other.Id  && this.FirstName==other.FirstName  && this.MiddleName==other.MiddleName  && this.LastName==other.LastName  && this.UserTypeId==other.UserTypeId  && this.GenderId==other.GenderId  && this.DateOfBirth==other.DateOfBirth  && this.NicNo==other.NicNo  && this.ReligionId==other.ReligionId  && this.Address1==other.Address1  && this.Address2==other.Address2  && this.ZipCode==other.ZipCode  && this.CountryId==other.CountryId  && this.CityId==other.CityId  && this.StateId==other.StateId  && this.AcadmicQualification==other.AcadmicQualification  && this.Designation==other.Designation  && this.Salary==other.Salary  && this.LoginId==other.LoginId  && this.Password==other.Password  && this.ImagePath==other.ImagePath  && this.IsActive==other.IsActive  && this.CreationDate==other.CreationDate  && this.UpdateDate==other.UpdateDate  && this.UpdateBy==other.UpdateBy  && this.UserIp==other.UserIp && this.JoiningDate == other.JoiningDate)
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}
		
		public virtual void CopyFrom(User other)
        {
			if(other!=null)
			{
				this.Id=other.Id;
				this.FirstName=other.FirstName;
				this.MiddleName=other.MiddleName;
				this.LastName=other.LastName;
				this.UserTypeId=other.UserTypeId;
				this.GenderId=other.GenderId;
				this.DateOfBirth=other.DateOfBirth;
				this.NicNo=other.NicNo;
				this.ReligionId=other.ReligionId;
				this.Address1=other.Address1;
				this.Address2=other.Address2;
				this.ZipCode=other.ZipCode;
				this.CountryId=other.CountryId;
				this.CityId=other.CityId;
				this.StateId=other.StateId;
				this.AcadmicQualification=other.AcadmicQualification;
				this.Designation=other.Designation;
				this.Salary=other.Salary;
				this.LoginId=other.LoginId;
				this.Password=other.Password;
				this.ImagePath=other.ImagePath;
				this.IsActive=other.IsActive;
				this.CreationDate=other.CreationDate;
				this.UpdateDate=other.UpdateDate;
				this.UpdateBy=other.UpdateBy;
				this.UserIp=other.UserIp;
                this.JoiningDate = other.JoiningDate;
            }
			
		
		}

        #endregion
		
		
		
	}
	
	
}
