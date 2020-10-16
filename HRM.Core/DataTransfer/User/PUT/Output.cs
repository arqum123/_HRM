using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.User
{
    [DataContract]
	public class PutOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String FirstName{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String MiddleName{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String LastName{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UserTypeId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? GenderId{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? DateOfBirth{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string DateOfBirthStr
		{
			 get {if(DateOfBirth.HasValue) return DateOfBirth.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { DateOfBirth = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.String NicNo{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? ReligionId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Address1{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Address2{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String ZipCode{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? CountryId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? CityId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? StateId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String AcadmicQualification{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Designation{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Decimal? Salary{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String LoginId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Password{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String ImagePath{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsActive{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

		[IgnoreDataMember]
		public System.DateTime? UpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string UpdateDateStr
		{
			 get {if(UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UpdateBy{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String UserIp{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? SalaryTypeId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String FatherName{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String AccountNumber{ get; set; }
        [IgnoreDataMember]
        public System.DateTime? JoiningDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string JoiningDateeStr
        {
            get { if (JoiningDate.HasValue) return JoiningDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty; }
            set { DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { JoiningDate = date.ToUniversalTime(); } }
        }




    }	
}
