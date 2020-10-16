using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.BranchDepartment
{
    [DataContract]
	public class PutOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? BranchId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? DepartmentId{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? CreatedDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string CreatedDateStr
		{
			 get {if(CreatedDate.HasValue) return CreatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreatedDate = date.ToUniversalTime();  }  } 
		}

		[IgnoreDataMember]
		public System.DateTime? UpdatedDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string UpdatedDateStr
		{
			 get {if(UpdatedDate.HasValue) return UpdatedDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdatedDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UpdatedBy{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String UserIp{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsActive{ get; set; }

	}	
}
