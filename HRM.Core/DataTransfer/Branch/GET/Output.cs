using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.Branch
{
    [DataContract]
	public class GetOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Name{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String AddressLine{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? CityId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? StateId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? CountryId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String ZipCode{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String PhoneNumber{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String Guid{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? CreatedDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string CreatedDateStr
		{
			 get {if(CreatedDate.HasValue) return CreatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreatedDate = date.ToUniversalTime();  }  } 
		}

		[IgnoreDataMember]
		public System.DateTime? UpdationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string UpdationDateStr
		{
			 get {if(UpdationDate.HasValue) return UpdationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdationDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UpdatedBy{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String UserIp{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsActive{ get; set; }

	}	
}
