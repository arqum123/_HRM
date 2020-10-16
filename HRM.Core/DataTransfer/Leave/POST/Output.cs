using System;
using System.Runtime.Serialization;
using Validation;

namespace HRM.Core.DataTransfer.Leave
{
    [DataContract]
	public class PostOutput
	{
			
		[DataMember (EmitDefaultValue=false)]
		public System.Int32 Id{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UserId{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? Date{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string DateStr
		{
			 get {if(Date.HasValue) return Date.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { Date = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.String Reason{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? LeaveTypeId{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.Boolean? IsActive{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? UpdateDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string UpdateDateStr
		{
			 get {if(UpdateDate.HasValue) return UpdateDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { UpdateDate = date.ToUniversalTime();  }  } 
		}

		[DataMember (EmitDefaultValue=false)]
		public System.Int32? UpdatedBy{ get; set; }

		[DataMember (EmitDefaultValue=false)]
		public System.String UserIp{ get; set; }

		[IgnoreDataMember]
		public System.DateTime? CreationDate{ get; set;}


		[DataMember (EmitDefaultValue=false)]
		public string CreationDateStr
		{
			 get {if(CreationDate.HasValue) return CreationDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); else return string.Empty;}
			 set  {  DateTime date = new DateTime(); if (DateTime.TryParse(value, out date)) { CreationDate = date.ToUniversalTime();  }  } 
		}

	}	
}
